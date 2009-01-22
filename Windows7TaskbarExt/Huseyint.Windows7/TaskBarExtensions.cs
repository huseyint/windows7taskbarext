namespace Huseyint.Windows7.WindowsForms
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using Huseyint.Windows7.Native;

    public class TaskBarExtensions : IMessageFilter, IDisposable
    {
        private static TaskBarExtensions instance;

        private uint messageIdentifier;

        private TaskBarExtensions()
        {
            messageIdentifier = Win32.WM_NULL;

            currentOverlayIcon = new IconHandle();
            currentOverlayIconAccessibilityText = string.Empty;
            currentProgressState = ProgressState.NoProgress;
        }

        ~TaskBarExtensions()
        {
            Dispose(false);
        }

        internal IntPtr MainWindowHandle { get; private set; }

        internal ITaskbarList3 TaskbarList { get; private set; }

        private IconHandle currentOverlayIcon;

        private string currentOverlayIconAccessibilityText;

        private ProgressState currentProgressState;

        public bool PreFilterMessage(ref Message m)
        {
            if (messageIdentifier == Win32.WM_NULL)
            {
                messageIdentifier = Win32.RegisterWindowMessage(Win32.TaskbarButtonCreatedMessage);

                Win32.ChangeWindowMessageFilter(messageIdentifier, Win32.MSGFLT_ADD);
            }

            if (m.Msg == messageIdentifier)
            {
                if (TaskbarList == null)
                {
                    MainWindowHandle = m.HWnd;

                    TaskbarList = (ITaskbarList3)Activator.CreateInstance(Type.GetTypeFromCLSID(Win32.CLSID_TaskbarList));
                }

                if (!currentOverlayIcon.IsInvalid || currentProgressState != ProgressState.NoProgress)
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        // HACK!
                        Thread.Sleep(2000);

                        SetOverlayIconCore(currentOverlayIcon, currentOverlayIconAccessibilityText);
                        SetProgressStateCore(currentProgressState);
                    });
                }
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (currentOverlayIcon != null)
                {
                    currentOverlayIcon.Dispose();
                }
            }
        }

        private void SetOverlayIconCore(IconHandle hIcon, string accessibilityText)
        {
            if (!currentOverlayIcon.IsInvalid && currentOverlayIcon != hIcon)
            {
                currentOverlayIcon.Dispose();
            }

            currentOverlayIcon = hIcon;
            currentOverlayIconAccessibilityText = accessibilityText;

            this.TaskbarList.SetOverlayIcon(
                MainWindowHandle, 
                currentOverlayIcon,
                currentOverlayIconAccessibilityText);
        }

        private bool SetProgressStateCore(ProgressState state)
        {
            if (TaskbarList == null)
            {
                return false;
            }

            currentProgressState = state;

            try
            {
                this.TaskbarList.SetProgressState(this.MainWindowHandle, (TBPFLAG)state);
            }
            catch (InvalidComObjectException)
            {
                return false;
            }
            catch (COMException ex)
            {
                // HRESULT = E_FAIL
                if (ex.ErrorCode == -2147467259)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        private bool SetProgressValueCore(ulong completed, ulong total)
        {
            if (this.TaskbarList == null)
            {
                return false;
            }

            currentProgressState = ProgressState.Normal;

            try
            {
                this.TaskbarList.SetProgressValue(this.MainWindowHandle, completed, total);
            }
            catch (InvalidComObjectException)
            {
                return false;
            }
            catch (COMException ex)
            {
                // HRESULT = E_FAIL
                if (ex.ErrorCode == -2147467259)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// Attaches a <see cref="IMessageFilter"/> to current <see cref="Application"/>
        /// to intercept window messages.
        /// </summary>
        /// <remarks>
        /// This method must be called before calling <see cref="Application.Run"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">If an instance is attached before.</exception>
        public static void Attach()
        {
            if (instance == null)
            {
                instance = new TaskBarExtensions();

                Application.AddMessageFilter(instance);
            }
            else
            {
                throw new InvalidOperationException("TaskBarExtensions already attached to current Application.");
            }
        }

        #region Overlay Icon Methods
        /// <summary>
        /// Applies an overlay to application's taskbar button to indicate application 
        /// status or a notification to the user.
        /// </summary>
        /// <param name="icon">A <see cref="Bitmap"/> to use as the overlay. This 
        /// should be a small icon, measuring 16x16 pixels at 96 dots per inch (dpi). 
        /// If an overlay icon is already applied to the taskbar button, that 
        /// existing overlay is replaced. It is the responsibility of the caller 
        /// to dispose <paramref name="icon"/> when it is no longer needed.</param>
        /// <returns>true if overlay set successfully.</returns>
        public static bool SetOverlayIcon(Bitmap icon)
        {
            return SetOverlayIcon(icon, string.Empty);
        }

        /// <summary>
        /// Applies an overlay to application's taskbar button to indicate application 
        /// status or a notification to the user.
        /// </summary>
        /// <param name="icon">A <see cref="Bitmap"/> to use as the overlay. This 
        /// should be a small icon, measuring 16x16 pixels at 96 dots per inch (dpi). 
        /// If an overlay icon is already applied to the taskbar button, that 
        /// existing overlay is replaced. It is the responsibility of the caller 
        /// to dispose <paramref name="icon"/> when it is no longer needed.</param>
        /// <param name="accessibilityText">A string that provides an alt text 
        /// version of the information conveyed by the overlay, for accessibility 
        /// purposes.</param>
        /// <returns>true if overlay is set successfully.</returns>
        public static bool SetOverlayIcon(Bitmap icon, string accessibilityText)
        {
            CheckOperation();

            var hIcon = icon == null ? new IconHandle() : new IconHandle(icon.GetHicon());

            try
            {
                instance.SetOverlayIconCore(hIcon, accessibilityText ?? string.Empty);
            }
            catch (COMException ex)
            {
                // HRESULT = E_FAIL
                if (ex.ErrorCode == -2147467259)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// Clears the overlay on application's taskbar button.
        /// </summary>
        /// <returns>true if overlay is cleared successfully.</returns>
        public static bool ClearOverlayIcon()
        {
            CheckOperation();

            try
            {
                instance.SetOverlayIconCore(new IconHandle(IntPtr.Zero), string.Empty);
            }
            catch (COMException ex)
            {
                // HRESULT = E_FAIL
                if (ex.ErrorCode == -2147467259)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        #endregion

        #region Progress Methods
        /// <summary>
        /// Sets the type and state of the progress indicator displayed on 
        /// application's taskbar button. 
        /// </summary>
        /// <param name="state"><see cref="ProgressState"/> to be set.</param>
        /// <returns>true if progress state is set successfully.</returns>
        public static bool SetProgressState(ProgressState state)
        {
            CheckOperation();

            return instance.SetProgressStateCore(state);
        }

        /// <summary>
        /// Displays or updates a progress bar hosted in a taskbar button 
        /// to show the specific percentage completed of the full operation.
        /// </summary>
        /// <param name="completed">An application-defined value that indicates 
        /// the proportion of the operation that has been completed at the time 
        /// the method is called.</param>
        /// <param name="total">An application-defined value that specifies the 
        /// value ullCompleted will have when the operation is complete.</param>
        /// <returns>true if progress value is set successfully.</returns>
        public static bool SetProgressValue(ulong completed, ulong total)
        {
            CheckOperation();

            return instance.SetProgressValueCore(completed, total);
        }
        #endregion

        private static void CheckOperation()
        {
            if (instance == null || instance.TaskbarList == null)
            {
                throw new InvalidOperationException("TaskBarExtensions is not attached to current Application.");
            }
        }
    }
}
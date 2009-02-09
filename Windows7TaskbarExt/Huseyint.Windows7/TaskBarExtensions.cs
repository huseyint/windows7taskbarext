namespace Huseyint.Windows7.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using Huseyint.Windows7.Native;

    /// <summary>
    /// Manages application's integration to Windows 7 Taskbar.
    /// </summary>
    public class TaskBarExtensions : IMessageFilter, IDisposable
    {
        private static TaskBarExtensions instance;

        private static Dictionary<IntPtr, IList<ThumbnailBarButton>> thumbnailBarButtons;

        private static Dictionary<IntPtr, IntPtr> thumbnailBarImageListHandles;

        private uint messageIdentifier;

        private IconHandle currentOverlayIcon;

        private string currentOverlayIconAccessibilityText;

        private ProgressState currentProgressState;

        static TaskBarExtensions()
        {
            thumbnailBarButtons = new Dictionary<IntPtr, IList<ThumbnailBarButton>>();
            thumbnailBarImageListHandles = new Dictionary<IntPtr, IntPtr>();
        }

        private TaskBarExtensions()
        {
            this.messageIdentifier = Win32.WindowMessageNull;

            this.currentOverlayIcon = new IconHandle();
            this.currentOverlayIconAccessibilityText = string.Empty;
            this.currentProgressState = ProgressState.NoProgress;
        }

        /// <summary>
        /// Finalizer for <see cref="TaskBarExtensions"/> type.
        /// </summary>
        ~TaskBarExtensions()
        {
            this.Dispose(false);
        }

        internal IntPtr MainWindowHandle { get; private set; }

        internal ITaskbarList3 TaskbarList { get; private set; }

        /// <summary>
        /// Attaches a <see cref="IMessageFilter"/> to current <see cref="Application"/>
        /// to intercept window messages.
        /// </summary>
        /// <remarks>
        /// This method must be called before calling <see cref="Application.Run(Form)"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">If an instance is attached before.</exception>
        public static void Attach()
        {
            if (!Win32.IsWindows7)
            {
                return;
            }

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
            if (!Win32.IsWindows7)
            {
                return false;
            }

            CheckOperation();

            var iconHandle = icon == null ? new IconHandle() : new IconHandle(icon.GetHicon());

            try
            {
                instance.SetOverlayIconCore(iconHandle, accessibilityText ?? string.Empty);
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
            if (!Win32.IsWindows7)
            {
                return false;
            }

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
            if (!Win32.IsWindows7)
            {
                return false;
            }

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
            if (!Win32.IsWindows7)
            {
                return false;
            }

            CheckOperation();

            return instance.SetProgressValueCore(completed, total);
        }
        #endregion

        /// <summary>
        /// Adds buttons to thumbnail toolbar with <see cref="Bitmap"/> icons.
        /// </summary>
        /// <remarks>
        /// By design of the taskbar, you can add buttons only one time. When necessary, you may add
        /// hidden buttons and make them visible when appropriate. Maximum 7 buttons is allowed.
        /// </remarks>
        /// <param name="form">The window which the buttons will be added to its taskbar thumbnail.</param>
        /// <param name="buttons">The buttons to be added.</param>
        public static void AddThumbnailBarButtons(Form form, IList<ThumbnailBarButton> buttons)
        {
            AddThumbnailBarButtons(form.Handle, buttons, IntPtr.Zero);
        }

        /// <summary>
        /// Adds buttons to thumbnail toolbar with icons reside in an <see cref="ImageList"/>.
        /// </summary>
        /// <remarks>
        /// By design of the taskbar, you can add buttons only one time. When necessary, you may add
        /// hidden buttons and make them visible when appropriate. Maximum 7 buttons is allowed.
        /// </remarks>
        /// <param name="form">The window which the buttons will be added to its taskbar thumbnail.</param>
        /// <param name="buttons">The buttons to be added.</param>
        /// <param name="imagelist">The imagelist which button icons will be used from.</param>
        public static void AddThumbnailBarButtons(Form form, IList<ThumbnailBarButton> buttons, ImageList imagelist)
        {
            AddThumbnailBarButtons(form.Handle, buttons, imagelist.Handle);
        }

        /// <summary>
        /// Filters application's window message loop.
        /// </summary>
        /// <param name="m">Current message.</param>
        /// <returns>This implementation always returns false which allows the message pass.</returns>
        public bool PreFilterMessage(ref Message m)
        {
            // Register for the message here...
            if (this.messageIdentifier == Win32.WindowMessageNull)
            {
                this.messageIdentifier = Win32.RegisterWindowMessage(Win32.TaskbarButtonCreatedMessage);

                Win32.ChangeWindowMessageFilter(this.messageIdentifier, Win32.MessageFilterAdd);
            }

            // ...and receive the message here.
            if (m.Msg == this.messageIdentifier)
            {
                if (this.TaskbarList == null)
                {
                    this.MainWindowHandle = m.HWnd;

                    this.TaskbarList = (ITaskbarList3)Activator.CreateInstance(Type.GetTypeFromCLSID(Win32.ClassIdTaskbarList));
                }

                // When explorer.exe gets closed and re-opened all Taskbar Extensions are lost,
                // so we should restore them.
                if (!this.currentOverlayIcon.IsInvalid ||
                    !string.IsNullOrEmpty(this.currentOverlayIconAccessibilityText) ||
                    this.currentProgressState != ProgressState.NoProgress ||
                    thumbnailBarButtons.Count > 0)
                {
                    // HACK: Immediately setting Taskbar Extensions won't work. The reason is that
                    // we get the "TaskbarButtonCreatedMessage" a little earlier and the Taskbar
                    // is not ready yet.
                    // 2000 milliseconds is just an arbitrarily selected number which *worked on my machine*.
                    // Use BackgroundWorker to wait 2 seconds and tunnel the real work to UI thread.
                    var worker = new BackgroundWorker();

                    worker.WorkerReportsProgress = true;

                    worker.DoWork += delegate(object sender, DoWorkEventArgs e)
                    {
                        Thread.Sleep(2000);
                        ((BackgroundWorker)sender).ReportProgress(0);
                    };

                    worker.ProgressChanged += this.ExplorerRestarted;

                    worker.RunWorkerAsync();
                }
            }
            else if (m.Msg == Win32.WindowMessageCommand && Win32.HiWord(m.WParam) == Win32.ThumbnailBarButtonClicked)
            {
                var buttonId = Win32.LoWord(m.WParam);
                
                IList<ThumbnailBarButton> buttons;
                if (thumbnailBarButtons.TryGetValue(m.HWnd, out buttons))
                {
                    foreach (var b in buttons)
                    {
                        if (b.Id == buttonId)
                        {
                            b.FireClickEvent();
                            break;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static void UpdateThumbnailBarButton(ThumbnailBarButton button)
        {
            if (!Win32.IsWindows7)
            {
                return;
            }

            instance.TaskbarList.ThumbBarUpdateButtons(
                button.WindowHandle,
                (uint)1,
                new THUMBBUTTON[1] { button.GetUnmanagedButton() });
        }

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        /// <param name="disposing">True if being disposed in a deterministic way.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.currentOverlayIcon != null)
                {
                    this.currentOverlayIcon.Dispose();
                }
            }
        }

        private static void AddThumbnailBarButtons(IntPtr windowHandle, IList<ThumbnailBarButton> buttons, IntPtr imagelistHandle)
        {
            if (!Win32.IsWindows7)
            {
                return;
            }

            CheckOperation();

            var buttonCount = buttons.Count;

            if (buttonCount < 1 || buttonCount > 7)
            {
                throw new ArgumentOutOfRangeException("buttons", "At least 1 and at most 7 buttons should be provided.");
            }

            if (imagelistHandle != IntPtr.Zero)
            {
                instance.TaskbarList.ThumbBarSetImageList(windowHandle, imagelistHandle);

                if (!thumbnailBarImageListHandles.ContainsKey(windowHandle))
                {
                    thumbnailBarImageListHandles.Add(windowHandle, imagelistHandle);
                }
            }

            var unmanagedButtons = new THUMBBUTTON[buttonCount];

            for (int i = 0; i < buttonCount; i++)
            {
                buttons[i].Initialize(windowHandle);
                unmanagedButtons[i] = buttons[i].GetUnmanagedButton();
            }

            instance.TaskbarList.ThumbBarAddButtons(windowHandle, (uint)buttonCount, unmanagedButtons);

            if (!thumbnailBarButtons.ContainsKey(windowHandle))
            {
                thumbnailBarButtons.Add(windowHandle, buttons);
            }
        }

        private static void CheckOperation()
        {
            if (instance == null || instance.TaskbarList == null)
            {
                throw new InvalidOperationException("TaskBarExtensions is not attached to current Application.");
            }
        }

        private void ExplorerRestarted(object sender, ProgressChangedEventArgs e)
        {
            this.SetOverlayIconCore(this.currentOverlayIcon, this.currentOverlayIconAccessibilityText);
            this.SetProgressStateCore(this.currentProgressState);
            foreach (var item in thumbnailBarButtons)
            {
                var windowHandle = item.Key;

                IntPtr imageListHandle;
                if (thumbnailBarImageListHandles.TryGetValue(windowHandle, out imageListHandle))
                {
                    AddThumbnailBarButtons(windowHandle, item.Value, imageListHandle);
                }
                else
                {
                    AddThumbnailBarButtons(windowHandle, item.Value, IntPtr.Zero);
                }
            }
        }

        private void SetOverlayIconCore(IconHandle iconHandle, string accessibilityText)
        {
            if (!this.currentOverlayIcon.IsInvalid && this.currentOverlayIcon != iconHandle)
            {
                this.currentOverlayIcon.Dispose();
            }

            this.currentOverlayIcon = iconHandle;
            this.currentOverlayIconAccessibilityText = accessibilityText;

            this.TaskbarList.SetOverlayIcon(
                this.MainWindowHandle,
                this.currentOverlayIcon,
                this.currentOverlayIconAccessibilityText);
        }

        private bool SetProgressStateCore(ProgressState state)
        {
            if (this.TaskbarList == null)
            {
                return false;
            }

            this.currentProgressState = state;

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

            this.currentProgressState = ProgressState.Normal;

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
    }
}
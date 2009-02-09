namespace Huseyint.Windows7.WPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Interop;
    using Huseyint.Windows7.Native;

    public static class TaskbarExtensions
    {
        public static readonly DependencyProperty TaskbarButtonProperty = DependencyProperty.RegisterAttached(
            "TaskbarButton",
            typeof(TaskbarButton),
            typeof(TaskbarExtensions),
            new UIPropertyMetadata(null, OnTaskbarButtonChanged));

        private static uint messageIdentifier;

        private static ITaskbarList3 taskbarList;

        private static Dictionary<IntPtr, Window> windows;

        static TaskbarExtensions()
        {
            if (!Win32.IsWindows7)
            {
                return;
            }

            // Since "TaskbarButton" attached property doesn't belong to the WPF's so called 
            // "Logical Tree", the DataContext can not be inherited from parent Window
            // and this breaks Data Binding. To solve the issue, the following technique is
            // applied. See http://blogs.msdn.com/jaimer/archive/2008/11/22/forwarding-the-datagrid-s-datacontext-to-its-columns.aspx
            // for more info.
            FrameworkElement.DataContextProperty.AddOwner(typeof(TaskbarButton));

            var metadata = new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnDataContextChanged);

            FrameworkElement.DataContextProperty.OverrideMetadata(typeof(Window), metadata);

            windows = new Dictionary<IntPtr, Window>();
        }

        public static TaskbarButton GetTaskbarButton(Window window)
        {
            return (TaskbarButton)window.GetValue(TaskbarButtonProperty);
        }

        public static void SetTaskbarButton(Window window, TaskbarButton value)
        {
            window.SetValue(TaskbarButtonProperty, value);
        }

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;

            if (window != null)
            {
                var taskbarButton = window.GetValue(TaskbarExtensions.TaskbarButtonProperty) as TaskbarButton;

                if (taskbarButton != null)
                {
                    taskbarButton.DataContext = e.NewValue;
                }
            }
        }

        private static void OnTaskbarButtonChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (!Win32.IsWindows7)
            {
                return;
            }

            var window = depObj as Window;

            if (window != null)
            {
                window.Loaded += WindowLoaded;
            }
        }

        private static void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var window = sender as Window;

            if (window != null)
            {
                var windowHandle = new WindowInteropHelper(window).Handle;

                var source = HwndSource.FromHwnd(windowHandle);
                source.AddHook(new HwndSourceHook(WndProc));

                windows.Add(windowHandle, window);
            }
        }

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (messageIdentifier == Win32.WindowMessageNull)
            {
                messageIdentifier = Win32.RegisterWindowMessage(Win32.TaskbarButtonCreatedMessage);

                Win32.ChangeWindowMessageFilter(messageIdentifier, Win32.MessageFilterAdd);
            }

            if (msg == messageIdentifier)
            {
                if (taskbarList == null)
                {
                    taskbarList = (ITaskbarList3)Activator.CreateInstance(Type.GetTypeFromCLSID(Win32.ClassIdTaskbarList));
                }

                Window window;

                if (windows.TryGetValue(hwnd, out window))
                {
                    var taskbarButton = window.GetValue(TaskbarExtensions.TaskbarButtonProperty) as TaskbarButton;

                    if (taskbarButton != null)
                    {
                        taskbarButton.Initialize(hwnd, taskbarList);

                        taskbarButton.AddThumbnailBarButtons();

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
                            ((BackgroundWorker)sender).ReportProgress(0, taskbarButton);
                        };

                        worker.ProgressChanged += ExplorerRestarted;

                        worker.RunWorkerAsync();
                    }
                }
            }
            else if (msg == Win32.WindowMessageCommand && Win32.HiWord(wParam) == Win32.ThumbnailBarButtonClicked)
            {
                var buttonId = Win32.LoWord(wParam);

                IList<ThumbnailBarButton> buttons;
                if (TaskbarButton.ThumbnailBarButtonsCache.TryGetValue(hwnd, out buttons))
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

            return IntPtr.Zero;
        }

        private static void ExplorerRestarted(object sender, ProgressChangedEventArgs e)
        {
            var taskbarButton = (TaskbarButton)e.UserState;

            taskbarButton.UpdateIcon();
            taskbarButton.UpdateProgressValue();
            taskbarButton.UpdateProgressState();
        }
    }
}
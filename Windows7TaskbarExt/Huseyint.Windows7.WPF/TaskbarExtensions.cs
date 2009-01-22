namespace Huseyint.Windows7.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using Huseyint.Windows7.Native;

    public static class TaskbarExtensions
    {
        private static uint messageIdentifier;

        private static Window window;

        private static IntPtr windowHandle;

        private static ITaskbarList3 TaskbarList;

        public static TaskbarButton GetTaskbarButton(Window window)
        {
            return (TaskbarButton)window.GetValue(TaskbarButtonProperty);
        }

        public static void SetTaskbarButton(Window window, TaskbarButton value)
        {
            window.SetValue(TaskbarButtonProperty, value);
        }

        public static readonly DependencyProperty TaskbarButtonProperty = DependencyProperty.RegisterAttached(
            "TaskbarButton", 
            typeof(TaskbarButton), 
            typeof(TaskbarExtensions),
            new UIPropertyMetadata(null, OnTaskbarButtonChanged));

        private static void OnTaskbarButtonChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            window = depObj as Window;

            if (window != null)
            {
                window.Loaded += window_Loaded;
            }
        }

        private static void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (window != null)
            {
                windowHandle = new WindowInteropHelper(window).Handle;

                var source = HwndSource.FromHwnd(windowHandle);
                source.AddHook(new HwndSourceHook(WndProc));
            }
        }

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (messageIdentifier == Win32.WM_NULL)
            {
                messageIdentifier = Win32.RegisterWindowMessage(Win32.TaskbarButtonCreatedMessage);

                Win32.ChangeWindowMessageFilter(messageIdentifier, Win32.MSGFLT_ADD);
            }

            if (msg == 49345)
            {
                TaskbarList = (ITaskbarList3)Activator.CreateInstance(Type.GetTypeFromCLSID(Win32.CLSID_TaskbarList));

                var taskbarButton = window.GetValue(TaskbarExtensions.TaskbarButtonProperty) as TaskbarButton;

                if (taskbarButton != null)
                {
                    taskbarButton.Initialize(windowHandle, TaskbarList);

                    taskbarButton.UpdateIcon();
                }
            }

            return IntPtr.Zero;
        }
    }
}
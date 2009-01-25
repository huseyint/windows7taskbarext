namespace Huseyint.Windows7.WPF
{
    using System;
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

        private static Window window;

        private static IntPtr windowHandle;

        private static ITaskbarList3 taskbarList;

        static TaskbarExtensions()
        {
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
            window = depObj as Window;

            if (window != null)
            {
                window.Loaded += WindowLoaded;
            }
        }

        private static void WindowLoaded(object sender, RoutedEventArgs e)
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
            if (messageIdentifier == Win32.WindowMessageNull)
            {
                messageIdentifier = Win32.RegisterWindowMessage(Win32.TaskbarButtonCreatedMessage);

                Win32.ChangeWindowMessageFilter(messageIdentifier, Win32.MessageFilterAdd);
            }

            if (msg == messageIdentifier)
            {
                taskbarList = (ITaskbarList3)Activator.CreateInstance(Type.GetTypeFromCLSID(Win32.ClassIdTaskbarList));

                var taskbarButton = window.GetValue(TaskbarExtensions.TaskbarButtonProperty) as TaskbarButton;

                if (taskbarButton != null)
                {
                    taskbarButton.Initialize(windowHandle, taskbarList);

                    taskbarButton.UpdateIcon();
                }
            }

            return IntPtr.Zero;
        }
    }
}
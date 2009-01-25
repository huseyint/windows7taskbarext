namespace Huseyint.Windows7.WPF
{
    using System;
    using System.Windows;
    using Huseyint.Windows7.Native;

    public class TaskbarButton : FrameworkElement
    {
        public static readonly DependencyProperty OverlayIconProperty = DependencyProperty.Register(
            "OverlayIcon",
            typeof(OverlayIcon),
            typeof(TaskbarButton),
            new UIPropertyMetadata(null, OnOverlayIconPropertyChanged));

        public static readonly DependencyProperty ProgressStateProperty = DependencyProperty.Register(
            "ProgressState",
            typeof(ProgressState),
            typeof(TaskbarButton),
            new UIPropertyMetadata(ProgressState.NoProgress, OnProgressStatePropertyChanged));

        public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(
            "ProgressValue",
            typeof(ulong),
            typeof(TaskbarButton),
            new UIPropertyMetadata(ulong.MinValue, OnProgressChanged));

        public static readonly DependencyProperty ProgressValueTotalProperty = DependencyProperty.Register(
            "ProgressValueTotal",
            typeof(ulong),
            typeof(TaskbarButton),
            new UIPropertyMetadata(ulong.MaxValue, OnProgressChanged));

        private IntPtr windowHandle;

        private ITaskbarList3 taskbarList;

        public OverlayIcon OverlayIcon
        {
            get { return (OverlayIcon)GetValue(OverlayIconProperty); }
            set { SetValue(OverlayIconProperty, value); }
        }

        public ProgressState ProgressState
        {
            get { return (ProgressState)GetValue(ProgressStateProperty); }
            set { SetValue(ProgressStateProperty, value); }
        }

        public ulong ProgressValue
        {
            get { return (ulong)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        public ulong ProgressValueTotal
        {
            get { return (ulong)GetValue(ProgressValueTotalProperty); }
            set { SetValue(ProgressValueTotalProperty, value); }
        }

        internal void UpdateIcon()
        {
            if (this.taskbarList == null || this.windowHandle == IntPtr.Zero || this.OverlayIcon == null)
            {
                return;
            }

            this.taskbarList.SetOverlayIcon(
                this.windowHandle,
                this.OverlayIcon.GetIconHandle(),
                this.OverlayIcon.AccessibilityText);
        }

        internal void UpdateProgressState()
        {
            if (this.taskbarList == null || this.windowHandle == IntPtr.Zero)
            {
                return;
            }

            this.taskbarList.SetProgressState(
                this.windowHandle,
                (TBPFLAG)this.ProgressState);
        }

        internal void UpdateProgressValue()
        {
            if (this.taskbarList == null || this.windowHandle == IntPtr.Zero)
            {
                return;
            }

            this.taskbarList.SetProgressValue(
                this.windowHandle,
                this.ProgressValue,
                this.ProgressValueTotal);
        }

        internal void Initialize(IntPtr windowHandle, ITaskbarList3 taskbarList)
        {
            this.windowHandle = windowHandle;
            this.taskbarList = taskbarList;
        }

        private static void OnOverlayIconPropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var tb = depObj as TaskbarButton;

            if (tb != null)
            {
                tb.UpdateIcon();
            }
        }

        private static void OnProgressStatePropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var tb = depObj as TaskbarButton;

            if (tb != null)
            {
                tb.UpdateProgressState();
            }
        }

        private static void OnProgressChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var tb = depObj as TaskbarButton;

            if (tb != null)
            {
                tb.UpdateProgressValue();
            }
        }
    }
}
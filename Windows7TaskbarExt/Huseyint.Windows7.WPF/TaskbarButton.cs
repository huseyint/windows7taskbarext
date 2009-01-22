namespace Huseyint.Windows7.WPF
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Media;
    using Huseyint.Windows7.Native;

    public class TaskbarButton : FrameworkElement
    {
        public ImageSource OverlayIcon
        {
            get { return (ImageSource)GetValue(OverlayIconProperty); }
            set { SetValue(OverlayIconProperty, value); }
        }
        public static readonly DependencyProperty OverlayIconProperty =
            DependencyProperty.Register("OverlayIcon", typeof(ImageSource), typeof(TaskbarButton),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnOverlayIconChanged));


        private IntPtr windowHandle;

        private ITaskbarList3 taskbarList;

        public TaskbarButton()
        {
        }

        static void OnOverlayIconChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var tb = depObj as TaskbarButton;

            if (tb != null)
            {
                tb.UpdateIcon();
            }
        }

        public void UpdateIcon()
        {
            if (this.taskbarList == null || this.windowHandle == IntPtr.Zero || this.OverlayIcon == null)
            {
                return;
            }

            taskbarList.SetOverlayIcon(this.windowHandle, this.GetOverlayIconHandle(), string.Empty);
        }

        internal SafeHandle GetOverlayIconHandle()
        {
            var type = Type.GetType("MS.Internal.AppModel.IconHelper, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            var handle = (SafeHandle)type.InvokeMember(
                "CreateIconHandleFromBitmapFrame",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic,
                null,
                null,
                new object[] { this.OverlayIcon });

            return handle;
        }

        internal void Initialize(IntPtr windowHandle, ITaskbarList3 taskbarList)
        {
            this.windowHandle = windowHandle;
            this.taskbarList = taskbarList;
        }
    }
}

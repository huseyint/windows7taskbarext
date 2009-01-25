namespace Huseyint.Windows7.WPF
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class OverlayIcon
    {
        public OverlayIcon(ImageSource icon)
            : this(icon, string.Empty)
        {
        }

        public OverlayIcon(ImageSource icon, string accessibilityText)
        {
            this.Icon = icon;
            this.AccessibilityText = accessibilityText ?? string.Empty;
        }

        public ImageSource Icon { get; private set; }

        public string AccessibilityText { get; private set; }

        internal SafeHandle GetIconHandle()
        {
            if (this.Icon == null)
            {
                return new IconHandle();
            }

            var type = Type.GetType("MS.Internal.AppModel.IconHelper, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            var handle = (SafeHandle)type.InvokeMember(
                "CreateIconHandleFromBitmapFrame",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic,
                null,
                null,
                new object[] { BitmapFrame.Create((BitmapSource)this.Icon) });

            return handle;
        }
    }
}
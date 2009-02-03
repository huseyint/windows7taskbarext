namespace Huseyint.Windows7.WPF
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class ThumbnailBarButton : ThumbnailBarButtonBase
    {
        private ImageSource icon;

        public ThumbnailBarButton()
            : base(string.Empty, false, false, false, true)
        {
        }

        internal event Action<ThumbnailBarButton> OnUpdate;

        public ImageSource Icon
        {
            get
            {
                return this.icon;
            }

            set
            {
                this.icon = value;

                var type = Type.GetType("MS.Internal.AppModel.IconHelper, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                var handle = (SafeHandle)type.InvokeMember(
                    "CreateIconHandleFromBitmapFrame",
                    BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic,
                    null,
                    null,
                    new object[] { BitmapFrame.Create((BitmapSource)this.icon) });

                this.IconHandle = handle;

                this.Update();
            }
        }

        protected override void Update()
        {
            var handler = this.OnUpdate;

            if (handler != null)
            {
                handler(this);
            }
        }
    }
}

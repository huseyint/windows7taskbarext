namespace Huseyint.Windows7.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using Huseyint.Windows7.Native;

    public class ThumbnailBarButton : IDisposable
    {
        private static Dictionary<IntPtr, uint> idCounters;

        private uint id;

        private bool isDisabled;

        private bool isDismissedOnClick;

        private bool hasBackground;

        private bool isHidden;

        private string tooltip;

        private Bitmap icon;

        private uint imageIndex;

        private IntPtr iconHandle;

        private IntPtr windowHandle;

        static ThumbnailBarButton()
        {
            idCounters = new Dictionary<IntPtr, uint>();
        }

        public ThumbnailBarButton(Bitmap icon, string tooltip, bool isHidden, bool isDisabled, bool isDismissedOnClick, bool hasBackground)
            : this(tooltip, isHidden, isDisabled, isDismissedOnClick, hasBackground)
        {
            this.icon = icon;
            if (icon != null)
            {
                this.iconHandle = icon.GetHicon();
            }
        }

        public ThumbnailBarButton(uint imageIndex, string tooltip, bool isHidden, bool isDisabled, bool isDismissedOnClick, bool hasBackground)
            : this(tooltip, isHidden, isDisabled, isDismissedOnClick, hasBackground)
        {
            this.imageIndex = imageIndex;
        }

        private ThumbnailBarButton(string tooltip, bool isHidden, bool isDisabled, bool isDismissedOnClick, bool hasBackground)
        {
            this.isHidden = isHidden;
            this.isDisabled = isDisabled;
            this.isDismissedOnClick = isDismissedOnClick;
            this.hasBackground = hasBackground;
            this.tooltip = tooltip;
        }

        ~ThumbnailBarButton()
        {
            this.Dispose(false);
        }

        public event EventHandler Click;

        public bool IsDisabled
        {
            get
            {
                return this.isDisabled;
            }

            set
            {
                this.isDisabled = value;

                this.Update();
            }
        }

        public bool IsDismissedOnClick
        {
            get
            {
                return this.isDismissedOnClick;
            }

            set
            {
                this.isDismissedOnClick = value;

                this.Update();
            }
        }

        public bool HasBackground
        {
            get
            {
                return this.hasBackground;
            }

            set
            {
                this.hasBackground = value;

                this.Update();
            }
        }

        public bool IsHidden
        {
            get
            {
                return this.isHidden;
            }

            set
            {
                this.isHidden = value;

                this.Update();
            }
        }

        public string Tooltip
        {
            get
            {
                return this.tooltip;
            }

            set
            {
                this.tooltip = value;

                this.Update();
            }
        }

        public Bitmap Icon
        {
            get
            {
                return this.icon;
            }

            set
            {
                this.icon = value;

                if (value != null)
                {
                    this.IconHandle = value.GetHicon();
                }

                this.Update();
            }
        }

        public uint ImageIndex
        {
            get
            {
                return this.imageIndex;
            }

            set
            {
                this.imageIndex = value;

                this.Update();
            }
        }

        internal uint Id
        {
            get
            {
                return this.id;
            }
        }

        internal IntPtr WindowHandle
        {
            get
            {
                return this.windowHandle;
            }
        }

        private IntPtr IconHandle
        {
            get
            {
                return this.iconHandle;
            }

            set
            {
                if (this.iconHandle != IntPtr.Zero)
                {
                    Win32.DestroyIcon(this.iconHandle);
                }

                this.iconHandle = value;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        internal void Initialize(IntPtr windowHandle)
        {
            Debug.Assert(this.id == 0, "ThumbnailBarButton has already been initialized.");

            this.windowHandle = windowHandle;

            uint id;
            if (idCounters.TryGetValue(windowHandle, out id))
            {
                idCounters[windowHandle]++;
            }
            else
            {
                id = 0;
                idCounters.Add(windowHandle, 1);
            }

            this.id = id;
        }

        internal THUMBBUTTON GetUnmanagedButton()
        {
            THUMBBUTTON button = new THUMBBUTTON();
            button.Id = this.id;
            button.Mask = THBMASK.THB_FLAGS;
            button.Flags = THBFLAGS.THBF_ENABLED;

            if (this.icon == null)
            {
                button.Bitmap = this.imageIndex;

                button.Mask |= THBMASK.THB_BITMAP;
            }
            else
            {
                if (this.IconHandle != IntPtr.Zero)
                {
                    button.IconHandle = this.IconHandle;

                    button.Mask |= THBMASK.THB_ICON;
                }
            }

            if (!string.IsNullOrEmpty(this.tooltip))
            {
                button.ToolTip = this.tooltip;

                button.Mask |= THBMASK.THB_TOOLTIP;
            }

            if (this.isDisabled)
            {
                button.Flags |= THBFLAGS.THBF_DISABLED;
            }

            if (this.isDismissedOnClick)
            {
                button.Flags |= THBFLAGS.THBF_DISMISSONCLICK;
            }

            if (!this.hasBackground)
            {
                button.Flags |= THBFLAGS.THBF_NOBACKGROUND;
            }

            if (this.isHidden)
            {
                button.Flags |= THBFLAGS.THBF_HIDDEN;
            }

            return button;
        }

        internal void FireClickEvent()
        {
            var handler = this.Click;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            if (this.iconHandle != IntPtr.Zero)
            {
                Win32.DestroyIcon(this.iconHandle);
            }
        }

        private void Update()
        {
            TaskBarExtensions.UpdateThumbnailBarButton(this);
        }
    }
}
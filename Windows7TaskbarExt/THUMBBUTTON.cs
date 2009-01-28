namespace Huseyint.Windows7.Native
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Used by methods of the <see cref="ITaskbarList3"/> interface to define buttons used in a toolbar embedded 
    /// in a window's thumbnail representation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct THUMBBUTTON
    {
        /// <summary>
        /// A bitmask that specifies which members of this structure contain valid data; 
        /// other members are ignored, with the exception of <see cref="THUMBBUTTON.Id"/>, which is always required.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public THBMASK Mask;

        /// <summary>
        /// The application-defined identifier of the button, unique within the toolbar.
        /// </summary>
        public uint Id;

        /// <summary>
        /// The zero-based index of the button image within the image list set through <see cref="ITaskbarList3.ThumbBarSetImageList"/>
        /// </summary>
        public uint Bitmap;

        /// <summary>
        /// The handle of an icon to use as the button image.
        /// </summary>
        public IntPtr IconHandle;

        /// <summary>
        /// A wide character array that contains the text of the button's tooltip, displayed when the mouse pointer hovers over the button.
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string ToolTip;

        /// <summary>
        /// Flags that control specific states and behaviors of the button.
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public THBFLAGS Flags;
    }
}
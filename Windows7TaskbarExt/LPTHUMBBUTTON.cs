namespace Huseyint.Windows7.Native
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct LPTHUMBBUTTON
    {
        public uint dwMask;

        public uint iId;

        public uint iBitmap;

        public IntPtr hIcon;

        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szTip;

        public uint dwFlags;
    }
}
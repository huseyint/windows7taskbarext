namespace Huseyint.Windows7.Native
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct LPTHUMBBUTTON
    {
        public uint Mask;

        public uint Id;

        public uint Bitmap;

        public IntPtr IconHandle;

        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string Tip;

        public uint Flags;
    }
}
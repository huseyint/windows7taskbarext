namespace Huseyint.Windows7.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Win32
    {
        public const string TaskbarButtonCreatedMessage = "TaskbarButtonCreated";

        public const uint WindowMessageNull = 0x0000; // WM_NULL

        public const uint WindowMessageCommand = 0x0111; // WM_COMMAND

        public const uint ThumbnailBarButtonClicked = 0x1800; // THBN_CLICKED

        public const uint MessageFilterAdd = 1; // MSGFLT_ADD

        static Win32()
        {
            ClassIdTaskbarList = new Guid("56FDF344-FD6D-11d0-958A-006097C9A090");
        }

        public static Guid ClassIdTaskbarList { get; private set; } // CLSID_TaskbarList

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string str);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr ChangeWindowMessageFilter(uint message, uint flag);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr iconHandle);

        public static uint HiWord(IntPtr i)
        {
            return ((uint)i.ToInt32()) >> 16;
        }

        public static uint LoWord(IntPtr i)
        {
            return (uint)(i.ToInt32() & 0xffff);
        }
    }
}
namespace Huseyint.Windows7.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Win32
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr ChangeWindowMessageFilter(uint message, uint dwFlag);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        public const string TaskbarButtonCreatedMessage = "TaskbarButtonCreated";

        public const uint WM_NULL = 0x0000;

        public const uint MSGFLT_ADD = 1;

        public static Guid CLSID_TaskbarList = new Guid("56FDF344-FD6D-11d0-958A-006097C9A090");
    }
}
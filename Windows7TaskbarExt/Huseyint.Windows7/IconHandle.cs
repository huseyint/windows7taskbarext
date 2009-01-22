namespace Huseyint.Windows7.WindowsForms
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using Huseyint.Windows7.Native;

    internal class IconHandle : SafeHandle
    {
        internal IconHandle()
            : this(IntPtr.Zero)
        {
        }

        [SecurityCritical, SecurityTreatAsSafe]
        internal IconHandle(IntPtr ptr)
            : base(ptr, true)
        {
        }

        [SecurityTreatAsSafe, SecurityCritical]
        internal IconHandle(IntPtr ptr, bool fOwnsHandle)
            : base(ptr, fOwnsHandle)
        {
        }

        public HandleRef MakeHandleRef(object wrapper)
        {
            return new HandleRef(wrapper, base.handle);
        }

        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            return Win32.DestroyIcon(base.handle);
        }

        public override bool IsInvalid
        {
            [SecurityTreatAsSafe, SecurityCritical]
            get
            {
                return (base.handle == IntPtr.Zero);
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as IconHandle;

            return other != null && other.handle.Equals(this.handle);
        }

        public override int GetHashCode()
        {
            return this.handle.ToInt32();
        }
    }
}
namespace Huseyint.Windows7.Native
{
    using System;

    [Flags]
    internal enum THBFLAGS : uint
    {
        /// <summary>
        /// Default. The button is active and available to the user.
        /// </summary>
        THBF_ENABLED = 0x0000,

        /// <summary>
        /// The button is disabled. It is present, but has a visual state that indicates that it will not respond to user action.
        /// </summary>
        THBF_DISABLED = 0x0001,

        /// <summary>
        /// When the button is clicked, the taskbar button's flyout closes immediately.
        /// </summary>
        THBF_DISMISSONCLICK = 0x0002,

        /// <summary>
        /// Do not draw a button border, use only the image.
        /// </summary>
        THBF_NOBACKGROUND = 0x0004,

        /// <summary>
        /// The button is not shown to the user.
        /// </summary>
        THBF_HIDDEN = 0x0008,
    }
}
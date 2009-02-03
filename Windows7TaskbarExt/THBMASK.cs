namespace Huseyint.Windows7.Native
{
    using System;

    [Flags]
    internal enum THBMASK : uint
    {
        /// <summary>
        /// The <see cref="THUMBBUTTON.Bitmap"/> member contains valid information.
        /// </summary>
        THB_BITMAP = 0x0001,
        
        /// <summary>
        /// The <see cref="THUMBBUTTON.IconHandle"/> member contains valid information.
        /// </summary>
        THB_ICON = 0x0002,
        
        /// <summary>
        /// The <see cref="THUMBBUTTON.ToolTip"/> member contains valid information.
        /// </summary>
        THB_TOOLTIP = 0x0004,
        
        /// <summary>
        /// The <see cref="THUMBBUTTON.Flags"/> member contains valid information.
        /// </summary>
        THB_FLAGS = 0x0008,
    }
}
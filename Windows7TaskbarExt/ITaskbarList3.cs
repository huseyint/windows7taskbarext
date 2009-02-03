namespace Huseyint.Windows7.Native
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ITaskbarList3
    {
        /* ITaskbarList */
        [PreserveSig]
        void HrInit();

        [PreserveSig]
        void AddTab([In] IntPtr hwnd);

        [PreserveSig]
        void DeleteTab([In] IntPtr hwnd);

        [PreserveSig]
        void ActivateTab([In] IntPtr hwnd);

        [PreserveSig]
        void SetActiveAlt([In] IntPtr hwnd);

        /* ITaskbarList2 */
        [PreserveSig]
        void MarkFullscreenWindow(
            [In] IntPtr hwnd, 
            [In, MarshalAs(UnmanagedType.Bool)] bool fullscreen);

        /* ITaskbarList3 */

        /// <summary>
        /// Displays or updates a progress bar hosted in a taskbar button to show 
        /// the specific percentage completed of the full operation.
        /// </summary>
        /// <param name="hwnd">The handle of the window whose associated taskbar 
        /// button is being used as a progress indicator.</param>
        /// <param name="ullCompleted">An application-defined value that indicates 
        /// the proportion of the operation that has been completed at the time 
        /// the method is called.</param>
        /// <param name="ullTotal">An application-defined value that specifies the 
        /// value ullCompleted will have when the operation is complete.</param>
        void SetProgressValue(
            [In] IntPtr hwnd,
            [In] ulong ullCompleted,
            [In] ulong ullTotal);

        /// <summary>
        /// Sets the type and state of the progress indicator displayed on a taskbar button.
        /// </summary>
        /// <param name="hwnd">The handle of the window in which the progress of an 
        /// operation is being shown. This window's associated taskbar button will 
        /// display the progress bar.</param>
        /// <param name="tbpFlags">Flags that control the current state of the progress 
        /// button. Specify only one of the <see cref="TBPFLAG"/>; all states are mutually 
        /// exclusive of all others.</param>
        void SetProgressState(
            [In] IntPtr hwnd,
            [In] TBPFLAG tbpFlags);

        void RegisterTab(
            [In] IntPtr hwndTab,
            [In] IntPtr hwndMDI);

        void UnregisterTab(
            [In] IntPtr hwndTab);

        void SetTabOrder(
            [In] IntPtr hwndTab,
            [In] IntPtr hwndInsertBefore);

        void SetTabActive(
            [In] IntPtr hwndTab,
            [In] IntPtr hwndMDI,
            [In] TBATFLAG tbatFlags);

        /// <summary>
        /// Adds a thumbnail toolbar with a specified set of buttons to the thumbnail image 
        /// of a window in a taskbar button flyout.
        /// </summary>
        /// <param name="hwnd">The handle of the window whose thumbnail representation will 
        /// receive the toolbar. This handle must belong to the calling process.</param>
        /// <param name="buttonsCount">The number of buttons defined in the array pointed 
        /// to by <paramref name="buttons"/>. The maximum number of buttons allowed is 7.</param>
        /// <param name="buttons">A pointer to an array of <see cref="THUMBBUTTON"/> structures. 
        /// Each <see cref="THUMBBUTTON"/> defines an individual button to be added to the toolbar. 
        /// Buttons cannot be added or deleted later, so this must be the full defined set. Buttons 
        /// also cannot be reordered, so their order in the array, which is the order in which they 
        /// are displayed left to right, will be their permanent order.</param>
        void ThumbBarAddButtons(
            [In] IntPtr hwnd,
            [In] uint buttonsCount,
            [In, MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] buttons);

        /// <summary>
        /// Shows, enables, disables, or hides buttons in a thumbnail toolbar as required by the 
        /// window's current state. A thumbnail toolbar is a toolbar embedded in a thumbnail image 
        /// of a window in a taskbar button flyout.
        /// </summary>
        /// <param name="hwnd">The handle of the window whose thumbnail representation contains 
        /// the toolbar.</param>
        /// <param name="buttonsCount">The number of buttons defined in the array <paramref name="buttons"/>. 
        /// The maximum number of buttons allowed is 7. This array contains only structures that represent 
        /// existing buttons that are being updated.</param>
        /// <param name="buttons">An array of <see cref="THUMBBUTTON"/> structures. Each <see cref="THUMBBUTTON"/> 
        /// defines an individual button. If the button already exists (the iId value is already defined), then that 
        /// existing button is updated with the information provided in the structure.</param>
        void ThumbBarUpdateButtons(
            [In] IntPtr hwnd,
            [In] uint buttonsCount,
            [In, MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] buttons);

        /// <summary>
        /// Specifies an image list that contains button images for a toolbar embedded in a thumbnail 
        /// image of a window in a taskbar button flyout.
        /// </summary>
        /// <param name="hwnd"> The handle of the window whose thumbnail representation contains the 
        /// toolbar to be updated. This handle must belong to the calling process.</param>
        /// <param name="imageListHandle">The handle of the image list that contains all button images 
        /// to be used in the toolbar.</param>
        void ThumbBarSetImageList(
            [In] IntPtr hwnd,
            [In] IntPtr imageListHandle);

        /// <summary>
        /// Applies an overlay to a taskbar button to indicate application 
        /// status or a notification to the user.
        /// </summary>
        /// <param name="hwnd">The handle of the window whose associated 
        /// taskbar button receives the overlay. This handle must belong 
        /// to a calling process associated with the button's application 
        /// and must be a valid HWND or the call is ignored.</param>
        /// <param name="iconHandle">
        ///  The handle of an icon to use as the overlay. This should be 
        ///  a small icon, measuring 16x16 pixels at 96 dots per inch (dpi). 
        ///  If an overlay icon is already applied to the taskbar button, 
        ///  that existing overlay is replaced.
        ///  
        ///  This value can be NULL. How a NULL value is handled depends on 
        ///  whether the taskbar button represents a single window or a 
        ///  group of windows.
        ///  
        ///      * If the taskbar button represents a single window, the overlay icon is removed from the display.
        ///      * If the taskbar button represents a group of windows and a previous overlay is still available (received earlier than the current overlay, but not yet freed by a NULL value), then that previous overlay is displayed in place of the current overlay.
        ///  
        ///  It is the responsibility of the calling application to free 
        ///  hIcon when it is no longer needed. This can generally be done 
        ///  after you've called SetOverlayIcon because the taskbar makes 
        ///  and uses its own copy of the icon.
        /// </param>
        /// <param name="description">A pointer to a string that provides 
        /// an alt text version of the information conveyed by the overlay, 
        /// for accessibility purposes.</param>
        void SetOverlayIcon(
            [In] IntPtr hwnd,
            [In] SafeHandle iconHandle,
            [In] string description);

        void SetThumbnailTooltip(
            [In] IntPtr hwnd,
            [In, MarshalAs(UnmanagedType.LPWStr)] string tip);

        void SetThumbnailClip(
            [In] IntPtr hwnd,
            [In] RECT clip);
    }
}
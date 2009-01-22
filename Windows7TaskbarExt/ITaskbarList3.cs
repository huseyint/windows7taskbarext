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
        void HrInit();
        void AddTab([In] IntPtr hWnd);
        void DeleteTab([In] IntPtr hWnd);
        void ActivateTab([In] IntPtr hWnd);
        void SetActiveAlt([In] IntPtr hWnd);

        /* ITaskbarList2 */
        void MarkFullscreenWindow([In] IntPtr hWnd, [In] int fFullscreen);

        /* ITaskbarList3 */

        /// <summary>
        /// Displays or updates a progress bar hosted in a taskbar button to show 
        /// the specific percentage completed of the full operation.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose associated taskbar 
        /// button is being used as a progress indicator.</param>
        /// <param name="ullCompleted">An application-defined value that indicates 
        /// the proportion of the operation that has been completed at the time 
        /// the method is called.</param>
        /// <param name="ullTotal">An application-defined value that specifies the 
        /// value ullCompleted will have when the operation is complete.</param>
        void SetProgressValue(
            [In] IntPtr hWnd,
            [In] ulong ullCompleted,
            [In] ulong ullTotal);

        /// <summary>
        /// Sets the type and state of the progress indicator displayed on a taskbar button.
        /// </summary>
        /// <param name="hWnd">The handle of the window in which the progress of an 
        /// operation is being shown. This window's associated taskbar button will 
        /// display the progress bar.</param>
        /// <param name="tbpFlags">Flags that control the current state of the progress 
        /// button. Specify only one of the <see cref="TBPFLAG"/>; all states are mutually 
        /// exclusive of all others.</param>
        void SetProgressState(
            [In] IntPtr hWnd,
            [In] TBPFLAG tbpFlags);

        void RegisterTab(
            [In] IntPtr hWndTab,
            [In] IntPtr hWndMDI);

        void UnregisterTab(
            [In] IntPtr hWndTab);

        void SetTabOrder(
            [In] IntPtr hWndTab,
            [In] IntPtr hWndInsertBefore);

        void SetTabActive(
            [In] IntPtr hWndTab,
            [In] IntPtr hWndMDI,
            [In] TBATFLAG tbatFlags);

        void ThumbBarAddButtons(
            [In] IntPtr hWnd,
            [In] uint cButtons,
            [In] LPTHUMBBUTTON pButton);

        void ThumbBarUpdateButtons(
            [In] IntPtr hWnd,
            [In] uint cButtons,
            [In] LPTHUMBBUTTON pButton);

        void ThumbBarSetImageList(
            [In] IntPtr hWnd,
            [In] IntPtr hImageList);

        /// <summary>
        /// Applies an overlay to a taskbar button to indicate application 
        /// status or a notification to the user.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose associated 
        /// taskbar button receives the overlay. This handle must belong 
        /// to a calling process associated with the button's application 
        /// and must be a valid HWND or the call is ignored.</param>
        /// <param name="hIcon">
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
        /// <param name="pszDescription">A pointer to a string that provides 
        /// an alt text version of the information conveyed by the overlay, 
        /// for accessibility purposes.</param>
        void SetOverlayIcon(
            [In] IntPtr hWnd,
            [In] SafeHandle hIcon,
            [In] string pszDescription);

        void SetThumbnailTooltip(
            [In] IntPtr hWnd,
            [In] string pszTip);

        void SetThumbnailClip(
            [In] IntPtr hWnd,
            [In] RECT prcClip);
    }
}
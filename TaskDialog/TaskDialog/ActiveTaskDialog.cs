
//******************************************************************************************//
//                                                                                          //
//     Part of the code in this file has no license attached to it                          //
//     and can be found at http://www.codeproject.com/KB/vista/TaskDialogWinForms.aspx.     //
//                                                                                          //
//     Additional code was added by Wintaskdialog.com to allow integration with             //
//     'FormTaskDialog' class which is used as a Task Dialog on operating systems           //
//     other than Microsoft Windows Vista.                                                  //
//                                                                                          //
//******************************************************************************************//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace WindowsTaskDialog
{
    /// <summary>
    /// Represents the active Task Dialog window. Provides methods for updating information displayed in the Task Dialog.
    /// </summary>
    public class ActiveTaskDialog : IWin32Window
    {
        /// <summary>
        /// The Task Dialog's window handle.
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")] // We don't own the window.
        private IntPtr handle;
        private FormTaskDialog frmTaskDialog;

        /// <summary>
        /// Initializes a new instance of the ActiveTaskDialog class.
        /// </summary>
        /// <param name="handle">The native Task Dialog's window handle.</param>
        internal ActiveTaskDialog(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException("handle");
            }

            this.handle = handle;
            this.frmTaskDialog = null;
        }

        /// <summary>
        /// Initializes a new instance of the ActiveTaskDialog class.
        /// </summary>
        /// <param name="frmTaskDialog">The Task Dialog's form (when native Task Dialog is not supported on this OS).</param>
        internal ActiveTaskDialog(FormTaskDialog frmTaskDialog)
        {
            if (frmTaskDialog == null)
            {
                throw new ArgumentNullException("frmTaskDialog");
            }

            this.handle = IntPtr.Zero;
            this.frmTaskDialog = frmTaskDialog;
        }

        /// <summary>
        /// Gets the Task Dialog's window handle.
        /// </summary>
        public IntPtr Handle
        {
            get { return (this.handle != IntPtr.Zero) ? (this.handle) : (this.frmTaskDialog.Handle); }
        }

        /// <summary>
        /// Updates the main instruction of the Task Dialog.
        /// </summary>
        /// <param name="mainInstruction">The new main instruction of the Task Dialog.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool SetMainInstruction(string mainInstruction)
        {
            if (this.handle != IntPtr.Zero)
            {

                // TDE_MAIN_INSTRUCTION
                // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                return UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_MAIN_INSTRUCTION,
                    mainInstruction) != IntPtr.Zero;
            }
            else
            {
                return this.frmTaskDialog.SetMainInstruction(mainInstruction);
            }
        }

        /// <summary>
        /// Updates the content of the Task Dialog.
        /// </summary>
        /// <param name="content">The new content of the Task Dialog.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool SetContent(string content)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDE_CONTENT,
                // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                return UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_CONTENT,
                    content) != IntPtr.Zero;
            }
            else
            {
                return this.frmTaskDialog.SetContent(content);
            }
        }

        /// <summary>
        /// Updates the expanded (additional) information of the Task Dialog.
        /// </summary>
        /// <param name="expandedInformation">The new expanded (additional) information of the Task Dialog.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool SetExpandedInformation(string expandedInformation)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDE_EXPANDED_INFORMATION,
                // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                return UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_EXPANDED_INFORMATION,
                    expandedInformation) != IntPtr.Zero;
            }
            else
            {
                return this.frmTaskDialog.SetExpandedInformation(expandedInformation);
            }
        }

        /// <summary>
        /// Indicates whether a marquee style progress bar should be displayed.
        /// </summary>
        /// <param name="marquee">Whether to display a marquee style progress bar.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool SetMarqueeProgressBar(bool marquee)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_SET_MARQUEE_PROGRESS_BAR        = WM_USER+103, // wParam = 0 (nonMarque) wParam != 0 (Marquee)
                return UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_MARQUEE_PROGRESS_BAR,
                    (marquee ? (IntPtr)1 : IntPtr.Zero),
                    IntPtr.Zero) != IntPtr.Zero;

                // Future: get more detailed error from and throw.
            }
            else
            {
                return this.frmTaskDialog.SetMarqueeProgressBar(marquee);
            }
        }

        /// <summary>
        /// Sets the state of the progress bar. This method has effect only on Windows Vista or higher.
        /// </summary>
        /// <param name="newState">The new progress bar state.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool SetProgressBarState(ProgressBarState newState)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_SET_PROGRESS_BAR_STATE          = WM_USER+104, // wParam = new progress state
                return UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_STATE,
                    (IntPtr)newState,
                    IntPtr.Zero) != IntPtr.Zero;

                // Future: get more detailed error from and throw.
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the minimum and maximum range values to represent the progress of a task of 
        /// the progress bar.
        /// </summary>
        /// <param name="minRange">The minimum value of the range of the progress bar. The default value is 0.</param>
        /// <param name="maxRange">The maximum value of the range of the progress bar. The default value is 100.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool SetProgressBarRange(short minRange, short maxRange)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_SET_PROGRESS_BAR_RANGE          = WM_USER+105, // lParam = MAKELPARAM(nMinRange, nMaxRange)
                // #define MAKELPARAM(l, h)      ((LPARAM)(DWORD)MAKELONG(l, h))
                // #define MAKELONG(a, b)      ((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
                IntPtr lparam = (IntPtr)((((Int32)minRange) & 0xffff) | ((((Int32)maxRange) & 0xffff) << 16));
                return UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_RANGE,
                    IntPtr.Zero,
                    lparam) != IntPtr.Zero;

                // Return value is actually prior range.
            }
            else
            {
                return this.frmTaskDialog.SetProgressBarRange(minRange, maxRange);
            }
        }

        /// <summary>
        /// Sets the current position of the progress bar.
        /// </summary>
        /// <param name="newPosition">The new position of the progress bar.</param>
        /// <returns>If the method succeeds the previous position is returned, otherwise 0 is returned.</returns>
        public int SetProgressBarPosition(int newPosition)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_SET_PROGRESS_BAR_POS            = WM_USER+106, // wParam = new position
                return (int)UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_POS,
                    (IntPtr)newPosition,
                    IntPtr.Zero);
            }
            else
            {
                return this.frmTaskDialog.SetProgressBarPosition(newPosition);
            }
        }

        /// <summary>
        /// Sets the animation state of the marquee style progress bar.
        /// </summary>
        /// <param name="startMarquee">Indicates whether to start the marquee animation.</param>
        /// <param name="speed">The time in milliseconds that it takes the progress block to scroll 
        /// across the progress bar.</param>
        public void SetProgressBarMarquee(bool startMarquee, uint speed)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_SET_PROGRESS_BAR_MARQUEE        = WM_USER+107, // wParam = 0 (stop marquee), wParam != 0 (start marquee), lparam = speed (milliseconds between repaints)
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_PROGRESS_BAR_MARQUEE,
                    (startMarquee ? new IntPtr(1) : IntPtr.Zero),
                    (IntPtr)speed);
            }
            else
            {
                this.frmTaskDialog.SetProgressBarMarquee(startMarquee, speed);
            }
        }

        /// <summary>
        /// Enables or disables a radio button in the Task Dialog.
        /// </summary>
        /// <param name="buttonId">Indicates the radio button ID to be enabled or disabled. This may be any 
        /// of the values specified in ButtonId properties of one of the TaskDialogButton structures 
        /// in the RadioButtons property of the TaskDialog class.</param>
        /// <param name="enable">Indicates whether to enable the specified radio button.</param>
        public void EnableRadioButton(int buttonId, bool enable)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_ENABLE_RADIO_BUTTON = WM_USER+112, // lParam = 0 (disable), lParam != 0 (enable), wParam = Radio Button ID
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_ENABLE_RADIO_BUTTON,
                    (IntPtr)buttonId,
                    (IntPtr)(enable ? 1 : 0));
            }
            else
            {
                this.frmTaskDialog.EnableRadioButton(buttonId, enable);
            }
        }
        
        /// <summary>
        /// Simulates the action of a radio button click in the Task Dialog.
        /// </summary>
        /// <param name="buttonId">Indicates the radio button ID to be selected. This may be any 
        /// of the values specified in ButtonId properties of one of the TaskDialogButton structures 
        /// in the RadioButtons property of the TaskDialog class.</param>
        public void ClickRadioButton(int buttonId)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_CLICK_RADIO_BUTTON = WM_USER+110, // wParam = Radio Button ID
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_RADIO_BUTTON,
                    (IntPtr)buttonId,
                    IntPtr.Zero);
            }
            else
            {
                this.frmTaskDialog.ClickRadioButton(buttonId);
            }
        }

        /// <summary>
        /// Enables or disables a button in the Task Dialog.
        /// </summary>
        /// <param name="buttonId">Indicates the button ID to be enabled or disabled. This may be any of the 
        /// values specified in ButtonId properties of one of the TaskDialogButton structures in the Buttons 
        /// property, or one of the TaskDialogCommonButtons enumeration values specified in 
        /// the CommonButtons property of the TaskDialog class.</param>
        /// <param name="enable">Indicates whether to enable the specified button.</param>
        public void EnableButton(int buttonId, bool enable)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_ENABLE_BUTTON = WM_USER+111, // lParam = 0 (disable), lParam != 0 (enable), wParam = Button ID
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_ENABLE_BUTTON,
                    (IntPtr)buttonId,
                    (IntPtr)(enable ? 1 : 0));
            }
            else
            {
                this.frmTaskDialog.EnableButton(buttonId, enable);
            }
        }

        /// <summary>
        /// Designates whether a given Task Dialog button or command link should have a 
        /// User Account Control (UAC) shield icon. This method has effect only on Windows Vista or higher.
        /// </summary>
        /// <param name="buttonId">Indicates the button ID to be updated. This may be any of the 
        /// values specified in ButtonId properties of one of the TaskDialogButton structures in the Buttons 
        /// property, or one of the TaskDialogCommonButtons enumeration values specified in 
        /// the CommonButtons property of the TaskDialog class.</param>
        /// <param name="elevationRequired">Indicates whether the action invoked by the specified button 
        /// requires elevation.</param>
        public void SetButtonElevationRequiredState(int buttonId, bool elevationRequired)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE = WM_USER+115, // wParam = Button ID, lParam = 0 (elevation not required), lParam != 0 (elevation required)
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE,
                    (IntPtr)buttonId,
                    (IntPtr)(elevationRequired ? new IntPtr(1) : IntPtr.Zero));
            }
        }

        /// <summary>
        /// Simulates the action of a button click in the Task Dialog.
        /// </summary>
        /// <param name="buttonId">Indicates the button ID to be selected. This may be any of the 
        /// values specified in ButtonId properties of one of the TaskDialogButton structures in the Buttons 
        /// property, or one of the TaskDialogCommonButtons enumeration values specified in 
        /// the CommonButtons property of the TaskDialog class.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool ClickButton(int buttonId)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_CLICK_BUTTON                    = WM_USER+102, // wParam = Button ID
                return UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_BUTTON,
                    (IntPtr)buttonId,
                    IntPtr.Zero) != IntPtr.Zero;
            }
            else
            {
                return this.frmTaskDialog.ClickButton(buttonId);
            }
        }

        /// <summary>
        /// Simulates a click on the verification checkbox of the Task Dialog, if it exists.
        /// </summary>
        /// <param name="checkedState">Indicates whether to set the state of the verification checkbox 
        /// to be checked.</param>
        /// <param name="setKeyboardFocusToCheckBox">Indicates whether to set the keyboard focus 
        /// to the verification checkbox.</param>
        public void ClickVerification(bool checkedState, bool setKeyboardFocusToCheckBox)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_CLICK_VERIFICATION = WM_USER+113, // wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_CLICK_VERIFICATION,
                    (checkedState ? new IntPtr(1) : IntPtr.Zero),
                    (setKeyboardFocusToCheckBox ? new IntPtr(1) : IntPtr.Zero));
            }
            else
            {
                this.frmTaskDialog.ClickVerification(checkedState, setKeyboardFocusToCheckBox);
            }
        }
        
        /// <summary>
        /// Updates the footer of the Task Dialog.
        /// </summary>
        /// <param name="footer">The new footer of the Task Dialog.</param>
        /// <returns>If the method succeeds the return value is true.</returns>
        public bool SetFooter(string footer)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDE_FOOTER,
                // TDM_SET_ELEMENT_TEXT                = WM_USER+108  // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                return UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_SET_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_FOOTER,
                    footer) != IntPtr.Zero;
            }
            else
            {
                return this.frmTaskDialog.SetFooter(footer);
            }
        }

        /// <summary>
        /// Updates the main icon for the Task Dialog. Does not cause the dialog box to resize.
        /// </summary>
        /// <param name="icon">The system (built-in) icon for the Task Dialog.</param>
        public void UpdateMainIcon(TaskDialogIcon icon)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_MAIN,
                    (IntPtr)icon);
            }
            else
            {
                this.frmTaskDialog.UpdateMainIcon(icon);
            }
        }

        /// <summary>
        /// Updates the main icon for the Task Dialog. Does not cause the dialog box to resize. 
        /// </summary>
        /// <param name="icon">The custom icon for the Task Dialog.</param>
        public void UpdateMainIcon(Icon icon)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_MAIN,
                    (icon == null ? IntPtr.Zero : icon.Handle));
            }
            else
            {
                this.frmTaskDialog.UpdateMainIcon(icon);
            }
        }

        /// <summary>
        /// Updates the main instruction of the Task Dialog. Does not cause the dialog box to resize.
        /// </summary>
        /// <param name="mainInstruction">The new main instruction of the Task Dialog.</param>
        public void UpdateMainInstruction(string mainInstruction)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDE_MAIN_INSTRUCTION
                // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_MAIN_INSTRUCTION,
                    mainInstruction);
            }
            else
            {
                this.frmTaskDialog.UpdateMainInstruction(mainInstruction);
            }
        }
        
        /// <summary>
        /// Updates the content of the Task Dialog. Does not cause the dialog box to resize.
        /// </summary>
        /// <param name="content">The new content of the Task Dialog.</param>
        public void UpdateContent(string content)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDE_CONTENT,
                // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_CONTENT,
                    content);
            }
            else
            {
                this.frmTaskDialog.UpdateContent(content);
            }
        }

        /// <summary>
        /// Updates the expanded (additional) information of the Task Dialog. Does not cause the dialog box to resize.
        /// </summary>
        /// <param name="expandedInformation">The new expanded (additional) information of the Task Dialog.</param>
        public void UpdateExpandedInformation(string expandedInformation)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDE_EXPANDED_INFORMATION,
                // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_EXPANDED_INFORMATION,
                    expandedInformation);
            }
            else
            {
                this.frmTaskDialog.UpdateExpandedInformation(expandedInformation);
            }
        }

        /// <summary>
        /// Updates the footer icon for the Task Dialog. Does not cause the dialog box to resize.
        /// </summary>
        /// <param name="icon">The system (built-in) icon for the Task Dialog.</param>
        public void UpdateFooterIcon(TaskDialogIcon icon)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_FOOTER,
                    (IntPtr)icon);
            }
            else
            {
                this.frmTaskDialog.UpdateFooterIcon(icon);
            }
        }

        /// <summary>
        /// Updates the footer icon for the Task Dialog. Does not cause the dialog box to resize. 
        /// </summary>
        /// <param name="icon">The custom icon for the Task Dialog.</param>
        public void UpdateFooterIcon(Icon icon)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDM_UPDATE_ICON = WM_USER+116  // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
                UnsafeNativeMethods.SendMessage(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ICON,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ICON_ELEMENTS.TDIE_ICON_FOOTER,
                    (icon == null ? IntPtr.Zero : icon.Handle));
            }
            else
            {
                this.frmTaskDialog.UpdateFooterIcon(icon);
            }
        }

        /// <summary>
        /// Updates the footer of the Task Dialog. Does not cause the dialog box to resize.
        /// </summary>
        /// <param name="footer">The new footer of the Task Dialog.</param>
        public void UpdateFooter(string footer)
        {
            if (this.handle != IntPtr.Zero)
            {
                // TDE_FOOTER,
                // TDM_UPDATE_ELEMENT_TEXT             = WM_USER+114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)
                UnsafeNativeMethods.SendMessageWithString(
                    this.handle,
                    (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_UPDATE_ELEMENT_TEXT,
                    (IntPtr)UnsafeNativeMethods.TASKDIALOG_ELEMENTS.TDE_FOOTER,
                    footer);
            }
            else
            {
                this.frmTaskDialog.UpdateFooter(footer);
            }
        }

        /// <summary>
        /// Dynamically changes the Task Dialog contents at run time. A new Task Dialog (looks like a new page) is
        /// created with the elements specified in the taskDialogParams parameter. 
        /// </summary>
        /// <param name="taskDialogParams">The new Task Dialog to create.</param>
        public void NavigatePage(TaskDialog taskDialogParams)
        {
            if (this.handle != IntPtr.Zero)
            {
                using (TaskDialog.TaskDialogConfig config = new TaskDialog.TaskDialogConfig(taskDialogParams, IntPtr.Zero))
                {
                    UnsafeNativeMethods.TASKDIALOGCONFIG nativeConfig = config.Config;
                    // TDM_NAVIGATE_PAGE                   = WM_USER+101,
                    UnsafeNativeMethods.SendMessageWithTaskDialogConfig(
                        this.handle,
                        (uint)UnsafeNativeMethods.TASKDIALOG_MESSAGES.TDM_NAVIGATE_PAGE,
                        IntPtr.Zero,
                        ref nativeConfig);
                }
            }
            else
            {
                this.frmTaskDialog.NavigatePage(taskDialogParams);
            }
        }
    }
}

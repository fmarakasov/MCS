
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
    /// Represents the method that will handle the Callback event of a TaskDialog.
    /// </summary>
    /// <param name="sender">The source of the event (a TaskDialog).</param>
    /// <param name="e">A TaskDialogCallbackEventArgs that contains the event data.</param>
    /// <returns></returns>
    public delegate void TaskDialogCallbackEventHandler(Object sender, TaskDialogCallbackEventArgs e);

    /// <summary>
    /// Specifies the common (built-in) push buttons displayed in the Task Dialog. 
    /// This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum TaskDialogCommonButtons
    {
        /// <summary>
        /// No common buttons.
        /// </summary>
        None = 0,

        /// <summary>
        /// OK common button.
        /// </summary>
        Ok = 0x0001,

        /// <summary>
        /// Yes common button.
        /// </summary>
        Yes = 0x0002,

        /// <summary>
        /// No common button.
        /// </summary>
        No = 0x0004,

        /// <summary>
        /// Cancel common button. 
        /// If this button is specified, the Task Dialog will be able to be closed using Alt-F4, 
        /// Escape and the title bar’s Close button. 
        /// </summary>
        Cancel = 0x0008,

        /// <summary>
        /// Retry common button.
        /// </summary>
        Retry = 0x0010,

        /// <summary>
        /// Close common button.
        /// </summary>
        Close = 0x0020,
    }

    /// <summary>
    /// Specifies the system (built-in) icons for the Task Dialog.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")] // Type comes from CommCtrl.h
    public enum TaskDialogIcon : uint
    {
        /// <summary>
        /// No icon.
        /// </summary>
        None = 0,

        /// <summary>
        /// Warning icon.
        /// </summary>
        Warning = 0xFFFF, // MAKEINTRESOURCEW(-1)

        /// <summary>
        /// Error icon.
        /// </summary>
        Error = 0xFFFE, // MAKEINTRESOURCEW(-2)

        /// <summary>
        /// Information icon.
        /// </summary>
        Information = 0xFFFD, // MAKEINTRESOURCEW(-3)

        /// <summary>
        /// Shield icon.
        /// </summary>
        Shield = 0xFFFC, // MAKEINTRESOURCEW(-4)
    }

    /// <summary>
    /// Specifies notifications for the Task Dialog’s Callback event.
    /// </summary>
    public enum TaskDialogNotification
    {
        /// <summary>
        /// Sent by the Task Dialog once the dialog has been created and before it is displayed.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Sent by the Task Dialog when a navigation to a new page (new Task Dialog contents) has occurred. 
        /// This notification is sent when the TaskDialogCallbackEventArgs.ActiveDialog.NavigatePage method 
        /// has been called.
        /// </summary>   
        Navigated = 1,

        /// <summary>
        /// Sent by the Task Dialog when the user selects a button or command link in the Task Dialog. 
        /// The button ID corresponding to the button selected will be available in the 
        /// TaskDialogCallbackEventArgs.ButtonId property. To prevent the Task Dialog from closing set 
        /// the TaskDialogCallbackEventArgs.CloseTaskDialog property to false, otherwise the Task Dialog 
        /// will be closed and the button ID returned via the TaskDialog.Show method.
        /// </summary>
        ButtonClicked = 2,            // wParam = Button ID

        /// <summary>
        /// Sent by the Task Dialog when the user clicks a hyperlink in the Task Dialog content, 
        /// expanded information or footer. The string containing the HREF of the hyperlink will be 
        /// available in the TaskDialogCallbackEventArgs.Hyperlink property.
        /// </summary>
        HyperlinkClicked = 3,            // lParam = (LPCWSTR)pszHREF

        /// <summary>
        /// Sent by the Task Dialog approximately every 200 milliseconds when the TaskDialog.CallbackTimer 
        /// property has been set to true. The number of milliseconds since the dialog was created or the 
        /// TaskDialogCallbackEventArgs.ResetTimerTickCount property was set to true is available in the 
        /// TaskDialogCallbackEventArgs.TimerTickCount property. To reset the tickcount set 
        /// the TaskDialogCallbackEventArgs.ResetTimerTickCount property to true, otherwise the tickcount will 
        /// continue to increment.
        /// </summary>
        Timer = 4,            // wParam = Milliseconds since dialog created or timer reset

        /// <summary>
        /// Sent by the Task Dialog when it is destroyed and its window handle is no longer valid.
        /// </summary>
        Destroyed = 5,

        /// <summary>
        /// Sent by the Task Dialog when the user selects a radio button in the task dialog. 
        /// The radio button ID corresponding to the radio button selected will be available in the 
        /// TaskDialogCallbackEventArgs.ButtonId property.
        /// </summary>
        RadioButtonClicked = 6,            // wParam = Radio Button ID

        /// <summary>
        /// Sent by the Task Dialog once the dialog has been constructed and before it is displayed.
        /// </summary>
        DialogConstructed = 7,

        /// <summary>
        /// Sent by the Task Dialog when the user clicks the Task Dialog verification check box. 
        /// The state of the verification checkbox will be available in the 
        /// TaskDialogCallbackEventArgs.VerificationFlagChecked property.
        /// </summary>
        VerificationClicked = 8,             // wParam = 1 if checkbox checked, 0 if not, lParam is unused and always 0

        /// <summary>
        /// Sent by the Task Dialog when the user presses F1 on the keyboard while the dialog has focus.
        /// </summary>
        Help = 9,

        /// <summary>
        /// Sent by the Task Dialog when the user clicks on the dialog's expand/collapse button. 
        /// The value indicating whether the expanded (additional) information is visible will be available 
        /// in the TaskDialogCallbackEventArgs.Expanded property.
        /// </summary>
        ExpandoButtonClicked = 10            // wParam = 0 (dialog is now collapsed), wParam != 0 (dialog is now expanded)
    }

    /// <summary>
    /// Specifies the progress bar states.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")] // Comes from CommCtrl.h PBST_* values which don't have a zero.
    public enum ProgressBarState
    {
        /// <summary>
        /// Normal state.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Error state.
        /// </summary>
        Error = 2,

        /// <summary>
        /// Paused state.
        /// </summary>
        Paused = 3
    }

    /// <summary>
    /// Represents a button of the Task Dialog.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")] // Would be unused code as not required for usage.
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct TaskDialogButton
    {
        /// <summary>
        /// The ID of the button. This value is returned by the TaskDialog.Show method when the button is clicked.
        /// </summary>
        private int buttonId;

        /// <summary>
        /// The string that appears on the button.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        private string buttonText;

        /// <summary>
        /// Initializes a new instance of the TaskDialogButton struct.
        /// </summary>
        /// <param name="id">The programmatic identifier assigned to the button. This value is returned 
        /// by the TaskDialog.Show method when the button is clicked.</param>
        /// <param name="text">The caption displayed on the button.</param>
        public TaskDialogButton(int id, string text)
        {
            this.buttonId = id;
            this.buttonText = text;
        }

        /// <summary>
        /// Gets or sets the programmatic identifier assigned to the button. 
        /// This value is returned by the TaskDialog.Show method when the button is clicked.
        /// </summary>
        public int ButtonId
        {
            get { return this.buttonId; }
            set { this.buttonId = value; }
        }

        /// <summary>
        /// Gets or sets the caption displayed on the button.
        /// </summary>
        public string ButtonText
        {
            get { return this.buttonText; }
            set { this.buttonText = value; }
        }
    }

    /// <summary>
    /// Represents the main class that allows you to create and display a task dialog.
    /// </summary>
    public class TaskDialog
    {
        /// <summary>
        /// The string to be used for the dialog box title. If this parameter is NULL, the filename of the executable program is used.
        /// </summary>
        private string windowTitle;

        /// <summary>
        /// The string to be used for the main instruction.
        /// </summary>
        private string mainInstruction;

        /// <summary>
        /// The string to be used for the dialog’s primary content. If the EnableHyperlinks member is true,
        /// then this string may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>. 
        /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
        /// </summary>
        private string content;

        /// <summary>
        /// Specifies the push buttons displayed in the dialog box.  This parameter may be a combination of flags.
        /// If no common buttons are specified and no custom buttons are specified using the Buttons member, the
        /// dialog box will contain the OK button by default.
        /// </summary>
        private TaskDialogCommonButtons commonButtons;

        /// <summary>
        /// Specifies a built in icon for the main icon in the dialog. If this is set to none
        /// and the CustomMainIcon is null then no main icon will be displayed.
        /// </summary>
        private TaskDialogIcon mainIcon;

        /// <summary>
        /// Specifies a custom icon for the main icon in the dialog. If this is null
        /// and the MainIcon member is set to none then no main icon will be displayed.
        /// </summary>
        private Icon customMainIcon;

        /// <summary>
        /// Specifies a built in icon for the icon to be displayed in the footer area of the
        /// dialog box. If this is set to none and the CustomFooterIcon member is null then no
        /// footer icon will be displayed.
        /// </summary>
        private TaskDialogIcon footerIcon;

        /// <summary>
        /// Specifies a custom icon for the icon to be displayed in the footer area of the
        /// dialog box. If this is null and the FooterIcon member is set to none then no
        /// footer icon will be displayed.
        /// </summary>
        private Icon customFooterIcon;

        /// <summary>
        /// Specifies the custom push buttons to display in the dialog. Use CommonButtons member for 
        /// common buttons: OK, Yes, No, Retry, Close and Cancel. Use Buttons member when you want different text 
        /// on the push buttons.
        /// </summary>
        private TaskDialogButton[] buttons;

        /// <summary>
        /// Specifies the radio buttons to display in the dialog.
        /// </summary>
        private TaskDialogButton[] radioButtons;

        /// <summary>
        /// The flags passed to TaskDialogIndirect.
        /// </summary>
        private UnsafeNativeMethods.TASKDIALOG_FLAGS flags;

        /// <summary>
        /// Indicates the default button for the dialog. This may be any of the values specified
        /// in ButtonId members of one of the TaskDialogButton structures in the Buttons array,
        /// or one a DialogResult value that corresponds to a buttons specified in the CommonButtons Member.
        /// If this member is zero or its value does not correspond to any button ID in the dialog,
        /// then the first button in the dialog will be the default. 
        /// </summary>
        private int defaultButton;

        /// <summary>
        /// Indicates the default radio button for the dialog. This may be any of the values specified
        /// in ButtonId members of one of the TaskDialogButton structures in the RadioButtons array.
        /// If this member is zero or its value does not correspond to any radio button ID in the dialog,
        /// then the first button in RadioButtons will be the default.
        /// The property NoDefaultRadioButton can be set to have no default.
        /// </summary>
        private int defaultRadioButton;

        /// <summary>
        /// The string to be used to label the verification checkbox. If this member is null, the
        /// verification checkbox is not displayed in the dialog box.
        /// </summary>
        private string verificationText;

        /// <summary>
        /// The string to be used for displaying additional information. The additional information is
        /// displayed either immediately below the content or below the footer text depending on whether
        /// the ExpandFooterArea member is true. If the EnableHyperlinks member is true, then this string
        /// may contain hyperlinks in the form: <A HREF="executablestring">Hyperlink Text</A>.
        /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
        /// </summary>
        private string expandedInformation;

        /// <summary>
        /// The string to be used to label the button for collapsing the expanded information. This
        /// member is ignored when the ExpandedInformation member is null. If this member is null
        /// and the CollapsedControlText is specified, then the CollapsedControlText value will be
        /// used for this member as well.
        /// </summary>
        private string expandedControlText;

        /// <summary>
        /// The string to be used to label the button for expanding the expanded information. This
        /// member is ignored when the ExpandedInformation member is null.  If this member is null
        /// and the ExpandedControlText is specified, then the ExpandedControlText value will be
        /// used for this member as well.
        /// </summary>
        private string collapsedControlText;

        /// <summary>
        /// The string to be used in the footer area of the dialog box. If the EnableHyperlinks member
        /// is true, then this string may contain hyperlinks in the form: <A HREF="executablestring">
        /// Hyperlink Text</A>.
        /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
        /// </summary>
        private string footer;

        /// <summary>
        /// The callback that receives notifications from the Task Dialog when various events occur.
        /// </summary>
        //private TaskDialogCallback callback;

        /// <summary>
        /// Application specific data that is passed to the callback.
        /// </summary>
        private object callbackData;

        /// <summary>
        /// Specifies the width of the Task Dialog’s client area in DLU’s. If 0, Task Dialog will calculate the ideal width.
        /// </summary>
        private uint width;

        /// <summary>
        /// Indicates whether to show a non-Vista Task Dialog on Vista operating system. This should be used for testing purposes.
        /// </summary>
        private static bool showNonVistaTaskDialogOnVistaOS = false;
        
        
        /// <summary>
        /// Initializes a new instance of the TaskDialog class.
        /// </summary>
        public TaskDialog()
        {
            this.Reset();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show a non-Vista Task Dialog on Vista operating system. This should be used for testing purposes.
        /// </summary>
        public static bool ShowNonVistaTaskDialogOnVistaOS
        {
            get { return showNonVistaTaskDialogOnVistaOS; }
            set { showNonVistaTaskDialogOnVistaOS = value; }
        }
        
        /// <summary>
        /// Gets a value indicating whether the current operating system supports a native Task Dialog. 
        /// </summary>
        public static bool IsAvailableOnThisOS
        {
            get
            {
                OperatingSystem os = Environment.OSVersion;
                if (os.Platform != PlatformID.Win32NT)
                {
                    return false;
                }

                return (os.Version.CompareTo(TaskDialog.RequiredOSVersion) >= 0);
            }
        }

        /// <summary>
        /// Gets a value indicating the minimum Windows version needed to support a native Task Dialog.
        /// </summary>
        public static Version RequiredOSVersion
        {
            get { return new Version(6, 0, 5243); }
        }

        /// <summary>
        /// Gets or sets the dialog box title. If this property is null or empty string, the filename of the executable program is used.
        /// </summary>
        public string WindowTitle
        {
            get { return this.windowTitle; }
            set { this.windowTitle = value; }
        }

        /// <summary>
        /// Gets or sets the main instruction of the Task Dialog.
        /// </summary>
        public string MainInstruction
        {
            get { return this.mainInstruction; }
            set { this.mainInstruction = value; }
        }

        /// <summary>
        /// Gets or sets the content of the Task Dialog. If the EnableHyperlinks property is true, 
        /// then this string may contain hyperlinks in the form: &lt;A HREF="executablestring">Hyperlink Text&lt;/A>. 
        /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
        /// </summary>
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets the common push buttons of the Task Dialog. 
        /// This parameter may be a bitwise combination of the TaskDialogCommonButtons enumeration values. 
        /// If no common buttons are specified and no custom buttons are specified using the Buttons property, 
        /// the Task Dialog will contain the OK button by default.
        /// </summary>
        public TaskDialogCommonButtons CommonButtons
        {
            get { return this.commonButtons; }
            set { this.commonButtons = value; }
        }

        /// <summary>
        /// Gets or sets the built-in main icon for the Task Dialog. 
        /// If this is set to None and the CustomMainIcon property is null then no main icon will be displayed.
        /// </summary>
        public TaskDialogIcon MainIcon
        {
            get { return this.mainIcon; }
            set { this.mainIcon = value; }
        }

        /// <summary>
        /// Gets or sets the custom main icon for the Task Dialog. 
        /// If this is set to null and the MainIcon property is None then no main icon will be displayed.
        /// </summary>
        public Icon CustomMainIcon
        {
            get { return this.customMainIcon; }
            set { this.customMainIcon = value; }
        }

        /// <summary>
        /// Gets or sets the built-in footer icon for the Task Dialog. 
        /// If this is set to None and the CustomFooterIcon property is null then no footer icon 
        /// will be displayed. In order to display the footer icon, Footer must also be specified.
        /// </summary>
        public TaskDialogIcon FooterIcon
        {
            get { return this.footerIcon; }
            set { this.footerIcon = value; }
        }

        /// <summary>
        /// Gets or sets the custom footer icon for the Task Dialog. 
        /// If this is set to null and the FooterIcon property is None then no footer icon 
        /// will be displayed. In order to display the footer icon, Footer must also be specified.
        /// </summary>
        public Icon CustomFooterIcon
        {
            get { return this.customFooterIcon; }
            set { this.customFooterIcon = value; }
        }

        /// <summary>
        /// Gets or sets the custom push buttons of the Task Dialog. 
        /// If no custom buttons are specified and no common buttons are specified using 
        /// the CommonButtons property, the Task Dialog will contain the OK button by default.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
        public TaskDialogButton[] Buttons
        {
            get
            {
                return this.buttons;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.buttons = value;
            }
        }

        /// <summary>
        /// Gets or sets the radio buttons of the Task Dialog. 
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
        public TaskDialogButton[] RadioButtons
        {
            get
            {
                return this.radioButtons;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.radioButtons = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether hyperlink processing has been enabled for the strings 
        /// specified in the Content, ExpandedInformation and Footer properties. When enabled, 
        /// these properties may be strings that contain hyperlinks in the form: 
        /// &lt;A HREF="executablestring">Hyperlink Text&lt;/A>. 
        /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities. 
        /// Note: The Task Dialog will not actually execute any hyperlinks. Hyperlink execution must be handled 
        /// in the callback method specified by the Callback event.
        /// </summary>
        public bool EnableHyperlinks
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Task Dialog should be able to be closed using Alt-F4, 
        /// Escape and the title bar’s Close button even if no Cancel button is specified in either the CommonButtons or Buttons properties.
        /// </summary>
        public bool AllowDialogCancellation
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the buttons specified in the Buttons property should be 
        /// displayed as command links (using a standard Task Dialog glyph) instead of push buttons. When 
        /// using command links, all characters up to the first new line character in the ButtonText property 
        /// (of the TaskDialogButton structure) will be treated as the command link’s main text, and the 
        /// remainder will be treated as the command link’s note. This property is ignored if 
        /// the Buttons property has no entries.
        /// </summary>
        public bool UseCommandLinks
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the buttons specified in the Buttons property should be 
        /// displayed as command links (without a glyph) instead of push buttons. When 
        /// using command links, all characters up to the first new line character in the ButtonText property 
        /// (of the TaskDialogButton structure) will be treated as the command link’s main text, and the 
        /// remainder will be treated as the command link’s note. This property is ignored if 
        /// the Buttons property has no entries.
        /// </summary>
        public bool UseCommandLinksNoIcon
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the string specified in the ExpandedInformation property 
        /// should be displayed at the bottom of the footer area instead of immediately after the content. 
        /// This property is ignored if the ExpandedInformation property is null or empty string.
        /// </summary>
        public bool ExpandFooterArea
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the string specified in the ExpandedInformation property 
        /// should be displayed when the Task Dialog is initially displayed. This property is ignored if 
        /// the ExpandedInformation property is null or empty string.
        /// </summary>
        public bool ExpandedByDefault
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the verification checkbox of the Task Dialog should be 
        /// checked when the Task Dialog is initially displayed. This property is ignored if 
        /// the VerificationText property is null or empty string.
        /// </summary>
        public bool VerificationFlagChecked
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a progress bar should be displayed. 
        /// Setting the progress bar range and position must be done in the callback method specified by 
        /// the Callback event using ActiveTaskDialog.SetProgressBarRange(short, short) and 
        /// ActiveTaskDialog.SetProgressBarPosition(int) methods.
        /// </summary>
        public bool ShowProgressBar
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a marquee style progress bar should be displayed. 
        /// Setting the progress bar block speed must be done in the callback method specified by 
        /// the Callback event using ActiveTaskDialog.SetProgressBarMarquee(bool, uint) method.
        /// </summary>
        public bool ShowMarqueeProgressBar
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Task Dialog’s Callback event should be called 
        /// approximately every 200 milliseconds.
        /// </summary>
        public bool CallbackTimer
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Task Dialog should be positioned (centered) relative 
        /// to the owner window passed when calling the Show method. If not set (or no owner window is passed), 
        /// the Task Dialog is positioned (centered) relative to the screen.
        /// </summary>
        public bool PositionRelativeToWindow
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Task Dialog text is displayed reading right to left.
        /// </summary>
        public bool RightToLeftLayout
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Task Dialog should have no default radio button.
        /// </summary>
        public bool NoDefaultRadioButton
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Task Dialog can be minimized. Works only when 
        /// owner window passed to the Show method is null or IntPtr.Zero.
        /// </summary>
        public bool CanBeMinimized
        {
            get { return (this.flags & UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED) != 0; }
            set { this.SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED, value); }
        }

        /// <summary>
        /// Gets or sets the default button for the Task Dialog. This may be any of the values specified 
        /// in ButtonId properties of one of the TaskDialogButton structures in the Buttons property, 
        /// or one of the TaskDialogCommonButtons enumeration values specified in the CommonButtons property. 
        /// If this property is zero or its value does not correspond to any button ID in the Task Dialog, 
        /// then the first button in the Task Dialog will be the default.
        /// </summary>
        public int DefaultButton
        {
            get { return this.defaultButton; }
            set { this.defaultButton = value; }
        }

        /// <summary>
        /// Gets or sets the default radio button for the Task Dialog. This may be any of the values specified 
        /// in ButtonId properties of one of the TaskDialogButton structures in the RadioButtons property. 
        /// If this property is zero or its value does not correspond to any radio button ID in the Task Dialog, 
        /// then the first button in the RadioButtons property will be the default. 
        /// The NoDefaultRadioButton property can be set to have no default radio button.
        /// </summary>
        public int DefaultRadioButton
        {
            get { return this.defaultRadioButton; }
            set { this.defaultRadioButton = value; }
        }

        /// <summary>
        /// Gets or sets the verification checkbox text of the Task Dialog. 
        /// If this property is null or empty string, the verification checkbox is not displayed in the Task Dialog.
        /// </summary>
        public string VerificationText
        {
            get { return this.verificationText; }
            set { this.verificationText = value; }
        }

        /// <summary>
        /// Gets or sets the expanded (additional) information of the Task Dialog. The additional information is 
        /// displayed either immediately below the content or below the footer text depending on whether 
        /// the ExpandFooterArea property is true. If the EnableHyperlinks property is true, 
        /// then this string may contain hyperlinks in the form: &lt;A HREF="executablestring">Hyperlink Text&lt;/A>. 
        /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
        /// </summary>
        public string ExpandedInformation
        {
            get { return this.expandedInformation; }
            set { this.expandedInformation = value; }
        }

        /// <summary>
        /// Gets or sets the text to be used to label the button for collapsing the expandable information. 
        /// This property is ignored if the ExpandedInformation property is null or empty string. 
        /// If this property is null or empty string and the CollapsedControlText is specified, 
        /// then the CollapsedControlText property value will be used for this property as well.
        /// </summary>
        public string ExpandedControlText
        {
            get { return this.expandedControlText; }
            set { this.expandedControlText = value; }
        }

        /// <summary>
        /// Gets or sets the text to be used to label the button for expanding the expandable information. 
        /// This property is ignored if the ExpandedInformation property is null or empty string. 
        /// If this property is null or empty string and the ExpandedControlText is specified, 
        /// then the ExpandedControlText property value will be used for this property as well.
        /// </summary>
        public string CollapsedControlText
        {
            get { return this.collapsedControlText; }
            set { this.collapsedControlText = value; }
        }

        /// <summary>
        /// Gets or sets the footer of the Task Dialog. If the EnableHyperlinks property is true, 
        /// then this string may contain hyperlinks in the form: &lt;A HREF="executablestring">Hyperlink Text&lt;/A>. 
        /// WARNING: Enabling hyperlinks when using content from an unsafe source may cause security vulnerabilities.
        /// </summary>
        public string Footer
        {
            get { return this.footer; }
            set { this.footer = value; }
        }

        /// <summary>
        /// Gets or sets the width of the Task Dialog's client area in DLU's. If 0, the Task Dialog will 
        /// calculate the ideal width.
        /// </summary>
        public uint Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        /// <summary>
        /// Occurs when various notifications are passed from the Task Dialog.
        /// </summary>
        public event TaskDialogCallbackEventHandler Callback;

        /// <summary>
        /// Returns true if there is at least one event hanlder defined for the Callback event.
        /// </summary>
        internal bool CallbackEventHandlerExists
        {
            get { return this.Callback != null; }
        }

        /// <summary>
        /// Raises Callback event.
        /// </summary>
        /// <param name="args"></param>
        internal void OnCallback(TaskDialogCallbackEventArgs args)
        {
            TaskDialogCallbackEventHandler callbackHandler = this.Callback;
            if (callbackHandler != null)
            {
                callbackHandler(this, args);
            }
        }

        /// <summary>
        /// Gets or sets the application specific data that is passed to the Task Dialog’s Callback event handler.
        /// </summary>
        public object CallbackData
        {
            get { return this.callbackData; }
            set { this.callbackData = value; }
        }

        /// <summary>
        /// Resets the Task Dialog to the state when first constructed, all properties are set to their default values.
        /// </summary>
        public void Reset()
        {
            this.windowTitle = null;
            this.mainInstruction = null;
            this.content = null;
            this.commonButtons = 0;
            this.mainIcon = TaskDialogIcon.None;
            this.customMainIcon = null;
            this.footerIcon = TaskDialogIcon.None;
            this.customFooterIcon = null;
            this.buttons = new TaskDialogButton[0];
            this.radioButtons = new TaskDialogButton[0];
            this.flags = 0;
            this.defaultButton = 0;
            this.defaultRadioButton = 0;
            this.verificationText = null;
            this.expandedInformation = null;
            this.expandedControlText = null;
            this.collapsedControlText = null;
            this.footer = null;
            this.Callback = null;
            this.callbackData = null;
            this.width = 0;
        }
        
        /// <summary>
        /// Displays the Task Dialog.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <returns>The result of the Task Dialog, either a TaskDialogCommonButtons enumeration value for 
        /// common push buttons set in the CommonButtons property or the ButtonId property of one of 
        /// the TaskDialogButton structures in the Buttons property.</returns>
        public int Show(IWin32Window owner)
        {
            bool verificationFlagChecked;
            int radioButtonResult;
            return this.Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
        }

        /// <summary>
        /// Displays the Task Dialog.
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window.</param>
        /// <returns>The result of the Task Dialog, either a TaskDialogCommonButtons enumeration value for 
        /// common push buttons set in the CommonButtons property or the ButtonId property of one of 
        /// the TaskDialogButton structures in the Buttons property.</returns>
        public int Show(IntPtr hwndOwner)
        {
            bool verificationFlagChecked;
            int radioButtonResult;
            return this.Show(hwndOwner, out verificationFlagChecked, out radioButtonResult);
        }
        
        /// <summary>
        /// Displays the Task Dialog.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the Task Dialog 
        /// was dismissed, otherwise false is returned.</param>
        /// <returns>The result of the Task Dialog, either a TaskDialogCommonButtons enumeration value for 
        /// common push buttons set in the CommonButtons property or the ButtonId property of one of 
        /// the TaskDialogButton structures in the Buttons property.</returns>
        public int Show(IWin32Window owner, out bool verificationFlagChecked)
        {
            int radioButtonResult;
            return this.Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
        }

        /// <summary>
        /// Displays the Task Dialog.
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window.</param>
        /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the Task Dialog 
        /// was dismissed, otherwise false is returned.</param>
        /// <returns>The result of the Task Dialog, either a TaskDialogCommonButtons enumeration value for 
        /// common push buttons set in the CommonButtons property or the ButtonId property of one of 
        /// the TaskDialogButton structures in the Buttons property.</returns>
        public int Show(IntPtr hwndOwner, out bool verificationFlagChecked)
        {
            // We have to call a private version or PreSharp gets upset about a unsafe
            // block in a public method. (PreSharp error 56505)
            int radioButtonResult;
            return this.PrivateShow(hwndOwner, out verificationFlagChecked, out radioButtonResult);
        }
        
        /// <summary>
        /// Displays the Task Dialog.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the Task Dialog 
        /// was dismissed, otherwise false is returned.</param>
        /// <param name="radioButtonResult">Returns the radio button selected by the user. This may be any of the values specified 
        /// in ButtonId properties of one of the TaskDialogButton structures in the RadioButtons property.</param>
        /// <returns>The result of the Task Dialog, either a TaskDialogCommonButtons enumeration value for 
        /// common push buttons set in the CommonButtons property or the ButtonId property of one of 
        /// the TaskDialogButton structures in the Buttons property.</returns>
        public int Show(IWin32Window owner, out bool verificationFlagChecked, out int radioButtonResult)
        {
            return this.Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
        }

        /// <summary>
        /// Displays the Task Dialog.
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window.</param>
        /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the Task Dialog 
        /// was dismissed, otherwise false is returned.</param>
        /// <param name="radioButtonResult">Returns the radio button selected by the user. This may be any of the values specified 
        /// in ButtonId properties of one of the TaskDialogButton structures in the RadioButtons property.</param>
        /// <returns>The result of the Task Dialog, either a TaskDialogCommonButtons enumeration value for 
        /// common push buttons set in the CommonButtons property or the ButtonId property of one of 
        /// the TaskDialogButton structures in the Buttons property.</returns>
        public int Show(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
        {
            // We have to call a private version or PreSharp gets upset about a unsafe
            // block in a public method. (PreSharp error 56505)
            return this.PrivateShow(hwndOwner, out verificationFlagChecked, out radioButtonResult);
        }

        /// <summary>
        /// Creates, displays, and operates a task dialog. The task dialog contains application-defined messages, title,
        /// verification check box, command links and push buttons, plus any combination of predefined icons and push buttons
        /// as specified on the other members of the class before calling Show.
        /// </summary>
        /// <param name="hwndOwner">Owner window the task Dialog will modal to.</param>
        /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked when the dialog
        /// was dismissed.</param>
        /// <param name="radioButtonResult">The radio botton selected by the user.</param>
        /// <returns>The result of the dialog, either a DialogResult value for common push buttons set in the CommonButtons
        /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
        private int PrivateShow(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
        {
            if (IsAvailableOnThisOS && !showNonVistaTaskDialogOnVistaOS)
            {
                verificationFlagChecked = false;
                radioButtonResult = 0;
                int result = 0;

                using (TaskDialog.TaskDialogConfig config = new TaskDialog.TaskDialogConfig(this, hwndOwner))
                {
                    UnsafeNativeMethods.TASKDIALOGCONFIG nativeConfig = config.Config;
                    // The call all this mucking about is here for.
                    UnsafeNativeMethods.TaskDialogIndirect(ref nativeConfig, out result, out radioButtonResult, out verificationFlagChecked);

                    switch (result)
                    {
                        case (int)DialogResult.OK:
                            result = (int)TaskDialogCommonButtons.Ok;
                            break;
                        case (int)DialogResult.Yes:
                            result = (int)TaskDialogCommonButtons.Yes;
                            break;
                        case (int)DialogResult.No:
                            result = (int)TaskDialogCommonButtons.No;
                            break;
                        case (int)DialogResult.Cancel:
                            result = (int)TaskDialogCommonButtons.Cancel;
                            break;
                        case (int)DialogResult.Retry:
                            result = (int)TaskDialogCommonButtons.Retry;
                            break;
                        case 8:
                            result = (int)TaskDialogCommonButtons.Close;
                            break;
                    }
                    return result;
                }
            }
            else
            {
                FormTaskDialog frmTaskDlg = new FormTaskDialog(hwndOwner, this);
                if (hwndOwner == IntPtr.Zero)
                {
                    frmTaskDlg.ShowInTaskbar = true;
                    return frmTaskDlg.ShowTaskDialog(false, out verificationFlagChecked, out radioButtonResult);
                }
                else
                {
                    frmTaskDlg.ShowInTaskbar = false;
                    return frmTaskDlg.ShowTaskDialog(true, out verificationFlagChecked, out radioButtonResult);
                }
            }
        }

        /// <summary>
        /// The callback from the native Task Dialog. This prepares the friendlier arguments and calls the simplier callback.
        /// </summary>
        /// <param name="hwnd">The window handle of the Task Dialog that is active.</param>
        /// <param name="uNotification">The notification. A TaskDialogNotification value.</param>
        /// <param name="wparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
        /// <param name="lparam">Specifies additional noitification information.  The contents of this parameter depends on the value of the msg parameter.</param>
        /// <param name="refData">Specifies the application-defined value given in the call to TaskDialogIndirect.</param>
        /// <returns>A HRESULT. It's not clear in the spec what a failed result will do.</returns>
        private int PrivateCallback([In] IntPtr hwnd, [In] uint uNotification, [In] UIntPtr wparam, [In] IntPtr lparam, [In] IntPtr refData)
        {
            TaskDialogCallbackEventHandler callbackHandler = this.Callback;
            if (callbackHandler != null)
            {
                // Prepare arguments for the callback to the user we are insulating from Interop casting sillyness.

                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(hwnd);
                args.CallbackData = this.callbackData;
                args.Notification = (TaskDialogNotification)uNotification;
                switch (args.Notification)
                {
                    case TaskDialogNotification.ButtonClicked:
                        switch ((int)wparam)
                        {
                            case (int)DialogResult.OK:
                                args.ButtonId = (int)TaskDialogCommonButtons.Ok;
                                break;
                            case (int)DialogResult.Yes:
                                args.ButtonId = (int)TaskDialogCommonButtons.Yes;
                                break;
                            case (int)DialogResult.No:
                                args.ButtonId = (int)TaskDialogCommonButtons.No;
                                break;
                            case (int)DialogResult.Cancel:
                                args.ButtonId = (int)TaskDialogCommonButtons.Cancel;
                                break;
                            case (int)DialogResult.Retry:
                                args.ButtonId = (int)TaskDialogCommonButtons.Retry;
                                break;
                            case 8:
                                args.ButtonId = (int)TaskDialogCommonButtons.Close;
                                break;
                            default:
                                args.ButtonId = (int)wparam;
                                break;
                        }
                        args.CloseTaskDialog = true;
                        break;
                    case TaskDialogNotification.RadioButtonClicked:
                        args.ButtonId = (int)wparam;
                        break;
                    case TaskDialogNotification.HyperlinkClicked:
                        args.Hyperlink = Marshal.PtrToStringUni(lparam);
                        break;
                    case TaskDialogNotification.Timer:
                        args.TimerTickCount = (uint)wparam;
                        args.ResetTimerTickCount = false;
                        break;
                    case TaskDialogNotification.VerificationClicked:
                        args.VerificationFlagChecked = (wparam != UIntPtr.Zero);
                        break;
                    case TaskDialogNotification.ExpandoButtonClicked:
                        args.Expanded = (wparam != UIntPtr.Zero);
                        break;
                }

                callbackHandler(this, args);

                if (args.Notification == TaskDialogNotification.ButtonClicked && !args.CloseTaskDialog) return (1);  // true
                if (args.Notification == TaskDialogNotification.Timer && args.ResetTimerTickCount) return (1);  // true
            }

            return 0; // false;
        }
        
        /// <summary>
        /// Helper function to set or clear a bit in the flags field.
        /// </summary>
        /// <param name="flag">The Flag bit to set or clear.</param>
        /// <param name="value">True to set, false to clear the bit in the flags field.</param>
        private void SetFlag(UnsafeNativeMethods.TASKDIALOG_FLAGS flag, bool value)
        {
            if (value)
            {
                this.flags |= flag;
            }
            else
            {
                this.flags &= ~flag;
            }
        }


        /// <summary>
        /// Represents the class for creating and disposing 'TASKDIALOGCONFIG' structure.
        /// </summary>
        internal sealed class TaskDialogConfig : IDisposable
        {
            UnsafeNativeMethods.TASKDIALOGCONFIG config;
            private bool disposed = false;


            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="taskDialog"></param>
            /// <param name="hwndOwner"></param>
            public TaskDialogConfig(TaskDialog taskDialog, IntPtr hwndOwner)
            {
                config = new UnsafeNativeMethods.TASKDIALOGCONFIG();

                config.cbSize = (uint)Marshal.SizeOf(typeof(UnsafeNativeMethods.TASKDIALOGCONFIG));
                config.hwndParent = hwndOwner;
                config.dwFlags = taskDialog.flags;
                config.dwCommonButtons = taskDialog.commonButtons;

                if (!string.IsNullOrEmpty(taskDialog.windowTitle))
                {
                    config.pszWindowTitle = taskDialog.windowTitle;
                }

                config.MainIcon = (IntPtr)taskDialog.mainIcon;
                if (taskDialog.customMainIcon != null)
                {
                    config.dwFlags |= UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_MAIN;
                    config.MainIcon = taskDialog.customMainIcon.Handle;
                }

                if (!string.IsNullOrEmpty(taskDialog.mainInstruction))
                {
                    config.pszMainInstruction = taskDialog.mainInstruction;
                }

                if (!string.IsNullOrEmpty(taskDialog.content))
                {
                    config.pszContent = taskDialog.content;
                }

                TaskDialogButton[] customButtons = taskDialog.buttons;
                if (customButtons.Length > 0)
                {
                    // Hand marshal the buttons array.
                    int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                    config.pButtons = Marshal.AllocHGlobal(elementSize * (int)customButtons.Length);
                    for (int i = 0; i < customButtons.Length; i++)
                    {
                        unsafe // Unsafe because of pointer arithmatic.
                        {
                            byte* p = (byte*)config.pButtons;
                            Marshal.StructureToPtr(customButtons[i], (IntPtr)(p + (elementSize * i)), false);
                        }

                        config.cButtons++;
                    }
                }

                TaskDialogButton[] customRadioButtons = taskDialog.radioButtons;
                if (customRadioButtons.Length > 0)
                {
                    // Hand marshal the buttons array.
                    int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                    config.pRadioButtons = Marshal.AllocHGlobal(elementSize * (int)customRadioButtons.Length);
                    for (int i = 0; i < customRadioButtons.Length; i++)
                    {
                        unsafe // Unsafe because of pointer arithmatic.
                        {
                            byte* p = (byte*)config.pRadioButtons;
                            Marshal.StructureToPtr(customRadioButtons[i], (IntPtr)(p + (elementSize * i)), false);
                        }

                        config.cRadioButtons++;
                    }
                }

                switch (taskDialog.defaultButton)
                {
                    case (int)TaskDialogCommonButtons.Ok:
                        config.nDefaultButton = (int)DialogResult.OK;
                        break;
                    case (int)TaskDialogCommonButtons.Yes:
                        config.nDefaultButton = (int)DialogResult.Yes;
                        break;
                    case (int)TaskDialogCommonButtons.No:
                        config.nDefaultButton = (int)DialogResult.No;
                        break;
                    case (int)TaskDialogCommonButtons.Cancel:
                        config.nDefaultButton = (int)DialogResult.Cancel;
                        break;
                    case (int)TaskDialogCommonButtons.Retry:
                        config.nDefaultButton = (int)DialogResult.Retry;
                        break;
                    case (int)TaskDialogCommonButtons.Close:
                        config.nDefaultButton = 8;
                        break;
                    default:
                        config.nDefaultButton = taskDialog.defaultButton;
                        break;
                }
                config.nDefaultRadioButton = taskDialog.defaultRadioButton;

                if (!string.IsNullOrEmpty(taskDialog.verificationText))
                {
                    config.pszVerificationText = taskDialog.verificationText;
                }

                if (!string.IsNullOrEmpty(taskDialog.expandedInformation))
                {
                    config.pszExpandedInformation = taskDialog.expandedInformation;
                }

                if (!string.IsNullOrEmpty(taskDialog.expandedControlText))
                {
                    config.pszExpandedControlText = taskDialog.expandedControlText;
                }

                if (!string.IsNullOrEmpty(taskDialog.collapsedControlText))
                {
                    config.pszCollapsedControlText = taskDialog.CollapsedControlText;
                }

                config.FooterIcon = (IntPtr)taskDialog.footerIcon;
                if (taskDialog.customFooterIcon != null)
                {
                    config.dwFlags |= UnsafeNativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_FOOTER;
                    config.FooterIcon = taskDialog.customFooterIcon.Handle;
                }

                if (!string.IsNullOrEmpty(taskDialog.footer))
                {
                    config.pszFooter = taskDialog.footer;
                }

                // If our user has asked for a callback then we need to ask for one to
                // translate to the friendly version.
                if (taskDialog.Callback != null)
                {
                    config.pfCallback = new UnsafeNativeMethods.TaskDialogCallback(taskDialog.PrivateCallback);
                }

                ////config.lpCallbackData = this.callbackData; // How do you do this? Need to pin the ref?
                config.cxWidth = taskDialog.width;
            }

            /// <summary>
            /// Gets the 'TASKDIALOGCONFIG' structure.
            /// </summary>
            public UnsafeNativeMethods.TASKDIALOGCONFIG Config
            {
                get { return config; }
                //private set { cnfg = value; }
            }

            /// <summary>
            /// Release allocated unmanaged resources.
            /// </summary>
            /// <param name="disposing"></param>
            private void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    // Free the unmanaged memory needed for the button arrays.
                    // There is the possiblity of leaking memory if the app-domain is destroyed in a non clean way
                    // and the hosting OS process is kept alive but fixing this would require using hardening techniques
                    // that are not required for the users of this class.
                    if (this.Config.pButtons != IntPtr.Zero)
                    {
                        int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                        for (int i = 0; i < this.Config.cButtons; i++)
                        {
                            unsafe
                            {
                                byte* p = (byte*)this.Config.pButtons;
                                Marshal.DestroyStructure((IntPtr)(p + (elementSize * i)), typeof(TaskDialogButton));
                            }
                        }

                        Marshal.FreeHGlobal(this.Config.pButtons);
                    }

                    if (this.Config.pRadioButtons != IntPtr.Zero)
                    {
                        int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                        for (int i = 0; i < this.Config.cRadioButtons; i++)
                        {
                            unsafe
                            {
                                byte* p = (byte*)this.Config.pRadioButtons;
                                Marshal.DestroyStructure((IntPtr)(p + (elementSize * i)), typeof(TaskDialogButton));
                            }
                        }

                        Marshal.FreeHGlobal(this.Config.pRadioButtons);
                    }

                    this.disposed = true;
                }
            }

            /// <summary>
            /// Explicitly release unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// C# destructor for finalization code.
            /// </summary>
            ~TaskDialogConfig()
            {
                this.Dispose(false);
            }
        }
    }
}

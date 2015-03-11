
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

namespace WindowsTaskDialog
{
    /// <summary>
    /// Provides data for the Callback event.
    /// </summary>
    public class TaskDialogCallbackEventArgs : System.EventArgs
    {
        /// <summary>
        /// What the TaskDialog callback is a notification of.
        /// </summary>
        private TaskDialogNotification notification;

        /// <summary>
        /// The button ID if the notification is about a button. This a DialogResult
        /// value or the ButtonID member of a TaskDialogButton set in the
        /// TaskDialog.Buttons or TaskDialog.RadioButtons members.
        /// </summary>
        private int buttonId;

        /// <summary>
        /// The HREF string of the hyperlink the notification is about.
        /// </summary>
        private string hyperlink;

        /// <summary>
        /// The number of milliseconds since the dialog was opened or the last time the
        /// callback for a timer notification reset the value by returning true.
        /// </summary>
        private uint timerTickCount;

        /// <summary>
        /// The state of the verification flag when the notification is about the verification flag.
        /// </summary>
        private bool verificationFlagChecked;

        /// <summary>
        /// The state of the dialog expando when the notification is about the expando.
        /// </summary>
        private bool expanded;

        /// <summary>
        /// Provides methods for updating information displayed in the Task Dialog.
        /// </summary>
        private ActiveTaskDialog activeDialog;
        
        /// <summary>
        /// The application specific data set on the TaskDialog.CallbackData property.
        /// </summary>
        private object callbackData;
        
        /// <summary>
        /// The value indicating whether to close the Task Dialog when the Notification property 
        /// is TaskDialogNotification.ButtonClicked.
        /// </summary>
        private bool closeTaskDialog;

        /// <summary>
        /// The value indicating whether to reset the tickcount when the Notification property 
        /// is TaskDialogNotification.Timer.
        /// </summary>
        private bool resetTimerTickCount;

        /// <summary>
        /// Initializes a new instance of the TaskDialogCallbackEventArgs class.
        /// </summary>
        public TaskDialogCallbackEventArgs()
        {
        }
        
        /// <summary>
        /// Gets or sets the notification for the Task Dialog’s Callback event.
        /// </summary>
        public TaskDialogNotification Notification
        {
            get { return this.notification; }
            set { this.notification = value; }
        }

        /// <summary>
        /// Gets or sets the button ID corresponding to the button or radio button selected in 
        /// the Task Dialog when the Notification property is either TaskDialogNotification.ButtonClicked or 
        /// TaskDialogNotification.RadioButtonClicked.
        /// </summary>
        public int ButtonId
        {
            get { return this.buttonId; }
            set { this.buttonId = value; }
        }

        /// <summary>
        /// Gets or sets the HREF of the hyperlink clicked in the Task Dialog when the Notification property 
        /// is TaskDialogNotification.HyperlinkClicked.
        /// </summary>
        public string Hyperlink
        {
            get { return this.hyperlink; }
            set { this.hyperlink = value; }
        }

        /// <summary>
        /// Gets or sets the number of milliseconds since the Task Dialog was created or the 
        /// ResetTimerTickCount property was set to true when the Notification property is 
        /// TaskDialogNotification.Timer.
        /// </summary>
        public uint TimerTickCount
        {
            get { return this.timerTickCount; }
            set { this.timerTickCount = value; }
        }

        /// <summary>
        /// Gets or sets the state of the verification checkbox of the Task Dialog when the Notification 
        /// property is TaskDialogNotification.VerificationClicked.  
        /// </summary>
        public bool VerificationFlagChecked
        {
            get { return this.verificationFlagChecked; }
            set { this.verificationFlagChecked = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the expanded (additional) information of the Task Dialog 
        /// is visible when the Notification property is TaskDialogNotification.ExpandoButtonClicked.  
        /// </summary>
        public bool Expanded
        {
            get { return this.expanded; }
            set { this.expanded = value; }
        }

        /// <summary>
        /// Gets or sets the active Task Dialog window. Provides methods for updating information 
        /// displayed in the Task Dialog.
        /// </summary>
        public ActiveTaskDialog ActiveDialog
        {
            get { return this.activeDialog; }
            set { this.activeDialog = value; }
        }
        
        /// <summary>
        /// Gets or sets the application specific data set on the TaskDialog.CallbackData property.
        /// </summary>
        public object CallbackData
        {
            get { return this.callbackData; }
            set { this.callbackData = value; }
        }
        
        /// <summary>
        /// Gets or sets the value indicating whether to close the Task Dialog when the Notification property 
        /// is TaskDialogNotification.ButtonClicked.
        /// </summary>
        public bool CloseTaskDialog
        {
            get { return this.closeTaskDialog; }
            set { this.closeTaskDialog = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to reset the tickcount when the Notification property 
        /// is TaskDialogNotification.Timer.
        /// </summary>
        public bool ResetTimerTickCount
        {
            get { return this.resetTimerTickCount; }
            set { this.resetTimerTickCount = value; }
        }
    }
}

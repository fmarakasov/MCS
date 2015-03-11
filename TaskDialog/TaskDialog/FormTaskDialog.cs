
//**********************************************//
//                                              //
//     Copyright (c) 2008 Wintaskdialog.com     //
//                                              //
//**********************************************//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;

namespace WindowsTaskDialog
{
    public partial class FormTaskDialog : Form
    {
        private static Icon iconMainInformation = new Icon(Properties.Resources.IconInformation, new Size(32, 32));
        private static Icon iconMainWarning = new Icon(Properties.Resources.IconWarning, new Size(32, 32));
        private static Icon iconMainError = new Icon(Properties.Resources.IconError, new Size(32, 32));
        private static Icon iconMainShield = new Icon(Properties.Resources.IconShield, new Size(32, 32));
        private static Icon iconFooterInformation = new Icon(Properties.Resources.IconInformation, new Size(16, 16));
        private static Icon iconFooterWarning = new Icon(Properties.Resources.IconWarning, new Size(16, 16));
        private static Icon iconFooterError = new Icon(Properties.Resources.IconError, new Size(16, 16));
        private static Icon iconFooterShield = new Icon(Properties.Resources.IconShield, new Size(16, 16));

        private ChevronButton chvbExpand = null;
        private TaskDialogCheckBox chbVerify = null;

        private string expandedControlText = "Hide &details";
        private string collapsedControlText = "See &details";

        bool mainIconVisible                  = false;
        bool mainInstructionVisible           = false;
        bool contentVisible                   = false;
        bool expandedInformationVisible       = false;
        bool progressBarVisible               = false;
        bool radioButtonsVisible              = false;
        bool commandLinksVisible              = false;
        bool expandVerifyVisible              = false;
        bool buttonsVisible                   = false;
        bool footerIconVisible                = false;
        bool footerVisible                    = false;        
        bool footerExpandedInformationVisible = false;

        private IntPtr hwndOwner = IntPtr.Zero;
        private int minimumWidth;
        private int maximumWidth;
        private DateTime tmStartTime;
        private Size mainIconSize;
        private Padding mainIconMargin;
        private Size footerIconSize;
        private Padding footerIconMargin;
        private TaskDialogRadioButton defaultRadioButton = null;
        
        private TaskDialog taskDialogParams;

        private bool dialogConstructed = false;
        private int dialogResultButtonId = -1;
        


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="taskDialogParams"></param>
        public FormTaskDialog(IntPtr hwndOwner, TaskDialog taskDialogParams)
        {
            InitializeComponent();
            this.hwndOwner = hwndOwner;

            //
            // Show icon if dialog doesn't have owner window.
            //
            if (hwndOwner == IntPtr.Zero) ShowIcon = true;

            //
            // Store original size and margin for main and footer icons.
            //
            mainIconSize = lblMainIcon.Size;
            mainIconMargin = lblMainIcon.Margin;
            footerIconSize = lblFooterIcon.Size;
            footerIconMargin = lblFooterIcon.Margin;

            //
            // Initial dialog positioning.
            //
            Control ownerWnd = null;
            if (taskDialogParams.PositionRelativeToWindow && hwndOwner != IntPtr.Zero)
            {
                ownerWnd = Control.FromHandle(hwndOwner);
                if (ownerWnd != null)
                {
                    //
                    // Owner window center.
                    //
                    StartPosition = FormStartPosition.Manual;
                }
                else
                {
                    //
                    // Screen center.
                    //
                    StartPosition = FormStartPosition.CenterScreen;
                }
            }
            else
            {
                //
                // Screen center.
                //
                StartPosition = FormStartPosition.CenterScreen;
            }

            //
            // Plays system sound according to main icon.
            //
            if (taskDialogParams.CustomMainIcon == null)
            {
                switch (taskDialogParams.MainIcon)
                {
                    case TaskDialogIcon.Information:
                        SystemSounds.Asterisk.Play();
                        break;

                    case TaskDialogIcon.Warning:
                        SystemSounds.Exclamation.Play();
                        break;

                    case TaskDialogIcon.Error:
                        SystemSounds.Hand.Play();
                        break;
                }
            }
            
            //
            // Constructs task dialog according to 'taskDialogParams' parameter.
            // (And sets 'this.taskDialogParams' to 'taskDialogParams'.)
            //
            ConstructTaskDialog(taskDialogParams);

            //
            // Final dialog positioning.
            //
            if (ownerWnd != null)
            {
                Left = ownerWnd.Left + ((ownerWnd.Width - Width) / 2);
                Top = ownerWnd.Top + ((ownerWnd.Height - Height) / 2);
                if (Left < ownerWnd.Left + 10) Left = ownerWnd.Left + 10;
                if (Top < ownerWnd.Top + 10) Top = ownerWnd.Top + 10;
            }

            //
            // 'TaskDialogNotification.Created' notification.
            //
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.Created;
                taskDialogParams.OnCallback(args);
            }

            //
            // Check default radio button.
            //
            if (defaultRadioButton != null)
            {
                defaultRadioButton.Checked = true;
                radioButton_Click(defaultRadioButton, null);
            }
        }

        #region private void ConstructTaskDialog(TaskDialog taskDialogParams)
        /// <summary>
        /// Constructs task dialog according to 'taskDialogParams' parameter.
        /// </summary>
        /// <param name="taskDialogParams"></param>
        private void ConstructTaskDialog(TaskDialog taskDialogParams)
        {
            this.taskDialogParams = taskDialogParams;

            //
            // Dialog is under construction.
            //
            dialogConstructed = false;
            
            //
            // Right to left layout.
            //
            RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
            RightToLeftLayout = taskDialogParams.RightToLeftLayout;

            //
            // Window title.
            //
            if (String.IsNullOrEmpty(taskDialogParams.WindowTitle)) Text = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            else Text = taskDialogParams.WindowTitle;

            //
            // Window Minimize and Cancel buttons.
            // (Must be put in constructor to avoid flickering ?!)
            //
            MinimizeBox = taskDialogParams.CanBeMinimized && this.hwndOwner == IntPtr.Zero;
            ControlBox = taskDialogParams.AllowDialogCancellation ||
                         (((int)taskDialogParams.CommonButtons & (int)TaskDialogCommonButtons.Cancel) > 0) ||
                         MinimizeBox;

            //
            // Set minimum and maximum width for task dialog client area.
            //
            TEXTMETRIC tm = GetTextMetrics(CreateGraphics(), Font);
            if (taskDialogParams.Width > 0)
            {
                //
                // 'taskDialogParams.Width' contains width of the Task Dialog's client area in DLU's.
                // 'taskDialogParams.Width' needs to be converted to pixels to set 'minimumWidth' and 'maximumWidth'.
                // A horizontal DLU is the average width of the dialog box font divided by 4.
                // (Experimenting with task dialog shows that for horizontal DLU average width is divided by 3 !!)
                // A vertical DLU is the average height of the font divided by 8.
                //
                minimumWidth = (int)(((double)tm.tmAveCharWidth / 3) * taskDialogParams.Width);
                maximumWidth = minimumWidth;
            }
            else
            {
                //minimumWidth = 360;  // For Tahoma, 8,25pt font.

                //
                // Minimum width of the Task Dialog's client area in DLU's (216) which 
                // needs to be converted to pixels to set 'minimumWidth' and 'maximumWidth'.
                // A horizontal DLU is the average width of the dialog box font divided by 4.
                // (Experimenting with task dialog shows that for horizontal DLU average width is divided by 3 !!)
                // A vertical DLU is the average height of the font divided by 8.
                //
                minimumWidth = (int)(((double)tm.tmAveCharWidth / 3) * 216);
                maximumWidth = Screen.PrimaryScreen.Bounds.Width - 8;
                if (maximumWidth < minimumWidth) maximumWidth = minimumWidth;
            }

            //
            // Set maximum width for 'pnlExpandVerify' that contains button for 
            // expanding/collapsing additional information and verification control.
            // (183 DLU's --> 305 pixels for Tahoma, 8,25pt font)
            //
            pnlExpandVerify.MaximumSize = new Size((int)(((double)tm.tmAveCharWidth / 3) * 183), Int32.MaxValue);

            //
            // Sets title bar icon according to main icon.
            //
            if (taskDialogParams.CustomMainIcon == null)
            {
                switch (taskDialogParams.MainIcon)
                {
                    case TaskDialogIcon.None:
                        Icon = Properties.Resources.IconTaskDialog;
                        break;

                    case TaskDialogIcon.Information:
                        Icon = iconFooterInformation;
                        break;

                    case TaskDialogIcon.Warning:
                        Icon = iconFooterWarning;
                        break;

                    case TaskDialogIcon.Error:
                        Icon = iconFooterError;
                        break;

                    case TaskDialogIcon.Shield:
                        Icon = iconFooterShield;
                        break;
                }
            }
            else
            {
                Icon = CreateIconStretch(taskDialogParams.CustomMainIcon, new Size(16, 16));
            }

            //
            // Set dialog control values.
            //
            SetControlValues();

            //
            // Arrange dialog controls and set dialog size.
            //
            ArrangeControls(minimumWidth);

            //
            // Dialog is constructed.
            //
            dialogConstructed = true;
            
            //
            // 'TaskDialogNotification.DialogConstructed' notification.
            //
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.DialogConstructed;
                taskDialogParams.OnCallback(args);
            }

            //
            // Start 'tmCallback' and set timer start time.
            //
            tmCallback.Stop();            
            if (taskDialogParams.CallbackTimer)
            {
                tmStartTime = DateTime.Now;
                tmCallback.Start();
            }
        }
        #endregion

        #region private void EnableRedrawing(bool enable)
        /// <summary>
        /// Enable/disable form redrawing to prevent flickering.
        /// </summary>
        /// <param name="enable"></param>
        private void EnableRedrawing(bool enable)
        {
            if (enable)
            {
                UnsafeNativeMethods.SendMessage(
                        Handle,
                        0xB,  // WM_SETREDRAW
                        (IntPtr)1,
                        IntPtr.Zero);
                Invalidate(true);
                Update();
            }
            else
            {
                UnsafeNativeMethods.SendMessage(
                        Handle,
                        0xB,  // WM_SETREDRAW
                        (IntPtr)0,
                        IntPtr.Zero);
            }
        }
        #endregion

        #region private Icon CreateIconStretch(Icon icon, Size newSize)
        /// <summary>
        /// Creates new icon from 'icon' and stretches it according to 'newSize'.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        private Icon CreateIconStretch(Icon icon, Size newSize)
        {
            Bitmap square = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(square);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawIcon(icon, new Rectangle(0, 0, newSize.Width, newSize.Height));
            g.Flush();
            return Icon.FromHandle(square.GetHicon());
        }
        #endregion

        #region private void SetControlValues()
        /// <summary>
        /// Sets control values based on 'taskDialogParams' and also sets '...Visible' variables.
        /// </summary>
        private void SetControlValues()
        {
            //
            // Main icon.
            //
            mainIconVisible = true;
            if (taskDialogParams.MainIcon == TaskDialogIcon.None && taskDialogParams.CustomMainIcon == null)
            {
                lblMainIcon.Size = new Size(0, 0);
                lblMainIcon.Margin = new Padding(0, 0, 7, 0);
                mainIconVisible = false;
            }

            //
            // Main instruction.
            //
            if (!String.IsNullOrEmpty(taskDialogParams.MainInstruction))
            {
                mainInstructionVisible = true;
                lblMainInstruction.Text = taskDialogParams.MainInstruction;
            }
            else
            {
                mainInstructionVisible = false;
                lblMainInstruction.Text = "";
            }
            lblMainInstruction.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);

            //
            // Content.
            //
            if (!String.IsNullOrEmpty(taskDialogParams.Content))
            {
                contentVisible = true;
                SetLinkLabelText(lblContent, taskDialogParams.Content, taskDialogParams.EnableHyperlinks);
            }
            else
            {
                contentVisible = false;
                lblContent.Text = "";
            }
            lblContent.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);

            //
            // Expanded information.
            //
            if (!String.IsNullOrEmpty(taskDialogParams.ExpandedInformation) && !taskDialogParams.ExpandFooterArea)
            {
                expandedInformationVisible = true;
                SetLinkLabelText(lblExpandedInformation, taskDialogParams.ExpandedInformation, taskDialogParams.EnableHyperlinks);
            }
            else
            {
                expandedInformationVisible = false;
                lblExpandedInformation.Text = "";
            }
            lblExpandedInformation.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);

            //
            // Progress bar.
            if (taskDialogParams.ShowProgressBar || taskDialogParams.ShowMarqueeProgressBar)
            {
                progressBarVisible = true;
                if (taskDialogParams.ShowMarqueeProgressBar) pbProgress.Style = ProgressBarStyle.Marquee;
                else pbProgress.Style = ProgressBarStyle.Blocks;
            }
            else
            {
                progressBarVisible = false;
            }
            pbProgress.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
            pbProgress.RightToLeftLayout = taskDialogParams.RightToLeftLayout;

            //
            // Radio buttons.
            //
            flpRadioButtons.Controls.Clear();
            defaultRadioButton = null;
            if (taskDialogParams.RadioButtons != null && taskDialogParams.RadioButtons.Length > 0)
            {
                radioButtonsVisible = true;
                foreach (TaskDialogButton tdb in taskDialogParams.RadioButtons)
                {
                    TaskDialogRadioButton tdrb = new TaskDialogRadioButton();
                    tdrb.AutoSizeHeight = true;
                    tdrb.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                    tdrb.Margin = new Padding(2, 6, 0, 2);
                    tdrb.Padding = new Padding(0);
                    tdrb.Tag = tdb.ButtonId;
                    tdrb.Text = tdb.ButtonText;
                    tdrb.TabStop = true;
                    tdrb.Checked = false;
                    tdrb.Click += new System.EventHandler(radioButton_Click);
                    flpRadioButtons.Controls.Add(tdrb);
                    if ((!taskDialogParams.NoDefaultRadioButton && defaultRadioButton == null) || tdb.ButtonId == taskDialogParams.DefaultRadioButton) defaultRadioButton = tdrb;
                }
            }
            else
                radioButtonsVisible = false;

            //
            // Expand/Collapse control.
            // 
            pnlExpandVerify.Controls.Clear();
            chvbExpand = null;
            if (!String.IsNullOrEmpty(taskDialogParams.ExpandedInformation))
            {
                if (taskDialogParams.ExpandedControlText != null && taskDialogParams.ExpandedControlText != "")
                {
                    expandedControlText = taskDialogParams.ExpandedControlText;
                    if (taskDialogParams.CollapsedControlText != null && taskDialogParams.CollapsedControlText != "") collapsedControlText = taskDialogParams.CollapsedControlText;
                    else collapsedControlText = taskDialogParams.ExpandedControlText;
                }
                else
                if (taskDialogParams.CollapsedControlText != null && taskDialogParams.CollapsedControlText != "")
                {
                    collapsedControlText = taskDialogParams.CollapsedControlText;
                    expandedControlText = taskDialogParams.CollapsedControlText;
                }
                
                chvbExpand = new ChevronButton();
                chvbExpand.Padding = new Padding(0);
                chvbExpand.TextLeftMargin = 4;
                chvbExpand.Expanded = taskDialogParams.ExpandedByDefault;
                chvbExpand.TabStop = true;
                chvbExpand.Click += new System.EventHandler(chvbExpand_Click);
                pnlExpandVerify.Controls.Add(chvbExpand);
            }

            //
            // Verify control.
            //
            chbVerify = null;
            if (!String.IsNullOrEmpty(taskDialogParams.VerificationText))
            {
                chbVerify = new TaskDialogCheckBox();
                chbVerify.Padding = new Padding(0);
                chbVerify.TextLeftMargin = 6;
                chbVerify.Checked = taskDialogParams.VerificationFlagChecked;
                chbVerify.Text = taskDialogParams.VerificationText;
                chbVerify.TabStop = true;
                chbVerify.Click += new System.EventHandler(chbVerify_Click);
                pnlExpandVerify.Controls.Add(chbVerify);
            }

            //
            // Adjust margins for 'chvbExpand' and 'chbVerify' and set size for 'pnlExpandVerify'.
            //
            if (chvbExpand != null) chvbExpand.Margin = new Padding(10, 9, 0, (chbVerify != null) ? (0) : (10));
            if (chbVerify != null) chbVerify.Margin = new Padding(13, (chvbExpand != null) ? (7) : (14), 0, 9);
            if (chvbExpand != null || chbVerify != null)
            {
                expandVerifyVisible = true;
                int flpExpandVerifyMinWidth = 0;
                int chvbExpandMaxHeightAndMargin = 0;
                if (chvbExpand != null)
                {
                    chvbExpand.Location = new Point(chvbExpand.Margin.Left, chvbExpand.Margin.Top);
                    chvbExpand.MaximumSize = pnlExpandVerify.MaximumSize - chvbExpand.Margin.Size;
                    chvbExpand.Text = (chvbExpand.Expanded) ? (collapsedControlText) : (expandedControlText);
                    Size chvbExpandSize = chvbExpand.GetPreferredSize(pnlExpandVerify.MaximumSize - chvbExpand.Margin.Size);
                    if (chvbExpand.Margin.Left + chvbExpandSize.Width + chvbExpand.Margin.Right > flpExpandVerifyMinWidth) flpExpandVerifyMinWidth = chvbExpand.Margin.Left + chvbExpandSize.Width + chvbExpand.Margin.Right;
                    if (chvbExpand.Margin.Top + chvbExpandSize.Height + chvbExpand.Margin.Bottom > chvbExpandMaxHeightAndMargin) chvbExpandMaxHeightAndMargin = chvbExpand.Margin.Top + chvbExpandSize.Height + chvbExpand.Margin.Bottom;
                    chvbExpand.Text = (chvbExpand.Expanded) ? (expandedControlText) : (collapsedControlText);
                    chvbExpandSize = chvbExpand.GetPreferredSize(pnlExpandVerify.MaximumSize - chvbExpand.Margin.Size);
                    if (chvbExpand.Margin.Left + chvbExpandSize.Width + chvbExpand.Margin.Right > flpExpandVerifyMinWidth) flpExpandVerifyMinWidth = chvbExpand.Margin.Left + chvbExpandSize.Width + chvbExpand.Margin.Right;
                    if (chvbExpand.Margin.Top + chvbExpandSize.Height + chvbExpand.Margin.Bottom > chvbExpandMaxHeightAndMargin) chvbExpandMaxHeightAndMargin = chvbExpand.Margin.Top + chvbExpandSize.Height + chvbExpand.Margin.Bottom;
                }
                if (chbVerify != null)
                {
                    chbVerify.Location = new Point(chbVerify.Margin.Left, chvbExpandMaxHeightAndMargin + chbVerify.Margin.Top);
                    chbVerify.MaximumSize = pnlExpandVerify.MaximumSize - chbVerify.Margin.Size;
                    int chbVerifyWidth = chbVerify.GetPreferredSize(pnlExpandVerify.MaximumSize - chbVerify.Margin.Size).Width;
                    if (chbVerify.Margin.Left + chbVerifyWidth + chbVerify.Margin.Right > flpExpandVerifyMinWidth) flpExpandVerifyMinWidth = chbVerify.Margin.Left + chbVerifyWidth + chbVerify.Margin.Right;
                }
                pnlExpandVerify.MinimumSize = new Size((flpExpandVerifyMinWidth < pnlExpandVerify.MaximumSize.Width) ? (flpExpandVerifyMinWidth) : (pnlExpandVerify.MaximumSize.Width), 0);
            }
            else
            {
                expandVerifyVisible = false;
            }
            pnlExpandVerify.AutoSize = true;
            if (chvbExpand != null)
            {
                if (taskDialogParams.RightToLeftLayout)
                {
                    SetRightToLeftLocation(chvbExpand);
                    chvbExpand.RightToLeft = RightToLeft.Yes;
                }
                else
                {
                    chvbExpand.RightToLeft = RightToLeft.No;
                }
            }
            if (chbVerify != null && taskDialogParams.RightToLeftLayout)
            {
                if (taskDialogParams.RightToLeftLayout)
                {
                    SetRightToLeftLocation(chbVerify);
                    chbVerify.RightToLeft = RightToLeft.Yes;
                }
                else
                {
                    chbVerify.RightToLeft = RightToLeft.No;
                }
            }

            //
            // Custom buttons.
            //
            flpButtons.Controls.Clear();
            Button firstBtn = null;
            bool defaultButtonSet = false;
            Padding marginBtn = new Padding(3, 0, 3, 0);
            List<Button> buttons = new List<Button>();
            if ((!taskDialogParams.UseCommandLinks && !taskDialogParams.UseCommandLinksNoIcon) && taskDialogParams.Buttons != null && taskDialogParams.Buttons.Length > 0)
            {
                foreach (TaskDialogButton tdb in taskDialogParams.Buttons)
                {
                    Button b = new Button();
                    b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                    b.AutoSize = false;
                    b.Margin = marginBtn;
                    b.Padding = new Padding(0);
                    b.Tag = tdb.ButtonId;
                    b.Text = tdb.ButtonText;
                    b.Click += new System.EventHandler(button_Click);
                    flpButtons.Controls.Add(b);
                    buttons.Add(b);
                    if (firstBtn == null) firstBtn = b;
                    if (tdb.ButtonId == taskDialogParams.DefaultButton)
                    {
                        ActiveControl = b;
                        defaultButtonSet = true;
                    }
                }
            }

            //
            // Common buttons.
            //
            if (((int)taskDialogParams.CommonButtons & (int)TaskDialogCommonButtons.Ok) > 0)
            {
                Button b = new Button();
                b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                b.AutoSize = false;
                b.Margin = marginBtn;
                b.Padding = new Padding(0);
                b.Tag = (int)TaskDialogCommonButtons.Ok;
                b.Text = "OK";
                b.Click += new System.EventHandler(button_Click);
                flpButtons.Controls.Add(b);
                buttons.Add(b);
                if (firstBtn == null) firstBtn = b;
                if ((int)b.Tag == taskDialogParams.DefaultButton)
                {
                    ActiveControl = b;
                    defaultButtonSet = true;
                }
            }

            if (((int)taskDialogParams.CommonButtons & (int)TaskDialogCommonButtons.Yes) > 0)
            {
                Button b = new Button();
                b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                b.AutoSize = false;
                b.Margin = marginBtn;
                b.Padding = new Padding(0);
                b.Tag = (int)TaskDialogCommonButtons.Yes;
                b.Text = "&Yes";
                b.Click += new System.EventHandler(button_Click);
                flpButtons.Controls.Add(b);
                buttons.Add(b);
                if (firstBtn == null) firstBtn = b;
                if ((int)b.Tag == taskDialogParams.DefaultButton)
                {
                    ActiveControl = b;
                    defaultButtonSet = true;
                }
            }

            if (((int)taskDialogParams.CommonButtons & (int)TaskDialogCommonButtons.No) > 0)
            {
                Button b = new Button();
                b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                b.AutoSize = false;
                b.Margin = marginBtn;
                b.Padding = new Padding(0);
                b.Tag = (int)TaskDialogCommonButtons.No;
                b.Text = "&No";
                b.Click += new System.EventHandler(button_Click);
                flpButtons.Controls.Add(b);
                buttons.Add(b);
                if (firstBtn == null) firstBtn = b;
                if ((int)b.Tag == taskDialogParams.DefaultButton)
                {
                    ActiveControl = b;
                    defaultButtonSet = true;
                }
            }

            if (((int)taskDialogParams.CommonButtons & (int)TaskDialogCommonButtons.Retry) > 0)
            {
                Button b = new Button();
                b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                b.AutoSize = false;
                b.Margin = marginBtn;
                b.Padding = new Padding(0);
                b.Tag = (int)TaskDialogCommonButtons.Retry;
                b.Text = "&Retry";
                b.Click += new System.EventHandler(button_Click);
                flpButtons.Controls.Add(b);
                buttons.Add(b);
                if (firstBtn == null) firstBtn = b;
                if ((int)b.Tag == taskDialogParams.DefaultButton)
                {
                    ActiveControl = b;
                    defaultButtonSet = true;
                }
            }

            if (((int)taskDialogParams.CommonButtons & (int)TaskDialogCommonButtons.Cancel) > 0)
            {
                Button b = new Button();
                b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                b.AutoSize = false;
                b.Margin = marginBtn;
                b.Padding = new Padding(0);
                b.Tag = (int)TaskDialogCommonButtons.Cancel;
                b.Text = "Cancel";
                b.Click += new System.EventHandler(button_Click);
                flpButtons.Controls.Add(b);
                buttons.Add(b);
                if (firstBtn == null) firstBtn = b;
                if ((int)b.Tag == taskDialogParams.DefaultButton)
                {
                    ActiveControl = b;
                    defaultButtonSet = true;
                }
            }

            if (((int)taskDialogParams.CommonButtons & (int)TaskDialogCommonButtons.Close) > 0)
            {
                Button b = new Button();
                b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                b.AutoSize = false;
                b.Margin = marginBtn;
                b.Padding = new Padding(0);
                b.Tag = (int)TaskDialogCommonButtons.Close;
                b.Text = "&Close";
                b.Click += new System.EventHandler(button_Click);
                flpButtons.Controls.Add(b);
                buttons.Add(b);
                if (firstBtn == null) firstBtn = b;
                if ((int)b.Tag == taskDialogParams.DefaultButton)
                {
                    ActiveControl = b;
                    defaultButtonSet = true;
                }
            }

            //
            // If there are no buttons or command links specified, OK button is created by default.
            //
            if (firstBtn == null && !(taskDialogParams.UseCommandLinks && taskDialogParams.Buttons != null && taskDialogParams.Buttons.Length > 0))
            {
                Button b = new Button();
                b.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                b.AutoSize = false;
                b.Margin = marginBtn;
                b.Padding = new Padding(0);
                b.Tag = (int)TaskDialogCommonButtons.Ok;
                b.Text = "OK";
                b.Click += new System.EventHandler(button_Click);
                flpButtons.Controls.Add(b);
                buttons.Add(b);
                if (firstBtn == null) firstBtn = b;
                if ((int)b.Tag == taskDialogParams.DefaultButton)
                {
                    ActiveControl = b;
                    defaultButtonSet = true;
                }
            }

            //
            // Adjust button widths.
            if (flpButtons.Controls.Count > 0)
            {
                buttonsVisible = true;
                AdjustButtonWidths(buttons.ToArray());
                flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                flpButtons.AutoSize = true;
            }
            else
                buttonsVisible = false;
           
            //
            // Command links.
            //
            flpCommandLinks.Controls.Clear();
            CommandLink firstCL = null;
            if ((taskDialogParams.UseCommandLinks || taskDialogParams.UseCommandLinksNoIcon) && taskDialogParams.Buttons != null && taskDialogParams.Buttons.Length > 0)
            {
                commandLinksVisible = true;
                foreach (TaskDialogButton tdb in taskDialogParams.Buttons)
                {
                    CommandLink cl = new CommandLink();
                    cl.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
                    cl.Margin = new Padding(0);
                    cl.Padding = new Padding(0);
                    cl.Tag = tdb.ButtonId;
                    cl.Text = tdb.ButtonText;
                    cl.ShowIcon = taskDialogParams.UseCommandLinks || !taskDialogParams.UseCommandLinksNoIcon;
                    cl.Click += new System.EventHandler(button_Click);
                    flpCommandLinks.Controls.Add(cl);
                    if (firstCL == null) firstCL = cl;
                    if (tdb.ButtonId == taskDialogParams.DefaultButton)
                    {
                        ActiveControl = cl;
                        defaultButtonSet = true;
                    }
                }
            }
            else
                commandLinksVisible = false;

            if (!defaultButtonSet)
            {
                if (firstCL != null) ActiveControl = firstCL;
                else
                if (firstBtn != null) ActiveControl = firstBtn;
                defaultButtonSet = true;
            }

            //
            // Footer icon and text.
            //
            if (!String.IsNullOrEmpty(taskDialogParams.Footer))
            {
                footerIconVisible = true;
                footerVisible = true;
                if (taskDialogParams.FooterIcon == TaskDialogIcon.None && taskDialogParams.CustomFooterIcon == null)
                {
                    lblFooterIcon.Size = new Size(0, 0);
                    lblFooterIcon.Margin = new Padding(0, 0, 8, 0);
                    footerIconVisible = false;
                }
                SetLinkLabelText(lblFooter, taskDialogParams.Footer, taskDialogParams.EnableHyperlinks);
            }
            else
            {
                footerIconVisible = false;
                footerVisible = false;
                lblFooter.Text = "";
            }
            lblFooter.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);

            //
            // Expanded information at footer.
            //
            if (!String.IsNullOrEmpty(taskDialogParams.ExpandedInformation) && taskDialogParams.ExpandFooterArea)
            {
                footerExpandedInformationVisible = true;
                SetLinkLabelText(lblFooterExpandedInformation, taskDialogParams.ExpandedInformation, taskDialogParams.EnableHyperlinks);
            }
            else
            {
                footerExpandedInformationVisible = false;
                lblFooterExpandedInformation.Text = "";
            }
            lblFooterExpandedInformation.RightToLeft = (taskDialogParams.RightToLeftLayout) ? (RightToLeft.Yes) : (RightToLeft.No);
        }
        #endregion

        #region private void lblMainIcon_Paint(object sender, PaintEventArgs e)
        /// <summary>
        /// Draws custom main icon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblMainIcon_Paint(object sender, PaintEventArgs e)
        {
            if (taskDialogParams.CustomMainIcon != null)
            {
                SolidBrush sb = new SolidBrush(lblMainIcon.BackColor);
                e.Graphics.FillRectangle(sb, lblMainIcon.ClientRectangle);
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawIcon(taskDialogParams.CustomMainIcon, new Rectangle(0, 0, mainIconSize.Width, mainIconSize.Height));
            }
            else
            {
                SolidBrush sb;
                switch (taskDialogParams.MainIcon)
                {
                    case TaskDialogIcon.Information:
                        sb = new SolidBrush(lblMainIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblMainIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconMainInformation, 0, 0);
                        break;

                    case TaskDialogIcon.Warning:
                        sb = new SolidBrush(lblMainIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblMainIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconMainWarning, 0, 0);
                        break;

                    case TaskDialogIcon.Error:
                        sb = new SolidBrush(lblMainIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblMainIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconMainError, 0, 0);
                        break;

                    case TaskDialogIcon.Shield:
                        sb = new SolidBrush(lblMainIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblMainIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconMainShield, 0, 0);
                        break;
                }
            }
        }
        #endregion

        #region private void lblFooterIcon_Paint(object sender, PaintEventArgs e)
        /// <summary>
        /// Draws custom footer icon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblFooterIcon_Paint(object sender, PaintEventArgs e)
        {
            if (taskDialogParams.CustomFooterIcon != null)
            {
                SolidBrush sb = new SolidBrush(lblFooterIcon.BackColor);
                e.Graphics.FillRectangle(sb, lblFooterIcon.ClientRectangle);
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawIcon(taskDialogParams.CustomFooterIcon, new Rectangle(0, 0, footerIconSize.Width, footerIconSize.Height));
            }
            else
            {
                SolidBrush sb;
                switch (taskDialogParams.FooterIcon)
                {
                    case TaskDialogIcon.Information:
                        sb = new SolidBrush(lblFooterIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblFooterIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconFooterInformation, 0, 0);
                        break;

                    case TaskDialogIcon.Warning:
                        sb = new SolidBrush(lblFooterIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblFooterIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconFooterWarning, 0, 0);
                        break;

                    case TaskDialogIcon.Error:
                        sb = new SolidBrush(lblFooterIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblFooterIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconFooterError, 0, 0);
                        break;

                    case TaskDialogIcon.Shield:
                        sb = new SolidBrush(lblFooterIcon.BackColor);
                        e.Graphics.FillRectangle(sb, lblFooterIcon.ClientRectangle);
                        e.Graphics.DrawIcon(iconFooterShield, 0, 0);
                        break;
                }
            }
        }
        #endregion

        #region private void panelBottom_Paint(object sender, PaintEventArgs e)
        /// <summary>
        /// Draws dividers for bottom panel, footer and expanded information at footer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {
            Color dividerDarkColor = Color.FromArgb((pnlBottom.BackColor.R - 17 >= 0) ? (pnlBottom.BackColor.R - 17) : (0),
                                                    (pnlBottom.BackColor.G - 17 >= 0) ? (pnlBottom.BackColor.G - 17) : (0),
                                                    (pnlBottom.BackColor.B - 17 >= 0) ? (pnlBottom.BackColor.B - 17) : (0));
            Pen penDark = new Pen(dividerDarkColor);
            Pen penWhite = new Pen(Color.White);

            //
            // Bottom panel top divider.
            //
            e.Graphics.DrawLine(penDark, 0, 0, pnlBottom.Width, 0);

            //
            // Footer divider.
            //
            int dividerTop;
            if (lblFooter.Visible)
            {
                dividerTop = lblFooter.Top - lblFooter.Margin.Top;
                if (dividerTop > 0)
                {
                    e.Graphics.DrawLine(penDark, 0, dividerTop, pnlBottom.Width, dividerTop);
                    e.Graphics.DrawLine(penWhite, 0, dividerTop + 1, pnlBottom.Width, dividerTop + 1);
                }
            }

            //
            // Expanded information at footer divider.
            //
            if (lblFooterExpandedInformation.Visible)
            {
                dividerTop = lblFooterExpandedInformation.Top - lblFooterExpandedInformation.Margin.Top;
                if (dividerTop > 0)
                {
                    e.Graphics.DrawLine(penDark, 0, dividerTop, pnlBottom.Width, dividerTop);
                    e.Graphics.DrawLine(penWhite, 0, dividerTop + 1, pnlBottom.Width, dividerTop + 1);
                }
            }
        }
        #endregion

        #region private void SetRightToLeftLocation(Control ctrl)
        /// <summary>
        /// Changes control location according to right to left layout.
        /// </summary>
        /// <param name="ctrl"></param>
        private void SetRightToLeftLocation(Control ctrl)
        {
            ctrl.Left = ctrl.Parent.ClientSize.Width - (ctrl.Left + ctrl.Width);
            ctrl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }
        #endregion

        #region private void ArrangeControls(int currMinimumWidth)
        /// <summary>
        /// Arranges dialog controls and calculates dialog size.
        /// </summary>
        private void ArrangeControls(int currMinimumWidth)
        {
            //
            // 'flpExpandVerify' left.
            //
            int buttonsLeftMargin;
            if (expandVerifyVisible)
            {
                pnlExpandVerify.Left = pnlExpandVerify.Margin.Left;
                buttonsLeftMargin = pnlExpandVerify.Margin.Left + pnlExpandVerify.Width + pnlExpandVerify.Margin.Right;
            }
            else
                buttonsLeftMargin = 15;

            //
            // 'flpButtons'
            //
            flpButtons.MaximumSize = new Size(maximumWidth - (buttonsLeftMargin + flpButtons.Margin.Left + flpButtons.Margin.Right), Int32.MaxValue);
            flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpButtons.AutoSize = true;
            if (buttonsLeftMargin + flpButtons.Margin.Left + flpButtons.Width + flpButtons.Margin.Right < currMinimumWidth) buttonsLeftMargin = currMinimumWidth - (flpButtons.Margin.Left + flpButtons.Width + flpButtons.Margin.Right);
            flpButtons.Left = buttonsLeftMargin + flpButtons.Margin.Left;
            flpButtons.Top = flpButtons.Margin.Top;
            if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(flpButtons);
            int buttonsHeight = (buttonsVisible) ? (flpButtons.Margin.Top + flpButtons.Height + flpButtons.Margin.Bottom) : (0);

            //
            // 'flpExpandVerify' top.
            //
            int expandVerifyHeight = (expandVerifyVisible) ? (pnlExpandVerify.Margin.Top + pnlExpandVerify.Height + pnlExpandVerify.Margin.Bottom) : (0);
            if (expandVerifyHeight > buttonsHeight) pnlExpandVerify.Top = pnlExpandVerify.Margin.Top;
            else pnlExpandVerify.Top = pnlExpandVerify.Margin.Top + ((buttonsHeight - expandVerifyHeight) / 2);
            if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(pnlExpandVerify);
            
            //
            // 'panelBottom' width.
            //
            if (buttonsVisible) pnlBottom.Width = buttonsLeftMargin + flpButtons.Margin.Left + flpButtons.Width + flpButtons.Margin.Right;
            else pnlBottom.Width = currMinimumWidth;

            //
            // Current height of already arranged controls.
            //
            int currentHeight = (expandVerifyHeight > buttonsHeight) ? (expandVerifyHeight) : (buttonsHeight);
            int currMarginTop;
            
            //
            // 'lblFooterIcon'
            //
            int footerTextLeftMargin;
            int footerIconHeight;
            if (footerIconVisible)
            {
                lblFooterIcon.Left = lblFooterIcon.Margin.Left;
                currMarginTop = lblFooterIcon.Margin.Top;
                if (currentHeight == 0) currMarginTop -= 2;
                lblFooterIcon.Top = currentHeight + currMarginTop;
                footerTextLeftMargin = lblFooterIcon.Margin.Left + lblFooterIcon.Width + lblFooterIcon.Margin.Right;
                footerIconHeight = currMarginTop + lblFooterIcon.Height + lblFooterIcon.Margin.Bottom;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(lblFooterIcon);
            }
            else
            {
                footerTextLeftMargin = 8;
                footerIconHeight = 0;
            }

            //
            // 'lblFooter'
            //
            int footerHeight;
            if (footerVisible)
            {
                lblFooter.MaximumSize = new Size(pnlBottom.Width - (footerTextLeftMargin + lblFooter.Margin.Left + lblFooter.Margin.Right), Int32.MaxValue);
                lblFooter.Left = footerTextLeftMargin + lblFooter.Margin.Left;
                currMarginTop = lblFooter.Margin.Top;
                if (currentHeight == 0) currMarginTop -= 2;
                lblFooter.Top = currentHeight + currMarginTop;
                footerHeight = currMarginTop + lblFooter.Height + lblFooter.Margin.Bottom;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(lblFooter);
            }
            else
                footerHeight = 0;

            currentHeight += (footerIconHeight > footerHeight) ? (footerIconHeight) : (footerHeight);

            //
            // 'lblFooterExpandedInformation'
            //
            if (footerExpandedInformationVisible)
            {
                lblFooterExpandedInformation.MaximumSize = new Size(pnlBottom.Width - (lblFooterExpandedInformation.Margin.Left + lblFooterExpandedInformation.Margin.Right), Int32.MaxValue);
                lblFooterExpandedInformation.Left = lblFooterExpandedInformation.Margin.Left;
                lblFooterExpandedInformation.Top = currentHeight + lblFooterExpandedInformation.Margin.Top;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(lblFooterExpandedInformation);
                if (chvbExpand.Expanded) currentHeight += lblFooterExpandedInformation.Margin.Top + lblFooterExpandedInformation.Height + lblFooterExpandedInformation.Margin.Bottom;
            }

            //
            // 'panelBottom' height.
            //
            pnlBottom.Height = currentHeight;

            //
            // 'panelTop' width.
            //
            pnlTop.Width = pnlBottom.Width;

            currentHeight = 0;

            //
            // 'lblMainIcon'
            //
            int textLeftMargin;
            int mainIconHeight;
            if (mainIconVisible)
            {
                lblMainIcon.Left = lblMainIcon.Margin.Left;
                lblMainIcon.Top = lblMainIcon.Margin.Top;
                textLeftMargin = lblMainIcon.Margin.Left + lblMainIcon.Width + lblMainIcon.Margin.Right;
                mainIconHeight = lblMainIcon.Margin.Top + lblMainIcon.Height + lblMainIcon.Margin.Bottom;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(lblMainIcon);
            }
            else
            {
                textLeftMargin = 7;
                mainIconHeight = 0;
            }

            //
            // 'lblMainInstruction'
            //
            if (mainInstructionVisible)
            {
                lblMainInstruction.MaximumSize = new Size(pnlTop.Width - (textLeftMargin + lblMainInstruction.Margin.Left + lblMainInstruction.Margin.Right), Int32.MaxValue);
                lblMainInstruction.Left = textLeftMargin + lblMainInstruction.Margin.Left;
                lblMainInstruction.Top = lblMainInstruction.Margin.Top;
                currentHeight = lblMainInstruction.Margin.Top + lblMainInstruction.Height + lblMainInstruction.Margin.Bottom;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(lblMainInstruction);
            }
            else
                currentHeight = 0;

            //
            // 'lblContent'
            //
            if (contentVisible)
            {
                lblContent.MaximumSize = new Size(pnlTop.Width - (textLeftMargin + lblContent.Margin.Left + lblContent.Margin.Right), Int32.MaxValue);
                lblContent.Left = textLeftMargin + lblContent.Margin.Left;
                lblContent.Top = currentHeight + lblContent.Margin.Top;
                currentHeight += lblContent.Margin.Top + lblContent.Height + lblContent.Margin.Bottom;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(lblContent);
            }
            
            //
            // 'lblExpandedInformation'
            //
            if (expandedInformationVisible)
            {
                lblExpandedInformation.MaximumSize = new Size(pnlTop.Width - (textLeftMargin + lblExpandedInformation.Margin.Left + lblExpandedInformation.Margin.Right), Int32.MaxValue);
                lblExpandedInformation.Left = textLeftMargin + lblExpandedInformation.Margin.Left;
                currentHeight += 10;
                currMarginTop = 1;
                if (!contentVisible)
                {
                    if (mainInstructionVisible) currentHeight += 13;
                    else currentHeight += 14;
                }
                lblExpandedInformation.Margin = new Padding(lblExpandedInformation.Margin.Left, currMarginTop, lblExpandedInformation.Margin.Right, lblExpandedInformation.Margin.Bottom);
                lblExpandedInformation.Top = currentHeight + currMarginTop;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(lblExpandedInformation);
                if (chvbExpand.Expanded) currentHeight += currMarginTop + lblExpandedInformation.Height + lblExpandedInformation.Margin.Bottom;
            }

            //
            // 'pbProgress'
            //
            if (progressBarVisible)
            {
                pbProgress.Width = pnlTop.Width - (textLeftMargin + pbProgress.Margin.Left + pbProgress.Margin.Right);
                pbProgress.Left = textLeftMargin + pbProgress.Margin.Left;
                currMarginTop = pbProgress.Margin.Top;
                if (!expandedInformationVisible && !contentVisible)
                {
                    if (mainInstructionVisible) currMarginTop = 28;
                    else currMarginTop = 29;
                }
                pbProgress.Top = currentHeight + currMarginTop;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(pbProgress);
                currentHeight += currMarginTop + pbProgress.Height + pbProgress.Margin.Bottom;
            }

            //
            // 'flpRadioButtons'
            //
            if (radioButtonsVisible)
            {
                flpRadioButtons.MaximumSize = new Size(pnlTop.Width - (textLeftMargin + flpRadioButtons.Margin.Left + flpRadioButtons.Margin.Right), Int32.MaxValue);
                flpRadioButtons.Width = flpRadioButtons.MaximumSize.Width;
                foreach (TaskDialogRadioButton tdrb in flpRadioButtons.Controls)
                {
                    tdrb.Width = flpRadioButtons.ClientRectangle.Width;                    
                }
                flpRadioButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                flpRadioButtons.AutoSize = true;
                int flpRadioButtonsHeight = flpRadioButtons.Height;
                flpRadioButtons.AutoSize = false;
                flpRadioButtons.Size = new Size(flpRadioButtons.MaximumSize.Width, flpRadioButtonsHeight);
                flpRadioButtons.Left = textLeftMargin + flpRadioButtons.Margin.Left;
                currMarginTop = flpRadioButtons.Margin.Top;
                if (!progressBarVisible)
                {
                    if (expandedInformationVisible || contentVisible) currMarginTop = 11;
                    else
                    if (mainInstructionVisible) currMarginTop = 24;
                    else
                        currMarginTop = 25;
                }
                flpRadioButtons.Top = currentHeight + currMarginTop;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(flpRadioButtons);
                currentHeight += currMarginTop + flpRadioButtons.Height + flpRadioButtons.Margin.Bottom;
            }

            //
            // 'flpCommandLinks'
            //
            if (commandLinksVisible)
            {
                flpCommandLinks.MaximumSize = new Size(pnlTop.Width - (textLeftMargin + flpCommandLinks.Margin.Left + flpCommandLinks.Margin.Right), Int32.MaxValue);
                flpCommandLinks.Width = flpCommandLinks.MaximumSize.Width;
                foreach (CommandLink cl in flpCommandLinks.Controls)
                {
                    cl.Width = flpCommandLinks.ClientRectangle.Width;
                }
                flpCommandLinks.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                flpCommandLinks.AutoSize = true;
                int flpCommandLinksHeight = flpCommandLinks.Height;
                flpCommandLinks.AutoSize = false;
                flpCommandLinks.Size = new Size(flpCommandLinks.MaximumSize.Width, flpCommandLinksHeight);
                flpCommandLinks.Left = textLeftMargin + flpCommandLinks.Margin.Left;
                currMarginTop = flpCommandLinks.Margin.Top;
                if (!radioButtonsVisible)
                {
                    if (progressBarVisible) currMarginTop = 9;
                    else
                    if (expandedInformationVisible || contentVisible) currMarginTop = 15;
                    else
                    if (mainInstructionVisible) currMarginTop = 28;
                    else
                        currMarginTop = 29;
                }
                flpCommandLinks.Top = currentHeight + currMarginTop;
                if (taskDialogParams.RightToLeftLayout) SetRightToLeftLocation(flpCommandLinks);
                currentHeight += currMarginTop + flpCommandLinks.Height + flpCommandLinks.Margin.Bottom;
            }

            //
            // Adjust bottom margin for upper part of task dialog.
            //
            if (commandLinksVisible) currentHeight += 9;
            else
            if (radioButtonsVisible) currentHeight += 14;
            else
            if (progressBarVisible) currentHeight += 9;
            else
            if (expandedInformationVisible || contentVisible) currentHeight += 15;
            else
            if (mainInstructionVisible) currentHeight += 28;
            else
                currentHeight += 29;

            //
            // 'panelTop' height.
            //
            pnlTop.Height = (currentHeight > mainIconHeight) ? (currentHeight) : (mainIconHeight);

            //
            // Position 'panelBottom' just below 'panelTop'.
            //
            pnlBottom.Top = pnlTop.Height;

            //
            // Show/hide controls.
            //
            lblMainIcon.Visible                  = mainIconVisible;
            lblMainInstruction.Visible           = mainInstructionVisible;
            lblContent.Visible                   = contentVisible;
            lblExpandedInformation.Visible       = (expandedInformationVisible && chvbExpand.Expanded);
            pbProgress.Visible                   = progressBarVisible;
            flpRadioButtons.Visible              = radioButtonsVisible;
            flpCommandLinks.Visible              = commandLinksVisible;
            pnlExpandVerify.Visible              = expandVerifyVisible;            
            flpButtons.Visible                   = buttonsVisible;
            lblFooterIcon.Visible                = footerIconVisible;
            lblFooter.Visible                    = footerVisible;
            lblFooterExpandedInformation.Visible = (footerExpandedInformationVisible && chvbExpand.Expanded);

            //
            // Adjust dialog width.
            //
            if (taskDialogParams.Width < 1)
            {
                int dialogClientHeightMax = pnlTop.Height + pnlBottom.Height;
                if (expandedInformationVisible && !chvbExpand.Expanded) dialogClientHeightMax += lblExpandedInformation.Margin.Top + lblExpandedInformation.Height + lblExpandedInformation.Margin.Bottom;
                if (footerExpandedInformationVisible && !chvbExpand.Expanded) dialogClientHeightMax += lblFooterExpandedInformation.Margin.Top + lblFooterExpandedInformation.Height + lblFooterExpandedInformation.Margin.Bottom;
                if ((double)pnlBottom.Width / dialogClientHeightMax <= 1.5)
                {
                    //
                    // This is needed to prevent 'ArrangeControls(...)' being called more than 2 times 
                    // since it is being called recursively.
                    //
                    uint taskDialogParWidth = taskDialogParams.Width;
                    taskDialogParams.Width = 1;

                    ArrangeControls((int)Math.Round((double)pnlBottom.Width + (((double)dialogClientHeightMax - ((double)pnlBottom.Width / 1.5)) / 2.45)));

                    taskDialogParams.Width = taskDialogParWidth;
                    return;
                }
            }

            //if (taskDialogParams.Width < 1 && (double)panelBottom.Width / (panelTop.Height + panelBottom.Height) <= 1.5)
            //{
            //    int heightLimit = (int)Math.Round((double)panelBottom.Width / 1.5);
            //    int widthDelta = 0;
            //    if (mainInstructionVisible && lblMainInstruction.Bounds.Bottom > heightLimit)
            //    {
            //        if (lblMainInstruction.Top > heightLimit) widthDelta += (int)Math.Round(lblMainInstruction.Height / 1.4);
            //        else widthDelta += (int)Math.Round((lblMainInstruction.Bounds.Bottom - heightLimit) / 1.4);
            //    }
            //    if (contentVisible && lblContent.Bounds.Bottom > heightLimit)
            //    {
            //        if (lblContent.Top > heightLimit) widthDelta += (int)Math.Round(lblContent.Height / 1.25);
            //        else widthDelta += (int)Math.Round((lblContent.Bounds.Bottom - heightLimit) / 1.25);
            //    }
            //    if (expandedInformationVisible && lblExpandedInformation.Bounds.Bottom > heightLimit)
            //    {
            //        if (lblExpandedInformation.Top > heightLimit) widthDelta += (int)Math.Round(lblExpandedInformation.Height / 1.5);
            //        else widthDelta += (int)Math.Round((lblExpandedInformation.Bounds.Bottom - heightLimit) / 1.5);
            //    }
            //    if (progressBarVisible && pbProgress.Bounds.Bottom > heightLimit)
            //    {
            //        if (pbProgress.Top > heightLimit) widthDelta += (int)Math.Round(pbProgress.Height / 1.5);
            //        else widthDelta += (int)Math.Round((pbProgress.Bounds.Bottom - heightLimit) / 1.5);
            //    }
            //    if (radioButtonsVisible && flpRadioButtons.Bounds.Bottom > heightLimit)
            //    {
            //        if (flpRadioButtons.Top > heightLimit) widthDelta += (int)Math.Round(flpRadioButtons.Height / 1.5);
            //        else widthDelta += (int)Math.Round((flpRadioButtons.Bounds.Bottom - heightLimit) / 1.5);
            //    }
            //    if (commandLinksVisible && flpCommandLinks.Bounds.Bottom > heightLimit)
            //    {
            //        if (flpCommandLinks.Top > heightLimit) widthDelta += (int)Math.Round(flpCommandLinks.Height / 5.86);
            //        else widthDelta += (int)Math.Round((flpCommandLinks.Bounds.Bottom - heightLimit) / 5.86);
            //    }
            //    if (footerVisible && panelBottom.Top + lblFooter.Bounds.Bottom > heightLimit)
            //    {
            //        if (panelBottom.Top + lblFooter.Top > heightLimit) widthDelta += (int)Math.Round(lblFooter.Height / 1.5);
            //        else widthDelta += (int)Math.Round(((panelBottom.Top + lblFooter.Bounds.Bottom) - heightLimit) / 1.5);
            //    }
            //    if (footerExpandedInformationVisible && panelBottom.Top + lblFooterExpandedInformation.Bounds.Bottom > heightLimit)
            //    {
            //        if (panelBottom.Top + lblFooterExpandedInformation.Top > heightLimit) widthDelta += (int)Math.Round(lblFooterExpandedInformation.Height / 1.5);
            //        else widthDelta += (int)Math.Round(((panelBottom.Top + lblFooterExpandedInformation.Bounds.Bottom) - heightLimit) / 1.5);
            //    }
            //    ArrangeControls(panelBottom.Width + widthDelta);
            //}

            //
            // Set dialog size to fit 'panelTop' and 'panelBottom'. 
            //
            ClientSize = new Size(pnlTop.Width, pnlTop.Height + pnlBottom.Height);
        }
        #endregion

        #region private void SetLinkLabelText(LinkLabel linkLabel, string text, bool enableHyperlinks)
        /// <summary>
        /// Sets text and links for 'linkLabel' based on 'text' content. 
        /// Links are defined within 'text' with '<a href="...">...</a>' notation.
        /// </summary>
        /// <param name="linkLabel"></param>
        /// <param name="enableHyperlinks"></param>
        /// <param name="text"></param>
        private void SetLinkLabelText(LinkLabel linkLabel, string text, bool enableHyperlinks)
        {
            linkLabel.LinkArea = new LinkArea(0, 0);
            linkLabel.Links.Clear();
            if (enableHyperlinks)
            {

                string textUpper = text.ToUpper();
                string textDest = "";
                int linkStartTagInd = textUpper.IndexOf("<A");
                while (linkStartTagInd > -1)
                {
                    int hrefInd = textUpper.IndexOf("HREF", linkStartTagInd);
                    if (hrefInd > -1 && (hrefInd - linkStartTagInd) - 2 >= 1 &&
                        textUpper.Substring(linkStartTagInd + 2, (hrefInd - linkStartTagInd) - 2).Trim() == "")
                    {
                        int equalInd = textUpper.IndexOf("=", hrefInd);
                        if (equalInd > -1 &&
                            textUpper.Substring(hrefInd + 4, (equalInd - hrefInd) - 4).Trim() == "")
                        {
                            int quoteStartInd = textUpper.IndexOf("\"", equalInd);
                            if (quoteStartInd > -1 &&
                                textUpper.Substring(equalInd + 1, (quoteStartInd - equalInd) - 1).Trim() == "")
                            {
                                int quoteEndInd = textUpper.IndexOf("\"", quoteStartInd + 1);
                                if (quoteEndInd > -1)
                                {
                                    int linkStartTagGtInd = textUpper.IndexOf(">", quoteEndInd);
                                    if (linkStartTagGtInd > -1 &&
                                        textUpper.Substring(quoteEndInd + 1, (linkStartTagGtInd - quoteEndInd) - 1).Trim() == "")
                                    {
                                        int linkEndTagInd = textUpper.IndexOf("</A", linkStartTagGtInd);
                                        if (linkEndTagInd > -1)
                                        {
                                            int linkEndTagGtInd = textUpper.IndexOf(">", linkEndTagInd);
                                            if (linkEndTagGtInd > -1 &&
                                                textUpper.Substring(linkEndTagInd + 3, (linkEndTagGtInd - linkEndTagInd) - 3).Trim() == "")
                                            {
                                                textDest += text.Substring(0, linkStartTagInd);
                                                string linkHref = text.Substring(quoteStartInd + 1, (quoteEndInd - quoteStartInd) - 1);
                                                string linkText = text.Substring(linkStartTagGtInd + 1, (linkEndTagInd - linkStartTagGtInd) - 1);
                                                linkLabel.Links.Add(textDest.Length, linkText.Length, linkHref);
                                                textDest += linkText;
                                                textUpper = textUpper.Remove(0, linkEndTagGtInd + 1);
                                                text = text.Remove(0, linkEndTagGtInd + 1);
                                            }
                                            else
                                            {
                                                textDest += text.Substring(0, linkEndTagInd + 3);
                                                textUpper = textUpper.Remove(0, linkEndTagInd + 3);
                                                text = text.Remove(0, linkEndTagInd + 3);
                                            }
                                        }
                                        else
                                        {
                                            textDest += text.Substring(0, linkStartTagGtInd + 1);
                                            textUpper = textUpper.Remove(0, linkStartTagGtInd + 1);
                                            text = text.Remove(0, linkStartTagGtInd + 1);
                                        }
                                    }
                                    else
                                    {
                                        textDest += text.Substring(0, quoteEndInd + 1);
                                        textUpper = textUpper.Remove(0, quoteEndInd + 1);
                                        text = text.Remove(0, quoteEndInd + 1);
                                    }
                                }
                                else
                                {
                                    textDest += text.Substring(0, quoteStartInd + 1);
                                    textUpper = textUpper.Remove(0, quoteStartInd + 1);
                                    text = text.Remove(0, quoteStartInd + 1);
                                }
                            }
                            else
                            {
                                textDest += text.Substring(0, equalInd + 1);
                                textUpper = textUpper.Remove(0, equalInd + 1);
                                text = text.Remove(0, equalInd + 1);
                            }
                        }
                        else
                        {
                            textDest += text.Substring(0, hrefInd + 4);
                            textUpper = textUpper.Remove(0, hrefInd + 4);
                            text = text.Remove(0, hrefInd + 4);
                        }
                    }
                    else
                    {
                        textDest += text.Substring(0, linkStartTagInd + 2);
                        textUpper = textUpper.Remove(0, linkStartTagInd + 2);
                        text = text.Remove(0, linkStartTagInd + 2);
                    }

                    linkStartTagInd = textUpper.IndexOf("<A");
                }
                textDest += text;
                linkLabel.Text = textDest;
            }
            else
            {
                linkLabel.Text = text;
            }
        }
        #endregion

        #region private void AdjustButtonWidths(Button[] buttons)
        /// <summary>
        /// Adjusts widths for the specified buttons.
        /// </summary>
        /// <param name="buttons"></param>
        private void AdjustButtonWidths(Button[] buttons)
        {
            int minButtonWidth = 66;  // 'Button.Width' is actually 68.
            int widthRangeStep = 13;
            int widthDelta = minButtonWidth % widthRangeStep;
            
            //
            // Auto size buttons.
            //
            Padding padding = new Padding(8, 0, 7, 0);
            foreach (Button b in buttons)
            {
                b.Padding = padding;
                b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                b.AutoSize = true;
            }
            
            //
            // Group buttons into width ranges.
            //
            Hashtable widthRanges = new Hashtable();
            List<Button> buttonRange;
            foreach (Button b in buttons)
            {
                int key = (((b.Width - 2 < minButtonWidth) ? (minButtonWidth) : (b.Width - 2)) - widthDelta) / widthRangeStep;
                if (widthRanges.Contains(key)) buttonRange = (List<Button>)widthRanges[key];
                else
                {
                    buttonRange = new List<Button>();
                    widthRanges.Add(key, buttonRange);
                }
                buttonRange.Add(b);
            }

            //
            // Set button width to widest button width in its width range.
            //
            foreach (List<Button> br in widthRanges.Values)
            {
                int maxWidth = minButtonWidth + 2;
                foreach (Button b in br) if (b.Width > maxWidth) maxWidth = b.Width;
                foreach (Button b in br)
                {
                    int btnHeigth = b.Height;
                    b.AutoSize = false;
                    b.Width = maxWidth;
                    b.Height = btnHeigth;
                }
            }
        }
        #endregion

        #region private void chvbExpand_Click(object sender, EventArgs e)
        /// <summary>
        /// Changes 'chvbExpand' text and shows/hides expanded information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chvbExpand_Click(object sender, EventArgs e)
        {
            EnableRedrawing(false);
            if (chvbExpand.Expanded)
            {
                if (taskDialogParams.ExpandFooterArea)
                {
                    pnlBottom.Height = lblFooterExpandedInformation.Top + lblFooterExpandedInformation.Height + lblFooterExpandedInformation.Margin.Bottom;
                    lblFooterExpandedInformation.Visible = true;
                }
                else
                {
                    int expandedInformationHeight = lblExpandedInformation.Margin.Top + lblExpandedInformation.Height + lblExpandedInformation.Margin.Bottom;
                    foreach (Control ctl in pnlTop.Controls)
                    {
                        if (ctl != (Control)lblExpandedInformation && ctl != (Control)lblMainIcon && ctl.Top >= lblExpandedInformation.Top - lblExpandedInformation.Margin.Top) ctl.Top += expandedInformationHeight;
                    }
                    lblExpandedInformation.Visible = true;
                    pnlTop.Height += expandedInformationHeight;
                    pnlBottom.Top = pnlTop.Height;
                }
                chvbExpand.Text = expandedControlText;
            }
            else
            {
                if (taskDialogParams.ExpandFooterArea)
                {
                    pnlBottom.Height = lblFooterExpandedInformation.Top - lblFooterExpandedInformation.Margin.Top;
                    lblFooterExpandedInformation.Visible = false;
                }
                else
                {
                    int expandedInformationHeight = lblExpandedInformation.Margin.Top + lblExpandedInformation.Height + lblExpandedInformation.Margin.Bottom;
                    lblExpandedInformation.Visible = false;
                    foreach (Control ctl in pnlTop.Controls)
                    {
                        if (ctl != (Control)lblExpandedInformation && ctl != (Control)lblMainIcon && ctl.Top >= lblExpandedInformation.Top - lblExpandedInformation.Margin.Top) ctl.Top -= expandedInformationHeight;
                    }
                    pnlTop.Height -= expandedInformationHeight;
                    pnlBottom.Top = pnlTop.Height;
                }
                chvbExpand.Text = collapsedControlText;
            }
            EnableRedrawing(true);
            ClientSize = new Size(pnlTop.Width, pnlTop.Height + pnlBottom.Height);
            Update();

            //
            // 'TaskDialogNotification.ExpandoButtonClicked' notification.
            //
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.ExpandoButtonClicked;
                args.Expanded = chvbExpand.Expanded;
                taskDialogParams.OnCallback(args);
            }
        }
        #endregion

        #region private void chbVerify_Click(object sender, EventArgs e)
        /// <summary>
        /// 'chbVerify' has been clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbVerify_Click(object sender, EventArgs e)
        {
            //
            // 'TaskDialogNotification.VerificationClicked' notification.
            //
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.VerificationClicked;
                args.VerificationFlagChecked = chbVerify.Checked;
                taskDialogParams.OnCallback(args);
            }
        }
        #endregion

        #region private void lblContentExpandedInformationFooter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        /// <summary>
        /// Occurrs when link in 'lblContent', 'lblExpandedInformation', 'lblFooterExpandedInformation' or
        /// 'lblFooter' has been clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblContentExpandedInformationFooter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //
            // 'TaskDialogNotification.HyperlinkClicked' notification.
            //
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.HyperlinkClicked;
                args.Hyperlink = (string)e.Link.LinkData;
                taskDialogParams.OnCallback(args);
            }
        }
        #endregion

        #region private void radioButton_Click(object sender, EventArgs e)
        /// <summary>
        /// Radio button has been clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Click(object sender, EventArgs e)
        {
            if (((TaskDialogRadioButton)sender).Checked) defaultRadioButton = (TaskDialogRadioButton)sender;
            
            //
            // 'TaskDialogNotification.RadioButtonClicked' notification.
            //
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.RadioButtonClicked;
                args.ButtonId = (int)(((Control)sender).Tag);
                taskDialogParams.OnCallback(args);
            }
        }
        #endregion       
        
        #region private void FormTaskDialog_KeyDown(object sender, KeyEventArgs e)
        /// <summary>
        /// Checks whether some keys have been pressed on the keyboard (F1, Esc).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTaskDialog_KeyDown(object sender, KeyEventArgs e)
        {
            //
            // 'TaskDialogNotification.Help' notification.
            //
            if (e.KeyCode == Keys.F1 && !(e.Alt || e.Control) &&
                taskDialogParams.CallbackEventHandlerExists && dialogConstructed)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.Help;
                taskDialogParams.OnCallback(args);
            }
                
            //
            // 'TaskDialogNotification.ButtonClicked' notification.
            //
            if (e.KeyCode == Keys.Escape && !(e.Alt || e.Control) && ControlBox && dialogConstructed)
            {
                bool closeDialog = true;
                if (taskDialogParams.CallbackEventHandlerExists)
                {
                    TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                    args.ActiveDialog = new ActiveTaskDialog(this);
                    args.CallbackData = taskDialogParams.CallbackData;
                    args.Notification = TaskDialogNotification.ButtonClicked;
                    args.ButtonId = (int)TaskDialogCommonButtons.Cancel;
                    args.CloseTaskDialog = true;
                    taskDialogParams.OnCallback(args);
                    closeDialog = args.CloseTaskDialog;
                }
                if (closeDialog)
                {
                    //
                    // Close dialog and return clicked button id.
                    //
                    dialogResultButtonId = (int)TaskDialogCommonButtons.Cancel;
                    Close();
                }
            }
        }
        #endregion

        #region private void tmCallback_Tick(object sender, EventArgs e)
        /// <summary>
        /// Tick event has occured for 'tmCallback'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmCallback_Tick(object sender, EventArgs e)
        {
            //
            // 'TaskDialogNotification.Timer' notification.
            //
            bool resetTickCount = false;
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.Timer;
                double totalMilliseconds = Math.Round((DateTime.Now - tmStartTime).TotalMilliseconds);
                args.TimerTickCount = (totalMilliseconds >= UInt32.MaxValue) ? (UInt32.MaxValue) : ((uint)totalMilliseconds);
                args.ResetTimerTickCount = false;
                taskDialogParams.OnCallback(args);
                resetTickCount = args.ResetTimerTickCount;
            }
            if (resetTickCount)
            {
                //
                // Reset timer start time.
                //
                tmStartTime = DateTime.Now;
            }
        }
        #endregion

        #region private void button_Click(object sender, EventArgs e)
        /// <summary>
        /// Closes dialog when button or command link is clicked. 
        /// Button id is stored in 'dialogResultButtonId'.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            //
            // 'TaskDialogNotification.ButtonClicked' notification.
            //
            bool closeDialog = true;
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.ButtonClicked;
                args.ButtonId = (int)(((Control)sender).Tag);
                args.CloseTaskDialog = true;
                taskDialogParams.OnCallback(args);
                closeDialog = args.CloseTaskDialog;
            }
            if (closeDialog)
            {
                //
                // Close dialog and return clicked button id.
                //
                dialogResultButtonId = (int)(((Control)sender).Tag);
                Close();
            }
        }
        #endregion

        #region private void FormTaskDialog_FormClosing(object sender, FormClosingEventArgs e)
        /// <summary>
        /// Dialog is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTaskDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //
                // 'TaskDialogNotification.ButtonClicked' notification.
                //
                bool closeDialog = true;
                if (taskDialogParams.CallbackEventHandlerExists && dialogResultButtonId == -1)
                {
                    if (!ControlBox)
                    {
                        e.Cancel = true;
                        return;
                    }
                    TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                    args.ActiveDialog = new ActiveTaskDialog(this);
                    args.CallbackData = taskDialogParams.CallbackData;
                    args.Notification = TaskDialogNotification.ButtonClicked;
                    args.ButtonId = (int)TaskDialogCommonButtons.Cancel;
                    args.CloseTaskDialog = true;
                    taskDialogParams.OnCallback(args);
                    closeDialog = args.CloseTaskDialog;
                    if (closeDialog) dialogResultButtonId = (int)TaskDialogCommonButtons.Cancel;
                }
                if (closeDialog)
                {
                    tmCallback.Stop();

                    //
                    // 'TaskDialogNotification.Destroyed' notification.
                    //
                    if (taskDialogParams.CallbackEventHandlerExists)
                    {
                        TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                        args.ActiveDialog = new ActiveTaskDialog(this);
                        args.CallbackData = taskDialogParams.CallbackData;
                        args.Notification = TaskDialogNotification.Destroyed;
                        taskDialogParams.OnCallback(args);
                    }
                }
                else
                    e.Cancel = true;
            }
            else
            {
                tmCallback.Stop();

                //
                // 'TaskDialogNotification.Destroyed' notification.
                //
                if (taskDialogParams.CallbackEventHandlerExists)
                {
                    TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                    args.ActiveDialog = new ActiveTaskDialog(this);
                    args.CallbackData = taskDialogParams.CallbackData;
                    args.Notification = TaskDialogNotification.Destroyed;
                    taskDialogParams.OnCallback(args);
                }
            }
        }
        #endregion



        #region internal bool SetMainInstruction(string mainInstruction)
        /// <summary>
        /// Sets text for 'lblMainInstruction'.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal bool SetMainInstruction(string mainInstruction)
        {
            if (mainInstruction != null)
            {
                EnableRedrawing(false);
                lblMainInstruction.Text = mainInstruction;
                lblMainInstruction.AutoSize = true;
                mainInstructionVisible = (mainInstruction != "") ? (true) : (false);
                ArrangeControls(minimumWidth);
                EnableRedrawing(true);
                return true;
            }
            return false;
        }
        #endregion
        
        #region internal bool SetContent(string content)
        /// <summary>
        /// Sets text for 'lblContent'.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal bool SetContent(string content)
        {
            if (content != null)
            {
                EnableRedrawing(false);
                SetLinkLabelText(lblContent, content, taskDialogParams.EnableHyperlinks);
                lblContent.AutoSize = true;
                contentVisible = (content != "") ? (true) : (false);
                ArrangeControls(minimumWidth);
                EnableRedrawing(true);
                return true;
            }
            return false;
        }
        #endregion

        #region internal bool SetExpandedInformation(string expandedInformation)
        /// <summary>
        /// Sets text for 'lblExpandedInformation' or 'lblFooterExpandedInformation'.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal bool SetExpandedInformation(string expandedInformation)
        {
            if (expandedInformation != null && (expandedInformationVisible || footerExpandedInformationVisible))
            {
                EnableRedrawing(false);
                if (taskDialogParams.ExpandFooterArea)
                {
                    SetLinkLabelText(lblFooterExpandedInformation, expandedInformation, taskDialogParams.EnableHyperlinks);
                    lblFooterExpandedInformation.AutoSize = true;
                    //footerExpandedInformationVisible = (expandedInformation != "") ? (true) : (false);
                }
                else
                {
                    SetLinkLabelText(lblExpandedInformation, expandedInformation, taskDialogParams.EnableHyperlinks);
                    lblExpandedInformation.AutoSize = true;
                    //expandedInformationVisible = (expandedInformation != "") ? (true) : (false);
                }
                ArrangeControls(minimumWidth);
                EnableRedrawing(true);
                return true;
            }
            return false;
        }
        #endregion

        #region internal bool SetMarqueeProgressBar(bool marquee)
        /// <summary>
        /// Whether progress bar should be displayed in marquee mode or not.
        /// </summary>
        /// <param name="marquee"></param>
        /// <returns></returns>
        internal bool SetMarqueeProgressBar(bool marquee)
        {
            if (marquee) pbProgress.Style = ProgressBarStyle.Marquee;
            else pbProgress.Style = ProgressBarStyle.Blocks;
            return true;
        }
        #endregion

        #region internal bool SetProgressBarRange(ushort minRange, ushort maxRange)
        /// <summary>
        /// Sets the minimum and maximum values for the progress bar.
        /// </summary>
        /// <param name="marquee"></param>
        /// <returns></returns>
        internal bool SetProgressBarRange(short minRange, short maxRange)
        {
            pbProgress.Minimum = minRange;
            pbProgress.Maximum = maxRange;
            return true;
        }
        #endregion

        #region internal int SetProgressBarPosition(int newPosition)
        /// <summary>
        /// Sets the current position for the progress bar.
        /// </summary>
        /// <param name="marquee"></param>
        /// <returns></returns>
        internal int SetProgressBarPosition(int newPosition)
        {
            int oldValue = pbProgress.Value;
            pbProgress.Value = newPosition;
            return oldValue;
        }
        #endregion

        #region internal void SetProgressBarMarquee(bool startMarquee, uint speed)
        /// <summary>
        /// Sets the animation state of the Marquee Progress Bar.
        /// </summary>
        /// <param name="startMarquee">True starts the marquee animation and false stops it.</param>
        /// <param name="speed">The time in milliseconds that it takes the progress block to scroll across the progress bar.</param>
        internal void SetProgressBarMarquee(bool startMarquee, uint speed)
        {
            if (startMarquee) pbProgress.MarqueeAnimationSpeed = (speed == 0) ? (20) : ((int)speed);
            else pbProgress.MarqueeAnimationSpeed = 0;
        }
        #endregion

        #region internal void EnableRadioButton(int buttonId, bool enable)
        /// <summary>
        /// Enables/disables specified radio button.
        /// </summary>
        /// <param name="buttonId"></param>
        /// <param name="enable"></param>
        internal void EnableRadioButton(int buttonId, bool enable)
        {
            if (radioButtonsVisible)
            {
                foreach (TaskDialogRadioButton tdrb in flpRadioButtons.Controls)
                {
                    if ((int)tdrb.Tag == buttonId)
                    {
                        tdrb.Enabled = enable;
                        break;
                    }
                }
            }
        }
        #endregion

        #region internal void ClickRadioButton(int buttonId)
        /// <summary>
        /// Simulates the action of a radio button click.
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns>True.</returns>
        internal void ClickRadioButton(int buttonId)
        {
            if (radioButtonsVisible)
            {
                foreach (TaskDialogRadioButton tdrb in flpRadioButtons.Controls)
                {
                    if ((int)tdrb.Tag == buttonId)
                    {
                        tdrb.Checked = true;
                        radioButton_Click(tdrb, null);
                        break;
                    }
                }
            }
        }
        #endregion

        #region internal void EnableButton(int buttonId, bool enable)
        /// <summary>
        /// Enables/disables specified button.
        /// </summary>
        /// <param name="buttonId"></param>
        /// <param name="enable"></param>
        internal void EnableButton(int buttonId, bool enable)
        {
            bool buttonFound = false;
            if (commandLinksVisible)
            {
                foreach (CommandLink cl in flpCommandLinks.Controls)
                {
                    if ((int)cl.Tag == buttonId)
                    {
                        cl.Enabled = enable;
                        buttonFound = true;
                        break;
                    }
                }
            }
            if (!buttonFound && buttonsVisible)
            {
                foreach (Button btn in flpButtons.Controls)
                {
                    if ((int)btn.Tag == buttonId)
                    {
                        btn.Enabled = enable;
                        buttonFound = true;
                        break;
                    }
                }
            }
        }
        #endregion
        
        #region internal void ClickButton(int buttonId)
        /// <summary>
        /// Simulates the action of a button click.
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns>True.</returns>
        internal bool ClickButton(int buttonId)
        {
            Control ctrl = new Control();
            ctrl.Tag = buttonId;
            button_Click(ctrl, null);
            return true;

            //bool buttonFound = false;
            //if (commandLinksVisible)
            //{
            //    foreach (CommandLink cl in flpCommandLinks.Controls)
            //    {
            //        if ((int)cl.Tag == buttonId)
            //        {
            //            button_Click(cl, null);
            //            buttonFound = true;
            //            break;
            //        }
            //    }
            //}
            //if (!buttonFound && buttonsVisible)
            //{
            //    foreach (Button btn in flpButtons.Controls)
            //    {
            //        if ((int)btn.Tag == buttonId)
            //        {
            //            button_Click(btn, null);
            //            buttonFound = true;
            //            break;
            //        }
            //    }
            //}
            //if (!buttonFound && taskDialogParams.AllowDialogCancellation && buttonId == (int)DialogResult.Cancel)
            //{
            //    button_Click(this, null);
            //    buttonFound = true;
            //}
            //return buttonFound;
        }
        #endregion

        #region internal void ClickVerification(bool checkedState, bool setKeyboardFocusToCheckBox)
        /// <summary>
        /// Simulates the action of a verification control click.
        /// </summary>
        /// <param name="checkedState"></param>
        /// <param name="setKeyboardFocusToCheckBox"></param>
        internal void ClickVerification(bool checkedState, bool setKeyboardFocusToCheckBox)
        {
            if (chbVerify != null)
            {
                chbVerify.Checked = checkedState;
                if (setKeyboardFocusToCheckBox) chbVerify.Focus();
                chbVerify_Click(chbVerify, null);
            }
        }
        #endregion

        #region internal bool SetFooter(string footer)
        /// <summary>
        /// Sets text for 'lblFooter'.
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        internal bool SetFooter(string footer)
        {
            if (footerVisible && footer != null)
            {
                EnableRedrawing(false);
                SetLinkLabelText(lblFooter, footer, taskDialogParams.EnableHyperlinks);
                lblFooter.AutoSize = true;
                footerVisible = (footer != "") ? (true) : (false);
                if (!footerVisible) footerIconVisible = false;
                ArrangeControls(minimumWidth);
                EnableRedrawing(true);
                return true;
            }
            return false;
        }
        #endregion

        #region internal void UpdateMainIcon(TaskDialogIcon icon)
        /// <summary>
        /// Sets icon for 'lblMainIcon' and for dialog title bar icon.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateMainIcon(TaskDialogIcon icon)
        {
            EnableRedrawing(false);
            Size clientSizeOld = ClientSize;
            mainIconVisible = true;
            lblMainIcon.Size = mainIconSize;
            lblMainIcon.Margin = mainIconMargin;
            taskDialogParams.MainIcon = icon;
            switch (taskDialogParams.MainIcon)
            {
                case TaskDialogIcon.None:
                    taskDialogParams.CustomMainIcon = null;
                    lblMainIcon.Size = new Size(0, 0);
                    lblMainIcon.Margin = new Padding(0, 0, 7, 0);
                    mainIconVisible = false;
                    Icon = Properties.Resources.IconTaskDialog;
                    break;

                case TaskDialogIcon.Information:
                    if (taskDialogParams.CustomMainIcon == null) Icon = iconFooterInformation;
                    break;

                case TaskDialogIcon.Warning:
                    if (taskDialogParams.CustomMainIcon == null) Icon = iconFooterWarning;
                    break;

                case TaskDialogIcon.Error:
                    if (taskDialogParams.CustomMainIcon == null) Icon = iconFooterError;
                    break;

                case TaskDialogIcon.Shield:
                    if (taskDialogParams.CustomMainIcon == null) Icon = iconFooterShield;
                    break;
            }
            // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
            uint taskDialogParWidth = taskDialogParams.Width;
            taskDialogParams.Width = 1;
            ArrangeControls(clientSizeOld.Width);
            taskDialogParams.Width = taskDialogParWidth;
            ClientSize = clientSizeOld;
            EnableRedrawing(true);
        }
        #endregion

        #region internal void UpdateMainIcon(Icon icon)
        /// <summary>
        /// Sets custom icon for 'lblMainIcon'.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateMainIcon(Icon icon)
        {
            if (taskDialogParams.CustomMainIcon != null || icon == null)
            {
                EnableRedrawing(false);
                Size clientSizeOld = ClientSize;
                mainIconVisible = true;
                lblMainIcon.Size = mainIconSize;
                lblMainIcon.Margin = mainIconMargin;
                taskDialogParams.MainIcon = TaskDialogIcon.None;
                if (icon != null)
                {
                    taskDialogParams.CustomMainIcon = icon;
                    Icon = CreateIconStretch(icon, new Size(16, 16));
                }
                else
                {
                    taskDialogParams.CustomMainIcon = null;
                    lblMainIcon.Size = new Size(0, 0);
                    lblMainIcon.Margin = new Padding(0, 0, 7, 0);
                    Icon = Properties.Resources.IconTaskDialog;
                }
                // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
                uint taskDialogParWidth = taskDialogParams.Width;
                taskDialogParams.Width = 1;
                ArrangeControls(clientSizeOld.Width);
                taskDialogParams.Width = taskDialogParWidth;
                ClientSize = clientSizeOld;
                EnableRedrawing(true);
            }
        }
        #endregion

        #region internal void UpdateMainInstruction(string mainInstruction)
        /// <summary>
        /// Updates text for 'lblMainInstruction'. Dialog box size is not changed.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateMainInstruction(string mainInstruction)
        {
            if (mainInstruction != null)
            {
                EnableRedrawing(false);
                Size clientSizeOld = ClientSize;
                lblMainInstruction.Text = mainInstruction;
                lblMainInstruction.AutoSize = true;
                mainInstructionVisible = (mainInstruction != "") ? (true) : (false);
                // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
                uint taskDialogParWidth = taskDialogParams.Width;
                taskDialogParams.Width = 1;
                ArrangeControls(clientSizeOld.Width);
                taskDialogParams.Width = taskDialogParWidth;
                ClientSize = clientSizeOld;
                EnableRedrawing(true);
            }
        }
        #endregion

        #region internal void UpdateContent(string content)
        /// <summary>
        /// Updates text for 'lblContent'. Dialog box size is not changed.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateContent(string content)
        {
            if (content != null)
            {
                EnableRedrawing(false);
                Size clientSizeOld = ClientSize;
                SetLinkLabelText(lblContent, content, taskDialogParams.EnableHyperlinks);
                lblContent.AutoSize = true;
                contentVisible = (content != "") ? (true) : (false);
                // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
                uint taskDialogParWidth = taskDialogParams.Width;
                taskDialogParams.Width = 1;
                ArrangeControls(clientSizeOld.Width);
                taskDialogParams.Width = taskDialogParWidth;
                ClientSize = clientSizeOld;
                EnableRedrawing(true);
            }
        }
        #endregion

        #region internal void UpdateExpandedInformation(string expandedInformation)
        /// <summary>
        /// Updates text for 'lblExpandedInformation' or 'lblFooterExpandedInformation'.
        /// Dialog box size is not changed.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateExpandedInformation(string expandedInformation)
        {
            if (expandedInformation != null && (expandedInformationVisible || footerExpandedInformationVisible))
            {
                EnableRedrawing(false);
                Size clientSizeOld = ClientSize;
                if (taskDialogParams.ExpandFooterArea)
                {
                    SetLinkLabelText(lblFooterExpandedInformation, expandedInformation, taskDialogParams.EnableHyperlinks);
                    lblFooterExpandedInformation.AutoSize = true;
                    //footerExpandedInformationVisible = (expandedInformation != "") ? (true) : (false);
                }
                else
                {
                    SetLinkLabelText(lblExpandedInformation, expandedInformation, taskDialogParams.EnableHyperlinks);
                    lblExpandedInformation.AutoSize = true;
                    //expandedInformationVisible = (expandedInformation != "") ? (true) : (false);
                }
                // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
                uint taskDialogParWidth = taskDialogParams.Width;
                taskDialogParams.Width = 1;
                ArrangeControls(clientSizeOld.Width);
                taskDialogParams.Width = taskDialogParWidth;
                ClientSize = clientSizeOld;
                EnableRedrawing(true);
            }
        }
        #endregion

        #region internal void UpdateFooterIcon(TaskDialogIcon icon)
        /// <summary>
        /// Sets icon for 'lblFooterIcon' if 'footerVisible' is true.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateFooterIcon(TaskDialogIcon icon)
        {
            if (footerVisible)
            {
                EnableRedrawing(false);
                Size clientSizeOld = ClientSize;
                footerIconVisible = true;
                lblFooterIcon.Size = footerIconSize;
                lblFooterIcon.Margin = footerIconMargin;
                taskDialogParams.FooterIcon = icon;
                if (taskDialogParams.FooterIcon == TaskDialogIcon.None)
                {
                    taskDialogParams.CustomFooterIcon = null;
                    lblFooterIcon.Size = new Size(0, 0);
                    lblFooterIcon.Margin = new Padding(0, 0, 8, 0);
                    footerIconVisible = false;
                }
                // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
                uint taskDialogParWidth = taskDialogParams.Width;
                taskDialogParams.Width = 1;
                ArrangeControls(clientSizeOld.Width);
                taskDialogParams.Width = taskDialogParWidth;
                ClientSize = clientSizeOld;
                EnableRedrawing(true);
            }
        }
        #endregion

        #region internal void UpdateFooterIcon(Icon icon)
        /// <summary>
        /// Sets custom icon for 'lblFooterIcon' if 'footerVisible' is true.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateFooterIcon(Icon icon)
        {
            if (footerVisible && (taskDialogParams.CustomFooterIcon != null || icon == null))
            {
                EnableRedrawing(false);
                Size clientSizeOld = ClientSize;
                footerIconVisible = true;
                lblFooterIcon.Size = footerIconSize;
                lblFooterIcon.Margin = footerIconMargin;
                taskDialogParams.FooterIcon = TaskDialogIcon.None;
                if (icon != null) taskDialogParams.CustomFooterIcon = icon;
                else
                {
                    taskDialogParams.CustomFooterIcon = null;
                    lblFooterIcon.Size = new Size(0, 0);
                    lblFooterIcon.Margin = new Padding(0, 0, 8, 0);
                }
                // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
                uint taskDialogParWidth = taskDialogParams.Width;
                taskDialogParams.Width = 1;
                ArrangeControls(clientSizeOld.Width);
                taskDialogParams.Width = taskDialogParWidth;
                ClientSize = clientSizeOld;
                EnableRedrawing(true);
            }
        }
        #endregion

        #region internal void UpdateFooter(string footer)
        /// <summary>
        /// Updates text for 'lblFooter'. Dialog box size is not changed.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal void UpdateFooter(string footer)
        {
            if (footerVisible && footer != null)
            {
                EnableRedrawing(false);
                Size clientSizeOld = ClientSize;
                SetLinkLabelText(lblFooter, footer, taskDialogParams.EnableHyperlinks);
                lblFooter.AutoSize = true;
                footerVisible = (footer != "") ? (true) : (false);
                if (!footerVisible) footerIconVisible = false;
                // This is needed to prevent 'ArrangeControls(...)' from resizing the dialog box. 
                uint taskDialogParWidth = taskDialogParams.Width;
                taskDialogParams.Width = 1;
                ArrangeControls(clientSizeOld.Width);
                taskDialogParams.Width = taskDialogParWidth;
                ClientSize = clientSizeOld;
                EnableRedrawing(true);
            }
        }
        #endregion

        #region internal void NavigatePage(TaskDialog taskDialogParams)
        /// <summary>
        /// Dynamically changes the Task Dialog contents at run time. A new Task Dialog (looks like a new page) is
        /// created with the elements specified in the taskDialogParams parameter.
        /// </summary>
        /// <param name="taskDialogParams"></param>
        internal void NavigatePage(TaskDialog taskDialogParams)
        {
            EnableRedrawing(false);
            ConstructTaskDialog(taskDialogParams);

            //
            // Check default radio button.
            //
            if (defaultRadioButton != null)
            {
                defaultRadioButton.Checked = true;
                radioButton_Click(defaultRadioButton, null);
            }
            
            EnableRedrawing(true);

            //
            // 'TaskDialogNotification.Navigated' notification.
            //
            if (taskDialogParams.CallbackEventHandlerExists)
            {
                TaskDialogCallbackEventArgs args = new TaskDialogCallbackEventArgs();
                args.ActiveDialog = new ActiveTaskDialog(this);
                args.CallbackData = taskDialogParams.CallbackData;
                args.Notification = TaskDialogNotification.Navigated;
                taskDialogParams.OnCallback(args);
            }
        }
        #endregion


        
        #region public int ShowTaskDialog(bool showModal, out bool verificationFlagChecked, out int radioButtonResult)
        /// <summary>
        /// Shows task dialog.
        /// </summary>
        /// <param name="showModal">Whether to show modal or modeless dialog</param>
        /// <param name="verificationFlagChecked">Returns true if the verification checkbox was checked</param>
        /// <param name="radioButtonResult">The radio botton id selected by the user</param>
        /// <returns>The result of the dialog, either a TaskDialogCommonButtons value for common push buttons set in the CommonButtons
        /// member or the ButtonID from a TaskDialogButton structure set on the Buttons member.</returns>
        public int ShowTaskDialog(bool showModal, out bool verificationFlagChecked, out int radioButtonResult)
        {
            if (showModal) ShowDialog();
            else
            {
                Show();
                while (Visible)
                {
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            tmCallback.Stop();
            verificationFlagChecked = (chbVerify != null) ? (chbVerify.Checked) :(false);
            radioButtonResult = (defaultRadioButton != null) ? ((int)defaultRadioButton.Tag) : (0);
            return dialogResultButtonId;
        }
        #endregion



        [DllImport("Gdi32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("Gdi32.dll", CharSet = CharSet.Unicode)]
        static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);

        [DllImport("Gdi32.dll", CharSet = CharSet.Unicode)]
        static extern bool DeleteObject(IntPtr hdc);

        [StructLayout(LayoutKind.Sequential)]
        public struct TEXTMETRIC
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public char tmFirstChar;
            public char tmLastChar;
            public char tmDefaultChar;
            public char tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }

        public static TEXTMETRIC GetTextMetrics(Graphics graphics, Font font)
        {
            IntPtr hDC = graphics.GetHdc();
            TEXTMETRIC textMetric;
            IntPtr hFont = font.ToHfont();
            try
            {
                IntPtr hFontPreviouse = SelectObject(hDC, hFont);
                bool result = GetTextMetrics(hDC, out textMetric);
                SelectObject(hDC, hFontPreviouse);
            }
            finally
            {
                DeleteObject(hFont);
                graphics.ReleaseHdc(hDC);
            }
            return textMetric;
        }
    }
}

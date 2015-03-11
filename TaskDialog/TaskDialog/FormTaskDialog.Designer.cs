namespace WindowsTaskDialog
{
    partial class FormTaskDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskDialog));
            this.tmCallback = new System.Windows.Forms.Timer(this.components);
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblMainIcon = new System.Windows.Forms.Label();
            this.lblMainInstruction = new WindowsTaskDialog.LabelClearType(this.components);
            this.lblContent = new System.Windows.Forms.LinkLabel();
            this.lblExpandedInformation = new System.Windows.Forms.LinkLabel();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.flpRadioButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.flpCommandLinks = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblFooter = new System.Windows.Forms.LinkLabel();
            this.lblFooterIcon = new System.Windows.Forms.Label();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFooterExpandedInformation = new System.Windows.Forms.LinkLabel();
            this.pnlExpandVerify = new System.Windows.Forms.Panel();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmCallback
            // 
            this.tmCallback.Interval = 200;
            this.tmCallback.Tick += new System.EventHandler(this.tmCallback_Tick);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblMainIcon);
            this.pnlTop.Controls.Add(this.lblMainInstruction);
            this.pnlTop.Controls.Add(this.lblContent);
            this.pnlTop.Controls.Add(this.lblExpandedInformation);
            this.pnlTop.Controls.Add(this.pbProgress);
            this.pnlTop.Controls.Add(this.flpRadioButtons);
            this.pnlTop.Controls.Add(this.flpCommandLinks);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(583, 269);
            this.pnlTop.TabIndex = 1;
            // 
            // lblMainIcon
            // 
            this.lblMainIcon.Location = new System.Drawing.Point(10, 10);
            this.lblMainIcon.Margin = new System.Windows.Forms.Padding(10, 10, 7, 10);
            this.lblMainIcon.Name = "lblMainIcon";
            this.lblMainIcon.Size = new System.Drawing.Size(32, 32);
            this.lblMainIcon.TabIndex = 7;
            this.lblMainIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMainIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.lblMainIcon_Paint);
            // 
            // lblMainInstruction
            // 
            this.lblMainInstruction.AutoSize = true;
            this.lblMainInstruction.BackColor = System.Drawing.Color.Transparent;
            this.lblMainInstruction.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblMainInstruction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this.lblMainInstruction.Location = new System.Drawing.Point(54, 11);
            this.lblMainInstruction.Margin = new System.Windows.Forms.Padding(0, 11, 10, 0);
            this.lblMainInstruction.Name = "lblMainInstruction";
            this.lblMainInstruction.Size = new System.Drawing.Size(124, 20);
            this.lblMainInstruction.TabIndex = 8;
            this.lblMainInstruction.Text = "lblMainInstruction";
            this.lblMainInstruction.UseMnemonic = false;
            // 
            // lblContent
            // 
            this.lblContent.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblContent.AutoSize = true;
            this.lblContent.BackColor = System.Drawing.Color.Transparent;
            this.lblContent.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblContent.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblContent.Location = new System.Drawing.Point(55, 45);
            this.lblContent.Margin = new System.Windows.Forms.Padding(1, 14, 10, 0);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(56, 13);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "lblContent";
            this.lblContent.UseMnemonic = false;
            this.lblContent.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblContent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblContentExpandedInformationFooter_LinkClicked);
            // 
            // lblExpandedInformation
            // 
            this.lblExpandedInformation.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblExpandedInformation.AutoSize = true;
            this.lblExpandedInformation.BackColor = System.Drawing.Color.Transparent;
            this.lblExpandedInformation.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblExpandedInformation.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblExpandedInformation.Location = new System.Drawing.Point(55, 69);
            this.lblExpandedInformation.Margin = new System.Windows.Forms.Padding(1, 11, 10, 0);
            this.lblExpandedInformation.Name = "lblExpandedInformation";
            this.lblExpandedInformation.Size = new System.Drawing.Size(121, 13);
            this.lblExpandedInformation.TabIndex = 1;
            this.lblExpandedInformation.Text = "lblExpandedInformation";
            this.lblExpandedInformation.UseMnemonic = false;
            this.lblExpandedInformation.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblExpandedInformation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblContentExpandedInformationFooter_LinkClicked);
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(57, 97);
            this.pbProgress.Margin = new System.Windows.Forms.Padding(3, 15, 10, 0);
            this.pbProgress.MarqueeAnimationSpeed = 0;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(502, 15);
            this.pbProgress.TabIndex = 11;
            // 
            // flpRadioButtons
            // 
            this.flpRadioButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpRadioButtons.Location = new System.Drawing.Point(65, 119);
            this.flpRadioButtons.Margin = new System.Windows.Forms.Padding(11, 5, 10, 0);
            this.flpRadioButtons.Name = "flpRadioButtons";
            this.flpRadioButtons.Size = new System.Drawing.Size(494, 65);
            this.flpRadioButtons.TabIndex = 2;
            this.flpRadioButtons.TabStop = true;
            this.flpRadioButtons.WrapContents = false;
            // 
            // flpCommandLinks
            // 
            this.flpCommandLinks.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpCommandLinks.Location = new System.Drawing.Point(57, 196);
            this.flpCommandLinks.Margin = new System.Windows.Forms.Padding(3, 14, 10, 0);
            this.flpCommandLinks.Name = "flpCommandLinks";
            this.flpCommandLinks.Size = new System.Drawing.Size(502, 56);
            this.flpCommandLinks.TabIndex = 3;
            this.flpCommandLinks.WrapContents = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lblFooter);
            this.pnlBottom.Controls.Add(this.lblFooterIcon);
            this.pnlBottom.Controls.Add(this.flpButtons);
            this.pnlBottom.Controls.Add(this.lblFooterExpandedInformation);
            this.pnlBottom.Controls.Add(this.pnlExpandVerify);
            this.pnlBottom.Location = new System.Drawing.Point(0, 287);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(583, 140);
            this.pnlBottom.TabIndex = 2;
            this.pnlBottom.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBottom_Paint);
            // 
            // lblFooter
            // 
            this.lblFooter.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblFooter.AutoSize = true;
            this.lblFooter.BackColor = System.Drawing.Color.Transparent;
            this.lblFooter.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblFooter.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblFooter.Location = new System.Drawing.Point(29, 69);
            this.lblFooter.Margin = new System.Windows.Forms.Padding(0, 12, 10, 10);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(49, 13);
            this.lblFooter.TabIndex = 6;
            this.lblFooter.Text = "lblFooter";
            this.lblFooter.UseMnemonic = false;
            this.lblFooter.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblFooter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblContentExpandedInformationFooter_LinkClicked);
            // 
            // lblFooterIcon
            // 
            this.lblFooterIcon.Location = new System.Drawing.Point(8, 69);
            this.lblFooterIcon.Margin = new System.Windows.Forms.Padding(12, 11, 5, 9);
            this.lblFooterIcon.Name = "lblFooterIcon";
            this.lblFooterIcon.Size = new System.Drawing.Size(16, 16);
            this.lblFooterIcon.TabIndex = 7;
            this.lblFooterIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.lblFooterIcon_Paint);
            // 
            // flpButtons
            // 
            this.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButtons.Location = new System.Drawing.Point(112, 17);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(14, 9, 7, 9);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(319, 32);
            this.flpButtons.TabIndex = 5;
            // 
            // lblFooterExpandedInformation
            // 
            this.lblFooterExpandedInformation.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblFooterExpandedInformation.AutoSize = true;
            this.lblFooterExpandedInformation.BackColor = System.Drawing.Color.Transparent;
            this.lblFooterExpandedInformation.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblFooterExpandedInformation.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblFooterExpandedInformation.Location = new System.Drawing.Point(8, 106);
            this.lblFooterExpandedInformation.Margin = new System.Windows.Forms.Padding(8, 12, 10, 10);
            this.lblFooterExpandedInformation.Name = "lblFooterExpandedInformation";
            this.lblFooterExpandedInformation.Size = new System.Drawing.Size(153, 13);
            this.lblFooterExpandedInformation.TabIndex = 7;
            this.lblFooterExpandedInformation.Text = "lblFooterExpandedInformation";
            this.lblFooterExpandedInformation.UseMnemonic = false;
            this.lblFooterExpandedInformation.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblFooterExpandedInformation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblContentExpandedInformationFooter_LinkClicked);
            // 
            // pnlExpandVerify
            // 
            this.pnlExpandVerify.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlExpandVerify.Location = new System.Drawing.Point(0, 12);
            this.pnlExpandVerify.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pnlExpandVerify.MaximumSize = new System.Drawing.Size(305, 5000);
            this.pnlExpandVerify.Name = "pnlExpandVerify";
            this.pnlExpandVerify.Size = new System.Drawing.Size(98, 46);
            this.pnlExpandVerify.TabIndex = 4;
            // 
            // FormTaskDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(627, 473);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTaskDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormTaskDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTaskDialog_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormTaskDialog_KeyDown);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmCallback;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblMainIcon;
        private LabelClearType lblMainInstruction;
        private System.Windows.Forms.LinkLabel lblContent;
        private System.Windows.Forms.LinkLabel lblExpandedInformation;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.FlowLayoutPanel flpRadioButtons;
        private System.Windows.Forms.FlowLayoutPanel flpCommandLinks;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.LinkLabel lblFooter;
        private System.Windows.Forms.Panel pnlExpandVerify;
        private System.Windows.Forms.Label lblFooterIcon;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.LinkLabel lblFooterExpandedInformation;
    }
}
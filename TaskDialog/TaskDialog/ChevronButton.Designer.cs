namespace WindowsTaskDialog
{
    partial class ChevronButton
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imglChevrons = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imglChevrons
            // 
            this.imglChevrons.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imglChevrons.ImageSize = new System.Drawing.Size(20, 19);
            this.imglChevrons.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // ChevronButton
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ImageList = this.imglChevrons;
            this.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.UseVisualStyleBackColor = false;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imglChevrons;
    }
}

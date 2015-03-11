
//**********************************************//
//                                              //
//     Copyright (c) 2008 Wintaskdialog.com     //
//                                              //
//**********************************************//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsTaskDialog
{
    public partial class LabelClearType : Label
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LabelClearType()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container"></param>
        public LabelClearType(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }



        #region public override Size GetPreferredSize(Size proposedSize)
        /// <summary>
        /// Calculate control's preferred size for 'proposedSize' (maximum size).
        /// </summary>
        /// <param name="proposedSize"></param>
        /// <returns></returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            Size maxSize;
            if (proposedSize.IsEmpty && AutoSize) maxSize = MaximumSize;
            else maxSize = proposedSize;
            if (proposedSize.Width > MaximumSize.Width) proposedSize.Width = MaximumSize.Width;
            if (proposedSize.Height > MaximumSize.Height) proposedSize.Height = MaximumSize.Height;
            Graphics g = Graphics.FromHwnd(Handle);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            StringFormat strFormat = new StringFormat();
            if (RightToLeft == RightToLeft.Yes) strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            SizeF sizeF = g.MeasureString(Text, Font, maxSize, strFormat);
            Size size = new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
            g.Dispose();
            return size + Padding.Size;
        }
        #endregion

        #region protected override void OnPaint(PaintEventArgs e)
        /// <summary>
        /// Draws control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            StringFormat strFormat = new StringFormat();
            if (RightToLeft == RightToLeft.Yes) strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            g.DrawString(Text, Font, new SolidBrush(ForeColor), new RectangleF(new PointF(0, 0), ClientRectangle.Size), strFormat);
        }
        #endregion
    }
}


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
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WindowsTaskDialog
{
    public partial class CommandLink : Button
    {
        private Color colorBackground              = Color.White;
        private Color colorBorderActive            = Color.FromArgb(134, 231, 255);
        private Color colorBackgroundHoveredTop    = Color.FromArgb(250, 250, 250);  //Color.White;
        private Color colorBackgroundHoveredBottom = Color.FromArgb(250, 250, 250);  //Color.FromArgb(251, 251, 251);
        private Color colorBorderHovered           = Color.FromArgb(198, 198, 198);
        private Color colorBackgroundPressedTop    = Color.FromArgb(240, 241, 241);  //Color.FromArgb(242, 243, 243);
        private Color colorBackgroundPressedBottom = Color.FromArgb(240, 241, 241);  //Color.FromArgb(240, 241, 241);
        private Color colorBorderPressed           = Color.FromArgb(173, 173, 173);
        private Color colorText                    = Color.FromArgb(21, 28, 85);     //Color.FromArgb(22, 29, 95);
        private Color colorTextHovered             = Color.FromArgb(7, 74, 229);
        private Color colorTextPressed             = Color.FromArgb(6, 32, 115);
        private Color colorTextDisabled            = Color.FromArgb(126, 133, 156);

        private bool isHovered   = false;
        private bool isKeyDown   = false;
        private bool isMouseDown = false;
       
        private string largeText = "";
        private string smallText = "";
        private Font smallTextFont = null;

        private int textLeftMargin   = 30;
        private int textTopMargin    = 12;
        private int textRightMargin  = 4;
        private int textBottomMargin = 8;

        private const int minHeight = 41;
        private bool showIcon = true;
        
        
        
        public CommandLink()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
        }


        public CommandLink(IContainer container)
        {
            container.Add(this);
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
        }



        #region public override string Text
        /// <summary>
        /// Handles command link text storage (large and small text).
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set 
            {
                int newLineIndex = value.IndexOf('\n');
                if (newLineIndex < 0)
                {
                    largeText = value;
                    smallText = "";
                }
                else
                {
                    largeText = value.Substring(0, newLineIndex);
                    smallText = (newLineIndex < value.Length - 1)?(value.Substring(newLineIndex + 1)):("");
                }
                Height = BestHeight;
                base.Text = largeText;
            }
        }
        #endregion

        #region public override Font Font
        /// <summary>
        /// Creates font for small text when font is changed.
        /// </summary>
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                smallTextFont = new Font(Font.FontFamily, (float)(Font.SizeInPoints * 0.80), Font.Style, GraphicsUnit.Point, Font.GdiCharSet, Font.GdiVerticalFont);
            }
        }
        #endregion

        #region public bool ShowIcon
        /// <summary>
        /// Whether to show command link icon.
        /// </summary>
        public bool ShowIcon
        {
            get { return showIcon; }
            set
            {
                showIcon = value;
                if (showIcon) textLeftMargin = 30;
                else textLeftMargin = 5;
                Invalidate();
            }
        }
        #endregion              
        
        #region private SizeF LargeTextSize
        /// <summary>
        /// Returns large (main) text size.
        /// </summary>
        private SizeF LargeTextSize
        {
            get
            {
                SizeF maxSize = new SizeF((float)(Width - textLeftMargin) - textRightMargin, float.MaxValue);
                Graphics g = Graphics.FromHwnd(Handle);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                SizeF textSize = g.MeasureString(largeText, Font, maxSize);
                g.Dispose();
                return textSize;
            }
        }
        #endregion

        #region private SizeF SmallTextSize
        /// <summary>
        /// Returns small text size.
        /// </summary>
        private SizeF SmallTextSize
        {
            get
            {
                SizeF maxSize = new SizeF((float)(Width - textLeftMargin) - textRightMargin, float.MaxValue);
                Graphics g = Graphics.FromHwnd(Handle);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                if (smallTextFont == null) smallTextFont = new Font(Font.FontFamily, (float)(Font.SizeInPoints * 0.80), Font.Style, GraphicsUnit.Point, Font.GdiCharSet, Font.GdiVerticalFont);
                SizeF textSize = g.MeasureString(smallText, smallTextFont, maxSize);
                g.Dispose();
                return textSize;
            }
        }
        #endregion

        #region private int BestHeight
        /// <summary>
        /// Returns best height according to large and small command link text.
        /// </summary>
        private int BestHeight
        {
            get { return textTopMargin + (int)Math.Ceiling(LargeTextSize.Height) + (int)Math.Ceiling(SmallTextSize.Height) + textBottomMargin; }
        }
        #endregion



        #region private GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        /// <summary>
        /// Creates rectangle with round borders.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;

            // topleft 
            path.AddArc(l, t, d, d, 180, 90);
            // top 
            path.AddLine(l + radius, t, l + w - radius, t);
            // topright 
            path.AddArc(l + w - d, t, d, d, 270, 90);
            // right 
            path.AddLine(l + w, t + radius, l + w, t + h - radius);
            // bottomright 
            path.AddArc(l + w - d, t + h - d, d, d, 0, 90);
            // bottom 
            path.AddLine(l + w - radius, t + h, l + radius, t + h);
            // bottomleft 
            path.AddArc(l, t + h - d, d, d, 90, 90);
            // left 
            path.AddLine(l, t + h - radius, l, t + radius);

            path.CloseFigure();
            return path;
        }
        #endregion
        
        #region protected override void OnPaint(PaintEventArgs pevent)
        /// <summary>
        /// Draws command link.
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0) rect.Width = 1;
            if (rect.Height <= 0) rect.Height = 1;
            Point greenArrowCoord;
            if (RightToLeft == RightToLeft.Yes) greenArrowCoord = new Point(rect.Width - (9 + Properties.Resources.ImgGreenArrow.Width), 12);
            else greenArrowCoord = new Point(9, 12);
            Graphics g = pevent.Graphics;
            GraphicsPath gp;
            LinearGradientBrush lgb;
            Pen p;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            if (Enabled)
            {
                if (isMouseDown || isKeyDown)
                {
                    rect.Width -= 1;
                    rect.Height -= 1;
                    gp = CreateRoundRectangle(rect, 3);
                    p = new Pen(colorBorderPressed);
                    g.DrawPath(p, gp);
                    rect.Inflate(-1, -1);
                    gp = CreateRoundRectangle(rect, 2);
                    lgb = new LinearGradientBrush(rect, colorBackgroundPressedTop, colorBackgroundPressedBottom, LinearGradientMode.Vertical);
                    g.FillPath(lgb, gp);
                    if (showIcon)
                    {
                        Image arrow = Properties.Resources.ImgGreenArrowPressed;
                        if (RightToLeft == RightToLeft.Yes) arrow.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        g.DrawImage(arrow, greenArrowCoord);
                    }
                    SizeF largeTextSize = LargeTextSize;
                    SizeF smallTextSize = SmallTextSize;
                    StringFormat strFormat = new StringFormat();
                    if (ShowKeyboardCues) strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                    else strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
                    float largeTextX;
                    float smallTextX;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                        largeTextX = ClientRectangle.Width - (textLeftMargin + largeTextSize.Width);
                        smallTextX = ClientRectangle.Width - (textLeftMargin + 1 + smallTextSize.Width);
                    }
                    else
                    {
                        largeTextX = textLeftMargin;
                        smallTextX = textLeftMargin + 1;
                    }
                    g.DrawString(largeText, Font, new SolidBrush(colorTextPressed), new RectangleF(new PointF(largeTextX, textTopMargin), largeTextSize), strFormat);
                    strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                    g.DrawString(smallText, smallTextFont, new SolidBrush(colorTextPressed), new RectangleF(new PointF(smallTextX, textTopMargin + (int)Math.Ceiling(largeTextSize.Height - 1)), smallTextSize), strFormat);
                }
                else
                if (isHovered)
                {
                    rect.Width -= 1;
                    rect.Height -= 1;
                    gp = CreateRoundRectangle(rect, 3);
                    p = new Pen(colorBorderHovered);
                    g.DrawPath(p, gp);
                    rect.Inflate(-1, -1);
                    gp = CreateRoundRectangle(rect, 2);
                    lgb = new LinearGradientBrush(rect, colorBackgroundHoveredTop, colorBackgroundHoveredBottom, LinearGradientMode.Vertical);
                    g.FillPath(lgb, gp);
                    if (showIcon)
                    {
                        Image arrow = Properties.Resources.ImgGreenArrowHovered;
                        if (RightToLeft == RightToLeft.Yes) arrow.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        g.DrawImage(arrow, greenArrowCoord);
                    }
                    SizeF largeTextSize = LargeTextSize;
                    SizeF smallTextSize = SmallTextSize;
                    StringFormat strFormat = new StringFormat();
                    if (ShowKeyboardCues) strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                    else strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
                    float largeTextX;
                    float smallTextX;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                        largeTextX = ClientRectangle.Width - (textLeftMargin + largeTextSize.Width);
                        smallTextX = ClientRectangle.Width - (textLeftMargin + 1 + smallTextSize.Width);
                    }
                    else
                    {
                        largeTextX = textLeftMargin;
                        smallTextX = textLeftMargin + 1;
                    }
                    g.DrawString(largeText, Font, new SolidBrush(colorTextHovered), new RectangleF(new PointF(largeTextX, textTopMargin), largeTextSize), strFormat);
                    strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                    g.DrawString(smallText, smallTextFont, new SolidBrush(colorTextHovered), new RectangleF(new PointF(smallTextX, textTopMargin + (int)Math.Ceiling(largeTextSize.Height - 1)), smallTextSize), strFormat);
                }
                else
                {
                    if (showIcon)
                    {
                        Image arrow = Properties.Resources.ImgGreenArrow;
                        if (RightToLeft == RightToLeft.Yes) arrow.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        g.DrawImage(arrow, greenArrowCoord);
                    }
                    SizeF largeTextSize = LargeTextSize;
                    SizeF smallTextSize = SmallTextSize;
                    StringFormat strFormat = new StringFormat();
                    if (ShowKeyboardCues) strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                    else strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
                    float largeTextX;
                    float smallTextX;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                        largeTextX = ClientRectangle.Width - (textLeftMargin + largeTextSize.Width);
                        smallTextX = ClientRectangle.Width - (textLeftMargin + 1 + smallTextSize.Width);
                    }
                    else
                    {
                        largeTextX = textLeftMargin;
                        smallTextX = textLeftMargin + 1;
                    }
                    g.DrawString(largeText, Font, new SolidBrush(colorText), new RectangleF(new PointF(largeTextX, textTopMargin), largeTextSize), strFormat);
                    strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                    g.DrawString(smallText, smallTextFont, new SolidBrush(colorText), new RectangleF(new PointF(smallTextX, textTopMargin + (int)Math.Ceiling(largeTextSize.Height - 1)), smallTextSize), strFormat);

                    if (IsDefault)
                    {
                        rect.Width -= 1;
                        rect.Height -= 1;
                        rect.X++;
                        rect.Width -= 2;
                        gp = CreateRoundRectangle(rect, 3);
                        p = new Pen(colorBorderActive);
                        g.DrawPath(p, gp);
                    }
                }

                if (Focused && ShowFocusCues)
                {
                    rect = ClientRectangle;
                    rect.Inflate(-2, -2);
                    rect.X++;
                    rect.Width -= 2;
                    rect.Y++;
                    rect.Height -= 2;
                    ControlPaint.DrawFocusRectangle(g, rect);
                }
            }
            else
            {
                if (showIcon)
                {
                    Image arrow = Properties.Resources.ImgGreenArrowDisabled;
                    if (RightToLeft == RightToLeft.Yes) arrow.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(arrow, greenArrowCoord);
                }

                SizeF largeTextSize = LargeTextSize;
                SizeF smallTextSize = SmallTextSize;
                StringFormat strFormat = new StringFormat();
                if (ShowKeyboardCues) strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                else strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
                float largeTextX;
                float smallTextX;
                if (RightToLeft == RightToLeft.Yes)
                {
                    strFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    largeTextX = ClientRectangle.Width - (textLeftMargin + largeTextSize.Width);
                    smallTextX = ClientRectangle.Width - (textLeftMargin + 1 + smallTextSize.Width);
                }
                else
                {
                    largeTextX = textLeftMargin;
                    smallTextX = textLeftMargin + 1;
                }
                g.DrawString(largeText, Font, new SolidBrush(colorTextDisabled), new RectangleF(new PointF(largeTextX, textTopMargin), largeTextSize), strFormat);
                strFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                g.DrawString(smallText, smallTextFont, new SolidBrush(colorTextDisabled), new RectangleF(new PointF(smallTextX, textTopMargin + (int)Math.Ceiling(largeTextSize.Height - 1)), smallTextSize), strFormat);
            }
            
            //base.OnPaint(pevent);
        }
        #endregion
                
        #region public override Size GetPreferredSize(Size proposedSize)
        /// <summary>
        /// Returns preffered control size.
        /// </summary>
        /// <param name="proposedSize"></param>
        /// <returns></returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize = base.GetPreferredSize(proposedSize);
            if (proposedSize.Height < minHeight) proposedSize.Height = minHeight;
            return proposedSize;
        }
        #endregion

        #region protected override void OnResize(EventArgs e)
        /// <summary>
        /// Makes sure height is always set to best height.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            int bestHeight = BestHeight;
            if (Height != bestHeight) Height = bestHeight;
            base.OnResize(e);
        }
        #endregion

        #region protected override void OnMouseEnter(EventArgs e)
        /// <summary>
        /// Performs effect when mouse enters command link.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }
        #endregion

        #region protected override void OnMouseLeave(EventArgs e)
        /// <summary>
        /// Performs effect when mouse leaves command link.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            isMouseDown = false;
            Invalidate();
            base.OnMouseLeave(e);
        }
        #endregion

        #region protected override void OnMouseMove(MouseEventArgs mevent)
        /// <summary>
        /// Performs effect when mouse is moved.
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                if (!ClientRectangle.Contains(mevent.X, mevent.Y))
                {
                    if (isMouseDown)
                    {
                        isMouseDown = false;
                        Invalidate();
                    }
                }
                else
                if (!isMouseDown)
                {
                    isMouseDown = true;
                    Invalidate();
                }
            }
            base.OnMouseMove(mevent);            
        }
        #endregion    
        
        #region protected override void OnKeyDown(KeyEventArgs kevent)
        /// <summary>
        /// Performs effect when space is pressed on keyboard.
        /// </summary>
        /// <param name="kevent"></param>
        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space)
            {
                isKeyDown = true;
                Invalidate();
            }
            base.OnKeyDown(kevent);
        }
        #endregion

        #region protected override void OnKeyUp(KeyEventArgs kevent)
        /// <summary>
        /// Performs effect when space is released on keyboard.
        /// </summary>
        /// <param name="kevent"></param>
        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (isKeyDown && kevent.KeyCode == Keys.Space)
            {
                isKeyDown = false;
                Invalidate();
            }
            base.OnKeyUp(kevent);
        }
        #endregion

        #region protected override void OnMouseDown(MouseEventArgs mevent)
        /// <summary>
        /// Performs effect when mouse left button is pressed.
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                Invalidate();
            }
            base.OnMouseDown(mevent);
        }
        #endregion

        #region protected override void OnClick(EventArgs e)
        /// <summary>
        /// Performs effect when mouse is clicked.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            isMouseDown = false;
            Invalidate();
            base.OnClick(e);
        }
        #endregion
    }
}

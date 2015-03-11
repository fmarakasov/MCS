
//**********************************************//
//                                              //
//     Copyright (c) 2008 Wintaskdialog.com     //
//                                              //
//**********************************************//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsTaskDialog
{
    public partial class ChevronButton : CheckBox
    {      
        private bool isHovered   = false;
        private bool isKeyDown   = false;
        private bool isMouseDown = false;
        private bool isExpanded  = false;

        private int imgChevronMoreIndex;
        private int imgChevronMoreHoveredIndex;
        private int imgChevronMorePressedIndex;
        private int imgChevronLessIndex;
        private int imgChevronLessHoveredIndex;
        private int imgChevronLessPressedIndex;

        private int textLeftMargin = 4; //5;
        private int maxLineCount = Int32.MaxValue;

        private Hashtable htDisplayTextSizes = new Hashtable();



        /// <summary>
        /// Constructor.
        /// </summary>
        public ChevronButton()
        {
            InitializeComponent();
            PopulateImglChevrons();
            ImageIndex = imgChevronMoreIndex;
            SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container"></param>
        public ChevronButton(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            PopulateImglChevrons();
            ImageIndex = imgChevronMoreIndex;
            SetStyle(ControlStyles.UserPaint, true);
        }



        #region public int TextLeftMargin
        /// <summary>
        /// Horizontal margin between image and text. 
        /// </summary>
        public int TextLeftMargin
        {
            get { return textLeftMargin; }
            set { textLeftMargin = value; base.OnResize(null); }
        }
        #endregion

        #region public int MaxLineCount
        /// <summary>
        /// Maximum number of text lines to show. If it's set to Int32.MaxValue, number of lines is unlimited.
        /// </summary>
        public int MaxLineCount
        {
            get { return maxLineCount; }
            set { maxLineCount = value; base.OnResize(null); }
        }
        #endregion

        #region public bool Expanded
        /// <summary>
        /// Designates whether chevron button is expanded or collapsed. 
        /// </summary>
        public bool Expanded
        {
            get { return isExpanded; }
            set { isExpanded = value; SetImage(); }
        }
        #endregion

        #region public override string Text
        /// <summary>
        /// Sets/gets control text.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                htDisplayTextSizes.Clear();
                base.Text = value;
            }
        }
        #endregion

        #region public override Font Font
        /// <summary>
        /// Sets/gets font.
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                htDisplayTextSizes.Clear();
                base.Font = value;
            }
        }
        #endregion


        
        #region private void PopulateImglChevrons()
        /// <summary>
        /// Populates 'imglChevrons' with chevron images and sets 'imgChevron...' indexes to valid values.
        /// </summary>
        private void PopulateImglChevrons()
        {
            imglChevrons.Images.Clear();
            imglChevrons.Images.Add(Properties.Resources.ImgChevronMore);
            imglChevrons.Images.Add(Properties.Resources.ImgChevronMoreHovered);
            imglChevrons.Images.Add(Properties.Resources.ImgChevronMorePressed);
            imglChevrons.Images.Add(Properties.Resources.ImgChevronLess);
            imglChevrons.Images.Add(Properties.Resources.ImgChevronLessHovered);
            imglChevrons.Images.Add(Properties.Resources.ImgChevronLessPressed);

            imgChevronMoreIndex        = 0;
            imgChevronMoreHoveredIndex = 1;
            imgChevronMorePressedIndex = 2;
            imgChevronLessIndex        = 3;
            imgChevronLessHoveredIndex = 4;
            imgChevronLessPressedIndex = 5;
        }
        #endregion

        #region private void SetImage()
        /// <summary>
        /// Displays appropriate image for the chevron button.
        /// </summary>
        private void SetImage()
        {
            if (isMouseDown || isKeyDown)
            {
                ImageIndex = (isExpanded) ? (imgChevronLessPressedIndex) : (imgChevronMorePressedIndex);
            }
            else
            if (isHovered)
            {
                ImageIndex = (isExpanded) ? (imgChevronLessHoveredIndex) : (imgChevronMoreHoveredIndex);
            }
            else
            {
                ImageIndex = (isExpanded) ? (imgChevronLessIndex) : (imgChevronMoreIndex);
            }
        }
        #endregion

        #region public static Size MeasureTextWordWrap(string text, out string textModified, Font font, int maxWidth, int maxLineCount)
        /// <summary>
        /// Returns size (in pixels) of the 'text' that fits into region 
        /// defined with 'maxWidth' (in pixels) and 'maxLineCount'.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textModified"></param>
        /// <param name="font"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxLineCount"></param>
        /// <returns></returns>
        public static Size MeasureTextWordWrap(string text, out string textModified, Font font, int maxWidth, int maxLineCount)
        {
            Size modifiedTextSize = new Size(0, 0);
            if (maxLineCount == Int32.MaxValue)
            {
                textModified = text;
                Size maxTextSize = new Size(maxWidth, Int32.MaxValue);
                modifiedTextSize = TextRenderer.MeasureText(textModified, font, maxTextSize, TextFormatFlags.WordBreak | TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                textModified = "";
                Size maxTextSize = new Size(maxWidth, Int32.MaxValue);
                Size textSize = new Size(0, 0);
                int lineCount = 0;
                string textOrig = new String(text.ToCharArray());
                int currOrigTextPos = 0;
                while (textOrig != "" && lineCount < maxLineCount)
                {
                    textSize = TextRenderer.MeasureText(textOrig, font, maxTextSize, TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.ModifyString);
                    int ellipsisPos = textOrig.IndexOf("...\0");
                    int newLinePos = textOrig.IndexOf("\n");
                    bool newLineReplaced = false;
                    if (newLinePos > -1 && !(ellipsisPos > -1 && newLinePos > ellipsisPos))
                    {
                        textOrig = textOrig.Remove(newLinePos, 1);
                        textOrig = textOrig.Insert(newLinePos, "...\0");
                        ellipsisPos = newLinePos;
                        newLineReplaced = true;
                    }
                    if (ellipsisPos < 0)
                    {
                        if (lineCount == 0) textModified += textOrig;
                        else textModified += "\n" + textOrig;
                        lineCount++;
                        textOrig = "";
                        if (modifiedTextSize.Width < textSize.Width) modifiedTextSize.Width = textSize.Width;
                        modifiedTextSize.Height += textSize.Height;
                        break;
                    }
                    else
                        if (lineCount == maxLineCount - 1)
                        {
                            if (lineCount == 0) textModified += textOrig.Substring(0, ellipsisPos + 3);
                            else textModified += "\n" + textOrig.Substring(0, ellipsisPos + 3);
                            lineCount++;
                            textOrig = "";
                            if (modifiedTextSize.Width < textSize.Width) modifiedTextSize.Width = textSize.Width;
                            modifiedTextSize.Height += textSize.Height;
                            break;
                        }
                        else
                        {
                            int ellipsisPosOrig = ellipsisPos;
                            ellipsisPos--;
                            while (ellipsisPos > 0)
                            {
                                if (Char.IsWhiteSpace(textOrig[ellipsisPos]) || Char.IsSeparator(textOrig[ellipsisPos]))
                                {
                                    ellipsisPos++;
                                    break;
                                }
                                ellipsisPos--;
                            }
                            if (ellipsisPos == 0) ellipsisPos = ellipsisPosOrig;
                            string currLine = textOrig.Substring(0, ellipsisPos);
                            if (lineCount == 0) textModified += currLine;
                            else textModified += "\n" + currLine;
                            lineCount++;
                            currOrigTextPos += currLine.Length;
                            if (newLineReplaced) currOrigTextPos++;
                            textOrig = new String(text.ToCharArray(currOrigTextPos, text.Length - currOrigTextPos));
                            textSize = TextRenderer.MeasureText(currLine, font, maxTextSize, TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.GlyphOverhangPadding);
                            if (modifiedTextSize.Width < textSize.Width) modifiedTextSize.Width = textSize.Width;
                            modifiedTextSize.Height += textSize.Height;
                        }
                }
            }
            return modifiedTextSize;
        }
        #endregion

        #region public override Size GetPreferredSize(Size proposedSize)
        /// <summary>
        /// Calculate control's preferred size for 'proposedSize' (maximum size).
        /// </summary>
        /// <param name="proposedSize"></param>
        /// <returns></returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (MaximumSize.Width > 0) proposedSize.Width = MaximumSize.Width;

            Size prefSize = new Size(Padding.Left + imglChevrons.ImageSize.Width + textLeftMargin + Padding.Right,
                                     Padding.Top + imglChevrons.ImageSize.Height + Padding.Bottom);
            int textTopMargin = 0;
            if (imglChevrons.ImageSize.Height > Font.Height) textTopMargin = (imglChevrons.ImageSize.Height - Font.Height) / 2;
            Size maxTextSize = new Size(proposedSize.Width - prefSize.Width, proposedSize.Height - (Padding.Top + textTopMargin + Padding.Bottom));

            //
            // Check cache ('htDisplayTextSizes') if there's already calculated display text size.
            //
            string displayText;
            Size displayTextSize;
            Size maxTextSizeLineCount = new Size(maxTextSize.Width, maxLineCount);
            if (htDisplayTextSizes.ContainsKey(maxTextSizeLineCount))
            {
                DisplayTextSize displayTS = (DisplayTextSize)htDisplayTextSizes[maxTextSizeLineCount];
                displayText = displayTS.text;
                displayTextSize = displayTS.size;
            }
            else
            {
                displayTextSize = ChevronButton.MeasureTextWordWrap(Text, out displayText, Font, maxTextSize.Width, maxLineCount);
                htDisplayTextSizes.Add(new Size(maxTextSize.Width, maxLineCount), new DisplayTextSize(displayText, displayTextSize));
            }
            
            
            //displayTextSize = MeasureTextWordWrap(Text, out displayText, Font, maxTextSize.Width, maxLineCount);

            prefSize.Width += displayTextSize.Width;
            if (displayTextSize.Height > 0)
            {
                if (displayTextSize.Height + textTopMargin > imglChevrons.ImageSize.Height) prefSize.Height += (displayTextSize.Height + textTopMargin) - imglChevrons.ImageSize.Height;
            }
            return prefSize;
        }
        #endregion

        #region protected override void OnPaint(PaintEventArgs pevent)
        /// <summary>
        /// Draws control.
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            Rectangle rect = ClientRectangle;

            SolidBrush sb = new SolidBrush(BackColor);
            g.FillRectangle(sb, rect);
            
            if (!(RightToLeft == RightToLeft.Yes)) g.DrawImage(imglChevrons.Images[ImageIndex], Padding.Left, Padding.Top);
            else g.DrawImage(imglChevrons.Images[ImageIndex], rect.Width - (Padding.Right + imglChevrons.ImageSize.Width), Padding.Top);

            int textTopMargin = 0;
            if (imglChevrons.ImageSize.Height > Font.Height) textTopMargin = (imglChevrons.ImageSize.Height - Font.Height) / 2;
            Size maxTextSize = new Size(rect.Width - (Padding.Left + imglChevrons.ImageSize.Width + textLeftMargin + Padding.Right), rect.Height - (Padding.Top + textTopMargin + Padding.Bottom));
            Rectangle textRect = new Rectangle(new Point(Padding.Left + imglChevrons.ImageSize.Width + textLeftMargin, Padding.Top + textTopMargin), maxTextSize);

            //
            // Check cache ('htDisplayTextSizes') for already calculated display text size.
            //
            string displayText;
            Size displayTextSize;
            Size maxTextSizeLineCount = new Size(maxTextSize.Width, maxLineCount);
            if (htDisplayTextSizes.ContainsKey(maxTextSizeLineCount))
            {
                DisplayTextSize displayTS = (DisplayTextSize)htDisplayTextSizes[maxTextSizeLineCount];
                displayText = displayTS.text;
                displayTextSize = displayTS.size;
            }
            else
            {
                // This shouldn't happen because 'GetPreferredSize(...)' is called before this method.
                displayTextSize = ChevronButton.MeasureTextWordWrap(Text, out displayText, Font, maxTextSize.Width, maxLineCount);
                htDisplayTextSizes.Add(new Size(maxTextSize.Width, maxLineCount), new DisplayTextSize(displayText, displayTextSize));
            }
            
            TextFormatFlags formatFlags = TextFormatFlags.TextBoxControl | TextFormatFlags.GlyphOverhangPadding;
            if (maxLineCount == Int32.MaxValue) formatFlags |= TextFormatFlags.WordBreak;
            if (!ShowKeyboardCues) formatFlags |= TextFormatFlags.HidePrefix;
            if (RightToLeft == RightToLeft.Yes)
            {
                formatFlags |= TextFormatFlags.Right | TextFormatFlags.RightToLeft;
                textRect = new Rectangle(new Point(1, textRect.Top), textRect.Size);
            }
            else
            {
                formatFlags |= TextFormatFlags.Left;
            }

            TextRenderer.DrawText(g, displayText, Font, textRect, ForeColor, BackColor, formatFlags);

            if (Focused && ShowFocusCues)
            {
                Rectangle focusRect = new Rectangle(new Point(textRect.Left, textRect.Top), displayTextSize);
                focusRect.Width -= 2;
                ControlPaint.DrawFocusRectangle(g, focusRect);
            }
        }
        #endregion

        #region protected override void OnMouseEnter(EventArgs e)
        /// <summary>
        /// Handles hover effect.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            SetImage();
            base.OnMouseEnter(e);
        }
        #endregion

        #region protected override void OnMouseLeave(EventArgs e)
        /// <summary>
        /// Handles hover effect.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            isMouseDown = false;
            SetImage();
            base.OnMouseLeave(e);
        }
        #endregion

        #region protected override void OnMouseDown(MouseEventArgs mevent)
        /// <summary>
        /// Handles pressed effect caused by mouse.
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                SetImage();
            }
            base.OnMouseDown(mevent);
        }
        #endregion

        #region protected override void OnMouseMove(MouseEventArgs mevent)
        /// <summary>
        /// Handles pressed effect caused by mouse.
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
                        SetImage();
                    }
                }
                else
                if (!isMouseDown)
                {
                    isMouseDown = true;
                    SetImage();
                }
            }
            base.OnMouseMove(mevent);
        }
        #endregion

        #region protected override void OnKeyDown(KeyEventArgs kevent)
        /// <summary>
        /// Handles pressed effect caused by keyboard (space key).
        /// </summary>
        /// <param name="kevent"></param>
        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space)
            {
                isKeyDown = true;
                SetImage();
            }
            base.OnKeyDown(kevent);
        }
        #endregion

        #region protected override void OnKeyUp(KeyEventArgs kevent)
        /// <summary>
        /// Handles pressed effect caused by keyboard (space key).
        /// </summary>
        /// <param name="kevent"></param>
        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (isKeyDown && kevent.KeyCode == Keys.Space)
            {
                isKeyDown = false;
                SetImage();
            }
            base.OnKeyUp(kevent);
        }
        #endregion

        #region protected override void OnClick(EventArgs e)
        /// <summary>
        /// Changes chevron button state to expanded or collapsed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            isExpanded = !isExpanded;
            isMouseDown = false;
            SetImage();
            base.OnClick(e);
        }
        #endregion
    }


    internal class DisplayTextSize
    {
        public string text;
        public Size size;

        public DisplayTextSize(string text, Size size)
        {
            this.text = text;
            this.size = size;
        }
    }
}

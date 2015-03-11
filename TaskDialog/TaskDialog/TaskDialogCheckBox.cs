
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

namespace WindowsTaskDialog
{
    internal partial class TaskDialogCheckBox : CheckBox
    {
        private int textLeftMargin = 6; //7;
        private int maxLineCount = Int32.MaxValue;
        private string ctrlText = "";
        private int displayTextPaddingTop = 0;
        private int displayTextPaddingBottom = 0;
        private Size imageSize = new Size(0, 0);
        private bool onPaintFakeFocusedText = false;

        private Hashtable htDisplayTextSizes = new Hashtable();



        /// <summary>
        /// Constructor.
        /// </summary>
        public TaskDialogCheckBox()
        {
            InitializeComponent();
            Padding paddingOld = Padding;
            imageSize = base.GetPreferredSize(new Size(Int32.MaxValue, Int32.MaxValue));
            Padding = paddingOld;
            AutoSize = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container"></param>
        public TaskDialogCheckBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            Padding paddingOld = Padding;
            imageSize = base.GetPreferredSize(new Size(Int32.MaxValue, Int32.MaxValue));
            Padding = paddingOld;
            AutoSize = true;
        }



        #region public int TextLeftMargin
        /// <summary>
        /// Horizontal margin between check box image and text. 
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

        #region public int PaddingTop
        /// <summary>
        /// Top padding. ('Padding.Top' value is dynamically modified in order to always display check box image in upper left corner.)
        /// </summary>
        public int PaddingTop
        {
            get { return displayTextPaddingTop; }
            set { displayTextPaddingTop = value; base.OnResize(null); }
        }
        #endregion
        
        #region public int PaddingBottom
        /// <summary>
        /// Bottom padding. ('Padding.Bottom' value is dynamically modified in order to always display check box image in upper left corner.)
        /// </summary>
        public int PaddingBottom
        {
            get { return displayTextPaddingBottom; }
            set { displayTextPaddingBottom = value; base.OnResize(null); }
        }
        #endregion

        #region public override bool Focused
        /// <summary>
        /// Indicates whether control has input focus and prevents control 
        /// from displaying original focus rectangle.
        /// </summary>
        public override bool Focused
        {
            get
            {
                if (onPaintFakeFocusedText) return false;
                else return base.Focused;
            }
        }
        #endregion

        #region public override string Text
        /// <summary>
        /// Sets control text. Returns empty string when 'onPaintFakeFocusedText' is true.
        /// </summary>
        public override string Text
        {
            get
            {
                if (onPaintFakeFocusedText) return "";
                else return base.Text;
            }
            set
            {
                htDisplayTextSizes.Clear();
                ctrlText = value;
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


        
        #region public override Size GetPreferredSize(Size proposedSize)
        /// <summary>
        /// Calculate control's preferred size for 'proposedSize' (maximum size).
        /// </summary>
        /// <param name="proposedSize"></param>
        /// <returns></returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (MaximumSize.Width > 0) proposedSize.Width = MaximumSize.Width;
            
            Size prefSize = new Size(Padding.Left + imageSize.Width + textLeftMargin + Padding.Right,
                                     displayTextPaddingTop + imageSize.Height + displayTextPaddingBottom);
            int textTopMargin = 0;
            if (imageSize.Height > Font.Height) textTopMargin = (imageSize.Height - Font.Height) / 2;
            Size maxTextSize = new Size(proposedSize.Width - prefSize.Width, proposedSize.Height - (displayTextPaddingTop + textTopMargin + displayTextPaddingBottom));

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
                displayTextSize = ChevronButton.MeasureTextWordWrap(ctrlText, out displayText, Font, maxTextSize.Width, maxLineCount);
                htDisplayTextSizes.Add(new Size(maxTextSize.Width, maxLineCount), new DisplayTextSize(displayText, displayTextSize));
            }
            
            prefSize.Width += displayTextSize.Width;
            if (displayTextSize.Height > 0 && displayTextSize.Height + textTopMargin > imageSize.Height)
            {
                prefSize.Height += (displayTextSize.Height + textTopMargin) - imageSize.Height;
            }

            return prefSize;
        }
        #endregion

        #region protected override void OnLayout(LayoutEventArgs levent)
        /// <summary>
        /// Adjust Padding.Bottom in order to always display check box image in upper left corner.
        /// </summary>
        /// <param name="levent"></param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            int paddingTop = (Font.Height - imageSize.Height) / 2;
            if (paddingTop < 0) paddingTop = 0;
            Padding = new Padding(Padding.Left, paddingTop, Padding.Right, Height - (paddingTop + imageSize.Height));
        }
        #endregion

        #region protected override void OnPaint(PaintEventArgs pevent)
        /// <summary>
        /// Draws control and focus rectangle when needed.
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            Rectangle rect = ClientRectangle;

            onPaintFakeFocusedText = true;
            base.OnPaint(pevent);
            onPaintFakeFocusedText = false;

            int textTopMargin = 0;
            if (imageSize.Height > Font.Height) textTopMargin = (imageSize.Height - Font.Height) / 2;
            Size maxTextSize = new Size(rect.Width - (Padding.Left + imageSize.Width + textLeftMargin + Padding.Right), rect.Height - (displayTextPaddingTop + textTopMargin + displayTextPaddingBottom));
            Rectangle textRect = new Rectangle(new Point(Padding.Left + imageSize.Width + textLeftMargin, displayTextPaddingTop + textTopMargin), maxTextSize);

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
                displayTextSize = ChevronButton.MeasureTextWordWrap(ctrlText, out displayText, Font, maxTextSize.Width, maxLineCount);
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
    }
}

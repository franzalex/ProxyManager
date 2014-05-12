/*
 * Original code by Matthew Noonan. (http://www.codeproject.com/kb/edit/promptedtextbox.asp)
 * Subsequent implementation of code suggested by k^n and additional modifications by Alex Essilfie.
 */

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    /// Draws a textbox with a prompt inside of it, similar to the "Quick Search" box 
    /// in Outlook 2007, IE7 or the Firefox 2.0 search box. The prompt will disappear when
    /// the Text property contains any value or the ReadOnly property is true. If the Text 
    /// property is empty and the ReadOnly property is false, then the prompt will display
    /// again.
    /// </summary>
    public class WatermarkTextBox : System.Windows.Forms.TextBox
    {
        // Windows message constants
        const int WM_SETFOCUS = 7;
        const int WM_KILLFOCUS = 8;
        const int WM_ERASEBKGND = 14;
        const int WM_PAINT = 15;
        const int OCM_COMMAND = 0x2111; /* OCM_COMMAND = OCM_BASE + WM_COMMAND
                                         * Where:
                                         *   WM_COMMAND = 0x111
                                         *   OCM_BASE   = (WM_USER + 0x1C00)
                                         *   WM_USER    = 0x400
                                         */

        // private internal variables
        private bool _focusSelect = true;
        private bool _drawPrompt = true;
        private string _promptText = String.Empty;
        private Color _promptColor = SystemColors.GrayText;
        private Font _promptFont = null;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <remarks>Uncomment the SetStyle line to activate the OnPaint logic in place of the WndProc logic</remarks>
        public WatermarkTextBox()
        {
            this.ResetPromptFont();
            this.ResetPromptForeColor();
        }

        /// <summary>The prompt text to display when there is nothing in the Text property.</summary>
        [Category("Appearance")]
        [Description("The prompt text to display when there is nothing in the Text property.")]
        [DefaultValue("")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, " +
                "Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
                typeof(System.Drawing.Design.UITypeEditor)), Localizable(true)]
        public string PromptText
        {
            get { return _promptText; }
            set { _promptText = value.Trim(); this.Invalidate(); }
        }

        /// <summary>The ForeColor to use when displaying the PromptText.</summary>
        [Category("Appearance"), Description("The ForeColor to use when displaying the PromptText.")]
        [AmbientValue(typeof(Color), "Empty")]
        public Color PromptForeColor
        {
            get
            {
                if (_promptColor == Color.Empty)
                {
                    _promptColor = SystemColors.GrayText;
                }
                return _promptColor;
            }
            set
            {
                _promptColor = value;
                this.Invalidate();
            }
        }

        /// <summary>The Font to use when displaying the PromptText.</summary>
        [Category("Appearance"), Description("The Font to use when displaying the PromptText.")]
        [AmbientValue(null)]
        public Font PromptFont
        {
            [DebuggerStepThrough]
            get
            {
                return _promptFont;
            }
            [DebuggerStepThrough]
            set
            {
                if (value == null) value = this.Font;
                _promptFont = value;
                this.Invalidate();
            }
        }

        /// <summary>Automatically select the text when control receives the focus.</summary>
        [Category("Behavior")]
        [Description("Automatically select the text when control receives the focus.")]
        [DefaultValue(true)]
        public bool SelectOnFocus
        {
            get { return _focusSelect; }
            set { _focusSelect = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only.
        /// </summary>
        [DefaultValue(false), Category("Behavior")]
        public new bool ReadOnly
        {
            [DebuggerStepThrough]
            get { return base.ReadOnly; }
            [DebuggerStepThrough]
            set
            {
                base.ReadOnly = value;
                SetDrawPrompt();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        [DefaultValue(true), Category("Behavior")]
        public new bool Enabled
        {
            [DebuggerStepThrough]
            get { return base.Enabled; }
            [DebuggerStepThrough]
            set
            {
                base.Enabled = value;
                SetDrawPrompt();
            }
        }

        /// <summary>
        /// When the textbox receives an OnEnter event, select all the text if any text is present
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            if (this.Text.Length > 0 && _focusSelect)
                this.SelectAll();

            base.OnEnter(e);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.KeyDown event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.KeyEventArgs that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.KeyUp event.
        /// </summary>
        /// <param name="e">A System.Windows.Forms.KeyEventArgs that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            //process Ctrl key shortcuts.
            if (this.ShortcutsEnabled)
            {
                switch (e.KeyData)
                {
                    case (Keys.Control | Keys.A):
                        this.SelectAll();
                        break;

                    case (Keys.Control | Keys.C):
                        if (this.PasswordChar == new char())
                        {
                            this.Copy();
                        }
                        break;

                    case (Keys.Control | Keys.X):
                        if (this.PasswordChar == new char())
                        {
                            this.Cut();
                        }
                        break;

                    case (Keys.Control | Keys.V):
                        this.Paste();
                        break;

                    case (Keys.Control | Keys.Z):
                        this.Undo();
                        break;

                    default:
                        base.OnKeyUp(e);
                        break;
                }
            }
        }

        /// <summary>Redraw the control when the text alignment changes</summary>
        protected override void OnTextAlignChanged(EventArgs e)
        {
            base.OnTextAlignChanged(e);
            this.Invalidate();
        }

        /// <summary>Control when the prompt will be showed.</summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            // if no text to draw. else draw
            //_drawPrompt = (Text.Length == 0 && base.ReadOnly == false);
            SetDrawPrompt();
        }

        /// <summary>
        /// Redraw the control with the prompt
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>This event will only fire if ControlStyles.UserPaint is set to true in the constructor</remarks>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Only draw the prompt in the OnPaint event and when the Text property is empty
            if (_drawPrompt)// && this.Text.Length == 0)
                DrawTextPrompt(e.Graphics);
        }

        /// <summary>
        /// Overrides the default WndProc for the control
        /// </summary>
        /// <param name="m">The Windows message structure</param>
        /// <remarks>
        /// This technique is necessary because the OnPaint event seems to be doing some
        /// extra processing that I haven't been able to figure out.
        /// </remarks>
        [DebuggerStepThrough()]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);

            //Draw the prompt only when the Text becomes empty.
            //The second condition controls when the text is erased.
            if ((m.Msg == WM_PAINT && _drawPrompt && !GetStyle(ControlStyles.UserPaint)) ||
                (m.Msg == OCM_COMMAND && _drawPrompt && !GetStyle(ControlStyles.UserPaint)))
            { DrawTextPrompt(); }
        }

        /// <summary>
        /// Overload to automatically create the Graphics region before drawing the text prompt
        /// </summary>
        /// <remarks>The Graphics region is disposed after drawing the prompt.</remarks>
        protected virtual void DrawTextPrompt()
        {
            using (Graphics g = this.CreateGraphics())
            {
                DrawTextPrompt(g);
            }
        }

        /// <summary>
        /// Draws the PromptText in the TextBox.ClientRectangle using the PromptFont and PromptForeColor
        /// </summary>
        /// <param name="g">The Graphics region to draw the prompt on</param>
        protected virtual void DrawTextPrompt(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.TextBoxControl | TextFormatFlags.EndEllipsis;
            Rectangle rect = this.ClientRectangle;

            // Offset the rectangle based on the HorizontalAlignment, 
            // otherwise the display looks a little strange
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    flags = flags | TextFormatFlags.HorizontalCenter;
                    rect.Offset(0, 1);
                    break;
                case HorizontalAlignment.Left:
                    flags = flags | TextFormatFlags.Left;
                    rect.Offset(1, 1);
                    break;
                case HorizontalAlignment.Right:
                    flags = flags | TextFormatFlags.Right;
                    rect.Offset(0, 1);
                    break;
            }

            // Draw the prompt text using TextRenderer
            Font font = this.PromptFont;

            TextRenderer.DrawText(g, _promptText, font, rect, _promptColor, this.BackColor, flags);
        }

        /// <summary>
        /// Sets the flag for drawing the prompt text.
        /// </summary>
        protected virtual void SetDrawPrompt()
        {
            _drawPrompt = (Text.Length == 0 && ReadOnly == false && Enabled == true);
        }

        #region AmbientValueAttribute Supporting Methods
        public void ResetPromptFont()
        {
            this.PromptFont = this.Font;
        }

        private bool ShouldSerializePromptFont()
        {
            return (this.PromptFont != this.Font);
        }

        public void ResetPromptForeColor()
        {
            this.PromptForeColor = SystemColors.GrayText;
        }

        private bool ShouldSerializePromptForeColor()
        {
            return (this.PromptForeColor != SystemColors.GrayText);
        }

        #endregion
    }
}

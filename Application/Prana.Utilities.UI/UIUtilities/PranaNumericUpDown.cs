using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{

    [DesignerCategory("code")]
    public class PranaNumericUpDown : NumericUpDown
    {
        // reference to the underlying TextBox control
        private TextBox _textbox;

        // reference to the underlying UpDownButtons control
        private Control _upDownButtons;

        /// <summary>
        /// object creator
        /// </summary>
        public PranaNumericUpDown()
            : base()
        {
            try
            {
                // get a reference to the underlying UpDownButtons field
                // Underlying private type is System.Windows.Forms.UpDownBase+UpDownButtons
                _upDownButtons = base.Controls[0];
                if (_upDownButtons == null || _upDownButtons.GetType().FullName != "System.Windows.Forms.UpDownBase+UpDownButtons")
                {
                    throw new ArgumentNullException(this.GetType().FullName + ": Can't find internal UpDown buttons field.");
                }
                // Get a reference to the underlying TextBox field.
                // Underlying private type is System.Windows.Forms.UpDownBase+UpDownButtons
                _textbox = base.Controls[1] as TextBox;
                if (_textbox == null || _textbox.GetType().FullName != "System.Windows.Forms.UpDownBase+UpDownEdit")
                {
                    throw new ArgumentNullException(this.GetType().FullName + ": Can't find internal TextBox field.");
                }
                // add handlers (MouseEnter and MouseLeave events of NumericUpDown
                // are not working properly)
                _textbox.Font = new Font("Verdana", 8.5f);
                _textbox.MouseEnter += _mouseEnter;
                _textbox.MouseLeave += _mouseLeave;
                _textbox.Leave += _textbox_Leave;
                _textbox.KeyPress += _textbox_KeyPress;
                _upDownButtons.MouseEnter += _mouseEnter;
                _upDownButtons.MouseLeave += _mouseLeave;
                base.MouseEnter += _mouseEnter;
                base.MouseLeave += _mouseLeave;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        void _textbox_Leave(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(_textbox.Text))
                    _textbox.Text = "0";
                SetDecimalPoints(_textbox.Text);
                if (Leave != null)
                {
                    Leave(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void _textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Char.IsLetter(e.KeyChar))
                    e.Handled = true;
                if (!String.IsNullOrEmpty(_textbox.Text))
                {
                    decimal value;
                    if (!_textbox.Text.StartsWith(".") && !_textbox.Text.StartsWith("0."))
                    {
                        if (Decimal.TryParse(_textbox.Text, out value))
                        {
                            if (value == Decimal.Zero && Char.IsDigit(e.KeyChar))
                            {
                                _textbox.Text = e.KeyChar.ToString();
                                e.Handled = true;
                                _textbox.SelectionStart = _textbox.Text.Length;
                                _textbox.SelectionLength = 0;
                            }
                            else if (value == Decimal.Zero && e.KeyChar == '.')
                            {
                                _textbox.Text = "0.";
                                e.Handled = true;
                                _textbox.SelectionStart = _textbox.Text.Length;
                                _textbox.SelectionLength = 0;
                            }
                            else if (ShowCommaSeperatorOnEditing)
                            {
                                if (_textbox.SelectionLength > 0)
                                {
                                    return;
                                }
                                var oldindex = _textbox.SelectionStart;
                                int oldlen = _textbox.TextLength;
                                if (e.KeyChar == '.' && _textbox.Text.Contains("."))
                                    _textbox.Text = _textbox.Text;
                                else if (e.KeyChar == '.' && _textbox.Text.Contains(",") && oldlen != oldindex)//handle the case where decimal is being placed before the comma
                                {
                                    string temp = _textbox.Text.Insert(oldindex, e.KeyChar.ToString());
                                    temp = temp.Replace(",", "");
                                    _textbox.Text = decimal.Parse(temp, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint).ToString("#,####0.###########");
                                }
                                else
                                    _textbox.Text = decimal.Parse(_textbox.Text.Insert(oldindex, e.KeyChar.ToString()), NumberStyles.AllowThousands).ToString("#,####0.###########");

                                int newlen = _textbox.TextLength;
                                if (newlen - oldlen >= 2)//To move the cursor after last updated value old index is incremented by 2 so to incorporate comma after format set
                                {
                                    _textbox.SelectionStart = oldindex + (newlen - oldlen);
                                }
                                else
                                {
                                    _textbox.SelectionStart = oldindex + 1;
                                }
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
                if (_upDownButtons.Visible == false)
                {
                    e.Graphics.Clear(this.BackColor);
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            base.OnPaint(e);
        }


        /// <summary>
        /// WndProc override to kill WN_MOUSEWHEEL message
        /// </summary>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_MOUSEWHEEL = 0x20a;

            try
            {
                if (m.Msg == WM_MOUSEWHEEL)
                {

                    switch (_interceptMouseWheel)
                    {

                        case InterceptMouseWheelMode.Always:
                            // standard message
                            base.WndProc(ref m);
                            break;
                        case InterceptMouseWheelMode.WhenMouseOver:
                            if (_mouseOver)
                            {
                                // standard message
                                base.WndProc(ref m);
                            }
                            break;
                        case InterceptMouseWheelMode.Never:
                            // kill the message
                            return;
                    }

                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        protected override void OnValueChanged(EventArgs e)
        {
            try
            {
                SetDecimalPoints(Value.ToString());
                base.OnValueChanged(e);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region New properties

        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Automatically select control text when it receives focus.")]
        public bool AutoSelect { get; set; }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Automatically removes thousands seperator when control is focussed, only works when AllowThousandSeperator is set to true")]
        public bool RemoveThousandSeperatorOnEdit
        {
            get { return _removeThousandSeperatorOnEdit; }
            set
            {
                _removeThousandSeperatorOnEdit = value;
            }
        }
        private bool _removeThousandSeperatorOnEdit = true;


        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Sets thousand seperator for the control")]
        public bool AllowThousandSeperator
        {
            get { return _allowThousandSeperator; }
            set
            {
                _allowThousandSeperator = value;
                base.ThousandsSeparator = value;
            }
        }

        private bool _allowThousandSeperator = false;


        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Add Thousand Comma Separator property in the specific numeric updown while editing")]
        public bool ShowCommaSeperatorOnEditing
        {
            get { return _showCommaSeperatorOnEditing; }
            set
            {
                _showCommaSeperatorOnEditing = value;
            }
        }
        private bool _showCommaSeperatorOnEditing = false;


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get { return _textbox.SelectionStart; }
            set { _textbox.SelectionStart = value; }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get { return _textbox.SelectionLength; }
            set { _textbox.SelectionLength = value; }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText
        {
            get { return _textbox.SelectedText; }
            set { _textbox.SelectedText = value; }
        }


        [DefaultValue(typeof(InterceptMouseWheelMode), "Always")]
        [Category("Behavior")]
        [Description("Enables MouseWheel only under certain conditions.")]
        public InterceptMouseWheelMode InterceptMouseWheel
        {
            get { return _interceptMouseWheel; }
            set { _interceptMouseWheel = value; }
        }

        private InterceptMouseWheelMode _interceptMouseWheel = InterceptMouseWheelMode.Always;

        public enum InterceptMouseWheelMode
        {
            /// <summary>MouseWheel always works (defauld behavior)</summary>
            Always,
            /// <summary>MouseWheel works only when mouse is over the (focused) control</summary>
            WhenMouseOver,
            /// <summary>MouseWheel never works</summary>
            Never
        }


        [Category("Behavior")]
        [DefaultValue(typeof(ShowUpDownButtonsMode), "Always")]
        [Description("Set UpDownButtons visibility mode.")]
        public ShowUpDownButtonsMode ShowUpDownButtons
        {
            get { return _showUpDownButtons; }
            set
            {
                _showUpDownButtons = value;
                // update UpDownButtons visibility
                UpdateUpDownButtonsVisibility();
            }
        }

        private ShowUpDownButtonsMode _showUpDownButtons = ShowUpDownButtonsMode.Always;

        public enum ShowUpDownButtonsMode
        {
            /// <summary>UpDownButtons are always visible (defauld behavior)</summary>
            Always,
            /// <summary>UpDownButtons are visible only when mouse is over the control</summary>
            WhenMouseOver,
            /// <summary>UpDownButtons are visible only when control has focus</summary>
            WhenFocus,
            /// <summary>UpDownButtons are visible when control has focus or mouse is over it</summary>
            WhenFocusOrMouseOver,
            /// <summary>UpDownButtons are never visible</summary>
            Never,
        }

        /// <summary>
        /// If set, incrementing value will cause it to restart from Minimum 
        /// when Maximum is reached (and viceversa).
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("If set, incrementing value will cause it to restart from Minimum when Maximum is reached (and viceversa).")]
        public bool WrapValue
        {
            get { return _wrapValue; }
            set { _wrapValue = value; }
        }
        private bool _wrapValue = false;

        [Bindable(false)]
        [DefaultValue(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool ThousandsSeparator
        {
            get { return base.ThousandsSeparator; }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Enable Up Down Button")]
        public bool UpDownButtonEnabled
        {
            set
            {
                _upDownButtons.Enabled = value;
            }
        }

        #endregion


        #region Text selection

        // select all the text on focus enter
        protected override void OnGotFocus(System.EventArgs e)
        {
            try
            {
                _haveFocus = true;
                if (RemoveThousandSeperatorOnEdit && AllowThousandSeperator)
                {
                    base.ThousandsSeparator = false;
                }
                if (AutoSelect)
                {
                    _textbox.SelectAll();
                }
                // Update UpDownButtons visibility
                if (_showUpDownButtons == ShowUpDownButtonsMode.WhenFocus | _showUpDownButtons == ShowUpDownButtonsMode.WhenFocusOrMouseOver)
                {
                    UpdateUpDownButtonsVisibility();
                }
                base.OnGotFocus(e);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        // indicate that we have lost the focus
        protected override void OnLostFocus(EventArgs e)
        {
            try
            {
                _haveFocus = false;
                if (RemoveThousandSeperatorOnEdit && AllowThousandSeperator)
                {
                    base.ThousandsSeparator = true;
                }
                // Update UpDownButtons visibility
                if (_showUpDownButtons == ShowUpDownButtonsMode.WhenFocus | _showUpDownButtons == ShowUpDownButtonsMode.WhenFocusOrMouseOver)
                {
                    UpdateUpDownButtonsVisibility();
                }
                base.OnLostFocus(e);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion


        #region Additional events

        // these events will be raised correctly, when mouse enters on the textbox
        public new event EventHandler<EventArgs> MouseEnter;
        public new event EventHandler<EventArgs> MouseLeave;

        public new event EventHandler<EventArgs> Leave;

        // Events raised BEFORE value decrement(increment
        public event CancelEventHandler BeforeValueDecrement;
        public event CancelEventHandler BeforeValueIncrement;

        // flag to track mouse position
        private bool _mouseOver = false;

        // flag to track focus
        private bool _haveFocus = false;

        // this handler is called at each mouse Enter/Leave movement
        private void _mouseEnter(object sender, System.EventArgs e)
        {
            try
            {
                Rectangle cr = RectangleToScreen(ClientRectangle);
                Point mp = MousePosition;

                // actual state
                bool isOver = cr.Contains(mp);

                // test if status changed
                if (_mouseOver ^ isOver)
                {
                    // update state
                    _mouseOver = isOver;
                    if (_mouseOver)
                    {
                        if (MouseEnter != null)
                        {
                            MouseEnter(this, EventArgs.Empty);
                        }
                    }
                }

                // update UpDownButtons visibility
                if (_showUpDownButtons != ShowUpDownButtonsMode.Always)
                {
                    UpdateUpDownButtonsVisibility();
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void _mouseLeave(object sender, System.EventArgs e)
        {
            try
            {
                Rectangle cr = RectangleToScreen(ClientRectangle);
                Point mp = MousePosition;

                // actual state
                bool isOver = cr.Contains(mp);
                // test if status changed
                if (_mouseOver ^ isOver)
                {
                    // update state
                    _mouseOver = isOver;
                    if (!_mouseOver)
                    {
                        //SetDecimalPoints();
                        if (MouseLeave != null)
                        {
                            MouseLeave(this, EventArgs.Empty);
                        }
                    }
                }

                // update UpDownButtons visibility
                if (_showUpDownButtons != ShowUpDownButtonsMode.Always)
                {
                    UpdateUpDownButtonsVisibility();
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void SetDecimalPoints(string value)
        {
            try
            {
                decimal textboxValue;
                _textbox.Text = TrimTrailingZeroes(value);
                if (Decimal.TryParse(value, out textboxValue))
                {
                    decimal decimalValue = Convert.ToDecimal(textboxValue) - Decimal.Floor(Convert.ToDecimal(textboxValue));
                    base.DecimalPlaces = decimalValue == Decimal.Zero ? 0 : BitConverter.GetBytes(Decimal.GetBits(Convert.ToDecimal(textboxValue))[3])[2];
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        public static string TrimTrailingZeroes(string strdec)
        {
            return strdec.Contains(".") ? strdec.TrimEnd('0').TrimEnd('.') : strdec;
        }

        #region Value increment/decrement management

        // raises the two new events
        public override void DownButton()
        {
            try
            {
                if (base.ReadOnly) return;
                CancelEventArgs e = new CancelEventArgs();
                if (BeforeValueDecrement != null)
                {
                    BeforeValueDecrement(this, e);
                }
                if (e.Cancel) return;

                SetDecimalPracesWhileIncrementDecrement();
                if (_wrapValue && Value - Increment < Minimum)
                {
                    Value = Maximum;
                }
                else
                {
                    base.DownButton();
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        public override void UpButton()
        {
            try
            {
                if (base.ReadOnly) return;
                CancelEventArgs e = new CancelEventArgs();
                if (BeforeValueIncrement != null)
                {
                    BeforeValueIncrement(this, e);
                }
                if (e.Cancel) return;

                SetDecimalPracesWhileIncrementDecrement();
                if (_wrapValue && Value + Increment > Maximum)
                {
                    Value = Minimum;
                }
                else
                {
                    base.UpButton();
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetDecimalPracesWhileIncrementDecrement()
        {
            try
            {
                int decimalPlacesNumber = BitConverter.GetBytes(Decimal.GetBits(Value)[3])[2];
                int decimalPlacesIncrement = BitConverter.GetBytes(Decimal.GetBits(Increment)[3])[2];
                if (decimalPlacesIncrement > decimalPlacesNumber)
                {
                    base.DecimalPlaces = decimalPlacesIncrement;
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion


        #region UpDownButtons visibility management

        /// <summary>
        /// Show or hide the UpDownButtons, according to ShowUpDownButtons property value
        /// </summary>
        public void UpdateUpDownButtonsVisibility()
        {
            // test new state
            try
            {
                bool newVisible = false;
                switch (_showUpDownButtons)
                {
                    case ShowUpDownButtonsMode.WhenMouseOver:
                        newVisible = _mouseOver;
                        break;
                    case ShowUpDownButtonsMode.WhenFocus:
                        newVisible = _haveFocus;
                        break;
                    case ShowUpDownButtonsMode.WhenFocusOrMouseOver:
                        newVisible = _mouseOver | _haveFocus;
                        break;
                    case ShowUpDownButtonsMode.Never:
                        newVisible = false;
                        break;
                    default:
                        newVisible = true;
                        break;
                }

                // assign only if needed
                if (_upDownButtons.Visible != newVisible)
                {
                    if (newVisible)
                    {
                        _textbox.Width = this.ClientRectangle.Width - _upDownButtons.Width;
                    }
                    else
                    {
                        _textbox.Width = this.ClientRectangle.Width;
                    }
                    _upDownButtons.Visible = newVisible;
                    OnTextBoxResize(_textbox, EventArgs.Empty);
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }

        }


        /// <summary>
        /// Custom textbox size management
        /// </summary>
        protected override void OnTextBoxResize(object source, System.EventArgs e)
        {
            try
            {
                if (_textbox == null)
                    return;
                if (_showUpDownButtons == ShowUpDownButtonsMode.Always)
                {
                    // standard management
                    base.OnTextBoxResize(source, e);
                }
                else
                {
                    // custom management

                    // change position if RTL
                    bool fixPos = this.RightToLeft == RightToLeft.Yes ^ this.UpDownAlign == LeftRightAlignment.Left;

                    if (_mouseOver)
                    {
                        _textbox.Width = this.ClientSize.Width - _textbox.Left - _upDownButtons.Width - 2;
                        if (fixPos)
                            _textbox.Location = new Point(16, _textbox.Location.Y);
                    }
                    else
                    {
                        if (fixPos)
                            _textbox.Location = new Point(2, _textbox.Location.Y);
                        _textbox.Width = this.ClientSize.Width - _textbox.Left - 2;
                    }

                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        #endregion

    }
}


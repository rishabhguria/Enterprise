using Prana.Global.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// Summary description for Spinner.
    /// </summary>
    /// 

    public class Spinner : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.Misc.UltraButton btnUp;
        private Infragistics.Win.Misc.UltraButton btnDown;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtbx;
        private double _increment = double.Parse("1.00");
        private double _minValue = double.Parse("1.00");
        private double _maxValue = double.Parse("99999.00");
        private double _value = double.Parse("1.00");
        private bool _isBlankAllowed = false;
        private bool _isBlankValue = false;
        private bool _isCustomValidation = false;

        private DataTypes _dataType = DataTypes.Numeric;
        private double _oldValue = double.Parse("1.00");
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public event EventHandler ValueChanged;

        private int _decimalPoints = int.MaxValue;
        private string _decimalFormat = "{0:#,0.###############}";
        public int DecimalPoints
        {
            get { return _decimalPoints; }
            set
            {
                _decimalPoints = value;
                if (_decimalPoints != int.MaxValue)
                {
                    _decimalFormat = "{0:#,0.";
                    for (int i = 0; i < _decimalPoints; i++)
                        _decimalFormat += "#";
                    _decimalFormat += "}";
                }
            }
        }

        private bool _decimalentered = false;

        public bool DecimalEntered
        {
            get { return _decimalentered; }
            set { _decimalentered = value; }
        }

        public Spinner()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            if (_dataType == DataTypes.Integer)//  &&  RegularExpressionValidation.IsInteger(txtbx.Text))
            {
                txtbx.Text = ((int)_value).ToString();
            }
            else
            {
                txtbx.Text = _value.ToString();
            }

            if (CustomThemeHelper.ApplyTheme)
            {
                this.btnDown.Appearance.Image = global::Prana.Utilities.UI.Properties.Resources.appearance3;
                this.btnUp.Appearance.Image = global::Prana.Utilities.UI.Properties.Resources.appearance4;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (btnUp != null)
                    btnUp.Dispose();

                if (btnDown != null)
                    btnDown.Dispose();

                if (txtbx != null)
                    txtbx.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Spinner));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.btnDown = new Infragistics.Win.Misc.UltraButton();
            this.txtbx = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnUp = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtbx)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.FontData.SizeInPoints = 10F;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnDown.Appearance = appearance1;
            this.btnDown.Location = new System.Drawing.Point(48, 9);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(18, 11);
            this.btnDown.TabIndex = 17;
            this.btnDown.TabStop = false;
            this.btnDown.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // txtbx
            // 
            this.txtbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.FontData.SizeInPoints = 10F;
            this.txtbx.Appearance = appearance2;
            this.txtbx.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtbx.Location = new System.Drawing.Point(0, 0);
            this.txtbx.Name = "txtbx";
            this.txtbx.Size = new System.Drawing.Size(50, 23);
            this.txtbx.TabIndex = 16;
            this.txtbx.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtbx.ValueChanged += new System.EventHandler(this.txtbx_ValueChanged);
            this.txtbx.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.txtbx_ControlAdded);
            this.txtbx.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.txtbx_ControlRemoved);
            this.txtbx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbx_KeyDown);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.SizeInPoints = 10F;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.btnUp.Appearance = appearance3;
            this.btnUp.Location = new System.Drawing.Point(48, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(18, 11);
            this.btnUp.TabIndex = 18;
            this.btnUp.TabStop = false;
            this.btnUp.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // Spinner
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.txtbx);
            this.Name = "Spinner";
            this.Size = new System.Drawing.Size(66, 20);
            this.Enter += new System.EventHandler(this.Spinner_Enter);
            this.Leave += new System.EventHandler(this.Spinner_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.txtbx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }



        private void CustomhandlingForOutOfRange(string txtValue)
        {
            if (_isCustomValidation && RegularExpressionValidation.IsNumber(txtValue))
            {
                double number = Double.Parse(txtValue);// Convert.ToDouble(strText);

                if (number <= _maxValue && number >= _minValue)
                {
                    if (DecimalPoints != int.MaxValue)
                    {
                        _oldValue = Math.Round(number, DecimalPoints);
                    }
                    else
                    {
                        _oldValue = number;
                    }
                }
                else
                {
                    txtbx.Text = number > _maxValue ? _maxValue.ToString() : _minValue.ToString(); ;
                }
                _value = Double.Parse(txtbx.Text);
            }
        }
        #endregion

        private void btnUp_Click(object sender, System.EventArgs e)
        {
            Increase();
        }
        private void Increase()
        {
            double number = int.MinValue;
            string txtValue = txtbx.Text.Trim();
            ///Rahul 20120305  Details : http://jira.nirvanasolutions.com:8080/browse/PRANA-910
            if (txtValue.Equals(string.Empty) || txtValue.Equals(""))
            {
                if (IsBlankAllowed)
                {
                    _oldValue = _minValue;
                    txtbx.Text = _oldValue.ToString();
                    return;
                }
                else
                {
                    txtbx.Text = _oldValue.ToString();
                }
            }

            //if (RegularExpressionValidation.IsNumber(txtbx.Text))
            //{
            if (_dataType == DataTypes.Integer)
            {
                number = int.Parse(txtbx.Text);
            }
            else
            {
                number = Double.Parse(txtbx.Text);
            }

            number = number + _increment;
            if (number <= _maxValue)
            {
                txtbx.Text = number.ToString();
                _oldValue = number;
            }
            // }
        }

        private void btnDown_Click(object sender, System.EventArgs e)
        {
            Decrease();
        }
        private void Decrease()
        {
            double number = int.MinValue;
            string txtValue = txtbx.Text.Trim();
            ///Rahul 20120305  Details : http://jira.nirvanasolutions.com:8080/browse/PRANA-910
            if (txtValue.Equals(string.Empty) || txtValue.Equals(""))
            {
                if (IsBlankAllowed)
                {
                    _oldValue = _maxValue;
                    txtbx.Text = _oldValue.ToString();
                    return;
                }
                else
                {
                    txtbx.Text = _oldValue.ToString();
                }
            }

            //if (RegularExpressionValidation.IsNumber(txtbx.Text))
            //{
            if (_dataType == DataTypes.Integer)
            {
                number = int.Parse(txtbx.Text);
            }
            else
            {
                number = Double.Parse(txtbx.Text);
            }
            number = number - _increment;
            if (number >= _minValue)
            {
                txtbx.Text = number.ToString();
                _oldValue = number;

            }

            // }
        }

        private void txtbx_ValueChanged(object sender, System.EventArgs e)
        {
            string strText = txtbx.Text.Replace(",", "");
            if ((strText == "-." || strText == "-" || strText == "" || strText == "-0" || strText == ".") && _isBlankAllowed)
            {
                txtbx.TextChanged -= txtbx_ValueChanged;
                txtbx.Text = "";
                _isBlankValue = true;
                if (ValueChanged != null)
                {
                    ValueChanged("", EventArgs.Empty);
                }
                txtbx.TextChanged += txtbx_ValueChanged;
                return;
            }
            if (strText == "-." || strText == "-" || strText == "" || strText == "-0" || strText == ".")
            {
                return;
            }
            else
            {
                if (!RegularExpressionValidation.IsNumber(strText) && !_isBlankAllowed)
                {
                    txtbx.Text = _oldValue.ToString();
                    return;
                }
                else if (!RegularExpressionValidation.IsNumber(strText) && _isBlankAllowed)
                {
                    System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-zA-Z])([a-zA-Z0-9]+)$");
                    bool isInCorrect = rgx.IsMatch(strText.Trim());

                    txtbx.TextChanged -= txtbx_ValueChanged;
                    txtbx.Text = isInCorrect ? "" : strText.Trim();
                    txtbx.TextChanged += txtbx_ValueChanged;
                    return;
                }
                else
                {
                    double qty;
                    if (double.TryParse(strText, out qty) && _decimalentered == false)
                    {
                        txtbx.TextChanged -= txtbx_ValueChanged;
                        if (!(strText.EndsWith(".")))
                        {
                            txtbx.Text = String.Format(_decimalFormat, qty);
                            txtbx.SelectionStart = txtbx.Text.Length;
                            txtbx.TextChanged += txtbx_ValueChanged;
                        }
                    }
                }
            }

            switch (_dataType)
            {
                case DataTypes.Numeric:
                    if (!_isCustomValidation)
                    {
                        if (RegularExpressionValidation.IsNumber(strText))
                        {
                            double number = Double.Parse(strText);// Convert.ToDouble(strText);

                            if (number <= _maxValue && number >= _minValue)
                            {
                                if (DecimalPoints != int.MaxValue)
                                {
                                    _oldValue = Math.Round(number, DecimalPoints);
                                }
                                else
                                {
                                    _oldValue = number;
                                }
                            }
                            else
                            {
                                txtbx.Text = _oldValue.ToString();
                            }
                            _value = _oldValue;
                        }
                    }
                    else if (_isCustomValidation)
                    {

                        if (RegularExpressionValidation.IsNumber(strText))
                        {
                            double number = Double.Parse(strText);// Convert.ToDouble(strText);
                            _value = number;
                        }
                    }
                    break;

                case DataTypes.PositiveNumeric:

                    if (RegularExpressionValidation.IsPositiveNumber(strText))
                    {
                        Double number = Double.Parse(strText);

                        if (number <= _maxValue && number >= _minValue)
                        {
                            if (DecimalPoints != int.MaxValue)
                            {
                                _oldValue = Math.Round(number, DecimalPoints);
                            }
                            else
                            {
                                _oldValue = number;
                            }
                        }
                        else
                        {
                            txtbx.Text = _oldValue.ToString();
                        }
                    }
                    else
                    {
                        txtbx.Text = _oldValue.ToString();

                    }

                    _value = _oldValue;
                    break;

                case DataTypes.Integer:

                    if (RegularExpressionValidation.IsInteger(strText))
                    {
                        int number1 = int.Parse(strText);
                        if (number1 <= _maxValue && number1 >= _minValue)
                            _oldValue = number1;
                        else
                            txtbx.Text = _oldValue.ToString();
                    }

                    else
                        txtbx.Text = _oldValue.ToString();
                    _value = _oldValue;
                    break;

                case DataTypes.PositiveInteger:

                    if (RegularExpressionValidation.IsPositiveInteger(txtbx.Text))
                    {
                        int number1 = int.Parse(txtbx.Text);
                        if (number1 <= _maxValue && number1 >= _minValue)
                            _oldValue = number1;

                        else
                            txtbx.Text = _oldValue.ToString();

                    }

                    else
                        txtbx.Text = _oldValue.ToString();
                    _value = _oldValue;
                    break;
            }

            ///Added Rajat 10-07-2006 to be used in rm admin
            ///This event passes the current value of the control to subscriber
            if (ValueChanged != null)
            {
                ValueChanged(_value, EventArgs.Empty);
            }

        }

        public DataTypes DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (_minValue <= value)
                {
                    _maxValue = SetValues(value);
                }

            }
        }

        public double MinValue
        {
            get { return _minValue; }
            set
            {
                if (_maxValue > value)
                {
                    _minValue = SetValues(value);
                }
            }
        }


        public bool IsBlankValue
        {
            get
            {
                if (!_isBlankAllowed)
                    _isBlankValue = false;
                return _isBlankValue;
            }
            set
            {
                _isBlankValue = value;
            }
        }

        public bool IsCustomValidation
        {
            get
            {
                return _isCustomValidation;
            }
            set
            {
                _isCustomValidation = value;
            }
        }

        public double Value
        {
            get
            {
                string strText = txtbx.Text.Trim();
                if ((strText == "-." || strText == "-" || strText == "" || strText == "-0" || strText == ".") && !_isBlankAllowed)
                    _value = Convert.ToDouble("0");
                else if ((strText == "-." || strText == "-" || strText == "" || strText == "-0" || strText == ".") && _isBlankAllowed)
                {
                    _value = Convert.ToDouble("0");
                    _isBlankValue = true;
                }
                else
                    _value = Convert.ToDouble(strText);
                return _value;
            }
            set
            {

                if (value <= _maxValue || value >= _minValue)
                {
                    _value = SetValues(value);
                }
                if (_isBlankAllowed && _isBlankValue)
                    txtbx.Text = "";
                else
                    txtbx.Text = _value.ToString();
            }
        }

        private double SetValues(double setValue)
        {
            switch (_dataType)
            {
                case DataTypes.PositiveNumeric:
                    setValue = Math.Abs(setValue);
                    break;
                case DataTypes.PositiveInteger:
                    setValue = (int)Math.Abs(setValue);

                    break;
                case DataTypes.Integer:
                    setValue = (int)setValue;
                    break;
            }

            return setValue;
        }

        private void txtbx_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyValue == 190 || e.KeyValue == 110) && !txtbx.Text.Contains(".") || ((e.KeyValue == 96 || e.KeyValue == 48) && txtbx.Text.Contains(".")))
                _decimalentered = true;
            else
                _decimalentered = false;

            if (e.KeyCode == System.Windows.Forms.Keys.Down)
                Decrease();

            if (e.KeyCode == System.Windows.Forms.Keys.Up)
                Increase();
        }

        public double Increment
        {
            get { return _increment; }
            set
            {
                _increment = SetValues(value);
            }
        }

        public bool IsBlankAllowed
        {
            get { return _isBlankAllowed; }
            set
            {
                _isBlankAllowed = value;
            }
        }



        private void Spinner_Enter(object sender, EventArgs e)
        {
            // Purpose: Highlight the whole number instead of just go to end of it, PRANA-13097
            this.txtbx.SelectAll();
            this.txtbx.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void Spinner_Leave(object sender, EventArgs e)
        {
            if (txtbx.Text.ToString().EndsWith("."))
            {
                txtbx.Text = txtbx.ToString().Remove(txtbx.ToString().Length - 1);
            }
            CustomhandlingForOutOfRange(txtbx.Text.Replace(",", ""));
            this.txtbx.Appearance.BackColor = Color.White;
        }


        private void txtbx_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick += Control_DoubleClick;
        }

        void Control_DoubleClick(object sender, EventArgs e)
        {
            if (DoubleClickHanle != null)
            {
                DoubleClickHanle(sender, e);
            }
        }

        private void txtbx_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.DoubleClick -= Control_DoubleClick;
        }

        public event EventHandler DoubleClickHanle;

    }
    public enum DataTypes
    {
        Integer,
        PositiveInteger,
        Numeric,
        PositiveNumeric
    }
}

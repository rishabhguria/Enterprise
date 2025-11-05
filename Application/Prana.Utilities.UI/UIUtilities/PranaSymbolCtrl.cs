using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class PranaSymbolCtrl : UserControl
    {
        public event EventHandler<EventArgs<string, string>> SymbolEntered;
        public event EventHandler<EventArgs<string>> SymbolSearch;
        private string _prevSymbolEntered = string.Empty;

        private bool _focused = false;

        public override bool Focused
        {
            get { return _focused; }
        }


        public PranaSymbolCtrl()
        {
            try
            {
                InitializeComponent();
                txtSymbol.Leave += txtSymbol_Leave;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void txtSymbol_Leave(object sender, EventArgs e)
        {

            validateSymbol(txtSymbol.Text);
        }


        public int MaxLength
        {
            set
            {

                txtSymbol.MaxLength = value;
            }
            get
            {
                return txtSymbol.MaxLength;
            }
        }
        public void SelectAll()
        {
            txtSymbol.SelectAll();

        }
        public CharacterCasing CharacterCasing
        {
            set { txtSymbol.CharacterCasing = value; }
            get { return txtSymbol.CharacterCasing; }
        }

        //public AutoCompleteStringCollection AutoCompleteCustomSource
        //{
        //    set { txtSymbol.AutoCompleteCustomSource = value; }
        //    get { return txtSymbol.AutoCompleteCustomSource; }
        //}
        public override string Text
        {
            get
            {
                return txtSymbol.Text == null ? null : txtSymbol.Text.TrimStart();
            }
            set
            {
                txtSymbol.Text = value;
                if (value == null || value == string.Empty)
                {
                    _prevSymbolEntered = string.Empty;
                }
            }
        }


        private void txtSymbol_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                validateSymbol(txtSymbol.Text);
                return;
            }
            if (e.KeyData == Keys.Down || e.KeyData == Keys.Up || e.KeyData == Keys.Left || e.KeyData == Keys.Right)
            {
                return;
            }
            if (txtSymbol.Text == null || txtSymbol.Text.Length == 0)
            {
                txtSymbol.Items.Clear();
                return;
            }
            string text = txtSymbol.Text.TrimStart().ToUpper();
            if (SymbolSearch != null)
            {
                SymbolSearch(this, new EventArgs<string>(text));
            }

        }

        private delegate void DropDownHandle(string startwith, IList<string> items);

        public void updateDropDown(string startwith, IList<string> items)
        {
            if (items == null)
            {
                return;
            }
            if (UIValidation.GetInstance().validate(this))
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new DropDownHandle(updateDropDown), new object[] { startwith, items });
                    return;
                }
            }
            try
            {
                int selectionStart = txtSymbol.SelectionStart;
                int selectionLength = txtSymbol.SelectionLength;
                string text = txtSymbol.Text.TrimStart().ToUpper();
                if (!startwith.Equals(text))
                {
                    return;
                }
                txtSymbol.Items.Clear();
                foreach (string s in items)
                {
                    txtSymbol.Items.Add(" " + s);
                }
                if (items.Count >= 1)
                    txtSymbol.DropDown();
                txtSymbol.Text = text;
                txtSymbol.Select(selectionStart, selectionLength);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        protected void validateSymbol(string symbol)
        {
            if (symbol == null)
            {
                return;
            }
            symbol = symbol.Trim();
            if (SymbolEntered != null && _prevSymbolEntered != symbol)
            {
                _prevSymbolEntered = symbol;
                SymbolEntered(this, new EventArgs<string, string>(symbol, ""));
            }
        }

        private void txtSymbol_SelectionChangeCommitted(object sender, EventArgs e)
        {
            validateSymbol(txtSymbol.Text);
        }

        public bool SymbolFocused
        {
            get { return txtSymbol.Focused; }
        }

        public string PrevSymbolEntered
        {
            get { return _prevSymbolEntered; }
            set { _prevSymbolEntered = value; }
        }

        private void txtSymbol_SelectionChanged(object sender, EventArgs e)
        {
            //txtSymbol.Text = txtSymbol.Value.ToString().Trim();
        }
    }
}


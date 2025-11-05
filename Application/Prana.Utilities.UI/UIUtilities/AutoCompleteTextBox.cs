using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class AutoCompleteTextBox : Infragistics.Win.UltraWinEditors.UltraTextEditor//, IDisposable
    {
        #region Private Members

        private ListBox _listBox;
        private String[] _values;
        private String _formerValue = String.Empty;

        #endregion

        #region Event handlers

        /// <summary>
        /// Hides the _listbox when the focus is lost from the related editbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AutoCompleteTextBox_LostFocus(object sender, EventArgs e)
        {
            Form frm = this.FindForm();
            if (frm != null)
            {
                if (!frm.Controls.Contains(_listBox))
                    frm.Controls.Add(_listBox);
            }
            if (!this.Focused && !(FindFocusedControl(this.FindForm()) == this))
                this._listBox.Visible = false;

        }

        /// <summary>
        /// To find focused control
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Control FindFocusedControl(Control control)
        {
            var container = control as ContainerControl;
            try
            {
                while (container != null)
                {
                    control = container.ActiveControl;
                    container = control as ContainerControl;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return control;
        }

        /// <summary>
        /// updates the listbox when a key is entered in the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateListBox();
        }

        /// <summary>
        /// checks what key is pressed and updates the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (_listBox.Visible)
                        {
                            InsertWord((String)_listBox.SelectedItem);
                            ResetListBox();
                            _formerValue = this.Text;
                        }
                        else
                        {
                            //Use event to send function to form so that default button on form get executed
                            if (this.FindForm().AcceptButton != null)
                                this.FindForm().AcceptButton.PerformClick();
                            this.FindForm().Focus();
                        }
                        break;
                    case Keys.Oemcomma:
                        {
                            if (_listBox.Visible)
                            {
                                InsertWord((String)_listBox.SelectedItem);
                                ResetListBox();
                                _formerValue = this.Text;
                            }
                            break;
                        }
                    case Keys.Tab:
                        if (_listBox.Visible)
                        {
                            InsertWord((String)_listBox.SelectedItem);
                            ResetListBox();
                            _formerValue = this.Text;
                        }
                        //else
                        //{
                        //    //Use event to send function to form so that default button on form get executed
                        //    this.FindForm().Focus();
                        //    System.Windows.Forms.SendKeys.Send(Keys.Tab.ToString());
                        //}
                        break;
                    case Keys.Down:
                        {
                            if ((_listBox.Visible) && (_listBox.SelectedIndex < _listBox.Items.Count - 1))
                            {
                                _listBox.SelectedIndex++;
                            }
                            break;
                        }
                    case Keys.Up:
                        {
                            if ((_listBox.Visible) && (_listBox.SelectedIndex > 0))
                            {
                                _listBox.SelectedIndex--;
                            }
                            break;
                        }
                        //default:
                        //    UpdateListBox();
                        //    break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Private Methods

        void AutoCompleteTextBox_GotFocus(object sender, EventArgs e)
        {
            if (!_listBox.Visible)
            {
                this.SelectAll();
            }

        }



        /// <summary>
        /// shows the listbox by making it visible and finding the current position of the textbox
        /// </summary>
        private void ShowListBox()
        {
            try
            {
                Form frm = this.FindForm();
                if (frm != null)
                {
                    if (!frm.Contains(_listBox))
                        frm.Controls.Add(_listBox);
                }
                _listBox.Location = FindLocation();
                _listBox.Visible = true;
                _listBox.BringToFront();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Function to set location of auto search listbox
        /// </summary>
        /// <returns></returns>
        private Point FindLocation()
        {
            Point loc = new Point(0, 0);
            try
            {
                Control cntr = this;
                while (cntr.Parent != null)
                {
                    loc.X += cntr.Location.X;
                    loc.Y += cntr.Location.Y;
                    cntr = cntr.Parent;
                }
                loc.Y += this.Height;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return loc;
        }

        /// <summary>
        /// hides the listbox
        /// </summary>
        private void ResetListBox()
        {
            _listBox.Visible = false;
            _listBox.MinimumSize = new Size(50, 10);
        }

        /// <summary>
        /// checks whether the argument word is in the values array(both starts with or contains)
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string[] StartsContains(string word)
        {
            List<string> lsString = new List<string>();
            try
            {
                if (_values != null)
                {
                    foreach (string value in _values)
                    {
                        if (value.StartsWith(word, StringComparison.OrdinalIgnoreCase) && !SelectedValues.Contains(value))
                            lsString.Add(value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return (lsString.ToArray());
        }

        /// <summary>
        /// updates the listbox according to the current values in the textbox
        /// </summary>
        private void UpdateListBox()
        {
            try
            {
                if (this.Text != _formerValue)
                {
                    _formerValue = this.Text;
                    String word = GetWord();

                    if (word.Length > 0)
                    {
                        String[] matches = StartsContains(word);
                        //String[] matches = Array.FindAll(_values,x => (x.StartsWith(word) && !SelectedValues.Contains(x)));

                        if (matches.Length > 0)
                        {
                            ShowListBox();
                            _listBox.Items.Clear();
                            foreach (string s in matches)
                            {
                                _listBox.Items.Add(s);
                            }
                            //Array.ForEach(matches, x => _listBox.Items.Add(x));
                            //x => _listBox.Items.Add(x))
                            _listBox.SelectedIndex = 0;
                            _listBox.Height = 0;
                            _listBox.Width = 0;
                            //this.Focus();
                            using (Graphics graphics = _listBox.CreateGraphics())
                            {
                                for (int i = 0; i < _listBox.Items.Count; i++)
                                {
                                    _listBox.Height += _listBox.GetItemHeight(i);
                                    // it item width is larger than the current one
                                    // set it to the new max item width
                                    // GetItemRectangle does not work for me
                                    // we add a little extra space by using '_'
                                    int itemWidth = (int)graphics.MeasureString(((String)_listBox.Items[i]) + "_", _listBox.Font).Width;
                                    _listBox.Width = (_listBox.Width < itemWidth) ? itemWidth : _listBox.Width;
                                }
                            }
                        }
                        else
                        {
                            ResetListBox();
                        }
                    }
                    else
                    {
                        ResetListBox();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// gets the word from the textbox after the last comma
        /// </summary>
        /// <returns></returns>
        private String GetWord()
        {
            String text = this.Text;
            int length = 0;
            int posStart = 0;
            try
            {
                int pos = this.SelectionStart;
                posStart = text.LastIndexOf(',', (pos < 1) ? 0 : pos - 1);
                posStart = (posStart == -1) ? 0 : posStart + 1;
                int posEnd = text.IndexOf(',', pos);
                posEnd = (posEnd == -1) ? text.Length : posEnd;

                length = ((posEnd - posStart) < 0) ? 0 : posEnd - posStart;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return text.Substring(posStart, length).Trim();
        }

        /// <summary>
        /// inserts the selected option to the textbox
        /// </summary>
        /// <param name="newTag"></param>
        private void InsertWord(String newTag)
        {
            try
            {
                String text = this.Text;
                int pos = this.SelectionStart;

                int posStart = text.LastIndexOf(',', (pos < 1) ? 0 : pos - 1);
                posStart = (posStart == -1) ? 0 : posStart + 1;
                int posEnd = text.IndexOf(',', pos);

                String firstPart = text.Substring(0, posStart) + newTag;
                String updatedText = firstPart + ((posEnd == -1) ? "" : text.Substring(posEnd, text.Length - posEnd));


                this.Text = updatedText;
                this.SelectionStart = firstPart.Length;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion

        #region TextBox Override

        protected override void OnBeginInit()
        {
            base.OnBeginInit();
            try
            {
                _listBox = new ListBox();
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.this_KeyDown);
                this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.this_KeyUp);
                this.GotFocus += new EventHandler(AutoCompleteTextBox_GotFocus);
                //this.LostFocus += new EventHandler(AutoCompleteTextBox_LostFocus);
                this.AfterExitEditMode += new EventHandler(AutoCompleteTextBox_LostFocus);
                Form frm = this.FindForm();
                if (frm != null)
                    frm.Controls.Add(_listBox);
                ResetListBox();
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

        /// <summary>
        /// overrides the textbox method IsInputKey to select the input keys
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            try
            {
                switch (keyData)
                {
                    //case Keys.Tab:
                    case Keys.Enter:
                        return true;
                    default:
                        return base.IsInputKey(keyData);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// public property Values
        /// </summary>
        public String[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

        /// <summary>
        /// Public property SelectedValues
        /// </summary>
        public List<String> SelectedValues
        {
            get
            {
                String[] result = Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<String> res = new List<String>(result);
                res.RemoveAt(res.Count - 1);
                return res;
            }
        }

        #endregion


        /// <summary>
        /// Unwires the events
        /// </summary>
        public void UnwireEvents()
        {
            this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.this_KeyDown);
            this.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.this_KeyUp);
            this.LostFocus -= new EventHandler(AutoCompleteTextBox_LostFocus);
        }
    }
}
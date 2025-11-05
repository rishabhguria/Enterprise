using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Nirvana.Middleware;

namespace CSBatchUI
{
    public partial class FundSymbolControl : UserControl
    {
        public FundSymbolControl()
        {
            InitializeComponent();
        }

        private FundUI _Fund;

        public FundUI Fund
        {
            get { return _Fund; }
            set
            {
                if (value == null)
                    return;

                if (_Fund == null)
                {
                    _Fund = value;
                    UpdateUI();
                    return;
                }
                else
                {
                    _Fund.PropertyChanged -= new PropertyChangedEventHandler(_Fund_PropertyChanged);
                    _Fund = value;
                    UpdateUI();
                    _Fund.PropertyChanged += new PropertyChangedEventHandler(_Fund_PropertyChanged);
                }
            }
        }

        public event EventHandler FundChanged;

        void _Fund_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateTextBox();
            ResetListBox();
            if (FundChanged != null)
                FundChanged(this, EventArgs.Empty);
        }

        private void UpdateTextBox()
        {
            textBox1.Text = Fund.Text;
            textBox1.SelectionStart = Fund.SelectionStart;
        }

        private void MementoTextBox()
        {
            Fund.SelectionStart = textBox1.SelectionStart;
            Fund.Text = textBox1.Text;
        }

        private void ResetListBox()
        {
            listBox1.DataSource = null;
            CloseListBox();
        }

        private void CloseListBox()
        {
            ultraPopupControlContainer1.Close();
            textBox1.Focus();
        }

        private void ShowListBox()
        {
            Point location = textBox1.PointToScreen(Point.Empty);
            if (location.X > 0 && location.Y > 0)
            {
                Size size = textBox1.Size;
                Rectangle rect = new Rectangle(location, size);
                ultraPopupControlContainer1.Show(new Point(rect.Left, rect.Bottom));
                listBox1.Focus();
            }
            else
            {

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Combo_KeyDown(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Combo_KeyPress(e);
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            int selectionstart = textBox1.SelectionStart;

            int nextcomma = textBox1.Text.IndexOf(',', textBox1.SelectionStart);
            if (nextcomma != -1)
                textBox1.Select(nextcomma + 1, 0);
            else
                textBox1.Select(textBox1.Text.Length, 0);
        }

        private void Combo_KeyDown(KeyEventArgs e)
        {
            string txt = textBox1.Text.Substring(0, textBox1.SelectionStart);
            int lastcomma = txt.LastIndexOf(',');
            string afterlastcomma = txt.Substring(lastcomma + 1);
            string beforelastcomma = string.Empty;
            if (lastcomma != -1)
                beforelastcomma = txt.Substring(0, lastcomma);

            switch (e.KeyCode)
            {
                case Keys.Oemcomma:
                    textBox1_SelectCurrent(e, afterlastcomma, beforelastcomma);
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Enter:
                    textBox1_SelectCurrent(e, afterlastcomma, beforelastcomma);
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Tab:
                    textBox1_SelectCurrent(e, afterlastcomma, beforelastcomma);
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                    listBox1_NavigateUp(e, afterlastcomma);

                    e.Handled = true;
                    break;
                case Keys.Down:
                    listBox1_NavigateDown(e, afterlastcomma);

                    e.Handled = true;
                    break;
                case Keys.Left:
                    if (textBox1.SelectionStart >= 1)
                    {
                        char selectedchar = textBox1.Text[textBox1.SelectionStart - 1];
                        if (selectedchar == ',')
                        {
                            string[] splitted = txt.Split(',');
                            string[] nonempty = splitted.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
                            string[] taked = splitted.Take(nonempty.Length - 1).ToArray();
                            string removed = string.Join(",", taked);
                            if (!string.IsNullOrWhiteSpace(removed))
                                removed = removed + ",";
                            textBox1.Select(removed.Length, 0);
                        }
                    }

                    e.Handled = true;
                    break;
                case Keys.Right:
                    if (textBox1.SelectionStart >= 1)
                    {
                        char selectedchar = textBox1.Text[textBox1.SelectionStart - 1];
                        if (selectedchar == ',')
                        {
                            int nextcomma = textBox1.Text.IndexOf(',', textBox1.SelectionStart);
                            if (nextcomma != -1)
                                textBox1.Select(nextcomma + 1, 0);
                            else
                                textBox1.Select(textBox1.Text.Length, 0);
                        }
                    }
                    else
                    {
                        int nextcomma = textBox1.Text.IndexOf(',', textBox1.SelectionStart);
                        if (nextcomma != -1)
                            textBox1.Select(nextcomma + 1, 0);
                        else
                            textBox1.Select(textBox1.Text.Length, 0);
                    }

                    e.Handled = true;
                    break;
                case Keys.Insert:
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    e.Handled = true;
                    break;
                case Keys.PageDown:
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void Combo_KeyPress(KeyPressEventArgs e)
        {
            string txt = textBox1.Text.Substring(0, textBox1.SelectionStart);
            int lastcomma = txt.LastIndexOf(',');
            string afterlastcomma = txt.Substring(lastcomma + 1);
            string beforelastcomma = string.Empty;
            if (lastcomma != -1)
                beforelastcomma = txt.Substring(0, lastcomma);

            bool skippedControl = false;

            if (char.IsControl(e.KeyChar))
            {
                if (e.KeyChar != (char)Keys.Back)
                    skippedControl = true;
                else
                {
                    if (string.IsNullOrWhiteSpace(afterlastcomma))
                    {
                        if (!string.IsNullOrWhiteSpace(beforelastcomma))
                        {
                            int lasttrimmedcomma = txt.TrimEnd(',').LastIndexOf(',');
                            textBox1.Text = textBox1.Text.Remove(lasttrimmedcomma + 1, textBox1.SelectionStart - lasttrimmedcomma - 1);
                            textBox1.Select(lasttrimmedcomma + 1, 0);

                            MementoTextBox();
                        }

                        skippedControl = true;
                    }
                }
            }

            if (!skippedControl)
            {
                string updatedText = textBox1.Text;
                int updatedSelectionStart = textBox1.SelectionStart;
                string updatedAfterlastcomma = afterlastcomma;

                if (e.KeyChar == (char)Keys.Back)
                {
                    if (!string.IsNullOrWhiteSpace(afterlastcomma))
                    {
                        updatedText = textBox1.Text.Remove(lastcomma + afterlastcomma.Length, 1);
                        updatedSelectionStart = lastcomma + afterlastcomma.Length;

                        updatedAfterlastcomma = afterlastcomma.Substring(0, afterlastcomma.Length - 1);
                    }
                }
                else
                {
                    updatedText = textBox1.Text.Insert(lastcomma + afterlastcomma.Length + 1, e.KeyChar.ToString());
                    updatedSelectionStart = lastcomma + 1 + afterlastcomma.Length + 1;

                    updatedAfterlastcomma = afterlastcomma + e.KeyChar;
                }

                if (string.IsNullOrWhiteSpace(updatedAfterlastcomma))
                {
                    ResetListBox();

                    textBox1.Text = updatedText;
                    textBox1.Select(updatedSelectionStart, 0);
                }
                else
                {
                    string wholesomeText = textBox1.Text;
                    if (lastcomma > -1)
                        wholesomeText = wholesomeText.Remove(lastcomma, afterlastcomma.Length);

                    string[] splitted = wholesomeText.Split(',').ToArray();

                    string[] splitdata = Fund.Symbols.Where(p => p.StartsWith(updatedAfterlastcomma, StringComparison.OrdinalIgnoreCase)).Except(splitted, new UpperCaseComparer()).ToArray();

                    if (splitdata.Length > 0)
                    {
                        listBox1.DataSource = splitdata;
                        ShowListBox();
                    }

                    textBox1.Text = updatedText;
                    textBox1.Select(updatedSelectionStart, 0);
                }
            }

            e.Handled = true;
        }

        private void listBox1_NavigateDown(KeyEventArgs e, string afterlastcomma)
        {
            if (!string.IsNullOrWhiteSpace(afterlastcomma))
            {
                if (listBox1.SelectedIndex + 1 < listBox1.Items.Count)
                    listBox1.SelectedIndex += 1;
                else
                {
                }
            }
        }

        private void listBox1_NavigateUp(KeyEventArgs e, string afterlastcomma)
        {
            if (!string.IsNullOrWhiteSpace(afterlastcomma))
            {
                if (listBox1.SelectedIndex - 1 >= 0)
                    listBox1.SelectedIndex -= 1;
                else
                {
                }
            }
        }

        private void textBox1_SelectCurrent(KeyEventArgs e, string afterlastcomma, string beforelastcomma)
        {
            if (!string.IsNullOrWhiteSpace(afterlastcomma))
            {
                string selecteditem = string.Empty;
                if (listBox1.SelectedItem != null)
                    selecteditem = listBox1.SelectedItem.ToString();
                if (!string.IsNullOrWhiteSpace(selecteditem))
                {
                    int lasttrimmedcomma = textBox1.Text.Substring(0, textBox1.SelectionStart).TrimEnd(',').LastIndexOf(',');
                    textBox1.Text = textBox1.Text.Remove(lasttrimmedcomma + 1, afterlastcomma.Length);
                    textBox1.Text = textBox1.Text.Insert(lasttrimmedcomma + 1, selecteditem + ",");
                    textBox1.Select(lasttrimmedcomma + 1 + selecteditem.Length + 1, 0);

                    MementoTextBox();
                }
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Combo_KeyDown(e);
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Combo_KeyPress(e);
        }

        private void textBox1_Resize(object sender, EventArgs e)
        {
            listBox1.Width = textBox1.Width;
        }
    }
}

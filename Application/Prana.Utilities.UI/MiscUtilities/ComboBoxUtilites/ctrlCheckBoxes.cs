using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.MiscUtilities
{
    public partial class ctrlCheckBoxes : UserControl, IMultiCombo
    {
        public ctrlCheckBoxes()
        {
            InitializeComponent();
        }

        public void AddCheckBoxes(List<string> list)
        {
            int yAxis = 0;
            foreach (string item in list)
            {
                CheckBox chkBox = new CheckBox();
                chkBox.AutoSize = true;
                chkBox.Text = item;
                chkBox.Location = new Point(5, yAxis);
                chkBox.UseVisualStyleBackColor = true;
                chkBox.Checked = true;
                this.Controls.Add(chkBox);
                yAxis += 20;
            }
        }

        private CloseComboHandler CloseEXTCombo;

        #region IEXTCombo Members

        public void SetUserInterface()
        {
            this.BorderStyle = BorderStyle.None;
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        }

        public CloseComboHandler CloseComboDelegate
        {
            get { return CloseEXTCombo; }
            set { CloseEXTCombo = value; }
        }

        public string DisplayText
        {
            get
            {
                string sRetVal = string.Empty;
                int count = 0;
                foreach (Control ctrl in this.Controls)
                {
                    CheckBox chkBox = (CheckBox)ctrl;
                    if (chkBox.Checked == true)
                    {
                        sRetVal = sRetVal + chkBox.Text + "-";
                        count++;
                    }
                }

                if (sRetVal.Length > 1)
                {
                    sRetVal = sRetVal.Substring(0, sRetVal.Length - 1);
                }
                if (count == this.Controls.Count)
                {
                    sRetVal = "All";
                }

                return sRetVal;
            }
        }
        #endregion

        public List<string> SelectedItemList
        {
            get
            {
                List<string> sItemsList = new List<string>();
                foreach (Control ctrl in this.Controls)
                {
                    CheckBox chkBox = (CheckBox)ctrl;
                    if (chkBox.Checked == true)
                    {
                        sItemsList.Add(chkBox.Text);
                    }
                }

                return sItemsList;
            }
        }
    }
}

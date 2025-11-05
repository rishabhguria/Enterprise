using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class ctrlSaveImportXSLT : UserControl
    {
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        ImportTradeXSLTFileCollection _importTrades = new ImportTradeXSLTFileCollection();

        public ctrlSaveImportXSLT()
        {
            InitializeComponent();
        }

        public void SetUpBinding()
        {
            try
            {
                _importTrades = DataManager.GetImportTradeDetails();
                ultraGrid1.DataSource = _importTrades;

                // Select Column is added 
                UltraGridColumn colSelect = ultraGrid1.DisplayLayout.Bands[0].Columns.Add("Select");
                colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colSelect.Header.Caption = "Select XSLT";
                colSelect.NullText = "Select File";
                ultraGrid1.DisplayLayout.Bands[0].Columns[1].Width = 350;
                ultraGrid1.DisplayLayout.Bands[0].Columns[1].CellActivation = Activation.NoEdit;
                ultraGrid1.DisplayLayout.Bands[0].Columns[2].Width = 65;
                ultraGrid1.DisplayLayout.Bands[0].Columns[0].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ultraGrid1.DisplayLayout.Bands[0].Columns[1].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ultraGrid1.DisplayLayout.Bands[0].Columns[2].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsValid = new bool();
                bool IsUnique = new bool();

                IsValid = true;
                IsUnique = true;

                foreach (ImportTradeXSLTFile item in _importTrades)
                {
                    if (item.EMSSource == null || item.EMSSource == String.Empty)
                    {
                        IsValid = false;
                        MessageBox.Show("Please enter Import Source", "EMS Import");
                        break;
                    }
                    if (item.FileName == null || item.EMSSource == String.Empty)
                    {
                        IsValid = false;
                        MessageBox.Show("Please Select XSLT", "EMS Import");
                        break;
                    }
                    int i = 0;
                    foreach (ImportTradeXSLTFile var in _importTrades)
                    {
                        if (var.EMSSource == item.EMSSource)
                        {
                            i++;
                        }
                    }
                    if (i > 1)
                    {
                        IsUnique = false;
                        MessageBox.Show("EMS Source name should be Unique !", "EMS Import");
                        break;
                    }
                }
                if (IsValid && IsUnique)
                {
                    DataManager.SaveImportTradeDetails(_importTrades);
                    _importTrades = DataManager.GetImportTradeDetails();
                    ultraGrid1.DataSource = _importTrades;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ultraGrid1_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == "Select")
            {
                string title = "Select XSLT that Maps Trades To Prana";
                string shortName = GetFileName(title);
                if (!String.IsNullOrEmpty(shortName))
                {
                    ultraGrid1.ActiveRow.Cells["FileName"].Value = shortName;
                }
            }
        }

        private string GetFileName(string title)
        {
            //openFileDialog1.InitialDirectory = "DeskTop";
            openFileDialog1.Title = title;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt";
            string strFileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileName = openFileDialog1.FileName;
            }
            return strFileName;

        }



        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (ultraGrid1.ActiveRow != null)
            {
                string msgText = "Are you sure you want to Remove" + " " + ultraGrid1.ActiveRow.Cells[0].Text.ToString() + " " + "?";
                string caption = "EMS Import";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(msgText, caption, buttons);

                if (result == DialogResult.Yes)
                {
                    ImportTradeXSLTFile row = (ImportTradeXSLTFile)ultraGrid1.ActiveRow.ListObject;
                    if (row.ImportSourceID != 0 && row.FileID != 0)
                    {
                        bool isRemoved = DataManager.RemoveEntry(row.ImportSourceID, row.FileID);
                        if (isRemoved)
                        {
                            _importTrades.Remove(row);
                            MessageBox.Show("SucessFully Removed !", "EMS Import");
                        }
                        else
                        {
                            MessageBox.Show("EMS Source is in use. Can not Delete !", "EMS Import");
                        }
                    }
                    else
                    {
                        _importTrades.Remove(row);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ImportTradeXSLTFile file = new ImportTradeXSLTFile();
            _importTrades.Add(file);
            ultraGrid1.DataSource = _importTrades;
        }

        private void ctrlSaveImportXSLT_Load(object sender, EventArgs e)
        {
            // btnSave.Enabled = false;
        }

        private void ultraGrid1_Error(object sender, ErrorEventArgs e)
        {
            e.Cancel = true;
        }

    }
}

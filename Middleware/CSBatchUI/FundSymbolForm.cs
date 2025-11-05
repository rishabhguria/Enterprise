using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Nirvana.Middleware;

namespace CSBatchUI
{
    public partial class FundSymbolForm : Form
    {
        List<FundUI> _FundUIs = new List<FundUI>();

		Arguments _Arguments = new Arguments();

        List<FundSymbol> _FetchedFundSymbols = new List<FundSymbol>();

        private Arguments FetchDefaultArgumentsInstance()
        {
            Arguments arguments = new Arguments();
            arguments.Parse(new string[] { });
            return arguments;
        }

        private Arguments FetchUpdatedArgumentsInstance()
        {
           Arguments arguments = new Arguments();
            arguments.Parse(new string[] { UpdatedArgumentsString() });
            return arguments;
        }

        private string UpdatedArgumentsString(string _FundIDSymbolFile = null)
        {
            string _FromDateString = string.Empty;
            if (dateTimePickerFrom.Checked)
                _FromDateString = string.Format("@FromDate={0},", dateTimePickerFrom.Value.ToShortDateString());
            string _ToDateString = string.Empty;
            if (dateTimePickerTo.Checked)
                _ToDateString = string.Format("@ToDate={0},", dateTimePickerTo.Value.ToShortDateString());

            string argumentsString = _FromDateString + _ToDateString
                + string.Format("@AdjustFrom={0},@AdjustTo={1},@RollBackDays={2},@IgnoreAUECID={3},@SaveEnabled={4},@Cores={5},@Step={6},@TouchStep={7}",
               numericAdjustFrom.Value, numericAdjustTo.Value, numericRollBackDays.Value, checkBoxIgnoreAUECID.Checked, checkBoxSaveEnabled.Checked, numericCores.Value, numericStep.Value, checkBoxTouchStep.Checked);

            if (!string.IsNullOrWhiteSpace(_FundIDSymbolFile))
                argumentsString += ",@FundIDSymbolFile=" + _FundIDSymbolFile + ".xml";

            return argumentsString;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string _TxtBatch = txtBatch.Text.Trim();
                if (_TxtBatch.Length == 0)
                {
                    MessageBox.Show("You need to input batch file's name");
                    return;
                }

                string _RootPath = ".";

                string _FundIDSymbolFileName = _TxtBatch;
                string _FundIDSymbolFilePath = Path.Combine(_RootPath, _FundIDSymbolFileName + ".xml");

                string _FundIDSymbol = SelectedFundIDSymbols();
                File.WriteAllText(_FundIDSymbolFilePath, _FundIDSymbol);

                string argumentsString = UpdatedArgumentsString(_FundIDSymbolFileName);

                string _ProcessFileName = _TxtBatch;
                string _ProcessFilePath = Path.Combine(_RootPath, _ProcessFileName + ".cmd");

                string _Cmd = "CSBatch.exe " + argumentsString;
                File.WriteAllText(_ProcessFilePath, _Cmd);

                MessageBox.Show("Done creating batch file");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process batch = new Process())
                {
                    //string _RootPath = @"C:\Users\Mia\SourceCodeMW\DEV2\VS2010\CSBatch\CSBatch\bin\Debug";
                    //string _RootPath = @"E:\Nirvanacode\SourceCode\Dev\Middleware\VS2010\CSBatch\CSBatch\bin\Debug";
                    string _RootPath = ".";

                    string _FundIDSymbolFileName = "Filter";
                    string _FundIDSymbolFilePath = Path.Combine(_RootPath, _FundIDSymbolFileName + ".xml");

                    string _FundIDSymbol = SelectedFundIDSymbols();
                    File.WriteAllText(_FundIDSymbolFilePath, _FundIDSymbol);

                    string argumentsString = UpdatedArgumentsString(_FundIDSymbolFileName);

                    string _ProcessFileName = "CSBatch";
                    string _ProcessFilePath = Path.Combine(_RootPath, _ProcessFileName + ".exe");

                    batch.StartInfo.FileName = _ProcessFilePath;
                    batch.StartInfo.Arguments = argumentsString;
                    batch.Start();

                    //Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string SelectedFundIDSymbols()
        {
            StringBuilder sb = new StringBuilder();

            List<FundUI> _SelectedFunds = _FundUIs.Where(p => p.Selected).ToList();

            if (_SelectedFunds.Count != 0)
            {
                foreach (FundUI _SelectedFund in _SelectedFunds)
                {
                    if (string.IsNullOrWhiteSpace(_SelectedFund.Text))
                    {
                        Fund _FundElement = _SelectedFund.Fund;
                        sb.Append(_FundElement.ToString());
                        sb.Append(Environment.NewLine);
                    }
                    else
                    {
                        List<string> _SelectedSymbols = _SelectedFund.Text.TrimEnd(',').Split(',').ToList();
                        foreach (string _SelectedSymbol in _SelectedSymbols)
                        {
                           FundSymbol _FundSymbolElement = new FundSymbol { Fund = _SelectedFund.Fund, Symbol = _SelectedSymbol };
                            sb.Append(_FundSymbolElement.ToString());
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }

            return sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        private void comboBoxFund_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        public FundSymbolForm()
        {
            try
            {
                InitializeComponent();
                comboBoxFund.MouseWheel += new MouseEventHandler(comboBoxFund_MouseWheel);

                _Arguments = FetchDefaultArgumentsInstance();
                dateTimePickerFrom.Checked = oldFromChecked;
                dateTimePickerTo.Checked = oldToChecked;
                numericAdjustFrom.Value = _Arguments.AdjustFrom;
                numericAdjustTo.Value = _Arguments.AdjustTo;
                numericRollBackDays.Value = _Arguments.RollBackDays;
                checkBoxIgnoreAUECID.Checked = _Arguments.IgnoreAUECID;
                dateTimePickerFrom.ValueChanged += new System.EventHandler(dateTimePickerFrom_ValueChanged);
                dateTimePickerTo.ValueChanged += new System.EventHandler(dateTimePickerTo_ValueChanged);
                numericAdjustFrom.ValueChanged += new System.EventHandler(numericAdjustFrom_ValueChanged);
                numericAdjustTo.ValueChanged += new System.EventHandler(numericAdjustTo_ValueChanged);
                numericRollBackDays.ValueChanged += new System.EventHandler(numericRollBackDays_ValueChanged);
                checkBoxIgnoreAUECID.CheckStateChanged += new System.EventHandler(checkBoxIgnoreAUECID_CheckStateChanged);
                checkBoxSaveEnabled.Checked = _Arguments.SaveEnabled;
                checkBoxTouchStep.Checked = _Arguments.TouchStep;
                numericCores.Value = _Arguments.Cores;
                numericStep.Value = _Arguments.Step;

                ResetDatesFundsAndRefreshFundSymbols();
            }
            catch (Exception)
            {
            }
        }

        #region FundSymbolFormRefresh Triggers

        bool oldFromChecked = false;
        bool oldToChecked = false;

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Checked != oldFromChecked)
            {
                oldFromChecked = dateTimePickerFrom.Checked;
                if (dateTimePickerFrom.Checked)
                    numericRollBackDays.Enabled = false;
                else
                    numericRollBackDays.Enabled = true;
            }

            FundSymbolFormRefresh();
        }

        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerTo.Checked != oldToChecked)
            {
                oldToChecked = dateTimePickerTo.Checked;
            }

            FundSymbolFormRefresh();
        }

        private void numericAdjustFrom_ValueChanged(object sender, EventArgs e)
        {
            FundSymbolFormRefresh();
        }

        private void numericAdjustTo_ValueChanged(object sender, EventArgs e)
        {
            FundSymbolFormRefresh();
        }

        private void numericRollBackDays_ValueChanged(object sender, EventArgs e)
        {
            FundSymbolFormRefresh();
        }

        private void checkBoxIgnoreAUECID_CheckStateChanged(object sender, EventArgs e)
        {
            FundSymbolFormRefresh();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FundSymbolFormRefresh();
        }

        #endregion

        private void FundSymbolFormRefresh()
        {
            ResetDatesFundsAndRefreshFundSymbols();
        }

        #region ResetDatesFundsAndRefreshFundSymbols

        private void ResetDatesFundsAndRefreshFundSymbols()
        {
            ResetDatesFundsFundUIs();

            RefreshFundSymbols();
        }

        private void ResetDatesFundsFundUIs()
        {
            _Arguments = FetchUpdatedArgumentsInstance();

            BindingSourceDates.DataSource = _Arguments.Dates;
            listBoxDates.DataSource = BindingSourceDates;

            BindingSourceFunds.DataSource = _Arguments.Funds;
            comboBoxFund.DataSource = BindingSourceFunds;
            comboBoxFund.DisplayMember = "FundName";

            comboBoxFund_SelectedIndexReset();

            ResetFundUIs();

            _FetchedFundSymbols = new List <FundSymbol>();
            labelSymbolsStatus.Text = "Symbols Status: " + "Not Fetched";
        }

        private void RefreshFundSymbols()
        {
            List<FundUI> _SelectedFunds = _FundUIs.Where(p => p.Selected).ToList();

            DataTable _FundSymbolsTable = new DataTable("FundSymbols");
            DataColumn _FundLabelColumn = new DataColumn("Fund Name", typeof(string));
            _FundSymbolsTable.Columns.Add(_FundLabelColumn);
            DataColumn _FundColumn = new DataColumn("Symbol List", typeof(FundUI));
            _FundSymbolsTable.Columns.Add(_FundColumn);
            foreach (FundUI selectedFund in _SelectedFunds)
            {
                selectedFund.Selected = true;
                DataRow dr = _FundSymbolsTable.NewRow();
                dr["Fund Name"] = selectedFund.Fund.FundName;
                dr["Symbol List"] = selectedFund;
                _FundSymbolsTable.Rows.Add(dr);
            }
            BindingSourceFundSymbols.DataSource = _FundSymbolsTable;

            ultraGridFundSymbol.DataSource = BindingSourceFundSymbols;
            UltraGridColumn ugc = ultraGridFundSymbol.DisplayLayout.Bands[0].Columns["Symbol List"];
            ultraControlContainerEditor1.EditingControl = ctrlFundSymbol;
            ultraControlContainerEditor1.EditingControlPropertyName = "Fund";
            ugc.EditorComponent = ultraControlContainerEditor1;
            UltraGridColumn ugl = ultraGridFundSymbol.DisplayLayout.Bands[0].Columns["Fund Name"];
            ugl.CellActivation = Activation.Disabled;

            int _Count = _SelectedFunds.Count;
            if (_Count == 0)
                checkBoxAllFunds.CheckState = CheckState.Unchecked;
            else if (_Count == _FundUIs.Count)
                checkBoxAllFunds.CheckState = CheckState.Checked;
            else
                checkBoxAllFunds.CheckState = CheckState.Indeterminate;

            labelFundsCount.Text = "Funds Count: " + _Count;
        }

        #endregion

        private void comboBoxFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Fund selected = (Fund)comboBoxFund.SelectedValue;
                FundUI selectedFund = _FundUIs.SingleOrDefault(p => new FundEqualityComparer().Equals(selected, p.Fund));
                if (selectedFund != null)
                {
                    if (selectedFund.Selected == false)
                    {
                        selectedFund.Selected = true;
                        RefreshFundSymbols();
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            catch
            {

            }
        }

        private void ultraGridFundSymbol_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {
            foreach (UltraGridRow row in e.Rows)
            {
                FundUI fund = row.Cells["Symbol List"].Value as FundUI;
                fund.Selected = false;
            }

            e.Cancel = true;

            comboBoxFund_SelectedIndexReset();

            RefreshFundSymbols();
        }

        private void ultraGridFundSymbol_AfterEnterEditMode(object sender, EventArgs e)
        {
            if (!bgwFetchSymbols.IsBusy && _FetchedFundSymbols.Count == 0)
            {
                ultraGridFundSymbol.PerformAction(UltraGridAction.ExitEditMode);
                UltraGridColumn ugc = ultraGridFundSymbol.DisplayLayout.Bands[0].Columns["Symbol List"];
                ugc.CellActivation = Activation.Disabled;
                btnRefresh.Enabled = false;
                btnCreate.Enabled = false;
                btnRun.Enabled = false;

                bgwFetchSymbols.RunWorkerAsync(_Arguments);
                labelSymbolsStatus.Text = "Symbols Status: " + "Fetching";
            }
            else
            {

            }
        }

        private void bgwFetchSymbols_DoWork(object sender, DoWorkEventArgs e)
        {
            Arguments arguments = (Arguments)e.Argument;
            _FetchedFundSymbols = FundSymbol.GetFundSymbols(arguments.From, arguments.To).ToList();
            FillFetchedSymbols();
        }

        private void ResetFundUIs()
        {
            _FundUIs = _Arguments.Funds.Select(p => new FundUI { Fund = p, Symbols = new List<string>() }).ToList();
        }

        private void FillFetchedSymbols()
        {
            foreach (FundUI fundUI in _FundUIs)
            {
                var symbols = _FetchedFundSymbols.Where(p => new FundEqualityComparer().Equals(p.Fund, fundUI.Fund)).Select(p => p.Symbol);
                fundUI.Symbols.AddRange(symbols);
            }
        }

        private void bgwFetchSymbols_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                labelSymbolsStatus.Text = "Symbols Status: " + e.Error.Message;
            }
            else
            {
                RefreshFundSymbols();
                labelSymbolsStatus.Text = "Symbols Status: " + "Fetched";
            }

            UltraGridColumn ugc = ultraGridFundSymbol.DisplayLayout.Bands[0].Columns["Symbol List"];
            ugc.CellActivation = Activation.AllowEdit;
            btnRefresh.Enabled = true;
            btnCreate.Enabled = true;
            btnRun.Enabled = true;
        }

        private void ultraGridFundSymbol_BeforeExitEditMode(object sender, BeforeExitEditModeEventArgs e)
        {
            int _Row = ultraGridFundSymbol.ActiveCell.Row.Index;
            int _Column = ultraGridFundSymbol.ActiveCell.Column.Index;
            DataTable _SourceTable = (DataTable)BindingSourceFundSymbols.DataSource;
            FundUI _Object = (FundUI)_SourceTable.Rows[_Row][_Column];
            if (!string.IsNullOrWhiteSpace(_Object.Text))
            {

            }
        }

        private void checkBoxAllFunds_CheckStateChanged(object sender, EventArgs e)
        {
            List<FundUI> _SelectedFunds = _FundUIs.Where(p => p.Selected).ToList();
            int _Count = _SelectedFunds.Count;

            if (checkBoxAllFunds.CheckState == CheckState.Checked)
            {
                if (_Count != _FundUIs.Count)
                {
                    ResetFundUIs();
                    foreach (FundUI fundUI in _FundUIs)
                        fundUI.Selected = true;
                    FillFetchedSymbols();

                    comboBoxFund_SelectedIndexReset();

                    RefreshFundSymbols();
                }
            }
            else if (checkBoxAllFunds.CheckState == CheckState.Unchecked)
            {
                if (_Count != 0)
                {
                    ResetFundUIs();
                    FillFetchedSymbols();

                    comboBoxFund_SelectedIndexReset();

                    RefreshFundSymbols();
                }
            }
            else if (checkBoxAllFunds.CheckState == CheckState.Indeterminate)
            {

            }
        }

        private void comboBoxFund_SelectedIndexReset()
        {
            comboBoxFund.SelectedIndexChanged -= new EventHandler(comboBoxFund_SelectedIndexChanged);
            comboBoxFund.SelectedIndex = -1;
            comboBoxFund.SelectedIndexChanged += new EventHandler(comboBoxFund_SelectedIndexChanged);
        }
}
}

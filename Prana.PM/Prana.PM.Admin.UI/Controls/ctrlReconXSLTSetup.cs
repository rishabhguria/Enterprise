using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.PM.DAL;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PM.Admin.UI.Controls
{
    public partial class ctrlReconXSLTSetup : UserControl
    {
        #region Constants and Global Varibles

        const string COL_ThirdPartyID = "ThirdPartyID";
        const string COL_ReconTypeID = "ReconTypeID";
        const string COL_FormatType = "FormatType";
        const string COL_XSLTName = "XSLTName";

        private Prana.PM.BLL.ReconSetups _reconSetupList = null;
        private UltraGridBand _gridBandReconSetup = null;
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();

        #endregion

        public ctrlReconXSLTSetup()
        {
            InitializeComponent();
        }

        private void gridReconSetup_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            _gridBandReconSetup = gridReconSetup.DisplayLayout.Bands[0];

            UltraGridColumn colThirdPartyID = _gridBandReconSetup.Columns[COL_ThirdPartyID];
            colThirdPartyID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            colThirdPartyID.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colThirdPartyID.ValueList = cmbPrimeBroker;
            colThirdPartyID.Header.Caption = "Prime Broker";
            colThirdPartyID.Header.VisiblePosition = 1;


            UltraGridColumn colReconType = _gridBandReconSetup.Columns["ReconType"];
            colReconType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            colReconType.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colReconType.ValueList = cmbReconType;
            colReconType.Header.Caption = "Recon Type";
            colReconType.Header.VisiblePosition = 2;
            colReconType.Width = 85;

            UltraGridColumn colFormatType = _gridBandReconSetup.Columns[COL_FormatType];
            colFormatType.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colFormatType.CellActivation = Activation.AllowEdit;
            colFormatType.Header.VisiblePosition = 3;
            colFormatType.Header.Caption = "Format Type";
            gridReconSetup.DisplayLayout.Bands[0].Columns[COL_FormatType].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

            UltraGridColumn colXSLTName = _gridBandReconSetup.Columns[COL_XSLTName];
            colXSLTName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
            colXSLTName.CellActivation = Activation.NoEdit;
            colXSLTName.Header.VisiblePosition = 4;
            colXSLTName.Header.Caption = "XSLT Name";
            colXSLTName.Width = 188;
            gridReconSetup.DisplayLayout.Bands[0].Columns[COL_XSLTName].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

            if (!_gridBandReconSetup.Columns.Exists("SelectXSLT"))
            {
                UltraGridColumn colXSLTSelectButton = _gridBandReconSetup.Columns.Add("SelectXSLT");
                colXSLTSelectButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colXSLTSelectButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colXSLTSelectButton.Header.VisiblePosition = 5;
                colXSLTSelectButton.Header.Caption = "Select XSLT";
                colXSLTSelectButton.NullText = "Select XSLT";
                colXSLTSelectButton.Width = 75;
                colXSLTSelectButton.Hidden = false;
            }


            UltraGridColumn colReconTypeID = _gridBandReconSetup.Columns["ReconTypeID"];
            colReconTypeID.Hidden = true;


        }

        private void BindGridComboBoxes()
        {
            try
            {
                // Prime Broker Combo Binding            
                List<ReconDataSource> reconDataSourcecol = RunUploadManager.GetCompanyDataSourceNames();
                ReconDataSource reconTemp = new ReconDataSource();
                reconTemp.ThirdPartyID = 0;
                reconTemp.DataSourceName = "-Select-";
                reconTemp.DataSourceShortName = "-Select-";
                reconDataSourcecol.Insert(0, reconTemp);
                cmbPrimeBroker.DataSource = null;
                cmbPrimeBroker.DataSource = reconDataSourcecol;
                cmbPrimeBroker.DisplayMember = "DataSourceShortName";
                cmbPrimeBroker.ValueMember = "ThirdPartyID";
                cmbPrimeBroker.SelectedRow = cmbPrimeBroker.Rows[0];
                Utils.UltraDropDownFilter(cmbPrimeBroker, "DataSourceShortName");

                // Recon Type Combo Binding
                Infragistics.Win.ValueList reconTypeValueList = new Infragistics.Win.ValueList();
                //EnumerationValueList lstReconType = new EnumerationValueList();
                //Prana.BusinessObjects.EnumerationValueList lstReconType = new EnumerationValueList();
                List<EnumerationValue> lstReconType = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(ReconType));
                cmbReconType.DataSource = null;
                cmbReconType.DataSource = lstReconType;
                cmbReconType.DisplayMember = "DisplayText";
                cmbReconType.ValueMember = "Value";
                cmbReconType.SelectedRow = cmbReconType.Rows[0];
                Utils.UltraDropDownFilter(cmbReconType, "DisplayText");
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

        public void PopulateReconSetUpDetails()
        {
            try
            {
                BindGridComboBoxes();

                _reconSetupList = RunUploadManager.GetReconXSLTSetupDetails();
                if (_reconSetupList != null)
                {
                    gridReconSetup.DataSource = _reconSetupList;
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

        private void gridReconSetup_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == "SelectXSLT")
            {
                string title = "Select XSLT for The Data Source";
                string shortName = GetFileName(title);
                if (!String.IsNullOrEmpty(shortName))
                {
                    gridReconSetup.ActiveRow.Cells["XSLTName"].Value = shortName;
                }
            }
        }

        private string GetFileName(string title)
        {
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

        public void SaveReconXSLTDetails()
        {
            try
            {
                if (_reconSetupList.IsValid.Equals(true) && _reconSetupList != null)
                {
                    RunUploadManager.SaveReconXSLTDetails(_reconSetupList);
                    PopulateReconSetUpDetails();
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

        public void RemoveReconXSLTDetails()
        {
            try
            {
                if (gridReconSetup.ActiveRow != null)
                {
                    string msgText = "Are you sure to Remove" + " " + gridReconSetup.ActiveRow.Cells[COL_FormatType].Text.ToString() + " " + "?";
                    string caption = "Remove EMS Source";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;
                    result = MessageBox.Show(msgText, caption, buttons);

                    if (result == DialogResult.Yes && _reconSetupList != null)
                    {
                        ReconSetup row = (ReconSetup)gridReconSetup.ActiveRow.ListObject;
                        RunUploadManager.RemoveReconXSLTDetailEntry(row.ReconThirdPartyID, row.XSLTID);
                        gridReconSetup.ActiveRow.Delete(false);
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

        public void AddReconXSLTDetails()
        {
            ReconSetup rcnSetup = new ReconSetup();
            rcnSetup.ThirdPartyID = 0;
            rcnSetup.FormatType = string.Empty;
            rcnSetup.XSLTName = string.Empty;
            _reconSetupList.Add(rcnSetup);
            if (gridReconSetup.Rows.Count > 0)
            {
                gridReconSetup.Update();
            }
        }
    }
}

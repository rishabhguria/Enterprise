using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Infragistics.Win.UltraWinGrid;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlRunUpload : UserControl
    {

        private RunUploadSummary _runUploadSetUpSummaryData = new RunUploadSummary();

        private SortableSearchableList<RunUpload> _runUploadData = new SortableSearchableList<RunUpload>();
              
        UltraGridBand _gridBandRunUpload = null;




        #region Grid Column Names

        const string COL_CompanyNameID = "CompanyNameID";
        const string COL_DataSourceNameID = "DataSourceNameID";
        const string COL_EnableAutoTime = "EnableAutoTime";
        const string COL_AutoTime = "AutoTime";
        const string COL_Status = "Status";        
        const string COL_DirPath = "DirPath";
        const string COL_FileName = "FileName";
        const string COL_ErrorsButton = "DetailsButton";
        const string COL_ExceptionsButton = "ManualEntryButton";        

        #endregion Grid Column Names






        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlRunUpload"/> class.
        /// </summary>
        public CtrlRunUpload()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clears the control.
        /// </summary>
        public void ClearControl()
        {
            PopulateRunUploadDetails(0);

        }
        /// <summary>
        /// Populates the run upload details.
        /// </summary>
        /// <param name="selectedTreeNodeID">The selected tree node ID.</param>
        public void PopulateRunUploadDetails(int uploadClientID)
        {
            _runUploadSetUpSummaryData = RunUploadManager.GetRunUploadDataForUploadClientID(uploadClientID);

            _runUploadData = _runUploadSetUpSummaryData.RunUpload;

            gridRunUpload.DataMember = "RunUpload";
            gridRunUpload.DataSource = _runUploadSetUpSummaryData;

            BindGridComboBoxes();

            foreach (RunUpload runUpload in _runUploadData)
            {
                AddComboDataBindings(runUpload);
            }
        }

        /// <summary>
        /// Adds the combo data bindings.
        /// </summary>
        /// <param name="runUploadSetUp">The run upload set up.</param>
        private void AddComboDataBindings(RunUpload runUpload)
        {

            cmbDataSources.DataBindings.Clear();
            cmbDataSources.DataBindings.Add(new System.Windows.Forms.Binding("Text", runUpload, "DataSourceNameID.ShortName", true));

            cmbCompanies.DataBindings.Clear();
            cmbCompanies.DataBindings.Add(new System.Windows.Forms.Binding("Text", runUpload, "CompanyNameID.ShortName", true));

          //  cmbStatus.DataBindings.Clear();
          //  cmbCompanies.DataBindings.Add(new System.Windows.Forms.Binding("Text", runUpload, "Status", true));
        }

        /// <summary>
        /// Binds the grid combo boxes.
        /// </summary>
        private void BindGridComboBoxes()
        {
            DataSourceNameIDList.GetInstance().SelectItemRequired = true;
            DataSourceNameIDList.GetInstance().IsAllDataSourceAvailable = false;
            cmbDataSources.DataSource = DataSourceNameIDList.GetInstance().Retrieve;
            cmbDataSources.DisplayMember = "ShortName";
            cmbDataSources.ValueMember = "ID";
            Utils.UltraDropDownFilter(cmbDataSources, "ShortName");


            cmbCompanies.DataSource = CompanyNameIDList.Retrieve();
            cmbCompanies.DisplayMember = "ShortName";
            cmbCompanies.ValueMember = "ID";
            Utils.UltraDropDownFilter(cmbCompanies, "ShortName");

            //cmbStatus.DisplayMember = "DisplayText";
            //cmbStatus.ValueMember = "Value";
            //cmbStatus.DataSource = EnumHelper.ConvertEnumForBinding(typeof(RunUploadStatus));
            //Utils.UltraDropDownFilter(cmbStatus, "DisplayText");

        }

        bool _isGridInitialized = false;
        /// <summary>
        /// Handles the InitializeLayout event of the gridRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void gridRunUpload_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            if(bool.Equals(_isGridInitialized, false))
            {
            
                gridRunUpload.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                gridRunUpload.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                gridRunUpload.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
                gridRunUpload.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                gridRunUpload.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                gridRunUpload.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                gridRunUpload.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                gridRunUpload.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;


                _gridBandRunUpload = gridRunUpload.DisplayLayout.Bands[0];
                _gridBandRunUpload.Override.AllowColSwapping = AllowColSwapping.NotAllowed;


                UltraGridColumn colCompanyNameID = _gridBandRunUpload.Columns[COL_CompanyNameID];
                colCompanyNameID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                colCompanyNameID.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                //Get the list of all companies.
                colCompanyNameID.ValueList = cmbCompanies;
                colCompanyNameID.Header.Caption = "Company";
                colCompanyNameID.Header.VisiblePosition = 1;

                UltraGridColumn colDataSourceNameID = _gridBandRunUpload.Columns[COL_DataSourceNameID];
                colDataSourceNameID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                colDataSourceNameID.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colDataSourceNameID.ValueList = cmbDataSources;
                colDataSourceNameID.Header.Caption = "Upload Source";
                colDataSourceNameID.Header.VisiblePosition = 2;

                UltraGridColumn colEnableAutoTime = _gridBandRunUpload.Columns[COL_EnableAutoTime];
                colEnableAutoTime.Header.Caption = "";
                //colEnableAutoTime.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                colEnableAutoTime.Header.VisiblePosition = 3;

                UltraGridColumn colAutoTime = _gridBandRunUpload.Columns[COL_AutoTime];
                colAutoTime.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.TimeWithSpin;
                colAutoTime.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colAutoTime.Header.Caption = "Auto Time";
                colAutoTime.Header.VisiblePosition = 4;

                UltraGridColumn colStatus = _gridBandRunUpload.Columns[COL_Status];
                //colStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
                //colStatus.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                //colStatus.ValueList = cmbStatus;
                colStatus.Header.Caption = "Status";
                colStatus.Header.VisiblePosition = 5;
               
                UltraGridColumn colDirPath = _gridBandRunUpload.Columns[COL_DirPath];
                colDirPath.Header.VisiblePosition = 6;
                colDirPath.Header.Caption = "Dir Path";

                UltraGridColumn colFileName = _gridBandRunUpload.Columns[COL_FileName];
                colFileName.Header.VisiblePosition = 7;
                colFileName.Header.Caption = "File Name";

                if (!_gridBandRunUpload.Columns.Exists(COL_ErrorsButton))
                {
                    UltraGridColumn colErrorsButton = _gridBandRunUpload.Columns.Add(COL_ErrorsButton);
                    colErrorsButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colErrorsButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                    colErrorsButton.Header.VisiblePosition = 8;
                    colErrorsButton.Header.Caption = "View Errors";
                }

                if (!_gridBandRunUpload.Columns.Exists(COL_ExceptionsButton))
                {
                    UltraGridColumn colExceptionsButton = _gridBandRunUpload.Columns.Add(COL_ExceptionsButton);
                    colExceptionsButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colExceptionsButton.CellDisplayStyle = CellDisplayStyle.FormattedText;
                    //colExceptionsButton.CellButtonAppearance. = 
                    colExceptionsButton.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                    colExceptionsButton.Header.VisiblePosition = 9;
                    colExceptionsButton.Header.Caption = "View Exceptions";
                }
                _isGridInitialized = true;
            }
        }

        /// <summary>
        /// Handles the ClickCellButton event of the gridRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.CellEventArgs"/> instance containing the event data.</param>
        private void gridRunUpload_ClickCellButton(object sender, CellEventArgs e)
        {
            if(string.Equals(e.Cell.Column.Key, COL_ErrorsButton) || string.Equals(e.Cell.Column.Key, COL_ExceptionsButton))
            {
                MessageBox.Show("Under Construction!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

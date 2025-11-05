using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana.PM.Client.UI
{
    public partial class ctrlReImport : UserControl
    {
        public ctrlReImport()
        {
            InitializeComponent();
        }

        #region CONSTANTS
        const string _importTypeNetPosition = "Net Position";
        const string COL_BATCHID = "BatchID";
        const string COL_IMPORTDATE = "Import Date";
        const string COL_IMPORTTYPE = "Import Type";
        const string COL_FILENAME = "File Name";
        const string COL_IMPORT = "Import";
        const string COL_FILEPATH = "File Path";
        const string COL_RECORDSCOUNT = "Records Count";
        const string COL_USERNAME = "User Name";
        #endregion

        private CtrlImportPreferences _ctrlImportPreferences = new CtrlImportPreferences();

        /// <summary>
        /// This will bind the Reimport tab from the existing file system on the hard disk.
        /// </summary>
        public void BindReImportGrid()
        {
            try
            {
                DataTable dtReimport = new DataTable();

                DataColumn colBatchID = new DataColumn(COL_BATCHID, typeof(string));
                DataColumn colUserName = new DataColumn(COL_USERNAME, typeof(string));
                DataColumn colDateOfImport = new DataColumn(COL_IMPORTDATE, typeof(DateTime));
                DataColumn colImportType = new DataColumn(COL_IMPORTTYPE, typeof(string));
                DataColumn colFileName = new DataColumn(COL_FILENAME, typeof(string));
                //DataColumn colImportBtn = new DataColumn(COL_IMPORT, typeof(Button));
                DataColumn colFilePath = new DataColumn(COL_FILEPATH, typeof(string));
                DataColumn colDataRecordsCount = new DataColumn(COL_RECORDSCOUNT, typeof(string));

                dtReimport.Columns.Add(colBatchID);
                dtReimport.Columns.Add(colUserName);
                dtReimport.Columns.Add(colDateOfImport);
                dtReimport.Columns.Add(colImportType);
                dtReimport.Columns.Add(colFileName);
                dtReimport.Columns.Add(colFilePath);
                dtReimport.Columns.Add(colDataRecordsCount);

                ImportPreferences importPrefs = _ctrlImportPreferences.ImportPrefs;

                //Preferred directory by the user to save the error files..
                if (importPrefs.DirectoryPath.Equals(string.Empty))
                {
                    importPrefs.DirectoryPath = System.Windows.Forms.Application.StartupPath;
                }
                string importDirectoryPath = importPrefs.DirectoryPath + @"\Import Data";

                //Directory in which actual files are kept with the dates..
                if (Directory.Exists(importDirectoryPath))
                {
                    DirectoryInfo[] importDirectories = (new DirectoryInfo(importDirectoryPath)).GetDirectories();

                    foreach (DirectoryInfo directory in importDirectories)
                    {
                        FileInfo[] files = directory.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            DataRow dr = dtReimport.NewRow();
                            string[] seperators = new string[] { Seperators.SEPERATOR_13 };
                            string[] fileSubstring = file.Name.Split(seperators, StringSplitOptions.None);
                            StringBuilder fileName = new StringBuilder();
                            for (int i = 2; i <= fileSubstring.Length - 1; i++)
                            {
                                fileName.Append(fileSubstring[i]);
                                fileName.Append(Seperators.SEPERATOR_13);
                            }

                            dr[colBatchID] = fileSubstring[0];//file.Name.Substring(0, file.Name.IndexOf(Seperators.SEPERATOR_13));
                            dr[colUserName] = CachedDataManager.GetInstance.GetUserText(int.Parse(fileSubstring[1]));
                            dr[colDateOfImport] = directory.Name;
                            dr[colImportType] = _importTypeNetPosition;
                            dr[colFileName] = fileName.ToString().Substring(0, fileName.Length - 1);
                            dr[colFilePath] = file.FullName;
                            dr[colDataRecordsCount] = Convert.ToString(CountLinesInFile(file.FullName));
                            dtReimport.Rows.Add(dr);
                        }
                    }
                }

                grdReImport.DataSource = null;
                if (dtReimport.Rows.Count > 0)
                {
                    grdReImport.DataSource = dtReimport;
                    lblStatus.Text = string.Empty;
                }
                else
                {
                    lblStatus.Text = "Files for re-import do not exist.";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void grdReImport_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            grdReImport.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            grdReImport.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            UltraGridBand band = grdReImport.DisplayLayout.Bands[0];

            if (band != null)
            {
                UltraGridColumn colBatchID = band.Columns[COL_BATCHID];
                colBatchID.CellActivation = Activation.NoEdit;
                //colBatchID.Width = 70;
                colBatchID.Header.VisiblePosition = 1;

                UltraGridColumn colUserName = band.Columns[COL_USERNAME];
                colUserName.CellActivation = Activation.NoEdit;
                //colImportDate.Width = 70;
                colUserName.Header.VisiblePosition = 2;

                UltraGridColumn colImportDate = band.Columns[COL_IMPORTDATE];
                colImportDate.CellActivation = Activation.NoEdit;
                //colImportDate.Width = 70;
                colImportDate.Header.VisiblePosition = 3;

                UltraGridColumn colImportType = band.Columns[COL_IMPORTTYPE];
                colImportType.CellActivation = Activation.NoEdit;
                //colImportType.Width = 70;
                colImportType.Header.VisiblePosition = 4;

                UltraGridColumn colFileName = band.Columns[COL_FILENAME];
                colFileName.CellActivation = Activation.NoEdit;
                //colFileName.Width = 100;
                colFileName.Header.VisiblePosition = 5;

                UltraGridColumn colRecordsCount = band.Columns[COL_RECORDSCOUNT];
                colRecordsCount.CellActivation = Activation.NoEdit;
                colRecordsCount.Header.VisiblePosition = 6;

                UltraGridColumn colFilePath = band.Columns[COL_FILEPATH];
                //colFilePath.CellActivation = Activation.NoEdit;
                colFilePath.Hidden = true;

                if (!band.Columns.Exists(COL_IMPORT))
                {
                    UltraGridColumn colImportBtn = band.Columns.Add(COL_IMPORT);
                    colImportBtn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colImportBtn.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                    colImportBtn.Width = 100;
                    colImportBtn.Header.Caption = COL_IMPORT;
                    colImportBtn.NullText = COL_IMPORT;
                    colImportBtn.Header.VisiblePosition = 7;
                }
            }
        }

        private long CountLinesInFile(string f)
        {
            long count = 0;
            try
            {
                using (StreamReader r = new StreamReader(f))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    // It will also include the header line as record. Hence subtracting 1 for that.
                    count--;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return count;
        }

        public delegate void ImportEventHandler(string filePath, string importType);
        public event EventHandler<EventArgs<string, string>> ImportFile;

        private Timer _timer = new Timer();
        private void InitializeTimer()
        {
            _timer.Interval = 5000;
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
            _timer.Enabled = true;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Enabled = false;
            _timer.Stop();
            lblStatus.Text = string.Empty;

        }

        private void grdReImport_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow selectedRow = e.Cell.Row;

                string filePath = selectedRow.Cells[COL_FILEPATH].Value.ToString();
                string importType = selectedRow.Cells[COL_IMPORTTYPE].Value.ToString();
                InitializeTimer();
                lblStatus.Text = "Started Reimporting...";
                // Fires the event to flow through the CtrlRunDownload Logic.
                if (ImportFile != null)
                {
                    ImportFile(this, new EventArgs<string, string>(filePath, importType));
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdReImport_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (grdReImport.Rows.Count > 0)
                    {
                        MenuItem mnuDelete = new MenuItem("Delete");
                        mnuDelete.Click += new EventHandler(mnuDelete_Click);
                        ContextMenu contextMenu = new ContextMenu();
                        contextMenu.MenuItems.Add(mnuDelete);
                        grdReImport.ContextMenu = contextMenu;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void mnuDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdReImport.ActiveRow != null)
                {
                    UltraGridRow row = grdReImport.ActiveRow;
                    string filePath = row.Cells[COL_FILEPATH].Value.ToString();

                    DialogResult res = MessageBox.Show("Do you really want to delete the dump file?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res.Equals(DialogResult.Yes))
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            BindReImportGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}

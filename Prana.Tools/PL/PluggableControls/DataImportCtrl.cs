using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
//using Prana.Reconciliation;
using Prana.ReconciliationNew;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class DataImportCtrl : UserControl, IPluggableUserControl
    {
        DataTable _dt = new DataTable();
        public event EventHandler DataReloaded;
        public event EventHandler FilterChanged;
        string _fileName = string.Empty;
        bool isDataLoaded = false;
        DateTime _fromDate = DateTimeConstants.MinValue;
        DateTime _toDate = DateTimeConstants.MinValue;

        //string _pbName = String.Empty;
        //string _reconType = String.Empty;

        string _templateKey = string.Empty;

        const string TickerSymbol = "Symbol";
        const string RICSymbol = "RIC";
        const string ISINSymbol = "ISIN";
        const string SEDOLSymbol = "SEDOL";
        const string CUSIPSymbol = "CUSIP";
        const string BloombergSymbol = "Bloomberg";
        const string OSIOptionSymbol = "OSIOptionSymbol";
        const string IDCOOptionSymbol = "IDCOOptionSymbol";
        const string OpraOptionSymbol = "OpraOptionSymbol";
        const string SMRequest = "SMRequest";

        //bool _isSMResponsewiredup = false;
        Timer timerSMRequest = new Timer();

        BackgroundWorker _bgImportData = new BackgroundWorker();

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public DataImportCtrl()
        {
            try
            {

                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
                    }
                    grdData.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                    grdData.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.Green;
                    grdData.DisplayLayout.Override.RowAppearance.ForeColor = Color.LightGray;
                    cmbbxXslts.Hide();
                    timerSMRequest.Tick += new EventHandler(timerSMRequest_Tick); // Everytime timer ticks, timer_Tick will be called
                                                                                  //timerSMRequest.Interval = (1000) * (3);              // Timer will tick every 3 second
                                                                                  //timerSMRequest.Stop();

                    //setup for import background thread
                    _bgImportData = new BackgroundWorker();
                    _bgImportData.DoWork += new DoWorkEventHandler(_bgImportData_DoWork);
                    _bgImportData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgImportData_RunWorkerCompleted);
                    _bgImportData.WorkerSupportsCancellation = true;
                }
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

        private void SetButtonsColor()
        {
            try
            {
                btnImport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnImport.ForeColor = System.Drawing.Color.White;
                btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnImport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnImport.UseAppStyling = false;
                btnImport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        List<string> _gridColumnList = new List<string>();
        public List<string> DisplayedColumnList
        {
            set
            {
                _gridColumnList = value;
            }
        }

        //private void BindSetupsCombo()
        //{
        //    string xsltFileName = string.Empty;
        //    if (_reconTemplate != ApplicationConstants.C_COMBO_SELECT)
        //    {
        //        string key = _reconTemplate.ToString();
        //        //string key = _pbName.Trim().ToUpper() + "," + _reconType.Trim().ToUpper();
        //        xsltFileName = ReconPrefManager.ReconPreferences.GetXsltSetups(key);
        //        //if (xsltSetups.Count > 0)
        //        //{
        //        //    List<string> setupNames = new List<string>();
        //        //    setupNames.Add("test");
        //        //    //cmbSetupName.DataSource = null;

        //        //    //foreach (XsltSetup xsltSetup in xsltSetups)
        //        //    //{
        //        //    //    setupNames.Add(xsltSetup.SetupName);
        //        //    //}
        //        //    //cmbSetupName.DataSource = setupNames;
        //        //    //cmbSetupName.DataBind();
        //        //    //cmbSetupName.Value = setupNames[0];
        //        //    //cmbSetupName.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //        //}
        //        //else
        //        //{
        //        //    //cmbSetupName.DataSource = null;
        //        //    //cmbSetupName.DisplayLayout.Bands[0].ColHeadersVisible = false;
        //        //}
        //    }
        //}

        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void GetFileName()
        {
            try
            {
                if (!string.IsNullOrEmpty(_templateKey))
                {
                    bool fileExists = false;
                    string fileNameTobeImported = string.Empty;
                    ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(_templateKey);
                    if (!string.IsNullOrEmpty(template.ImportFileDetails.ImportFilePath))
                    {
                        openFileDialog1.InitialDirectory = template.ImportFileDetails.ImportFilePath;
                    }
                    else
                    {
                        //browse desktop
                        openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                    if (!string.IsNullOrEmpty(template.ImportFileDetails.NamingConvention))
                    {
                        string fileName = template.ImportFileDetails.NamingConvention;
                        //int startIndex = fileName.IndexOf("<");
                        //int lastIndex = fileName.LastIndexOf(">");
                        int startIndexBraces = fileName.IndexOf("{");
                        int endIndexBraces = fileName.LastIndexOf("}");
                        if (startIndexBraces != -1 && endIndexBraces != -1)
                        {
                            //if (startIndexBraces == -1 || endIndexBraces == -1)
                            //{
                            //    MessageBox.Show("The Naming Convention of The File to be Imported is not correct.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    return;
                            //}
                            int lengthOfFile = (endIndexBraces - startIndexBraces) - 1;
                            if (lengthOfFile <= 0)
                            {
                                MessageBox.Show("The Naming Convention of The File to be Imported is not correct.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            string FileDateFormat = fileName.Substring(startIndexBraces + 1, lengthOfFile);
                            string strFileNameBeforeStartBraces = fileName.Substring(0, fileName.IndexOf("{"));
                            string strFileNameBeforeClosingBraces = fileName.Substring(0, fileName.IndexOf("}"));
                            string strFileNameAfterClosingBraces = fileName.Substring(strFileNameBeforeClosingBraces.Length + 1);
                            //string fileNameTobeSavedAndDisplay = string.Empty;
                            //string pranaFilePath = string.Empty;
                            string DateFormat = _fromDate.Date.ToString(FileDateFormat.ToLower().Replace("mm", "MM"));
                            fileNameTobeImported = strFileNameBeforeStartBraces + DateFormat + strFileNameAfterClosingBraces;
                            _fileName = template.ImportFileDetails.ImportFilePath + "\\" + fileNameTobeImported;
                        }
                        else
                        {
                            _fileName = template.ImportFileDetails.ImportFilePath + "\\" + template.ImportFileDetails.NamingConvention;
                        }
                        //condition to check whether the file name contains extension...
                        if (_fileName.LastIndexOf('.') == -1)
                        {
                            string fileExtension = string.Empty;
                            switch (template.ImportFileDetails.FileFormat)
                            {
                                case AutomationEnum.DataSourceFileFormat.Excel:
                                    fileExtension = AutomationEnum.FileFormat.xls.ToString();
                                    break;
                                case AutomationEnum.DataSourceFileFormat.Csv:
                                    fileExtension = AutomationEnum.FileFormat.csv.ToString();
                                    break;
                                case AutomationEnum.DataSourceFileFormat.Text:
                                    fileExtension = "txt";
                                    break;
                                case AutomationEnum.DataSourceFileFormat.FixPlace:
                                    break;
                                case AutomationEnum.DataSourceFileFormat.Default:
                                    break;
                                default:
                                    break;
                            }


                            if (!string.IsNullOrEmpty(fileExtension.Trim()))
                            {
                                fileNameTobeImported = fileNameTobeImported + "." + fileExtension.Trim();
                                _fileName = _fileName + "." + fileExtension.Trim();
                            }
                        }
                        // check for the File Path i.e. Is Path Valid
                        bool isFilePathExists = System.IO.Directory.Exists(template.ImportFileDetails.ImportFilePath);
                        if (isFilePathExists)
                        {
                            // check if file exists...
                            fileExists = System.IO.File.Exists(_fileName);
                        }
                    }
                    if (string.IsNullOrEmpty(template.ImportFileDetails.NamingConvention) || !fileExists)
                    {
                        openFileDialog1.Title = "Select File to Import";
                        openFileDialog1.Filter = openFileDialog1.Filter = "Data Files (*.xls,*.csv)|*.xls;*.csv| All Files|*.*";
                        openFileDialog1.FileName = String.Empty;
                        DialogResult importResult = openFileDialog1.ShowDialog();
                        if (importResult == DialogResult.OK)
                        {
                            _fileName = openFileDialog1.FileName;
                        }
                        else if (importResult == DialogResult.Cancel)
                        {
                            _fileName = string.Empty;
                            MessageBox.Show("Operation cancelled by User.", "Exception Report");
                        }
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        private bool ImportFile()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_fileName))
                {
                    //run DoWork Method of backgroundworker
                    _bgImportData.RunWorkerAsync();

                    //done in dowork method
                    //_dt = GetDataTableFromDifferentFileFormats(_fileName);
                    //_dt.AcceptChanges();
                    //NewUtilities.AddPrimaryKey(_dt);

                    // btnTransform.Enabled = true;
                    //  btnGroup.Enabled = true;
                    //_dt.TableName = "Comparision";
                    //btnTransform_Click(null, null);
                    //btnGroup_Click(null, null);

                    //done in runworkercompleted
                    //grdData.DataSource = _dt;
                    //grdData.DataBind();
                    return true;
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
            return false;
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateSelectedFormat())
                {
                    GetFileName();
                    //disable import button while fetching data and change text from Import to Fetching Data
                    DisableImportUI();
                    if (ImportFile())
                    {
                        //string DeletePath = ReconConstants.ReconDataDirectoryPath + @"\" + _templateKey;
                        //#Comented
                        //DeleteOldFiles(DeletePath);

                        //following lines which write xml are commented because xml writing is done in the timer_tick

                        //string dirName = ReconConstants.ReconDataDirectoryPath + @"\" + _reconTemplate + @"\" + _selectedDate.Date.ToString("MM-dd-yyyy");
                        //string fileName = dirName + @"\" + cmbSetupName.Value.ToString();
                        //ReconUtilities.SaveTransformedPbData(dsPB, dirName, fileName);
                    }
                    else
                    {
                        EnableImportUI();
                    }
                    if (DataReloaded != null)
                    {
                        DataReloaded(null, null);
                    }

                }
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



        //Modified By: Surendra Bisht
        //Modification date: Jan 24, 2013
        // http://jira.nirvanasolutions.com:8080/browse/PRANA-2888

        //private void DeleteOldFiles(String DeletePath)     //  Exception Report delete days limit
        //{
        //    try
        //    {
        //        int DeleteDaysLimit;
        //        if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["DeleteDaysLimit"].ToString(), out DeleteDaysLimit))
        //            DeleteDaysLimit = 365;
        //        foreach (string ReconDate in Directory.GetDirectories(DeletePath, "*.*", SearchOption.TopDirectoryOnly))
        //        {

        //            FileInfo file = new FileInfo(ReconDate + "\\recon.xml");
        //            if ((DateTime.Today.Date - file.LastWriteTime.Date).TotalDays > DeleteDaysLimit)
        //                Directory.Delete(ReconDate, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}
        //Following method is moved to filereader class
        #region commented
        //private static DataTable GetDataTableFromDifferentFileFormats(string fileName)
        //{
        //    DataTable dTable = null;
        //    try
        //    {
        //        string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

        //        switch (fileFormat.ToUpperInvariant())
        //        {
        //            case "CSV":
        //                dTable = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
        //                break;
        //            case "XLS":
        //                dTable = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(fileName);
        //                break;
        //            default:
        //                dTable = FileReaderFactory.Create(DataSourceFileFormat.Default).GetDataTableFromUploadedDataFile(fileName);
        //                break;
        //        }

        //        ///Modify the table schema and assign in the same table.
        //        //dTable = ModifyTableSchema(dTable);
        //    }
        //    catch (System.IO.IOException ex)
        //    {
        //        //if (ex.TargetSite.MethodHandle.Value.ToString() == "2032573600")
        //        //{
        //        MessageBox.Show("File in use! Please close the file and retry.");
        //        throw;
        //        // }
        //    }
        //    catch (Exception)
        //    {
        //        throw;

        //    }
        //    return dTable;
        //}
        #endregion

        public DataTable Data
        {
            get { return _dt; }
            set { _dt = value; }
        }
        public void ExportToExcel()
        {
            try
            {
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.ExportToExcel(grdData);
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
        public void SetUp(DataTable filenameData, string name)
        {
            try
            {
                grdData.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                grdData.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.Green;
                grdData.DisplayLayout.Override.RowAppearance.ForeColor = Color.LightGray;
                ultraExpandableGroupBox2.Text = name;
                if (filenameData != null)
                {
                    cmbbxXslts.DataSource = filenameData;
                    cmbbxXslts.DisplayMember = "DisplayName";
                    cmbbxXslts.ValueMember = "Name";
                    cmbbxXslts.DataBind();
                    cmbbxXslts.DisplayLayout.Bands[0].Columns["Name"].Hidden = true;
                    cmbbxXslts.Value = int.MinValue;
                }
                else
                {
                    // btnTransform.Visible = false;
                    // btnGroup.Visible = false;
                    cmbbxXslts.Visible = false;
                    //cmbSetupName.Visible = false;
                    grdData.Dock = DockStyle.Fill;
                }
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Name = name;

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
        //private DataTable CreateDataTable(XmlNode xmlnode)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {

        //        dt.Columns.Add("DisplayName");
        //        dt.Columns.Add("Name");
        //        XmlNodeList xmlnodes = xmlnode.SelectNodes("Xslts/Xslt");
        //        foreach (XmlNode xsltNode in xmlnodes)
        //        {
        //            string displayName = xsltNode.Attributes["DisplayName"].Value;
        //            string Name = xsltNode.Attributes["Name"].Value;
        //            dt.Rows.Add(new object[] { displayName, Name });
        //        }
        //        dt.Rows.Add(new object[] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return dt;
        //}
        //int _hashCode = 0;

        //private void FillSMData()
        //{
        //    try
        //    {
        //        if (!_isSMResponsewiredup)
        //        {
        //            NewUtilities.SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<ISecMasterBase>>(_secMaster_SecMstrDataResponse);
        //            //new SecMasterDataHandler(_secMaster_SecMstrDataResponse);
        //            _isSMResponsewiredup = true;
        //        }
        //        _hashCode = this.GetHashCode();

        //        // NewUtilities.AddPrimaryKey(_dt);
        //        //timerSMRequest.Enabled = true;                       // Enable the timer
        //        //timerSMRequest.Interval = (1000) * (3);              // Timer will tick after every 3 second
        //        //timerSMRequest.Start();                             //start timer
        //        if (IsSecurityMasterRequestRequired())
        //        {
        //            GetUniqueSymbolCollection();
        //            NewUtilities.GetSMData(_uniqueSymbolDict, _dt, _hashCode);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //This method moved to recon manager

        #region commented
        //private void btnTransform_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        string tempPath = ReconConstants.ReconDataDirectoryPath + @"\" + _templateKey + @"\" + _fromDate.Date.ToString("MM-dd-yyyy") + "\\xmls\\Transformation\\Temp";
        //        // string tempPath = Application.StartupPath + "\\xmls\\Transformation\\Temp";
        //        //DirectoryInfo dir = new DirectoryInfo(tempPath);
        //        if (!Directory.Exists(tempPath))
        //        {
        //            Directory.CreateDirectory(tempPath);
        //        }

        //        string inputXML = tempPath + "\\InputXML.xml";
        //        string outputXML = tempPath + "\\OutPutXML.xml";


        //        //string inputXML = Application.StartupPath + "\\xmls\\Transformation\\Temp\\InputXML.xml";
        //        //string outputXML = Application.StartupPath + "\\xmls\\Transformation\\Temp\\OutPutXML.xml";
        //        string path = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + "\\";
        //        string xsltName = GetXsltFileName();
        //        string xsltPath = path + xsltName;
        //        // cmbbxXslts.Value.ToString();
        //        //System.IO.Stream outstream = new System.IO.MemoryStream();
        //        //System.IO.Stream stream = new System.IO.MemoryStream();
        //        //dt.WriteXml(stream, XmlWriteMode.IgnoreSchema);
        //        _dt.WriteXml(inputXML);
        //        //XmlTextReader tr = new XmlTextReader(stream);
        //        //XmlTextWriter textWriter = new XmlTextWriter(outstream, Encoding.UTF8);
        //        System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
        //        xslt.Load(xsltPath);
        //        //xslt.Transform(tr, textWriter);
        //        xslt.Transform(inputXML, outputXML);

        //        DataSet ds = new DataSet();
        //        //ds.ReadXml(outputXML);
        //        ds = XMLUtilities.ReadXmlUsingBufferedStream(outputXML);
        //        if (ds.Tables.Count > 0 && ds.Tables[0] != null)
        //        {
        //            _dt = ds.Tables[0];
        //            // NewUtilities.AddPrimaryKey(_dt);
        //            //timerSMRequest.Enabled = true;                       // Enable the timer
        //            //timerSMRequest.Interval = (1000) * (3);              // Timer will tick after every 3 second
        //            //timerSMRequest.Start();                             //start timer
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        #endregion

        //private bool IsSecurityMasterRequestRequired()
        //{
        //    if (_dt.Columns.Contains(SMRequest))
        //    {
        //        if (_dt.Rows[0][SMRequest].ToString().ToUpper().Equals("TRUE"))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        Dictionary<int, List<string>> _uniqueSymbolDict = new Dictionary<int, List<string>>();

        //private void GetUniqueSymbolCollection()
        //{
        //    try
        //    {
        //        _uniqueSymbolDict.Clear();
        //        foreach (DataRow dataRow in _dt.Rows)
        //        {
        //            //if (_dt.Columns.Contains(TickerSymbol) && !string.IsNullOrEmpty(dataRow[TickerSymbol].ToString()))
        //            //{
        //            //    if (_uniqueSymbolDict.ContainsKey(0))
        //            //    {
        //            //        List<string> sameSymbolList = _uniqueSymbolDict[0];
        //            //        if (!sameSymbolList.Contains(dataRow[TickerSymbol].ToString()))
        //            //        {
        //            //            sameSymbolList.Add(dataRow[TickerSymbol].ToString());
        //            //        }
        //            //    }
        //            //    else
        //            //    {
        //            //        List<string> symbolList = new List<string>();
        //            //        symbolList.Add(dataRow[TickerSymbol].ToString());
        //            //        _uniqueSymbolDict.Add(0, symbolList);
        //            //    }
        //            //}
        //            if (_dt.Columns.Contains(RICSymbol) && !string.IsNullOrEmpty(dataRow[RICSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(1))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[1];
        //                    if (!sameSymbolList.Contains(dataRow[RICSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[RICSymbol].ToString());
        //        }
        //    }
        //    else
        //    {
        //        List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[RICSymbol].ToString());
        //                    _uniqueSymbolDict.Add(1, symbolList);
        //    }
        //}
        //            else if (_dt.Columns.Contains(ISINSymbol) && !string.IsNullOrEmpty(dataRow[ISINSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(2))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[2];
        //                    if (!sameSymbolList.Contains(dataRow[ISINSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[ISINSymbol].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[ISINSymbol].ToString());
        //                    _uniqueSymbolDict.Add(2, symbolList);
        //                }
        //            }
        //            else if (_dt.Columns.Contains(CUSIPSymbol) && !string.IsNullOrEmpty(dataRow[CUSIPSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(4))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[4];
        //                    if (!sameSymbolList.Contains(dataRow[CUSIPSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[CUSIPSymbol].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[CUSIPSymbol].ToString());
        //                    _uniqueSymbolDict.Add(4, symbolList);
        //                }
        //            }
        //            else if (_dt.Columns.Contains(SEDOLSymbol) && !string.IsNullOrEmpty(dataRow[SEDOLSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(3))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[3];
        //                    if (!sameSymbolList.Contains(dataRow[SEDOLSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[SEDOLSymbol].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[SEDOLSymbol].ToString());
        //                    _uniqueSymbolDict.Add(3, symbolList);
        //                }
        //            }
        //            else if (_dt.Columns.Contains(BloombergSymbol) && !string.IsNullOrEmpty(dataRow[BloombergSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(5))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[5];
        //                    if (!sameSymbolList.Contains(dataRow[BloombergSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[BloombergSymbol].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[BloombergSymbol].ToString());
        //                    _uniqueSymbolDict.Add(5, symbolList);
        //                }
        //            }
        //            else if (_dt.Columns.Contains(OSIOptionSymbol) && !string.IsNullOrEmpty(dataRow[OSIOptionSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(6))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[6];
        //                    if (!sameSymbolList.Contains(dataRow[OSIOptionSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[OSIOptionSymbol].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[OSIOptionSymbol].ToString());
        //                    _uniqueSymbolDict.Add(6, symbolList);
        //                }
        //            }
        //            else if (_dt.Columns.Contains(IDCOOptionSymbol) && !string.IsNullOrEmpty(dataRow[IDCOOptionSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(7))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[7];
        //                    if (!sameSymbolList.Contains(dataRow[IDCOOptionSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[IDCOOptionSymbol].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[IDCOOptionSymbol].ToString());
        //                    _uniqueSymbolDict.Add(7, symbolList);
        //                }
        //            }
        //            else if (_dt.Columns.Contains(OpraOptionSymbol) && !string.IsNullOrEmpty(dataRow[OpraOptionSymbol].ToString()))
        //            {
        //                if (_uniqueSymbolDict.ContainsKey(8))
        //                {
        //                    List<string> sameSymbolList = _uniqueSymbolDict[8];
        //                    if (!sameSymbolList.Contains(dataRow[OpraOptionSymbol].ToString()))
        //                    {
        //                        sameSymbolList.Add(dataRow[OpraOptionSymbol].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    List<string> symbolList = new List<string>();
        //                    symbolList.Add(dataRow[OpraOptionSymbol].ToString());
        //                    _uniqueSymbolDict.Add(8, symbolList);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        public delegate void SecMasterObjHandler(object sender, EventArgs<SecMasterBaseObj> e);

        void _secMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SecMasterObjHandler secMasterObjHandler = new SecMasterObjHandler(_secMaster_SecMstrDataResponse);
                        this.BeginInvoke(secMasterObjHandler, new object[] { sender, e });
                    }
                    else
                    {
                        FillObjFromSM(e.Value);
                        timerSMRequest.Stop();
                        timerSMRequest.Start();
                    }
                }
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


        private void FillObjFromSM(SecMasterBaseObj secMasterObj)
        {
            try
            {
                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                int requestedSymbologyID = secMasterObj.RequestedSymbology;
                string requestedSymbology = TickerSymbol;
                switch (requestedSymbologyID)
                {
                    case 0:
                        requestedSymbology = TickerSymbol;
                        break;
                    case 1:
                        requestedSymbology = RICSymbol;
                        break;
                    case 2:
                        requestedSymbology = ISINSymbol;
                        break;
                    case 3:
                        requestedSymbology = SEDOLSymbol;
                        break;
                    case 4:
                        requestedSymbology = CUSIPSymbol;
                        break;
                    case 5:
                        requestedSymbology = BloombergSymbol;
                        break;
                    case 6:
                        requestedSymbology = OSIOptionSymbol;
                        break;
                    case 7:
                        requestedSymbology = IDCOOptionSymbol;
                        break;
                    case 8:
                        requestedSymbology = OpraOptionSymbol;
                        break;
                    default:
                        break;
                }

                if (_uniqueSymbolDict.ContainsKey(requestedSymbologyID))
                {
                    List<string> symbolList = _uniqueSymbolDict[requestedSymbologyID];
                    if (symbolList.Contains(secMasterObj.RequestedSymbol))
                    {
                        DataRow[] rows = _dt.Select(requestedSymbology + " = '" + secMasterObj.RequestedSymbol + "'");
                        foreach (DataRow dataRow in rows)
                        {
                            dataRow[TickerSymbol] = pranaSymbol;
                            //dataRow["CompanyName"] = secMasterObj.LongName;
                        }
                    }
                }
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

        //This method is of no use
        #region to be commented
        //private string GetXsltFileName()
        //{
        //    string xsltName = String.Empty;
        //    if (_templateKey != ApplicationConstants.C_COMBO_SELECT)
        //    {
        //        //bool IsXsltFound = new bool();
        //        xsltName = ReconPrefManager.ReconPreferences.GetXsltFileName(_templateKey);
        //        if (string.IsNullOrWhiteSpace(xsltName))
        //        {
        //            MessageBox.Show("Set XSLT File Name for selected Template in Preferences > File Mapping!", "Recon", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    return xsltName;
        //}
        #endregion

        public void BindData()
        {
            try
            {
                grdData.DataSource = _dt;
                _dt.TableName = "Comparision";
                //    Utilities.UIUtilities.UltraWinGridUtils.SetColumns(_gridColumnList, grdData);
                grdData.DataBind();
                //  List<MasterColumn> listColumns = ReconPrefManager.ReconPreferences.GetNirvanaMasterColumns(_reconTemplate);
                if (!string.IsNullOrEmpty(_templateKey))
                {
                    List<string> listColumns = ReconPrefManager.ReconPreferences.GetPBGridDisplayColumnNames(_templateKey);
                    ReconUtilities.SetGridColumns(grdData, listColumns);
                }
                else
                {
                    ReconUtilities.SetGridColumns(grdData, _gridColumnList);
                }
                //
                if (grdData.DisplayLayout.Bands[0].Columns.Exists("RowID"))
                {
                    grdData.DisplayLayout.Bands[0].Columns["RowID"].Hidden = true;
                }
                if (grdData.DisplayLayout.Bands[0].Columns.Exists("Quantity"))
                {
                    Infragistics.Win.UltraWinGrid.UltraGridColumn colQuantity = grdData.DisplayLayout.Bands[0].Columns["Quantity"];
                    //CHMW-2955	CLONE -Quantity digit round off issue in Transaction Recon.
                    colQuantity.Format = "#,#.####";
                }
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

        public UserControl Control
        {
            get { return this; }
        }
        private void cmbbxXslts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        //Todo: This method is of no use code moved to ReconManager
        //private void UpdateTableSchemaForCustomCoulms()
        //{
        //    try
        //    {
        //        DataTable NewDT = _dt.Copy();
        //        List<string> lstCustomColumns = new List<string>();
        //        //make expression column readonly property to false so that on that column grouping can be done
        //        //http://jira.nirvanasolutions.com:8080/browse/GUGGENHEIM-12
        //        foreach (DataColumn col in _dt.Columns)
        //        {
        //            if (!string.IsNullOrEmpty(col.Expression))
        //            {
        //                col.Expression = string.Empty;
        //                col.ReadOnly = false;
        //                lstCustomColumns.Add(col.ColumnName);
        //            }
        //        }
        //        if (lstCustomColumns.Count > 0)
        //        {
        //            _dt.Clear();
        //            foreach (DataRow row in NewDT.Rows)
        //            {
        //                DataRow newRow = _dt.NewRow();
        //                newRow.ItemArray = row.ItemArray;
        //                _dt.Rows.Add(newRow);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //Todo: This method is of no usem code moved to ReconManager
        //private void btnGroup_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //  List<MatchingRule> rules=ReconPrefManager.GetListOfRules("Grouping");
        //        //
        //        GroupingCriteria groupingCriteria = ReconPrefManager.ReconPreferences.GetGroupingCriteria(_templateKey);

        //        ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(_templateKey);
        //        //if (rules != null)
        //        //{

        //        if (template != null)
        //{
        //            UpdateTableSchemaForCustomCoulms();
        //            GroupingComponent.Group(_dt, groupingCriteria, template.ReconType, template.DictGroupingSummary);
        //            //BindData() method commented and moved to runworkercompleted because it invokes main ui controls                    
        //}
        //        //btnGroup.Enabled = false;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        public new bool Validate()
        {
            try
            {
                if (!ValidateSelectedFormat())
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(_fileName) && !isDataLoaded)
                {
                    MessageBox.Show("Please select a File to Import!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
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
            return false;

        }
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        private bool ValidateSelectedFormat()
        {
            try
            {
                string path = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + "\\";

                //if (_pbName == ApplicationConstants.C_COMBO_SELECT || _reconType == ApplicationConstants.C_COMBO_SELECT || _pbName == String.Empty || _reconType == String.Empty)
                //{
                //    MessageBox.Show("Select valid PrimeBroker and ReconType !", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return false;
                //}
                if (_templateKey == ApplicationConstants.C_COMBO_SELECT || string.IsNullOrWhiteSpace(_templateKey))
                {
                    MessageBox.Show("Select valid ReconTemplate Type !", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //else if (cmbSetupName.DataSource == null || cmbSetupName.Value.ToString() == String.Empty)
                //{
                //    MessageBox.Show("Set some XSLT Setup for Selected Template!", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return false;
                //}
                //else if (cmbSetupName.DataSource == null || cmbSetupName.Value.ToString() == String.Empty)
                //{
                //    MessageBox.Show("Set some XSLT Setup for Prime Broker & Recon Type !", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return false;
                //}

                string xsltName = ReconPrefManager.ReconPreferences.GetXsltFileName(_templateKey);
                string xsltPath = path + xsltName;
                if (string.IsNullOrWhiteSpace(xsltName))
                {
                    return false;
                }
                else if (!File.Exists(xsltPath))
                {
                    MessageBox.Show("  Copy xslt file to MappingFiles/ReconXSLT folder of application.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
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
            return true;
        }

        bool dataReLoaded = false;
        public void Reload()
        {
            try
            {
                if (!dataReLoaded)
                {
                    ReconParameters reconParameters = new ReconParameters();
                    reconParameters.DTFromDate = _fromDate;
                    reconParameters.DTToDate = _toDate;
                    reconParameters.TemplateKey = _templateKey;
                    DataSet transformedDataSet = ReconUtilities.GetCorrespondingTransformedPBData(reconParameters);
                    if (transformedDataSet != null && transformedDataSet.Tables.Count > 0)
                    {
                        _dt = transformedDataSet.Tables[0];

                        btnImport.Enabled = true;
                        btnImport.Text = "Import";
                        isDataLoaded = true;

                    }
                    else
                    {
                        ImportFile();
                    }
                    _dt.AcceptChanges();
                    BindData();

                }
                dataReLoaded = false;
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

        private void grdData_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (e.Row.Cells.Exists("Mismatch"))
            {
                string percentageMismatch = e.Row.Cells["Mismatch"].Text;
                if (!string.IsNullOrWhiteSpace(percentageMismatch))
                {
                    e.Row.Appearance.ForeColor = Color.Brown;
                    e.Row.Appearance.BackColor = Color.Gray;
                }
            }
        }

        private void grdData_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            if (FilterChanged != null)
            {
                FilterChanged(sender, e);
            }
        }


        #region IPluggableUserControl Members

        public void ApplyFilters(object sender, EventArgs e)
        {
            AfterRowFilterChangedEventArgs ef = (AfterRowFilterChangedEventArgs)e;
            if (grdData.DisplayLayout.Bands[0].ColumnFilters.Exists(ef.Column.Key))
            {
                grdData.DisplayLayout.Bands[0].ColumnFilters[ef.Column.Key].ClearFilterConditions();

                foreach (FilterCondition fc in ef.NewColumnFilter.FilterConditions)
                {
                    grdData.DisplayLayout.Bands[0].ColumnFilters[ef.Column.Key].FilterConditions.Add((FilterCondition)fc.Clone());
                }
            }
        }
        public string GetSelectedValue(int type)
        {
            return string.Empty;
        }
        public void ValueSelected(object sender, EventArgs e)
        {
            List<string> values = (List<string>)sender;
            //_reconType = values[0];
            //_pbName = values[1];
            _templateKey = values[0];
            _fromDate = DateTime.Parse(values[1].ToString());
            _toDate = DateTime.Parse(values[2].ToString());
            if (!string.IsNullOrWhiteSpace(_templateKey))
            {
                //BindSetupsCombo();
                btnImport.Enabled = false;
                btnImport.Text = "Fetching PB Data";

                ReconParameters reconParameters = new ReconParameters();
                reconParameters.DTFromDate = _fromDate;
                reconParameters.DTToDate = _toDate;
                reconParameters.TemplateKey = _templateKey;
                DataSet transformedDataSet = ReconUtilities.GetCorrespondingTransformedPBData(reconParameters);
                //bug fixed dataset should contain atleast 1 DataTable
                if (transformedDataSet != null && transformedDataSet.Tables.Count > 0)
                {
                    _dt = transformedDataSet.Tables[0];
                    //Dictionary<ReconFilterType, Dictionary<int, string>> dictReconFilters = ReconPrefManager.ReconPreferences.GetReconFilters(_reconTemplate);
                    //if (dictReconFilters != null)
                    //{
                    //    DataTable dtFiltered = FilteringLogic.GetFilteredPBData(dictReconFilters, _dt);
                    //    _dt = dtFiltered;
                    //}
                    dataReLoaded = true;
                    if (DataReloaded != null)
                    {
                        DataReloaded(null, null);
                    }
                    // grdData.DataSource = _dt;
                    btnImport.Enabled = true;
                    btnImport.Text = "Import";
                    isDataLoaded = true;
                    // BindData();

                }
                else
                {
                    _dt = new DataTable();
                    //grdData.DataSource = _dt;
                    isDataLoaded = false;
                    btnImport.Enabled = true;
                    btnImport.Text = "Import";
                    // BindData();
                    //toolStripImportStatus.Text = "No data found for the given date,Please select the relevant file to import";
                    //MessageBox.Show("No data found for the given date,Please select the relevant file to import");
                }
                _dt.AcceptChanges();
                BindData();
                // btnGroup_Click(null, null);
            }
        }

        public event EventHandler SelectedValueChanged;
        /// <summary>
        /// Wire SelectedValueChanged Event
        /// CHMW-2585	[ManagedRules] - Break down IPluggableUserControl as some of its events are not used in all class inheriting it.
        //  It is just binded to be used later.
        /// </summary>
        public void WireEvent()
        {
            SelectedValueChanged += new EventHandler(ValueChanged);
            SelectedValueChanged(this, new EventArgs());
        }
        /// <summary>
        /// CHMW-2585	[ManagedRules] - Break down IPluggableUserControl as some of its events are not used in all class inheriting it.
        ///  It is just binded to be used later.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ValueChanged(object sender, EventArgs e)
        { }

        #endregion

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {

            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdData);

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

        private void grdData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        //private void SetGridColumns(UltraGrid grid, List<string> listColumns)
        //{
        //    Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
        //    if (listColumns != null)
        //    {
        //        //Hide all columns
        //        foreach (UltraGridColumn col in columns)
        //        {
        //            columns[col.Key].Hidden = true;
        //            col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        }
        //        //Unhide and Set postions for required columns
        //        int visiblePosition = 1;
        //        foreach (string col in listColumns)
        //        {
        //            if (columns.Exists(col))
        //            {
        //                UltraGridColumn column = columns[col];
        //                column.Hidden = false;
        //                column.Header.VisiblePosition = visiblePosition;
        //                column.Width = 80;
        //                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
        //                visiblePosition++;
        //            }
        //        }
        //    }
        //}

        //Narendra Kumar Jangir 2012 Nov 01
        //tick timer to write the xml after fetching symbols for sedols from SMRequest
        void timerSMRequest_Tick(object sender, EventArgs e)
        {
            try
            {

                if (_dt != null && _dt.Rows.Count > 0)
                {
                    dataReLoaded = true;
                    DataSet dsPB = new DataSet();
                    dsPB.Tables.Add(_dt.Copy());
                    dsPB.AcceptChanges();

                    ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(_templateKey);

                    if (template != null)
                    {
                        ReconParameters reconParameters = new ReconParameters();
                        reconParameters.TemplateKey = _templateKey;
                        string reconXmlDataDirectoryPath = ReconUtilities.GetReconDirectoryPath(ReconConstants.ReconDataDirectoryPath, reconParameters);
                        string tempPath = reconXmlDataDirectoryPath + "\\xmls\\Transformation\\Temp\\";
                        string fileName = tempPath + "Recon" + Seperators.SEPERATOR_6 + _fromDate.ToString(ApplicationConstants.DateFormat) + Seperators.SEPERATOR_6 + _toDate.ToString(ApplicationConstants.DateFormat) + ".xml";
                        ReconUtilities.SaveTransformedPBData(dsPB, tempPath, fileName);
                    }
                }
                //enable import button while fetching data and change text from Fetching Data to Import
                EnableImportUI();
                timerSMRequest.Stop();
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
        //enable import button while fetching data and change text from Fetching Data to Import
        private void EnableImportUI()
        {
            try
            {
                btnImport.Enabled = true;
                btnImport.Text = "Import";
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
        //disable import button while fetching data and change text from Import to Fetching Data
        private void DisableImportUI()
        {
            btnImport.Text = "Fetching Data";
            btnImport.Enabled = false;
        }

        void _bgImportData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!_bgImportData.CancellationPending)//checks for cancel request
                {
                    _dt = FileReaderFactory.GetDataTableFromDifferentFileFormats(_fileName);
                    _dt.AcceptChanges();
                    _dt.TableName = "Comparision";
                    //btnTransform_Click(null, null);
                    NewUtilities.AddPrimaryKey(_dt);
                    ReconTemplate reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(_templateKey);
                    //string templateName = ReconUtilities.GetTemplateNameFromTemplateKey(_templateKey);
                    ReconParameters reconParameters = new ReconParameters();
                    reconParameters.DTFromDate = _fromDate;
                    reconParameters.DTToDate = _toDate;
                    reconParameters.TemplateKey = reconTemplate.TemplateKey;
                    string reconXmlDataDirectoryPath = ReconUtilities.GetReconDirectoryPath(ReconConstants.ReconDataDirectoryPath, reconParameters);
                    _dt = ReconManager.TransformData(_dt, reconXmlDataDirectoryPath, reconParameters);
                    //CHMW-3328	Recon not working properly in first attempt
                    ReconManager.Instance.SetDataTableToValidate(_dt);
                    _dt = ReconManager.Instance.FillSMData();

                    _dt = ReconManager.ProcessReconData(_dt, _templateKey, DataSourceType.PrimeBroker, null);
                    //btnGroup_Click(null, null);
                }
                else
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void _bgImportData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)//it doesn't matter if the BG worker ends normally, or gets cancelled,
                {              //both cases RunWorkerCompleted is invoked, so we need to check what has happened
                    MessageBox.Show("Operation has been cancelled!", "Close Trade Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    BindData();
                    dataReLoaded = true;
                    DataSet dsPB = new DataSet();
                    dsPB.Tables.Add(_dt.Copy());
                    dsPB.AcceptChanges();

                    //timer will ticks after every 3 seconds and 
                    //tick timer to write the xml after fetching symbols for sedols from SMRequest
                    timerSMRequest.Enabled = true;                       // Enable the timer
                    timerSMRequest.Interval = 3000;              // Timer will tick after every 3 second
                    timerSMRequest.Start();                             //start timer
                }
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

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}

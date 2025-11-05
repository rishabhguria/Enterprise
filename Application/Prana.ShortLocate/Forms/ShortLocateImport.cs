using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.DAL;
using Prana.ShortLocate.Classes;
using Prana.ShortLocate.Preferences;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.ShortLocate.Forms
{
    public partial class ShortLocateImport : Form
    {
        private RunUploadList _runUploadList = new RunUploadList();
        private static ShortLocateUIPreferences _shortLocatePreferences;

        private string ClientMasterFund = string.Empty;

        private BindingList<ShortLocateOrder> _shortLocateOrderList = new BindingList<ShortLocateOrder>();
        public BindingList<ShortLocateOrder> ShortLocateOrderList
        {
            get { return _shortLocateOrderList; }
            set { _shortLocateOrderList = value; }
        }
        public ShortLocateImport()
        {
            InitializeComponent();
            _shortLocatePreferences = ctrlShortLocatePrefDataManager.GetShortLocatePrefs(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
            if (CustomThemeHelper.ApplyTheme)
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_DAILY_PM_CLIENTUI);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Short Locate Upload", CustomThemeHelper.UsedFont);
            }
            SetButtonsColor();
        }

        private void SetButtonsColor()
        {
            btnSelectFile.ButtonStyle = UIElementButtonStyle.Button3D;
            btnSelectFile.BackColor = Color.DimGray;
            btnSelectFile.ForeColor = Color.White;
            btnSelectFile.UseAppStyling = false;
            btnSelectFile.UseOsThemes = DefaultableBoolean.False;

            btnUploadData.ButtonStyle = UIElementButtonStyle.Button3D;
            btnUploadData.BackColor = Color.DimGray;
            btnUploadData.ForeColor = Color.White;
            btnUploadData.UseAppStyling = false;
            btnUploadData.UseOsThemes = DefaultableBoolean.False;
        }

        public void BindCombo(string Clientmasterfund)
        {
            try
            {
                if (Clientmasterfund != String.Empty)
                    ClientMasterFund = Clientmasterfund;
                int pMCompanyID = RunUploadManager.GetPMCompanyID(CachedDataManager.GetInstance.LoggedInUser.CompanyID);
                _runUploadList = RunUploadManager.GetRunUploadDataByCompanyID(pMCompanyID, true);
                DataTable broker = new DataTable();
                broker.Columns.Add("CounterPartyID");
                broker.Columns.Add("Name");
                object[] rowBroker = new object[2];
                if (_runUploadList.Count > 0)
                {
                    foreach (var uploadlist in _runUploadList)
                    {
                        if (uploadlist.ImportTypeAcronym.Equals(ImportType.ShortLocate))
                        {
                            rowBroker[0] = uploadlist.DataSourceNameIDValue.ID;
                            rowBroker[1] = uploadlist.DataSourceNameIDValue.ShortName;
                            broker.Rows.Add(rowBroker);
                            XsltMapping.Add(uploadlist.DataSourceNameIDValue.ShortName, uploadlist.DataSourceXSLT);
                        }
                    }
                }
                SetBrokerComboDataSource(cmbBroker, broker, "Name", "CounterPartyID");
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

        private void SetBrokerComboDataSource(UltraComboEditor ultraComboEditor, object dataSource, string displayMember, string valueMember)
        {
            try
            {
                ultraComboEditor.Value = null;
                ultraComboEditor.DataSource = dataSource;
                ultraComboEditor.DisplayMember = displayMember;
                ultraComboEditor.ValueMember = valueMember;
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

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private string _fileNameWithPath;
        public string FileNameWithPath
        {
            get { return _fileNameWithPath; }
            set { _fileNameWithPath = value; }
        }

        private Dictionary<string, string> _xsltMapping = new Dictionary<string, string>();
        public Dictionary<string, string> XsltMapping
        {
            get { return _xsltMapping; }
            set { _xsltMapping = value; }
        }

        private void ShortLocateImport_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean CheckAccessPermission = true;
                FileNameWithPath = OpenFileDialogHelper.GetFileNameUsingOpenFileDialog(CheckAccessPermission);


                if (!string.IsNullOrWhiteSpace(FileNameWithPath))
                {
                    if (File.Exists((Application.StartupPath + Path.GetFileName(FileNameWithPath))))
                    {
                        File.Delete(Application.StartupPath + Path.GetFileName(FileNameWithPath));
                    }
                    File.Copy(FileNameWithPath, Application.StartupPath + Path.GetFileName(FileNameWithPath));
                    FileName = Path.GetFileName(FileNameWithPath);
                }
                else
                {
                    //fileNameTobeImported = string.Empty;
                    //MessageBox.Show("Operation canceled by User.", "Exception Report");
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

        private void btnUploadData_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataSource = null;
                if (FileNameWithPath != null)
                    dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(FileNameWithPath);
                if (dataSource == null)
                {
                    System.Windows.Forms.MessageBox.Show("File already in use", "Short Locate", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    return;
                }
                if (dataSource != null && cmbBroker.SelectedItem != null)
                {
                    dataSource = ArrangeTable(dataSource);

                    // now generate the xml of table dataSource
                    if (!Directory.Exists(Application.StartupPath + @"\xmls\Transformation\Temp"))
                        Directory.CreateDirectory(Application.StartupPath + @"\xmls\Transformation\Temp");
                    string serializedPMXml = Application.StartupPath + @"\xmls\Transformation\Temp\serializedXMLforPM.xml";
                    dataSource.WriteXml(serializedPMXml);
                    // get a new mapped xml
                    string mappedxml = Application.StartupPath + @"\xmls\Transformation\Temp\ConvertedXMLforPM.xml";
                    // get the XSLT name only
                    string strXSLTName = XsltMapping[cmbBroker.SelectedItem.ToString()];
                    // set the XSLT path as StartUp Path
                    string dirPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString();
                    string strXSLTStartUpPath = Application.StartupPath + "\\" + dirPath + "\\" + strXSLTName;
                    //                                                  serializedXML,MappedserXML, XSLTPath             
                    string xsdPath = Application.StartupPath + "\\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + "\\" + ApplicationConstants.MappingFileType.PranaXSD.ToString() + @"\ImportShortLocate.xsd";
                    string mappedfilePath = Prana.Utilities.XMLUtilities.XMLUtilities.GetTransformed(serializedPMXml, mappedxml, strXSLTStartUpPath);
                    string tmpError;
                    bool isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(mappedfilePath, xsdPath, "", out tmpError);
                    if (!isValidated)
                    {
                        return;
                    }
                    if (!mappedfilePath.Equals(""))
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(mappedfilePath);

                        //GenerateSMMapping(dirPath, ds);

                        // Now we have arranged and updated XML
                        // as above we inserted "*" in the blank columns, but "*" needs extra treatment, so
                        // again we replace the "*" with blank string, the following looping does the same
                        ReArrangeDataSet(ds);
                        updateShortLocateOrderCollection(ds);
                        ShortLocateDataManager.GetInstance.SaveShortLocateData(_shortLocateOrderList, TransactionSource.ShortLocate);
                    }
                    this.Close();
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

        private void updateShortLocateOrderCollection(DataSet ds)
        {
            try
            {
                DataTable dTable = ds.Tables[0];

                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    ShortLocateOrder slOrder = new ShortLocateOrder();
                    slOrder.Ticker = dTable.Rows[i]["Ticker"].ToString();
                    slOrder.Broker = cmbBroker.SelectedItem.ToString();
                    slOrder.ClientMasterfund = ClientMasterFund;
                    slOrder.TradeQuantity = Convert.ToDouble(dTable.Rows[i]["TradeQuantity"]);
                    slOrder.BorrowSharesAvailable = Convert.ToDouble(dTable.Rows[i]["BorrowSharesAvailable"]);
                    slOrder.BorrowRate = Convert.ToDouble(dTable.Rows[i]["BorrowRate"]);
                    if (_shortLocatePreferences.Rebatefees == "%")
                    {
                        slOrder.BorrowRate = slOrder.BorrowRate / 100;
                    }
                    slOrder.BorrowerId = dTable.Rows[i]["BorrowerId"].ToString();
                    slOrder.BorrowedShare = 0;
                    slOrder.BorrowedRate = 0;
                    slOrder.SODBorrowshareAvailable = Convert.ToDouble(dTable.Rows[i]["BorrowSharesAvailable"]);
                    slOrder.SODBorrowRate = slOrder.BorrowRate;
                    slOrder.StatusSource = dTable.Rows[i]["StatusSource"].ToString();
                    _shortLocateOrderList.Add(slOrder);
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

        private DataTable ArrangeTable(DataTable dataSource)
        {
            try
            {
                // what XML we will generate, all the tagname will be like COL1,COL2 .                
                dataSource.TableName = "PositionMaster";

                // update the Table columns value with "*" where columns value blank in the excel sheet
                // when we generate the XML for that table, the blank coluns do not comes in the generated XML
                // the indexing of the generated XML changed because of blank columns
                // so defalut value of the columns will be  "*"
                for (int irow = 0; irow < dataSource.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dataSource.Columns.Count; icol++)
                    {
                        string val = dataSource.Rows[irow].ItemArray[icol].ToString();
                        if (String.IsNullOrEmpty(val.Trim()))
                        {
                            dataSource.Rows[irow][icol] = "*";
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
            return dataSource;
        }

        private void ReArrangeDataSet(DataSet ds)
        {
            try
            {
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string val = ds.Tables[0].Rows[irow].ItemArray[icol].ToString();
                        if (val.Equals("*"))
                        {
                            ds.Tables[0].Rows[irow][icol] = string.Empty;
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


    }
}

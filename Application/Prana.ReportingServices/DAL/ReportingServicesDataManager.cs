using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Interfaces;
using Prana.AutomationHandlers;
using System.IO;
using Prana.Utilities.XMLUtilities;
using System.Data.Common;

namespace Prana.ReportingServices
{
    public class ReportingServicesDataManager
    {
        static ReportingServicesDataManager()
        {
            if (_dicClientDetails == null)
                fillClientDetailsDictionary();
            if (baseSettingsObj == null)
                GetBasicDetail();
            if (ImportXMLSettings == null)
                getDataFromImportXMLFile();
        }

        const string ReportsConnectionString = "RiskReportsConnectionString";

        #region Property Section

        // static BaseSettings baseSettingsObj = null;
        private static BaseSettings baseSettingsObj=null;

        public static BaseSettings BaseSettings
        {
            get
            {
                if (baseSettingsObj == null)
                    GetBasicDetail(); 
                return baseSettingsObj;
            }
            set { baseSettingsObj = value; }
        }
	
        //Global Beacuse It Will Be The Same For each Client key:--ClientName
        static Dictionary<string, ClientSettings> _dicClientDetails;
        public static Dictionary<string, ClientSettings> DicClientDetails
        {
            get
            {
                if (_dicClientDetails == null)
                    fillClientDetailsDictionary(); 
                return _dicClientDetails;
            }
            set { _dicClientDetails = value; }
        }

        private static  ClientSettingsPref _importXMLSettings;

        public static ClientSettingsPref ImportXMLSettings
        {
            get { return _importXMLSettings; }
            set { _importXMLSettings = value; }              
        }

        static Dictionary<string, ClientSettings> _dicInputSettings;
        public static Dictionary<string, ClientSettings> DicInputSettings
        {
            get
            {
                if (_dicInputSettings == null)
                    getDataFromImportXMLFile();
                return _dicInputSettings;
            }
            set { _dicInputSettings = value; }
        }

        #endregion        

        #region Functionality Section

        public static BaseSettings GetBasicDetail()
        {

            try
            {

                baseSettingsObj = new BaseSettings();

                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("GetBasicDetails"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        baseSettingsObj.FilePath = row[0].ToString();
                        baseSettingsObj.SupoortMailID = row[1].ToString();
                        baseSettingsObj.SupportMailPassword = row[2].ToString();
                        baseSettingsObj.HostID = row[3].ToString();
                        baseSettingsObj.MailSubject = row[4].ToString();
                        baseSettingsObj.MailBody = row[5].ToString();
                        baseSettingsObj.CronExpression = row[6].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return baseSettingsObj;

        }

        public static void GetClientDetail(ClientSettings clientSettingsObj)
        {
            
            ClientSettings _clientSetting;
            String clientname = clientSettingsObj.ClientName.Trim().ToUpper();
            if (_dicClientDetails.ContainsKey(clientname))
            {
                //Details From DataBase
                _clientSetting = (ClientSettings)_dicClientDetails[clientname].Clone();
            }
            else
            {
                throw new Exception("Setting for this client "+clientname +" does not exist in DB ");
            }
            
            //Adding Db details to Object
            //clientSettingsObj = (ClientSettings)_clientSetting.Clone();
             clientSettingsObj.BaseSettings = _clientSetting.BaseSettings;
             clientSettingsObj.ClientInformation = _clientSetting.ClientInformation;
            clientSettingsObj.ClientName = _clientSetting.ClientName;
             clientSettingsObj.ClientID = _clientSetting.ClientID;
            clientSettingsObj.DataBaseSettings = _clientSetting.DataBaseSettings;
            clientSettingsObj.DeliveryMethod = _clientSetting.DeliveryMethod;
            clientSettingsObj.EmailID = _clientSetting.EmailID;
            //clientSettingsObj.ThirdPartyNames=
           // clientSettingsObj.FileFormatter = _clientSetting.FileFormatter;
            clientSettingsObj.InputDataLocationPath = _clientSetting.InputDataLocationPath;
            if (clientSettingsObj.Date == DateTime.MinValue)
                clientSettingsObj.Date = DateTime.Now;
            if(clientSettingsObj.RiskPreferences.EndDate== DateTime.MinValue)
            {
                clientSettingsObj.RiskPreferences.EndDate = DateTime.Now;
            }
            if (ColumnManager.DicRRTypeColumns.ContainsKey(clientSettingsObj.ReportType.ToString()))
                clientSettingsObj.DicColumns = ColumnManager.DicRRTypeColumns[clientSettingsObj.ReportType.ToString()];

            //return clientSettingsObj;
        }

        static void fillClientDetailsDictionary()
        {
            try
            {

                _dicClientDetails = new Dictionary<string, ClientSettings>();

                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);
                ClientSettings clientSettingsObj;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("GetAllClientDetails"))
                {

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        clientSettingsObj = new ClientSettings();
                        clientSettingsObj.ClientID = Convert.ToInt32(row[0]);
                        clientSettingsObj.ClientName = row[1].ToString();
                        clientSettingsObj.DeliveryMethod = row[2].ToString();

                        Array arrFileFormates = Enum.GetValues(typeof(AutomationEnum.FileFormate));
                        foreach (AutomationEnum.FileFormate fileFormate in arrFileFormates)
                        {
                            if (fileFormate.ToString().ToLower() == row[3].ToString().ToLower())
                            {
                                //clientSettingsObj.FileFormatter = fileFormate;
                                break;
                            }
                        }

                        #region Client DataBase Settings Region

                        DBSettings clientDBSettings = new DBSettings();
                        clientDBSettings.ClientDB = row[4].ToString();
                        clientDBSettings.SecDB = row[5].ToString();
                        clientSettingsObj.DataBaseSettings = clientDBSettings;
                        ISecMasterServices secMaster = new SecurityMasterNew.SecMasterCacheManager(clientDBSettings.SecDB);
                        SecurityMasterWrapper.AddSecMaster(clientSettingsObj.ClientID, secMaster, clientDBSettings.SecDB);

                        #endregion

                        clientSettingsObj.EmailID = row[6].ToString();

                        #region ClientInfo Settings

                        ClientInfo _clientSpecificDetail = new ClientInfo();
                        _clientSpecificDetail.UserId = Convert.ToInt32(row[7]);
                        _clientSpecificDetail.TradingAccountID = Convert.ToInt32(row[8]);
                        _clientSpecificDetail.CompanyID = Convert.ToInt32(row[9]);

                        #endregion

                        clientSettingsObj.ClientInformation = _clientSpecificDetail;

                        if (clientSettingsObj.InputDataLocationType == AutomationEnum.InputOutputType.DB)
                            clientSettingsObj.InputDataLocationPath = clientDBSettings.ClientDB;


                        //else if (clientSettingsObj.InputDataLocationType == AutomationEnum.InputOutputType.FileSystem)
                        //    clientSettingsObj.InputDataLocationPath = baseSettingsObj.FilePath; 

                        //if(clientSettingsObj.ReportType==AutomationEnum.ReprotTypeEnum.RiskReport)
                        //   clientSettingsObj.OutputReportLocationPath = baseSettingsObj.FilePath;

                        if (baseSettingsObj == null)
                            GetBasicDetail();

                        clientSettingsObj.BaseSettings = baseSettingsObj;

                        _dicClientDetails.Add(clientSettingsObj.ClientName.Trim().ToUpper(), clientSettingsObj);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        static void SetAllSecMasters()
        {





        }

        public static void Refresh()
        {
            try
            {
                GetBasicDetail();

                fillClientDetailsDictionary();

                getDataFromImportXMLFile();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }     

        public static void getDataFromImportXMLFile()
        {            
            ClientSettingsPref ObjectDeSerialize = new ClientSettingsPref();
            try
            {
              


                if (baseSettingsObj == null)
                    GetBasicDetail();
                string ImportClientSettingXmlFilePath = getImportXMLFilePath();
                string InputFilesPath = baseSettingsObj.FilePath + "\\Input files\\";
                Directory.CreateDirectory(ImportClientSettingXmlFilePath);
                Directory.CreateDirectory(InputFilesPath);

                //XMLUtilities.SerializeToXMLFile<ClientSettingsPref>(ObjectToSerialize, ImportClientSettingXmlFilePath);

                ImportClientSettingXmlFilePath += "settings.xml";
                if (!File.Exists(ImportClientSettingXmlFilePath))
                    throw new Exception("Input XML File does'nt Exist At Location:--" + ImportClientSettingXmlFilePath);
               
                ObjectDeSerialize = XMLUtilities.DeserializeFromXMLFile<ClientSettingsPref>(ImportClientSettingXmlFilePath);
                ObjectDeSerialize.LoadDictionary();
                ImportXMLSettings = ObjectDeSerialize;

                #region To create Dictionary Of ImportSettings With Key:--SettingName

                DicInputSettings=new Dictionary<string,ClientSettings>();
                foreach (ClientSettings _clientSettings in ObjectDeSerialize.ClientSettingsList)
                    DicInputSettings.Add(_clientSettings.ClientSettingName, _clientSettings);

                #endregion

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            
        }        

        public static string getImportXMLFilePath()
        {           
            try
            {
                if (baseSettingsObj == null)
                    GetBasicDetail();

                string ImportClientSettingXmlFilePath = baseSettingsObj.FilePath + "\\ImportClientSettingXml\\";

                return ImportClientSettingXmlFilePath;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        internal static void SaveCronExpression(string cronExpression)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(ReportsConnectionString);
                //DbCommand cmd
                int rowsEffected = db.ExecuteNonQuery("SaveCronExpression", new object[] {cronExpression});
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        
    }
}

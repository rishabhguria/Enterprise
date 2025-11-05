using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ClientSettings
    {

        private BaseSettings _baseSettings;
        [XmlIgnore]
        public BaseSettings BaseSettings
        {
            get { return _baseSettings; }
            set { _baseSettings = value; }
        }

        private ClientInfo _clientInfo;
        [XmlIgnore]
        public ClientInfo ClientInformation
        {
            get { return _clientInfo; }
            set { _clientInfo = value; }
        }


        private int _clientID;
        [XmlIgnore]
        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        private string _clientName;

        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }


        private string _deliveryMethod;

        public string DeliveryMethod
        {
            get { return _deliveryMethod; }
            set { _deliveryMethod = value; }
        }

        private AutomationEnum.FileFormat _fileFormatter;

        public AutomationEnum.FileFormat FileFormatter
        {
            get { return _fileFormatter; }
            set { _fileFormatter = value; }
        }

        private AutomationEnum.InputOutputType _inputDataLocationType;

        public AutomationEnum.InputOutputType InputDataLocationType
        {
            get { return _inputDataLocationType; }
            set { _inputDataLocationType = value; }
        }

        private string _inputDataLocationPath;

        public string InputDataLocationPath
        {
            get { return _inputDataLocationPath; }
            set { _inputDataLocationPath = value; }
        }

        private string _emailID;

        public string EmailID
        {
            get { return _emailID; }
            set { _emailID = value; }
        }


        private string _outputReportLocationPath;

        public string OutputReportLocationPath
        {
            get { return _outputReportLocationPath; }
            set { _outputReportLocationPath = value; }
        }

        private string _reportFileName;

        public string ReportFileName
        {
            get { return _reportFileName; }
            set { _reportFileName = value; }
        }

        private ClientRiskPref _riskPref;

        public ClientRiskPref RiskPreferences
        {
            get { return _riskPref; }
            set { _riskPref = value; }
        }


        //Currently Only Two reportType is Hare Named:---RiskReport,Internal
        private AutomationEnum.ReprotTypeEnum _reportType;

        public AutomationEnum.ReprotTypeEnum ReportType
        {
            get { return _reportType; }
            set { _reportType = value; }
        }


        private List<FileSetting> _fileSettings = new List<FileSetting>();
        [XmlIgnore]
        public List<FileSetting> FileSettings
        {
            get { return _fileSettings; }
            set { _fileSettings = value; }
        }


        private AutomationEnum.ImportTypeEnum _importType;

        public AutomationEnum.ImportTypeEnum ImportType
        {
            get { return _importType; }
            set { _importType = value; }
        }

        private string _thirdPartyName;
        public string ThirdPartyNames
        {
            get { return _thirdPartyName; }
            set { _thirdPartyName = value; }
        }

        private int _thirdPartyID;

        public int ThirdPartyID
        {
            get { return _thirdPartyID; }
            set { _thirdPartyID = value; }
        }

        private DBSettings _dataBaseSettings;
        [XmlIgnore]
        public DBSettings DataBaseSettings
        {
            get { return _dataBaseSettings; }
            set { _dataBaseSettings = value; }
        }

        private string _clientSettingName;

        public string ClientSettingName
        {
            get { return _clientSettingName; }
            set { _clientSettingName = value; }
        }

        [NonSerialized]
        private Dictionary<string, string> _dicColumns;
        [XmlIgnore]
        public Dictionary<string, string> DicColumns
        {
            get { return _dicColumns; }
            set { _dicColumns = value; }
        }

        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }

        public override string ToString()
        {
            string[] data1 = _inputDataLocationPath.Split(';');
            string nameofpath = "";
            if (data1.Length > 0)
            {
                nameofpath = data1[0];
            }
            string retData = "\n Senario Name = " + _clientSettingName + " \n Client Name " + _clientName + "\n Report Type=" + _reportType + "" + "\n Import Type=" + _importType + "\n Location Path=" + nameofpath;
            if (_reportType != AutomationEnum.ReprotTypeEnum.Internal)
            {
                retData = retData + "\n " + _riskPref.ToString();
            }
            return retData;
        }

        private bool isNewRow;
        [XmlIgnore]
        public bool IsNewRow
        {
            get { return isNewRow; }
            set { isNewRow = value; }
        }

    }
}

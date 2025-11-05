using System;
//using Prana.Utilities.ImportExportUtilities;
namespace Prana.BusinessObjects
{
    [Serializable]
    public class ImportFileDetail
    {

        private string _importFilePath = string.Empty;
        public string ImportFilePath
        {
            get { return _importFilePath; }
            set { _importFilePath = value; }
        }
        private string _namingConvention = string.Empty;
        public string NamingConvention
        {
            get { return _namingConvention; }
            set { _namingConvention = value; }
        }
        //We have 
        private AutomationEnum.DataSourceFileFormat _fileFormat = AutomationEnum.DataSourceFileFormat.Csv;

        public AutomationEnum.DataSourceFileFormat FileFormat
        {
            get { return _fileFormat; }
            set { _fileFormat = value; }
        }


    }
}

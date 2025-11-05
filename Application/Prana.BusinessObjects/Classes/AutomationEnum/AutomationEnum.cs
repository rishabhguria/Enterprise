using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class AutomationEnum
    {
        public enum ImportTypeEnum
        {
            Position,
            DailyCash,
            MarkPrice,
            FXRate,
            CashActivity,
            CashDividend,
            OMIImport
        }
        public enum FileTypeEnum
        {
            XSD,
            XSLT
        }
        public enum InOutDirectories
        {
            Input,
            Output,
            TempXML
        }
        public enum ReprotTypeEnum
        {
            RiskReport,
            Internal,
            StressTest
        }
        public enum InputOutputType
        {
            DB,
            FileSystem
        }
        public enum FileFormat
        {
            csv,
            xls,
            pdf,
            txt
        }

        //We have to add this enum because this enum is used in recon perefernce and we cant delete existing preference
        public enum DataSourceFileFormat : ushort
        {
            Excel = 0,
            Csv = 1,
            Text = 2,
            FixPlace = 3,
            Default = 4
        }
    }
}

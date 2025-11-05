using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyBatchCommon : ThirdPartyBatchExtensions
    {
        private int _ThirdPartyBatchId;
        private string _Description;
        private int _ThirdPartyTypeId;
        private int _ThirdPartyId;
        private int _CounterPartyID;
        private int _ThirdPartyFormatId;
        private bool _IsLevel2Data;
        private bool _Active;
        private bool _IncludeSent;
        private bool? _AllowedFixTransmission;
        private bool _FileEnabled;
        private int _ThirdPartyCompanyId;
        private int _GnuPGId;
        private int _FtpId;
        private int _EmailDataId;
        private int _EMailLogId;

        [Browsable(false)]
        public int ThirdPartyBatchId
        {
            get { return _ThirdPartyBatchId; }
            set { _ThirdPartyBatchId = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [Browsable(false)]
        public int ThirdPartyTypeId
        {
            get { return _ThirdPartyTypeId; }
            set { _ThirdPartyTypeId = value; }
        }

        [Browsable(false)]
        public int ThirdPartyId
        {
            get { return _ThirdPartyId; }
            set { _ThirdPartyId = value; }
        }

        [Browsable(false)]
        public int CounterPartyID
        {
            get { return _CounterPartyID; }
            set { _CounterPartyID = value; }
        }

        [Browsable(false)]
        public int ThirdPartyFormatId
        {
            get { return _ThirdPartyFormatId; }
            set { _ThirdPartyFormatId = value; }
        }

        [Browsable(true)]
        public bool IsLevel2Data
        {
            get { return _IsLevel2Data; }
            set { _IsLevel2Data = value; }
        }

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        public bool IncludeSent
        {
            get { return _IncludeSent; }
            set { _IncludeSent = value; }
        }

        public bool? AllowedFixTransmission
        {
            get { return _AllowedFixTransmission; }
            set { _AllowedFixTransmission = value; }
        }

        [Browsable(false)]
        public bool FileEnabled
        {
            get { return _FileEnabled; }
            set { _FileEnabled = value; }
        }

        [Browsable(false)]
        public int ThirdPartyCompanyId
        {
            get { return _ThirdPartyCompanyId; }
            set { _ThirdPartyCompanyId = value; }
        }

        [Browsable(false)]
        public int GnuPGId
        {
            get { return _GnuPGId; }
            set { _GnuPGId = value; }
        }

        [Browsable(false)]
        public int FtpId
        {
            get { return _FtpId; }
            set { _FtpId = value; }
        }

        [Browsable(false)]
        public int EmailDataId
        {
            get { return _EmailDataId; }
            set { _EmailDataId = value; }
        }

        [Browsable(false)]
        public int EmailLogId
        {
            get { return _EMailLogId; }
            set { _EMailLogId = value; }
        }

        [Browsable(false)]
        public string LayoutFile
        {
            get
            {
                return string.Format(".\\Prana Preferences\\EOD\\Layouts\\{0}.layout", this.ThirdPartyBatchId);
            }
            set { }
        }
    }
}

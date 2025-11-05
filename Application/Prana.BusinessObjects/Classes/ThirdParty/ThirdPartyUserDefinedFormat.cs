using Prana.Global.Utilities;
using System;
using System.Data;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyUserDefinedFormat
    {
        public string StartupPath;

        public ThirdPartyFlatFileSaveDetail FileSpec;
        public ThirdPartyFileFormat Formatter;

        public DateTime Date;

        public string FileName;
        public string FilePath;
        public string ArchivePath;
        public string LogPath;
        public string SymbolToken;

        public ThirdPartyUserDefinedFormat()
        {
        }
        public bool ValidPath
        {
            get
            {
                return System.IO.Directory.Exists(FilePath);
            }
            set { }
        }
    }
}

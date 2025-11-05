using System;
using System.Data;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class corresponding to table T_ImportFileLog in DB. Only ImportFilename added other columns can be added later
    /// </summary>
    [Serializable]
    public class ImportFileLog
    {
        public ImportFileLog()
        {
        }
        public ImportFileLog(DataRow dr)
        {
            if (!string.IsNullOrEmpty(dr["ImportFileID"].ToString()))
                _importFileID = Convert.ToInt32(dr["ImportFileID"]);
            if (!string.IsNullOrEmpty(dr["ImportFileName"].ToString()))
                _importFileName = dr["ImportFileName"].ToString();
        }

        int _importFileID;

        public virtual int ImportFileID
        {
            get { return _importFileID; }
            set { _importFileID = value; }
        }
        String _importFileName = string.Empty;

        public virtual String ImportFileName
        {
            get { return _importFileName; }
            set { _importFileName = value; }
        }
    }
}

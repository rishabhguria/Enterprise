using Csla;
using System;

namespace Prana.PM.BLL
{
    /// <summary>
    /// TODO : Check the performance of this class, as a lot of objects will be created for this class,
    /// We might need to use a datatable instead of 
    /// </summary>

    [Serializable()]
    public class UploadedData : BusinessBase<UploadedData>
    {
        private int _uploadID;

        public int UploadID
        {
            get { return _uploadID; }
            set { _uploadID = value; }
        }

        private int _dataSourceColumnID;

        public int DataSourceColumnID
        {
            get { return _dataSourceColumnID; }
            set { _dataSourceColumnID = value; }
        }

        private int _dataRowID;

        public int DataRowID
        {
            get { return _dataRowID; }
            set { _dataRowID = value; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }


        protected override object GetIdValue()
        {
            return _uploadID;
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        private int _pmCompanyID;
        //TODO : Need to remove the pmcompanyid from this table untill any info is required 
        // for joining.
        public int PMCompanyID
        {
            get { return _pmCompanyID; }
            set { _pmCompanyID = value; }
        }

    }
}

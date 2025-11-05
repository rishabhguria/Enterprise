using Csla;
using System;
namespace Prana.PM.BLL
{

    [Serializable()]
    public class Column : BusinessBase<Column>
    {
        public Column()
        {

        }

        public Column(string name, string type)
        {
            this._columnName = name;
            this._type = type;
        }


        private string _columnName;

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }


        protected override object GetIdValue()
        {
            return _columnName;
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }
    }

    [Serializable()]
    public class DataSourceColumn : Column
    {
        private int _dataSourceColumnID;

        public int DataSourceColumnID
        {
            get { return _dataSourceColumnID; }
            set { _dataSourceColumnID = value; }
        }

        //private string _columnName;

        //public string ColumnName
        //{
        //    get { return _columnName; }
        //    set { _columnName = value; }
        //}

        private int _columnType;

        public int ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; }
        }

        private int _applicationColumnID;

        public int ApplicationColumnID
        {
            get { return _applicationColumnID; }
            set { _applicationColumnID = value; }
        }


        private bool _isRequiredInUpload;

        public bool IsRequiredInUpload
        {
            get { return _isRequiredInUpload; }
            set { _isRequiredInUpload = value; }
        }

        private int _columnSequenceNumber;

        public int ColumnSequenceNumber
        {
            get { return _columnSequenceNumber; }
            set { _columnSequenceNumber = value; }
        }



    }
}

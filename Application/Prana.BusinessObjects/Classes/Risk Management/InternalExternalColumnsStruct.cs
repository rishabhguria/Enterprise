namespace Prana.BusinessObjects
{
    public class InternalExternalColumnsStruct : IKeyable
    {
        private int _columnID;

        public int ColumnID
        {
            get { return _columnID; }
            set { _columnID = value; }
        }

        private string _internalColumnName;

        public string InternalColumnName
        {
            get { return _internalColumnName; }
            set { _internalColumnName = value; }
        }

        private string _externalColumnName;

        public string ExternalColumnName
        {
            get { return _externalColumnName; }
            set { _externalColumnName = value; }
        }

        #region IKeyable Members

        public string GetKey()
        {
            return ColumnID.ToString();
        }

        public void Update(IKeyable item)
        {
            InternalExternalColumnsStruct updatedData = item as InternalExternalColumnsStruct;
            this.ExternalColumnName = updatedData.ExternalColumnName;
            this.InternalColumnName = updatedData.InternalColumnName;
        }

        #endregion
    }
}

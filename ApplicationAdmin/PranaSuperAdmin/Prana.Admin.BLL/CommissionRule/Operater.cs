namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for operater.
    /// </summary>
    public class Operater
    {
        #region Private Members
        private int _operatorID = int.MinValue;
        private string _name = string.Empty;
        #endregion
        public Operater()
        {

        }
        #region Private Members
        public int OperatorID
        {
            get { return _operatorID; }
            set { _operatorID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion
        public Operater(int operatorID, string name)
        {
            _operatorID = operatorID;
            _name = name;
        }
    }
}

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Unit.
    /// </summary>
    public class Unit
    {
        #region Private members

        private int _unitID = int.MinValue;
        private string _unitName = string.Empty;

        #endregion

        public Unit()
        {
        }

        public Unit(int unitID, string unitName)
        {
            _unitID = unitID;
            _unitName = unitName;
        }

        #region Properties

        public int UnitID
        {
            get { return _unitID; }
            set { _unitID = value; }
        }

        public string UnitName
        {
            get { return _unitName; }
            set { _unitName = value; }
        }

        #endregion
    }
}

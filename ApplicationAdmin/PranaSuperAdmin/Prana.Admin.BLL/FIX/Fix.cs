namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Fix.
    /// </summary>
    public class Fix
    {
        #region Private members

        private int _fixID = int.MinValue;
        private string _fixVersion = string.Empty;

        #endregion

        #region Constructors

        public Fix()
        {

        }

        public Fix(int fixID, string fixVersion)
        {
            _fixID = fixID;
            _fixVersion = fixVersion;
        }

        #endregion

        #region Properties

        public int FixID
        {
            get { return _fixID; }
            set { _fixID = value; }
        }

        public string FixVersion
        {
            get { return _fixVersion; }
            set { _fixVersion = value; }
        }

        #endregion

    }
}

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for FixCapability.
    /// </summary>
    public class FixCapability
    {
        #region Private members

        private int _fixcapabilityID = int.MinValue;
        private string _name = string.Empty;

        #endregion

        #region Constructors

        public FixCapability()
        {

        }

        public FixCapability(int fixCapabilityID, string name)
        {
            _fixcapabilityID = fixCapabilityID;
            _name = name;

        }


        #endregion

        #region Properties

        public int FixCapabilityID
        {
            get { return _fixcapabilityID; }
            set { _fixcapabilityID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
    }
}

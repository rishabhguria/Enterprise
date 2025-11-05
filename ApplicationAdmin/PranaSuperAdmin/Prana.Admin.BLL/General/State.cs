namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for State.
    /// </summary>
    public class State
    {
        #region Private
        int _stateID = int.MinValue;
        string _state = string.Empty;
        int _countryID = int.MinValue;
        #endregion

        #region Constructors
        public State()
        {
        }
        public State(int stateID, string state, int countryID)
        {
            _stateID = stateID;
            _state = state;
            _countryID = countryID;
        }
        #endregion

        #region Properties

        public int StateID
        {
            get { return _stateID; }
            set { _stateID = value; }
        }

        public string StateName
        {
            get { return _state; }
            set { _state = value; }
        }

        public int CountryID
        {
            get { return _countryID; }
            set { _countryID = value; }
        }
        #endregion
    }
}

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionCalculation.
    /// </summary>


    public class CommissionCalculation
    {
        #region Private Members
        private int _calculationID = int.MinValue;
        private string _calculationType = string.Empty;
        #endregion
        public CommissionCalculation()
        {

        }
        public CommissionCalculation(int calculationID, string calculationType)
        {
            _calculationID = calculationID;
            _calculationType = calculationType;
        }

        #region Public Properties
        public int CommissionCalculationID
        {
            get { return _calculationID; }
            set { _calculationID = value; }
        }
        public string CalculationType
        {
            get { return _calculationType; }
            set { _calculationType = value; }
        }
        #endregion

    }
}

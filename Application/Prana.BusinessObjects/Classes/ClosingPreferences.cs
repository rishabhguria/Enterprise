using Prana.LogManager;
using System;


namespace Prana.BusinessObjects
{
    [Serializable]
    public class ClosingPreferences : IPreferenceData, IDisposable
    {

        private ClosingMethodology _closingMethodology = new ClosingMethodology();

        public ClosingMethodology ClosingMethodology
        {
            get { return _closingMethodology; }
            set { _closingMethodology = value; }
        }


        private bool _isFetchDataAutomatically = true;
        public bool IsFetchDataAutomatically
        {
            get { return _isFetchDataAutomatically; }
            set { _isFetchDataAutomatically = value; }
        }


        private PostTradeEnums.DateType _dateType;
        public PostTradeEnums.DateType DateType
        {
            get { return _dateType; }
            set { _dateType = value; }
        }


        private int _priceRoundOffDigits;
        public int PriceRoundOffDigits
        {
            get { return _priceRoundOffDigits; }
            set { _priceRoundOffDigits = value; }
        }

        private decimal _autoOptExerciseValue;
        public decimal AutoOptExerciseValue
        {
            get { return _autoOptExerciseValue; }
            set { _autoOptExerciseValue = value; }
        }

        public int QtyRoundoffDigits { get; set; }

        public bool CopyOpeningTradeAttributes { get; set; } 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (ClosingMethodology != null)
                        ClosingMethodology.Dispose();
                    if (_closingMethodology != null)
                        _closingMethodology.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}

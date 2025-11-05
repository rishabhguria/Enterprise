using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
//using Prana.FeedSubscriber;

namespace Prana.ExpnlService
{
    public class CalculatorProvider : ICalculatorProvider
    {
        private List<ICalculator> _placeholderForCalculators;


        private ICalculator _equityCalculator = null;
        private ICalculator _optionsCalculator = null;
        private ICalculator _futuresCalculator = null;
        private ICalculator _fXCalculator = null;
        private ICalculator _fixedIncomeCalculator = null;


        internal CalculatorProvider(Dictionary<int, DateTime> auecWiseAdjustedCurrentDates, Dictionary<int, DateTime> clearanceTimes)
        {
            _placeholderForCalculators = new List<ICalculator>();
            _equityCalculator = new EquityCalculator(auecWiseAdjustedCurrentDates, clearanceTimes);
            _fXCalculator = new FXCalculator(auecWiseAdjustedCurrentDates, clearanceTimes);
            _optionsCalculator = new OptionsCalculator(auecWiseAdjustedCurrentDates, clearanceTimes);
            _futuresCalculator = new FuturesCalculator(auecWiseAdjustedCurrentDates, clearanceTimes);
            _fixedIncomeCalculator = new FixedIncomeCalculator(auecWiseAdjustedCurrentDates, clearanceTimes);

            _placeholderForCalculators.Add(_equityCalculator);
            _placeholderForCalculators.Add(_fXCalculator);
            _placeholderForCalculators.Add(_optionsCalculator);
            _placeholderForCalculators.Add(_futuresCalculator);
            _placeholderForCalculators.Add(_fixedIncomeCalculator);
        }


        #region ICalculatorProvider Members


        public ICalculator GetCalculator(EPnLClassID classID)
        {
            try
            {
                switch (classID)
                {
                    case EPnLClassID.EPnLOrderEquity:
                    case EPnLClassID.EPnLOrderEquitySwap:
                        return _equityCalculator;

                    case EPnLClassID.EPnLOrderOption:
                        return _optionsCalculator;

                    case EPnLClassID.EPnLOrderFuture:
                        return _futuresCalculator;

                    case EPnLClassID.EPnLOrderFX:
                        return _fXCalculator;

                    case EPnLClassID.EPnLOrderFXForward:
                        return _fXCalculator;

                    case EPnLClassID.EPnLOrderFixedIncome:
                        return _fixedIncomeCalculator;

                    default:
                        //Call EquitiesCalculator by setting CalculatorContext
                        return _equityCalculator;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return _equityCalculator;
            }
        }

        public void UpdateCurrentDatesAndClearanceTime(Dictionary<int, DateTime> updatedAuecWiseAdjustedCurrentDates, Dictionary<int, DateTime> updatedClearanceTimes)
        {
            try
            {
                //update each calculator datetimes
                foreach (ICalculator calculator in _placeholderForCalculators)
                {
                    calculator.AUECWiseAdjustedDateTime = updatedAuecWiseAdjustedCurrentDates;
                    calculator.ClearanceDateTime = updatedClearanceTimes;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        #endregion
    }
}

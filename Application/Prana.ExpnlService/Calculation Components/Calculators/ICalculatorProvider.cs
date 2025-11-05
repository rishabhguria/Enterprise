using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    public interface ICalculatorProvider
    {
        ICalculator GetCalculator(EPnLClassID classID);
        void UpdateCurrentDatesAndClearanceTime(Dictionary<int, DateTime> updatedAuecWiseAdjustedCurrentDates, Dictionary<int, DateTime> updatedClearanceTimes);
    }
}

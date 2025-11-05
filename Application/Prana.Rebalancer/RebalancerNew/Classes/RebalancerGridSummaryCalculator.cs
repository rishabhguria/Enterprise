using Infragistics.Windows.DataPresenter;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.LogManager;
using System;
using System.Globalization;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    class RebalancerGridSummaryCalculator : SummaryCalculator
    {
        private bool _multipleAccounts;
        private int _previousAccountId;
        private RebalancerEnums.CalculationLevel _rebalCalculationLevel;
        private decimal _sum;
        public override void Aggregate(object dataValue, SummaryResult summaryResult, Record record)
        {
            try
            {
                string fieldName = summaryResult.SourceField.Name;
                DataRecord currentRecord = record as DataRecord;
                int tempAccountId = int.MinValue;
                if (currentRecord != null)
                {

                    tempAccountId = (int)currentRecord.Cells[RebalancerConstants.COL_AccountId].Value;
                    decimal val;
                    decimal.TryParse(currentRecord.Cells[fieldName].Value.ToString(), out val);
                    _sum += val;
                }
                if (_previousAccountId != Int32.MinValue && !(_previousAccountId.Equals(tempAccountId)))
                {
                    _multipleAccounts = true;
                }
                _previousAccountId = tempAccountId;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public override void BeginCalculation(SummaryResult summaryResult)
        {
            _multipleAccounts = false;
            _previousAccountId = Int32.MinValue;
            _sum = 0;
            _rebalCalculationLevel = RebalancerCache.Instance.GetCalculationLevel();
        }

        public override bool CanProcessDataType(Type dataType)
        {
            return dataType == typeof(double);
        }

        public override string Description
        {
            get { return string.Empty; }
        }

        public override object EndCalculation(SummaryResult summaryResult)
        {
            if (_rebalCalculationLevel.Equals(RebalancerEnums.CalculationLevel.Account) && _multipleAccounts)
                return null;
            return summaryResult.SourceField.Name.Contains("Percentage")
                ? _sum.ToString("#,0.00\\%", CultureInfo.InvariantCulture)
                : _sum.ToString("#,0", CultureInfo.InvariantCulture);
        }

        public override string Name
        {
            get { return "RebalancerGridSummaryCalculator"; }
        }
    }
}

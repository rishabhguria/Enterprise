using Infragistics.Windows.DataPresenter;
using Prana.Rebalancer.Converters;
using System;
using System.Globalization;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    class StringsValuesSummaryCalculator : SummaryCalculator
    {
        private bool _multipleStrings;
        private string _previousString;
        private readonly AccountIDToAccountNameConverter _accountIdToAccountNameConverter = new AccountIDToAccountNameConverter();
        public override void Aggregate(object dataValue, SummaryResult summaryResult, Record record)
        {
            string fieldName = summaryResult.SourceField.Name;
            DataRecord currentRecord = record as DataRecord;
            string tempValue = null;
            if (currentRecord != null)
            {
                switch (fieldName)
                {
                    case RebalancerConstants.COL_AccountId:
                        tempValue = _accountIdToAccountNameConverter.Convert(currentRecord.Cells[fieldName].Value, null, null, CultureInfo.CurrentCulture).ToString();
                        break;
                    default:
                        tempValue = currentRecord.Cells[fieldName].Value != null ? currentRecord.Cells[fieldName].Value.ToString() : string.Empty;
                        break;
                }
            }
            if (_previousString != null && !string.IsNullOrWhiteSpace(tempValue) && !(_previousString.Equals(tempValue)))
            {
                _multipleStrings = true;
            }
            _previousString = string.IsNullOrWhiteSpace(tempValue) ? _previousString : tempValue;
        }

        public override void BeginCalculation(SummaryResult summaryResult)
        {
            _multipleStrings = false;
            _previousString = null;
        }

        public override bool CanProcessDataType(Type dataType)
        {
            return dataType == typeof(string);
        }

        public override string Description
        {
            get { return string.Empty; }
        }

        public override object EndCalculation(SummaryResult summaryResult)
        {
            if (_multipleStrings)
                return "Multiple";
            return _previousString;
        }

        public override string Name
        {
            get { return "StringSummaryCalculator"; }
        }
    }
}

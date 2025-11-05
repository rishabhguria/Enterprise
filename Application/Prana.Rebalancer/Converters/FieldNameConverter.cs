using System.Windows.Data;
using System;
using Infragistics.Windows.DataPresenter;

namespace Prana.Rebalancer.Converters
{
    public class FieldNameConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is SummaryResultEntry)
            {
                return (value as SummaryResultEntry).SummaryResult.SourceField.Name.ToString();
            }
            else if (value is SummaryResult)
            {
                return (value as SummaryResult).SourceField.Name.ToString();
            }
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
        #endregion
    }
}
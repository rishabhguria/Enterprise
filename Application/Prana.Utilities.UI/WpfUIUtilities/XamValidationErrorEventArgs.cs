using System.Windows;
using System.Windows.Controls;

namespace Prana.Utilities.UI
{
    public class XamValidationErrorEventArgs : RoutedEventArgs
    {
        public string DataError { get; set; }
        public ValidationErrorEventAction Action { get; set; }
    }
}

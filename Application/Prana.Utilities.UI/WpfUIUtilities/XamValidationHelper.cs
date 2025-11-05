using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Prana.Utilities.UI
{
    public class XamValidationHelper
    {

        #region dataRecorderrors

        #region AddError

        public void AddError(DataRecord dataRecord)
        {
            if (!DataRecordErrors.ContainsKey(dataRecord))
            {
                DataRecordErrors.Add(dataRecord, dataRecord.DataError.ToString());
            }
            else
            {
                DataRecordErrors[dataRecord] = dataRecord.DataError.ToString();
            }

            dataRecord.DataPresenter.RaiseEvent(new XamValidationErrorEventArgs
            {
                DataError = dataRecord.DataError.ToString(),
                Action = ValidationErrorEventAction.Added,
                RoutedEvent = XamDataErrorEvent
            });
        }

        #endregion AddError

        #region RemoveError

        public void RemoveError(DataRecord dataRecord)
        {
            if (DataRecordErrors.ContainsKey(dataRecord))
            {
                var removedError = DataRecordErrors[dataRecord];
                DataRecordErrors.Remove(dataRecord);

                dataRecord.DataPresenter.RaiseEvent(new XamValidationErrorEventArgs
                {
                    DataError = removedError,
                    Action = ValidationErrorEventAction.Removed,
                    RoutedEvent = XamDataErrorEvent
                });
            }
        }

        #endregion RemoveError

        #endregion dataRecorderrors

        private Dictionary<DataRecord, string> DataRecordErrors = new Dictionary<DataRecord, string>();

        #region RoutedEvent

        public static readonly RoutedEvent XamDataErrorEvent = EventManager.RegisterRoutedEvent(
        "XamDataErrorEvent", RoutingStrategy.Bubble, typeof(EventHandler<XamValidationErrorEventArgs>), typeof(XamValidationHelper));

        public static void AddXamDataErrorEventHandler(DependencyObject o, EventHandler<XamValidationErrorEventArgs> handler)
        {
            ((UIElement)o).AddHandler(XamDataErrorEvent, handler);
        }

        public static void RemoveXamDataErrorEventHandler(DependencyObject o, EventHandler<XamValidationErrorEventArgs> handler)
        {
            ((UIElement)o).RemoveHandler(XamDataErrorEvent, handler);
        }

        #endregion RoutedEvent
    }
}
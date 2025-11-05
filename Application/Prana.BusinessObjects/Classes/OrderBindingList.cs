using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class OrderBindingList : BindingList<OrderSingle>
    {
        public void AddRange(OrderBindingList collection)
        {
            // The given collection may not be null.
            if (collection == null)
                throw new ArgumentNullException("OrderBindingList-AddRange");

            // Remember the current setting for RaiseListChangedEvents
            // (if it was already deactivated, we shouldn't activate it after adding!).
            var oldRaiseEventsValue = base.RaiseListChangedEvents;

            // Try adding all of the elements to the binding list.
            try
            {
                base.RaiseListChangedEvents = false;

                foreach (var value in collection)
                    base.Add(value);
            }

            // Restore the old setting for RaiseListChangedEvents (even if there was an exception),
            // and fire the ListChanged-event once (if RaiseListChangedEvents is activated).
            finally
            {
                base.RaiseListChangedEvents = oldRaiseEventsValue;

                if (base.RaiseListChangedEvents)
                    base.ResetBindings();
            }
        }
    }
}

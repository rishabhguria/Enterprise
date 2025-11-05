using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for AllocationOrderCollection.
	/// </summary>
 
	public class AllocationOrderCollection:CollectionBase,IBindingList 
	{
		private ListChangedEventArgs resetEvent = null;
        private Dictionary<string, AllocationOrder> _dictOrders = new Dictionary<string, AllocationOrder>();
        public AllocationOrderCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public AllocationOrder   GetOrder(string clOrderID)
		{
            return _dictOrders[clOrderID];
		}
        public bool ContainsOrder(string clOrderID)
        {
            return _dictOrders.ContainsKey(clOrderID);
        }
        //public void AddOrders(AllocationOrderCollection orders)
        //{
        //    foreach (AllocationOrder order in orders)
        //    {
        //        _dictOrders.Add(order.ClOrderID,order);
        //        List.Add(order);
        //    }
        //}

		public double  SumOfExeQuantity()
		{
            double sumOfExeQty = 0;
			foreach(AllocationOrder    order in List)
			{
				sumOfExeQty= order.CumQty +sumOfExeQty;
			}
			return sumOfExeQty;
		}

		public double  SumOfTotalQuantity()
		{
            double sumOfTotalQty = 0;
			foreach(AllocationOrder    order in List)
			{
				sumOfTotalQty= order.Quantity +sumOfTotalQty;
			}
			return sumOfTotalQty;
		}


		#region " Implements IBindingList"
		
		public bool AllowEdit
		{
			get
			{
				return true;
			}
		}
		
		public bool AllowNew
		{
			get
			{
				return true;
			}
		}
		
		public bool AllowRemove
		{
			get
			{
				return true;
			}
		}
		
		public bool SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}
		
		public bool SupportsSearching
		{
			get
			{
				return false;
			}
		}
		
		public bool SupportsSorting
		{
			get
			{
				return false;
			}
		}
		
		// Events.
		private ListChangedEventHandler ListChangedEvent;
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				ListChangedEvent = (ListChangedEventHandler) System.Delegate.Combine(ListChangedEvent, value);
			}
			remove
			{
				ListChangedEvent = (ListChangedEventHandler) System.Delegate.Remove(ListChangedEvent, value);
			}
		}
		
		
		// Methods.
		public object AddNew()
		{
			AllocationOrder order = new AllocationOrder();
            //SK20071207: Need to check why it is called and automatically.
            // LH defect #120 and 118 were coming because of this so commented for now.

			//List.Add(order);
			return order;
		} //IBindingList.AddNew
		
		// Unsupported properties.
		public bool IsSorted
		{
			get
			{
				throw (new NotSupportedException());
			}
		}
		
		public ListSortDirection SortDirection
		{
			get
			{
				throw (new NotSupportedException());
			}
		}
		
		public PropertyDescriptor SortProperty
		{
			get
			{
				throw (new NotSupportedException());
			}
		}
		
		// Unsupported Methods.
		public void AddIndex (PropertyDescriptor prop)
		{
			throw (new NotSupportedException());
		} //IBindingList.AddIndex
		
		public void ApplySort (PropertyDescriptor prop, ListSortDirection direction)
		{
			throw (new NotSupportedException());
		} //IBindingList.ApplySort
		
		public int Find(PropertyDescriptor prop, object key)
		{
			throw (new NotSupportedException());
		} //IBindingList.Find
		
		public void RemoveIndex (PropertyDescriptor prop)
		{
			throw (new NotSupportedException());
		} //IBindingList.RemoveIndex
		
		public void RemoveSort ()
		{
			throw (new NotSupportedException());
		} //IBindingList.RemoveSort
		
		#endregion

		#region ClientEnumerator Class

		public class ClientEnumerator: IEnumerator
		{
			AllocationOrderCollection  _orders;
			int _location;

			public ClientEnumerator (AllocationOrderCollection orders)
			{
				_orders = orders;
				_location = -1;
			}

			#region IEnumerator Members
			public void Reset()
			{
				_location = -1;	
			}
			public object Current
			{
				get
				{
					if ((_location < 0) || (_location >= _orders.Count))
					{
						throw (new InvalidOperationException());
					}
					else
					{
						return _orders[_location];
					}
				}
			}

			public bool MoveNext()
			{
				_location++;

				if (_location >= _orders.Count)
				{
					return false;
				}
				else
				{
					return true;
				}
			}            
			#endregion
		}

		#endregion

		#region " Public Properties"
		
		public AllocationOrder this[int index]
		{
			get
			{
				return ((AllocationOrder) List[index]);
			}
			set
			{
				List[index] = value;
			}
		}
		
		#endregion
		
		#region " Public Methods"
		
		public int Add(AllocationOrder value)
		{
            try
            {
                if(!_dictOrders.ContainsKey(value.ClOrderID))
                {
                    _dictOrders.Add(value.ClOrderID, value);
                }
                return List.Add(value);
               
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                return int.MinValue;

            }
            
			
		}
        public void  Add(AllocationOrderCollection value)
        {
            foreach (AllocationOrder order in value)
            {
                Add(order);
            }
        }
		
		public AllocationOrder AddNew2()
		{
			return ((AllocationOrder)(((IBindingList) this).AddNew()));
		}
		
		public void Remove (AllocationOrder value)
		{
            _dictOrders.Remove(value.ClOrderID);
				List.Remove(value);
				
		}
        public void Remove(AllocationOrderCollection value)
        {

            foreach (AllocationOrder order in value)
            {
                Remove(order);
            }

        }
		
		protected virtual void OnListChanged (ListChangedEventArgs ev)
		{
			if (ListChangedEvent != null) //&& bAllowEvent)
				ListChangedEvent(this, ev);
		}
		
		protected override void OnClear ()
		{
			try
			{
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		protected override void OnClearComplete ()
		{
			OnListChanged(resetEvent);
		}
		
		protected override void OnInsertComplete (int index, object value)
		{
			AllocationOrder order = ((AllocationOrder) value);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			AllocationOrder order = ((AllocationOrder) value);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
			if (oldValue != newValue)
			{
				AllocationOrder neworder = ((AllocationOrder) oldValue);
				AllocationOrder oldorder = ((AllocationOrder) newValue);
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
		}
		
	
		internal void OrderChanged (AllocationOrder order)
		{
			int index = List.IndexOf(order);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
		}
		#endregion

        public AllocationOrderCollection Clone()
        {
            AllocationOrderCollection orders = new AllocationOrderCollection();
            foreach (AllocationOrder order in List)
            {
                orders.Add(order.Clone());
            }
            return orders;
        }
        public void Clear()
        {
            _dictOrders.Clear();
        }
	}
}






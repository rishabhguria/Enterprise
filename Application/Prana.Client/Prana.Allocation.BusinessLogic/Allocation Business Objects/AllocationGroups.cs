using System;
using System.Collections;
using Prana.BusinessObjects;
using Prana.Utilities.UIUtilities;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for AllocationGroups.
	/// </summary>
 
	public class AllocationGroups:CollectionBase,IBindingList 
	{
		//private ArrayList _groups = new ArrayList();
		//private bool bAllowEvent=true;
		private ListChangedEventArgs resetEvent = null;
        private Dictionary<string, AllocationGroup> _dictGroups = new Dictionary<string, AllocationGroup>();
		//private const string MULTIPLE="MUL";
		
		public AllocationGroups()
		{
			//
			// TODO: Add constructor logic here
			//
		}


        public double SumOfExeQuantity()
		{
            double sumOfExeQty = 0;
			foreach(AllocationGroup  group in List)
			{
				sumOfExeQty= group.CumQty  +sumOfExeQty;
			}
			return sumOfExeQty;
		}

		public double  SumOfTotalQuantity()
		{
			double  sumOfTotalQty=0;
			foreach(AllocationGroup  group in List)
			{
				sumOfTotalQty= group.Quantity +sumOfTotalQty;
			}
			return sumOfTotalQty;
		}

		public void Remove(string groupID)
		{
			foreach(AllocationGroup  group in List)
			{
				if(group.GroupID.Equals(groupID))
				{
					List.Remove((AllocationGroup) group);
					break;
				}
			}
		}

        #region " Implements IBindingList"

        public bool AllowEdit
        {
            get
            {
             
                //return false;
                return true;
            }
        }

        public bool AllowNew
        {
            get
            {
                return false;
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
                ListChangedEvent = (ListChangedEventHandler)System.Delegate.Combine(ListChangedEvent, value);
            }
            remove
            {
                ListChangedEvent = (ListChangedEventHandler)System.Delegate.Remove(ListChangedEvent, value);
            }
        }


        // Methods.
        public object AddNew()
        {
            AllocationGroup group = new AllocationGroup();
            //List.Add(group);
            return group;
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
        public void AddIndex(PropertyDescriptor prop)
        {
            throw (new NotSupportedException());
        } //IBindingList.AddIndex

        public void ApplySort(PropertyDescriptor prop, ListSortDirection direction)
        {
            throw (new NotSupportedException());
        } //IBindingList.ApplySort

        public int Find(PropertyDescriptor prop, object key)
        {
            throw (new NotSupportedException());
        } //IBindingList.Find

        public void RemoveIndex(PropertyDescriptor prop)
        {
            throw (new NotSupportedException());
        } //IBindingList.RemoveIndex

        public void RemoveSort()
        {
            throw (new NotSupportedException());
        } //IBindingList.RemoveSort

        #endregion

        #region ClientEnumerator Class

        public class ClientEnumerator : IEnumerator
        {
            AllocationGroups _groups;
            int _location;

            public ClientEnumerator(AllocationGroups groups)
            {
                _groups = groups;
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
                    if ((_location < 0) || (_location >= _groups.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _groups[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _groups.Count)
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

        public AllocationGroup this[int index]
        {
            get
            {
                return ((AllocationGroup)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        #endregion

        #region " Public Methods"

        public int Add(AllocationGroup value)
        {
            if(_dictGroups.ContainsKey(value.GroupID))
            {
                _dictGroups.Remove(value.GroupID);
            }
            _dictGroups.Add(value.GroupID, value);

            int added = 0;
            FormMarshaller formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.ALLOCATION_FORM);
            if (formMarshaller == null)
            {
                formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.COMMISSION_FORM);
            }
            if (formMarshaller != null && formMarshaller.Form != null)
            {
                if (formMarshaller.InvokeRequired)
                {
                    AddHandler addHandler = new AddHandler(List.Add);
                    added = (int)formMarshaller.Invoke(addHandler, new object[] { value });
                }
                else
                {
                    added = List.Add(value);
                }
            }

            return added; // List.Add(value);
        }

        public AllocationGroup AddNew2()
        {
            return ((AllocationGroup)(((IBindingList)this).AddNew()));
        }

        public void Remove(AllocationGroup value)
        {
            _dictGroups.Remove(value.GroupID);

            FormMarshaller formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.ALLOCATION_FORM);
            if (formMarshaller == null)
            {
                formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.COMMISSION_FORM);
            }
            if (formMarshaller != null && formMarshaller.Form != null)
            {
                if (formMarshaller.InvokeRequired)
                {
                    RemoveHandler removeHandler = new RemoveHandler(List.Remove);
                    formMarshaller.Invoke(removeHandler, new object[] { value });
                }
                else
                {
                    List.Remove(value);
                }
            }

        }


        public void Clear()
        {
            FormMarshaller formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.ALLOCATION_FORM);
            if (formMarshaller == null)
            {
                formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.COMMISSION_FORM);
            }
            if (formMarshaller != null && formMarshaller.Form != null)
            {
                if (formMarshaller.InvokeRequired)
                {
                    MethodInvokerVoid methodInvokerVoid = new MethodInvokerVoid(base.Clear);
                    formMarshaller.Invoke(methodInvokerVoid);
                }
                else
                {
                    base.Clear();
                }
            }

        }

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (ListChangedEvent != null) //&& bAllowEvent)
                ListChangedEvent(this, ev);
        }

        protected override void OnClear()
        {
            try
            {


                foreach (AllocationGroup temp in List)
                {
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnClearComplete()
        {
            resetEvent = new ListChangedEventArgs(ListChangedType.Reset,-1);
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            AllocationGroup group = ((AllocationGroup)value);
          
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            AllocationGroup group = ((AllocationGroup)value);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                AllocationGroup newgroup = ((AllocationGroup)oldValue);
                AllocationGroup oldgroup = ((AllocationGroup)newValue);



                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }


        internal void GroupChanged(AllocationGroup group)
        {
            int index = List.IndexOf(group);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }
        #endregion

        public AllocationGroup GetGroup(string groupID)
        {
           
                if (_dictGroups.ContainsKey(groupID))
                {
                    return _dictGroups[groupID];
                }
                else
                    return null;
        }
        public bool Contains(string groupID)
        {
            return _dictGroups.ContainsKey(groupID);
        }
        public string  ContainsOrder(string clOrderID)
        {
            string groupID = string.Empty;
            foreach (AllocationGroup group in List)
            {
                if (group.ContainsOrder(clOrderID))
                {
                    return group.GroupID;
                }
            }
            return groupID;
        }
        public void ClearCollection()
        {
            _dictGroups.Clear();
        }

        public AllocationGroups Clone()
        {
            AllocationGroups allocationGroups = new AllocationGroups();
            foreach (AllocationGroup allocationGroup in List)
            {
                allocationGroups.Add(allocationGroup.Clone());
            }
            return allocationGroups;
        }


       

        
	}
}

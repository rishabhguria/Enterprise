using System;
using System.Collections;
using Prana.BusinessObjects;
using Prana.Utilities.UIUtilities;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Prana.Allocation.BLL
{
    
  [XmlInclude(typeof(BasketGroup))]
    public class BasketGroupCollection : CollectionBase, IBindingList
    {
        private Dictionary<string, BasketGroup> _dictBasketGroups = new Dictionary<string, BasketGroup>();
        public BasketGroupCollection()
        {
        }
        #region " Class Declarations"

        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset,-1);

        #endregion

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
            BasketGroup basketGroup = new BasketGroup();
            //	List.Add(basketGroup);
            return basketGroup;
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

        #region " Public Properties"

        public BasketGroup this[int index]
        {
            get
            {
                return ((BasketGroup)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }


        #endregion

        #region " Public Methods"

        public int Add(BasketGroup value)
        {
            _dictBasketGroups.Add(value.BasketGroupID, value);

            int added = 0;
            FormMarshaller formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.ALLOCATION_FORM);
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
        public bool Contains(BasketGroup value)
        {
            return List.Contains(value);
        }
        public BasketGroup AddNew2()
        {
            return ((BasketGroup)(((IBindingList)this).AddNew()));
        }

        public void Remove(BasketGroup value)
        {
            try
            {
                BasketGroup group = _dictBasketGroups[value.BasketGroupID];
                _dictBasketGroups.Remove(group.BasketGroupID);
                
                FormMarshaller formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.ALLOCATION_FORM);
                if (formMarshaller != null && formMarshaller.Form != null)
                {
                    if (formMarshaller.InvokeRequired)
                    {
                        RemoveHandler removeHandler = new RemoveHandler(List.Remove);
                        formMarshaller.Invoke(removeHandler, new object[] { group });
                    }
                    else
                    {
                        List.Remove(group);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Clear()
        {
            FormMarshaller formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.ALLOCATION_FORM);
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
            if (ListChangedEvent != null)
                ListChangedEvent(this, ev);
        }

        protected override void OnClear()
        {
            BasketGroup basketGroup;
            foreach (BasketGroup tempLoopVar_c in List)
            {
                basketGroup = tempLoopVar_c;
            }
        }

        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            BasketGroup basketGroup = ((BasketGroup)value);
            //mpid.parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            BasketGroup basketGroup = ((BasketGroup)value);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                BasketGroup oldorder = ((BasketGroup)oldValue);
                BasketGroup neworder = ((BasketGroup)newValue);

                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        // Called by BasketGroup when it changes.
        internal void OrderChanged(BasketGroup basketGroup)
        {
            int index = List.IndexOf(basketGroup);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }
        #endregion

        public BasketGroup GetBasketGroup(string groupID)
        {
            BasketGroup temp = null;
            foreach (BasketGroup basketGroup in List)
            {
                if (basketGroup.BasketGroupID == groupID)
                {
                    temp = basketGroup;
                    break;
                }
            }
            return temp;
        }

    }


}

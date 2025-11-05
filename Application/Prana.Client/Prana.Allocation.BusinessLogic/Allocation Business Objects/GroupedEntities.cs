//using System;
//using System.Collections;
//using System.ComponentModel;

//namespace Prana.Allocation.BLL
//{
//    /// <summary>
//    /// Summary description for Allocations.
//    /// </summary>
//    public class GroupedEntities:CollectionBase,IBindingList 
//    {
//        //private ArrayList _groupedentities = new ArrayList();
//        private ListChangedEventArgs resetEvent = null;
//        public GroupedEntities()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

//        public GroupedEntity  GetGroupEntity(string GroupedEntityID)
//        {
			
//            foreach(GroupedEntity groupedentitity in List )
//            {
//                if(groupedentitity.GroupedEntityID == GroupedEntityID)
//                    return groupedentitity;
			
//            }
//            return null;



			
//        }

//        public bool ContainsOrder(string clOrderID)
//        {
//            bool isFound = false;
//            foreach (GroupedEntity groupEntity in this)
//            {
//                if (groupEntity.ClOrderID == clOrderID)
//                {
//                    isFound = true;
//                    break;
//                }
//            }
//            return isFound;
//        }
	
//        #region " Public Properties"
		
//        public GroupedEntity  this[int index]
//        {
//            get
//            {
//                return ((GroupedEntity) List[index]);
//            }
//            set
//            {
//                List[index] = value;
//            }
//        }
		
//        #endregion
		
//        #region " Public Methods"
		
//        public int Add(GroupedEntity value)
//        {
//            return List.Add(value);
			
//        }
		
//        public GroupedEntity AddNew2()
//        {
//            return ((GroupedEntity)(((IBindingList) this).AddNew()));
//        }
		
//        public void Remove (GroupedEntity value)
//        {
//            List.Remove(value);
//        }
		
//        protected virtual void OnListChanged (ListChangedEventArgs ev)
//        {
//            if (ListChangedEvent != null)// && bAllowEvent)
//                ListChangedEvent(this, ev);
//        }
		
//        protected override void OnClear ()
//        {
//            try
//            {
				
				
//                //				foreach (GroupedEntity temp in List)
//                //				{
//                //					temp.Parent=null;
//                //				}
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
//        }
		
//        protected override void OnClearComplete ()
//        {
//            OnListChanged(resetEvent);
//        }
		
//        protected override void OnInsertComplete (int index, object value)
//        {
//            GroupedEntity groupedEntity = ((GroupedEntity) value);
//            //groupedEntity.Parent = this;
//            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
//        }
		
//        protected override void OnRemoveComplete (int index, object value)
//        {
//            GroupedEntity groupedEntity = ((GroupedEntity) value);
//            //groupedEntity.Parent = this;
//            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
//        }
		
//        protected override void OnSetComplete (int index, object oldValue, object newValue)
//        {
//            if (oldValue != newValue)
//            {
				
//                GroupedEntity neworder = ((GroupedEntity) oldValue);
//                GroupedEntity oldorder = ((GroupedEntity) newValue);
				
//                //neworder.Parent = null;
//                //oldorder.Parent = this;
				
//                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
//            }
//        }
		
	
		
//        #endregion

//        #region " Implements IBindingList"
		
//        public bool AllowEdit
//        {
//            get
//            {
//                return true;
//            }
//        }
		
//        public bool AllowNew
//        {
//            get
//            {
//                return false;
//            }
//        }
		
//        public bool AllowRemove
//        {
//            get
//            {
//                return true;
//            }
//        }
		
//        public bool SupportsChangeNotification
//        {
//            get
//            {
//                return true;
//            }
//        }
		
//        public bool SupportsSearching
//        {
//            get
//            {
//                return false;
//            }
//        }
		
//        public bool SupportsSorting
//        {
//            get
//            {
//                return false;
//            }
//        }
		
//        // Events.
//        private ListChangedEventHandler ListChangedEvent;
//        public event ListChangedEventHandler ListChanged
//        {
//            add
//            {
//                ListChangedEvent = (ListChangedEventHandler) System.Delegate.Combine(ListChangedEvent, value);
//            }
//            remove
//            {
//                ListChangedEvent = (ListChangedEventHandler) System.Delegate.Remove(ListChangedEvent, value);
//            }
//        }
		
		
//        // Methods.
//        public object AddNew()
//        {
//            GroupedEntity groupedEntity = new GroupedEntity();
//           // List.Add(groupedEntity);
//            return groupedEntity;
//        }
		
//        // Unsupported properties.
//        public bool IsSorted
//        {
//            get
//            {
//                throw (new NotSupportedException());
//            }
//        }
		
//        public ListSortDirection SortDirection
//        {
//            get
//            {
//                throw (new NotSupportedException());
//            }
//        }
		
//        public PropertyDescriptor SortProperty
//        {
//            get
//            {
//                throw (new NotSupportedException());
//            }
//        }
		
//        // Unsupported Methods.
//        public void AddIndex (PropertyDescriptor prop)
//        {
//            throw (new NotSupportedException());
//        } //IBindingList.AddIndex
		
//        public void ApplySort (PropertyDescriptor prop, ListSortDirection direction)
//        {
//            throw (new NotSupportedException());
//        } //IBindingList.ApplySort
		
//        public int Find(PropertyDescriptor prop, object key)
//        {
//            throw (new NotSupportedException());
//        } //IBindingList.Find
		
//        public void RemoveIndex (PropertyDescriptor prop)
//        {
//            throw (new NotSupportedException());
//        } //IBindingList.RemoveIndex
		
//        public void RemoveSort ()
//        {
//            throw (new NotSupportedException());
//        } //IBindingList.RemoveSort
		
//        #endregion
//    }
//}

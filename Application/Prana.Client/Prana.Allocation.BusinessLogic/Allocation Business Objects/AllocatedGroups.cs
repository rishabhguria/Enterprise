using System;
using System.Collections;
using System.ComponentModel;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for AllocatedGroups.
	/// </summary>
	public class AllocatedGroups:CollectionBase,IBindingList
	{
		private ListChangedEventArgs resetEvent = null;

		public AllocatedGroups()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region " Public Methods"
		
		public int Add(AllocatedGroup value)
		{
			return List.Add(value);
			
		}
		
		public AllocatedGroup AddNew2()
		{
			return ((AllocatedGroup)(((IBindingList) this).AddNew()));
		}

        public void Remove(AllocatedGroup value)
		{
			List.Remove(value);
		}
		
		protected virtual void OnListChanged (ListChangedEventArgs ev)
		{
			if (ListChangedEvent != null)// && bAllowEvent)
				ListChangedEvent(this, ev);
		}
		
		protected override void OnClear ()
		{
			try
			{
				
				
				//				foreach (GroupedEntity temp in List)
				//				{
				//					temp.Parent=null;
				//				}
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
			AllocatedGroup allocatedGroup = ((AllocatedGroup) value);
			//groupedEntity.Parent = this;
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			AllocatedGroup allocatedGroup = ((AllocatedGroup) value);
			//groupedEntity.Parent = this;
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
			if (oldValue != newValue)
			{
				
				AllocatedGroup newallocatedGroup = ((AllocatedGroup) oldValue);
				AllocatedGroup oldallocatedGroup = ((AllocatedGroup) newValue);
				
				//neworder.Parent = null;
				//oldorder.Parent = this;
				
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
		}
		
	
		internal void OrderChanged (AllocatedGroup allocatedGroup)
		{
			int index = List.IndexOf(allocatedGroup);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
		}
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
			AllocatedGroup  allocatedGroup = new AllocatedGroup();
			List.Add(allocatedGroup);
			return allocatedGroup;
		}
		
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

	}
}

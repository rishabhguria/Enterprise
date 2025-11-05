

#region IBindingList version

using System;
using System.ComponentModel;
using System.Collections;


namespace Nirvana.PNL
{
	/// <summary>
	/// This class contains the pnl data collection. We have created this
	/// class in order to directly bind it from the Infragistics DataGrid by implementing 
	/// the interface IBindingList. Deriving from the CollectionBase class makes it strongly typed collection
	/// 
	/// </summary>

	public class PNLLevel2DataCollection: CollectionBase,IBindingList
	{
		private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
		private ListChangedEventHandler onListChanged;

		public PNLLevel2Data this[int index] 
		{
			get 
			{
				return (PNLLevel2Data)(List[index]);
			}
			set 
			{
				List[index] = value;
			}
		}

		public int Add (PNLLevel2Data value) 
		{
			return List.Add(value);
		}

		public PNLLevel2Data AddNew() 
		{
			return (PNLLevel2Data)((IBindingList)this).AddNew();
		}

		public void Remove (PNLLevel2Data value) 
		{
			List.Remove(value);
		}

    
		protected virtual void OnListChanged(ListChangedEventArgs ev) 
		{
			if (onListChanged != null) 
			{
				onListChanged(this, ev);
			}
		}
    

		protected override void OnClear() 
		{
			//			foreach (PNLLevel2Data c in List) 
			//			{
			//				c.Parent = null;
			//			}
		}

		protected override void OnClearComplete() 
		{
			OnListChanged(resetEvent);
		}

		protected override void OnInsertComplete(int index, object value) 
		{
				PNLLevel2Data c = (PNLLevel2Data)value;
				//c.Parent = this;
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
		}

		protected override void OnRemoveComplete(int index, object value) 
		{
				PNLLevel2Data c = (PNLLevel2Data)value;
				//c.Parent = this;
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}

		protected override void OnSetComplete(int index, object oldValue, object newValue) 
		{
				if (oldValue != newValue) 
				{
	
					PNLLevel2Data oldcust = (PNLLevel2Data)oldValue;
					PNLLevel2Data newcust = (PNLLevel2Data)newValue;
			    
					//oldcust.Parent = null;
					//newcust.Parent = this;
			    
			    
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
				}
		}
    
		// Called by PNLLevel2Data when it changes.
		internal void PNLLevel2DataChanged(PNLLevel2Data cust) 
		{
        
			int index = List.IndexOf(cust);
        
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
		}
    

		// Implements IBindingList.
		bool IBindingList.AllowEdit 
		{ 
			get { return true ; }
		}

		bool IBindingList.AllowNew 
		{ 
			get { return true ; }
		}

		bool IBindingList.AllowRemove 
		{ 
			get { return true ; }
		}

		bool IBindingList.SupportsChangeNotification 
		{ 
			get { return true ; }
		}
    
		bool IBindingList.SupportsSearching 
		{ 
			get { return false ; }
		}

		bool IBindingList.SupportsSorting 
		{ 
			get { return false ; }
		}


		// Events.
		public event ListChangedEventHandler ListChanged 
		{
			add 
			{
				onListChanged += value;
			}
			remove 
			{
				onListChanged -= value;
			}
		}

		// Methods.
		object IBindingList.AddNew() 
		{
			PNLLevel2Data c = new PNLLevel2Data();
			List.Add(c);
			return c;
		}


		// Unsupported properties.
		bool IBindingList.IsSorted 
		{ 
			get { throw new NotSupportedException(); }
		}

		ListSortDirection IBindingList.SortDirection 
		{ 
			get { throw new NotSupportedException(); }
		}


		PropertyDescriptor IBindingList.SortProperty 
		{ 
			get { throw new NotSupportedException(); }
		}


		// Unsupported Methods.
		void IBindingList.AddIndex(PropertyDescriptor property) 
		{
			throw new NotSupportedException(); 
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction) 
		{
			throw new NotSupportedException(); 
		}

		int IBindingList.Find(PropertyDescriptor property, object key) 
		{
			throw new NotSupportedException(); 
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property) 
		{
			throw new NotSupportedException(); 
		}

		void IBindingList.RemoveSort() 
		{
			throw new NotSupportedException(); 
		}

	
	}
}

#endregion

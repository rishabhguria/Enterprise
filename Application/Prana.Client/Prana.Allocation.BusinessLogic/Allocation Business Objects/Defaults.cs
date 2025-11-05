using System;
using System.Collections;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for Defaults.
	/// </summary>
	public class Defaults:IList
	{
		private ArrayList _defaults= new ArrayList();
		public Defaults()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public  Default  GetDefault(string defaultID)
		{
			Default defaultEdit=null;
			foreach(Default default1 in _defaults)
			{
				if(default1.DefaultID.ToString().Equals(defaultID))
				{
						defaultEdit=default1;
					break;
				}
				
				
			
			}
			return defaultEdit;
		
		}

		#region IList Members

		public bool IsReadOnly
		{
			get
			{	
				return _defaults.IsReadOnly;
				//return false;
			}
		}

		public object this[int index]
		{
			get
			{
				//Add Clients.this getter implementation
				return _defaults[index];
			}
			set
			{
				//Add Clients.this setter implementation
				_defaults[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			//Add Clients.RemoveAt implementation
			_defaults.RemoveAt(index);
		}

		public void Insert(int index, Object defaultObject)
		{
			//Add Clients.Insert implementation
			_defaults.Insert(index, (Default)defaultObject);
		}

		public void Remove(Object defaultObject)
		{
			//Add Clients.Remove implementation
			_defaults.Remove((Default) defaultObject);
		}

		public bool Contains(object defaultObject)
		{
			//Add Clients.Contains implementation
			return _defaults.Contains((Default)defaultObject);
		}

		public void Clear()
		{
			//Add Clients.Clear implementation
			_defaults.Clear();
		}

		public int IndexOf(object defaultObject)
		{
			//Add Clients.IndexOf implementation
			return _defaults.IndexOf((Default) defaultObject);
		}

		public int Add(object defaultObject)
		{
			//Add Clients.Add implementation
			return _defaults.Add((Default)defaultObject);
		}

		public bool IsFixedSize
		{
			get
			{
				//Add Clients.IsFixedSize getter implementation
				return _defaults.IsFixedSize;
			}
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				// TODO:  Add Clients.IsSynchronized getter implementation
				return false;
			}
		}

		public int Count
		{
			get
			{
				return _defaults.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_defaults.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _defaults.SyncRoot;
				//return null;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return (new ClientEnumerator(this));
		}

		#endregion

		#region ClientEnumerator Class

		public class ClientEnumerator: IEnumerator
		{
			Defaults  _defaults;
			int _location;

			public ClientEnumerator (Defaults funds)
			{
				_defaults = funds;
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
					if ((_location < 0) || (_location >= _defaults.Count))
					{
						throw (new InvalidOperationException());
					}
					else
					{
						return _defaults[_location];
					}
				}
			}

			public bool MoveNext()
			{
				_location++;

				if (_location >= _defaults.Count)
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
	}
}

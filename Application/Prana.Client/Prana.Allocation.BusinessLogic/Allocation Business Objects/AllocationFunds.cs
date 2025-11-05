using System;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;


namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for Allocations.
	/// </summary>
    [XmlInclude(typeof(AllocationFund))]
    public class AllocationFunds:IList
	{
		private ArrayList _funds = new ArrayList();

		
		public AllocationFunds()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public AllocationFund GetFund(int fundID)
		{
			AllocationFund temp  = new AllocationFund();
			foreach(AllocationFund fund in _funds)
			{
				if(fund.FundID==fundID)
				{
					temp=fund;
					break;
				}

			
			}
			return temp;
		}


		
		#region IList Members

		public bool IsReadOnly
		{
			get
			{	
				return _funds.IsReadOnly;
				//return false;
			}
		}

		public object this[int index]
		{
			get
			{
				//Add Clients.this getter implementation
				return _funds[index];
			}
			set
			{
				//Add Clients.this setter implementation
				_funds[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			//Add Clients.RemoveAt implementation
			_funds.RemoveAt(index);
		}

		public void Insert(int index, Object fund)
		{
			//Add Clients.Insert implementation
			_funds.Insert(index, (AllocationFund)fund);
		}

		public void Remove(Object fund)
		{
			//Add Clients.Remove implementation
			_funds.Remove((AllocationFund) fund);
		}

		public bool Contains(object fund)
		{
			//Add Clients.Contains implementation
			return _funds.Contains((AllocationFund)fund);
		}

		public void Clear()
		{
			//Add Clients.Clear implementation
			_funds.Clear();
		}

		public int IndexOf(object fund)
		{
			//Add Clients.IndexOf implementation
			return _funds.IndexOf((AllocationFund) fund);
		}

		public int Add(object fund)
		{
			//Add Clients.Add implementation
			return _funds.Add((AllocationFund)fund);
		}

		public bool IsFixedSize
		{
			get
			{
				//Add Clients.IsFixedSize getter implementation
				return _funds.IsFixedSize;
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
				return _funds.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_funds.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _funds.SyncRoot;
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
			AllocationFunds  _funds;
			int _location;

			public ClientEnumerator (AllocationFunds funds)
			{
				_funds = funds;
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
					if ((_location < 0) || (_location >= _funds.Count))
					{
						throw (new InvalidOperationException());
					}
					else
					{
						return _funds[_location];
					}
				}
			}

			public bool MoveNext()
			{
				_location++;

				if (_location >= _funds.Count)
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

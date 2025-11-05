using System;
using System.Collections;


namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for Allocations.
	/// </summary>
    public class AllocationStrategies : IList
	{
		private ArrayList _strategies = new ArrayList();

		
		public AllocationStrategies()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public AllocationStrategy GetStrategy(int strategyID)
		{
			AllocationStrategy temp  = new AllocationStrategy();
			foreach(AllocationStrategy companyStrategy in _strategies)
			{
				if(companyStrategy.StrategyID==strategyID)
				{
					temp=companyStrategy;
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
				return _strategies.IsReadOnly;
				//return false;
			}
		}

		public object this[int index]
		{
			get
			{
				//Add Clients.this getter implementation
				return _strategies[index];
			}
			set
			{
				//Add Clients.this setter implementation
				_strategies[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			//Add Clients.RemoveAt implementation
			_strategies.RemoveAt(index);
		}

		public void Insert(int index, Object companyStrategy)
		{
			//Add Clients.Insert implementation
			_strategies.Insert(index, (AllocationStrategy)companyStrategy);
		}

		public void Remove(Object companyStrategy)
		{
			//Add Clients.Remove implementation
			_strategies.Remove((AllocationStrategy) companyStrategy);
		}

		public bool Contains(object companyStrategy)
		{
			//Add Clients.Contains implementation
			return _strategies.Contains((AllocationStrategy)companyStrategy);
		}

		public void Clear()
		{
			//Add Clients.Clear implementation
			_strategies.Clear();
		}

		public int IndexOf(object companyStrategy)
		{
			//Add Clients.IndexOf implementation
			return _strategies.IndexOf((AllocationStrategy) companyStrategy);
		}

		public int Add(object companyStrategy)
		{
			//Add Clients.Add implementation
			return _strategies.Add((AllocationStrategy)companyStrategy);
		}

		public bool IsFixedSize
		{
			get
			{
				//Add Clients.IsFixedSize getter implementation
				return _strategies.IsFixedSize;
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
				return _strategies.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_strategies.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _strategies.SyncRoot;
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
			AllocationStrategies   _strategies;
			int _location;

			public ClientEnumerator (AllocationStrategies companyStrategies)
			{
				_strategies = companyStrategies;
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
					if ((_location < 0) || (_location >= _strategies.Count))
					{
						throw (new InvalidOperationException());
					}
					else
					{
						return _strategies[_location];
					}
				}
			}

			public bool MoveNext()
			{
				_location++;

				if (_location >= _strategies.Count)
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

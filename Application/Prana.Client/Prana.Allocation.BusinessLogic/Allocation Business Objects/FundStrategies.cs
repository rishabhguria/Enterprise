using System;
using System.Collections;
namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for FundStrategies.
	/// </summary>
	public class FundStrategies:IList 
	{
		private ArrayList _fundStrategies = new ArrayList();

		
		public FundStrategies()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public  FundStrategy  GetFundStrategy(int fundID,int strategyID)
		{
			FundStrategy temp=null;
			foreach(FundStrategy  fundStrategy in _fundStrategies )
			{
				if(fundStrategy.FundID==fundID && fundStrategy.StrategyID==strategyID  )
					temp=fundStrategy;
					
			}
			return temp;



		}

		
		#region IList Members

		public bool IsReadOnly
		{
			get
			{	
				return _fundStrategies.IsReadOnly;
				//return false;
			}
		}

		public object this[int index]
		{
			get
			{
				//Add Clients.this getter implementation
				return _fundStrategies[index];
			}
			set
			{
				//Add Clients.this setter implementation
				_fundStrategies[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			//Add Clients.RemoveAt implementation
			_fundStrategies.RemoveAt(index);
		}

		public void Insert(int index, Object fundStrategy)
		{
			//Add Clients.Insert implementation
			_fundStrategies.Insert(index, (FundStrategy)fundStrategy);
		}

		public void Remove(Object fundStrategies)
		{
			//Add Clients.Remove implementation
			_fundStrategies.Remove((FundStrategy) fundStrategies);
		}

		public bool Contains(object fundStrategies)
		{
			//Add Clients.Contains implementation
			return _fundStrategies.Contains((FundStrategy)fundStrategies);
		}

		public void Clear()
		{
			//Add Clients.Clear implementation
			_fundStrategies.Clear();
		}

		public int IndexOf(object fundStrategies)
		{
			//Add Clients.IndexOf implementation
			return _fundStrategies.IndexOf((FundStrategy) fundStrategies);
		}

		public int Add(object fundStrategies)
		{
			//Add Clients.Add implementation
			return _fundStrategies.Add((FundStrategy)fundStrategies);
		}

		public bool IsFixedSize
		{
			get
			{
				//Add Clients.IsFixedSize getter implementation
				return _fundStrategies.IsFixedSize;
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
				return _fundStrategies.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_fundStrategies.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _fundStrategies.SyncRoot;
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
			FundStrategies  _fundStrategies;
			int _location;

			public ClientEnumerator (FundStrategies funds)
			{
				_fundStrategies = funds;
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
					if ((_location < 0) || (_location >= _fundStrategies.Count))
					{
						throw (new InvalidOperationException());
					}
					else
					{
						return _fundStrategies[_location];
					}
				}
			}

			public bool MoveNext()
			{
				_location++;

				if (_location >= _fundStrategies.Count)
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

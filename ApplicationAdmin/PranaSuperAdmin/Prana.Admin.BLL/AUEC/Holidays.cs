using System;
using System.Collections;

namespace Prana.Admin.BLL
{
	/// <summary>
	/// Summary description for Holidays.
	/// </summary>
    /// 

     public class Holidays: IList
	{
        ArrayList _selectedHOLIDAYS = new ArrayList();
		ArrayList _holidays = new ArrayList();

		public Holidays()
		{
		}
		#region IList Members

		public bool IsReadOnly
		{
			get
			{	
				return _holidays.IsReadOnly;
				//return false;
			}
		}

		public object this[int index]
		{
			get
			{
				//Add Holidays.this getter implementation
				return _holidays[index];
			}
			set
			{
				//Add Holidays.this setter implementation
				_holidays[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			//Add Holidays.RemoveAt implementation
			_holidays.RemoveAt(index);
		}

		public void Insert(int index, Object holiday)
		{
			//Add Holidays.Insert implementation
			_holidays.Insert(index, (Holiday)holiday);
		}

         public void Sort()
         {
             _holidays.Sort(sortHolidayAscending());
         }

		public void Remove(Object holiday)
		{
			//Add Holidays.Remove implementation
			_holidays.Remove((Holiday) holiday);
		}

		public bool Contains(object holiday)
		{
			//Add Holidays.Contains implementation
			return _holidays.Contains((Holiday)holiday);
		}

		public void Clear()
		{
			//Add Holidays.Clear implementation
			_holidays.Clear();
		}

		public int IndexOf(object holiday)
		{
			//Add Holidays.IndexOf implementation
			return _holidays.IndexOf((Holiday)holiday);
		}

		public int Add(object holiday)
		{
			//Add Holidays.Add implementation
			return _holidays.Add((Holiday)holiday);
		}

		public bool IsFixedSize
		{
			get
			{
				//Add Holidays.IsFixedSize getter implementation
				return _holidays.IsFixedSize;
			}
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				// TODO:  Add Holidays.IsSynchronized getter implementation
				return false;
			}
		}

		public int Count
		{
			get
			{
				return _holidays.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_holidays.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _holidays.SyncRoot;
				//return null;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return (new HolidayEnumerator(this));
		}

		#endregion

		#region HolidayEnumerator Class

		public class HolidayEnumerator : IEnumerator
		{
			Holidays  _holidays;
			int _location;

			public HolidayEnumerator (Holidays holidays)
			{
				_holidays = holidays;
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
					if ((_location < 0) || (_location >= _holidays.Count))
					{
						throw (new InvalidOperationException());
					}
					else
					{
						return _holidays[_location];
					}
				}
			}

			public bool MoveNext()
			{
				_location++;

				if (_location >= _holidays.Count)
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

         private class HolidaySorter : IComparer
         {
             int IComparer.Compare(object a, object b)
             {
                 Holiday h1 = (Holiday)a;
                 Holiday h2 = (Holiday)b;
                 if (h1.Date > h2.Date)
                     return 1;
                 if (h1.Date < h2.Date)
                     return -1;
                 else
                     return 0;
             }
         }

         public static IComparer sortHolidayAscending()
         {
             return (IComparer)new HolidaySorter();
         }
	
	}
}

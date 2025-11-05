using Csla;

namespace Prana.PM.BLL
{
    public class StepAnalysisList : BusinessListBase<StepAnalysisList, StepAnalysis>
    {




        /*ArrayList _stepAnalysisColl = new ArrayList();

        public StepAnalysisList()
		{
		}
		#region IList Members

		public bool IsReadOnly
		{
			get
			{	
				return _stepAnalysisColl.IsReadOnly;
				//return false;
			}
		}

		public object this[int index]
		{
			get
			{
				//Add StepAnalysisColl.this getter implementation
				return _stepAnalysisColl[index];
			}
			set
			{
				//Add StepAnalysisColl.this setter implementation
				_stepAnalysisColl[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			//Add StepAnalysisColl.RemoveAt implementation
			_stepAnalysisColl.RemoveAt(index);
		}

		public void Insert(int index, Object stepAnalysis)
		{
			//Add StepAnalysisColl.Insert implementation
			_stepAnalysisColl.Insert(index, (StepAnalysis)stepAnalysis);
		}

		public void Remove(Object stepAnalysis)
		{
			//Add StepAnalysisColl.Remove implementation
            _stepAnalysisColl.Remove((StepAnalysis)stepAnalysis);
		}

		public bool Contains(object stepAnalysis)
		{
			//Add StepAnalysisColl.Contains implementation
            return _stepAnalysisColl.Contains((StepAnalysis)stepAnalysis);
		}

		public void Clear()
		{
			//Add StepAnalysisColl.Clear implementation
			_stepAnalysisColl.Clear();
		}

		public int IndexOf(object stepAnalysis)
		{
			//Add StepAnalysisColl.IndexOf implementation
            return _stepAnalysisColl.IndexOf((StepAnalysis)stepAnalysis);
		}

		public int Add(object stepAnalysis)
		{
			//Add StepAnalysisColl.Add implementation
            return _stepAnalysisColl.Add((StepAnalysis)stepAnalysis);
		}

		public bool IsFixedSize
		{
			get
			{
				//Add StepAnalysisColl.IsFixedSize getter implementation
				return _stepAnalysisColl.IsFixedSize;
			}
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				// TODO:  Add StepAnalysisColl.IsSynchronized getter implementation
				return false;
			}
		}

		public int Count
		{
			get
			{
				return _stepAnalysisColl.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			_stepAnalysisColl.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return _stepAnalysisColl.SyncRoot;
				//return null;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return (new StepAnalysisEnumerator(this));
		}

		#endregion

        #region StepAnalysisEnumerator Class

        public class StepAnalysisEnumerator: IEnumerator
		{
			StepAnalysisList  _stepAnalysisColl;
			int _location;

			public StepAnalysisEnumerator (StepAnalysisList StepAnalysisColl)
			{
				_stepAnalysisColl = StepAnalysisColl;
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
					if ((_location < 0) || (_location >= _stepAnalysisColl.Count))
					{
						throw (new InvalidOperationException());
					}
					else
					{
						return _stepAnalysisColl[_location];
					}
				}
			}

			public bool MoveNext()
			{
				_location++;

				if (_location >= _stepAnalysisColl.Count)
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
        */
    }
}

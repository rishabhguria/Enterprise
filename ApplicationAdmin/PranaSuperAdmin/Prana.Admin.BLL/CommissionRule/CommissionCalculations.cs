using System;
using System.Collections;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionCalculations.
    /// </summary>
    public class CommissionCalculations : IList
    {
        ArrayList _commissioncalculations = new ArrayList();
        public CommissionCalculations()
        {
        }

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return _commissioncalculations.IsReadOnly;
                //return false;
            }
        }

        public object this[int index]
        {
            get
            {
                //Add CommissionCalculations.this getter implementation
                return _commissioncalculations[index];
            }
            set
            {
                //Add CommissionCalculations.this setter implementation
                _commissioncalculations[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            //Add CommissionCalculations.RemoveAt implementation
            _commissioncalculations.RemoveAt(index);
        }

        public void Insert(int index, Object CommissionCalculation)
        {
            //Add CommissionCalculations.Insert implementation
            _commissioncalculations.Insert(index, (CommissionCalculation)CommissionCalculation);
        }

        public void Remove(Object CommissionCalculation)
        {
            //Add CommissionCalculations.Remove implementation
            _commissioncalculations.Remove((CommissionCalculation)CommissionCalculation);
        }

        public bool Contains(object CommissionCalculation)
        {
            //Add CommissionCalculations.Contains implementation
            return _commissioncalculations.Contains((CommissionCalculation)CommissionCalculation);
        }

        public void Clear()
        {
            //Add CommissionCalculations.Clear implementation
            _commissioncalculations.Clear();
        }

        public int IndexOf(object CommissionCalculation)
        {
            //Add CommissionCalculations.IndexOf implementation
            return _commissioncalculations.IndexOf((CommissionCalculation)CommissionCalculation);
        }

        public int Add(object CommissionCalculation)
        {
            //Add CommissionCalculations.Add implementation
            return _commissioncalculations.Add((CommissionCalculation)CommissionCalculation);
        }

        public bool IsFixedSize
        {
            get
            {
                //Add CommissionCalculations.IsFixedSize getter implementation
                return _commissioncalculations.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add CommissionCalculations.IsSynchronized getter implementation
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _commissioncalculations.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _commissioncalculations.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _commissioncalculations.SyncRoot;
                //return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new CommissionCalculationEnumerator(this));
        }

        #endregion

        #region CommissionCalculationEnumerator Class

        public class CommissionCalculationEnumerator : IEnumerator
        {
            CommissionCalculations _commissioncalculations;
            int _location;

            public CommissionCalculationEnumerator(CommissionCalculations commissioncalculations)
            {
                _commissioncalculations = commissioncalculations;
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
                    if ((_location < 0) || (_location >= _commissioncalculations.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _commissioncalculations[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _commissioncalculations.Count)
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

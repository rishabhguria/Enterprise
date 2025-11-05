using System;
using System.Collections;
using System.Collections.Generic;


namespace Prana.BusinessObjects
{

    public class Waves : IList<Wave>
    {

        List<Wave> _waveCollection = new List<Wave>();

        #region IList<Wave> Members

        public int IndexOf(Wave wave)
        {
            return _waveCollection.IndexOf(wave);
        }


        public void Insert(int index, Wave wave)
        {

            _waveCollection.Insert(index, wave);
        }


        public void RemoveAt(int index)
        {
            _waveCollection.RemoveAt(index);
        }


        public Wave this[int index]
        {
            get
            {
                return _waveCollection[index];
            }
            set
            {
                _waveCollection[index] = value;
            }
        }


        #endregion

        #region ICollection<Wave> Members

        public void Add(Wave item)
        {
            _waveCollection.Add(item);

        }

        public void Clear()
        {
            _waveCollection.Clear();

        }

        public bool Contains(Wave item)
        {
            return _waveCollection.Remove(item);

        }

        public void CopyTo(Wave[] array, int arrayIndex)
        {
            _waveCollection.CopyTo(array, arrayIndex);

        }

        public int Count
        {
            get
            {
                return _waveCollection.Count;
            }

        }
        public int NumberOfOrders
        {
            get
            {
                int numOfOrders = 0;
                foreach (Wave wave in this._waveCollection)
                {
                    numOfOrders = wave.WaveOrders.Count + numOfOrders;
                }
                return numOfOrders;
            }

        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }


        }

        public bool Remove(Wave item)
        {
            return _waveCollection.Remove(item);

        }

        #endregion

        #region IEnumerable<Wave> Members

        public IEnumerator<Wave> GetEnumerator()
        {
            return (new WaveEnumerator1(this));

        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (new WaveEnumerator(this));

        }

        #endregion
        public Wave GetWaveByWaveID(string waveID)
        {
            Wave wave = null;
            foreach (Wave temp in _waveCollection)
            {
                if (temp.WaveID.Equals(waveID))
                {
                    wave = temp;
                    break;
                }
            }
            return wave;
        }

        public class WaveEnumerator : IEnumerator
        {
            Waves _waves;
            int _location;

            public WaveEnumerator(Waves itemValuesCollection)
            {
                _waves = itemValuesCollection;
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
                    if ((_location < 0) || (_location >= _waves.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _waves[_location];
                    }
                }
            }

            public bool MoveNext()
            {
                _location++;

                if (_location >= _waves.Count)
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
        public sealed class WaveEnumerator1 : IEnumerator<Wave>
        {
            Waves _waves;
            int _location;
            public WaveEnumerator1(Waves itemValuesCollection)
            {
                _waves = itemValuesCollection;
                _location = -1;
            }
            public void Reset()
            {
                _location = -1;
            }
            public bool MoveNext()
            {
                _location++;

                if (_location >= _waves.Count)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            public void Dispose()
            {

            }
            object IEnumerator.Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _waves.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _waves[_location];
                    }
                }
            }
            public Wave Current
            {
                get
                {
                    if ((_location < 0) || (_location >= _waves.Count))
                    {
                        throw (new InvalidOperationException());
                    }
                    else
                    {
                        return _waves[_location];
                    }
                }
            }


        }




    }
}

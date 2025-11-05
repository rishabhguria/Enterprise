using System;

namespace Prana.BusinessObjects
{
    [Serializable()]
    public class WeeklyHolidaysCollection //:IList<WeeklyHoliday>
    {

        //List<WeeklyHoliday> _weeklyHoidayCollection = new List<WeeklyHoliday>();
        //#region IList<WeeklyHoliday> Members

        //public int IndexOf(WeeklyHoliday weeklyHoiday)
        //{
        //    return _weeklyHoidayCollection.IndexOf(weeklyHoiday);
        //}

        //public void Insert(int index, WeeklyHoliday weeklyHoiday)
        //{
        //    _weeklyHoidayCollection.Insert(index, weeklyHoiday);
        //}

        //public void RemoveAt(int index)
        //{
        //    _weeklyHoidayCollection.RemoveAt(index);
        //}

        //public WeeklyHoliday this[int index]
        //{
        //    get
        //    {
        //        return _weeklyHoidayCollection[index];
        //    }
        //    set
        //    {
        //        _weeklyHoidayCollection[index] = value;
        //    }
        //}

        //#endregion

        //#region ICollection<WeeklyHoliday> Members

        //public void Add(WeeklyHoliday weeklyHoiday)
        //{
        //    _weeklyHoidayCollection.Add(weeklyHoiday);
        //}

        //public void Clear()
        //{
        //    _weeklyHoidayCollection.Clear();
        //}

        //public bool Contains(WeeklyHoliday weeklyHoiday)
        //{
        //    return _weeklyHoidayCollection.Remove(weeklyHoiday);
        //}

        //public void CopyTo(WeeklyHoliday[] array, int arrayIndex)
        //{
        //    _weeklyHoidayCollection.CopyTo(array, arrayIndex);
        //}

        //public int Count
        //{
        //    get
        //    {
        //        return _weeklyHoidayCollection.Count;
        //    }
        //}

        //public bool IsReadOnly
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}

        //public bool Remove(WeeklyHoliday weeklyHoiday)
        //{
        //    return _weeklyHoidayCollection.Remove(weeklyHoiday);

        //}

        //#endregion

        //#region IEnumerable<WeeklyHoliday> Members

        //public IEnumerator<WeeklyHoliday> GetEnumerator()
        //{
        //    return (new WeeklyHoidayEnumerator1(this));

        //}

        //#endregion

        //#region IEnumerable Members

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return (new WeeklyHoidayEnumerator(this));
        //}

        //#endregion

        //public class WeeklyHoidayEnumerator : IEnumerator
        //{
        //    WeeklyHolidaysCollection _WeeklyHolidaysCollection;
        //    int _location;

        //    public WeeklyHoidayEnumerator(WeeklyHolidaysCollection itemValuesCollection)
        //    {
        //        _WeeklyHolidaysCollection = itemValuesCollection;
        //        _location = -1;
        //    }

        //    #region IEnumerator Members
        //    public void Reset()
        //    {
        //        _location = -1;
        //    }
        //    public object Current
        //    {
        //        get
        //        {
        //            if ((_location < 0) || (_location >= _WeeklyHolidaysCollection.Count))
        //            {
        //                throw (new InvalidOperationException());
        //            }
        //            else
        //            {
        //                return _WeeklyHolidaysCollection[_location];
        //            }
        //        }
        //    }

        //    public bool MoveNext()
        //    {
        //        _location++;

        //        if (_location >= _WeeklyHolidaysCollection.Count)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    #endregion
        //}
        //public class WeeklyHoidayEnumerator1 : IEnumerator<WeeklyHoliday>
        //{
        //    WeeklyHolidaysCollection _WeeklyHolidaysCollection;
        //    int _location;
        //    public WeeklyHoidayEnumerator1(WeeklyHolidaysCollection itemValuesCollection)
        //    {
        //        _WeeklyHolidaysCollection = itemValuesCollection;
        //        _location = -1;
        //    }
        //    public void Reset()
        //    {
        //        _location = -1;
        //    }
        //    public bool MoveNext()
        //    {
        //        _location++;

        //        if (_location >= _WeeklyHolidaysCollection.Count)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }

        //    object IEnumerator.Current
        //    {
        //        get
        //        {
        //            if ((_location < 0) || (_location >= _WeeklyHolidaysCollection.Count))
        //            {
        //                throw (new InvalidOperationException());
        //            }
        //            else
        //            {
        //                return _WeeklyHolidaysCollection[_location];
        //            }
        //        }
        //    }
        //    public WeeklyHoliday Current
        //    {
        //        get
        //        {
        //            if ((_location < 0) || (_location >= _WeeklyHolidaysCollection.Count))
        //            {
        //                throw (new InvalidOperationException());
        //            }
        //            else
        //            {
        //                return _WeeklyHolidaysCollection[_location];
        //            }
        //        }
        //    }
        //    void IDisposable.Dispose()
        //    {

        //    }
        //}
    }
}

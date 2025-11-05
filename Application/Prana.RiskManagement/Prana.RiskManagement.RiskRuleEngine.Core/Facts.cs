using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Nirvana.RuleEngine.Core
{
    public class Facts : IList<IFact>
    {
        ArrayList factsArrayList = new ArrayList();

        #region IList<IFact> Members

        public int IndexOf(IFact item)
        {
            return factsArrayList.IndexOf(item);
        }

        public void Insert(int index, IFact item)
        {
            factsArrayList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            factsArrayList.RemoveAt(index);
        }

        public IFact this[int index]
        {
            get
            {
                return (IFact)factsArrayList[index];
            }
            set
            {
                factsArrayList[index] = value;
            }
        }

        #endregion

        #region ICollection<IFact> Members

        public void Add(IFact item)
        {
            factsArrayList.Add(item);
        }

        public void Clear()
        {
            factsArrayList.Clear();
        }

        public bool Contains(IFact item)
        {
            return factsArrayList.Contains(item);
        }

        public void CopyTo(IFact[] array, int arrayIndex)
        {
            factsArrayList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return factsArrayList.Count; }
        }

        public bool IsReadOnly
        {
            get { return factsArrayList.IsReadOnly; }
        }

        public bool Remove(IFact item)
        {
            if (factsArrayList.Count > 0)
            {
                factsArrayList.Remove(item);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<IFact> Members

        public IEnumerator<IFact> GetEnumerator()
        {
            foreach (IFact myFact in factsArrayList)
                yield return myFact;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return factsArrayList.GetEnumerator();
        }

        #endregion
    }
    //public class Facts : IDictionary<string, string>
    //{
    //    Dictionary<string, string> factsDictionary = new Dictionary<string, string>();

    //    #region IDictionary<string,string> Members

    //    public void Add(string key, string value)
    //    {
    //        factsDictionary.Add(key, value);
    //    }

    //    public bool ContainsKey(string key)
    //    {
    //        return factsDictionary.ContainsKey(key);
    //    }

    //    public ICollection<string> Keys
    //    {
    //        get { return factsDictionary.Keys;}
    //    }

    //    public bool Remove(string key)
    //    {
    //        return factsDictionary.Remove(key);
    //    }

    //    public bool TryGetValue(string key, out string value)
    //    {
    //        return factsDictionary.TryGetValue(key,out value);
    //    }

    //    public ICollection<string> Values
    //    {
    //        get { return factsDictionary.Values; }
    //    }

    //    public string this[string key]
    //    {
    //        get
    //        {
    //            return factsDictionary[key];
    //        }
    //        set
    //        {
    //            factsDictionary[key] = value;
    //        }
    //    }

    //    #endregion

    //    #region ICollection<KeyValuePair<string,string>> Members

    //    public void Add(KeyValuePair<string, string> item)
    //    {
    //      Add(item.Key, item.Value);
    //    }

    //    public void Clear()
    //    {
    //        factsDictionary.Clear();
    //    }

    //    public bool Contains(KeyValuePair<string, string> item)
    //    {
    //        return factsDictionary.ContainsKey(item.Key) || factsDictionary.ContainsValue(item.Value);
    //    }

    //    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    //    {
    //        int intCounter = arrayIndex;

    //        foreach (
    //          KeyValuePair<string, string> objPair
    //          in factsDictionary)
    //        {
    //            array[intCounter++] = objPair;
    //        }
    //    }

    //    public int Count
    //    {
    //        get { return factsDictionary.Count; }
    //    }

    //    public bool IsReadOnly
    //    {
    //        get { return false; }
    //    }

    //    public bool Remove(KeyValuePair<string, string> item)
    //    {
    //        return factsDictionary.Remove(item.Key);
    //    }

    //    #endregion

    //    #region IEnumerable<KeyValuePair<string,string>> Members

    //    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    //    {
    //        foreach (KeyValuePair<string, string> factKeyValuePair in factsDictionary)
    //            yield return factKeyValuePair;
    //    }

    //    #endregion

    //    #region IEnumerable Members

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return factsDictionary.GetEnumerator();
    //    }

    //    #endregion
    //}
}

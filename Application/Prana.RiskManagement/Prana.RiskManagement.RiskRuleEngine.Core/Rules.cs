using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Nirvana.RuleEngine.Core
{
    public class Rules : IList<IRule>
    {
        ArrayList rulesArrayList = new ArrayList();

        #region IList<IRule> Members

        public int IndexOf(IRule item)
        {
            return rulesArrayList.IndexOf(item);
        }

        public void Insert(int index, IRule item)
        {
            rulesArrayList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            rulesArrayList.RemoveAt(index);
        }

        public IRule this[int index]
        {
            get
            {
                return (IRule) rulesArrayList[index];
            }
            set
            {
                rulesArrayList[index] = value;
            }
        }

        #endregion

        #region ICollection<IRule> Members

        public void Add(IRule item)
        {
            rulesArrayList.Add(item);
        }

        public void Clear()
        {
            rulesArrayList.Clear();
        }

        public bool Contains(IRule item)
        {
            return rulesArrayList.Contains(item);
        }

        public void CopyTo(IRule[] array, int arrayIndex)
        {
            rulesArrayList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return rulesArrayList.Count; }
        }

        public bool IsReadOnly
        {
            get { return rulesArrayList.IsReadOnly; }
        }

        public bool Remove(IRule item)
        {
            if (rulesArrayList.Count > 0)
            {
                rulesArrayList.Remove(item);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<IRule> Members

        public IEnumerator<IRule> GetEnumerator()
        {
            foreach (IRule myRule in rulesArrayList)
                yield return myRule;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return rulesArrayList.GetEnumerator();
        }

        #endregion
    }
}

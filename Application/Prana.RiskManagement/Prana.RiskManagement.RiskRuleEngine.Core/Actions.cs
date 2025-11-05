using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Nirvana.RuleEngine.Core
{
    public class Actions : IList<IAction>
    {
        ArrayList actionsArrayList = new ArrayList();

        #region IList<IAction> Members

        public int IndexOf(IAction item)
        {
            return actionsArrayList.IndexOf(item);
        }

        public void Insert(int index, IAction item)
        {
            actionsArrayList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            actionsArrayList.RemoveAt(index);
        }

        public IAction this[int index]
        {
            get
            {
                return (IAction)actionsArrayList[index];
            }
            set
            {
                actionsArrayList[index] = value;
            }
        }

        #endregion

        #region ICollection<IAction> Members

        public void Add(IAction item)
        {
            actionsArrayList.Add(item);
        }

        public void Clear()
        {
            actionsArrayList.Clear();
        }

        public bool Contains(IAction item)
        {
            return actionsArrayList.Contains(item);
        }

        public void CopyTo(IAction[] array, int arrayIndex)
        {
            actionsArrayList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return actionsArrayList.Count; }
        }

        public bool IsReadOnly
        {
            get { return actionsArrayList.IsReadOnly; }
        }

        public bool Remove(IAction item)
        {
            if (actionsArrayList.Count > 0)
            {
                actionsArrayList.Remove(item);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<IAction> Members

        public IEnumerator<IAction> GetEnumerator()
        {
            foreach (IAction myRule in actionsArrayList)
                yield return myRule;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return actionsArrayList.GetEnumerator();
        }

        #endregion
    }
}

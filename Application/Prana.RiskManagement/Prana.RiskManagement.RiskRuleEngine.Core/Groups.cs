using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Nirvana.RuleEngine.Core
{
    public class Groups : IList<IGroup>
    {
        ArrayList groupsArrayList = new ArrayList();

        #region IList<IGroup> Members

        public int IndexOf(IGroup item)
        {
            return groupsArrayList.IndexOf(item);
        }

        public void Insert(int index, IGroup item)
        {
            groupsArrayList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            groupsArrayList.RemoveAt(index);
        }

        public IGroup this[int index]
        {
            get
            {
                return (IGroup)groupsArrayList[index];
            }
            set
            {
                groupsArrayList[index] = value;
            }
        }

        #endregion

        #region ICollection<IGroup> Members

        public void Add(IGroup item)
        {
            groupsArrayList.Add(item);
        }

        public void Clear()
        {
            groupsArrayList.Clear();
        }

        public bool Contains(IGroup item)
        {
            return groupsArrayList.Contains(item);
        }

        public void CopyTo(IGroup[] array, int arrayIndex)
        {
            groupsArrayList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return groupsArrayList.Count; }
        }

        public bool IsReadOnly
        {
            get { return groupsArrayList.IsReadOnly; }
        }

        public bool Remove(IGroup item)
        {
            if (groupsArrayList.Count > 0)
            {
                groupsArrayList.Remove(item);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<IGroup> Members

        public IEnumerator<IGroup> GetEnumerator()
        {
            foreach (IGroup myGroup in groupsArrayList)
                yield return myGroup;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return groupsArrayList.GetEnumerator();
        }

        #endregion
    }
}

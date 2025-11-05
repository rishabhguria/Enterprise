using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.RuleEngine.Core;

namespace Nirvana.RiskManagement
{
    public class CompanyFactDataCollection : IFactDataCollection
    {
        #region IList<IDataMapping> Members

        public int IndexOf(IFactData item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, IFactData item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IFactData this[int index]
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<IDataMapping> Members

        public void Add(IFactData item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(IFactData item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(IFactData[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(IFactData item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<IDataMapping> Members

        public IEnumerator<IFactData> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}

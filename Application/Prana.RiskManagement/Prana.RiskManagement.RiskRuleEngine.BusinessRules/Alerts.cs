using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.RiskManagement.RiskRuleEngine.BusinessRules
{
    public class Alerts : IList<Alert>
    {

        #region IList<Alert> Members

        public int IndexOf(Alert item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, Alert item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Alert this[int index]
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

        #region ICollection<Alert> Members

        public void Add(Alert item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(Alert item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(Alert[] array, int arrayIndex)
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

        public bool Remove(Alert item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<Alert> Members

        public IEnumerator<Alert> GetEnumerator()
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

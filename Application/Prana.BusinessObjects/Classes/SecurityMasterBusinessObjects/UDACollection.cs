using System;
using System.ComponentModel;
//Collection of UDA attributes key value paires
namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class UDACollection : BindingList<UDA>
    {
        public UDA GetUDA(int id)
        {
            foreach (UDA uda in this.Items)
            {
                if (uda.ID == id)
                {
                    return uda;
                }
            }
            return null;
        }
        public bool Contains(int id)
        {
            foreach (UDA uda in this.Items)
            {
                if (uda.ID == id)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Contains(string name)
        {
            foreach (UDA uda in this.Items)
            {
                if (string.Compare(uda.Name, name, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetUDAId(string name)
        {
            foreach (UDA uda in this.Items)
            {
                if (string.Compare(uda.Name, name, true) == 0)
                {
                    return uda.ID;
                }
            }
            return -1;
        }
    }
}

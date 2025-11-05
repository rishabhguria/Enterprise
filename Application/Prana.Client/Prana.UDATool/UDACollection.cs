using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.ComponentModel;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.UDATool
{
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
                if (string.Compare(uda.Name,name,true)==0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

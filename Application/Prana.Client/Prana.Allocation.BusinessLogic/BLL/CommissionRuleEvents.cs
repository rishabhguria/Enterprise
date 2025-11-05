using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Allocation.BLL
{
    public class CommissionRuleEvents : EventArgs
    {
        bool _groupwise = false;
       
        public bool GroupWise
        {
            get { return _groupwise; }
            set { _groupwise = value; }
        }
        
    }
}

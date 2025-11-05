using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BasketTrading
{
   public  class TemplateOrderSide
    {       
       private string sideMappingString = string.Empty;
       
       public string SideMappingString
        {
            set { sideMappingString = value; }
            get { return sideMappingString; }
        }
    }
}

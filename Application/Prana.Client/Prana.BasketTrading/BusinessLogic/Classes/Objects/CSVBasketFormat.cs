using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
namespace Prana.BasketTrading
{
    class CSVBasketFormat :BasketFormat 
    {
        public  override OrderCollection  LoadFile(string path,Templates template)
        {
            OrderCollection orders = new OrderCollection();
            //foreach (string str in template.SelectedColumns)
            //{ 

            //}
            return orders;

        }
    }
}

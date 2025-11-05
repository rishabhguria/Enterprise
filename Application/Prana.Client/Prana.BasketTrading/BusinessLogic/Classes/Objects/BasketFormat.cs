using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
namespace Prana.BasketTrading
{
   abstract  class BasketFormat
    {
       public abstract OrderCollection  LoadFile(string path,Templates template);
      
    }
}

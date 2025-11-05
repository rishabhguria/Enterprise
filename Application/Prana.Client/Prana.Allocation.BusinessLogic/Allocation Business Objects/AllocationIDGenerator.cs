using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Allocation.BLL
{
   public  class AllocationIDGenerator
    {
        //private static Int64 _seed = Int64.Parse(DateTime.Now.GetDateTimeFormats()[105].Remove(4, 1).Remove(6, 1).Remove(8, 1).Remove(10, 1).Remove(12, 1) + String.Format("{0:000}", DateTime.Now.Millisecond));
        public static string GenerateOrderEntityID()
        {
            return System.Guid.NewGuid().ToString();
        }
       public static string GenerateOrderGroupID()
       {
           return System.Guid.NewGuid().ToString();
       }
       public static string GenerateBasketGroupID()
       {
           return System.Guid.NewGuid().ToString();
       }
       public static string GenerateDefaultID()
       {
           return System.Guid.NewGuid().ToString();
       }
       public static string GenerateGroupID()
       {
           return System.Guid.NewGuid().ToString();
       }
    }
}

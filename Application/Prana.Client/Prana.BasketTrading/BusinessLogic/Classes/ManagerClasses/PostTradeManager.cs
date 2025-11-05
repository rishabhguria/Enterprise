using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
namespace Prana.BasketTrading
{
   public class PostTradeManager
    {
        private static PostTradeManager _postTradeManager = null;
        private PostTradeManager()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
       public static PostTradeManager GetInstance()
        {
            if (_postTradeManager == null)
            {
                _postTradeManager = new PostTradeManager();
            }

            return _postTradeManager;

        }
       public void PostTradeAnalysis()
       { }

    }
}

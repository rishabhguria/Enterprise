using System;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// Summary description for NewsEventArg.
    /// </summary>
    public class NewsEventArg : EventArgs
    {
        private string strSymbolList = "";
        //		private string strServiceList ="";
        public NewsEventArg()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public NewsEventArg(string SymbolList)//, string ServiceList)
        {
            this.strSymbolList = SymbolList;
            //			this.strServiceList = ServiceList;
            //
            // TODO: Add constructor logic here
            //
        }


        public string SymbolList
        {
            get
            {
                return strSymbolList;
            }
        }
        //		public string ServiceList
        //		{
        //			get
        //			{
        //				return strServiceList;
        //			}
        //		}
    }
}

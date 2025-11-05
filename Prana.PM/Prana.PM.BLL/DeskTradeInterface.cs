using Csla;
using Prana.BusinessObjects;
using System.ComponentModel;
namespace Prana.PM.BLL
{
    public class DeskTradeInterface : BusinessBase<DeskTradeInterface>
    {
        //private User _trader;

        /// <summary>
        /// Gets or sets the trader.
        /// </summary>
        /// <value>The trader.</value>
        //public User Trader
        //{
        //    get { return _trader; }
        //    set { _trader = value; }
        //}

        //private TradingAccount _tradingAccount;

        //public TradingAccount TargetTradingAccount
        //{
        //    get { return _tradingAccount; }
        //    set { _tradingAccount = value; }
        //}


        private BindingList<TradingInstruction> _deskTradeList;

        /// <summary>
        /// Gets or sets the desk trade list.
        /// </summary>
        /// <value>The desk trade list.</value>
        public BindingList<TradingInstruction> DeskTradeList
        {
            get { return _deskTradeList; }
            set { _deskTradeList = value; }
        }


        // int _id;
        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            //return _id;
            return 0;
        }

    }
}

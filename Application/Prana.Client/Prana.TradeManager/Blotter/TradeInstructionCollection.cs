using Prana.BusinessObjects;
//using Prana.InstanceCreator;
using Prana.CommonDataCache;
using System.Collections.Generic;
using System.ComponentModel;
namespace Prana.TradeManager
{
    public class TradeInstructionCollection
    {
        static private TradeInstructionCollection _tradeInstCollection = null;
        BindingList<TradingInstruction> _tradeList = new BindingList<TradingInstruction>();
        Dictionary<string, TradingInstruction> _dictTradingInst = new Dictionary<string, TradingInstruction>();

        private TradeInstructionCollection()
        {
        }

        public static TradeInstructionCollection GetInstance()
        {
            if (_tradeInstCollection == null)
            {
                _tradeInstCollection = new TradeInstructionCollection();
            }
            return _tradeInstCollection;
        }

        public void UpdateTradingInstruction(TradingInstruction tradingInst)
        {
            if (_dictTradingInst.ContainsKey(tradingInst.ClOrderID))
            {
                TradingInstruction tradInst = _dictTradingInst[tradingInst.ClOrderID];
                tradInst.UserID = tradingInst.UserID;
                tradInst.User = CachedDataManager.GetInstance.GetUserText(tradInst.UserID);
                tradInst.IsSelected = false;
                tradInst.Status = tradingInst.Status;
            }
        }

        public BindingList<TradingInstruction> TradingInstructionColleciton
        {
            get { return _tradeList; }
        }
    }
}
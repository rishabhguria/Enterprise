using Prana.BusinessObjects;
using System.Collections.Generic;


namespace Prana.TradingTicket
{
    public class AlgoStrategy
    {
        private string ID;

        public string StrategyID
        {
            get { return ID; }
            set { ID = value; }
        }

        private string _name;

        public string StrategyName
        {
            get { return _name; }
            set { _name = value; }
        }


        Dictionary<string, AlgoStrategyUserControl> _algoStrategyCtrlDict = new Dictionary<string, AlgoStrategyUserControl>();
        public Dictionary<string, AlgoStrategyUserControl> AlgoStrategyCtrlDict
        {
            get { return _algoStrategyCtrlDict; }
        }

    }
}

using System;

namespace Prana.Allocation.BLL

{
	/// <summary>
	/// Summary description for Allocation.
	/// </summary>
	public class AllocationStrategy
	{
		//string _fundAccountGroupID;

		int  _strategyID;
		string _strategyName;
        string _groupID;
        double _orderQty;
        double _percentage;
		public AllocationStrategy()
		{
			

		}

		
		public AllocationStrategy(int strategyAccountID,string strategyname,Int64 orderQty)
		{
			_strategyID=strategyAccountID;
			_strategyName=strategyname;
			_orderQty=orderQty;

		}
        public AllocationStrategy(int strategyAccountID, string strategyname, double orderQty, double percentage)
		{
			_strategyID=strategyAccountID;
			_strategyName=strategyname;
			_orderQty=orderQty;
			_percentage=percentage;

		}
		public AllocationStrategy(int strategyAccountID,string strategyname)
		{
			_strategyID=strategyAccountID;
			_strategyName=strategyname;
			

		}

		//		

		public string StrategyName
		{
			set{_strategyName=value;}
			get{return _strategyName;}
		}
		public int StrategyID
		{
			set{_strategyID=value;}
			get{return _strategyID;}
		}

        public double Percentage
		{
			set{_percentage=value;}
			get{return _percentage;}
		}
        public double AllocatedQty
		{
			set{_orderQty=value;}
			get{return _orderQty;}
		}

        public string GroupID
        {
            set { _groupID = value; }
            get { return _groupID; }
        }

	}
}

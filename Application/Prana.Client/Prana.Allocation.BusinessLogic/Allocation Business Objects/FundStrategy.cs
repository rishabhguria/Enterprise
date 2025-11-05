using System;

namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for FundStrategy.
	/// </summary>
	public class FundStrategy
	{
		public FundStrategy()
		{
			
		}
	
		int _fundID;
		string _fundName;
		int _strategyID;
		string _strategyName;
		
		public string FundName
		{
			set{_fundName=value;}
			get{return _fundName;}
		}
		public int FundID
		{
			set{_fundID=value;}
			get{return _fundID;}
		}
		
		public string StrategyName
		{
			set{_strategyName =value;}
			get{return _strategyName ;}
		}
		public int StrategyID
		{
			set{_strategyID =value;}
			get{return _strategyID ;}
		}
		

	}
}

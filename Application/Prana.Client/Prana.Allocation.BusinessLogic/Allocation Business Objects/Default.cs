using System;

namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for Default.
	/// </summary>
	public class Default
	{
		
		private string _defaultID;
		private string _defaultName;
		private string _fundIDs;
		private string _percentages;


		
		public Default()
		{
		}
		public Default(string defaultname,string id)
		{
			_defaultName=defaultname;
			_defaultID=id;
		}


		public string DefaultID
		{
			set{_defaultID=value;}
			get{return _defaultID;}
		}

		public string DefaultName
		{
			set{_defaultName=value;}
			get{return _defaultName;}
		}
		
		public string FundIDs
		{
			set{_fundIDs=value;}
			get{return _fundIDs;}
		}

		public string Percentages
		{
			set{_percentages=value;}
			get{return _percentages;}
		}

	}
}

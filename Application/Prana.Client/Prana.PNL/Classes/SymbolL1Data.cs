using System;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for SymbolL1Data.
	/// </summary>
	public class SymbolL1Data
	{
		public SymbolL1Data()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		string _symbol = string.Empty;
		public string Symbol
		{
			get{return _symbol;}
			set{_symbol = value;}
		}

		double _last = double.MinValue;
		public double Last
		{
			get{return _last;}
			set{_last = value;}
		}

		double _bidPrice = double.MinValue;
		public double Bid
		{
			get{return _bidPrice;}
			set{_bidPrice = value;}
		}

		double _askPrice = double.MinValue;
		public double Ask
		{
			get{return _askPrice;}
			set{_askPrice = value;}
		}

		double _change = double.MinValue;
		public double Change
		{
			get{return _change;}
			set{_change = value;}
		}

		double _close = double.MinValue;
		public double Close
		{
			get{return _close;}
			set{_close = value;}
		}		

		double _previous = double.MinValue;
		public double Previous
		{
			get{return _previous;}
			set{_previous = value;}
		}	

	}
}

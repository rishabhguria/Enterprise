using System;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
	/// <summary>
	/// Summary description for Country.
	/// </summary>
	public class Country
	{
		#region Private
		
		int _countryID = int.MinValue;
		string _countryName = string.Empty;

		#endregion

		#region Constructors

		public Country()
		{
		}

		public Country(int countryID, string countryName)
		{
			_countryID = countryID;
			_countryName = countryName;
		}

		#endregion

		#region Properties
		
		public int CountryID
		{
			get{return _countryID;}
			set{_countryID = value;}
		}

		public string Name
		{
			get{return _countryName;}
			set{_countryName = value;}
		}

		#endregion
	}
}

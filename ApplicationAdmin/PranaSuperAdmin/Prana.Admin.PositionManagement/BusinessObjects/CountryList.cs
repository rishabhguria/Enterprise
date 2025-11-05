using System;
using System.Collections.Generic;
using Nirvana.Admin.BLL;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CountryList : SortableSearchableList<Country>
    {
        public static SortableSearchableList<Country> Retrieve
        {
            get { return RetrieveCountryList(); }
        }

        /// <summary>
        /// Retrieves the country list.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<Country> RetrieveCountryList()
        {
            SortableSearchableList<Country> CountryList = DataSourceManager.GetCountryList();
            CountryList.Insert(0, new Country(0, "--Select--"));
            return CountryList;
        }
    }
}

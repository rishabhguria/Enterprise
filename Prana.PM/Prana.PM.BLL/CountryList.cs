using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.PM.DAL;
using Prana.BusinessObjects;
//

namespace Prana.PM.BLL
{
    public class CountryList : SortableSearchableList<Country>
    {
        public static SortableSearchableList<Country> Retrieve
        {
            get
            {
                SortableSearchableList<Country> countryList = new SortableSearchableList<Country>();
                try
                {
                    countryList = RetrieveCountryList();
                }
                catch (Exception ex)
                {

                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                return countryList;
            }
        }

        /// <summary>
        /// Retrieves the country list.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<Country> RetrieveCountryList()
        {
            SortableSearchableList<Country> CountryList = new SortableSearchableList<Country>();
            try
            {
                CountryList = DataSourceManager.GetCountryList();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            CountryList.Insert(0, new Country(0, Prana.Global.ApplicationConstants.C_COMBO_SELECT));
            return CountryList;
        }
    }
}

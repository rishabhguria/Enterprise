using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CompanyNameIDList : SortableSearchableList<CompanyNameID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyNameIDList"/> class.
        /// </summary>
        private CompanyNameIDList()
        {

        }

        /// <summary>
        /// Retrieves the list of all the companies permissioned for PM.
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<CompanyNameID> Retrieve()
        {
            SortableSearchableList<CompanyNameID> companyNameIDList = CompanyManager.GetCompanyNameIDList();
            companyNameIDList.Insert(0, new CompanyNameID(-1, Constants.C_COMBO_SELECT, Constants.C_COMBO_SELECT));
            return companyNameIDList;
        }

        
    }
}

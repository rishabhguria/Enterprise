using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CompanyTypeList : SortableSearchableList<CompanyType>
    {
        public static SortableSearchableList<CompanyType> Retrieve
        {
            get { return RetrieveCompanyList(); }
        }

        /// <summary>
        /// Retrieves the Company Type list.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<CompanyType> RetrieveCompanyList()
        {
            SortableSearchableList<CompanyType> companyTypeList = CompanyManager.GetCompanyTypeList();
            companyTypeList.Insert(0, new CompanyType(0, "--Select--"));
            return companyTypeList;
        }
    }    
}

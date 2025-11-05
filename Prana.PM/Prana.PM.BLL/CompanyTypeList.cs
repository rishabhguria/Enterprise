using Csla;
using Prana.PM.DAL;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class CompanyTypeList : BusinessListBase<CompanyTypeList, CompanyType>
    {
        public static CompanyTypeList Retrieve
        {
            get { return RetrieveCompanyList(); }
        }

        /// <summary>
        /// Retrieves the Company Type list.
        /// </summary>
        /// <returns></returns>
        private static CompanyTypeList RetrieveCompanyList()
        {
            CompanyTypeList companyTypeList = CompanyManager.GetCompanyTypeList();
            companyTypeList.Add(new CompanyType(0, Prana.Global.ApplicationConstants.C_COMBO_SELECT));
            return companyTypeList;
        }
    }
}

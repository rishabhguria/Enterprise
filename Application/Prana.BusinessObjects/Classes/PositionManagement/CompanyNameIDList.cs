//using Prana.PM.ApplicationConstants;
//using Prana.PM.DAL;

using Csla;
using System;


namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class CompanyNameIDList : BusinessListBase<CompanyNameIDList, CompanyNameID>
    {


        /// <summary>
        /// Retrieves the list of all the companies permissioned for PM.
        /// </summary>
        /// <returns></returns>
        //public static CompanyNameIDList Retrieve()
        //{
        //    CompanyNameIDList companyNameIDList = null; 
        //    //companyNameIDList.Insert(0, new CompanyNameID(-1, Constants.C_COMBO_SELECT, Constants.C_COMBO_SELECT));
        //    return companyNameIDList;
        //}


    }
}

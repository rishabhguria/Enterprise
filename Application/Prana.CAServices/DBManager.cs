using Prana.BusinessObjects;
using Prana.SecurityMasterNew;
using System.Data;

namespace Prana.CAServices
{
    static class DBManager
    {

        internal static DataSet GetFullCAData(CAOnProcessObjects caOnProcessObject)
        {
            return SecMasterDataManager.GetFullCAData();
        }

        internal static DataSet GetAllCAs(CAOnProcessObjects caOnProcessObject)
        {
            return SecMasterDataManager.GetAllCorporateActions(caOnProcessObject.CAType, caOnProcessObject.FromDate, caOnProcessObject.ToDate, caOnProcessObject.IsApplied);
        }

        internal static DataSet getLatestCorpAction(string caSymbols)
        {
            return SecMasterDataManager.getLatestCorpAction(caSymbols);
        }

        internal static bool SaveCorporateActionWithoutApplying(CAOnProcessObjects caOnProcessObject)
        {
            int affectedRows = SecMasterDataManager.SaveCorporateAction(caOnProcessObject.CorporateActionListString, caOnProcessObject.IsApplied);
            if (affectedRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool UpdateCorporateActionWithoutApplying(CAOnProcessObjects caOnProcessObject)
        {
            int affectedRows = SecMasterDataManager.UpdateCorporateAction(caOnProcessObject.CorporateActionListString, caOnProcessObject.IsApplied);
            if (affectedRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteCorproateActions(string caIDs)
        {
            int affectedRows = SecMasterDataManager.DeleteCorproateActions(caIDs);
            if (affectedRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}

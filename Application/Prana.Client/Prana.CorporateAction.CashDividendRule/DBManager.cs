using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.SecurityMasterNew;
using System.Collections.Generic;
using System.Data;

namespace Prana.CorporateAction.CashDividendRule
{
    static class DBManager
    {
        internal static bool SaveCAForCashDividend(CAOnProcessObjects caOnProcessObject, IActivityServices activityService)
        {
            Logger.LoggerWrite("Corporate Action (Cash Dividend) data saving starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            Logger.LoggerWrite("Corporate Action (Cash Dividend): saving in Security Master DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions1 = SecMasterDataManager.SaveCorporateAction(caOnProcessObject.CorporateActionListString, caOnProcessObject.IsApplied);
            Logger.LoggerWrite("Corporate Action (Cash Dividend): saved in Security Master DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            Logger.LoggerWrite("Corporate Action (Cash Dividend): Taxlots affected due to Corporate action, saving in tables T_CashTransactions and PM_CorpActionsTaxlots client DB...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            DataSet ds = AllocationDataManager.SaveCashDividendForTaxlots(caOnProcessObject.Taxlots);
            Logger.LoggerWrite("Corporate Action (Cash Dividend): Taxlots affected due to Corporate action, saved in tables T_CashTransactions and PM_CorpActionsTaxlots client DB", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

            Logger.LoggerWrite("Corporate Action (Cash Dividend) data saving completed", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            int affectedPositions2 = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    dr.SetAdded();

                List<CashActivity> lsCashActivity = activityService.GenerateCashActivity(ds, CashTransactionType.CorpAction);
                affectedPositions2 = ds.Tables[0].Rows.Count;
            }

            bool isSuccessful = ((affectedPositions1 > 0) && (affectedPositions2 > 0));
            return isSuccessful;
        }

        internal static bool UndoCashDividend(CAOnProcessObjects caOnProcessObject, IActivityServices activityService, string taxlotIDs, bool isSMModificationRequired)
        {
            string caIds = caOnProcessObject.CorporateActionIDs;
            int affectedPositions1 = 0;
            Logger.LoggerWrite("Undo Corporate Action (Cash Dividend) Delete CA Information from Security Master starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            if (isSMModificationRequired)
                affectedPositions1 = SecMasterDataManager.UndoCA(caIds);

            //List<string> cashIDs =  AllocationDataManager.GetFKIDForDivUndo(caIds);
            Logger.LoggerWrite("Undo Corporate Action (Cash Dividend) Delete Taxlots starts...", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            DataSet undoDividends = AllocationDataManager.UndoCashDividend(caIds, taxlotIDs);

            int affectedPositions2 = 0;
            if (undoDividends != null && undoDividends.Tables.Count > 0 && undoDividends.Tables[0] != null)
            {
                //so that rows flow with deleted state
                foreach (DataRow dr in undoDividends.Tables[0].Rows)
                    dr.Delete();

                List<CashActivity> lsCashActivity = activityService.GenerateCashActivity(undoDividends.GetChanges(), CashTransactionType.CorpAction);
                affectedPositions2 = undoDividends.Tables[0].Rows.Count;

            }
            Logger.LoggerWrite("Corporate Action (Cash Dividend): Undo data completed", LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
            bool isSuccessful = (affectedPositions1 > 0 || affectedPositions2 > 0);
            return isSuccessful;
        }

    }
}

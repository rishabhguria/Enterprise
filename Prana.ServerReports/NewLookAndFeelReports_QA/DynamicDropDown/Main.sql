SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF Not EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors_ForPatch')) CREATE TABLE #tmpErrors_ForPatch (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION T_ForPatch
GO
:r "foldername\ClientDB_1_T_GetReportGroupRowColors_New.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\ClientDB_2_P_GetReportGroupRowColors_New.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\ClientDB_P_GetCurrentPlusOneBusinessDate.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\ClientDB_P_MW_GetStartOfDayNAVMTM.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\ClientDB_P_MW_GetTradingNontradingType.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_GetAllCounterPartyDynamically_Activity.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_GetAssetsDynamically.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_GetAssetsDynamically_Activity.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_GetTradeCurrencyDynamically.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_GetTradeCurrencyDynamically_Activity.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_GetTransactionTypesDynamically_Activity.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_UDAGetAllSectorDynamically.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_UDAGetAllSectorDynamically_Activity.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_UDAGetAllSubSectorDynamically.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
:r "foldername\P_UDAGetAllSubSectorDynamically_Activity.sql"
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors_ForPatch (Error) SELECT 1 BEGIN TRANSACTION T_ForPatch END
GO
IF EXISTS (SELECT * FROM #tmpErrors_ForPatch) ROLLBACK TRANSACTION T_ForPatch
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION T_ForPatch
END
ELSE PRINT 'The database update failed. No need to restore DB since no changes made.'
GO

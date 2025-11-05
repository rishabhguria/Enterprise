
CREATE PROCEDURE [dbo].[P_AL_GetCostAdjustmentTaxlots]
AS

BEGIN TRY

DECLARE @intErrorCode INT

BEGIN TRAN                                                 
                                                          
BEGIN                                                                               

	SELECT	CA.CostAdjustmentID, 
			CA.TaxlotID, 
			CA.GroupID, 
			CA.ClosingTaxlotID,
			CA.ClosingID, 
			CA.ParentRow_Pk, 
			MAX(PM.TaxLot_PK) AS Taxlot_PK,
			CA.UTCInsertionTime
	FROM	T_AL_CostAdjustedTaxlots CA
	INNER JOIN PM_Taxlots PM
	ON		CA.TaxlotID = PM.TaxLotID
			AND PM.TaxLotOpenQty > 0
	WHERE	CA.CostAdjustmentID IS NOT NULL
			AND CA.CostAdjustmentID <> '00000000-0000-0000-0000-000000000000'
	GROUP BY CA.CostAdjustmentID, CA.TaxlotID, CA.GroupID, CA.ClosingTaxlotID, CA.ParentRow_Pk, CA.UTCInsertionTime, CA.ClosingID


END 

COMMIT 

END TRY


BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK
    DECLARE @ErrorMessage	NVARCHAR(4000);
    DECLARE @ErrorSeverity	INT;
    DECLARE @ErrorState		INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	
    -- Use RAISERROR inside the CATCH block to return 
    -- error information about the original error that 
    -- caused execution to jump to the CATCH block.
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );

END CATCH


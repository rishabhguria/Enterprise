
CREATE PROCEDURE [dbo].[P_AL_SaveCostAdjustmentTaxlots]  
(   
	@xml			NTEXT,                                                          
	@ErrorMessage	VARCHAR(500)	OUTPUT,                                                                                                   
	@ErrorNumber	INT				OUTPUT 
)                                                        
AS
                                                                                  
SET @ErrorNumber = 0                                                                                  
SET @ErrorMessage = 'Success'  

BEGIN TRY

BEGIN TRAN TRAN1                                                                               

	DECLARE @handle INT

	CREATE TABLE #T_AL_CostAdjustedTaxlots
	(
		[CostAdjustmentID]	UNIQUEIDENTIFIER,
		[FKID]				BIGINT,
		[ParentRow_Pk]		BIGINT,
		[TaxlotID]			VARCHAR(50),
		[GroupID]			VARCHAR(50),
		[ClosingTaxlotID]	VARCHAR(50),
		[ClosingID]			UNIQUEIDENTIFIER
	)

	EXEC sp_xml_preparedocument @handle OUTPUT,@Xml

	INSERT INTO #T_AL_CostAdjustedTaxlots(CostAdjustmentID, TaxlotID, GroupID, ClosingTaxlotID, ClosingID)
	SELECT
		CAID,
		TaxlotID,
		GroupID,
		ClosingTaxlotID,
		ClosingUniqueID                                                
	FROM OPENXML(@handle, '//ArrayOfCostAdjustmentTaxlotsForSave/CostAdjustmentTaxlotsForSave', 2)
	WITH
	(   
		CAID		VARCHAR(500),
		TaxlotID	VARCHAR(50),
		GroupID		VARCHAR(50),  
		ClosingTaxlotID		VARCHAR(50),
		ClosingUniqueID		VARCHAR(500)
	)

	EXEC sp_xml_removedocument @handle

	UPDATE	PCA
	SET		PCA.FKID			= A.FK,
			PCA.ParentRow_Pk	= A.ParentPk
	FROM	#T_AL_CostAdjustedTaxlots PCA
	INNER JOIN (
				SELECT	MAX(TaxLotID) AS FK
						,TaxLotID
						,MIN(ParentRow_Pk) AS ParentPk
				FROM PM_Taxlots
				GROUP BY TaxLotID
				) A ON A.TaxLotID = PCA.TaxLotID
	WHERE	PCA.CostAdjustmentID IS NOT NULL
				AND PCA.CostAdjustmentID <> '00000000-0000-0000-0000-000000000000'


	INSERT INTO T_AL_CostAdjustedTaxlots( CostAdjustmentID, FKID, ParentRow_Pk, TaxlotID, GroupID, ClosingTaxlotID, ClosingID)
	SELECT		CostAdjustmentID, FKID, ParentRow_Pk, TaxlotID, GroupID, ClosingTaxlotID, ClosingID
	FROM		#T_AL_CostAdjustedTaxlots

COMMIT TRANSACTION TRAN1  

END TRY


BEGIN CATCH

	SET @ErrorMessage = ERROR_MESSAGE();                                                                          
	SET @ErrorNumber = ERROR_NUMBER();
	
	ROLLBACK TRANSACTION TRAN1

END CATCH


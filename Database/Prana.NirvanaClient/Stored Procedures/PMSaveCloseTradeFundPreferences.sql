/****************************************************************************
Name :   PMSaveCloseTradeFundPreferences
Date Created: 04-dec-2006 
Purpose:  To save fund preferences for the last close trade.
Author: Bhupesh Bareja
Parameters: 
			@FundID int
		 @CloseTradeReportID datetime
		 @ErrorNumber int output
		 @ErrorMessage varchar(100) output
Execution StateMent: 
			
			EXEC PMSaveCloseTradeFundPreferences 2, 1, 0, ' '

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMSaveCloseTradeFundPreferences]
	(
		  @FundID int
		, @CloseTradeReportID int
		, @ErrorNumber int output
		, @ErrorMessage varchar(100) output
	)
AS 

--Declare @Error int

SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'
BEGIN TRY

BEGIN TRAN

INSERT  INTO
		PM_CloseTradeReportRunsFunds 
			(
				  CompanyFundID
				, CloseTradeReportID
			)
VALUES
			(
				  @FundID
				, @CloseTradeReportID
			)

COMMIT TRAN

END TRY
BEGIN CATCH
	
	SET @ErrorNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
	
	ROLLBACK TRAN
END CATCH;






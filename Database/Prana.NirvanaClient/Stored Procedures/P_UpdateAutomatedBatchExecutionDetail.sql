CREATE PROCEDURE [dbo].[P_UpdateAutomatedBatchExecutionDetail]
	 @Id INT,
     @isSuccess BIT,
     @time DATETIME
AS

UPDATE T_ThirdPartyTimeBatches
SET BatchSuccess = @isSuccess,
    LastRunDate = @time
WHERE Id = @Id;


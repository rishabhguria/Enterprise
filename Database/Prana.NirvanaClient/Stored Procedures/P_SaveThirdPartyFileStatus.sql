CREATE PROCEDURE P_SaveThirdPartyFileStatus
@BatchRunDate DateTime,
@ThirdPartyBatchId int
AS
BEGIN
    Insert into T_ThirdPartyFileStatus(BatchRunDate, ThirdPartyBatchId) values (@BatchRunDate, @ThirdPartyBatchId)
	select @@IDENTITY
END
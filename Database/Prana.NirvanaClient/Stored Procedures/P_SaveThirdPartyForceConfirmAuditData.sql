CREATE Procedure [dbo].[P_SaveThirdPartyForceConfirmAuditData]
(
	@UserID INT,
	@ConfirmationDateTime datetime,
	@Broker NVARCHAR(50),
	@Symbol VARCHAR (100),
	@Side NVARCHAR(50),
	@Quantity NVARCHAR(50),
	@AllocationId NVARCHAR(50),
	@ThirdPartyBatchId INT,
    @Comment NVARCHAR(MAX),
	@BlockID INT
)
AS
BEGIN
	UPDATE T_ThirdPartyAllocationBlocks SET AllocStatus = 0 WHERE BlockId = @BlockID

	INSERT INTO T_ThirdPartyForceConfirmAudit([CompanyUserId], ConfirmationDateTime, [Broker], Symbol, Side, Quantity, AllocationId, ThirdPartyBatchId, Comment) 
	VALUES (@UserID, @ConfirmationDateTime, @Broker, @Symbol, @Side, @Quantity, @AllocationId, @ThirdPartyBatchId, @Comment)
END
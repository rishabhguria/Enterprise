
CREATE PROCEDURE [dbo].[P_InActiveAuditData]
(
 @AuditDimensionValue INT,
 @CreationID INT,
 @UpdationID INT,
 @ApprovalID INT
)

AS
BEGIN

UPDATE [dbo].[T_AdminAuditTrail] SET IsActive = 0 WHERE AuditDimensionValue = @AuditDimensionValue and 
ActionID in (@CreationID,@UpdationID,@ApprovalID)

END


CREATE PROCEDURE [dbo].[P_SaveAuditData]
(@CompanyID INT,
 @CompanyFundID INT,
 @UserID INT,
 @ActionID INT,
 @ExecutionTime DATETIME,
 @StatusID INT,
 @Comments VARCHAR(100),
 @ModuleID INT,
 @ActualActionDate DATETIME,
 @AuditDimensionValue INT,
 @IsActive BIT
)

AS
BEGIN

INSERT INTO [dbo].[T_AdminAuditTrail]
           ([CompanyID]
           ,[CompanyFundID]
           ,[UserID]
           ,[ActionID]
           ,[ExecutionTime]
           ,[StatusID]
           ,[Comments]
           ,[ModuleID]
           ,[ActualActionDate]
           ,[AuditDimensionValue]
           ,[IsActive])
     VALUES
           (@CompanyID
           ,@CompanyFundID
           ,@UserID
           ,@ActionID
           ,@ExecutionTime
           ,@StatusID
           ,@Comments
           ,@ModuleID
           ,@ActualActionDate
           ,@AuditDimensionValue
           ,@IsActive)

END

CREATE PROCEDURE [dbo].[P_SaveCashAuditAction]
(
@userId int, 
@accountIds varchar(MAX), 
@fromDate datetime,
@toDate datetime,
@action varchar(150)	
)
AS 

INSERT INTO T_CashAudit
VALUES (GETDATE(),@action,@fromDate,@toDate,@accountIds,@userId)
CREATE PROCEDURE dbo.P_DeleteRMAUEC 
	
	(
	@companyID int ,
	@auecID int
	)
	
AS
	DELETE FROM T_RMAUECs
	WHERE        (CompanyID = @companyID) AND (AUECID = @auecID)

CREATE PROCEDURE dbo.P_DeleteAllRMAUEC
	
	(
	@companyID int 

	)
	AS
	DELETE FROM T_RMAUECs
	WHERE        (CompanyID = @companyID)

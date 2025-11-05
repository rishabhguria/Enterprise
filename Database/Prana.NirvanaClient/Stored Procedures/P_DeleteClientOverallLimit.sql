CREATE PROCEDURE dbo.P_DeleteClientOverallLimit
	(
	@companyID int ,
	@companyClientID int
	)
	
AS
BEGIN TRAN
	DELETE FROM T_RMCompanyClientOverall
	WHERE        (CompanyID = @companyID) AND (ClientID = @companyClientID)

if (@@error <> 0)
Begin 
 ROLLBACK TRAN
End
Commit tran
return @@ROWCOUNT

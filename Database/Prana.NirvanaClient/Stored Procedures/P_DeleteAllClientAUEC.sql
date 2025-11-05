

CREATE PROCEDURE dbo.P_DeleteAllClientAUEC

	(
		@CompanyClientID int
	)
as

delete from T_CompanyClientAUEC  where CompanyClientID = @CompanyClientID
	



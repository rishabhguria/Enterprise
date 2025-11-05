

CREATE PROCEDURE dbo.P_DeleteCompanyClientByIDForRM
	(
		@companyID int,
		@companyClientID int,
		@deleteForceFully int
	)
AS
	--Delete Corresponding Company Client Details	
	--if ( @deleteForceFully = 1)
	--	begin 
		Delete T_RMCompanyClientOverall
			Where ClientID = @companyClientID
		--end
return 1



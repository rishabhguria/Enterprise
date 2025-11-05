
/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientByID    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientByID
	(
		@companyID int,
		@companyClientID int,
		@deleteForceFully int
	)
AS
	--Delete Corresponding Company Client Details	
	if ( @deleteForceFully = 1)
		begin 
			Declare @total int
			Select @total = Count(1) 
			From T_RMCompanyClientOverall
			Where ClientID = @companyClientID
	
			if ( @total = 0)
			begin
				-- If Company Client is referenced anywhere and still we want to delete it.
				--Delete Company Client and related information.
				------------------------ Start : Delete dependent data ---------------------------------
				exec P_DeleteCompanyClientFund @companyClientID
				
				exec P_DeleteCompanyClientTradingAccounts @companyClientID
				
				exec P_DeleteCompanyClientTrader @companyClientID
				
				exec P_DeleteCompanyClientClearers @companyClientID
				
				exec P_DeleteCompanyClientAUECs @companyClientID
				
				exec P_DeleteCompanyClientFIXs @companyClientID
				
				exec P_DeleteCompanyClientIdentifiers @companyClientID
				------------------------ End   : Delete dependent data ----------------------------------
				
				Delete T_CompanyClient
				Where CompanyClientID = @companyClientID
				
				return @companyClientID
			end
			else
			begin
				return -1
			end
		end

				
	

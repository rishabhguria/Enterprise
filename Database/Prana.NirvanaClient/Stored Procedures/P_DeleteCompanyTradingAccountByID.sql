
/****** Object:  Stored Procedure dbo.P_DeleteCompanyTradingAccountByID    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE [dbo].[P_DeleteCompanyTradingAccountByID]
	(
		@companyTradingAccountID int	
	)
AS
--Declare @companyTradingAccountID int

--		Select @total = Count(1) 
--		From T_CompanyClientTradingAccount
--		Where CompanyTradingAccountID = @companyTradingAccountID
	--set @companyTradingAccountID = 28
--edited by amit on 23/04/2015
--http://jira.nirvanasolutions.com:8080/browse/PRANA-7307
		if ((select count(CompanyTradingAccountID) from T_CompanyClientTradingAccount where CompanyTradingAccountID = @companyTradingAccountID) = 0 
		and (select count(TradingAccountID) from T_CompanyUserTradingAccounts where TradingAccountID = @companyTradingAccountID) = 0
		and (select count(TradingAccountID) from T_Group where TradingAccountID = @companyTradingAccountID) = 0)
		begin
			Delete T_CompanyClientTradingAccount
			Where CompanyTradingAccountID = @companyTradingAccountID
			
			Delete T_CompanyUserTradingAccounts
			Where TradingAccountID = @companyTradingAccountID
			
			Delete T_CompanyTradingAccounts
			Where CompanyTradingAccountsID = @companyTradingAccountID
		end



/****** Object:  Stored Procedure dbo.P_DeleteCompanyTradingAccounts    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyTradingAccounts
	(		
		@companyID int,
		@tradingAccountsID varchar(2000) = ''
	)
AS
	/*Declare @total int 
	Set @total = 0
	
	SELECT @total = Count(*)
	FROM T_CompanyTradingAccounts
	WHERE CompanyTradingAccountsID = @tradingAccountsID AND CompanyID = @companyID
	
	if(@total = 0)
	begin
		Delete T_CompanyTradingAccounts
		Where CompanyTradingAccountsID = @tradingAccountsID
		
		Delete T_CompanyUserTradingAccounts
		Where TradingAccountID = @tradingAccountsID
	end */
	
	if(@tradingAccountsID = '') 
	begin
		Delete T_CompanyTradingAccounts
			Where CompanyID = @companyID	
	end
	else
	begin
	
		exec ('Delete T_CompanyTradingAccounts
		Where convert(varchar, CompanyTradingAccountsID) NOT IN(' + @tradingAccountsID + ') AND CompanyID = ' + @companyID)
			
			--Where CompanyTradingAccountsID NOT IN(@tradingAccountsID) AND CompanyID = @companyID
		
		exec ( 'Delete T_CompanyUserTradingAccounts
			    Where convert(varchar, TradingAccountID) NOT IN( ' + @tradingAccountsID + ') And  CompanyUserID in (Select UserID from T_CompanyUSER Where CompanyID = ' + @companyID + ')')
			
	end




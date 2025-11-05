


/****** Object:  Stored Procedure dbo.P_SaveCompanyTradingAccount    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_SaveCompanyTradingAccount
(
		@companyTradingAccountsID int,
		@tradingAccountName varchar(50),
		@tradingShortName varchar(50),
		@companyID int
		
)
AS 
Declare @result int
Declare @total int 
Set @total = 0

Select @total = Count(*)
From T_CompanyTradingAccounts
Where CompanyTradingAccountsID = @companyTradingAccountsID

if(@total > 0)
begin	
	--Update CompanyTradingAccounts
	Update T_CompanyTradingAccounts 
	Set TradingAccountName = @tradingAccountName, 
		TradingShortName = @tradingShortName,
		CompanyID = @companyID
		
	Where CompanyTradingAccountsID = @companyTradingAccountsID
	
	Set @result = @companyTradingAccountsID
end
else
--Insert CompanyTradingAccounts
begin
	INSERT T_CompanyTradingAccounts(TradingAccountName, TradingShortName, CompanyID)
	Values(@tradingAccountName, @tradingShortName, @companyID)
	
	Set @result = scope_identity()
		--	end
end
select @result





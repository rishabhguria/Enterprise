



/****** Object:  Stored Procedure dbo.P_SaveTradingAccountsForUser    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE [dbo].[P_SaveTradingAccountsForUser]
	(
		@tradingAccountID int,
		@userID int
	)
AS

	Declare @result int
	Declare @total int 
	Set @total = 0
	
	Select @total = Count(*)
	From T_CompanyUserTradingAccounts
	Where CompanyUserID = @userID AND TradingAccountID = @tradingAccountID
	
	if(@total > 0)
	begin	
		--Update T_CompanyUserTradingAccounts
		Update T_CompanyUserTradingAccounts 
		Set CompanyUserID = @userID, 
			TradingAccountID = @tradingAccountID
			
		Where CompanyUserID = @userID AND TradingAccountID = @tradingAccountID
		
		Select @result = CompanyUserTradingAccountID From T_CompanyUserTradingAccounts Where CompanyUserID = @userID AND TradingAccountID = @tradingAccountID
	end
	else
	--Insert T_CompanyUserTradingAccounts
	begin
	
		Insert T_CompanyUserTradingAccounts(TradingAccountID, CompanyUserID)
		Values(@tradingAccountID, @userID)
		
		Set @result = scope_identity()
	end	
select @result




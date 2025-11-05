



CREATE   PROCEDURE dbo.P_GetCompanyClientTrAccByCompanyClientID

	(
		@CompanyClientID int
		
	)

AS
select CompanyClientTradingAccount ,CompanyTradingAccountID,
TradingAccountName,T_Temp.CompanyClientID,TraderID,CCTrader.ShortName from
(select CompanyClientTradingAccount,CCTrAcc.CompanyTradingAccountID,CompanyClientID,
CTrAcc.TradingAccountName,ClientTraderID from 
T_CompanyTradingAccounts as CTrAcc join T_CompanyClientTradingAccount as CCTrAcc
on CTrAcc.CompanyTradingAccountsID=CCTrAcc.CompanyTradingAccountID)
as T_Temp join
T_CompanyClientTrader as CCTrader
on  CCTrader.TraderID=T_Temp.ClientTraderID
 where T_Temp.CompanyClientID=@CompanyClientID






	
	 





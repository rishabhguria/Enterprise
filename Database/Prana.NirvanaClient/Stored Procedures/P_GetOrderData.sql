
-----------------------------------------------------------------------------------------

--Modified By : Rajat.
--Modified Date : 08 Jan 2007.
--Comment : Started taking data from V_TradedOrders. Original version is commented below

--Modified By : Rajat.
--Modified Date : 22 Jan 2007.
--Comment : Included @LastOrderSeqNumber so that trades could be fetched after the sequence no till which trades are already picked.

--Modified By : Rajat.
--Modified Date : 05 Feb 2007.
--Comment : Replaced getdate() with its UTC equivalent. Changed call to GETUTCDATE().

----------------------------------------------------------------------------------------

--@LastOrderSeqNumber is included 
CREATE PROCEDURE [dbo].[P_GetOrderData]( @UserID int,  @OrderBy varchar(100), @LastOrderSeqNumber bigint )

AS
EXEC('SELECT DISTINCT 
CLOrderID as ClOrderID, TradingAccountID as TradingAccountID, TradingAccountName AS TradingAccountName, 
AUECID, AssetID, AssetName, UnderLyingID, UnderLyingName, ExchangeID, ExchangeName,
CurrencyID, CurrencyName, CumQty as ExeQty, AvgPrice, Upper(Symbol) as Symbol, OrderSideName, 
OrderSidetagValue AS OrderSideID, UserID
FROM
V_TradedOrders
where AssetID=1 And Month(InsertionTime) = Month(GETUTCDATE()) AND Day(InsertionTime) = Day(GETUTCDATE()) 
AND Year(InsertionTime) = Year(GETUTCDATE()) And  UserID = ' + @UserID + 
'And NirvanaSeqNumber >' + @LastOrderSeqNumber + 'ORDER BY '+@OrderBy)

/*
'SELECT DISTINCT 
                      ORD.ClOrderID, ORD.TradingAccountID as TradingAccountID, dbo.T_CompanyTradingAccounts.TradingAccountName AS TradingAccountName, ORD.AUECID, ORD.AssetID, ORD.AssetName, 
                      ORD.UnderLyingID, ORD.UnderLyingName, ORD.ExchangeID, ORD.ExchangeName, ORD.CurrencyID, ORD.CurrencyName, FD.ExeQty, FD.AvgPrice, 
                      Upper(dbo.T_Sub.Symbol) as Symbol, dbo.T_Side.side AS OrderSideName, dbo.T_Sub.Side AS OrderSideID
FROM         dbo.V_OrderFillDetails FD INNER JOIN
                      dbo.V_OrderAUECDetails ORD ON FD.ParentID = ORD.ClOrderID INNER JOIN
                      dbo.T_Sub ON dbo.T_Sub.ParentClOrderID = ORD.ClOrderID INNER JOIN
                      dbo.T_Side ON dbo.T_Side.SideTagValue  = dbo.T_Sub.Side INNER JOIN
                      dbo.T_CompanyUserTradingAccounts ON ORD.TradingAccountID = dbo.T_CompanyUserTradingAccounts.TradingAccountID AND 
                      dbo.T_CompanyUserTradingAccounts.CompanyUserID = '+@UserID+' INNER JOIN
                      dbo.T_CompanyTradingAccounts ON dbo.T_CompanyTradingAccounts.CompanyTradingAccountsID = dbo.T_CompanyUserTradingAccounts.TradingAccountID
	   where ORD.AssetID=1 And Month(T_Sub.InsertionTime) = Month(GetDate()) AND Day(T_Sub.InsertionTime) = Day(GetDate()) AND Year(T_Sub.InsertionTime) = Year(GetDate()) 		          
		      ORDER BY '+@OrderBy)

*/

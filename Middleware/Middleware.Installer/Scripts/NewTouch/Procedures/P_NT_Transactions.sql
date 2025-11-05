
GO
/****** Object:  StoredProcedure [dbo].[P_NT_Transactions]    Script Date: 05/13/2015 16:36:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_Transactions '01/01/2014','09/07/2014'
Exec P_NT_Transactions '01/01/2014','08/07/2014','06/07/2014'
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_Transactions] 
-- Add the parameters for the stored procedure here
@StartDate datetime,
@EndDate datetime,
@AsOfDate datetime = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--SET NOCOUNT ON;
SET NOCOUNT OFF;
SET FMTONLY OFF;
-- Insert statements for procedure here
If @AsOfDate Is Null 
Begin 
Select CheckSumId,
AcctId,AcctName,RunDate,
Symbol,UnderlyingSymbol,Open_CloseTag,AvgPrice,Quantity,Side,TradeCurrency,NetAmountBase,Dividend,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
GroupID,
TransactionType
From T_NT_Transactions Where Rundate Between @StartDate And @EndDate
End 
Else 
Begin 
Select CheckSumId,
AcctId,AcctName,RunDate,
Symbol,UnderlyingSymbol,Open_CloseTag,AvgPrice,Quantity,Side,TradeCurrency,NetAmountBase,Dividend,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
GroupID,
TransactionType 
From T_NT_ApprovedTransactions Where Rundate Between @StartDate And @EndDate And AsOfDate = @AsOfDate
End 
END

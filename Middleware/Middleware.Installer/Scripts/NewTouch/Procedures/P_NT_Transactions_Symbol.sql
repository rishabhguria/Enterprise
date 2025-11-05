GO
/****** Object:  StoredProcedure [dbo].[P_NT_Transactions_Symbol]    Script Date: 03/10/2016 04:42:27 ******/
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
Create PROCEDURE [dbo].[P_NT_Transactions_Symbol] 
-- Add the parameters for the stored procedure here
@fundId varchar(max) =null,
@accountId varchar(max) =null,
@symbol varchar(100),
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
create table #accounts
(
AcctId int
)

If @FundId Is Null 
     begin
		insert INTO #accounts
		select CompanyFundID from T_CompanyFunds where StartDate is NOT null
    end

  else IF(@accountId is null)
	begin
		insert INTO #accounts
		select CompanyFundID from T_CompanyMasterFundSubAccountAssociation where CompanyMasterFundID =	@FundId	
    end
	else
	begin
       insert INTO #accounts
		select @accountId
  end

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
From T_NT_Transactions Where Rundate Between @StartDate And @EndDate and Symbol=@symbol and AcctId in (SELECT AcctId from #accounts) order by RunDate
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
From T_MW_ApprovedTransactions Where Rundate Between @StartDate And @EndDate And AsOfDate = @AsOfDate and Symbol=@symbol and AcctId in (SELECT AcctId from #accounts) order by RunDate
End 
drop TABLE #accounts
END


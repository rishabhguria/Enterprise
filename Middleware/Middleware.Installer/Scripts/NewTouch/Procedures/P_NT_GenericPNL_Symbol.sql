GO
/****** Object:  StoredProcedure [dbo].[P_NT_GenericPNL_Symbol]    Script Date: 03/10/2016 04:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_GenericPNL '01/01/2014','08/07/2014'
Exec P_NT_GenericPNL '01/01/2014','06/07/2014','06/07/2014'
*/
-- =============================================
Create PROCEDURE [dbo].[P_NT_GenericPNL_Symbol] 
-- Add the parameters for the stored procedure here
@FundId varchar(max) =NUll,
@accountId varchar(max) =null,
@Symbol varchar(max),
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

Select [CheckSumId]
      ,[AcctId]
      ,[AcctName]
      ,[RunDate]
      ,[Symbol]
      ,[UnderlyingSymbol]
      ,[TradeDate]
      ,[ClosingDate]
      ,[Open_CloseTag]
      ,[UdaSector]
      ,[UdaSubSector]
      ,[UdaCountry]
      ,[Strategy]
      ,[SymbolDescription]
      ,[UnderlyingSymbolDescription]
      ,[BloombergSymbol]
      ,[PutOrCall]
      ,[ExpirationDate]
      ,[StrikePrice]
      ,[Delta]
      ,[Beta]
      ,[ImpliedVol]
      ,[Asset]
      ,[SetupAsset]
      ,[CommissionAndFees]
      ,[FXPNL]
      ,[PriceMultiplier]
      ,[DeltaAdjPosMultiplier]
      ,[ZeroOrEndingMVOrUnrealized]
      ,[CouponRate]
      ,[BlackScholesOrBlack76]
      ,[Side]
      ,[TradeCurrency]
      ,[OpeningFXRate]
      ,[BeginningFXRate]
      ,[EndingFXRate]
      ,[TotalCostLocal]
      ,[BeginningMarketValueLocal]
      ,[EndingMarketValueLocal]
      ,[TotalOpenCommissionAndFeesLocal]
      ,[TotalClosedCommissionAndFeesLocal]
	  ,[DividendLocal]
      ,[TotalCostBase]
      ,[BeginningMarketValueBase]
      ,[EndingMarketValueBase]
      ,[TotalOpenCommissionAndFeesBase]
      ,[TotalClosedCommissionAndFeesBase]
	  ,[DividendBase]
      ,[BeginningQuantity]
      ,[EndingQuantity]
      ,[Quantity]
      ,[UnitCostLocal]
      ,[BeginningPriceLocal]
      ,[ClosingPriceLocal]
      ,[EndingPriceLocal]
      ,[UnderlyingSymbolPriceLocal]
      ,[UnitCostBase]
      ,[BeginningPriceBase]
      ,[ClosingPriceBase]
      ,[EndingPriceBase]
      ,[UnderlyingSymbolPriceBase]
      ,[SideMultiplier]
      ,[Multiplier]
      ,[UnderlyingDelta]
      ,[TaxlotID]
      ,[TaxlotClosingID]
      ,[DaySplitFactor]
      ,[TillSplitFactor]
      ,[K0]
      ,[K1]
      ,[K2]
      ,[AveDaystoLiquidate]
      ,[DaysToLiquidate]
      ,[AveNDaysTradingVolume]
      ,[AveNDaysTradingValueLocal]
      ,[AveNDaysTradingValueBase]
      ,[BeginningDelta]
      ,[BeginningUnderlyingSymbolPriceLocal]
      ,[BeginningUnderlyingSymbolPriceBase]
From T_NT_GenericPNL Where Rundate Between @StartDate And @EndDate and Symbol=@Symbol and AcctId in (SELECT AcctId from #accounts) order by RunDate
End 
Else 
Begin 
Select [CheckSumId]
      ,[AcctId]
      ,[AcctName]
      ,[RunDate]
      ,[Symbol]
      ,[UnderlyingSymbol]
      ,[TradeDate]
      ,[ClosingDate]
      ,[Open_CloseTag]
      ,[UdaSector]
      ,[UdaSubSector]
      ,[UdaCountry]
      ,[Strategy]
      ,[SymbolDescription]
      ,[UnderlyingSymbolDescription]
      ,[BloombergSymbol]
      ,[PutOrCall]
      ,[ExpirationDate]
      ,[StrikePrice]
      ,[Delta]
      ,[Beta]
      ,[ImpliedVol]
      ,[Asset]
      ,[SetupAsset]
      ,[CommissionAndFees]
      ,[FXPNL]
      ,[PriceMultiplier]
      ,[DeltaAdjPosMultiplier]
      ,[ZeroOrEndingMVOrUnrealized]
      ,[CouponRate]
      ,[BlackScholesOrBlack76]
      ,[Side]
      ,[TradeCurrency]
      ,[OpeningFXRate]
      ,[BeginningFXRate]
      ,[EndingFXRate]
      ,[TotalCostLocal]
      ,[BeginningMarketValueLocal]
      ,[EndingMarketValueLocal]
      ,[TotalOpenCommissionAndFeesLocal]
      ,[TotalClosedCommissionAndFeesLocal]
	  ,[DividendLocal]
      ,[TotalCostBase]
      ,[BeginningMarketValueBase]
      ,[EndingMarketValueBase]
      ,[TotalOpenCommissionAndFeesBase]
      ,[TotalClosedCommissionAndFeesBase]
	  ,[DividendBase]
      ,[BeginningQuantity]
      ,[EndingQuantity]
      ,[Quantity]
      ,[UnitCostLocal]
      ,[BeginningPriceLocal]
      ,[ClosingPriceLocal]
      ,[EndingPriceLocal]
      ,[UnderlyingSymbolPriceLocal]
      ,[UnitCostBase]
      ,[BeginningPriceBase]
      ,[ClosingPriceBase]
      ,[EndingPriceBase]
      ,[UnderlyingSymbolPriceBase]
      ,[SideMultiplier]
      ,[Multiplier]
      ,[UnderlyingDelta]
      ,[TaxlotID]
      ,[TaxlotClosingID]
      ,[DaySplitFactor]
      ,[TillSplitFactor]
      ,[K0]
      ,[K1]
      ,[K2]
      ,[AveDaystoLiquidate]
      ,[DaysToLiquidate]
      ,[AveNDaysTradingVolume]
      ,[AveNDaysTradingValueLocal]
      ,[AveNDaysTradingValueBase]
      ,[BeginningDelta]
      ,[BeginningUnderlyingSymbolPriceLocal]
      ,[BeginningUnderlyingSymbolPriceBase]
From T_NT_ApprovedGenericPNL Where Rundate Between @StartDate And @EndDate And AsOfDate = @AsOfDate and Symbol=@Symbol and AcctId in (SELECT AcctId from #accounts) order by RunDate
End 

drop TABLE #accounts
END


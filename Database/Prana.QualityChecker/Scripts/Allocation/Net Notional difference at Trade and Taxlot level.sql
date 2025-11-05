DECLARE @errormsg VARCHAR(MAX)
DECLARE @FundIds VARCHAR(MAX)
DECLARE @FromDate DATETIME
DECLARE @ToDate DATETIME

set @FromDate=''
set @ToDate=''
set @errormsg=''
SET @FundIds=''

Declare @VarNetNotionalDiff Float
Set @VarNetNotionalDiff = 1

Select 
CF.FundName,
VT.Symbol, 
VT.TransactionType,
S.Side, 
VT.AUECLocalDate As TradeDate, 
--Round(VT.AvgPrice,6) As Trade_AvgPrice,
VT.AvgPrice As Trade_AvgPrice,
VT.TaxlotQty As Trade_Quantity,  
--Round(PT.AvgPrice,6) As Taxlot_AvgPrice,
Isnull(VT.Commission,0)+ ISNULL(VT.SoftCommission,0) +  
Isnull(VT.OtherBrokerFees,0)+Isnull(VT.ClearingBrokerFee,0) +
Isnull(VT.StampDuty,0) + Isnull(VT.TransactionLevy ,0) + Isnull( VT.ClearingFee ,0) + 
Isnull( VT.TaxOnCommissions ,0) + Isnull( VT.MiscFees,0)+ Isnull( VT.SecFee,0)+ 
Isnull( VT.OccFee,0)+ Isnull( VT.OrfFee,0) As Trade_TotalCommAndFees_Local, 
PT.AvgPrice As Taxlot_AvgPrice,
PT.TaxlotOpenQty As Taxlot_Quantity, 
Isnull( PT.OpenTotalCommissionandFees,0) As Taxlot_TotalCommAndFees_Local, 
IsNull(VT.AvgPrice * VT.TaxLotQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(VT.OrderSideTagValue),0) as  Trade_NetNotional_Local,
IsNull(PT.AvgPrice * VT.TaxLotQty * IsNUll(SM.Multiplier,0) * dbo.GetSideMultiplier(VT.OrderSideTagValue),0) as  Taxlot_NetNotional_Local

INTO #TempAveragePriceDiff
From V_Taxlots VT
Inner Join PM_Taxlots PT On PT.TaxlotID = VT.TaxlotID And PT.TaxlotOpenQty = VT.TaxlotQty
Inner Join V_SecMasterData SM On SM.TickerSymbol = VT.Symbol
Inner Join T_Side S On S.SideTagValue = VT.OrderSideTagValue
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Where 
--Round(PT.AvgPrice,6) <> Round(VT.AvgPrice,6)
--And 
PT.TaxlotOpenQty > 0
And DATEDIFF(DAY,@FromDate,VT.AUECLocalDate) >= 0 AND DATEDIFF(DAY,@ToDate,VT.AUECLocalDate) <=0
And PT.TaxLotClosingId_Fk Is Null
And VT.TransactionType Not In ('Assignment', 'Exercise')
Order By VT.AUECLocalDate Desc

Update #TempAveragePriceDiff
Set Trade_NetNotional_Local = Trade_NetNotional_Local + Trade_TotalCommAndFees_Local,
    Taxlot_NetNotional_Local = Taxlot_NetNotional_Local + Taxlot_TotalCommAndFees_Local

Alter Table #TempAveragePriceDiff
Add NetNotionalDiff Float

Update #TempAveragePriceDiff
Set NetNotionalDiff = 0

Update #TempAveragePriceDiff
Set NetNotionalDiff = Abs(Trade_NetNotional_Local - Taxlot_NetNotional_Local)

IF EXISTS(SELECT * FROM #TempAveragePriceDiff Where NetNotionalDiff >= 1)
BEGIN
	SET @errormsg='Net Notional difference found at Trade and Taxlot level.'
	SELECT * From #TempAveragePriceDiff Where NetNotionalDiff >= 1
END

SELECT @errormsg AS ErrorMsg

DROP TABLE #TempAveragePriceDiff
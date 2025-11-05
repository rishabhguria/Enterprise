/*
exec [P_GetEOD_OPS_Meraki_KarstPeak] @thirdPartyID=46,@companyFundIDs=N'25,21,22,20,18,19,31,30,24,27,28,23,26,29',@inputDate='2020-06-16 06:33:30',@companyID=7,
@auecIDs=N'66,175,20,163,171,65,71,67,76,63,44,56,21,180,1,15,62,12,22,243,26,16,17',@TypeID=1,@dateType=0,@fileFormatID=118,@includeSent=0
*/

CREATE PROCEDURE [dbo].[P_GetEOD_OPS_Meraki_KarstPeak] 
(
	 @thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                      
	,@fileFormatID INT
	,@includeSent BIT = 1
	)
AS

 --Declare 
	--@thirdPartyID INT
	--,@companyFundIDs VARCHAR(max)
	--,@inputDate DATETIME
	--,@companyID INT
	--,@auecIDs VARCHAR(max)
	--,@TypeID INT
	--,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                      
	--,@fileFormatID INT
	--,@includeSent BIT

	--Set @thirdPartyID = 45
	--Set @companyFundIDs = N'25,21,22,20,18,19,31,30,24,27,28,23,26,29'
	--Set @inputDate = '2020-06-16 06:33:30'
	--Set @companyID = 7
	--Set @auecIDs=N'66,175,20,163,171,65,71,67,76,63,44,56,21,180,1,15,62,12,22,243,26,16,17'
	--Set @TypeID = 1
	--Set @dateType = 0
	--Set @fileFormatID = 118
	--Set @includeSent = 0



Declare @MasterFundName Varchar(200)
Set @MasterFundName = 'Karst Peak'

DECLARE @Fund TABLE (FundID INT)
DECLARE @AUECID TABLE (AUECID INT)
DECLARE @IncludeExpiredSettledTransaction INT
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT
DECLARE @IncludeCATransaction INT

SET @IncludeExpiredSettledTransaction = (
		SELECT IncludeExercisedAssignedTransaction
		FROM T_ThirdPartyFileFormat
		WHERE FileFormatId = @fileFormatID
		)
SET @IncludeExpiredSettledUnderlyingTransaction = (
		SELECT IncludeExercisedAssignedUnderlyingTransaction
		FROM T_ThirdPartyFileFormat
		WHERE FileFormatId = @fileFormatID
		)
SET @IncludeCATransaction = (
		SELECT IncludeCATransaction
		FROM T_ThirdPartyFileFormat
		WHERE FileFormatId = @fileFormatID
		)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

INSERT INTO @AUECID
SELECT Cast(Items AS INT)
FROM dbo.Split(@auecIDs, ',')

---- Excluding Future Asset Category: Group By Symbol, Side and Broker
Select 
Case 
	When Count(CF.FundName) > 1
	Then 'Mult_' + @MasterFundName
	Else Max(CF.FundName) 
End  As AccountName,
----CF.FundName  As AccountName,
S.Side,
Sum(VT.TaxLotQty) As AllocatedQty,
Sum(VT.AvgPrice * VT.TaxLotQty) As AveragePrice,
VT.Symbol,
Max(SM.CompanyName) As FullSecurityName,
Max(SM.BloombergSymbol) As BBCode,
A.AssetName As Asset,
Max(TradeCurr.CurrencySymbol) As CurrencySymbol,
Max(SettleCurr.CurrencySymbol) As SettlCurrency,
Max(VT.FXConversionMethodOperator_Taxlot) As FXConversionMethodOperator_Trade,
Max(VT.FXRate_Taxlot) As FXRate_Taxlot,
Max(Convert(Varchar,VT.AUECLocalDate,101)) As TradeDate,
Max(Convert(Varchar,VT.SettlementDate,101)) As SettlementDate,
Max(IsNull(CP.ShortName,'Undefined')) As CounterParty,
Max(VT.GroupRefID) As PBUniqueID,
Max(VT.Level1AllocationID) As EntityID,
Max(SM.Multiplier) As AssetMultiplier,
Sum(VT.CumQty) As ExecutedQty,
Sum(VT.Quantity) As TotalQty,
Sum(VT.Commission) As CommissionCharged,
Sum(VT.OtherBrokerFees) As OtherBrokerFee,
Sum(VT.StampDuty) As StampDuty,
Sum(VT.TransactionLevy) As TransactionLevy,
Sum(VT.ClearingFee) As ClearingFee,
Sum(VT.TaxOnCommissions) As TaxOnCommissions,
Sum(VT.MiscFees) As MiscFees,
Sum(VT.SecFee) As SecFee,
Sum(VT.OccFee) As OccFee,
Sum(VT.OrfFee) As OrfFee,
Sum(VT.ClearingBrokerFee) As ClearingBrokerFee,
Sum(VT.SoftCommission) As SoftCommissionCharged,
Max(VT.SideMultiplier) As SideMultiplier,
Sum(VT.TotalExpenses) As TotalExpenses,
Sum((IsNull(VT.AvgPrice * VT.TaxLotQty * SM.Multiplier ,0))) As GrossAmount,
Sum((IsNull((VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) + (VT.SideMultiplier * VT.TotalExpenses) ,0))) As NetAmount,
U.UnderLyingName,
1 As OrderByCol
InTo #TempEOD
From V_Taxlots VT With (NoLock)
Inner Join T_Side S On S.SideTagValue = VT.OrderSideTagValue 
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
--Inner Join @Fund F On F.FundID = VT.FundID
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = VT.FundID 
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = CF.CompanyFundID
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Currency TradeCurr On TradeCurr.CurrencyID = VT.CurrencyID
Inner Join T_Currency SettleCurr On SettleCurr.CurrencyID = VT.SettlCurrency_Taxlot
Inner Join T_Asset A On A.AssetID = VT.AssetID 
Inner Join T_UnderLying U On U.UnderLyingID = VT.UnderLyingID
Left Outer Join T_CounterParty CP On CP.CounterPartyID = VT.CounterPartyID  
WHERE 
MF.MasterFundName = @MasterFundName
And A.AssetName <> 'Future'
And Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) = 0
	AND (
			(
				VT.TransactionType IN ('Buy','BuytoClose','BuytoOpen','Sell','Sellshort','SelltoClose','SelltoOpen','LongAddition','LongWithdrawal','ShortAddition','ShortWithdrawal','')
				AND (VT.TransactionSource IN (0,1,2,3,4,14)
			)
		)
	OR (
			@IncludeExpiredSettledTransaction = 1
			AND VT.TransactionType IN ('Exercise','Expire','Assignment')
			AND VT.AssetID IN (2,4)
			)
		OR (
			@IncludeExpiredSettledTransaction = 1
			AND VT.TransactionType IN ('CSCost','CSZero','DLCost','CSClosingPx','Expire','DLCostAndPNL')
			AND VT.AssetID IN (3)
			)
		OR (
			@IncludeExpiredSettledUnderlyingTransaction = 1
			AND VT.TransactionType IN ('Exercise','Expire','Assignment')
			AND TaxlotClosingID_FK IS NOT NULL
			AND VT.AssetID IN (1,3))
		OR (
			@IncludeCATransaction = 1
			AND (VT.TransactionSource IN (6,7,8,9,10,11))
			)
		OR TransactionSource = 13
		)
Group By A.AssetName,VT.Symbol, S.Side, VT.CounterPartyID, U.UnderLyingName

---- For Future Asset Category: Group By Account, Symbol, Side and Broker
InSert InTo #TempEOD

Select 
--Case 
--	When Count(CF.FundName) > 1
--	Then 'Mult_' + @MasterFundName
--	Else Max(CF.FundName) 
--End  As AccountName,
CF.FundName  As AccountName,
S.Side,
(VT.TaxLotQty) As AllocatedQty,
(VT.AvgPrice * VT.TaxLotQty) As AveragePrice,
VT.Symbol,
(SM.CompanyName) As FullSecurityName,
(SM.BloombergSymbol) As BBCode,
A.AssetName As Asset,
(TradeCurr.CurrencySymbol) As CurrencySymbol,
(SettleCurr.CurrencySymbol) As SettlCurrency,
(VT.FXConversionMethodOperator_Taxlot) As FXConversionMethodOperator_Trade,
(VT.FXRate_Taxlot) As FXRate_Taxlot,
(Convert(Varchar,VT.AUECLocalDate,101)) As TradeDate,
(Convert(Varchar,VT.SettlementDate,101)) As SettlementDate,
(IsNull(CP.ShortName,'Undefined')) As CounterParty,
(VT.GroupRefID) As PBUniqueID,
(VT.Level1AllocationID) As EntityID,
(SM.Multiplier) As AssetMultiplier,
(VT.CumQty) As ExecutedQty,
(VT.Quantity) As TotalQty,
(VT.Commission) As CommissionCharged,
(VT.OtherBrokerFees) As OtherBrokerFee,
(VT.StampDuty) As StampDuty,
(VT.TransactionLevy) As TransactionLevy,
(VT.ClearingFee) As ClearingFee,
(VT.TaxOnCommissions) As TaxOnCommissions,
(VT.MiscFees) As MiscFees,
(VT.SecFee) As SecFee,
(VT.OccFee) As OccFee,
(VT.OrfFee) As OrfFee,
(VT.ClearingBrokerFee) As ClearingBrokerFee,
(VT.SoftCommission) As SoftCommissionCharged,
(VT.SideMultiplier) As SideMultiplier,
(VT.TotalExpenses) As TotalExpenses,
((IsNull(VT.AvgPrice * VT.TaxLotQty * SM.Multiplier ,0))) As GrossAmount,
((IsNull((VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) + (VT.SideMultiplier * VT.TotalExpenses) ,0))) As NetAmount,
U.UnderLyingName,
2 As OrderByCol

From V_Taxlots VT With (NoLock)
Inner Join T_Side S On S.SideTagValue = VT.OrderSideTagValue 
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
----Inner Join @Fund F On F.FundID = VT.FundID
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = VT.FundID 
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = CF.CompanyFundID
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Currency TradeCurr On TradeCurr.CurrencyID = VT.CurrencyID
Inner Join T_Currency SettleCurr On SettleCurr.CurrencyID = VT.SettlCurrency_Taxlot
Inner Join T_Asset A On A.AssetID = VT.AssetID 
Inner Join T_UnderLying U On U.UnderLyingID = VT.UnderLyingID
Left Outer Join T_CounterParty CP On CP.CounterPartyID = VT.CounterPartyID  
WHERE 
MF.MasterFundName = @MasterFundName
And A.AssetName = 'Future'
And Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) = 0
	AND (
			(
				VT.TransactionType IN ('Buy','BuytoClose','BuytoOpen','Sell','Sellshort','SelltoClose','SelltoOpen','LongAddition','LongWithdrawal','ShortAddition','ShortWithdrawal','')
				AND (VT.TransactionSource IN (0,1,2,3,4,14)
			)
		)
	OR (
			@IncludeExpiredSettledTransaction = 1
			AND VT.TransactionType IN ('Exercise','Expire','Assignment')
			AND VT.AssetID IN (2,4)
			)
		OR (
			@IncludeExpiredSettledTransaction = 1
			AND VT.TransactionType IN ('CSCost','CSZero','DLCost','CSClosingPx','Expire','DLCostAndPNL')
			AND VT.AssetID IN (3)
			)
		OR (
			@IncludeExpiredSettledUnderlyingTransaction = 1
			AND VT.TransactionType IN ('Exercise','Expire','Assignment')
			AND TaxlotClosingID_FK IS NOT NULL
			AND VT.AssetID IN (1,3))
		OR (
			@IncludeCATransaction = 1
			AND (VT.TransactionSource IN (6,7,8,9,10,11))
			)
		OR TransactionSource = 13
		)
--Group By CF.FundName, A.AssetName,VT.Symbol, S.Side, VT.CounterPartyID

---- UnAllocated trades, excluding Future: Group By Symbol, Side and Broker
Insert InTo #TempEOD

Select 
Case 
	When Count(MF.MasterFundName) > 1
	Then 'Mult_' + @MasterFundName
	Else Max(MF.MasterFundName) 
End  As AccountName,
----MF.MasterFundName  As AccountName,
S.Side,
Sum(G.CumQty) As AllocatedQty,
Sum(G.AvgPrice * G.CumQty) As AveragePrice,
G.Symbol,
Max(SM.CompanyName) As FullSecurityName,
Max(SM.BloombergSymbol) As BBCode,
A.AssetName As Asset,
Max(TradeCurr.CurrencySymbol) As CurrencySymbol,
Max(SettleCurr.CurrencySymbol) As SettlCurrency,
Max(G.FXConversionMethodOperator) As FXConversionMethodOperator_Trade,
Max(G.FXRate) As FXRate_Taxlot,
Max(Convert(Varchar,G.AUECLocalDate,101)) As TradeDate,
Max(Convert(Varchar,G.SettlementDate,101)) As SettlementDate,
Max(IsNull(CP.ShortName,'Undefined')) As CounterParty,
Max(G.GroupRefID) As PBUniqueID,
Max(G.GroupID) As EntityID,
Max(SM.Multiplier) As AssetMultiplier,
Sum(G.CumQty) As ExecutedQty,
Sum(G.Quantity) As TotalQty,
Sum(G.Commission) As CommissionCharged,
Sum(G.OtherBrokerFees) As OtherBrokerFee,
Sum(G.StampDuty) As StampDuty,
Sum(G.TransactionLevy) As TransactionLevy,
Sum(G.ClearingFee) As ClearingFee,
Sum(G.TaxOnCommissions) As TaxOnCommissions,
Sum(G.MiscFees) As MiscFees,
Sum(G.SecFee) As SecFee,
Sum(G.OccFee) As OccFee,
Sum(G.OrfFee) As OrfFee,
Sum(G.ClearingBrokerFee) As ClearingBrokerFee,
Sum(G.SoftCommission) As SoftCommissionCharged,
Max([dbo].GetSideMultiplier(G.OrderSideTagValue)) As SideMultiplier,
Sum(G.Commission + G.OtherBrokerFees + G.StampDuty + G.TransactionLevy + G.ClearingFee + G.TaxOnCommissions + G.MiscFees + G.SecFee + G.OccFee + G.OrfFee + G.ClearingBrokerFee + G.SoftCommission + G.OptionPremiumAdjustment) AS TotalExpenses,
Sum((IsNull(G.AvgPrice * G.CumQty * SM.Multiplier ,0))) As GrossAmount,
Sum((IsNull((G.AvgPrice * G.CumQty * SM.Multiplier) +
 ([dbo].GetSideMultiplier(G.OrderSideTagValue) * G.Commission + G.OtherBrokerFees + G.StampDuty + G.TransactionLevy + G.ClearingFee +
 G.TaxOnCommissions + G.MiscFees + G.SecFee + G.OccFee + G.OrfFee + G.ClearingBrokerFee + G.SoftCommission + G.OptionPremiumAdjustment) ,0))) As NetAmount,
 U.UnderLyingName,
 3 As OrderByCol

From T_Group G With (NoLock)
Inner Join T_Side S On S.SideTagValue = G.OrderSideTagValue 
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = G.Symbol 
Inner Join T_Currency TradeCurr On TradeCurr.CurrencyID = G.CurrencyID
Inner Join T_Currency SettleCurr On SettleCurr.CurrencyID = G.SettlCurrency
Inner Join T_Asset A On A.AssetID = G.AssetID 
Inner Join T_CompanyMasterFunds MF On MF.MasterFundName = G.TradeAttribute6
Inner Join T_UnderLying U On U.UnderLyingID = G.UnderLyingID
--Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyMasterFundID = MF.CompanyMasterFundID
--Inner Join T_CompanyFunds CF On CF.CompanyFundID = MFA.CompanyFundID
Left Outer Join T_CounterParty CP On CP.CounterPartyID = G.CounterPartyID
Where 
MF.MasterFundName = @MasterFundName
And A.AssetName <> 'Future'
And G.StateID = 1
And G.CumQty > 0
And (TradeAttribute6 Is Not Null And TradeAttribute6 <> '')
And Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
				THEN G.AUECLocalDate
				ELSE G.ProcessDate
			END
			), @inputdate) = 0

Group By A.AssetName,G.Symbol, S.Side, G.CounterPartyID, U.UnderLyingName

---- UnAllocated trades, Only Future: Group By Master fund, Symbol, Side and Broker
Insert InTo #TempEOD

Select 
MF.MasterFundName  As AccountName,
S.Side,
(G.CumQty) As AllocatedQty,
(G.AvgPrice * G.CumQty) As AveragePrice,
G.Symbol,
(SM.CompanyName) As FullSecurityName,
(SM.BloombergSymbol) As BBCode,
A.AssetName As Asset,
(TradeCurr.CurrencySymbol) As CurrencySymbol,
(SettleCurr.CurrencySymbol) As SettlCurrency,
(G.FXConversionMethodOperator) As FXConversionMethodOperator_Trade,
(G.FXRate) As FXRate_Taxlot,
(Convert(Varchar,G.AUECLocalDate,101)) As TradeDate,
(Convert(Varchar,G.SettlementDate,101)) As SettlementDate,
(IsNull(CP.ShortName,'Undefined')) As CounterParty,
(G.GroupRefID) As PBUniqueID,
(G.GroupID) As EntityID,
(SM.Multiplier) As AssetMultiplier,
(G.CumQty) As ExecutedQty,
(G.Quantity) As TotalQty,
(G.Commission) As CommissionCharged,
(G.OtherBrokerFees) As OtherBrokerFee,
(G.StampDuty) As StampDuty,
(G.TransactionLevy) As TransactionLevy,
(G.ClearingFee) As ClearingFee,
(G.TaxOnCommissions) As TaxOnCommissions,
(G.MiscFees) As MiscFees,
(G.SecFee) As SecFee,
(G.OccFee) As OccFee,
(G.OrfFee) As OrfFee,
(G.ClearingBrokerFee) As ClearingBrokerFee,
(G.SoftCommission) As SoftCommissionCharged,
([dbo].GetSideMultiplier(G.OrderSideTagValue)) As SideMultiplier,
(G.Commission + G.OtherBrokerFees + G.StampDuty + G.TransactionLevy + G.ClearingFee + G.TaxOnCommissions + G.MiscFees + G.SecFee + G.OccFee + G.OrfFee + G.ClearingBrokerFee + G.SoftCommission + G.OptionPremiumAdjustment) AS TotalExpenses,
((IsNull(G.AvgPrice * G.CumQty * SM.Multiplier ,0))) As GrossAmount,
((IsNull((G.AvgPrice * G.CumQty * SM.Multiplier) +
 ([dbo].GetSideMultiplier(G.OrderSideTagValue) * G.Commission + G.OtherBrokerFees + G.StampDuty + G.TransactionLevy + G.ClearingFee +
 G.TaxOnCommissions + G.MiscFees + G.SecFee + G.OccFee + G.OrfFee + G.ClearingBrokerFee + G.SoftCommission + G.OptionPremiumAdjustment) ,0))) As NetAmount,
 U.UnderLyingName,
4 As OrderByCol

From T_Group G With (NoLock)
Inner Join T_Side S On S.SideTagValue = G.OrderSideTagValue 
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = G.Symbol 
Inner Join T_Currency TradeCurr On TradeCurr.CurrencyID = G.CurrencyID
Inner Join T_Currency SettleCurr On SettleCurr.CurrencyID = G.SettlCurrency
Inner Join T_Asset A On A.AssetID = G.AssetID 
Inner Join T_CompanyMasterFunds MF On MF.MasterFundName = G.TradeAttribute6
Inner Join T_UnderLying U On U.UnderLyingID = G.UnderLyingID
Left Outer Join T_CounterParty CP On CP.CounterPartyID = G.CounterPartyID
Where 
MF.MasterFundName = @MasterFundName
And A.AssetName = 'Future'
And G.StateID = 1
And G.CumQty > 0
And (TradeAttribute6 Is Not Null And TradeAttribute6 <> '')
And Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
				THEN G.AUECLocalDate
				ELSE G.ProcessDate
			END
			), @inputdate) = 0

----Group By MF.MasterFundName, A.AssetName,G.Symbol, S.Side, G.CounterPartyID


Update #TempEOD
Set AveragePrice = AveragePrice/AllocatedQty

Select * From #TempEOD
Order By OrderByCol, AccountName, Symbol

Drop Table #TempEOD
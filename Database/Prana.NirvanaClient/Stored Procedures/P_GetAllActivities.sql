
/*************************************************    
Author : Bharat Raturi  
Creation date : Oct 08, 2014    
Descritpion : Get the overriding activities for exception  
Execution Method :     
exec P_GetAllActivities @fromdate=N'8/8/2014 12:00:00 AM', @todate=N'10/8/2014 12:00:00 AM', @fundIDs=N'1184,1185,1190,1183,1182', @activitydatetype=N'Settlement Date'  
*************************************************/
CREATE PROC [dbo].[P_GetAllActivities] (
	@fromdate DATETIME
	,@todate DATETIME
	,@fundIDs VARCHAR(max)
	,@activitydatetype VARCHAR(max)
	,@isActivitySource  bit
	)
AS
CREATE TABLE #funds (fundID INT)

INSERT INTO #funds
SELECT Items
FROM dbo.Split(@fundIDs, ',')

-------------------------------- 
IF (@isActivitySource =0)
begin     
IF (@activitydatetype <> 'All')
BEGIN
	SELECT act.UniqueKey
		,ActivityID
		,ActivityTypeId_FK
		,FKID
		,BalanceType
		,Symbol
		,act.FundID
		,TradeDate
		,SettlementDate
		,CurrencyID
		,LeadCurrencyID
		,VsCurrencyID
		,ClosedQty
		,STR(Amount, 28, 12) AS Amount
		,STR(PnL, 28, 12) AS PnL
		,STR(FXPnL, 28, 12) AS FXPnL
		,STR(Commission, 28, 12) AS Commission
		,STR(SoftCommission, 28, 12) AS SoftCommission
		,STR(OtherBrokerFees, 28, 12) AS OtherBrokerFees
		,STR(ClearingBrokerFee, 28, 12) AS ClearingBrokerFee
		,STR(StampDuty, 28, 12) AS StampDuty
		,STR(TransactionLevy, 28, 12) AS TransactionLevy
		,STR(ClearingFee, 28, 12) AS ClearingFee
		,STR(TaxOnCommissions, 28, 12) AS TaxOnCommissions
		,STR(MiscFees, 28, 12) AS MiscFees
		,STR(SecFee, 28, 12) AS SecFee
		,STR(OccFee, 28, 12) AS OccFee
		,STR(OrfFee, 28, 12) AS OrfFee
		,FXRate
		,FXConversionMethodOperator
		,ActivitySource
		,TransactionSource
		,ActivityNumber
		,Description
		,Subactivity
		,SideMultiplier
		,SettlCurrency
		,OptionPremiumAdjustment
		,ModifyDate
		,EntryDate
		,UserId
	FROM T_AllActivity act
	INNER JOIN T_CashPreferences tcpref ON act.FundID = tcpref.FundID
	INNER JOIN #funds ON act.FundID = #funds.fundID
	--where DATEDIFF(d,act.TradeDate,tcpref.CashMgmtStartDate)<=0    Commented for the activity having only payout entries(Exdate before and payout after CashmgmtStartDate)      
	WHERE (
			DATEDIFF(d, act.TradeDate, tcpref.CashMgmtStartDate) <= 0
			OR (
				DATEDIFF(d, act.SettlementDate, tcpref.CashMgmtStartDate) <= 0
				AND ActivitySource = 2
				)
				)
		AND DATEDIFF(d, CASE 
				WHEN @activitydatetype LIKE 'Trade Date'
					THEN act.TradeDate
				ELSE act.SettlementDate
				END, @fromdate) <= 0
		AND DATEDIFF(d, CASE 
				WHEN @activitydatetype LIKE 'Trade Date'
					THEN act.TradeDate
				ELSE act.SettlementDate
				END, @todate) >= 0
	ORDER BY act.TradeDate
		,act.SettlementDate
END
ELSE
BEGIN
	SELECT act.UniqueKey
		,ActivityID
		,ActivityTypeId_FK
		,FKID
		,BalanceType
		,Symbol
		,act.FundID
		,TradeDate
		,SettlementDate
		,CurrencyID
		,LeadCurrencyID
		,VsCurrencyID
		,ClosedQty
		,STR(Amount, 28, 12) AS Amount
		,STR(PnL, 28, 12) AS PnL
		,STR(PnL, 28, 12) AS FXPnL
		,STR(Commission, 28, 12) AS Commission
		,STR(SoftCommission, 28, 12) AS SoftCommission
		,STR(OtherBrokerFees, 28, 12) AS OtherBrokerFees
		,STR(ClearingBrokerFee, 28, 12) AS ClearingBrokerFee
		,STR(StampDuty, 28, 12) AS StampDuty
		,STR(TransactionLevy, 28, 12) AS TransactionLevy
		,STR(ClearingFee, 28, 12) AS ClearingFee
		,STR(TaxOnCommissions, 28, 12) AS TaxOnCommissions
		,STR(MiscFees, 28, 12) AS MiscFees
		,STR(SecFee, 28, 12) AS SecFee
		,STR(OccFee, 28, 12) AS OccFee
		,STR(OrfFee, 28, 12) AS OrfFee
		,FXRate
		,FXConversionMethodOperator
		,ActivitySource
		,TransactionSource
		,ActivityNumber
		,Description
		,Subactivity
		,SideMultiplier
		,SettlCurrency
		,OptionPremiumAdjustment
		,ModifyDate
		,EntryDate	
		,UserId
	FROM T_AllActivity act
	INNER JOIN T_CashPreferences tcpref ON act.FundID = tcpref.FundID
	INNER JOIN #funds ON act.FundID = #funds.fundID
	--where DATEDIFF(d,act.TradeDate,tcpref.CashMgmtStartDate)<=0    Commented for the activity having only payout entries(Exdate before and payout after CashmgmtStartDate)      
	WHERE (
			DATEDIFF(d, act.TradeDate, tcpref.CashMgmtStartDate) <= 0
			OR (
				DATEDIFF(d, act.SettlementDate, tcpref.CashMgmtStartDate) <= 0
				AND ActivitySource = 2
				)
			)
		AND (
			(
				DATEDIFF(d, act.TradeDate, @fromdate) <= 0
				AND DATEDIFF(d, act.TradeDate, @todate) >= 0
				)
			OR (
				(
					DATEDIFF(d, act.SettlementDate, @fromdate) <= 0
					AND DATEDIFF(d, act.SettlementDate, @todate) >= 0
					)
				)
			)
	ORDER BY act.TradeDate
		,act.SettlementDate
END
end
else
begin
IF (@activitydatetype <> 'All')
BEGIN
	SELECT act.UniqueKey
		,ActivityID
		,ActivityTypeId_FK
		,FKID
		,BalanceType
		,Symbol
		,act.FundID
		,TradeDate
		,SettlementDate
		,CurrencyID
		,LeadCurrencyID
		,VsCurrencyID
		,ClosedQty
		,STR(Amount, 28, 12) AS Amount
		,STR(PnL, 28, 12) AS PnL
		,STR(FXPnL, 28, 12) AS FXPnL
		,STR(Commission, 28, 12) AS Commission
		,STR(SoftCommission, 28, 12) AS SoftCommission
		,STR(OtherBrokerFees, 28, 12) AS OtherBrokerFees
		,STR(ClearingBrokerFee, 28, 12) AS ClearingBrokerFee
		,STR(StampDuty, 28, 12) AS StampDuty
		,STR(TransactionLevy, 28, 12) AS TransactionLevy
		,STR(ClearingFee, 28, 12) AS ClearingFee
		,STR(TaxOnCommissions, 28, 12) AS TaxOnCommissions
		,STR(MiscFees, 28, 12) AS MiscFees
		,STR(SecFee, 28, 12) AS SecFee
		,STR(OccFee, 28, 12) AS OccFee
		,STR(OrfFee, 28, 12) AS OrfFee
		,FXRate
		,FXConversionMethodOperator
		,ActivitySource
		,TransactionSource
		,ActivityNumber
		,Description
		,Subactivity
		,SideMultiplier
		,SettlCurrency
		,OptionPremiumAdjustment
		,ModifyDate
		,EntryDate
		,UserId
	FROM T_AllActivity act
	INNER JOIN T_CashPreferences tcpref ON act.FundID = tcpref.FundID
	INNER JOIN #funds ON act.FundID = #funds.fundID
	--where DATEDIFF(d,act.TradeDate,tcpref.CashMgmtStartDate)<=0    Commented for the activity having only payout entries(Exdate before and payout after CashmgmtStartDate)      
	WHERE
	   DATEDIFF(d, CASE 
				WHEN @activitydatetype LIKE 'Trade Date'
					THEN act.TradeDate
				ELSE act.SettlementDate
				END, @fromdate) <= 0
		AND DATEDIFF(d, CASE 
				WHEN @activitydatetype LIKE 'Trade Date'
					THEN act.TradeDate
				ELSE act.SettlementDate
				END, @todate) >= 0
	ORDER BY act.TradeDate
		,act.SettlementDate
END
ELSE
BEGIN
	SELECT act.UniqueKey
		,ActivityID
		,ActivityTypeId_FK
		,FKID
		,BalanceType
		,Symbol
		,act.FundID
		,TradeDate
		,SettlementDate
		,CurrencyID
		,LeadCurrencyID
		,VsCurrencyID
		,ClosedQty
		,STR(Amount, 28, 12) AS Amount
		,STR(PnL, 28, 12) AS PnL
		,STR(PnL, 28, 12) AS FXPnL
		,STR(Commission, 28, 12) AS Commission
		,STR(SoftCommission, 28, 12) AS SoftCommission
		,STR(OtherBrokerFees, 28, 12) AS OtherBrokerFees
		,STR(ClearingBrokerFee, 28, 12) AS ClearingBrokerFee
		,STR(StampDuty, 28, 12) AS StampDuty
		,STR(TransactionLevy, 28, 12) AS TransactionLevy
		,STR(ClearingFee, 28, 12) AS ClearingFee
		,STR(TaxOnCommissions, 28, 12) AS TaxOnCommissions
		,STR(MiscFees, 28, 12) AS MiscFees
		,STR(SecFee, 28, 12) AS SecFee
		,STR(OccFee, 28, 12) AS OccFee
		,STR(OrfFee, 28, 12) AS OrfFee
		,FXRate
		,FXConversionMethodOperator
		,ActivitySource
		,TransactionSource
		,ActivityNumber
		,Description
		,Subactivity
		,SideMultiplier
		,SettlCurrency
		,OptionPremiumAdjustment
		,ModifyDate
		,EntryDate	
		,UserId
	FROM T_AllActivity act
	INNER JOIN T_CashPreferences tcpref ON act.FundID = tcpref.FundID
	INNER JOIN #funds ON act.FundID = #funds.fundID
	--where DATEDIFF(d,act.TradeDate,tcpref.CashMgmtStartDate)<=0    Commented for the activity having only payout entries(Exdate before and payout after CashmgmtStartDate)      
	WHERE 
		(
			(
				DATEDIFF(d, act.TradeDate, @fromdate) <= 0
				AND DATEDIFF(d, act.TradeDate, @todate) >= 0
				)
			OR (
				(
					DATEDIFF(d, act.SettlementDate, @fromdate) <= 0
					AND DATEDIFF(d, act.SettlementDate, @todate) >= 0
					)
				)
			)
	ORDER BY act.TradeDate
		,act.SettlementDate
END
end
drop table #funds
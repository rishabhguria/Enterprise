CREATE Proc [dbo].[P_GetOpenPositionsForADate_Fithian]
(
@Date DateTime
)
As
Set NoCount On
--Declare @Date DateTime
--Set @Date = '2023-03-04'

Declare @Fund Table                                                         
(              
FundID int                    
)                        
               
Insert into @Fund                                                                                                  
Select CompanyFundID From T_CompanyFunds 
Where CompanyFundID In (30,31,32)
                                           

-- get Mark Price for End Date              
CREATE TABLE #MarkPriceForEndDate   
(    
  Finalmarkprice FLOAT    
 ,Symbol VARCHAR(200)    
 ,FundID INT    
 )   
  
INSERT INTO #MarkPriceForEndDate   
(    
 FinalMarkPrice    
 ,Symbol    
 ,FundID    
 )    
SELECT   
  DMP.FinalMarkPrice    
 ,DMP.Symbol    
 ,DMP.FundID    
FROM PM_DayMarkPrice DMP  With (NoLOck)
WHERE DateDiff(Day,DMP.DATE,@Date) = 0 --And DMP.FundID = 0 

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE FundID = 0

Delete From #MarkPriceForEndDate
Where FundID = 0 

Select 
PT.Symbol, 
CF.FundName As Account, 
Side.Side,
Case 
when Side='Sell short' or Side='Sell'
then 
PT.TaxLotOpenQty * -1
Else pt.TaxLotOpenQty 
end AS Quantity,
(PT.AvgPrice* PT.TaxLotOpenQty + PT.OpenTotalCommissionandFees) / PT.TaxLotOpenQty As CostBasis, 
PT.FXRate,
Convert(Varchar(10),G.AUECLocalDate,101) As TradeDate, 
CS.StrategyName As Strategy, 
Case 
	When A.AssetName = 'Equity' And G.IsSwapped = 1
	Then 'EquitySwap'
	Else A.AssetName 
End As AsetClass, 
Cast(PT.OpenTotalCommissionandFees As Decimal(18,10)) As TotalCommissionandFees,
IsNull(MPEndDate.Val, 0) As MarkPrice,
IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) As MarketValue

From PM_Taxlots PT With (NoLOck)
Inner Join @Fund FUND On FUND.FundID = PT.FundID 
Inner Join T_CompanyFunds CF With (NoLOck) On CF.CompanyFundID = PT.FundID 
Inner Join T_Group G With (NoLOck) On G.GroupID = PT.GroupID
Inner Join T_Side Side With (NoLOck) On Side.SideTagValue = PT.OrderSideTagValue
Inner Join T_CompanyStrategy CS With (NoLOck) On CS.CompanyStrategyID = PT.Level2ID
Inner Join T_Asset A With (NoLOck) On A.AssetID = G.AssetID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol 
LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
		MPE.Symbol = PT.Symbol AND MPE.FundID = PT.FundID )
LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
		PT.Symbol = MPZeroEndDate.Symbol AND MPZeroEndDate.FundID = 0)
CROSS APPLY (
	SELECT CASE 
			WHEN MPE.FinalMarkPrice IS NULL
				THEN CASE 
						WHEN MPZeroEndDate.FinalMarkPrice IS NULL
						THEN 0
						ELSE MPZeroEndDate.FinalMarkPrice
					END
			ELSE MPE.Finalmarkprice
			END
	) AS MPEndDate(Val)
Where PT.TaxLot_PK In
(
Select Max(Taxlot_PK) From PM_Taxlots With (NoLOck)
Where DateDiff(Day,AUECModifiedDate, @Date) >= 0
Group By TaxlotID
)
And PT.TaxLotOpenQty > 0
Order By Account, Symbol, Side 

Drop Table #MarkPriceForEndDate, #ZeroFundMarkPriceEndDate
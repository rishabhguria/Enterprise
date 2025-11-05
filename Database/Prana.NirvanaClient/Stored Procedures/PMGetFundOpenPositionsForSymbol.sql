/*                                                  
-- Usage :                                       
PMGetFundOpenPositionsForSymbol '2011-03-22', 'MXXX'          
                                                  
-- Author : Dileep                                                  
-- Description : It provides data to corp action module for applying the corporate action                                          
--------------------------------------------------------------------                                      
-- Modified By : Rajat  Dated : 23 Oct 08                                      
-- Description : REmoved condition PT.Symbol = @symbol from outside and put in inner query  to improve the performance.            
      
--Modified By :Ishant Kathuria  Date:20th dec 2011                                   
-- Description : Returns  UserID as well        
Modified By Sandeep Singh    
Date: Dec 19, 2013    
Desc: Attribute 1-6, LotID, ExternalTransID and AssetMultiplier columns added and fundID filter added i.e. @commaSeparatedFundIds   

Modified By: Sandeep Singh    
Date: NOV 23,2014    
Desc: TransactionType field added    

Modified By: Sandeep Singh    
Date: DEC 2,2019    
Desc: Notional Local and Base columns added    
*/                                      
                                      
CREATE PROCEDURE [dbo].[PMGetFundOpenPositionsForSymbol]                                                          
(                                                                                                                                                                                
@SelectedDate datetime,                                                            
@Symbol varchar(100),  
@commaSeparatedFundIds varchar(2000)                                                                       
)                                                                                                                              
As                                                                                                                                  
Begin                                                                                                                  
--Declare @SelectedDate Datetime                                                              
--Declare @Symbol varchar(50)           
--Declare @commaSeparatedFundIds varchar(50)  
  
--Set @selectedDate = '2019-12-03'      
--Set @Symbol = 'GOOG'--'AGFSW'--'AGFB-EEB'
--Set @commaSeparatedFundIds =''         
                                                                                         
DECLARE @DefaultAUECID INT		
DECLARE @MinTradeDate DATETIME
  
Create Table #Funds                                           
(                                          
 FundID int,
 LocalCurrencyId Int                                        
)                                                          
If (@commaSeparatedFundIds is null or @commaSeparatedFundIds = '')                                          
Begin                                                          
 Insert into #Funds                                                          
	 Select 
	 CompanyFundID as FundID,
	 LocalCurrency As LocalCurrencyId
	 From T_CompanyFunds  
	 Where IsActive=1                                             
End                                          
Else                                                          
 Insert into #Funds                                                          
	 Select 
	 CompanyFundID as FundID,
	 LocalCurrency As LocalCurrencyId
	 From T_CompanyFunds    
	 Where CompanyFundID In (Select Items as FundID from dbo.Split(@commaSeparatedFundIds,','))
  
Create Table #SecMasterTable  
(  
TickerSymbol Varchar(200),  
Multiplier Float, 
CompanyName Varchar(200),
CurrencyID Int
)  
  
Insert InTo #SecMasterTable  
Select  
TickerSymbol,  
Multiplier,
CompanyName,
CurrencyID
From V_SecMasterData  
Where TickerSymbol = @Symbol    
  
SET @DefaultAUECID = (
		SELECT TOP 1 DefaultAUECID
		FROM T_Company
		WHERE CompanyID <> - 1
		)

SET @MinTradeDate = (SELECT Min(G.AUECLocalDate)
						FROM PM_Taxlots PT
						INNER JOIN #Funds ON #Funds.FundID = PT.FundID  
						INNER JOIN T_Group G ON PT.GroupID = G.GroupID
						WHERE Taxlot_PK IN (
								SELECT Max(IPT.Taxlot_PK)
								FROM PM_Taxlots IPT
								WHERE IPT.Symbol = @Symbol And DateDiff(DAY, IPT.AUECModifiedDate, datediff(d, - 1, @SelectedDate)) > 0
								GROUP BY IPT.TaxlotID
								)
						AND PT.TaxlotOpenQty > 0						
					)    
                                                                                         
SET @MinTradeDate = dbo.AdjustBusinessDays(@MinTradeDate, - 1, @DefaultAUECID)

-- get forex rates for 2 date ranges                    
CREATE TABLE #FXConversionRates 
(
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,FundID INT
)
Declare @CurrencyID Int
Set @CurrencyID = (Select CurrencyId From #SecMasterTable)

If(@CurrencyID <> 1)
Begin
--insert FX Rate for date ranges in the temp table                    
INSERT INTO #FXConversionRates
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @MinTradeDate
	,@SelectedDate
End
--TODO: Find the reason why fxrates are deleted where base currency id is equal to ToCurrencyID                    
--Delete From #FXConversionRates Where ToCurrencyID <> @BaseCurrencyID                    
-- Adjusting FxRates based on the conversion method....                    
UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
	AND ConversionMethod = 1

-- For Fund Zero              
SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE fundID = 0
  
                          
Select max(Taxlot_PK) as Taxlot_PK  into #TempTaxlot_PK              
from               
(              
 Select max(Taxlot_PK) as Taxlot_PK, Taxlotid as Taxlotid from PM_Taxlots               
 where  Symbol = @Symbol and DateDiff(d, AUECModifiedDate,@selectedDate) > 0                   
 group by taxlotid              
              
 Union              
           
 Select max(PM_Taxlots.taxlot_PK) as Taxlot_PK, PM_Taxlots.Taxlotid as Taxlotid from PM_Taxlots               
 Left outer join PM_CorpActiontaxlots on PM_Taxlots.TaxLot_PK = PM_CorpActiontaxlots.FKId              
 where  PM_Taxlots.Symbol = @Symbol and DateDiff(d,PM_Taxlots.AUECModifiedDate,@selectedDate) = 0                   
 and PM_CorpActiontaxlots.CorpActionId is not null              
 group by PM_Taxlots.taxlotid              
          
 Union          
 -- Only for closing taxlots due to corporate action          
 Select PT.Taxlot_PK as Taxlot_PK, PT.Taxlotid as Taxlotid from PM_taxlots PT           
 inner join PM_CorpActiontaxlots Corp on PT.TaxLotClosingId_Fk = Corp.ClosingId          
 where (PT.TaxLotID = Corp.TaxlotId or PT.TaxLotID = Corp.ClosingTaxlotId)          
 and PT.Symbol = @Symbol and DateDiff(d,PT.AUECModifiedDate,@selectedDate) = 0              
) as TaxlotPK group by Taxlotid              
              
                                                                                       
Select       
PT.TaxLotID as TaxLotID,                                                                                                                                                    
PT.OrderSideTagValue as SideID,                 
PT.Symbol as Symbol ,                                                      
PT.GroupID as GroupID,                                        
PT.TaxLotOpenQty as TaxLotOpenQty ,             
PT.AvgPrice as AvgPrice ,                                                                                                                                                                           
PT.FundID as FundID,                        
PT.Level2ID as Level2ID,                      
G.AUECID as AUECID,                      
G.AUECLocalDate as AUECLocalDate,                                                                             
PT.TaxLot_PK as TaxLot_PK,                                    
G.AssetID,                     
G.UnderLyingID,                    
G.ExchangeID,                         
G.CurrencyID,                    
PT.OpenTotalCommissionandFees,                 
PT.ClosedTotalCommissionandFees,                    
G.OrderTypeTagValue,                    
G.CounterPartyID,                    
G.VenueID,                    
G.TradingAccountID,                    
G.CumQty,                    
G.AllocatedQty,                    
G.ListID,                    
G.ISProrataActive,                    
G.AutoGrouped,                    
G.IsManualGroup,                    
G.Description,                    
CASE 
	WHEN G.CurrencyID <> CF.LocalCurrencyId
	THEN 
		CASE 
		WHEN ISNULL(PT.FXRate, 0) > 0
		THEN 
				CASE ISNULL(PT.FXConversionMethodOperator, 'M')
					WHEN 'M'
					THEN PT.FXRate
					WHEN 'D'
					THEN 1 / PT.FXRate
				END
		WHEN ISNULL(G.FXrate, 0) > 0
		THEN 
				CASE ISNULL(G.FXConversionMethodOperator, 'M')
					WHEN 'M'
					THEN G.FXrate
					WHEN 'D'
					THEN 1 / G.FXrate
				END
		ELSE ISNULL(FXRatesForTradeDate.Val, 0)
		End 
	ELSE 1
END AS FXRate,                   
'M' As FXConversionMethodOperator,        
G.ProcessDate,                
G.OriginalPurchaseDate,         
G.UserId,      
PT.TradeAttribute1,      
PT.TradeAttribute2,      
PT.TradeAttribute3,      
PT.TradeAttribute4,      
PT.TradeAttribute5,                    
PT.TradeAttribute6,    
PT.LotID,    
PT.ExternalTransID,  
SM.Multiplier,
G.TransactionType ,
SM.CompanyName,
((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + (PT.OpenTotalCommissionandFees * dbo.GetSideMultiplier(PT.OrderSideTagValue))) AS NotionalValue_Local,
CASE 
	WHEN G.CurrencyID <> CF.LocalCurrencyId
	THEN 
	CASE 
		WHEN ISNULL(PT.FXRate, 0) > 0
		THEN 
			CASE ISNULL(PT.FXConversionMethodOperator, 'M') 
				WHEN 'M'
				THEN IsNull(((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + 
					(PT.OpenTotalCommissionandFees * dbo.GetSideMultiplier(PT.OrderSideTagValue))) * Isnull(PT.FXRate,0), 0)
				WHEN 'D'
				THEN IsNull(((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + 
					(PT.OpenTotalCommissionandFees * dbo.GetSideMultiplier(PT.OrderSideTagValue))) * 1 / Isnull(PT.FXRate, 0), 0)
			End 
		WHEN ISNULL(G.FXRate, 0) > 0
		THEN 
			CASE ISNULL(G.FXConversionMethodOperator, 'M') 
				WHEN 'M'
				THEN IsNull(((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + 
				(PT.OpenTotalCommissionandFees * dbo.GetSideMultiplier(PT.OrderSideTagValue))) * Isnull(G.FXRate,0), 0)
				WHEN 'D'
				THEN IsNull(((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + 
				(PT.OpenTotalCommissionandFees * dbo.GetSideMultiplier(PT.OrderSideTagValue))) * 1 / Isnull(G.FXRate, 0), 0)
			End 			
		ELSE IsNull(((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + 
			(PT.OpenTotalCommissionandFees * dbo.GetSideMultiplier(PT.OrderSideTagValue))) * IsNull(FXRatesForTradeDate.Val, 0), 0)
		END
	ELSE ((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + (PT.OpenTotalCommissionandFees * dbo.GetSideMultiplier(PT.OrderSideTagValue)))
END AS NotionalValue_Base,
G.SettlementDate  
      
From PM_Taxlots PT                     
Inner join  #TempTaxlot_PK temp on  PT.Taxlot_PK =  temp.Taxlot_PK              
Inner join  T_Group G on G.GroupID=PT.GroupID                                                                                        
inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID    
Inner Join #SecMasterTable SM On SM.TickerSymbol = PT.Symbol            
INNER JOIN #Funds CF ON CF.FundID = PT.FundID 
LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrencyId
		AND DateDiff(DAY, G.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = PT.FundID
		)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
		ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID
		AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrencyId
		AND DateDiff(DAY, G.AUECLocalDate, ZeroFundFxRateTradeDate.DATE) = 0
		AND ZeroFundFxRateTradeDate.FundID = 0
		)
CROSS APPLY (
	SELECT CASE 
			WHEN FXDayRatesForTradeDate.RateValue IS NULL
				THEN 
					CASE 
						WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
						THEN 0
					ELSE ZeroFundFxRateTradeDate.RateValue
					END
			ELSE FXDayRatesForTradeDate.RateValue
			END
	) AS FXRatesForTradeDate(Val)
Where PT.TaxLotOpenQty > 0 
                                                        
                              
Drop Table #TempTaxlot_PK,#Funds,#SecMasterTable, #FXConversionRates, #ZeroFundFxRate
                                               
End     

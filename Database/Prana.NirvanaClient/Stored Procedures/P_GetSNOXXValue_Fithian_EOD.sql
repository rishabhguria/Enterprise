
                             
/*                        
exec [P_GetSNOXXValue_Fithian_EOD] @thirdPartyID=27,@companyFundIDs=N'30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72',
@inputDate='07-21-2023',@companyID=5,@auecIDs=N'1,15',@TypeID=0,@dateType=0,@fileFormatID=47                      
*/                        
                             
CREATE Procedure [dbo].[P_GetSNOXXValue_Fithian_EOD]                      
(                               
@ThirdPartyID int,                                          
@CompanyFundIDs varchar(max),                                                                                                                                                                        
@InputDate datetime,                                                                                                                                                                    
@CompanyID int,                                                                                                                                    
@AUECIDs varchar(max),                                                                          
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                          
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                          
@FileFormatID int                                
)                              
                              
AS                                
                                                                                               
Begin 

Set Nocount On                        
                        
--Declare @ThirdPartyID int                                         
--Declare @CompanyFundIDs varchar(max)                                                                                                                                                                        
--Declare @inputDate datetime                                                                                                                                                                    
--Declare @companyID int                                                                                                                                   
--Declare @auecIDs varchar(max)                                                                          
--Declare @TypeID int  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                          
--Declare @DateType int -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                          
--Declare @fileFormatID int                        
                        
--Set @thirdPartyID=50
--Set @companyFundIDs=N'30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72'
--Set @inputDate='07-24-2023'
--Set @companyID=7
--Set @auecIDs=N'20,109,65,67,64,63,102,29,44,34,43,78,56,59,31,54,21,18,119,61,74,1,15,11,62,73,12,32,81'
--Set @TypeID=0
--Set @dateType=0
--Set @fileFormatID=108   


Declare @PreviousDate DateTime

Set @PreviousDate = DBO.AdjustBusinessDays(@inputDate,-1,1)                  
                     
Declare @Fund Table                                                
(                
FundID int                      
)                                              
                              
Insert into @Fund                        
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')   

/*

Remove 3 accounts as per the request by Sukhjinder

30	Irwin - Evercore
31	SURS - NT
32	MassPrim - BNY

*/

Delete From @Fund Where FundID In (30,31,32)

-- get forex rates for 2 date ranges                        
CREATE TABLE #FXConversionRates (
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,FundID INT
	)
	
	--insert FX Rate for date ranges in the temp table                        
INSERT INTO #FXConversionRates
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @inputDate ,@inputDate
                      
UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0 AND ConversionMethod = 1

--Select *
--From #FXConversionRates

-- For Fund Zero                  
SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE FundID = 0    

Delete From #FXConversionRates Where FundID = 0                        
                              
Create Table #SecMasterDataTempTable                      
(                                                                                                                                                                                           
TickerSymbol Varchar(100),                                                              
Multiplier Float              
)                                                                                                                      
                                                                                                                   
Insert Into #SecMasterDataTempTable                                                            
Select                                                                                                           
TickerSymbol,                                                                                                          
Multiplier                       
 From V_SecMasterData SM          
                            
Create Table #Temp_NetNotionalForADate_Detailed                              
(                                                                                                                                                                
	Symbol Varchar(200), 
	Side Varchar(50),                                                                     
	TaxlotQty Float,                 
	AvgPrice Float,  
	AccountID Int,                                            
	Account Varchar(50),                                                                     
	TradeDateFXRate Float,
	FXConversionMethodOperator Varchar(5),
	SideMultiplier Int, 
	TotalCommissionAndFee_Local Float,
	TotalCost_Local Float,           
	NetNotional_Base Float                                                                                                                               
)                                                                                                        
                                                                                               
 Insert Into #Temp_NetNotionalForADate_Detailed                                                                                                        
(                                                                                                        
Symbol,   
Side,                                                                                           
TaxLotQty,                                                                                                        
AvgPrice,
AccountID,                                                                                                                                                                                             
Account,                       
TradeDateFXRate  ,
FXConversionMethodOperator, 
SideMultiplier,
TotalCommissionAndFee_Local,
TotalCost_Local,  
NetNotional_Base   
)                                
                             
Select                                                                                                                     
VT.Symbol,  
S.Side,                                                                                                                
VT.TaxLotQty ,                                                
VT.AvgPrice as AvgPrice , 
CF.CompanyFundID As AccountID,                                                                                                                                                                                           
CF.FundName as Account,                      
CASE 
	WHEN VT.CurrencyID <> CF.LocalCurrency
		THEN	
			CASE 
				WHEN Isnull(VT.FXRate_Taxlot,0) > 0
				Then VT.FXRate_Taxlot
				WHEN Isnull(VT.FXRate,0) > 0
				Then VT.FXRate
				Else IsNull(FXRatesForTradeDate.Val, 0)
			END
	ELSE 1
END AS FXRate,

CASE 
	WHEN VT.CurrencyID <> CF.LocalCurrency
	THEN	
		CASE 
			WHEN Isnull(VT.FXRate_Taxlot,0) > 0
			Then IsNull(VT.FXConversionMethodOperator_Taxlot,'M')
			WHEN Isnull(VT.FXRate,0) > 0
			Then IsNull(VT.FXConversionMethodOperator,'M')
		Else 'M'
		END
	ELSE 'M'
END AS FXConversionMethodOperator,

VT.SideMultiplier,
(TotalExpenses  - OptionPremiumAdjustment)  As TotalCommissionAndFee_Local,         
((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier)) As TotalCost_Local,
0 As NetNotional_Base

                              
From V_TaxLots VT With (NoLock)
Inner Join @Fund Fund On Fund.FundID = VT.FundID 
Inner Join T_Side S With (NoLock) On S.SideTagValue = VT.OrderSideTagValue                 
Inner Join T_Companyfunds CF With (NoLock) On CF.Companyfundid = VT.FundID         
Inner Join #SecMasterDataTempTable SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (
		FXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(DAY, VT.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = CF.CompanyFundID
		)
		LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (
		ZeroFundFxRateTradeDate.FromCurrencyID = VT.CurrencyID
		AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, VT.AUECLocalDate, ZeroFundFxRateTradeDate.DATE) = 0
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
 Where DateDiff(Day,VT.AUECLocalDate, @inputDate) = 0                    

Update #Temp_NetNotionalForADate_Detailed
Set TradeDateFXRate = 
Case
When FXConversionMethodOperator = 'D' And TradeDateFXRate > 0
Then 1/TradeDateFXRate
Else TradeDateFXRate
End

Update #Temp_NetNotionalForADate_Detailed
Set NetNotional_Base = ((TotalCost_Local + (TotalCommissionAndFee_Local * SideMultiplier)) * TradeDateFXRate) * (SideMultiplier * -1)

--Select *
--From #Temp_NetNotionalForADate_Detailed 
--Order By Account, Symbol 


Select 
Account,
AccountID,
Sum(NetNotional_Base) As AccountNotional
InTo #Temp_AccountNotionalBaseValue
From #Temp_NetNotionalForADate_Detailed
Group By AccountID,Account

--Select *
--From #Temp_AccountNotionalBaseValue
                
                  
Select                   
CF.FundName As AccountName,
CF.CompanyFundID As AccountID,                   
Sum(EODCash.CashValueBase) As EODCash_Base
InTo #Temp_EODCash                 
From PM_CompanyFundCashCurrencyValue EODCash With (NoLock) 
Inner Join @Fund Fund on Fund.FundID = EODCash.FundID                    
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = EODCash.FundID                  
Where DateDiff(Day,EODCash.Date,@PreviousDate) = 0          
Group By CF.CompanyFundID,CF.FundName,EODCash.BaseCurrencyID  


--Select *
--From #Temp_EODCash

Select 
ANNB.AccountID,
ANNB.Account,
'SNOXX' As Symbol,
'Sell' As Side,
(Abs(Round((IsNull(ANNB.AccountNotional,0) + IsNull(EODCash.EODCash_Base,0)),2))) + 200 As Quantity,
(Abs(Round((IsNull(ANNB.AccountNotional,0) + IsNull(EODCash.EODCash_Base,0)),2))) + 200 As NotionalValue,
convert(varchar,@inputDate,101) As TradeDate

From #Temp_AccountNotionalBaseValue ANNB
Left Outer Join #Temp_EODCash EODCash On EODCash.AccountName =  ANNB.Account
Where (IsNull(ANNB.AccountNotional,0) + IsNull(EODCash.EODCash_Base,0)) < 0
Order By ANNB.Account                       
                              
Drop Table #Temp_NetNotionalForADate_Detailed,#SecMasterDataTempTable
Drop table #FXConversionRates,#ZeroFundFxRate,#Temp_AccountNotionalBaseValue,#Temp_EODCash
          
End
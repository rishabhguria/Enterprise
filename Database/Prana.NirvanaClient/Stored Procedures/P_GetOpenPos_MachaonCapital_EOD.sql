
/*

EXEC [P_GetOpenPos_MachaonCapital_EOD] 1, '1,5', '03-09-2023',1,1,1,1,1

*/

CREATE Procedure [dbo].[P_GetOpenPos_MachaonCapital_EOD]                        
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

--Declare @ThirdPartyID int,                                            
--@CompanyFundIDs varchar(max),                                                                                                                                                                          
--@InputDate datetime,                                                                                                                                                                      
--@CompanyID int,                                                                                                                                      
--@AUECIDs varchar(max),                                                                            
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
--@FileFormatID int  

--set @thirdPartyID=103
--set @companyFundIDs=N'1'
--set @inputDate='2023-12-20 05:39:14'
--set @companyID=7
--set @auecIDs=N'1,15,12'
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=49


Declare @Fund Table                                                                     
(                          
FundID int                                
)            
          
Insert into @Fund                                                                                                              
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')   


Create Table #Fund                                                           
(                
FundID int,
FundName Varchar(100),
LocalCurrency Int,
MasterFundName Varchar(100)                      
)  

Insert into #Fund 
Select 
CF.CompanyFundID,
CF.FundName,
CF.LocalCurrency, 
MF.MasterFundName
From T_CompanyFunds CF WITH(NOLOCK)
Inner Join @Fund F On F.FundID = CF.CompanyFundID
Inner Join T_CompanyMasterFundSubAccountAssociation MFA WITH(NOLOCK) On MFA.CompanyFundID = CF.CompanyFundID 
Inner Join T_CompanyMasterFunds MF WITH(NOLOCK) On MF.CompanyMasterFundID = MFA.CompanyMasterFundID

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
FROM PM_DayMarkPrice DMP With (NoLock)
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE FundID = 0

Create Table #TempTaxlotPK
( 
Taxlot_PK BigInt
)

Insert InTo #TempTaxlotPK
SELECT Distinct PT.Taxlot_PK    

FROM PM_Taxlots PT With (NoLock)          
Inner Join @Fund Fund on Fund.FundID = PT.FundID  
Where PT.Taxlot_PK in                                       
(                                                                                                
 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                 
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                      
 Group by TaxlotId                                                                      
)                                                                                          
And Round(PT.TaxLotOpenQty,2) > 0 

Select  
CF.MasterFundName As MasterFund, 
CF.FundName As Account,  
A.AssetName As AssetClass,
SM.TickerSymbol As Symbol,  
Case  dbo.GetSideMultiplier(PT.OrderSideTagValue)                    
	When 1                      
	Then 'LONG'                      
Else 'SHORT'                      
End as PositionSide,        
SM.CompanyName As SecurityName,  
PT.TaxlotOpenQty As OpenQty,
IsNull(MPEndDate.Val, 0) As MarkPrice,
SM.ISINSymbol As ISINSymbol,
SM.CUSIPSymbol As CUSIPSymbol,
SM.SEDOLSymbol As SEDOLSymbol,
SM.OSISymbol As OSISymbol,
SM.BloombergSymbol AS BloombergSymbol,

dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
TradeCurr.CurrencySymbol As TradeCurrency,
PortfolioCurr.CurrencySymbol As PortfolioCurrency,
SM.UnderLyingSymbol,
SM.PutOrCall

Into #TempOpenPositionsTable         
From PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join #Fund CF With (NoLock) On CF.FundID = PT.FundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency TradeCurr With (NoLock) On TradeCurr.CurrencyID = SM.CurrencyID  
Inner Join T_Currency PortfolioCurr With (NoLock) On PortfolioCurr.CurrencyID = CF.LocalCurrency
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID
INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID

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


Where PT.TaxlotOpenQty > 0


----Select * from #TempOpenPositionsTable

Select  
Temp.MasterFund,
Temp.Account As Account, 
AssetClass As AssetClass,    
Temp.Symbol,       
Max(SecurityName) As SecurityName,
Sum(Temp.OpenQty * Temp.SideMultiplier) As OpenQty, 
Max(MarkPrice) As MarkPrice,     
Max(ISINSymbol) As ISINSymbol,
Max(CUSIPSymbol) As CUSIPSymbol,
Max(SEDOLSymbol) As SEDOLSymbol,
Max(OSISymbol) As OSISymbol,
Max(BloombergSymbol) As BloombergSymbol,
Max(TradeCurrency) As TradeCurrency,
Max(PortfolioCurrency) As PortfolioCurrency, 
Max(UnderlyingSymbol) As UnderlyingSymbol,
Max(PutOrCall) AS PutOrCall

Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.MasterFund, Temp.Account,Temp.AssetClass,Temp.Symbol
 

Select 
Convert(varchar, @InputDate,101) As TradeDate,  
AssetClass,
T.MasterFund,
Account,
Symbol,
SecurityName,
Round(OpenQty,0) As OpenQty,
MarkPrice,
ISINSymbol,
CUSIPSymbol,
SEDOLSymbol,
OSISymbol,
BloombergSymbol,
TradeCurrency,
PortfolioCurrency,
UnderlyingSymbol,
PutOrCall,
2 As CustomOrder

InTo #Temp_FinalTable
From #TempTable T


Insert into #Temp_FinalTable        
select    
Convert(varchar, @InputDate,101) As TradeDate,
'Cash' As AssetClass,  
CF.MasterFundName As MasterFund,  
CF.FundName As Account,                                 
('CASH_'+CurrencyLocal.CurrencySymbol) As Symbol,
CurrencyLocal.CurrencySymbol As SecurityName,                                  
Cash.CashValueLocal as OpenQty,
0.0 As MarkPrice,
'' As ISINSymbol,
'' As CUSIPSymbol,
'' As SEDOLSymbol,
'' As OSISymbol,
'' AS BloombergSymbol,
CurrencyLocal.CurrencySymbol As TradeCurrency,
CurrencyBase.CurrencySymbol As PortfolioCurrency ,
'' As UnderlyingSymbol,
'' As PutOrCall,
1 As CustomOrder                           
From PM_CompanyFundCashCurrencyValue Cash  With (NoLock)                     
Inner join #Fund CF On CF.FundId = Cash.FundID                      
Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = Cash.LocalCurrencyID                                
Inner join T_Currency CurrencyBase  With (NoLock) On CurrencyBase.CurrencyId = Cash.BaseCurrencyID                                
Where DateDiff(Day, Cash.Date, @inputDate) = 0


Select *
From #Temp_FinalTable
Order By CustomOrder, MasterFund,Account,Symbol  
 

Drop Table #TempOpenPositionsTable, #TempTable,#Temp_FinalTable, #Fund
Drop Table #TempTaxlotPK,#MarkPriceForEndDate,#ZeroFundMarkPriceEndDate

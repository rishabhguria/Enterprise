
/*

EXEC [P_GetCustomHolding_NDS_BBG_EOD] 1, '1,5', '03-09-2023',1,1,1,1,1

*/

CREATE Procedure [dbo].[P_GetCustomHolding_PrevattCapital_EOD]                        
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

Declare @PreviousBusinessDate DateTime

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

Set @PreviousBusinessDate = (Select Min(G.AUECLocalDate) As MinTradeDate
FROM PM_Taxlots PT WITH(NOLOCK)
Inner Join #TempTaxlotPK T On T.Taxlot_PK = PT.Taxlot_PK
INNER JOIN T_Group G WITH(NOLOCK) ON PT.GroupID = G.GroupID)

Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@PreviousBusinessDate,-1,1)


Select   
CF.FundName As Account,  
A.AssetName As AssetClass,
SM.TickerSymbol As Symbol,          
SM.CompanyName As SecurityName, 
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier, 
PT.TaxlotOpenQty As OpenQty,
IsNull(Finalmarkprice, 0) As MarkPrice,
SM.ISINSymbol As ISINSymbol,
SM.CUSIPSymbol As CUSIPSymbol,
SM.SEDOLSymbol As SEDOLSymbol,
TradeCurr.CurrencySymbol As TradeCurrency,
CP.ShortName AS Broker

Into #TempOpenPositionsTable         
From PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join #Fund CF With (NoLock) On CF.FundID = PT.FundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency TradeCurr With (NoLock) On TradeCurr.CurrencyID = SM.CurrencyID  
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID
INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID
INNER JOIN T_CounterParty CP With (NoLock) ON CP.CounterPartyID = G.CounterPartyID
LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
		MPE.Symbol = PT.Symbol AND MPE.FundID = 0 )
Where PT.TaxlotOpenQty > 0

Select  
Temp.Account As Account, 
AssetClass As AssetClass,    
Temp.Symbol,       
Max(SecurityName) As SecurityName,
Sum(Temp.OpenQty * Temp.SideMultiplier) As OpenQty, 
Max(MarkPrice) As MarkPrice,   
Max(ISINSymbol) As ISINSymbol,
Max(CUSIPSymbol) As CUSIPSymbol,
Max(SEDOLSymbol) As SEDOLSymbol,
Max(TradeCurrency) As TradeCurrency,
Max(Broker) As Broker


Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By  Temp.Account,Temp.AssetClass,Temp.Symbol


Select 
CONVERT(varchar, @InputDate, 101) As HoldingDate,  
AssetClass,
Account,
Symbol,
SecurityName,
Round(OpenQty,0) As Quantity,
MarkPrice,
ISINSymbol,
CUSIPSymbol,
SEDOLSymbol,
TradeCurrency,
Broker

InTo #Temp_FinalTable
From #TempTable T


Insert into #Temp_FinalTable        
select    
 CONVERT(varchar, @InputDate, 101) As HoldingDate,
'' As AssetClass,    
CF.FundName As Account,                                 
'' As Symbol,
'' As SecurityName,                                  
0.0 As Quantity,
0.0 As MarkPrice,
'' As ISINSymbol,
'' As CUSIPSymbol,
'' As SEDOLSymbol,
CurrencyLocal.CurrencySymbol As TradeCurrency,
'' As Broker
                        
From PM_CompanyFundCashCurrencyValue Cash  With (NoLock)                     
Inner join #Fund CF On CF.FundId = Cash.FundID                      
Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = Cash.LocalCurrencyID                                
Inner join T_Currency CurrencyBase  With (NoLock) On CurrencyBase.CurrencyId = Cash.BaseCurrencyID                                
Where DateDiff(Day, Cash.Date, @inputDate) = 0


Select *
From #Temp_FinalTable
--Where SEDOLSymbol = 'BQT3XY6'
Order By Account,Symbol  
 

Drop Table #TempOpenPositionsTable, #TempTable,#Temp_FinalTable, #Fund
Drop Table #TempTaxlotPK,#MarkPriceForEndDate
--Drop Table #TempMasterFundNAV
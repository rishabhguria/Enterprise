/*                      
exec P_GetOpenPosWithAccruals_Chimera_BBG @inputDate='07-29-2020'
*/                      
                          
CREATE Procedure [dbo].[P_GetOpenPosWithAccruals_Chimera_BBG]                    
(                             
@InputDate Datetime                                                                                                                                                                
)                              
AS                                                                                               
Begin                       
  
 SET NOCOUNT ON;
                     
--Declare @inputDate datetime                                                                                                                                                                  
--Set @inputDate =   '07-30-2020'                                                                                                                                                               
  
---- Previous business date for which accruals has to get  
DECLARE @DayPriorToStartDate DATETIME      
Set @DayPriorToStartDate = dbo.AdjustBusinessDays(@InputDate, - 1, 1)    
                   

Declare @Fund Table                                                         
(              
FundID int                    
)                        
               
Insert into @Fund                                                                                                  
Select CompanyFundID From T_CompanyFunds 
Where FundName <> 'Chimera Systematic'  

Declare @Strategy Table                                                         
(              
StrategyID int                    
)                        
               
Insert into @Strategy                                                                                                  
Select CompanyStrategyID From T_CompanyStrategy 
Where StrategyName <> 'Systematic'                                                                                                  
                                                                           
  
Create Table #TempEODCashAndAccruals  
(  
AccountName Varchar(200)  
,Symbol Varchar(200)  
,Qty Float  
,TradeDate DateTime  
,AvgPrice Float  
,BBCode Varchar(200)  
,ISIN Varchar(200)  
,CUSIP Varchar(200)  
,SEDOL Varchar(200)  
,FullSecurityName Varchar(200)  
,ForexRate Float  
,OSISymbol Varchar(200)  
,Asset Varchar(200)  
,UDASectorName Varchar(200)  
,CurrencySymbol Varchar(20)  
)  
  
Insert Into #TempEODCashAndAccruals  
SELECT   
Max(CF.FundName) As AccountName  
,Max(CURR.CurrencySymbol) As Symbol  
,SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)) AS Qty    
,MAX(SubAccountBalances.TransactionDate) AS TradeDate    
,SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)) AS AvgPrice  
,Max(CURR.CurrencySymbol) + ' Curncy'  as BBCode   
,Max(CURR.CurrencySymbol) as ISIN                    
,Max(CURR.CurrencySymbol) as CUSIP                   
,Max(CURR.CurrencySymbol) as SEDOL                          
,Max(CURR.CurrencySymbol) as FullSecurityName    
,0 as ForexRate  
,Max(CURR.CurrencySymbol) as OSISymbol  
,'Cash' As Asset  
,'Accruals' As UDASectorName  
,'' As CurrencySymbol  
   
FROM T_SubAccountBalances SubAccountBalances With (NoLock)   
INNER JOIN T_SubAccounts SubAccounts With (NoLock) ON SubAccounts.SubAccountID = SubAccountBalances.SubAccountID    
INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeID = TransType.TransactionTypeID    
INNER JOIN @Fund Fund ON SubAccountBalances.FundID = Fund.FundID  
inner join T_Companyfunds CF With (NoLock) on CF.Companyfundid = Fund.FundID                 
Inner Join T_Currency CURR On CURR.CurrencyID =  SubAccountBalances.CurrencyID    
WHERE (DateDiff(Day, SubAccountBalances.TransactionDate, @DayPriorToStartDate) = 0)    
 AND TransType.TransactionType = 'Accrued Balance'                
GROUP BY CF.FundName ,CURR.CurrencySymbol ,SubAccounts.NAME    
HAVING SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)) <> 0   
  
  
Insert Into #TempEODCashAndAccruals                
Select                 
CF.FundName As AccountName,   
CURR.CurrencySymbol As Symbol,                
Sum(FCC.CashValueBase) As Qty,                
@InputDate as TradeDate,       
Sum(CashValueLocal),                
CURR.CurrencySymbol + ' Curncy'  as BBCode ,                     
CURR.CurrencySymbol as ISIN,                    
CURR.CurrencySymbol as CUSIP,                    
CURR.CurrencySymbol as SEDOL,                           
CURR.CurrencySymbol as FullSecurityName,                     
0 as ForexRate  ,          
CURR.CurrencySymbol as OSISymbol,          
'Cash',        
'',      
''        
From PM_CompanyFundCashCurrencyValue FCC With (NoLock)                
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = FCC.FundID                
Inner Join T_Currency CURR On CURR.CurrencyID = FCC.BaseCurrencyID                
INNER JOIN @Fund Fund ON FCC.FundID = Fund.FundID  
Where DateDiff(Day,FCC.Date,@InputDate) = 0                
Group By CF.FundName,CURR.CurrencySymbol   
                            
                            
Create Table #SecMasterDataTempTable                     
(                                                                                                                                                                                         
TickerSymbol Varchar(100),                                                            
UnderlyingSymbol varchar(100),                          
BloombergSymbol varchar(100),                      
ISINSymbol varchar(100),                    
CUSIPSymbol varchar(100),                    
SEDOLSymbol varchar(100),                        
CompanyName varchar(100) ,        
SectorName Varchar(100),  
OSISymbol varchar(100)             
)                                                                                                                    
                                                                                                                 
Insert Into #SecMasterDataTempTable                                                          
Select                                                                                                         
TickerSymbol,                                                                                                        
UnderlyingSymbol,                          
BloombergSymbol,                    
ISINSymbol,                    
CUSIPSymbol,                    
SEDOLSymbol,                          
CompanyName,        
SectorName,  
OSISymbol                       
 From V_SecMasterData SM With (NoLock)        
  
  
SELECT Distinct PT.Taxlot_PK  
InTo #TempTaxlotPK       
FROM PM_Taxlots PT With (NoLock)       
--Inner Join @Fund Fund on Fund.FundID = PT.FundID
Inner Join @Strategy S On S.StrategyID = PT.Level2ID             
Where PT.Taxlot_PK in                                     
(                                                                                              
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                 
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                    
 group by TaxlotId                                                                    
)                                                                                        
And PT.TaxLotOpenQty <> 0  

  
Create Table #TEMPFundPositionsForDate                            
(                                                                
 TaxLotID  varchar(50),                                                                                      
 Symbol varchar(200),                                                                    
 OpenQuantity float,            
 AveragePrice float,               
 Fund varchar(50),                                                                   
 UnderLyingID int,                                                                                                                                                                                               
 SettlementDate datetime,                                
 AUECLocalDate datetime,                               
 UnderlyingSymbol varchar(100),                          
 BloombergSymbol  varchar(100)  ,                    
 ISINSymbol varchar(100),             
 CUSIPSymbol varchar(100),                    
 SEDOLSymbol varchar(100),       
 CompanyName varchar(100) ,                    
 TaxLotState int,                    
 FXRate float   ,          
 OSISymbol VARCHAR(200),          
 Asset varchar(100) ,        
 SectorName Varchar(100),      
 CurrencySymbol Varchar(20) ,
 Strategy Varchar(100)             
)                                                                                                        
                                                                                             
 Insert Into #TEMPFundPositionsForDate                                                                                                      
(                                                                                                      
 TaxLotID,                                                                                                                                                            
 Symbol,                                                                                           
 OpenQuantity,                                                                                                      
 AveragePrice,                                                                                                                                                                                           
 Fund,                                                                                                                                     
 UnderLyingID,  
 SettlementDate,                        
 AUECLocalDate,                            
 UnderlyingSymbol,                          
 BloombergSymbol  ,                     
 ISINSymbol,                    
 CUSIPSymbol,                    
 SEDOLSymbol,                      
 CompanyName ,                    
 TaxLotState ,                    
 FXRate  ,          
 OSISymbol ,          
 Asset   ,        
 SectorName ,      
 CurrencySymbol,
 Strategy
 )                                 
                                                                                      
Select                                                                                                                   
 PT.TaxLotID as TaxLotID,                                                                                              
 PT.Symbol as Symbol ,                                                                                                                
 (PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)) as TaxLotOpenQty ,                                              
 PT.AvgPrice as AvgPrice ,                                                                                                                                                                                          
 Funds.FundName as Fund,                                                                                                           
 G.UnderLyingID as UnderLyingID,                                                     
 G.SettlementDate as SettlementDate,                                                                                                      
 G.ProcessDate as AUECLocalDate,                           
 SM.UnderlyingSymbol,                          
 SM.BloombergSymbol ,                      
 SM.ISINSymbol,                    
 SM.CUSIPSymbol,                    
 SM.SEDOLSymbol,                          
 SM.CompanyName,                    
 0,                  
 PT.FXRate  ,          
 SM.OSISymbol as OSISymbol ,          
 Asset.AssetName as Asset,        
 SM.SectorName AS UDASectorName,      
 TC.CurrencySymbol as CurrencySymbol,
 CS.StrategyShortName As Strategy                                                                              
                            
from PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK            
Inner join  T_Group G With (NoLock) on G.GroupID=PT.GroupID                                        
Inner join T_Currency TC on TC.CurrencyID = G.CurrencyID      
inner join T_Asset Asset on Asset.AssetId= G.AssetID                             
inner join T_Companyfunds Funds With (NoLock) on Funds.Companyfundid = PT.FundID  
inner join T_CompanyStrategy CS On CS.CompanyStrategyID = PT.Level2ID                 
Inner JOIN #SecMasterDataTempTable AS SM ON SM.TickerSymbol = PT.Symbol                               
Order By  Funds.FundName, Strategy, PT.Symbol                
  
--- Open Positions
Select 
Fund As PortfolioName,  
Case 
	When Asset = 'Equity' And Max(BloombergSymbol) = '' And Max(CurrencySymbol) = 'USD'
	Then Symbol + ' US'+ ' EQUITY'
	When Asset = 'EquityOption'
	Then MAX(OSISymbol)
	Else Max(BloombergSymbol)
End As [Security],
Max(SEDOLSymbol) As Sedol,
Max(CUSIPSymbol) As Cusip, 
Max(ISINSymbol) As ISIN, 
MAX(CompanyName) As SecurityName, 
SUM(OpenQuantity) as Position,  
'' As [Weight],
'' As MktPx,
AVG(AveragePrice) As CostPrice,
Max(Convert(varchar,@InputDate, 101)) As AsOfDate,
Asset As NewClassification 
From #TEMPFundPositionsForDate  
Group By Fund, Strategy, Asset, Symbol   

Union All

Select 
'Unallocated' As PortfolioName,  
Case 
	When A.AssetName = 'Equity' And Max(SM.BloombergSymbol) = '' And Max(CurrencySymbol) = 'USD'
	Then G.Symbol + ' US'+ ' EQUITY'
	When A.AssetName = 'EquityOption'
	Then MAX(SM.OSISymbol)
	Else Max(SM.BloombergSymbol)
End As [Security],
Max(SM.SEDOLSymbol) As Sedol,
Max(SM.CUSIPSymbol) As Cusip, 
Max(SM.ISINSymbol) As ISIN, 
MAX(SM.CompanyName) As SecurityName, 
SUM(G.CumQty * dbo.GetSideMultiplier(G.OrderSideTagValue)) as Position,
'' As [Weight],
'' As MktPx,
AVG(G.AvgPrice) As CostPrice,
Max(Convert(varchar,@InputDate, 101)) As AsOfDate,
A.AssetName As NewClassification 
From T_Group G With (NoLock) 
Inner join T_Currency TC on TC.CurrencyID = G.CurrencyID      
inner join T_Asset A on A.AssetId= G.AssetID                                            
Inner JOIN #SecMasterDataTempTable SM ON SM.TickerSymbol = G.Symbol
Where G.CumQty > 0 
And G.StateID = 1   
And DateDiff(Day,AUECLocalDate,@inputDate) = 0  
Group By A.AssetName,G.Symbol              
           
Union All   
  
Select   
AccountName As PortfolioName,  
Max(BBCode) As [Security],
Max(SEDOL) As Sedol,
Max(CUSIP) As Cusip, 
Max(ISIN) As ISIN, 
MAX(FullSecurityName) As SecurityName, 
SUM(Qty) as Position,  
'' As [Weight],
'' As MktPx,
Sum(AvgPrice) As CostPrice,
Max(Convert(varchar,TradeDate, 101)) As AsOfDate,
Asset As NewClassification 
From #TempEODCashAndAccruals  
Group By AccountName, Asset, Symbol                

Drop Table #TEMPFundPositionsForDate,#SecMasterDataTempTable,#TempEODCashAndAccruals,#TempTaxlotPK        
End
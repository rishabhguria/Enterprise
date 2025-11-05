/*                        
exec [P_GetSymbolOpenPosWithAccruals_Chimera] @thirdPartyID=93,@companyFundIDs=N'2,1,3,4',@inputDate='2019-02-06 04:30:24:000',    
@companyID=7,@auecIDs=N'1,15,11,12',@TypeID=0,@dateType=0,@fileFormatID=180    
*/                        
--                            
CREATE Procedure [dbo].[P_GetSymbolOpenPosWithAccruals_Chimera]                      
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
                        
--Declare @ThirdPartyID int                                         
--Declare @CompanyFundIDs varchar(max)                                                                                                                                                                        
--Declare @inputDate datetime                                                                                                                                                                    
--Declare @companyID int                                                                                                                                   
--Declare @auecIDs varchar(max)                                                                          
--Declare @TypeID int  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                          
--Declare @DateType int -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                          
--Declare @fileFormatID int                        
--                        
--Set @ThirdPartyID =   93                                        
--Set @companyFundIDs = '2,1,3,4'    
--Set @InputDate =  '2019-02-06'                                                                                                                                                                 
--Set @companyID = 7                                                                                                                                 
--Set @auecIDs = '1,15,11,12'                                                                        
--Set @TypeID = 0                                     
--Set @DateType =  0                                                                                                                                                                       
--Set @fileFormatID = 180      
    
---- Previous business date for which accruals has to get    
DECLARE @DayPriorToStartDate DATETIME        
Set @DayPriorToStartDate = dbo.AdjustBusinessDays(@InputDate, - 1, 1)      
                     
        
Declare @Fund Table                                                           
(                
FundID int                      
)                          
                 
Declare @AUECID Table                                                                                                                                                      
(                                                                                                                                                      
AUECID int                                                                                                                                                  
)                                
                              
Insert into @Fund                                                                                                    
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')                                                                                                       
                                                                             
Insert into @AUECID                                                          
Select Cast(Items as int) from dbo.Split(@auecIDs,',')     
  
  
-- get Mark Price for End Date                        
CREATE TABLE #MarkPriceForEndDate   
(    
  Finalmarkprice FLOAT    
 ,Symbol VARCHAR(max)    
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
FROM PM_DayMarkPrice DMP  
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0  
And FundID = 0  
    
    
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
--Max(CF.FundName) As AccountName    
'Chimera' As AccountName  
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
     
FROM T_SubAccountBalances SubAccountBalances      
INNER JOIN T_SubAccounts SubAccounts ON SubAccounts.SubAccountID = SubAccountBalances.SubAccountID      
INNER JOIN T_TransactionType TransType ON SubAccounts.TransactionTypeID = TransType.TransactionTypeID      
INNER JOIN @Fund Fund ON SubAccountBalances.FundID = Fund.FundID    
inner join T_Companyfunds CF on CF.Companyfundid = Fund.FundID                   
Inner Join T_Currency CURR On CURR.CurrencyID =  SubAccountBalances.CurrencyID      
WHERE (DateDiff(Day, SubAccountBalances.TransactionDate, @DayPriorToStartDate) = 0)      
 AND TransType.TransactionType = 'Accrued Balance'                  
GROUP BY CURR.CurrencySymbol ,SubAccounts.NAME      
HAVING SUM(IsNull(SubAccountBalances.CloseDRBalBase - SubAccountBalances.CloseCRBalBase, 0)) <> 0     
    
    
Insert Into #TempEODCashAndAccruals                  
Select                   
--CF.FundName As AccountName,  
'Chimera' As AccountName,                   
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
From PM_CompanyFundCashCurrencyValue FCC                  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = FCC.FundID                  
Inner Join T_Currency CURR On CURR.CurrencyID = FCC.BaseCurrencyID                  
INNER JOIN @Fund Fund ON FCC.FundID = Fund.FundID    
Where DateDiff(Day,FCC.Date,@InputDate) = 0                  
Group By CURR.CurrencySymbol     
                              
                              
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
OSISymbol varchar(100),  
Multiplier Float               
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
OSISymbol,  
Multiplier                         
 From V_SecMasterData SM          
    
    
SELECT PT.Taxlot_PK    
InTo #TempTaxlotPK         
FROM PM_Taxlots PT          
Inner Join @Fund Fund on Fund.FundID = PT.FundID              
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
 CurrencySymbol Varchar(20),  
 ClosingPrice float ,             
 TotalExposure float  
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
 ClosingPrice,             
 TotalExposure         
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
 PT.FXRate ,            
 SM.OSISymbol as OSISymbol ,            
 Asset.AssetName as Asset,          
 SM.SectorName AS UDASectorName,        
 TC.CurrencySymbol as CurrencySymbol,  
 IsNull(MPZeroEndDate.FinalMarkPrice,0) As ClosingPrice,  
 IsNull(MPZeroEndDate.FinalMarkPrice,0) * (PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)) * SM.Multiplier As TotalExposure                                                                             
                              
From PM_Taxlots PT    
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK              
Inner join  T_Group G on G.GroupID=PT.GroupID                                          
Inner join T_Currency TC on TC.CurrencyID = G.CurrencyID        
inner join T_Asset Asset on Asset.AssetId= G.AssetID                               
inner join T_Companyfunds Funds on Funds.Companyfundid = PT.FundID                   
Inner JOIN #SecMasterDataTempTable AS SM ON SM.TickerSymbol = PT.Symbol    
LEFT OUTER JOIN #MarkPriceForEndDate MPZeroEndDate ON (PT.Symbol = MPZeroEndDate.Symbol)                                 
                  
    
Select                         
'Chimera' As AccountName,  
--Fund as AccountName,                              
Symbol,                               
SUM(OpenQuantity) as Qty,                              
@InputDate as TradeDate,           
AVG(AveragePrice) as AvgPrice ,                         
MAX(BloombergSymbol)  as BBCode ,                       
MAX(ISINSymbol) as ISIN,                      
MAX(CUSIPSymbol) as CUSIP,                      
MAX(SEDOLSymbol) as SEDOL,                             
MAX(CompanyName) as FullSecurityName,                       
MAX(isnull(FXRate,0)) as ForexRate,            
MAX(OSISymbol) as OSISymbol ,            
MAX(Asset) as Asset  ,          
max(SectorName) AS UDASectorName,        
max(CurrencySymbol) As CurrencySymbol,  
Max(ClosingPrice) As ClosingPrice,  
Sum(TotalExposure) As TotalExposure                         
From #TEMPFundPositionsForDate                              
group by Symbol                    
                  
Union All     
    
Select     
AccountName,    
Symbol,    
SUM(Qty) as Qty,      
Max(TradeDate) As TradeDate,    
Sum(AvgPrice) As AvgPrice,    
Max(BBCode) As BBCode,    
Max(ISIN) As ISIN,    
Max(CUSIP) As CUSIP,    
Max(SEDOL) As SEDOL,    
MAX(FullSecurityName) As FullSecurityName,    
0 As ForexRate,    
MAX(OSISymbol) as OSISymbol ,    
Max(Asset) As Asset,    
'' As UDASectorName,    
'' As CurrencySymbol,  
1 As CLosingPrice,  
SUM(Qty) As TotalExposure    
From #TempEODCashAndAccruals    
Group By AccountName , Symbol                  
                     
                              
Drop Table #TEMPFundPositionsForDate,#SecMasterDataTempTable,  
 #TempEODCashAndAccruals,#TempTaxlotPK, #MarkPriceForEndDate  
          
End 
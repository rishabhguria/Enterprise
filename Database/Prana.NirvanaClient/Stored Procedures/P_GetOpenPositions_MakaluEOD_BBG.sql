                             
/*                        
exec [P_GetOpenPositions_MakaluEOD_BBG] @thirdPartyID=27,@companyFundIDs=N'1184,1183,1182,1185',@inputDate='2015-10-12 15:26:19:000',@companyID=5,@auecIDs=N'1,15',@TypeID=0,@dateType=0,@fileFormatID=47                      
*/                        
                             
CREATE Procedure [dbo].[P_GetOpenPositions_MakaluEOD_BBG]                      
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
                        
--Set @thirdPartyID=50
--Set @companyFundIDs=N'1267,1269,1266'
--Set @inputDate='2023-05-22 05:36:01'
--Set @companyID=7
--Set @auecIDs=N'20,109,65,67,64,63,102,29,44,34,43,78,56,59,31,54,21,18,119,61,74,1,15,11,62,73,12,32,81'
--Set @TypeID=0
--Set @dateType=0
--Set @fileFormatID=108                     
                     
Declare @Fund Table                                                
(                
FundID int                      
)                          
               
--Declare @AUECID Table                                                                                                                                                      
--(                                                                                                                                                      
--AUECID int                                                
--)                                
                              
Insert into @Fund                                                                                                    
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')                                                                                                       
                                                                             
--Insert into @AUECID                                                          
--Select Cast(Items as int) from dbo.Split(@auecIDs,',')                               
                              
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
Issuer Varchar(200),
OSISymbol Varchar(200),
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
IsNull(Issuer,'Undefined') As Issuer,
OSISymbol,
Multiplier                       
 From V_SecMasterData SM          
left OUTER join V_UDA_DynamicUDA UDA on UDA.Symbol_PK = SM.Symbol_PK                
                            
Create Table #TEMPFundPositionsForDate                              
(                                                                                                                                                                
 TaxLotID  varchar(50),                                                                                        
 Symbol varchar(200),                                                                      
 OpenQuantity float, -- quantity is the net quantity of the position fetched i.e. the current quantity.                   
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
Issuer Varchar(200),
TotalCost Float,
OpenQty Float                                                                                                                                  
)    

Select Max(Taxlot_PK) As Taxlot_PK
InTo #TemP_TaxlotPK
From PM_Taxlots PT
Inner Join @Fund Fund on Fund.FundID = PT.FundID                                                                                    
Where DateDiff(d, PT.AUECModifiedDate,@InputDate) >= 0                                                                      
Group By TaxlotId                                                                                                       
                                                                                               
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
Issuer,
TotalCost,
OpenQty     
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
0 As TaxLotState,                    
PT.FXRate  ,            
SM.OSISymbol as OSISymbol ,            
Asset.AssetName as Asset,          
SM.SectorName AS UDASectorName,        
TC.CurrencySymbol as CurrencySymbol,  
SM.Issuer,
((PT.TaxLotOpenQty * PT.AvgPrice * SM.Multiplier) + (dbo.GetSideMultiplier(PT.OrderSideTagValue))) As TotalCost,
(PT.TaxLotOpenQty * SM.Multiplier) As OpenQty                                                                         
                              
from PM_Taxlots PT
Inner Join #TemP_TaxlotPK T On T.Taxlot_PK = PT.TaxLot_PK
--inner join @Fund Fund on Fund.FundID = PT.FundID                  
Inner join  T_Group G on G.GroupID=PT.GroupID                                          
Inner join T_Currency TC on TC.CurrencyID = G.CurrencyID        
inner join T_Asset Asset on Asset.AssetId= G.AssetID                               
inner join T_Companyfunds Funds on Funds.Companyfundid = PT.FundID         
Inner JOIN #SecMasterDataTempTable AS SM ON SM.TickerSymbol = PT.Symbol                                 
Where PT.TaxLotOpenQty > 0                          
  
  
Select 
Fund as AccountName,                              
Symbol,                               
SUM(OpenQuantity) as Qty,                              
@InputDate as TradeDate,           
AVG(AveragePrice) as AvgPrice  ,                         
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
Max(Issuer) As Issuer ,
Sum(TotalCost) As TotalCost,
Sum(OpenQty) As OpenQty

InTo #Temp_GroupedOpenPositions 
From #TEMPFundPositionsForDate
Group by Fund,Symbol 

--Select *
--From #Temp_GroupedOpenPositions
--Order By AccountName,Symbol 

Update #Temp_GroupedOpenPositions
Set AvgPrice = 
case
When OpenQty <> 0
Then TotalCost / OpenQty
Else 0
End

--Select *
--From #Temp_GroupedOpenPositions
--Order By AccountName,Symbol 
                              
Select                         
AccountName,                              
Symbol,                               
Qty,                              
TradeDate,           
AvgPrice  ,                         
BBCode ,                       
ISIN,                      
CUSIP,                      
SEDOL,                             
FullSecurityName,                       
ForexRate,            
OSISymbol ,            
Asset  ,          
UDASectorName,        
CurrencySymbol,           
Issuer                       
From #Temp_GroupedOpenPositions                              
                   
                  
Union All                  
                  
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
'',  
''                 
From PM_CompanyFundCashCurrencyValue FCC 
Inner Join @Fund Fund on Fund.FundID = FCC.FundID                    
Inner Join T_CompanyFunds CF On CF.CompanyFundID = FCC.FundID                  
Inner Join T_Currency CURR On CURR.CurrencyID = FCC.LocalCurrencyID           
Where DateDiff(Day,FCC.Date,@InputDate) = 0                  
Group By CF.FundName,CURR.CurrencySymbol                             
                              
Drop Table #TEMPFundPositionsForDate,#SecMasterDataTempTable,#TemP_TaxlotPK,#Temp_GroupedOpenPositions
          
End 
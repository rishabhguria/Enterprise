                     
/*                
exec P_GetOpenPositions_Robinson @thirdPartyID=27,@companyFundIDs=N'1184,1183,1182,1185',@inputDate='2015-10-12 15:26:19:000',@companyID=5,@auecIDs=N'1,15',@TypeID=0,@dateType=0,@fileFormatID=47              
*/                
                    
                     
CREATE Procedure [dbo].[P_GetOpenPositions_Robinson]              
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
--Set @ThirdPartyID =   27                                
--Set @companyFundIDs = '1247'--,1235,1240,1247'                                                                                                                                                               
--Set @inputDate =   '2015-10-14'                                                                                                                                                         
--Set @companyID =   5                                                                                                                         
--Set @auecIDs =  '1,15'                                                                
--Set @TypeID =     0                             
--Set @DateType =  0                                                                                                                                                               
--Set @fileFormatID = 63               
                
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
                      
Create Table #SecMasterDataTempTable                                                                                                                                                                                                                          
        
              
(                                                                                                                                                                                   
TickerSymbol Varchar(100),                                                      
UnderlyingSymbol varchar(100),                    
BloombergSymbol varchar(100),                
ISINSymbol varchar(100),              
CUSIPSymbol varchar(100),              
SEDOLSymbol varchar(100),                  
CompanyName varchar(100) ,  
SectorName Varchar(100)        
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
SectorName                  
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
SectorName Varchar(100)                                                                                                                                              
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
SectorName  
)                           
                                                                                
 Select                                                                                                             
PT.TaxLotID as TaxLotID,                                                                                        
PT.Symbol as Symbol ,                                                                                                          
(PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)) as TaxLotOpenQty ,                                        
PT.AvgPrice as AvgPrice ,                                                                                                                                                                                    
Funds.FundName as Account,                                                                                                     
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
SMD.SectorName AS UDASectorName                                                                             
                      
from PM_Taxlots PT                 
inner Join V_SecMasterData SM on  PT.Symbol = SM.TickerSymbol                                                                                                                               
Inner join  T_Group G on G.GroupID=PT.GroupID                                  
inner join T_Asset Asset on Asset.AssetId= G.AssetID                       
inner join T_Companyfunds Funds on Funds.Companyfundid = PT.FundID           
inner join @Fund Fund on Fund.FundID = PT.FundID    
Inner JOIN #SecMasterDataTempTable AS SMD ON SMD.TickerSymbol = PT.Symbol                         
Where PT.Taxlot_PK in                               
(                                                                                        
 Select Max(Taxlot_PK) from PM_Taxlots                                                                           
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                              
 group by TaxlotId                                                                   )                                                                                  
and PT.TaxLotOpenQty <> 0                  
          
                      
Select                 
Fund as AccountName,                      
Symbol,                       
SUM(OpenQuantity) as Qty,                      
@InputDate as TradeDate,                      
MAX(BloombergSymbol)  as BBCode ,               
MAX(ISINSymbol) as ISIN,              
MAX(CUSIPSymbol) as CUSIP,              
MAX(SEDOLSymbol) as SEDOL,                     
MAX(CompanyName) as FullSecurityName,               
MAX(isnull(FXRate,0)) as ForexRate,    
MAX(OSISymbol) as OSISymbol ,    
MAX(Asset) as Asset  ,  
max(SectorName) AS UDASectorName   
               
from #TEMPFundPositionsForDate                      
group by Fund,Symbol            
          
Union All          
          
Select           
CF.FundName As AccountName,           
CURR.CurrencySymbol As Symbol,          
Sum(FCC.CashValueBase) As Qty,          
@InputDate as TradeDate,           
CURR.CurrencySymbol + ' Curncy'  as BBCode ,               
CURR.CurrencySymbol as ISIN,              
CURR.CurrencySymbol as CUSIP,              
CURR.CurrencySymbol as SEDOL,                     
CURR.CurrencySymbol as FullSecurityName,               
0 as ForexRate  ,    
CURR.CurrencySymbol as OSISymbol,    
'Cash',  
''         
           
from PM_CompanyFundCashCurrencyValue FCC          
Inner Join T_CompanyFunds CF On CF.CompanyFundID = FCC.FundID          
Inner Join T_Currency CURR On CURR.CurrencyID = FCC.BaseCurrencyID          
Inner Join @Fund Fund on Fund.FundID = FCC.FundID      
Where DateDiff(Day,FCC.Date,@InputDate) = 0          
Group By CF.FundName,CURR.CurrencySymbol                     
                      
drop table #TEMPFundPositionsForDate,#SecMasterDataTempTable  
End 
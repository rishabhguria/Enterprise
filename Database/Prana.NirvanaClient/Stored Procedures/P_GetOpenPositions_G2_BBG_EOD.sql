                         
                             
CREATE Procedure [dbo].[P_GetOpenPositions_G2_BBG_EOD]                        
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
                         
--set @thirdPartyID=39  
--set @companyFundIDs=N'1240,1239,1246,1247,1234,1238,1244,1245,1242,1243'  
--set @inputDate='2025-04-11 10:53:34'  
--set @companyID=7  
--set @auecIDs=N'63,44,34,43,59,31,54,18,161,61,74,103,1,15,11,62,73,12,32,81'  
--set @TypeID=0  
--set @dateType=0  
--set @fileFormatID=42                        
                    
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
Issuer Varchar(200)                 
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
IsNull(Issuer,'Undefined') As Issuer                            
 From V_SecMasterData SM            
left OUTER join V_UDA_DynamicUDA UDA WITH(NOLOCK) On UDA.Symbol_PK = SM.Symbol_PK                  
                              
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
Issuer Varchar(200) ,    
PositionSide Varchar(10) ,  
MasterFund varchar(200),  
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
Issuer ,    
PositionSide,  
MasterFund  
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
SMD.SectorName AS UDASectorName,          
TC.CurrencySymbol as CurrencySymbol,    
SMD.Issuer,    
Case                          
 When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                          
 Then 'Long'                          
 Else 'Short'                          
End as PositionSide,  
CF.MasterFundName As MasterFund  
                                
from PM_Taxlots PT                 
inner Join V_SecMasterData SM WITH(NOLOCK) On  PT.Symbol = SM.TickerSymbol                                                                                                                                         
Inner join  T_Group G WITH(NOLOCK) On G.GroupID=PT.GroupID                                            
Inner join T_Currency TC WITH(NOLOCK) On TC.CurrencyID = G.CurrencyID          
inner join T_Asset Asset WITH(NOLOCK) On Asset.AssetId= G.AssetID                                 
inner join T_Companyfunds Funds WITH(NOLOCK) On Funds.Companyfundid = PT.FundID                     
inner join @Fund Fund on Fund.FundID = PT.FundID   
Inner Join #Fund CF With (NoLock) On CF.FundID = PT.FundID  
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
Max(Issuer) As Issuer,    
PositionSide,  
MasterFund  
from #TEMPFundPositionsForDate                                
group by Fund,Symbol,PositionSide,MasterFund                    
                    
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
'',    
'',  
MCF.MasterFundName AS MasterFund  
                     
from PM_CompanyFundCashCurrencyValue FCC                    
Inner Join T_CompanyFunds CF WITH(NOLOCK) On CF.CompanyFundID = FCC.FundID            
Inner Join T_Currency CURR WITH(NOLOCK) On CURR.CurrencyID = FCC.LocalCurrencyID                    
Inner Join @Fund Fund on Fund.FundID = FCC.FundID   
Inner Join #Fund MCF With (NoLock) On MCF.FundID = FCC.FundID  
Where DateDiff(Day,FCC.Date,@InputDate) = 0                    
Group By CF.FundName,CURR.CurrencySymbol,MCF.MasterFundName                             
                                
drop table #TEMPFundPositionsForDate,#SecMasterDataTempTable            
End 
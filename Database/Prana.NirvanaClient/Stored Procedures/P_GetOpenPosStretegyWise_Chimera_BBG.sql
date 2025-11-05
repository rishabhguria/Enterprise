/*                      
exec P_GetOpenPosStretegyWise_Chimera_BBG @inputDate='07-28-2020'  
*/                      
                          
CREATE Procedure [dbo].[P_GetOpenPosStretegyWise_Chimera_BBG]                    
(                             
@InputDate Datetime                                                                                                                                                                
)                              
AS                                                                                               
Begin                       
  
 SET NOCOUNT ON;
                     
--Declare @inputDate datetime 
--Set @inputDate =   '07-28-2020' 
    

Declare @Fund Table                                                         
(              
FundID int                    
)                        
               
Insert into @Fund                                                                                                  
Select CompanyFundID From T_CompanyFunds 
                                                                                                  
  
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
  
  
SELECT PT.Taxlot_PK  
InTo #TempTaxlotPK       
FROM PM_Taxlots PT With (NoLock)       
Inner Join @Fund Fund on Fund.FundID = PT.FundID            
Where PT.Taxlot_PK in                                     
(                                                                                              
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                 
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                    
 group by TaxlotId                                                                    
)                                                                                        
And PT.TaxLotOpenQty <> 0 and PT.Symbol!='ZVZZT'
  
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
 StrategyShortName Varchar(100),
 Long_Short Varchar(10)                
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
 StrategyShortName,
 Long_Short     
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
 CS.StrategyShortName,
 Case
	 When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1
	 Then 'Long'
	 Else 'Short'
 End As Long_Short                                                                          
                            
from PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK            
Inner join  T_Group G With (NoLock) on G.GroupID=PT.GroupID                                        
Inner join T_Currency TC on TC.CurrencyID = G.CurrencyID      
inner join T_Asset Asset on Asset.AssetId= G.AssetID                             
inner join T_Companyfunds Funds With (NoLock) on Funds.Companyfundid = PT.FundID                 
Inner JOIN #SecMasterDataTempTable AS SM ON SM.TickerSymbol = PT.Symbol 
Left Outer Join T_CompanyStrategy CS On CS.CompanyStrategyID = PT.Level2ID                              
Order By  Funds.FundName, CS.StrategyShortName, PT.Symbol               
  
--- Open Positions
Select 
Fund As PortfolioName,  
Case 
	When Asset = 'Equity' And Max(Symbol) = '' And Max(CurrencySymbol) = 'USD'
	Then Symbol + ' US'+ ' EQUITY'
	When Asset = 'EquityOption'
	Then MAX(OSISymbol)
	Else Max(Symbol)
End As [Security], 
SUM(OpenQuantity) as Position,
StrategyShortName As Strategy

From #TEMPFundPositionsForDate  
Group By Fund,StrategyShortName, Asset, Symbol, Long_Short     

Drop Table #TEMPFundPositionsForDate,#SecMasterDataTempTable,#TempTaxlotPK        
End
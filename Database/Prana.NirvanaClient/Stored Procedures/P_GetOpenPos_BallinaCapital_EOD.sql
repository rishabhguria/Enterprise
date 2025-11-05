 --https://jira.nirvanasolutions.com:8443/browse/ONB-6538    
 
CREATE Procedure [dbo].[P_GetOpenPos_BallinaCapital_EOD]                            
(                                     
@ThirdPartyID int,                                                
@CompanyFundIDs varchar(max),                                                                                                                                                                              
@InputDate datetime,                                                                                                                                                                          
@CompanyID int,                                                                                                                                          
@AUECIDs varchar(max),                                                                                
@TypeID int,  
@DateType int,
@FileFormatID int                                      
)                                      
AS          
    
    
Declare @Fund Table                                                               
(                    
FundID int                          
)      
    
Insert into @Fund                                                                                                        
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')     
    
    
SELECT PT.Taxlot_PK        
InTo #TempTaxlotPK             
FROM PM_Taxlots PT With (NOLock)              
Inner Join @Fund Fund on Fund.FundID = PT.FundID                  
Where PT.Taxlot_PK in                                           
(                                                                                                    
 Select Max(Taxlot_PK) from PM_Taxlots With (NOLock)                                                                                      
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                          
 group by TaxlotId                             
)                                                                                              
And PT.TaxLotOpenQty > 0           
        
Select                   
PT.Symbol,                            
Curr.CurrencySymbol As LocalCurrency,    
PT.TaxlotOpenQty As OpenPositions,    
SM.Multiplier As AssetMultiplier, 
SM.ISINSymbol As ISINSymbol,
SM.SEDOLSymbol As SEDOLSymbol,
SM.CUSIPSymbol As CUSIPSymbol, 
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost_Local,     
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier  

Into #TempOpenPositionsTable             
From PM_Taxlots PT With (NOLock)   
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK     
Inner Join V_SecMasterData SM With (NOLock) On SM.TickerSymbol = PT.Symbol    
Inner Join T_Currency Curr With (NOLock) On Curr.CurrencyID = SM.CurrencyID      
Where PT.TaxlotOpenQty > 0    
    
    
Select                   
Temp.Symbol as Symbol,       
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions,      
Max(LocalCurrency) As LocalCurrency,       
Sum(Temp.TotalCost_Local) As TotalCost_Local,     
Max(ISINSymbol) As ISINSymbol,
max(CUSIPSymbol)As CUSIPSymbol,
Max(SEDOLSymbol) As SEDOLSymbol

Into #TempTable                  
From #TempOpenPositionsTable Temp      
Group By Temp.symbol
    
Alter Table #TempTable          
Add UnitCostLocal Float Null,
  OrderSide Varchar(10) Null        
          
UPdate #TempTable          
Set UnitCostLocal = 0.0          
    
    
Update #TempTable      
Set UnitCostLocal  = Abs(TotalCost_Local/OpenPositions)      
Where OpenPositions <> 0  

UPdate #TempTable
Set OrderSide = 
Case
	When OpenPositions > 0
	Then 'Long'
	Else 'Short'  
End       
      
Select *, CONVERT(VARCHAR(10), @InputDate, 101) AS TradeDate
From #TempTable
Order By Symbol, OrderSide  
 
Drop Table #TempOpenPositionsTable, #TempTable,#TempTaxlotPK    

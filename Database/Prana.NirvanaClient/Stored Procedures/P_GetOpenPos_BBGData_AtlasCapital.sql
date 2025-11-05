  
  
CREATE Procedure [dbo].[P_GetOpenPos_BBGData_AtlasCapital]                          
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
      
--Declare @InputDate DateTime      
--Set @InputDate = '07-22-2017'  
  
--Declare @CompanyFundIDs varchar(max)   
--Set @companyFundIDs = '1257'     
  
Declare @Fund Table                                                             
(                  
FundID int                        
)    
  
Insert into @Fund                                                                                                      
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')   
  
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
And PT.TaxLotOpenQty > 0         
      
Select                 
CF.FundName As AccountName,            
PT.Symbol,                
Case                        
 When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                        
 Then 'Long'                        
 Else 'Short'                        
End as PositionIndicator,    
PT.TaxlotOpenQty As OpenPositions,  
SM.BloombergSymbol,  
Curr.CurrencySymbol As LocalCurrency,  
SM.Multiplier As AssetMultiplier,   
SM.CompanyName As SecurityDescription,  
SM.SEDOLSymbol,  
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier  
  
Into #TempOpenPositionsTable           
From PM_Taxlots PT  
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK   
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol  
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID    
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID  
Inner Join T_Asset A On A.AssetID = SM.AssetID  
INNER JOIN T_Group G ON G.GroupID = PT.GroupID   
Where PT.TaxlotOpenQty > 0  
  
  
Select                 
Temp.AccountName As AccountName,                
Temp.Symbol,                
Temp.PositionIndicator As PositionIndicator,     
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions,   
CONVERT(VARCHAR(10), @InputDate, 101) As TradeDate,  
Max(BloombergSymbol) As BloombergSymbol,   
Max(LocalCurrency) As LocalCurrency,   
Max(Temp.AssetMultiplier) As AssetMultiplier,        
Max(SecurityDescription) As SecurityDescription,   
Max(SEDOLSymbol) As SEDOLSymbol  
  
Into #TempTable                
From #TempOpenPositionsTable Temp    
Group By Temp.AccountName,Temp.Symbol,Temp.PositionIndicator      
        
Select * from #TempTable   
Order By AccountName,Symbol,PositionIndicator    
  
Drop Table #TempOpenPositionsTable, #TempTable  
Drop Table #TempTaxlotPK
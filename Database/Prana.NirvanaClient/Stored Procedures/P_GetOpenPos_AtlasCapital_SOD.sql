  
  
CREATE Procedure [dbo].[P_GetOpenPos_AtlasCapital_SOD]                          
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
  
  
--declare  
--@ThirdPartyID int,                                              
--@CompanyFundIDs varchar(max),                                                                                                                                                                            
--@InputDate datetime,                                                                                                                                                                        
--@CompanyID int,                                                                                                                                        
--@AUECIDs varchar(max),                                                                              
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                              
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                              
--@FileFormatID int    
  
-- set @thirdPartyID=57  
-- set @companyFundIDs=N'4,'  
-- set @inputDate='2025-01-21 04:53:38.293'  
-- set @companyID=7  
-- set @auecIDs=N'18,114,1,15,11,80,'  
-- set @TypeID=1  
--  set @dateType=1  
--  set @fileFormatID=122      
      
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
  
-- get Mark Price for End Date                          
CREATE TABLE #MarkPriceForEndDate     
(      
  Finalmarkprice Float      
 ,Symbol Varchar(100)      
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
Inner Join @Fund F On F.FundID = DMP.FundID    
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0   
  
SELECT PT.Taxlot_PK      
InTo #TempTaxlotPK           
FROM PM_Taxlots PT With (NoLock)           
Inner Join @Fund Fund on Fund.FundID = PT.FundID                
Where PT.Taxlot_PK in                                         
(                                                                                                  
 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                    
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
SM.ISINSymbol,  
SM.CUSIPSymbol,  
SM.OSISymbol,  
SM.AssetName,  
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost_Local,   
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,  
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier  
  
Into #TempOpenPositionsTable           
From PM_Taxlots PT With (NoLock)  
Inner Join #TempTaxlotPK Temp With (NoLock) On Temp.Taxlot_PK = PT.Taxlot_PK   
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol  
Inner Join T_Currency Curr With (NoLock) On Curr.CurrencyID = SM.CurrencyID    
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = PT.FundID  
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID  
INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID   
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID)   
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
Max(MarkPrice) As MarkPrice,  
Max(SEDOLSymbol) As SEDOLSymbol,  
Max(ISINSymbol) As ISINSymbol,  
Max(CUSIPSymbol) As CUSIPSymbol,  
Max(AssetName) As UDAAssetClass,  
Sum(TotalCost_Local) AS TotalCost_Local  
  
Into #TempGroupedOpenPosTable                
From #TempOpenPositionsTable Temp    
Group By Temp.AccountName,Temp.Symbol,Temp.PositionIndicator    
  
Alter Table #TempGroupedOpenPosTable        
Add UnitCost Float Null    
  
  
Alter Table #TempGroupedOpenPosTable        
Add UnitCostLocal Float Null      
        
UPdate #TempGroupedOpenPosTable        
Set UnitCost = 0.0     
  
  
UPdate #TempGroupedOpenPosTable        
Set UnitCostLocal = 0.0       
        
UPdate #TempGroupedOpenPosTable        
Set UnitCost =         
Case          
 When OpenPositions <> 0 And AssetMultiplier <> 0          
 Then (TotalCost_Local/OpenPositions) /AssetMultiplier          
 Else 0          
End    
    
--UPdate #TempGroupedOpenPosTable        
--Set UnitCostLocal = FXRate * (        
--Case          
-- When OpenPositions <> 0 And AssetMultiplier <> 0          
-- Then (TotalCost_Local/OpenPositions) /AssetMultiplier          
-- Else 0          
--End   
--)     
        
Select * from #TempGroupedOpenPosTable   
Order By AccountName,Symbol,PositionIndicator    
  
Drop Table #TempOpenPositionsTable, #TempGroupedOpenPosTable  
Drop Table #TempTaxlotPK
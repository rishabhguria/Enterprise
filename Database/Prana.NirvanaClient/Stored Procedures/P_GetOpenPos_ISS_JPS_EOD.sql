CREATE Procedure [dbo].[P_GetOpenPos_ISS_JPS_EOD]                                
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
  
--Declare @ThirdPartyID int,                                                    
--@CompanyFundIDs varchar(max),                                                                                                                                                                                  
--@InputDate datetime,                                                                                                                                                                              
--@CompanyID int,                                                                                                                                              
--@AUECIDs varchar(max),                                                                                    
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                    
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                    
--@FileFormatID int  
  
----MEDP  
  
--Set @thirdPartyID=66 --Set @companyFundIDs='11'--N'2,3,4,5,6,7,8,9,10,11,' --Set @inputDate='2025-01-15' --Set @companyID=7 --Set @auecIDs=N'44,43,59,54,21,1,15,11,62,73,32,81,' --Set @TypeID=1 --Set @dateType=1 --Set @fileFormatID=132
Declare @Fund Table                                                                   
(                        
FundID int                              
)          
        
Insert into @Fund                                                                                                            
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')         
  
  
Create Table #TempTaxlotPK  
(  
Taxlot_PK BigInt  
)  
  
Create Table #Temp_OpenDataAsOfDate  
(  
FundID Int,  
Symbol Varchar(100),  
Quantity Float,  
OpenSide Varchar(20),  
Price Float,  
CommissionAndFees Float  
)  
  
Create Table #Temp_ClosedDataAsOfDate  
(  
FundID Int,  
Symbol Varchar(100),  
Quantity Float,  
OpenSide Varchar(20),  
Price Float,  
CommissionAndFees Float  
)  
  
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
FROM PM_DayMarkPrice DMP WITH(NOLOCK)        
Inner Join @Fund F On F.FundID = DMP.FundID          
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0         
   
Insert InTo #TempTaxlotPK  
SELECT PT.Taxlot_PK                 
FROM PM_Taxlots PT WITH(NOLOCK)                 
Inner Join @Fund Fund on Fund.FundID = PT.FundID         
Where PT.Taxlot_PK in                                               
(                                                                                                        
 Select Max(PTInner.Taxlot_PK) from PM_Taxlots PTInner WITH(NOLOCK)    
 INNER JOIN T_Group GInner WITH(NOLOCK) ON PTInner.GroupID = GInner.GroupID    
 Where DateDiff(Day,GInner.SettlementDate ,@InputDate) >= 0        
 group by PTInner.TaxlotId                                                                              
)                                                                                                  
And PT.TaxLotOpenQty > 0     
  
Insert InTo #Temp_OpenDataAsOfDate  
Select  
PT.FundID,  
PT.Symbol,  
PT.TaxLotOpenQty As Quantity,  
PT.OrderSideTagValue As OpenSide,  
PT.AvgPrice As Price,  
PT.OpenTotalCommissionandFees As CommissionAndFees  
From PM_Taxlots PT With (NoLock)        
Inner Join #TempTaxlotPK Temp With (NoLock) On Temp.Taxlot_PK = PT.Taxlot_PK         
Where PT.TaxlotOpenQty > 0   
--And PT.Symbol = 'MEDP'  
  
--Select *  
--From #Temp_OpenDataAsOfDate  
   
  
Insert InTo #Temp_ClosedDataAsOfDate  
Select   
PT.FundID As FundID,  
PT.Symbol,  
ClosedQty As Quantity,  
PT.OrderSideTagValue As OpenSide,   
PT.AvgPrice As Price,  
PT.OpenTotalCommissionandFees As CommissionAndFees  
  
From PM_TaxlotClosing  PTC                                                   
Inner Join PM_Taxlots PT on (PTC.PositionalTaxlotID=PT.TaxlotID And PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                                                                                              
Inner Join PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID And PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)                                                                                                                                             
 
Inner Join @Fund Fund on Fund.FundID = PT.FundID                                                                                                   
Where DateDiff(d,@InPutDate,PTC.AUECLocalDate) = 0    
--DateDiff(d,@InPutDate,PTC.AUECLocalDate) >=0                                                                                                                                   
--and  DateDiff(d,PTC.AUECLocalDate,@InPutDate)>=0                                                                                                                     
--And PT.Symbol = 'MEDP'  
  
  Select *  
  InTo #Temp_Total_Qty_Account_Symbol  
  From #Temp_OpenDataAsOfDate  
  
  Insert InTo #Temp_Total_Qty_Account_Symbol  
  Select *  
  From #Temp_ClosedDataAsOfDate  
  
 --Select * From #Temp_Total_Qty_Account_Symbol   
   
 Select                       
CF.FundName As AccountName,                  
T.Symbol,                      
Case                              
 When dbo.GetSideMultiplier(T.OpenSide) = 1                              
 Then 'Long'                              
 Else 'Short'                              
End As PositionIndicator,          
T.Quantity As OpenPositions,        
SM.BloombergSymbol,        
Curr.CurrencySymbol As LocalCurrency,        
SM.Multiplier As AssetMultiplier,         
SM.CompanyName As SecurityDescription,        
SM.SEDOLSymbol,        
SM.ISINSymbol,        
SM.CUSIPSymbol,        
SM.OSISymbol,        
SM.AssetName,        
SM.CountryName,        
(T.Quantity * T.Price * SM.Multiplier * dbo.GetSideMultiplier(T.OpenSide)) + (T.CommissionAndFees) TotalCost_Local,         
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,        
dbo.GetSideMultiplier(T.OpenSide) As SideMultiplier        
Into #Temp_OpenPositionsAsOfSettleDateTable                       
From #Temp_Total_Qty_Account_Symbol T       
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = T.Symbol        
Inner Join T_Currency Curr With (NoLock) On Curr.CurrencyID = SM.CurrencyID          
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = T.FundID        
--Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID        
--INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID         
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (T.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = T.FundID)         
--Where T.Quantity > 0   
  
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
Max(CountryName) AS UDACountery        
        
Into #TempGroupedOpenPosTable                      
From #Temp_OpenPositionsAsOfSettleDateTable Temp          
Group By Temp.AccountName,Temp.Symbol,Temp.PositionIndicator           
              
Select * from #TempGroupedOpenPosTable         
Order By AccountName,Symbol,PositionIndicator   
   
Drop Table #MarkPriceForEndDate, #TempTaxlotPK  
Drop Table #Temp_OpenDataAsOfDate, #Temp_ClosedDataAsOfDate , #Temp_Total_Qty_Account_Symbol  
Drop Table #Temp_OpenPositionsAsOfSettleDateTable,#TempGroupedOpenPosTable     
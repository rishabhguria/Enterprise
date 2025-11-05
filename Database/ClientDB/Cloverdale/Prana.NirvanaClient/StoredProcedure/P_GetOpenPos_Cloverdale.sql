/*  
exec [P_GetOpenPos_Cloverdale] @thirdPartyID=86,@companyFundIDs=N'1257',  
@inputDate='07-22-2017',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167  
*/  
  
CREATE Procedure [dbo].[P_GetOpenPos_Cloverdale]                          
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
       
  
Declare @Fund Table                                                             
(                  
FundID int                        
)    
  
Insert into @Fund                                                                                                      
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')   
  
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
Inner Join @Fund F On F.FundID = DMP.FundID    
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0    
  
-- get forex rates for 2 date ranges            
CREATE TABLE #FXConversionRates (  
 FromCurrencyID INT  
 ,ToCurrencyID INT  
 ,RateValue FLOAT  
 ,ConversionMethod INT  
 ,DATE DATETIME  
 ,eSignalSymbol VARCHAR(max)  
 ,FundID INT  
 )  
  
INSERT INTO #FXConversionRates  
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @InputDate  
 ,@InputDate  
  
UPDATE #FXConversionRates  
SET RateValue = 1.0 / RateValue  
WHERE RateValue <> 0  
 AND ConversionMethod = 1  
  
-- Delete records for FundID = 0, Contellation maintained Fundwise FX Rate  
Delete #FXConversionRates  
WHERE fundID = 0  
  
--Select * From #FXConversionRates  
--Where FundID = 1257  
  
  
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
Case   
 When G.IsSwapped  = 1  
 Then 'EquitySwap'  
 Else A.AssetName   
End As AssetClass,  
SM.ISINSymbol,  
SM.SEDOLSymbol,  
SM.OSISymbol,  
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,  
SM.LeadCurrency,  
SM.VsCurrency,  
SM.CUSIPSymbol,  
SM.ExpirationDate,  
SM.PutOrCall,  
SM.UnderLyingSymbol,  
PT.AvgPrice,  
SM.StrikePrice,  
Ex.DisplayName  
Into #TempOpenPositionsTable           
From PM_Taxlots PT  
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK   
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol  
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID    
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID  
Inner Join T_Asset A On A.AssetID = SM.AssetID  
INNER JOIN T_Group G ON G.GroupID = PT.GroupID  
Inner join T_Exchange Ex ON Ex.ExchangeID = G.ExchangeID   
Where PT.TaxlotOpenQty > 0  
  
  
Select                 
Temp.AccountName As AccountName,                
Temp.Symbol,                
Temp.PositionIndicator As PositionIndicator,     
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions,   
Max(BloombergSymbol) As BloombergSymbol,   
Max(LocalCurrency) As LocalCurrency,   
Max(Temp.AssetMultiplier) As AssetMultiplier,        
Max(SecurityDescription) As SecurityDescription,  
AssetClass As AssetClass,  
Max(ISINSymbol) As ISINSymbol,  
Max(SEDOLSymbol) As SEDOLSymbol,  
Max(CUSIPSymbol) As CUSIPSymbol,  
Max(AvgPrice) As AvgPrice,  
Max(DisplayName) As Exchange,  
Max(PutOrCall) As PutOrCall,  
Max(StrikePrice) As StrikePrice,  
Max(UnderLyingSymbol) As UnderLyingSymbol,  
Max(OSISymbol) As OSISymbol,  
Temp.ExpirationDate    
Into #TempTable                
From #TempOpenPositionsTable Temp    
Group By Temp.AccountName,Temp.Symbol,Temp.PositionIndicator,Temp.AssetClass,temp.ExpirationDate  
  
Select * from #TempTable   
Order By AccountName,Symbol,PositionIndicator    
  
  
Drop Table #TempOpenPositionsTable, #TempTable  
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates
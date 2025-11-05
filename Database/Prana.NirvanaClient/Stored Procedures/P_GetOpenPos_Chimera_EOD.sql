
/*
exec [P_GetOpenPos_Conste_Shelterhaven] @thirdPartyID=86,@companyFundIDs=N'1257',
@inputDate='07-22-2017',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167
*/

CREATE Procedure [dbo].[P_GetOpenPos_Chimera_EOD]                        
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
WHERE DateDiff(Day,DMP.[Date],@InputDate) = 0


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
Case 
	When G.IsSwapped  = 1
	Then 'EquitySwap'
	Else A.AssetName 
End As AssetClass, 
Case                      
	When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                      
	Then 'Long'                      
	Else 'Short'                      
End As LongShort,
SM.PutOrCall As PutCall,
PT.Symbol,
SM.UnderLyingSymbol,
PT.TaxlotOpenQty As Quantity,
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,
SM.Multiplier,
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
CF.FundName,
SM.SEDOLSymbol AS SEDOL,
SM.BloombergSymbol AS BloombergSymbol,
TC.CurrencySymbol AS SettlCurrency,
Curr.CurrencySymbol AS TradeCurrency

Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID  
INNER JOIN T_Currency AS TC ON TC.CurrencyID = PT.SettlCurrency
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
Inner Join T_Group G ON G.GroupID = PT.GroupID
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID)  
Where PT.TaxlotOpenQty > 0


Select               
Temp.AssetClass,    
Temp.LongShort,
Temp.PutCall,        
Temp.Symbol,
Temp.UnderLyingSymbol,
Sum(Temp.Quantity * Temp.SideMultiplier) As OpenPositions,
Max(MarkPrice) As MarkPrice,
Max(Temp.Multiplier) As Multiplier,
Temp.FundName As AccountName,
Temp.SEDOL,
Temp.BloombergSymbol,
Temp.SettlCurrency,
Temp.TradeCurrency
Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.FundName,Temp.PutCall,Temp.Symbol,Temp.LongShort,Temp.AssetClass,Temp.UnderLyingSymbol,
Temp.SEDOL,Temp.BloombergSymbol,Temp.SettlCurrency,Temp.TradeCurrency           
      
Select * from #TempTable 
Order By AccountName,Symbol,LongShort,PutCall,UnderLyingSymbol

Drop Table #TempOpenPositionsTable, #TempTable
Drop Table #TempTaxlotPK,#MarkPriceForEndDate

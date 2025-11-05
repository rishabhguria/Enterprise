/*

JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-5768

DESC: Constellation-Tareo account integration
*/

CREATE Procedure [dbo].[P_GetOPS_Neapen_Constellation_EOD]                        
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
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0  

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
SM.ISINSymbol As ISIN,
SM.CUSIPSymbol AS CUSIP, 
PT.TaxLot_PK AS TaxLot_PK,
Case 
	When G.IsSwapped  = 1
	Then 'EquitySwap'
	Else A.AssetName 
End As AssetClass,

dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
IsNull(MP_FundWise.Finalmarkprice,0) As MarkPrice,
PT.AUECModifiedDate AS TardeDate,
G.SettlementDate AS SettlementDate,
PT.OrderSideTagValue

Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
INNER JOIN T_Group G ON G.GroupID = PT.GroupID
LEFT OUTER JOIN #MarkPriceForEndDate MP_FundWise ON (PT.Symbol = MP_FundWise.Symbol And MP_FundWise.FundID = PT.FundID) 

Where PT.TaxlotOpenQty > 0


Select 
Cast(Max(Temp.TaxLot_PK) As Varchar(20))  as TicketNumber,           
Temp.AccountName As AccountName,              
Temp.Symbol,              
Temp.PositionIndicator As PositionIndicator,   
Sum(Temp.OpenPositions * Temp.SideMultiplier) As OpenPositions, 
Max(BloombergSymbol) As BloombergSymbol, 
Max(Temp.AssetMultiplier) As AssetMultiplier,      
Max(ISIN) As ISIN,
Max(CUSIP) AS CUSIP,
Max(MarkPrice) As MarkPrice,
Max(@InputDate) AS TradeDate,
Max(@InputDate) as SettlementDate,
AssetClass As AssetClass,
Temp.LocalCurrency AS LocalCurrency

Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.AccountName,Temp.Symbol,Temp.PositionIndicator,AssetClass,Temp.LocalCurrency
  
Select * from #TempTable 
Order By AccountName,Symbol,PositionIndicator  


Drop Table #TempOpenPositionsTable, #TempTable
Drop Table #TempTaxlotPK,#MarkPriceForEndDate
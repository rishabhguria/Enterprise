      
CREATE Procedure [dbo].[P_GetOpenPositions_Kettlehill]                                
(                                         
@ThirdPartyID int,                                                    
@CompanyFundIDs varchar(max),                                                                                                                                                                                  
@InputDate DateTime,                                                                                                                                                                              
@CompanyID Int,                                                                                                                                              
@AUECIDs varchar(max),                                                                                    
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                    
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                    
@FileFormatID int                                          
)                                          
AS        
      
--Declare @ThirdPartyID int                                                    
--Declare @CompanyFundIDs varchar(max)      
--Declare @InputDate DateTime                                                                                                                                                                         
--Declare @CompanyID int      
--Declare @AUECIDs varchar(max)      
--Declare @TypeID int      
--Declare @DateType int      
--Declare @FileFormatID int        
      
      
--Set @ThirdPartyID = 93      
--Set @CompanyFundIDs = '18,16,17,13,1,2,11,3,4,12,8,9,10,14,15,5,6,7,19'      
--Set @InputDate = '12/19/2017'      
--Set @CompanyID =7      
--Set @AUECIDs = '63,34,43,56,59,47,21,18,1,15,11,62,73,80,81'      
--Set @TypeID = 0      
--Set @DateType = 0      
--Set @FileFormatID = 179      
      
DECLARE @Fund TABLE (FundID INT)      
      
INSERT INTO @Fund      
SELECT Cast(Items AS INT)      
FROM dbo.Split(@companyFundIDs, ',')      
      
DECLARE @AUEC TABLE (AUECID INT)      
      
INSERT INTO @AUEC      
SELECT Cast(Items AS INT)      
FROM dbo.Split(@AUECIDs, ',')      
      
      
SELECT       
Max(Taxlot_PK) As Taxlot_PK           
InTo #TempTaxlotPK           
From PM_Taxlots              
INNER JOIN @Fund F On F.FundID =  PM_Taxlots.FundId                   
Where DateDiff(Day,PM_Taxlots.AUECModifiedDate,@InputDate) >= 0           
GROUP BY TaxlotId        
      
Select             
FC.FundName As AccountName,            
PM.Symbol,            
PM.TaxLotOpenQty * dbo.GetSideMultiplier(PM.OrderSideTagValue) As Qty,            
SM.Multiplier As Multiplier,            
SM.OSISymbol As OSISymbol,      
@InputDate as TradeDate,        
A.AssetName as Asset,        
SM.BloombergSYmbol as BloombergSYmbol,        
SM.CUSIPSymbol as CUSIPSymbol,        
SM.SEDOLSymbol as SEDOLSymbol,        
TC.CurrencySymbol as Currency,        
SM.CompanyName as SecurityName,        
Case dbo.GetSideMultiplier(PM.OrderSideTagValue)                          
 When 1                          
 Then 'Long'                          
Else 'Short'                          
End As Side        
       
Into #TempTable       
      
From PM_Taxlots PM         
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PM.Taxlot_PK        
inner join T_Companyfunds FC on FC.CompanyFundID= PM.FundID      
inner join V_secmasterdata SM on PM.Symbol = SM.TickerSymbol      
Inner Join T_Asset A On A.AssetID = SM.AssetID       
inner join T_Currency TC on TC.CurrencyID =SM.CurrencyID       
Where TaxLotOpenQty<>0       
      
Select       
AccountName,      
Symbol,            
Sum(Qty) As Qty,            
Max(OSISymbol) As OSISymbol,        
Replace(convert(varchar, @InputDate, 102),'.','-') as TradeDate,        
--@InputDate as TradeDate,        
Max(Asset) as Asset,        
Max(BloombergSYmbol) as BloombergSYmbol,        
Max(CUSIPSymbol) as CUSIPSymbol,        
Max(SEDOLSymbol) as SEDOLSymbol,        
Max(Currency) as Currency,        
Max(SecurityName) as SecurityName,        
--Max(Side) As Side        
Side    
From #TempTable      
Group by AccountName, Symbol, Side      
Order By AccountName, Symbol, Side      
      
Drop Table #TempTaxlotPK,#TempTable 
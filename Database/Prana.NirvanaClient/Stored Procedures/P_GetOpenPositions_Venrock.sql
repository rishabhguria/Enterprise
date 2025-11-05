      
      
            
CREATE Procedure [dbo].[P_GetOpenPositions_Venrock]                                      
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
            
            
      
--Set @thirdPartyID=37      
--Set @companyFundIDs=N'9,13,12,11,3,8,2,10,1,14,4,5,6'      
--Set @inputDate='2019-07-19 15:23:24'      
--Set @companyID=7      
--Set @auecIDs=N'20,30,63,182,44,34,43,56,53,59,31,54,45,21,60,18,180,1,15,62,73,32,81'      
--Set @TypeID=0      
--Set @dateType=0      
--Set @fileFormatID=102           
            
Select   
TickerSymbol ,  
UnderLyingSymbol ,  
BloombergSymbol   
InTo #Temp_SMUnderlying  
From V_SecMasterData_WithUnderlying With (NoLock)        
            
Select distinct Symbol      
InTo #Temp      
From PM_Taxlots With (NoLock)        
Where Taxlot_PK In                                                                                                  
(                                                                                                         
   Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)      
   Where DateDiff(d,PM_Taxlots.AUECModifiedDate,GetDate()) >= 0                                                                            
   Group By TaxLotID                                                         
 )    
 And TaxLotOpenQtY > 0    
 --And Symbol In ('VOR','VOR_PFW_0.0001')    
      
      
 Select      
 SM.BloombergSymbol As BBCode      
 From #Temp Temp      
 Inner Join V_SecMasterData SM With (NoLock) On Temp.Symbol= SM.tickersymbol     
     
 Union    
    
Select     
SMUnder.BloombergSymbol as BBCode      
from #Temp Temp      
INNER JOIN V_SecMasterData SM With (NoLock) On Temp.symbol= SM.tickersymbol    
INNER JOIN #Temp_SMUnderlying SMUnder ON SMUnder.TickerSymbol = SM.UnderlyingSymbol    
    
Union      
    
Select DISTINCT C.CurrencySymbol + C2.CurrencySymbol + ' Curncy' as BBCode from T_CurrencyConversionRate CR      
inner join T_CurrencyStandardPairs CP With (NoLock) On CR.CurrencyPairID_FK=CP.CurrencyPairID      
inner join T_Currency C on CP.FromCurrencyID = C.CurrencyID       
inner join T_Currency C2 on CP.ToCurrencyID = C2.CurrencyID       
where datediff(dd,CR.Date,getdate()-1)=0  
  
Order By BBCode  
      
Drop Table #Temp,#Temp_SMUnderlying 
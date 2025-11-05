

/*                             
P_GetFundWiseOpenPositions '04/17/2013','1184,1183'   
Author: Sandeep Singh  
Date: April 23, 2013  
Description: Get Open Position for selected funds                                         
*/                                 
CREATE Procedure P_GetFundWiseOpenPositions                            
(                            
@Date Datetime,                    
@fundIDs Varchar(Max)                             
)                            
As                            
                            
--Declare @Date Datetime                                          
--                                          
--Set @Date=GetDate()                    
--                    
--Declare @fundIDs Varchar(Max)                    
--                    
--Set @fundIDs = ''                    
                    
Create Table #Funds                     
(                    
 FundID int                    
)                                                            
If (@fundIDs Is NULL Or @fundIDs = '')                                                            
 Insert InTo #Funds                                                            
 Select                    
 CompanyFundID as FundID                     
 From T_CompanyFunds Where IsActive=1                                                        
Else                                                            
 Insert InTo #Funds                                                            
 Select Items as FundID from dbo.Split(@fundIDs,',')                                  
                         
Create Table #TempOpenTaxlots                                                                                        
(                                 
 TaxlotID Varchar(50),                            
 GroupID Varchar(50),                                             
 Symbol Varchar(200),                                                                                        
 TaxlotOpenQty Float,                                          
 FundID Int,                              
 OrderSideTagValue Varchar(10)                                      
)                                             
 --Get Open Taxlots                                         
 Insert Into #TempOpenTaxlots                                          
 Select                            
  PT.TaxlotID,                            
  PT.GroupID,                                           
  PT.Symbol,                                          
  PT.TaxlotOpenQty As TaxlotOpenQty,                                          
  PT.FundID,                              
  PT.OrderSideTagValue                                              
 From PM_Taxlots PT                     
 Inner join #Funds on PT.FundID = #Funds.FundID                                           
 Where Taxlot_PK in                                                                                                     
 (                                                                                
   Select Max(Taxlot_PK) from PM_Taxlots                                           
   Where Datediff(d, PM_Taxlots.AUECModifiedDate,@Date) >= 0                                                    
   Group By TaxlotID                                          
 )                                                                                                    
 And TaxLotOpenQty<>0                             
              
--Select * from #TempOpenTaxlots         
--Where Symbol='B'        
--Order by Symbol,FundID               
        
                     
--Open Quantity Group by Fund and Symbol                        
Select                             
 PT.Symbol,                                          
 SUM(PT.TaxlotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)) As Quantity,                                          
 PT.FundID                              
Into #OpenTaxlots_GroupBySymbolAndFund                            
From #TempOpenTaxlots PT                            
Group By PT.Symbol,PT.FundID                             
                
--Select * from #OpenTaxlots_GroupBySymbolAndFund          
--Order by Symbol                  
                  
-- add Fund Name and Position Type i.e. Long/Short           
        
Create Table #TempOpenPositions        
(        
Symbol Varchar(200),        
FundID Int,        
Fund Varchar(200),        
Quantity Float,        
GrossQty Float,        
NetQty Float,        
PositionType_FundLevel Varchar(20),        
PositionType_ConsolidatedLevel varchar(20),        
IsBoxedPosition Bit        
)        
         
-- Collect data in a table with Fund Name and Fund Level Position Type i.e. Long and Short at Fund Lavel        
Insert InTo #TempOpenPositions                
Select                                       
PT.Symbol As Symbol,                            
CF.CompanyFundID As FundID,                                              
CF.FundName As Fund,                                         
PT.Quantity As Quantity,                            
0 As GrossQty,                
0 As NetQty,                              
Case                
 When PT.Quantity > 0                
 Then 'Long'                
 When PT.Quantity < 0                
 Then 'Short'                
 Else ''                
End As PositionType_FundLevel,                
'' As PositionType_ConsolidatedLevel,        
0 IsBoxedPosition                                               
 From #OpenTaxlots_GroupBySymbolAndFund PT                                            
 Inner Join T_CompanyFunds CF On PT.FundID=CF.CompanyFundID                               
Where  PT.Quantity <> 0                                      
Order by Symbol,Fund,PositionType_FundLevel                          
                   
--Select * from #TempOpenPositions                 
          
--Collect data Symbol wise in a table to update Net and Gross quantity                      
Select                         
Symbol,                        
Sum(ABS(Quantity)) As GrossQty,                
Sum(Quantity) As NetQty                         
Into #TempOpenPositions_GrossAndNetQty                        
From #TempOpenPositions                        
Group By Symbol            
        
--Select * from #TempOpenPositions_GrossAndNetQty          
        
----Collect data Symbol wise in a table to update IsBoxedPosition if Fund level some positions are short and some are long        
--Select                         
--Symbol,                        
--PositionType_FundLevel                       
--Into #TempOpenPositions_GroupbySymbolAndFundLevelPositionType                      
--From #TempOpenPositions                        
--Group By Symbol,PositionType_FundLevel              
--                
----Select * from #TempOpenPositions_GroupbySymbolAndFundLevelPositionType         
--        
--Select Symbol         
--Into #TempTable_FundLevelFlagCheck        
--From #TempOpenPositions_GroupbySymbolAndFundLevelPositionType          
--Group By Symbol        
--Having Count(Symbol) > 1          
        
--Select * from #TempTable_FundLevelFlagCheck            
                        
Update #TempOpenPositions                        
Set GrossQty = ABS(Temp1.GrossQty),                
NetQty = ABS(Temp1.NetQty),                
Quantity = ABS(Quantity),                
PositionType_ConsolidatedLevel =                
Case                
 When Temp1.NetQty >= 0                
 Then 'Long'                
 When Temp1.NetQty < 0                
 Then 'Short'                
 Else ''                
End                      
From #TempOpenPositions                        
Inner Join #TempOpenPositions_GrossAndNetQty Temp1 On Temp1.Symbol = #TempOpenPositions.Symbol         
        
--Update #TempOpenPositions        
--Set IsBoxedPosition = 1         
--From #TempOpenPositions        
--inner Join #TempTable_FundLevelFlagCheck On #TempTable_FundLevelFlagCheck.Symbol=  #TempOpenPositions.Symbol        
    
Update #TempOpenPositions        
Set IsBoxedPosition = 1         
From #TempOpenPositions       
Where NetQty <> GrossQty    
              
                        
Select * from #TempOpenPositions Where NetQty > 0                 
                            
Drop Table #TempOpenTaxlots,#OpenTaxlots_GroupBySymbolAndFund,#TempOpenPositions,#Funds      
Drop Table #TempOpenPositions_GrossAndNetQty


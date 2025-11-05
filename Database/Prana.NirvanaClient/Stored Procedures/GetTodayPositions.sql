       
CREATE Procedure GetTodayPositions          
(          
  @date datetime          
)           
As        
      
--Declare @date datetime          
--          
--Set @date=GetDate()         
          
Declare @YesterDayDate Datetime          
Select  @YesterDayDate = dbo.AdjustBusinessDays(@Date,-1,1)              
          
 Create Table #OpenTaxlots                                                        
 (                 
    Symbol varchar(200),                                                        
    Quantity decimal,          
    FundID Int       
 )             
          
 Insert Into #OpenTaxlots          
 Select           
 Symbol,          
 SUM(PT.TaxlotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)),          
 FundID      
              
 From PM_Taxlots PT            
 Where  taxlot_PK in                                                                     
 (                                                
   Select Max(Taxlot_PK) from PM_Taxlots           
   Where Datediff(d, PM_Taxlots.AUECModifiedDate,@Date) >= 0                    
   Group By TaxlotID          
 )                                                                    
 and TaxLotOpenQty<>0 Group By Symbol,FundID         
          
Select       
PT.Symbol As COL1,            
CF.FundName As COL2,            
Abs(PT.Quantity) As COL3          
 From #OpenTaxlots PT            
 Inner Join T_CompanyFunds CF On PT.FundID=CF.CompanyFundID          
Where  PT.Quantity <> 0      
Order by COL1,COL2      
      
Drop Table #OpenTaxlots 


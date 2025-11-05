/*  
 Usage:  
Declare @date datetime  
  
Set @date=GetDate()  
 Exec GetPositionForADate @date  
*/  
CREATE Procedure GetPositionForADate  
(  
  @date datetime  
)   
As  
  
Declare @YesterDayDate Datetime  
Select  @YesterDayDate = dbo.AdjustBusinessDays(@Date,-1,1)  
  
  
 Declare @OpenTaxlots Table                                                
 (         
    Symbol varchar(200),                                                
    Quantity Int,  
    FundID Int,  
    UnitCost float  
 )     
  
 Insert Into @OpenTaxlots  
 Select   
 Symbol,  
 SUM(PT.TaxlotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)),  
 FundID,  
    Sum(PT.AvgPrice * PT.TaxlotOpenQty)/Sum(PT.TaxlotOpenQty)  
      
 From PM_Taxlots PT    
 Where  taxlot_PK in                                                             
 (                                        
   Select Max(Taxlot_PK) from PM_Taxlots   
   Where Datediff(d, PM_Taxlots.AUECModifiedDate,@Date) >= 0            
   Group By TaxlotID  
 )                                                            
 and TaxLotOpenQty<>0 Group By Symbol,FundID 
  
 Declare @DayMarkPrices Table                                                
 (         
  Symbol varchar(200),                                                
  TodayMarkPrice float                                              
 )                                                
  
 INSERT Into @DayMarkPrices                                                
  Select          
  DayMarkPrice.Symbol,           
  FinalMarkPrice                                               
  From PM_DayMarkPrice DayMarkPrice           
  Inner Join V_SymbolAUEC ON DayMarkPrice.Symbol = V_SymbolAUEC.Symbol          
  Where Datediff(d,DayMarkPrice.Date, @YesterDayDate) = 0   
  
--Select * from @OpenTaxlots  

 Select   
  CF.FundName as AccountNumber,  
  Case   
   When SM.AssetID=2  
   Then SM.OSISymbol  
  Else PT.Symbol  
  End As Symbol,  
  PT.UnitCost,  
  PT.Quantity,  
  IsNull(DM.TodayMarkPrice,0) as ClosingMarkPrice  
  
 From @OpenTaxlots PT    
 Inner Join T_CompanyFunds CF On PT.FundID=CF.CompanyFundID  
 Inner Join V_secmasterData SM On SM.TickerSymbol = PT.Symbol  
 Left Outer Join @DayMarkPrices DM On Dm.Symbol = PT.Symbol  
Order by AccountNumber,Symbol  
  
  

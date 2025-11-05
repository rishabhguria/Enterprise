
/*********************************************                              
                              
AUTHOR : RAHUL GUPTA                              
CREATED ON : 2012-06-01                        
Exec P_GetAllocationSchemeReconReport 'Scheme 06292012','2012-06-29 12:33:00.000' ,'2012-06-29 12:33:00.000'                             
                              
This will generate the recon report for the imported allocation scheme                              
for the required allocation date.                              
                              
*********************************************/                              
                              
CREATE Procedure P_GetAllocationSchemeReconReport                              
(                              
 @allocationSchemeName varchar(100),                      
 @fromAllocationDate datetime,              
 @toAllocationDate datetime                        
)                              
AS    
                      
--Declare @allocationSchemeName varchar(100)                      
--Set @allocationSchemeName='Latest Alloc Scheme'                      
--Declare @fromAllocationDate datetime                        
--Set @fromAllocationDate='2012-11-26 17:47:41.690'               
--Declare @toAllocationDate datetime                        
--Set @toAllocationDate='2012-11-26 17:47:41.690'                                 
                              
DECLARE @xml xml                              
SET @xml = (Select allocationScheme from T_AllocationScheme where allocationSchemeName = @allocationSchemeName)                              
                              
DECLARE @allocationSchemeID int                              
SET @allocationSchemeID = (Select allocationSchemeID from T_AllocationScheme where allocationSchemeName = @allocationSchemeName)                              
                              
DECLARE @handle int   
                                                                                                                                                                                   
exec sp_xml_preparedocument @handle OUTPUT,@xml                                 
                              
CREATE TABLE #tempAllocationScheme                              
(                              
FundName varchar(100),                              
LongName  varchar(100),                              
SEDOL varchar(20),                              
Bloomberg varchar(100),                                 
Quantity float,                               
RoundLot int,                                       
AllocationBasedOn varchar(100),                               
Validated varchar(100),                                
Symbol varchar(100),                              
RIC varchar(100),                              
ISIN varchar(20),                               
CUSIP varchar(20),                              
OSIOptionSymbol varchar(25),                                  
IDCOOptionSymbol varchar(25),                               
FundID int,                                
Percentage float,                                
TotalQty float ,                    
OrderSideTagValue Varchar(10),                    
Side Varchar(20),          
TradeType Varchar(100),          
Currency Varchar(5),          
PB Varchar(100),  
AllocationSchemeKey Varchar(100)          
)                              
                              
Insert into #tempAllocationScheme                              
(                        
FundName,                              
LongName,                              
SEDOL,                              
Bloomberg,                                 
Quantity,                               
RoundLot,                                     
AllocationBasedOn,                               
Validated,                                
Symbol,                              
RIC,                              
ISIN,                        
CUSIP,           
OSIOptionSymbol,                                  
IDCOOptionSymbol,                               
FundID,                         
Percentage,                                
TotalQty,                    
OrderSideTagValue,                    
Side,          
TradeType,          
Currency ,          
PB,  
AllocationSchemeKey               
)                              
                              
select                                                                                                                
FundName,                   
LongName,                              
SEDOL,                              
Bloomberg,                                 
Quantity,                               
RoundLot,                                        
AllocationBasedOn,                               
Validated,                                
Symbol,                              
RIC,                              
ISIN,                               
CUSIP,                              
OSIOptionSymbol,                                  
IDCOOptionSymbol,                               
FundID,                                
Percentage,                                
TotalQty ,                    
OrderSideTagValue,                    
Side,          
TradeType,          
Currency,          
PB,  
AllocationSchemeKey         
                                                                
FROM  OPENXML(@handle, '/DocumentElement/PositionMaster',2)                              
WITH                              
(                              
FundName varchar(100),                              
LongName  varchar(100),                              
SEDOL varchar(20),                              
Bloomberg varchar(100),                                 
Quantity float,                               
RoundLot int,                                     
AllocationBasedOn varchar(100),                               
Validated varchar(100),                                
Symbol varchar(100),                              
RIC varchar(100),                              
ISIN varchar(20),                               
CUSIP varchar(20),                              
OSIOptionSymbol varchar(25),                                  
IDCOOptionSymbol varchar(25),                               
FundID int,                                
Percentage float,                                
TotalQty float ,                    
OrderSideTagValue Varchar(10),                    
Side Varchar(20),          
TradeType Varchar(100),          
Currency Varchar(5),          
PB Varchar(100),  
AllocationSchemeKey Varchar(100)                         
)                      
                      
Alter Table #tempAllocationScheme                      
Add AllocationSchemeID int                      
                      
Update #tempAllocationScheme                      
Set AllocationSchemeID=@allocationSchemeID        
  
CREATE TABLE #tempRecon                              
(                              
FundName varchar(100),                              
LongName  varchar(100),                              
SEDOL varchar(20),                              
Bloomberg varchar(100),                                 
Quantity float,                          
RoundLot int,                                       
AllocationBasedOn varchar(100),                               
Validated varchar(100),                                
Symbol varchar(100),                              
RIC varchar(100),                              
ISIN varchar(20),                               
CUSIP varchar(20),           
OSIOptionSymbol varchar(25),                                  
IDCOOptionSymbol varchar(25),                               
FundID int,                                
Percentage float,                                
TotalQty float,                              
AllocatedQty float,                              
AllocatedPercentage float,                           
Matched bit,                              
PercentDifference float,                    
OrderSideTagValue Varchar(10),                    
Side Varchar(20),                    
IsDuplicate bit,          
TradeType Varchar(100),          
Currency Varchar(5),          
PB Varchar(100) ,  
AllocationSchemeKey Varchar(100)                        
)    
  
CREATE TABLE #TempGroup                              
(  
 Symbol Varchar(100),  
 AllocationSchemeID int,  
 AllocatedQty Float,  
 Percentage Float,  
 FundID int,  
 OrderSideTagValue Varchar(10)  
)  
  
Create Table #tempTotalQty  
(  
TotalQty Float,  
Symbol Varchar(100)  
)  
  
Declare @tempASchemeKey Varchar(100)  
Set @tempASchemeKey = (Select Top 1 AllocationSchemeKey from #tempAllocationScheme )   
--Select @tempASchemeKey     
  
If((@tempASchemeKey = 'SymbolSide') OR (@tempASchemeKey = 'PBSymbolSide'))  
 Begin  
       
Insert Into #TempGroup  
 Select                       
 G.Symbol,                      
 Min(G.AllocationSchemeID) As AllocationSchemeID,                  
 Sum(L1.AllocatedQty) As AllocatedQty,                      
 0 as Percentage,                    
 L1.FundID,                    
 G.OrderSideTagValue                      
  From T_Group G                       
  Inner Join T_FundAllocation L1 On L1.GroupID = G.GroupID                       
  Where G.AllocationSchemeID = @allocationSchemeID and DateDiff(Day,G.AllocationDate,@fromAllocationDate) <= 0                
  and DateDiff(Day,G.AllocationDate,@toAllocationDate) >= 0                      
  Group By G.Symbol,G.OrderSideTagValue,L1.FundID               
    
Insert Into #tempTotalQty               
 Select               
 Sum(AllocatedQty) as TotalQty,              
 Symbol              
 from #TempGroup               
 group by symbol                           
                            
                               
 Insert into #tempRecon                              
 Select                               
 allScheme.FundName,                              
 allScheme.LongName,                              
 allScheme.SEDOL,                              
 allScheme.Bloomberg,                                 
 allScheme.Quantity,                               
 allScheme.RoundLot,                                       
 allScheme.AllocationBasedOn,                               
 allScheme.Validated,                                
 allScheme.Symbol,                              
 allScheme.RIC,                              
 allScheme.ISIN,                               
 allScheme.CUSIP,                              
 allScheme.OSIOptionSymbol,                                  
 allScheme.IDCOOptionSymbol,                               
 allScheme.FundID,                                
 allScheme.Percentage,                                
 allScheme.TotalQty,                              
 isnull(G.AllocatedQty,0) As AllocatedQty,                               
 isnull(G.Percentage,0) As AllocatedPercentage,                      
 0 As Matched,                      
 0 As PercentDifference,                    
 allScheme.OrderSideTagValue,                    
 allScheme.Side,                    
 0 As IsDuplicate,          
 allScheme.TradeType,          
 allScheme.Currency,          
 allScheme.PB ,  
 allScheme.AllocationSchemeKey  
                   
 from  #tempAllocationScheme allScheme                      
 Left Outer Join  #TempGroup G on allScheme.AllocationSchemeID = G.AllocationSchemeID                     
  And allScheme.Symbol = G.Symbol And G.FundID =allScheme.FundID And G.OrderSideTagValue =allScheme.OrderSideTagValue    
 End  
Else -- Key =  Symbol  
 Begin  
  
Insert Into #TempGroup  
 Select                       
 G.Symbol,                      
 Min(G.AllocationSchemeID) As AllocationSchemeID,                  
 Sum(L1.AllocatedQty) As AllocatedQty,                      
 0 as Percentage,                    
 L1.FundID,                    
 Min(G.OrderSideTagValue) As OrderSideTagValue  
 From T_Group G                       
 Inner Join T_FundAllocation L1 On L1.GroupID = G.GroupID                       
 Where G.AllocationSchemeID = @allocationSchemeID and DateDiff(Day,G.AllocationDate,@fromAllocationDate) <= 0                
 and DateDiff(Day,G.AllocationDate,@toAllocationDate) >= 0                      
 Group By G.Symbol,L1.FundID               
               
    
Insert Into #tempTotalQty             
 Select               
 Sum(AllocatedQty) as TotalQty,              
 Symbol              
 from #TempGroup               
 group by symbol                           
                            
                               
 Insert into #tempRecon                              
 Select                               
 allScheme.FundName,                              
 allScheme.LongName,                              
 allScheme.SEDOL,                              
 allScheme.Bloomberg,                                 
 allScheme.Quantity,                               
 allScheme.RoundLot,                                       
 allScheme.AllocationBasedOn,                               
 allScheme.Validated,                                
 allScheme.Symbol,                              
 allScheme.RIC,                              
 allScheme.ISIN,                               
 allScheme.CUSIP,                              
 allScheme.OSIOptionSymbol,                                  
 allScheme.IDCOOptionSymbol,                               
 allScheme.FundID,                                
 allScheme.Percentage,                                
 allScheme.TotalQty,                              
 isnull(G.AllocatedQty,0) As AllocatedQty,                               
 isnull(G.Percentage,0) As AllocatedPercentage,                      
 0 As Matched,                      
 0 As PercentDifference,                    
 '' as OrderSideTagValue,--allScheme.OrderSideTagValue,                    
 '' as Side,--allScheme.Side,                   
 0 As IsDuplicate,          
 allScheme.TradeType,          
 allScheme.Currency,          
 allScheme.PB ,  
 allScheme.AllocationSchemeKey  
                   
 from  #tempAllocationScheme allScheme                      
 Left Outer Join  #TempGroup G on allScheme.AllocationSchemeID = G.AllocationSchemeID                     
  And allScheme.Symbol = G.Symbol And G.FundID =allScheme.FundID --And G.OrderSideTagValue =allScheme.OrderSideTagValue    
  
  
 End                  
        
  
                        
Update #tempRecon                      
Set                       
Matched =                       
 Case                       
  When Quantity = AllocatedQty                      
  Then 1                      
  Else 0                      
 End,                 
PercentDifference =                       
 Case                      
  When Quantity = AllocatedQty                      
  Then 0                    
  When Quantity <> AllocatedQty And AllocatedQty > 0                      
  Then Abs(Quantity - AllocatedQty)                       
  Else 0                    
 End                      
                       
              
Update #tempRecon              
Set AllocatedPercentage =  AllocatedQty * 100/tQty.TotalQty              
from #tempRecon tRecon   
Inner Join #tempTotalQty tQty On tRecon.Symbol = tQty.Symbol                
              
Select * from #tempRecon Order by Symbol--,Side                           
                             
drop table #tempAllocationScheme                              
drop table #tempRecon ,#TempGroup,#tempTotalQty 
exec sp_xml_removedocument @handle
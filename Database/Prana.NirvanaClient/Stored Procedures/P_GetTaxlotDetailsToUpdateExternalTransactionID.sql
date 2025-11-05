
/*     
Author:Sandeep Singh
Date: Sept 10, 2013
Description: This is used to get transaction(s) to update it external transaction ID. This is very mych specific to SS client.
			 It splits a transaction in more than one if it closes with more than one transaction. Example is given below.
Example: Assume 2 buy taxlots of 100 qty each with different strategy closed against a sell trade of 200 qty. Now sell trade splits in 2 transactions,
but we keep only one transaction in our database. 

Exec P_GetTaxlotDetailsToUpdateExternalTransactionID '130904143231011820'            
*/            
            
CREATE Procedure P_GetTaxlotDetailsToUpdateExternalTransactionID            
(            
@TaxlotID Varchar(50)            
)            
As            
        
--Declare @TaxlotID Varchar(50)            
--Set @TaxlotID = '1309101501530118216'        
        
Declare @TaxlotID_Local Varchar(50)        
Set @TaxlotID_Local = @TaxlotID      
--Declare @TaxlotID Varchar(50)            
--Set @TaxlotID = '130904143231011820'        
      
/* Create Temp tables and use them */      
Select *      
Into #PM_Taxlots      
From PM_Taxlots      
      
Select *      
Into #PM_TaxlotClosing      
From PM_TaxlotClosing      
        
/* Split External Transaction ID */        
        
Select         
Top 1        
TaxlotID,        
ExternalTransID        
Into #Temp_TaxlotAndExternalTransIDTable        
From #PM_Taxlots        
Where TaxlotID = @TaxlotID_Local        
        
Create Table #Temp_SplittedWithComma        
(        
SplitValue varchar(500),        
TaxlotID varchar(50)        
)         
        
;WITH CTE(TaxlotID,ExternalTransID) AS         
(        
   Select TaxlotID,ExternalTransID from #Temp_TaxlotAndExternalTransIDTable        
)        
Insert Into #Temp_SplittedWithComma        
select         
  b.items as SplitValue        
, TaxlotID        
        
from CTE a        
cross apply dbo.Split(a.ExternalTransID,',') b         
Order by TaxlotID        
        
Select        
TaxlotID,       
Case      
 When Len(SplitValue) > 0  And CharIndex(':', SplitValue) > 0    
 Then Substring(SplitValue,(CharIndex(':', SplitValue) + 1),Len(SplitValue))      
 When Len(SplitValue) > 0  And CharIndex(':', SplitValue) <= 0    
 Then SplitValue     
Else ''      
End As ExternalTransID,       
Case       
  When Len(SplitValue) > 0 And CharIndex(':', SplitValue) > 0    
 Then Substring(SplitValue,1,(CharIndex(':', SplitValue)-1))     
 When Len(SplitValue) > 0 And CharIndex(':', SplitValue) <= 0    
 Then 0     
Else 0       
End As StrategyID        
Into #Temp_Splitted_Final        
 from #Temp_SplittedWithComma        
       
--Select * from #Temp_Splitted_Final     
/* Split External Transaction ID */              
            
Create Table #Temp_ExternalTranIDTable            
(            
TaxlotID Varchar(50) Not Null,            
Symbol varchar(200) Not Null,            
Side varchar(20) Not Null,            
Quantity Float Not Null,            
StrategyID Int Null,            
Strategy varchar(100) Null,            
ExternalTransactionID varchar(200) Null            
)                  
            
Select             
PT1.TaxlotID,            
PT.Symbol,            
PT1.OrderSideTagValue,            
T_Side.Side,            
PTC.ClosedQty As Quantity,            
PT.Level2ID As StrategyID,            
IsNull(Strategy.StrategyShortName,'') As Strategy            
Into #Temp_ClosingInfo            
FROM #PM_TaxlotClosing  PTC                                                                                          
Inner Join #PM_Taxlots PT on (PTC.PositionalTaxlotID=PT.TaxlotID And  PTC. TaxLotClosingId = PT.TaxLotClosingId_Fk)                                                                              
Inner Join #PM_Taxlots PT1 on (PTC.ClosingTaxlotID=PT1.TaxlotID And  PTC. TaxLotClosingId = PT1.TaxLotClosingId_Fk)              
Left Outer Join T_CompanyStrategy Strategy On Strategy.CompanyStrategyID = PT.Level2ID            
Inner Join T_Side On T_Side.SideTagValue = PT1.OrderSideTagValue            
Where PT1.TaxlotID = @TaxlotID_Local             
            
            
Declare @RecordCount Int            
Set @RecordCount = (Select Count(*) from #Temp_ClosingInfo)            
Print @RecordCount            
            
If(@RecordCount > 0)            
 Begin            
  Insert Into #Temp_ExternalTranIDTable            
   Select             
   TaxlotID,                
   Max(Symbol) As Symbol,                
   Max(Side) As Side,            
   Sum(Quantity) As Quantity,             
  StrategyID,            
   Strategy,            
   '' As ExternalTransacionID               
   From #Temp_ClosingInfo            
   Group By TaxlotID,StrategyID, Strategy            
 End            
            
Else            
            
 Begin              
 Insert Into #Temp_ExternalTranIDTable            
   Select             
   VT.TaxlotID,                
   VT.Symbol,                
   Side,            
   VT.TaxlotQty As Quantity,             
   VT.Level2ID As StrategyID,            
   IsNull(Strategy.StrategyShortName,'') As Strategy,            
   '' As ExternalTransacionID             
   From V_Taxlots VT            
   Inner Join T_Side On T_Side.SideTagValue = VT.OrderSideTagValue            
   Left Outer Join T_CompanyStrategy Strategy On Strategy.CompanyStrategyID = VT.Level2ID               
Where VT.TaxlotID = @TaxlotID_Local             
 End            
            
          
-- Update External Trasaction ID if already exists        
Update #Temp_ExternalTranIDTable            
Set ExternalTransactionID = Mapping.ExternalTransID            
From #Temp_ExternalTranIDTable            
Inner Join #Temp_Splitted_Final Mapping On Mapping.TaxLotID = #Temp_ExternalTranIDTable.TaxLotID          
And Mapping.StrategyID = #Temp_ExternalTranIDTable.StrategyID          
            
Select * from #Temp_ExternalTranIDTable            
            
Drop Table #Temp_ClosingInfo,#Temp_ExternalTranIDTable        
Drop Table #Temp_TaxlotAndExternalTransIDTable,#Temp_SplittedWithComma,#Temp_Splitted_Final      
Drop Table #PM_Taxlots,#PM_TaxlotClosing


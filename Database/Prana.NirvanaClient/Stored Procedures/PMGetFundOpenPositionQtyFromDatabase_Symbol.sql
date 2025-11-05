  
Create PROCEDURE [dbo].[PMGetFundOpenPositionQtyFromDatabase_Symbol]                                                
(                                                                                                                      
 @ToAllAUECDatesString VARCHAR(MAX),  
 @FundIds VARCHAR(MAX),                                                
 @Symbols varchar(MAX)                                                              
)                                                                                                                      
As                                                                                                                          
Begin                                                                                                          
                                                                                       
 Create Table #PositionTable          
 (                                                                                                                                      
  Symbol Varchar(100),                                                                                                                                                                       
  OpenQuantity float,
  Positiontag int                                                                                                                                                                          
 )    
         
   
Insert Into #PositionTable EXEC P_GetFundOpenPositionQty_symbol @ToAllAUECDatesString,@FundIds,@Symbols      
                                                                                                
Select * from #PositionTable          
End   


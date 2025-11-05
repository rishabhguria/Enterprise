/*    
Declare @Date DateTime    
Set @Date ='08-25-2010'    
P_DeleteSuperNirvanaTrades '08-17-2010'     
*/    
CREATE Proc P_DeleteSuperNirvanaTrades     
(    
 @DeclaredDate datetime    
)    
As    
Set Nocount On    
  
BEGIN TRANSACTION TRAN1                                           
BEGIN TRY   
    
-- Check Day whether Sunday or Not    
-- If Sunday then below query returns 7    
    
--Declare @DeclaredDate datetime      
--Set @DeclaredDate='08-17-2010'--GetDate()          
Declare @weekDay tinyInt    
SET @weekDay =((@@dateFirst+datePart(dw,@DeclaredDate)-2) % 7) + 1        
    
--Select @weekDay as WeekDay    
Delete T_Fills  
from T_Fills  
Inner Join T_Sub on T_Sub.ClOrderID=T_Fills.ClOrderID  
Inner Join T_Order O On O.ParentClOrderID=T_Sub.ParentClOrderID  
Inner Join T_AUEC A On A.AuecID=O.AUECID    
Where A.AssetID <> 3  
  
Delete T_Sub  
from T_Sub  
Inner Join T_Order O On O.ParentClOrderID=T_Sub.ParentClOrderID  
Inner Join T_AUEC A On A.AuecID=O.AUECID    
Where A.AssetID <> 3  
  
Delete T_Order  
from T_Order O    
Inner Join T_AUEC A On A.AuecID=O.AUECID    
Where A.AssetID <> 3    
  
Delete T_GroupOrder  
from T_GroupOrder    
Inner Join T_Group G On T_GroupOrder.GroupID=G.GroupID    
Inner Join T_AUEC on T_AUEC.AUECID=G.AUECID    
Where T_AUEC.AssetID <> 3    
  
Delete T_Level2Allocation    
 From  T_Level2Allocation L2    
 Inner Join T_FundAllocation L1 On L1.AllocationID=L2.Level1AllocationID    
 Inner Join T_Group G On G.GroupID=L1.GroupID   
 Inner Join T_AUEC on T_AUEC.AUECID=G.AUECID    
Where T_AUEC.AssetID <> 3      
 --    
 Delete T_FundAllocation    
 From T_FundAllocation L1    
 Inner Join T_Group G On G.GroupID=L1.GroupID   
 Inner Join T_AUEC on T_AUEC.AUECID=G.AUECID    
Where T_AUEC.AssetID <> 3      
    
Delete PM_Taxlots  
From PM_Taxlots Taxlots    
Inner Join V_SecMasterData SM On SM.TickerSymbol=Taxlots.Symbol   
Inner Join T_AUEC on T_AUEC.AUECID=SM.AUECID    
Where T_AUEC.AssetID <> 3    
    
Delete T_Group    
From T_Group G    
Inner Join T_AUEC on T_AUEC.AUECID=G.AUECID    
Where T_AUEC.AssetID <> 3    
    
Declare @CutOffHour int    
Declare @CutOffMin int    
    
Declare @SubTable Table    
(    
 Symbol Varchar(100),    
 ClOrderID varchar(100),    
 ParentClOrderID varchar(100),    
 AUECLocalDate DateTime,    
 ProcessDate DateTime    
)    
    
Declare @GroupTable Table    
(    
 Symbol Varchar(100),    
 GroupID varchar(100),    
 AUECLocalDate DateTime,    
 ProcessDate DateTime    
)    
    
Declare      
@Symbol varchar(100),         
@SundayCutOffHour int,          
@SundayCutOffMIn int,          
@WeekCutOffHour int,    
@WeekCutOffMin int    
          
DECLARE FutureSymbol_Cursor CURSOR FAST_FORWARD FOR                                      
Select Distinct           
  G.Symbol                        
 ,SundayCutOffHour                          
 ,SundayCutOffMIn                     
 ,WeekCutOffHour                     
 ,WeekCutOffMin                       
From  T_FutureSymbolwiseCutoffTime FM    
Inner Join T_Group G ON SUBSTRING(G.Symbol, 0, CHARINDEX(' ', G.Symbol)) = FM.Symbol        
          
Open FutureSymbol_Cursor;                                    
                                    
FETCH NEXT FROM FutureSymbol_Cursor INTO             
@Symbol,         
@SundayCutOffHour,          
@SundayCutOffMIn,          
@WeekCutOffHour,    
@WeekCutOffMin      
    
WHILE @@fetch_status = 0                                      
BEGIN     
    
--Select @Symbol    
    
if @weekDay = 7    
 Begin    
  Set @CutOffHour =@SundayCutOffHour    
  Set @CutOffMin = @SundayCutOffMIn    
 End    
Else    
 Begin    
  Set @CutOffHour =@WeekCutOffHour    
  Set @CutOffMin = @WeekCutOffMin    
 End    
    
--Select @Symbol,@CutOffHour,@CutOffMin    
    
If(@CutOffHour>0)    
Begin    
    
 Insert into @SubTable    
 SELECT     
 T_Order.Symbol,    
 ClOrderID,     
 T_Sub.ParentClOrderID,    
 AUECLocalDate,    
 Case    
  When [dbo].[IsGreaterThan](AUECLocalDate,@CutOffHour,@CutOffMin)=1    
  Then DateAdd(d,1,AUECLocalDate)    
 Else AUECLocalDate    
 End as ProcessDate    
 FROM T_Sub     Inner Join T_Order On T_Order.ParentClOrderID=T_Sub.ParentClOrderID    
 Where T_Order.Symbol=@Symbol     
 order by T_Order.Symbol,AUECLocalDate    
    
    
 --Select * from @SubTable    
 --    
 Delete T_Fills     
 From T_Fills    
 Inner Join @SubTable SubTemp On SubTemp.ClOrderID=T_Fills.ClOrderID    
 Where DateDiff(day,SubTemp.ProcessDate,@DeclaredDate) > 0 and SubTemp.Symbol=@Symbol    
    
 Delete T_Sub    
 From T_Sub    
 Inner Join @SubTable SubTemp On SubTemp.ParentClOrderID=T_Sub.ParentClOrderID    
 Where  DateDiff(day,SubTemp.ProcessDate,@DeclaredDate) > 0 and SubTemp.Symbol=@Symbol    
    
 Delete T_Order    
 From T_Order    
 Inner Join @SubTable SubTemp On SubTemp.ParentClOrderID=T_Order.ParentClOrderID    
 Where DateDiff(day,SubTemp.ProcessDate,@DeclaredDate) > 0 and SubTemp.Symbol=@Symbol    
    
 Delete T_GroupOrder    
 From T_GroupOrder    
 Inner Join @SubTable SubTemp On SubTemp.ParentClOrderID=T_GroupOrder.ClOrderID    
 Where DateDiff(day,SubTemp.ProcessDate,@DeclaredDate) > 0 and SubTemp.Symbol=@Symbol    
    
 Insert into @GroupTable    
 SELECT     
 Symbol,    
 GroupID,     
 AUECLocalDate,    
 Case    
  When [dbo].[IsGreaterThan](AUECLocalDate,@CutOffHour,@CutOffMin)=1    
  Then DateAdd(d,1,AUECLocalDate)    
  Else AUECLocalDate    
 End as ProcessDate    
 FROM T_Group    
 Where Symbol=@Symbol     
 order by Symbol,AUECLocalDate    
 --    
 --Select * from @GroupTable Where ProcessDate is not null    
    
 Delete T_Level2Allocation    
 From  T_Level2Allocation L2    
 Inner Join T_FundAllocation L1 On L1.AllocationID=L2.Level1AllocationID    
 Inner Join @GroupTable Temp On Temp.GroupID=L1.GroupID    
 Where DateDiff(day,Temp.ProcessDate,@DeclaredDate) > 0 and Temp.Symbol=@Symbol     
 --    
 Delete T_FundAllocation    
 From T_FundAllocation L1    
 Inner Join @GroupTable Temp On Temp.GroupID=L1.GroupID    
 Where DateDiff(day,Temp.ProcessDate,@DeclaredDate) > 0 and Temp.Symbol=@Symbol

Delete PM_TaxlotClosing 
From  PM_TaxlotClosing PTC
Inner Join PM_Taxlots PT
 on ( PTC.PositionalTaxlotID=PT.TaxlotID  and  PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk) 
 Inner Join @GroupTable Temp On Temp.GroupID=PT.GroupID  
 Where DateDiff(day,Temp.ProcessDate,@DeclaredDate) > 0 and Temp.Symbol=@Symbol     
    
 Delete PM_Taxlots    
 From PM_Taxlots Taxlots    
 Inner Join @GroupTable Temp On Temp.GroupID=Taxlots.GroupID    
 Where DateDiff(day,Temp.ProcessDate,@DeclaredDate) > 0 and Temp.Symbol=@Symbol    
    
 Delete T_Group    
 From T_Group G    
 Inner Join @GroupTable Temp On Temp.GroupID=G.GroupID    
 Where DateDiff(day,Temp.ProcessDate,@DeclaredDate) > 0 and Temp.Symbol=@Symbol    
    
End    
    
    
FETCH NEXT FROM FutureSymbol_Cursor INTO                                       
          
@Symbol,         
@SundayCutOffHour,          
@SundayCutOffMIn,          
@WeekCutOffHour,    
@WeekCutOffMin   ;          
          
END                                      
CLOSE FutureSymbol_Cursor;                                      
DEALLOCATE FutureSymbol_Cursor;   
  
COMMIT TRANSACTION TRAN1                                                                                         
                                                                                               
END TRY     
                                                                                                                       
BEGIN CATCH                                                                                  
ROLLBACK TRANSACTION TRAN1                                                                     
END CATCH;                                                       
                
    
--Select * from @GroupTable --Where Symbol='ES U0'    
------Where  DateDiff(d,AUECLocalDate,ProcessDate)>0--    
----order by Symbol, AUECLocalDate    
--    
--Select * from T_Group G    
--Inner Join T_AUEC A On A.AuecID=G.AUECID    
--Where A.AssetID=3    
--Order By G.Symbol    
----    
--Select * from T_Order O    
--Inner Join T_AUEC A On A.AuecID=O.AUECID    
--Where A.AssetID=3    
--Order By O.Symbol    
--Select * from T_FundAllocation     
--Select * from T_Level2Allocation    
--    
--Select * from PM_Taxlots    
--Inner Join T_Group Temp On PM_Taxlots.GroupID=Temp.GroupID    
--Inner Join T_AUEC on T_AUEC.AUECID=Temp.AUECID    
--Where T_AUEC.AssetID=3    
--    
--Select * from T_GroupOrder    
--Inner Join T_Group Temp On T_GroupOrder.GroupID=Temp.GroupID    
--Inner Join T_AUEC on T_AUEC.AUECID=Temp.AUECID    
--Where T_AUEC.AssetID=3    
    
    
    
    
    


CREATE procedure [dbo].[P_SaveAllocatedStrategies] (      
 @GroupID varchar(50),      
 @StrategyID int,      
 @AllocatedQty float,      
 @Percentage float    
        
)      
As      
Insert Into  T_StrategyAllocation      
(      
 GroupID,      
 StrategyID,      
 AllocatedQty,      
 Percentage      
)       
Values      
(      
 @GroupID,      
 @StrategyID,      
 @AllocatedQty,      
 @Percentage      
)

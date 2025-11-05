CREATE PROCEDURE [dbo].[P_GetGroupIdDateByClOrderID]    
  @parentClOrderID VARCHAR(MAX)    
AS    
  
BEGIN  
 CREATE TABLE #ParentClOrderIDs (ParentClOrderID VARCHAR(50))   
 INSERT INTO #ParentClOrderIDs   
 SELECT * FROM dbo.Split((@parentClOrderID),',')    
  
 SELECT
 
 Case G.StateID
 when 2 then G.AllocationDate 
 when 1  then G.AUECLocalDate 
 end as AUECLocalDate,
 T.GroupID  
 FROM T_TradedOrders as T with (nolock) left outer join 
 T_Group  as G with (nolock) on  T.GroupID=G.GroupID
 WHERE ParentClOrderID IN (SELECT ParentClOrderID FROM #ParentClOrderIDs)   
  
 DROP TABLE #ParentClOrderIDs  
END 
Create PROCEDURE [dbo].[P_SetIsHiddenInSubTable] (    
 @ParentCLOrderID VARCHAR(MAX) 
)
As  

SELECT * INTO #temp1 FROM dbo.Split(@ParentCLOrderID,',')   

Update T_Sub set IsHidden = 1 where ParentCLOrderID in (select * from #temp1)

RETURN 0

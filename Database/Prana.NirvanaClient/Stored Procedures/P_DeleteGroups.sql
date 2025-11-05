-- Modified By: Disha Sharma
-- Date : 11/25/2015
-- Description: Added Error Parameters and Try catch
CREATE PROCEDURE [dbo].[P_DeleteGroups] (          
	@groupXml VARCHAR(MAX) 
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT         
)          
AS          
SET @ErrorNumber = 0  
SET @ErrorMessage = 'Success'   

BEGIN TRY 

	DECLARE	@handle	INT
	EXEC	sp_xml_preparedocument @handle OUTPUT,@groupXml

	CREATE TABLE #Temp_Group(
		GroupID VARCHAR(50)
	)      
      
	INSERT INTO #Temp_Group
	SELECT	GroupID
	FROM	OPENXML(@handle,'/Groups/AllocationGroup',1)      
			WITH      
			(      
			GroupID VARCHAR(50) '@GroupID'      
			)

	DELETE FROM T_GroupOrder	WHERE GroupID IN (SELECT GroupID FROM #Temp_Group)        
    
	-- Delete from position layer for the groupid created on the supplied date.    
	DELETE FROM PM_Taxlots WHERE GroupID IN (SELECT GroupID FROM #Temp_Group) 
	--(Select TaxLot_PK from PM_Taxlots PT Inner Join #Temp_Group temp on PT.GroupID = temp.GroupID)    
      
	DELETE FROM T_Level2Allocation WHERE Level1AllocationID IN (SELECT AllocationID FROM T_FundAllocation WHERE GroupID IN (SELECT GroupID FROM #Temp_Group))          
	DELETE FROM T_FundAllocation WHERE GroupID IN (SELECT GroupID FROM #Temp_Group)              
	DELETE FROM T_Group WHERE GroupID IN (SELECT GroupID FROM #Temp_Group)          
	Delete From T_UngroupedAllocationGroups where GroupID in (Select GroupID from #Temp_Group)            
	DROP TABLE #Temp_Group 

	EXEC	sp_xml_removedocument @handle
END TRY  
  
BEGIN CATCH  
 SET @ErrorMessage = ERROR_MESSAGE();  
 SET @ErrorNumber = Error_number();  
END CATCH;


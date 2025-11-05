CREATE PROCEDURE [dbo].[P_HideOrderFromBlotter]    
(  
  @parentClOrderID VARCHAR(MAX),   
  @rowAffected INT OUTPUT,
  @ErrorMessage varchar(500) output,                                                                     
  @ErrorNumber int output      
)
AS      
    SET @ErrorNumber = 0                                                    
    SET @ErrorMessage = 'Success' 
BEGIN TRY   
	BEGIN TRAN 
		CREATE TABLE #ParentClOrderIDs (ParentClOrderID VARCHAR(50))     
		INSERT INTO #ParentClOrderIDs     
		SELECT * FROM dbo.Split((@parentClOrderID),',')      
    
		UPDATE T_Sub SET IsHidden = 1  
		WHERE ParentClOrderID IN (SELECT ParentClOrderID FROM #ParentClOrderIDs)     
    SET @rowAffected = @@ROWCOUNT  
	DROP TABLE #ParentClOrderIDs     
	COMMIT TRAN
END TRY                                                                                    
BEGIN CATCH        
 ROLLBACK TRAN                                                 
 SET @ErrorMessage = ERROR_MESSAGE()                                                   
 SET @ErrorNumber = ERROR_NUMBER()                              
END CATCH   
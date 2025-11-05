CREATE PROCEDURE [dbo].[P_HideSubOrdersFromBlotter]  
(  
  @SubOrderClOrderIDs VARCHAR(MAX),   
  @rowAffected INT OUTPUT,
  @ErrorMessage varchar(500) output,                                                                     
  @ErrorNumber int output      
)
AS      
    SET @ErrorNumber = 0                                                    
    SET @ErrorMessage = 'Success' 
BEGIN TRY   
	BEGIN TRAN 
		CREATE TABLE #SubOrderClOrderIDs (SubOrderClOrderID VARCHAR(50))     
		INSERT INTO #SubOrderClOrderIDs     
		SELECT * FROM dbo.Split((@SubOrderClOrderIDs),',')      

		UPDATE T_Sub SET IsHidden = 1  
		WHERE ClOrderID IN (SELECT SubOrderClOrderID FROM #SubOrderClOrderIDs)     

		SET @rowAffected = @@ROWCOUNT 

		UPDATE T_Sub SET IsHidden = 1  
		WHERE ClOrderID IN (SELECT OrigClOrderID FROM T_Sub WHERE ClOrderID IN (SELECT SubOrderClOrderID FROM #SubOrderClOrderIDs))

		IF @rowAffected = 0
			SET @rowAffected = @@ROWCOUNT
	DROP TABLE #SubOrderClOrderIDs     
	COMMIT TRAN
END TRY                                                                                    
BEGIN CATCH        
 ROLLBACK TRAN                                                 
 SET @ErrorMessage = ERROR_MESSAGE()                                                   
 SET @ErrorNumber = ERROR_NUMBER()                              
END CATCH 
  GO 
  print 'Started Executing: ClientDB_P_GetCurrentPlusOneBusinessDate.sql'
GO 
                      
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[P_GetCurrentPlusOneBusinessDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
DROP PROCEDURE [dbo].[P_GetCurrentPlusOneBusinessDate]
END
GO  
-- =============================================      
---- Description: Returns Previous Business Day based on selected date.  
/*      
 Usage:       
  Exec [P_GetCurrentPlusOneBusinessDate] '08-01-2014'      
      
*/      
-- =============================================      
CREATE PROCEDURE [dbo].[P_GetCurrentPlusOneBusinessDate]      
(  
 @Date DateTime  
)     
AS    
--  
--declare @Date DateTime  
--Set @Date ='07-01-2014'  
    
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
     
DECLARE @companyID int                        
SET @companyID = (select top 1 CompanyID from T_Company Where CompanyID <> -1)                        
                        
DECLARE @auecID int                        
SET @auecID = (SELECT DefaultAUECID FROM T_Company WHERE CompanyID = @companyID)   
  
Select dbo.AdjustBusinessDays(@Date,1,@auecID) As CurrentPlusOneBusinessDate  
      
END    

GO 
  print 'Done Executing: ClientDB_P_GetCurrentPlusOneBusinessDate.sql'
 GO 

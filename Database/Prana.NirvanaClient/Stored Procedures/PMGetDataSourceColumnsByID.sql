    
    
-- =============================================    
-- Author:  Rajat    
-- Create date: 28 Nov 2006    
-- Description: Get all the columns for the data source    
/*    
  
select * from pm_datasourcecolumns where ThirdPartyID = 1  
EXEC [dbo].[PMGetDataSourceColumnsByID] @ThirdPartyID = 1, @TableTypeID = 2   
 @ErrorMessage = '',    
 @ErrorNumber = 0    
*/    
-- =============================================    
CREATE PROCEDURE [dbo].[PMGetDataSourceColumnsByID]    
 -- Add the parameters for the stored procedure here    
 @ThirdPartyID int,    
 @TableTypeID int,   
 @ErrorMessage varchar(500) output ,    
 @ErrorNumber int output     
AS    
BEGIN    
SET @ErrorMessage  = 'Success';    
SET @ErrorNumber = 0;    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    BEGIN TRY    
    -- Insert statements for procedure here    
 SELECT     
  DataSourceColumnID,     
  ColumnName,     
  Type,     
  ISNULL(ApplicationColumnId, 0) AS [ApplicationColumnId],     
  ISNULL(RequiredInUpload, 0) AS [RequiredInUpload],     
  ISNULL(ColumnSequenceNo , 0) AS [ColumnSequenceNo]    
 from     
  PM_DataSourceColumns    
 where     
  ThirdPartyID = @ThirdPartyID    
  And   
  TableTypeID = @TableTypeID  
  
END TRY    
BEGIN CATCH     
 SET @ErrorMessage = ERROR_MESSAGE();    
 SET @ErrorNumber = Error_number();     
END CATCH;    
END 
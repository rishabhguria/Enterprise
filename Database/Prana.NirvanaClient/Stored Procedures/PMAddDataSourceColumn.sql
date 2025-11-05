  
  
/****************************************************************************  
Name :   PMAddDataSourceColumn  
Date Created: 14-Nov-2006   
Purpose:  Adds data source column info  
Author: Ram Shankar Yadav  
Parameters:   
  @ThirdPartyID int,  
  @ColumnName nvarchar(50),  
  @Description nvarchar(500),  
  @Type tinyint,  
  @SampleValue nvarchar(100),  
  @Notes nvarchar(500),  
  @Error int output,  
  @ErrorMessage varchar(100) output  
  
Execution StateMent:   
     
   EXEC PMAddDataSourceColumn 1, 'test', 'test desc', 0, 'sample test value', 'test note'  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMAddDataSourceColumn]  
 (  
  @ThirdPartyID int,  
  @ColumnName nvarchar(50),  
  @Description nvarchar(500),  
  @Type tinyint,  
  @SampleValue nvarchar(100),  
  @Notes nvarchar(500),  
  @Error int output,  
  @ErrorMessage varchar(100) output  
 )  
AS   
  
--Declare @Error int  
  
SET @Error = 0  
SET @ErrorMessage = 'Success'  
BEGIN TRY  
  
INSERT  INTO  
  PM_DataSourceColumns   
   (  
    ThirdPartyID ,  
    ColumnName,  
    Description,  
    Type ,  
    SampleValue,  
    Notes  
   )  
VALUES  
   (  
    @ThirdPartyID ,  
    @ColumnName,  
    @Description,  
    @Type ,  
    @SampleValue,  
    @Notes  
   )  
  
END TRY  
BEGIN CATCH  
 SET @ERROR = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
END CATCH;  
--RETURN @ERROR  
  
  
  
  

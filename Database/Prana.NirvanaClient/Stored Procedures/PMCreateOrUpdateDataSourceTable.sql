/****************************************************************************      
Name :   [PMCreateOrUpdateDataSourceTable]     
Date Created: 21-dec-2006       
Purpose:  Create or Update Table for a data source.      
Author: Bhupesh Bareja      
Parameters:       
 @ThirdPartyID int,      
 @Xml nText,      
 @ErrorNumber int output,      
 @ErrorMessage varchar(200) output       
      
Execution Statement :       
 exec PMCreateOrUpdateDataSourceTable   
 'PM_CameronNetPosition', 165, 2,   
 'CREATE TABLE PM_CameronNetPosition( Quantity int NOT NULL, Symbol varchar(500) NOT NULL, Side varchar(500) NOT NULL, UploadID int NOT NULL, Sugandh varchar(500) NULL)'  
    ,0, ''  
drop table pm_cameronnetposition  
select * from pm_cameronnetposition  
delete pm_Datasourcecolumns where ThirdPartyID = 165  
  
****************************************************************************/      
CREATE Proc [dbo].[PMCreateOrUpdateDataSourceTable]      
(      
 @TableName varchar(50),      
 @ThirdPartyID int,      
 @TableTypeID int,      
 @command nText,      
 @ErrorNumber int output,      
 @ErrorMessage varchar(200) output      
 )      
AS       
      
SET @ErrorNumber = 0      
SET @ErrorMessage = 'Success'      
--Set @alreadyExists = ''       
declare @alreadyExists varchar(50)      
      
BEGIN TRY      
      
BEGIN TRAN      
Set @alreadyExists = object_id(@TableName)      
      
if @alreadyExists = 'NULL'      
begin      
 --This code creates new table.      
 exec ( @command )    
      
end      
   
else      
begin      
  --This code alters the already created table.      
 exec ( @command )       
 --declare @sd int     
End       
      
DECLARE @total int      
Set @total = 0      
Select @total = Count(*) FROM PM_DataSourceTables Where ThirdPartyID = @ThirdPartyID AND TableTypeID = @TableTypeID      
if (@total = 0)      
begin      
 INSERT PM_DataSourceTables(ThirdPartyID, TableName, TableTypeID)      
 Values (@ThirdPartyID, @TableName, @TableTypeID)      
end      
      
COMMIT TRAN      
      
END TRY      
BEGIN CATCH      
       
 SET @ErrorNumber = ERROR_NUMBER();      
 SET @ErrorMessage = ERROR_MESSAGE();      
       
 ROLLBACK TRAN      
      
END CATCH; 
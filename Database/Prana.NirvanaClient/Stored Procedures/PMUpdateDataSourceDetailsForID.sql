




/****************************************************************************
Name :   [PMUpdateDataSourceDetailsForID]
Date Created: 14-Nov-2006 
Purpose:  Get all the DataSources
Author: Sugandh Jain
Execution Statement : exec [PMUpdateDataSourceDetailsForID] 1, '1', '3', '2dfbj~ldhs~1~1~23756~13344~9869', '~~~~~'

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMUpdateDataSourceDetailsForID]
(
		@ID int 
		, @Type int
		, @Status int
		, @Address1 nvarchar(100)
		, @Address2 nvarchar(100)
		, @StateID int
		, @Zip nvarchar(50)
		, @CountryID int
		, @WorkNumber nvarchar(50)
		, @FaxNumber nvarchar(100)
		, @PrimaryContactFirstName varchar(50)
		, @PrimaryContactLastName varchar(50)
		, @PrimaryContactTitle varchar(50)
		, @PrimaryContactEmail varchar(100)
		, @PrimaryContactWorkNumber varchar(50)
		, @PrimaryContactCellNumber varchar(50)
		, @ErrorNumber int output
		, @ErrorMessage varchar(200) output
	)
AS 

SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRY

BEGIN TRAN

UPDATE 
	PM_DataSources
SET
	  StatusID = @Status
	, SourceTypeID = @Type
	, Address1 = @Address1
	, Address2 = @Address2
	, StateID = @StateID
	, Zip = @Zip
	, CountryID = @CountryID
	, WorkNumber = @WorkNumber	
	, FaxNumber = @FaxNumber
	, PrimaryContactFirstName = @PrimaryContactFirstName
	, PrimaryContactLastName = @PrimaryContactLastName
	, PrimaryContactTitle = @PrimaryContactTitle
	, PrimaryContactEmail = @PrimaryContactEmail
	, PrimaryContactWorkNumber = @PrimaryContactWorkNumber
	, PrimaryContactCellNumber = @PrimaryContactCellNumber
	
Where
	DataSourceID = @ID


COMMIT TRAN

END TRY
BEGIN CATCH
	
	SET @ErrorNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
	
	ROLLBACK TRAN

END CATCH;










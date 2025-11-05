





/****************************************************************************
Name :   [PMAddDataSourceFromThirdPartyID]
Date Created: 16-Nov-2006 
Purpose:  Import details of the Thirdparty into the DataSourceTable
Author: Sugandh Jain
Execution Statement : exec [PMUpdateDataSourceDetailsForID] 3, 'BNY'

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMAddDataSourceFromThirdPartyID]
(
		  @ID int 		
		, @Error int output
		, @ErrorMessage varchar(100) output
		

)
AS

SET @Error = 0
SET @ErrorMessage = 'Success'

BEGIN TRY

INSERT INTO
	PM_DataSources
(
	  Name
	, ShortName
	, Address1
	, Address2
	, StatusID
	, CountryID
	, StateID
	, Zip
	, WorkNumber
	, FaxNumber
	, PrimaryContactFirstName
	, PrimaryContactLastName
	, PrimaryContactTitle
	, PrimaryContactEMail
	, PrimaryContactWorkNumber
	, PrimaryContactCellNumber
)
SELECT 
	  ThirdPartyName AS [Name]
	, ShortName AS [ShortName]
	, Address1	AS [Address1]
	, Address2 AS [Address2]
	, 1 AS [Status] -- Set the status of the DataSource to Dormant
	, CountryID AS [ CountryID]
	, StateID AS [ StateID]
	, Zip AS [ Zip]
	, WorkTelephone AS [ WorkNumber]
	, Fax AS [ FaxNumber]
	, ContactPerson AS [PrimaryContactFirstName ]
	, ContactPersonLastName AS [PrimaryContactLastName ]
	, ContactPersonTitle AS [ PrimaryContactTitle]
	, Email AS [ PrimaryContactEMail]
	, ContactPersonWorkPhone AS [ PrimaryContactWorkNumber]
	, CellPhone AS [ PrimaryContactCellNumber]
FROM
	T_ThirdParty
WHERE
	ThirdPartyID = @ID
	 

SELECT 
		dataSourceID
		, Name
		, ShortName
FROM
		PM_DataSources
WHERE
		dataSourceID IN ( Select max(dataSourceID) from PM_DataSources)

END TRY
BEGIN CATCH
	SET @ERROR = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
END CATCH;















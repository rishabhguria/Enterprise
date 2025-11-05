


/****************************************************************************
Name :   [PMGetCompanyAdminContactDetailsForID]
Date Created: 20-Nov-2006 
Purpose:  Get the details for the userID sent
Author: Sugandh Jain
Execution Statement : exec [PMGetCompanyAdminContactDetailsForID] 2

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetCompanyAdminContactDetailsForID]
(
		@ID int
		, @ErrorMessage varchar(500) output
		, @ErrorNumber int output 

)
AS 


SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY


Select 	
	  
	  TCU.FirstName + ', ' + TCU.LastName AS [ AdminContactUserName ]
	, ISNULL(TCU.FirstName , '') AS  [ AdminContactFirstName ]
    , ISNULL(TCU.LastName , '') AS   [ AdminContactLastName] 
    , ISNULL(TCU.Title , '') AS  [ AdminContactTitle ]
	, ISNULL(TCU.Login , '') AS [ AdminContactUserID ]	
    , ISNULL(PMC.AdminPassword, '') AS [ AdminContactPassword ]
    , ISNULL(TCU.Email, '') AS [ AdminContactEmail ]    
    , ISNULL(TCU.TelphoneWork, '') AS [ AdminContactWorkNumber ]
    , ISNULL(TCU.TelphoneMobile, '') AS [ AdminContactCellNumber ]
    , ISNULL(TCU.TelphonePager, '') AS [ AdminContactPagerNumber ]
    , ISNULL(TCU.TelphoneHome, '') AS [ AdminContactHomeNumber ] 
    , ISNULL(TCU.Fax, '') AS [ AdminContactFaxNumber ]
	
From
	T_CompanyUser AS TCU 
	LEFT OUTER JOIN PM_Company AS PMC on TCU.UserID = PMC.AdminContactUserID and TCU.CompanyID = PMC.NOMSCompanyID
WHERE
	TCU.UserID = @ID
	AND
	ISNULL(TCU.IsActive, 1) = 1


END TRY
BEGIN CATCH
	SET @ERRORNumber = ERROR_NUMBER();
	SET @ErrorMessage = ERROR_MESSAGE();
END CATCH;








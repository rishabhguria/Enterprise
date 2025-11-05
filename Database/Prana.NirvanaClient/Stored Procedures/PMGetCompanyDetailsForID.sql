
	




/****************************************************************************
Name :   [PMGetDataSourceDetailsForID]
Date Created: 20-Nov-2006 
Purpose:  Get the details for the company for which the ID is sent.
Author: Sugandh Jain
Execution Statement : exec [PMGetCompanyDetailsForID] 1, '' , 0 

select * from PM_Company
select * from t_companyuser

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetCompanyDetailsForID]
(
		  @ID int
		, @ErrorMessage varchar(500) output
		, @ErrorNumber int output
)
AS 

SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0
--BEGIN TRAN TRAN1 

BEGIN TRY


Select 
	  TC.CompanyID AS [ID]
	, TC.Name AS [ FullName ]
	, TC.ShortName AS [ ShortName ]	
	, ISNULL(TC.CompanyTypeID, 0) AS [ companyTypeID ]
	, ISNULL(TC.Address1, '') AS [ Address1 ]
	, ISNULL(TC.Address2, '') AS [ Address2 ]
	, ISNULL(TC.CountryID, 0) as [ CountryID ]	
	, ISNULL(TC.StateID , 0) AS [ StateID ]	
	, ISNULL(TC.Zip , '')as [ Zip ]	
	, ISNULL(TC.Telephone , '') as [ WorkNumber ]
	, ISNULL(TC.Fax , '') as [ FaxNumber ]

	, TCU.FirstName + ', ' + TCU.LastName AS [ AdminContactLoginName ]
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
	
	, ISNULL(PMC.NofUserLicenses, '') AS [ NumberOfUserLicences ]
	, ISNULL(PMC.AdminContactUserID, '') AS [CompanyUserID]

FROM 
	T_Company as TC
	INNER JOIN PM_Company AS PMC ON TC.CompanyID = PMC.NOMSCompanyID
	LEFT OUTER JOIN T_CompanyUser AS TCU ON PMC.NOMSCompanyID = TCU.CompanyID AND PMC.AdminContactUserID = TCU.UserID
WHERE
	PMC.PMCompanyID = @ID
	AND
	ISNULL(TCU.IsActive, 1) = 1
	AND
	PMC.IsActive = 1

END TRY
BEGIN CATCH	
		
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();	
	
	
END CATCH;









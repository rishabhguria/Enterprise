



/****************************************************************************
Name :   [PMGetAllThirdPartyNames]
Date Created: 17-Nov-2006 
Purpose:  Get all the DataSources
Author: Sugandh Jain
Execution Statement : exec [PMGetAllThirdPartyNames]

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [dbo].[PMGetAllThirdPartyNames]
AS


Select 
	  ThirdPartyID AS [THIRDPARTYID]
	, ThirdPartyName AS [THIRDPARTYNAME]
	, ShortName AS [SHORTNAME]
From 
	dbo.T_ThirdParty
Order By 
	ThirdPartyName Asc





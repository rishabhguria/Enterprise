




/****************************************************************************
Name :   [PMGetSymbolConventionsList]
Date Created: 16-Feb-2007 
Purpose:  Get all the DataSources
Author: Sugandh Jain
Execution Statement : exec [PMGetAllThirdPartyNames]

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/
CREATE PROCEDURE [PMGetSymbolConventionsList]
AS

Select 
	  ID AS [ID]
	, Name AS [NAME]	
From 
	PM_SymbolConvention
Order By 
	Name Asc
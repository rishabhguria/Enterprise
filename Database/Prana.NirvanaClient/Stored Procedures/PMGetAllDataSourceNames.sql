/****************************************************************************  
Name :   PMAddNewDataSource  
Date Created: 10-oct-2006   
Purpose:  Get all the DataSources  
Author: Sugandh Jain  
Execution Statement : exec PMGetDataSources  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
Create PROCEDURE [dbo].[PMGetAllDataSourceNames]  
AS  
Select   
   ThirdPartyID AS [ThirdPartyID]  
 , ThirdPartyName AS ThirdPartyName  
 , ShortName AS ShortName  
From   
 T_ThirdParty  
Order By   
 ThirdPartyID Asc  
  
  
  
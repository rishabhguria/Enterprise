/****************************************************************************  
Name :   PMAddNewDataSource  
Date Created: 09-oct-2006   
Purpose:  Get all the DataSourceTypes  
Author: Sugandh Jain  
Execution Statement : exec PMGetDataSourceTypes  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
Create PROCEDURE [dbo].[PMGetDataSourceTypes]  
AS     
Select   
  ThirdPartyTypeID AS [DATASOURCETYPEID]  
 ,ThirdPartyTypeName AS [DATASOURCETYPENAME]  
From   
 T_ThirdPartyType  
Order By   
 ThirdPartyTypeName Asc  
  
  
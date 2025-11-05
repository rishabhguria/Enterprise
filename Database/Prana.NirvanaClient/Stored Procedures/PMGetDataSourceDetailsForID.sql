/****************************************************************************  
Name :   [PMGetDataSourceDetailsForID]  
Date Created: 10-oct-2006   
Purpose:  Get all the DataSources  
Author: Sugandh Jain  
Execution Statement : exec [PMGetDataSourceDetailsForID] 1  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
Create PROCEDURE [dbo].[PMGetDataSourceDetailsForID]  
(  
  @ID int  
)  
AS  
  
  
Select   
   PMD.ThirdPartyID AS [ID]  
 , PMD.ThirdPartyName AS [FullName]  
 , PMD.ShortName AS [ShortName]   
 , ISNULL(PMD.ThirdPartyTypeID, 0) AS [SourceTypeID]  
 , ISNULL(PMD.Address1, '') AS [Address1]  
 , ISNULL(PMD.Address2, '') AS [Address2]  
 , ISNULL(PMD.StateID , 0) AS [StateID]  
 , ISNULL(TS.State , '')as [State]  
 , ISNULL(PMD.Zip , '')as [Zip]  
 , ISNULL(PMD.CountryID, 0) as [CountryID]  
 , ISNULL(TC.CountryName , '') as [CountryName]  
 , ISNULL(PMD.WorkTelephone , '') as [WorkNumber]  
 , ISNULL(PMD.Fax , '') as [FaxNumber]  
 , ISNULL(PMD.ContactPerson , '') as [PrimaryContactFirstName]  
 , ISNULL(PMD.ContactPersonLastName , '') as [PrimaryContactLastName]  
 , ISNULL(PMD.ContactPersonTitle , '') as [PrimaryContactTitle]  
 , ISNULL(PMD.Email , '') as [PrimaryContactEmail]  
 , ISNULL(PMD.CellPhone , '') as [PrimaryContactCellNumber]  
 , ISNULL(PMD.ContactPersonWorkPhone , '') as [PrimaryContactWorkNumber]  
 , ISNULL(PMD.StatusID, 0) AS [StatusID]  
From   
 T_ThirdParty as PMD   
 LEFT OUTER JOIN T_State as TS on PMD.StateID = TS.StateID  
 left outer join T_Country as TC on PMD.CountryID = TC.CountryID  
Where  
 PMD.ThirdPartyID = @ID 

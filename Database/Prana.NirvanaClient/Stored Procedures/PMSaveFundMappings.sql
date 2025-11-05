/****************************************************************************        
Name :   PMAddUpdateAssetMappings        
Date Created: 24-Nov-2006         
Purpose:  Save Column Mapping        
Author: Sugandh Jain        
Parameters:         
  @Xml nText        
 , @ThirdPartyID int        
 , @CompanyID int         
        
Execution Statement :         
        
select * from t_companyFunds        
 exec [PMSaveFundMappings]         
 '<?xml version="1.0"?>        
<ArrayOfMappingItem xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">        
  <MappingItem>        
    <BrokenRulesCollection />        
    <SourceItemID>1</SourceItemID>        
    <SourceItemName>706710</SourceItemName>        
    <ApplicationItemId>1</ApplicationItemId>        
    <Lock>false</Lock>        
  </MappingItem>        
  <MappingItem>        
    <BrokenRulesCollection />        
    <SourceItemID>31</SourceItemID>        
    <SourceItemName>Infrasgdhgkajgsdh</SourceItemName>        
    <ApplicationItemId>2</ApplicationItemId>        
    <Lock>false</Lock>        
  </MappingItem>        
  <MappingItem>        
    <BrokenRulesCollection />        
    <SourceItemID>0</SourceItemID>        
    <SourceItemName>test</SourceItemName>        
    <ApplicationItemId>2</ApplicationItemId>        
    <ApplicationItemName>-- Select --</ApplicationItemName>        
    <ApplicationItemFullName />        
    <Lock>false</Lock>        
  </MappingItem>        
</ArrayOfMappingItem>', 1, 1, '',''        
        
Date Modified: 14-Dec-2006         
Description:     Introduced transaction handling, the SP will be successfull in all the steps or none.         
Modified By:     Sugandh Jain        
****************************************************************************/        
Create Proc [dbo].[PMSaveFundMappings]        
(        
   @Xml nText        
 , @ThirdPartyID int        
 , @CompanyID int        
 , @ErrorMessage varchar(500) output        
 , @ErrorNumber int output        
)        
AS         
        
SET @ErrorMessage = 'Success'        
SET @ErrorNumber = 0        
BEGIN TRAN TRAN1         
        
BEGIN TRY        
        
      
DECLARE @NOMSCompanyID int;      
SET @NOMScompanyID = (SELECT NOMSCompanyID from PM_Company where PMCompanyID= @CompanyID)      
        
DECLARE @handle int           
exec sp_xml_preparedocument @handle OUTPUT,@Xml           
        
--This code updates old data.        
UPDATE T_CompanyFunds        
SET         
 T_CompanyFunds.CompanyThirdPartyID = @ThirdPartyID         
         
FROM         
 OPENXML(@handle, '//MappingItem', 2)           
 WITH         
  (SourceItemID Integer, SourceItemName nvarchar(50), ApplicationItemId Integer)  XmlItem        
 WHERE         
  T_CompanyFunds.CompanyFundID = XmlItem.ApplicationItemId        
          
        
    
        
EXEC sp_xml_removedocument @handle        
        
COMMIT TRANSACTION TRAN1        
        
END TRY        
BEGIN CATCH         
 SET @ErrorMessage = ERROR_MESSAGE();        
 SET @ErrorNumber = Error_number();         
 ROLLBACK TRANSACTION TRAN1        
         
END CATCH;        
         
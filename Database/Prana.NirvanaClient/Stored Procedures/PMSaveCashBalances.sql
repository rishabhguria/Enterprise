/****************************************************************************                            
Name :   PMSaveCashBalances                            
Date Created: 14-May-2007  
Purpose:  Save Cash Balances                            
Author: Sugandh Jain                            
Parameters:                             
   @Xml nText                        
 , @ErrorMessage varchar(500) output                            
 , @ErrorNumber int output                             
                             
                            
Execution Statement :                           
exec PMSaveCashBalances '<?xml version="1.0"?>  
<ArrayOfCashBalance xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">  
  <CashBalance>  
    <BrokenRulesCollection />  
    <FundID>0</FundID>  
    <FundValue>  
      <BrokenRulesCollection />  
      <ID>0</ID>  
      <ShortName />  
      <FullName>Fund1</FullName>  
    </FundValue>  
    <PreviousBalance>10000</PreviousBalance>  
    <CurrentBalance>2000000</CurrentBalance>  
    <Date>0001-01-01T00:00:00</Date>  
    <DatasourceNameId>  
      <BrokenRulesCollection />  
      <ID>0</ID>  
      <FullName />  
      <ShortName />  
    </DatasourceNameId>  
  </CashBalance>  
  <CashBalance>  
    <BrokenRulesCollection />  
    <FundID>0</FundID>  
    <FundValue>  
      <BrokenRulesCollection />  
      <ID>0</ID>  
      <ShortName />  
      <FullName>fund2</FullName>  
    </FundValue>  
    <PreviousBalance>1222</PreviousBalance>  
    <CurrentBalance>111111</CurrentBalance>  
    <Date>0001-01-01T00:00:00</Date>  
    <DatasourceNameId>  
      <BrokenRulesCollection />  
      <ID>0</ID>  
      <FullName />  
      <ShortName />  
    </DatasourceNameId>  
  </CashBalance>  
</ArrayOfCashBalance>' , '', 0  
  
select * from PM_CompanyDatasourceCompanyFundCashBalance    
                            
                            
Date Modified: <>                            
Description:   <>                  
Modified By:   <>                            
****************************************************************************/                            
                           
Create PROCEDURE PMSaveCashBalances                            
                            
   @Xml nText                        
 , @ErrorMessage varchar(500) output                            
 , @ErrorNumber int output                            
                            
AS                             
BEGIN                            
                        
SET @ErrorMessage = 'Success'                            
SET @ErrorNumber = 0                            
                            
--BEGIN TRAN TRAN1                             
                          
BEGIN TRY                            
                
                            
DECLARE @handle int                               
exec sp_xml_preparedocument @handle OUTPUT,@Xml                               
                               
                            
insert into dbo.PM_CompanyDatasourceCompanyFundCashBalance                             
 (                            
   CompanyFundID                            
  , Date                             
  , CashBalance    
 )                            
Select                             
      TCF.CompanyFundID  
 , GETUTCDATE()  
 , UIData.CashBalance  
FROM   
 T_CompanyFunds TCF  
 INNER JOIN   
 (  
  SELECT     
  CurrentBalance AS CashBalance  
  , FullName AS FundFullName  
    
  FROM   
  OPENXML(@handle, '//CashBalance', 2)                               
  WITH          
  (                            
     CurrentBalance float  
  , FullName varchar(100) 'FundValue/FullName'                              
  )   
 )  
  AS UIData ON TCF.FundName  = UIDATA.FundFullName  
              
 exec sp_xml_removedocument @handle 
            
END TRY                            
BEGIN CATCH                             
 SET @ErrorMessage = ERROR_MESSAGE();                            
 SET @ErrorNumber = Error_number();        
                             
END CATCH;                            
                            
END         
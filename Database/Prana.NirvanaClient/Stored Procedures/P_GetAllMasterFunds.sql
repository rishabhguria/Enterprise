/* -- Fectching data based on company ID  
     --Author : omshiv   
    --Dated : 31 March 2014    
*/  
  
/*Purpose: To get all the master funds available in the database.  
  Date: 17th July, 2008  
  Execution: exec P_GetAllPermissions  
  Name: Bhupesh Bareja  
*/  
  
CREATE PROCEDURE [dbo].[P_GetAllMasterFunds]  
(  
@companyID int  
)  
AS  
  
SELECT   CompanyMasterFundID, MasterFundName  
FROM         T_CompanyMasterFunds where companyID =@companyID and IsActive=1 
   

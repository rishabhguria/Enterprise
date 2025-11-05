/**  
Author: Aman Seth  
Date: 09/11/2014 11:04:00  
Description: 
**/  
CREATE PROCEDURE [dbo].[P_GetClosingPreferences_New]      
AS      
BEGIN    
SELECT   
CompanyFundID as FundID
,ClosingMethodology as  ClosingMethodology
,SecSortCriteriaID  as SecondarySort 
from T_CompanyFunds   
Where IsActive=1 
end

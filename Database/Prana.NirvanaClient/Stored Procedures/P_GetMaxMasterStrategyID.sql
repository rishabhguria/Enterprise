/**  
Author: Faisal Shah 
Date: 20/06/2014  
**/  
CREATE PROCEDURE [dbo].[P_GetMaxMasterStrategyID]    
AS  
BEGIN  
  
SELECT MAX(CompanyMasterStrategyID)+1 from T_CompanyMasterStrategy  
      
             
END 

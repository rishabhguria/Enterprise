/**  
Author: Faisal Shah 
Date: 03/07/2014  
**/  
CREATE PROCEDURE [dbo].[P_GetMaxStrategyID]    
AS  
BEGIN  
  
SELECT MAX(CompanyStrategyID)+1 from T_CompanyStrategy  

                  
END 


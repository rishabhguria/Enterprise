-- =============================================  
-- Author:  <Sandeep Singh>  
-- Create date: <13 August 2009>  
-- Description: <it returns pairs of Company Strategies and master Strategy that the comp. Strategy belongs to>  
-- =============================================  
CREATE PROCEDURE GetCompanyStrategyMasterStrategyRelationShip  
AS  
BEGIN  
   
Select 
T_CompanyStrategy.CompanyStrategyID,
T_CompanyStrategy.StrategyName,
T_CompanyMasterStrategy.CompanyMasterStrategyID,
T_CompanyMasterStrategy.MasterStrategyName 
	From T_CompanyStrategy 
	Inner Join T_CompanyMasterStrategySubAccountAssociation   
	on T_CompanyStrategy.CompanyStrategyID = T_CompanyMasterStrategySubAccountAssociation.CompanyStrategyID  
	Join T_CompanyMasterStrategy on 
	T_CompanyMasterStrategySubAccountAssociation.CompanyMasterStrategyID =  T_CompanyMasterStrategy.CompanyMasterStrategyID  		  
   
END 
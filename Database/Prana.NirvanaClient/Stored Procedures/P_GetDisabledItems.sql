    
/*            
Modified By: Ankit Gupta on 18 Feb, 2015
Description: For enable-disable items UI, get list of disabled Third Parties.    
JIRA: http://jira.nirvanasolutions.com:8080/browse/chmw-2496
*/            
CREATE PROCEDURE [dbo].[P_GetDisabledItems]              
(          
 @ItemType int                            
 )                   
AS                   
     
IF @ItemType = 1    
BEGIN    
SELECT tc.CompanyID as ID, tc.Name, tc.ShortName,    
ta.ExecutionTime, cu.ShortName as DeletedBy from  T_Company tc inner join  T_AdminAuditTrail ta    
ON tc.CompanyID = ta.AuditDimensionValue  
INNER JOIN T_CompanyUser cu ON ta.UserId = cu.UserID    
where tc.Isactive = 0 and ta.ActionID = 33    
END    
ELSE IF @ItemType = 2    
BEGIN    
SELECT mf.CompanyMasterFundID as ID, mf.MasterFundname, mf.MasterFundName as ShortName,  
ta.ExecutionTime, cu.ShortName as DeletedBy from  T_CompanyMasterFunds mf inner join  T_AdminAuditTrail ta    
ON mf.CompanyMasterFundID = ta.AuditDimensionValue  
INNER JOIN T_CompanyUser cu ON ta.UserId = cu.UserID    
where mf.Isactive = 0 and ta.ActionID = 13  
END     
    
ELSE IF @ItemType = 3    
BEGIN    
SELECT tc.CompanyFundID as ID, tc.FundName, tc.FundShortName,    
ta.ExecutionTime, cu.ShortName as DeletedBy from  T_CompanyFunds tc inner join  T_AdminAuditTrail ta    
ON tc.CompanyFundID = ta.AuditDimensionValue  
INNER JOIN T_CompanyUser cu ON ta.UserId = cu.UserID    
where tc.Isactive = 0 and ta.ActionID = 29  
END     
    
ELSE IF @ItemType = 4    
BEGIN    
SELECT tc.CompanyMasterStrategyID as ID, tc.MasterStrategyName, tc.MasterStrategyName AS ShortName,    
ta.ExecutionTime, cu.ShortName as DeletedBy from  T_CompanyMasterStrategy tc inner join  T_AdminAuditTrail ta    
ON tc.CompanyMasterStrategyID = ta.AuditDimensionValue  
INNER JOIN T_CompanyUser cu ON ta.UserId = cu.UserID    
where tc.Isactive = 0 and ta.ActionID = 37  
END     
    
ELSE IF @ItemType = 5    
BEGIN    
SELECT tc.CompanyStrategyID as ID, tc.StrategyName, tc.StrategyName AS ShortName,    
ta.ExecutionTime, cu.ShortName as DeletedBy from  T_CompanyStrategy tc inner join  T_AdminAuditTrail ta    
ON tc.CompanyStrategyID = ta.AuditDimensionValue  
INNER JOIN T_CompanyUser cu ON ta.UserId = cu.UserID    
where tc.Isactive = 0 and ta.ActionID = 49   
END                                                                                                               
    
ELSE IF @ItemType = 6    
BEGIN    
SELECT tc.UserID as ID, tc.FirstName, tc.ShortName,    
ta.ExecutionTime, cu.ShortName as DeletedBy from  T_CompanyUser tc inner join  T_AdminAuditTrail ta    
ON tc.UserID = ta.AuditDimensionValue  
INNER JOIN T_CompanyUser cu ON ta.UserId = cu.UserID    
where tc.Isactive = 0 and ta.ActionID = 5     
END         
    
ELSE IF @ItemType = 7    
BEGIN    
SELECT tc.ThirdPartyID as ID, tc.ThirdPartyName, tc.ShortName,    
ta.ExecutionTime, cu.ShortName as DeletedBy from  T_ThirdParty tc inner join  T_AdminAuditTrail ta    
ON tc.ThirdPartyID = ta.AuditDimensionValue  
INNER JOIN T_CompanyUser cu ON ta.UserId = cu.UserID    
where tc.Isactive = 0 and ta.ActionID = 41     
END    

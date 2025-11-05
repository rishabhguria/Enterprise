/****************************                              
Author : Pooja Porwal                              
Creation date : 32-Oct-2015                               
Description :Compliance Alert History                           
Jira Link: http://jira.nirvanasolutions.com:8080/browse/PRANA-11643          
EXEC [P_ComplianceAlertHistory] '2011/5/1','2015/10/1',0,0   

Modified By : Pooja Porwal
Description : Add System alert and deleted rules
Jira Link: http://jira.nirvanasolutions.com:8080/browse/PRANA-12749       
          
****************************/                              
CREATE Procedure [dbo].[P_ComplianceAlertHistory]    
 (              
 @StartDate DATETIME              
 ,@EndDate DATETIME
 ,@SystemAlerts bit
 ,@DeletedRules bit         
             
 )              
    
AS              
BEGIN    


;WITH AA as
(
Select
CA.RuleId,     
CASE
	WHEN CA.RuleId='-1' 
	then 'System alert'
	Else COALESCE(CR.RuleName,'Rule Deleted','')	
END
 as RuleName
,CA.RuleType
,CA.ValidationTime    
,CA.Summary    
,CA.[Description]    
,CA.Parameters  
,COALESCE(CR.IsDeleted,0)  as IsDeleted 
from T_CA_AlertHistory CA left outer JOIN T_CA_RulesUserDefined CR    
ON CA.RuleID=CR.RuleID
where 
CA.ValidationTime between @StartDate AND @EndDate+1
and (CA.RuleId!= CASE WHEN @SystemAlerts=0 then '-1' ELSE '0'  END) 
)

SELECT * from AA 
where 
(((@DeletedRules=1 and IsDeleted in (0,1) )) 
or (@DeletedRules=0 and IsDeleted =0))

END
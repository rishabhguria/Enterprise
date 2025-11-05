--- To Delete All Rule Accordingly CompanyId
CREATE PROCEDURE [dbo].[P_DeletCompanyCVAUECFundCommissionRules](     
  @companyID int    
)           
AS      
DELETE FROM T_CommissionRulesForCVAUEC         
WHERE CompanyID = @companyID       

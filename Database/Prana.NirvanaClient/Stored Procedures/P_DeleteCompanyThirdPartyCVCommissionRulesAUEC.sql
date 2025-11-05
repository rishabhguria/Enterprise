 /****** Object:  Stored Procedure dbo.P_DeleteCompanyThirdPartyCVCommissionRulesAUEC    Script Date: 04/24/2006 5:30:24 PM ******/  
CREATE PROCEDURE dbo.P_DeleteCompanyThirdPartyCVCommissionRulesAUEC  
(  
 @companyID int,  
 @companyAUECID varchar(MAX)
)  
AS  
  
 declare @abc int  
 Set @abc = 0  
   
 exec ('Delete T_CompanyThirdPartyCVCommissionRules Where CVAUECID not in  
  (Select CTPCCR.CVAUECID FROM T_CVAUEC CVA inner join T_CompanyThirdPartyCVCommissionRules CTPCCR   
 on CTPCCR.CVAUECID = CVA.CVAUECID inner join T_AUEC AUEC on CVA.AUECID = AUEC.AUECID, T_CompanyAUEC CA   
 where CA.CompanyID = ' + @companyID + ' AND CA.CompanyAUECID in (' + @companyAUECID + '))')  
  
  
/* Delete T_CompanyThirdPartyCVCommissionRules Where @abc < (Select CVA.AUECID From T_CompanyAUEC CA, T_CVAUEC CVA inner join T_CompanyThirdPartyCVCommissionRules CTPCCR  
 on CVA.CVAUECID = CTPCCR.CVAUECID inner join T_AUEC AUEC on CVA.AUECID = AUEC.AUECID   
 where CA.CompanyID = ' + @companyID + ' AND convert(varchar, CA.CompanyAUECID) not in (' + @companyAUECID + ')) */  
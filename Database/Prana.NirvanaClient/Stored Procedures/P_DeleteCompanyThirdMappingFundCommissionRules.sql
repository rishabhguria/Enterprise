/****** Object:  Stored Procedure dbo.P_DeleteCompanyThirdMappingFundCommissionRules    Script Date: 06/04/2006 08:40:22 PM ******/
CREATE PROCEDURE [dbo].[P_DeleteCompanyThirdMappingFundCommissionRules]
(		
       @CompanyId int		 
)
AS 
Declare @total int 
Set @total = 0

Delete from T_CompanyThirdPartyCVCommissionRules Where  CompanyFundID in
(Select CompanyFundID from T_CompanyFunds Where CompanyID=@CompanyID)






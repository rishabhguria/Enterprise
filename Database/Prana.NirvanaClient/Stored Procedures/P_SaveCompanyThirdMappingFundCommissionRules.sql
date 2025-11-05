

/****** Object:  Stored Procedure dbo.P_SaveCompanyThirdMappingFundCommissionRules    Script Date: 06/04/2006 08:40:22 PM ******/
CREATE PROCEDURE [dbo].[P_SaveCompanyThirdMappingFundCommissionRules]
(
		@CompanyFundID int,
		@CompanyCounterPartyCVID int,
		@CVAUECID int,
		@SingleRuleID int,
		@BasketRuleID int,
		@result int       	 
)
AS 
Declare @total int 
Set @total = 0
--Insert T_CompanyThirdPartyCVCommissionRules
begin
	INSERT T_CompanyThirdPartyCVCommissionRules(CompanyFundID, CompanyCounterPartyCVID, CVAUECID, SingleRuleID, BasketRuleID)
	Values(@CompanyFundID, @CompanyCounterPartyCVID, @CVAUECID, @SingleRuleID, @BasketRuleID)  
					
				Set @result = @CompanyFundID
end
select @result
 





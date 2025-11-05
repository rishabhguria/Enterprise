CREATE PROCEDURE dbo.P_DeleteCommissionRuleCriteriaUp
	(
		
		@CommissionCriteriaID_FK int		
	)
AS
	Declare @result int
	set @result = 1

	Delete From T_CommissionRuleCriteriaUpdated Where CommissionCriteriaID_FK=@CommissionCriteriaID_FK
	
select @result	


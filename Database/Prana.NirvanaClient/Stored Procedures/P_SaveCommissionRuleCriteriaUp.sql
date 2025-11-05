

CREATE PROCEDURE [dbo].[P_SaveCommissionRuleCriteriaUp] (
		
		@CommissionCriteriaID_FK int,
		@ValueFrom bigint, 
		@ValueTo bigint,
		@CommissionRateID_FK int,
		@CommisionRate Decimal(18,4)
	)
AS
	Declare @result int
	set @result = 1

Insert INTO  T_CommissionRuleCriteriaUpdated (CommissionCriteriaID_FK,ValueFrom,ValueTo,CommissionRateID_FK,CommisionRate) 
             Values(@CommissionCriteriaID_FK,@ValueFrom,@ValueTo,@CommissionRateID_FK,@CommisionRate)
	
	
select @result


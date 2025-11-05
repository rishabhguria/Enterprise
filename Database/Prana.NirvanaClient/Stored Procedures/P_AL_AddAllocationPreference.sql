
CREATE PROCEDURE [dbo].[P_AL_AddAllocationPreference]
(
	@CompanyId		INT,
	@NAME			NVARCHAR(200),	
	@IsPrefVisible	BIT,
	@FileName		NVARCHAR(200)
)
AS

DECLARE @intErrorCode INT,@NewPresetDefId int,@AllocationBase int,@MatchingRule int,@PreferencedFund int,@MatchPortfolio int,@ProrataFundList nvarchar(max)
,@ProrataDaysBack int,@PositionPriority INT,@defaultRuleExist int

select @AllocationBase=1,@MatchingRule =1,@PreferencedFund = null,@MatchPortfolio= 0,@ProrataFundList='',@ProrataDaysBack= 0
,@defaultRuleExist= count(*)	
from T_AL_AllocationDefaultRule
Where CompanyId = @CompanyId

if(@defaultRuleExist>0)
begin

Select 
	@AllocationBase=[AllocationBase],
	@MatchingRule = case
		when [MatchingRule] IN (4,5,6) then 1
		else [MatchingRule] end,
	@MatchPortfolio=[MatchPortfolioPosition],
	@PreferencedFund=[PreferencedFundId],
	@ProrataFundList = [ProrataFundList],
	@ProrataDaysBack=[ProrataDaysBack]
from T_AL_AllocationDefaultRule
Where CompanyId = @CompanyId

end
-------------------------------------------------------------------------------
-------------------------------------------------------------------------------
SET @PositionPriority = ISNULL((SELECT MAX(PositionPriority) FROM T_AL_AllocationPreferenceDef),0) + 1

insert into T_AL_AllocationPreferenceDef
	(
		[Name],[CompanyId],[AllocationBase],[MatchingRule],[MatchPortfolioPosition],
		[PreferencedFundId],[UpdateDateTime],[ProrataDaysBack], [PositionPriority], [IsPrefVisible],[RebalancerFileName]
	)
VALUES( @NAME, @CompanyId, @AllocationBase,@MatchingRule,@MatchPortfolio,null,GETDATE(),
		@ProrataDaysBack, @PositionPriority, @IsPrefVisible, @FileName )
SELECT @NewPresetDefId =SCOPE_IDENTITY()

Exec P_AL_GetAllocationPreferenceById @NewPresetDefId



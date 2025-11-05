
Create PROCEDURE [dbo].[P_CA_GetCompliancePermissionsUser] 
	
	@CompanyId int, 
	@UserId int 
AS
BEGIN
	Select
	OTHER.UserId, 
	OTHER.CompanyId,
	OTHER.PowerUser ,
	OTHER.IsApplyToManual ,
    OTHER.IsOverridePermission ,
	OTHER.Trading as IsTrading,
	OTHER.Staging as IsStaging,
    OTHER.IsPreTradeEnabled as IsPreTradeCheckEnabled ,
    RWPOST.IsCreate as PostTradeIsCreate,
    RWPOST.IsRename as PostTradeIsRename,
    RWPOST.IsDelete as PostTradeIsDelete,
    RWPOST.IsEnable as PostTradeIsEnable,
    RWPOST.IsExport as PostTradeIsExport,
    RWPOST.IsImport as PostTradeIsImport,
    RWPRE.IsCreate as PreTradeIsCreate,
    RWPRE.IsRename as PreTradeIsRename,
    RWPRE.IsDelete as PreTradeIsDelete,
    RWPRE.IsEnable as PreTradeIsEnable,
    RWPRE.IsExport as PreTradeIsExport,
    RWPRE.IsImport as PreTradeIsImport,
	OTHER.DefaultPrePopUp as DefaultPrePopUp,
	OTHER.DefaultPostPopUp as DefaultPostPopUp,
	OTHER.DefaultRuleOverrideType as DefaultRuleOverrideType,
	OTHER.EnableBasketComplianceCheck as EnableBasketComplianceCheck
    
from 
T_CA_UserReadWritePermission as RWPRE join
T_CA_UserReadWritePermission as RWPOST 
	on RWPOST.UserId = RWPRE.UserId and RWPOST.CompanyId = RWPRE.CompanyId join
T_CA_OtherCompliancePermission as OTHER
	on OTHER.UserId = RWPRE.UserId and OTHER.CompanyId = RWPRE.CompanyId

Where OTHER.UserId = @UserId and OTHER.CompanyId = @CompanyId and RWPRE.RuleType='PreTrade' and RWPOST.RuleType='PostTrade'

END


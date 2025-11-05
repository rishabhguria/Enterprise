
CREATE PROCEDURE [dbo].[P_CA_GetCompliancePermissionCompany] 
@CompanyId	INT 
AS
BEGIN
	SELECT	OCP.UserId, 
			OCP.CompanyId,
			OCP.PowerUser ,
			OCP.IsApplyToManual ,
			OCP.IsOverridePermission ,
			OCP.IsPreTradeEnabled AS IsPreTradeCheckEnabled ,
			OCP.Trading AS IsTrading,
			OCP.Staging AS IsStaging,
			RWPOST.IsCreate AS PostTradeIsCreate,
			RWPOST.IsRename AS PostTradeIsRename,
			RWPOST.IsDelete AS PostTradeIsDelete,
			RWPOST.IsEnable AS PostTradeIsEnable,
			RWPOST.IsExport AS PostTradeIsExport,
			RWPOST.IsImport AS PostTradeIsImport,
			RWPRE.IsCreate AS PreTradeIsCreate,
			RWPRE.IsRename AS PreTradeIsRename,
			RWPRE.IsDelete AS PreTradeIsDelete,
			RWPRE.IsEnable AS PreTradeIsEnable,
			RWPRE.IsExport AS PreTradeIsExport,
			RWPRE.IsImport AS PreTradeIsImport,
			OCP.DefaultPrePopUp AS DefaultPrePopUp,
			OCP.DefaultPostPopUp AS DefaultPostPopUp,
			OCP.DefaultRuleOverrideType AS DefaultRuleOverrideType,
			OCP.EnableBasketComplianceCheck AS EnableBasketComplianceCheck    
	FROM	T_CA_UserReadWritePermission AS RWPRE 
	JOIN	T_CA_UserReadWritePermission AS RWPOST 
			ON RWPOST.UserId = RWPRE.UserId 
			AND RWPOST.CompanyId = RWPRE.CompanyId 
	JOIN	T_CA_OtherCompliancePermission AS OCP
			ON OCP.UserId = RWPRE.UserId 
			AND OCP.CompanyId = RWPRE.CompanyId
	WHERE	OCP.CompanyId = @CompanyId 
			AND RWPRE.RuleType='PreTrade' 
			AND RWPOST.RuleType='PostTrade'
END

CREATE PROCEDURE [dbo].[P_CA_GetScheduleAlertHis]
AS
BEGIN
SELECT
ValidationTime,
Dimension,
RuleId,
Parameters,
RuleName,
UserId,
RuleType,
Summary,
CompressionLevel,
[Status],
[Description],
ActionUser,
PreTradeType,
PreTradeActionType from
(SELECT AlertHis.*,RuleUser.RuleName,ROW_NUMBER() OVER (
	PARTITION BY AlertHis.RuleId, AlertHis.Dimension
	ORDER BY AlertHis.ValidationTime DESC
) AS row_num
FROM T_CA_AlertHistory AlertHis LEFT OUTER JOIN T_CA_RulesUserDefined RuleUser ON AlertHis.RuleId = ruleUser.RuleID
WHERE DATEDIFF(DAY, AlertHis.ValidationTime, GETDATE()) = 0 AND AlertHis.RuleType = 'PostTrade')  as a where row_num=1
END
CREATE PROCEDURE [dbo].[P_CA_GetRuleValidationTime]   
AS
Begin
CREATE Table #AlertHistoryTable
(
RuleID varchar(50),
Dimension varchar(50),
ValidationTime datetime
)

CREATE Table #ArchivedAlertHistoryTable
(
RuleID varchar(50),
Dimension varchar(50),
ValidationTime datetime
)


INSERT INTO #AlertHistoryTable(RuleID, Dimension, ValidationTime)
SELECT RuleID,Dimension, Max(ValidationTime) AS ValidationTime  
FROM T_CA_AlertHistory as t  
WHERE  t.RuleType='PostTrade'   
GROUP BY t.RuleID,t.Dimension


INSERT INTO #ArchivedAlertHistoryTable(RuleID, Dimension, ValidationTime)
SELECT RuleID,Dimension, Max(ValidationTime) AS ValidationTime  
FROM T_CA_ArchivedAlertHistory as t  
WHERE  t.RuleType='PostTrade' and t.ArchiveType=1
GROUP BY t.RuleID,t.Dimension,t.ArchiveType

select

CASE when coalesce(A.ValidationTime,'1/1/1800')>coalesce(B.ValidationTime,'1/1/1800')
		THEN A.ValidationTime
	ELSE B.ValidationTime
	END as validationtime,

CASE when coalesce(A.ValidationTime,'1/1/1800')>coalesce(B.ValidationTime,'1/1/1800')
		THEN A.RuleID
	ELSE B.RuleID
	END as RuleID,

CASE when coalesce(A.ValidationTime,'1/1/1800')>coalesce(B.ValidationTime,'1/1/1800')
		THEN A.Dimension
	ELSE B.Dimension
	END as Dimension
  --A.RuleID, A.Dimension
FROM #AlertHistoryTable as A full join
#ArchivedAlertHistoryTable as B on A.RuleID=B.RuleID AND A.Dimension=B.Dimension

END

DROP TABLE #ArchivedAlertHistoryTable
DROP TABLE #AlertHistoryTable


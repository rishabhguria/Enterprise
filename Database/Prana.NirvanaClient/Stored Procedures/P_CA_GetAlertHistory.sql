  
CREATE PROCEDURE [dbo].[P_CA_GetAlertHistory](  
 @dateFrom			DATETIME=NULL,  
 @dateTo			DATETIME=NULL,  
 @pageSize			INT=NULL,  
 @pageNO			INT=NULL,  
 @sortedColumnName	VARCHAR(30),
 @filteredColumns	VARCHAR(MAX), 
 @totalRows			INT=NULL OUTPUT  
)  
AS  
 /* SELECT   PermissionID, PermissionName  
FROM         T_Permission */  
BEGIN  
DECLARE @SQL_SCRIPT VARCHAR(MAX) 
CREATE TABLE #AlertHistoryPaging
(RowNum					INT,  
 [Name]					VARCHAR(50),  
 [User Name]			VARCHAR(100),
 [Rule Type]			VARCHAR(100),  
 [Summary]				VARCHAR(MAX),  
 [Compression Level]	VARCHAR(100),  
 [Parameters]			VARCHAR(MAX),  
 [Validation Time]		DATETIME,  
 [IsDeleted]			BIT,  
 [OrderId]				VARCHAR(50),
 [Status]				VARCHAR(100),  
 [Description]			VARCHAR(MAX),  
 [Dimension]			VARCHAR(50),  
 [RuleId]				VARCHAR(50),  
 [PreTradeType]			VARCHAR(50),
 [ComplianceOfficerNotes] VARCHAR(MAX),
 [UserNotes] VARCHAR(MAX),
 [TradeDetails] VARCHAR(MAX),
 [AlertPopUpResponse] INT)

IF @dateFrom IS NULL  
SET @dateFrom=GETDATE();  
IF @dateTo IS NULL  
SET @dateTo=GETDATE();  
IF @pageNO IS NULL  
SET @pageNO=1;  
IF @pageSize IS NULL  
SET @pageSize=50;  
IF @totalRows IS NULL  
SET @totalRows=0;  
IF @sortedColumnName IS NULL
SET @sortedColumnName = 'ValidationTime';
IF @filteredColumns IS NULL OR LTrim(RTrim(@filteredColumns)) = ''
SET @filteredColumns = '1=1' ;
 
SET @SQL_SCRIPT = ' INSERT into #AlertHistoryPaging (RowNum,[Name], [User Name], [Rule Type], [Summary], [Compression Level], [Parameters],[Validation Time], [IsDeleted], [OrderId], [Status], [Description], [Dimension], [RuleId], [PreTradeType], [ComplianceOfficerNotes],[UserNotes],[TradeDetails],[AlertPopUpResponse])
SELECT     
 ROW_NUMBER() OVER(ORDER BY ValidationTime) AS RowNum ,  
 coalesce(r.RuleName,''N/A'') AS [Name],  
 coalesce(c.FirstName,''N/A'') AS [User Name],  
 RuleType AS [Rule Type],  
 [Summary],  
 CompressionLevel AS [Compression Level],  
 [Parameters],  
 ValidationTime AS [Validation Time],  
 r.IsDeleted  AS [IsDeleted],  
 CASE RuleType  
  WHEN ''PreTrade'' THEN OrderId  
  WHEN ''PostTrade'' THEN ''N/A''  
 END as ''OrderId'',  
 [Status],  
 coalesce(Description,''N/A'') AS [Description],  
 coalesce(Dimension,''N/A'') AS [Dimension],  
 a.RuleId AS [RuleId],  
 a.PreTradeType AS [PreTradeType],
 a.ComplianceOfficerNotes AS [ComplianceOfficerNotes],
 a.UserNotes AS [UserNotes],
 a.TradeDetails AS [TradeDetails],
 a.AlertPopUpResponse AS [AlertPopUpResponse]
FROM   T_CA_AlertHistory a  
LEFT OUTER JOIN T_CA_RulesUserDefined r ON a.RuleId = r.RuleID  
LEFT OUTER JOIN T_CompanyUser c ON a.UserId = c.UserID  
WHERE Datediff(d, ''' + CONVERT(VARCHAR(50),@dateFrom)+ ''', a.validationTime) >= 0
AND Datediff(d, a.validationTime, ''' + CONVERT(VARCHAR(50),@dateTo)+ ''') >= 0 AND ' + @filteredColumns + '
ORDER BY '+ @sortedColumnName

EXEC (@SQL_SCRIPT)

SELECT [Name],   
[User Name],  
[Rule Type],  
[Summary],  
[Compression Level],  
[Parameters],  
[Validation Time],  
OrderId,  
[Status],  
[Description],  
[Dimension],  
[RuleId],  
(CASE [IsDeleted] WHEN 1 THEN 'True' WHEN 0 THEN 'False' ELSE 'False' END) as RuleDeleted,
--coalesce([IsDeleted],'False') as RuleDeleted,   
[PreTradeType] AS PreTradeType,
[ComplianceOfficerNotes] AS ComplianceOfficerNotes,
[UserNotes] AS UserNotes,
[TradeDetails] AS TradeDetails,
[AlertPopUpResponse]
FROM #AlertHistoryPaging     
WHERE RowNum BETWEEN (((@pageNO-1)*@pageSize)+1) AND (@pageNO*@pageSize)

SET @totalRows = (SELECT count(*) FROM #AlertHistoryPaging)    
  
DROP TABLE #AlertHistoryPaging
  
END
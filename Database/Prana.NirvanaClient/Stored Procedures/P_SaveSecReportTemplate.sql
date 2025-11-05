
-- =============================================  
-- Created by: Bharat raturi
-- Date: 30 apr 2014
-- Purpose: Save report template for security management 
-- Usage: P_SaveSecReportTemplate 0,reppp, 
-- =============================================
CREATE procedure [dbo].[P_SaveSecReportTemplate]
@reportID int,
@reportName varchar(50),
@startDate datetime,
@endDate datetime,
@thirdPartyID varchar(max),
@fund varchar(max),
@columns varchar(max),
@groupingby varchar(max),
@cronExp varchar(100),
@formatType varchar(50),
@whereClause varchar(max)
as
declare @countrec int 
select @countrec= COUNT(*) from T_ReportTemplate where ReportID=@reportID
IF (@countrec>0)
update T_ReportTemplate
SET
ReportName=@reportName,
StartDate=@startdate,
EndDate=@endDate,
ThirdPartyID=@thirdpartyID,
Funds=@fund,
[Columns]=@columns,
GroupingBy=@groupingby,
CronExpression=@cronExp,
FormatType=@formatType,
WhereClause=@whereClause
where ReportID=@reportID
else
insert INTO
T_ReportTemplate
(
ReportName,
StartDate,
EndDate,
ThirdPartyID,
Funds,
[Columns],
GroupingBy,
CronExpression,
FormatType,
WhereClause
)   
VALUES
(
@reportName,
@startDate,
@endDate,
@thirdPartyID,
@fund,
@columns,
@groupingby,
@cronExp,
@formatType,
@whereClause
)


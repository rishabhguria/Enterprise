-- =============================================  
-- Created by: Bharat raturi
-- Date: 01 may 2014
-- Purpose: get  report details from DB 
-- Usage: P_GetSecReportDetails 17
-- =============================================
CREATE procedure [dbo].[P_GetSecReportDetails]
@reportID int
as
select 
ReportID, 
ReportName, 
StartDate, 
EndDate, 
ThirdPartyID, 
Funds, 
[Columns], 
GroupingBy, 
CronExpression, 
FormatType,
WhereClause,
LastRunTime  
from T_ReportTemplate
where ReportID=@reportID
--declare @columnSet varchar(max)
--declare @groupingcolumns varchar(200)
--declare @selectData varchar(2000)
--select @columnSet= [Columns] from T_ReportTemplate where ReportID=@reportID 
--select @groupingcolumns= GroupingBy from T_ReportTemplate where ReportID= @reportID 
--set @selectData='select '+@columnSet+' from V_SecMasterData'-- group BY ' + @groupingcolumns
--print @selectData
--exec (@selectData)


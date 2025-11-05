-- =============================================  
-- Purpose: update report template from database 
-- Usage: P_UpdateSecReportTemplate id
-- =============================================
CREATE procedure [dbo].[P_UpdateSecReportTemplate]
@reportID int
as
Update T_ReportTemplate SET LastRunTime = GETDATE() where ReportID=@reportID


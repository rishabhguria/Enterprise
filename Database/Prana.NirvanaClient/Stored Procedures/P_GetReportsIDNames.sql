-- =============================================  
-- Created by: Bharat raturi
-- Date: 30 apr 2014
-- Purpose: get reportID_names from DB 
-- Usage: P_GetReportsIDNames 
-- =============================================
CREATE procedure P_GetReportsIDNames
as
select ReportID, ReportName from T_ReportTemplate

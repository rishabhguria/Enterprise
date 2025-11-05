-- =============================================  
-- Created by: Bharat raturi
-- Date: 01 may 2014
-- Purpose: Delete report template from database 
-- Usage: P_DeleteSecReport
-- =============================================
CREATE procedure [dbo].[P_DeleteSecReport]
@reportID int
as
delete from T_ReportTemplate where ReportID=@reportID

-- ==========================================================    
-- Modified by: Bhavana  
-- Date: 15 July 2014  
-- Purpose: SM batch type renamed to Fields in T_SMBatchSetup   
-- ==========================================================   
-- Modified by: Bharat raturi  
-- Date: 16 may 2014  
-- Purpose: get  SM batch details from DB   
-- Usage: P_GetSMBatchDetails  
-- =============================================  
Create procedure [dbo].[P_GetSMBatchDetails]  
as  
select SMBatchID, SystemLevelName, CronExpression, Fields, RunTimeTypeID, IsHistoricDataRequired ,DaysOfHistoricData   
, UserDefinedName, FundID, Indices, FilterClause, BatchType from T_SMBatchSetup    

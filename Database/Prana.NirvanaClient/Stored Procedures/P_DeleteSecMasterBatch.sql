-- =============================================  
-- Created by: Bharat raturi
-- Date: 16 may 2014
-- Purpose: get  SM batch details deleted from DB 
-- Usage: P_DeleteSecMasterBatch
-- =============================================
CREATE procedure P_DeleteSecMasterBatch
@smBatchID int
as
delete from T_SMBatchSetup where SMBatchID=@smBatchID

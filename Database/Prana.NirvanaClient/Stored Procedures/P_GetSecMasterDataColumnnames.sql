-- =============================================  
-- Created by: Bharat raturi
-- Date: 30 apr 2014
-- Purpose: Get V_SecmasterData column names for report 
-- Usage: P_GetSecMasterDataColumnnames
-- =============================================
Create procedure P_GetSecMasterDataColumnnames
as
SELECT COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='V_SecMasterData'  

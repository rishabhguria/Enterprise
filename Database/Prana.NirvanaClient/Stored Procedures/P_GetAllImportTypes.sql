--------------------------------------------------------
--Created By: Bharat Raturi
--Date: 01/04/14
--Purpose: Get all the schedule types
--usage P_GetAllImportTypes
---------------------------------------------------------

CREATE procedure [dbo].[P_GetAllImportTypes]
as
select TableTypeID, TableTypeName from PM_TableTypes

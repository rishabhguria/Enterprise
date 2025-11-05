create procedure [dbo].[P_GetAllBatchFormats]
as
select TableTypeID, TableTypeName from PM_TableTypes

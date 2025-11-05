--------------------------------------------------------
--Created By: Aman Seth
--Date: 18/04/14
--Purpose: Get all the schedule types
--usage P_GetAllImportAcronymTypes
---------------------------------------------------------

CREATE procedure [dbo].[P_GetAllImportAcronymTypes]
as
select	TableTypeID
		,Acronym

from PM_TableTypes

-- =============================================
-- Author:		harshkumar
-- Create date: <Create Date,,>
-- Description:	Gets Template Order Side Mapping String by TemplateID
-- =============================================
CREATE PROCEDURE P_GetSideMappingbyTemplateID 
(
@templateID varchar(200)
)
as
select SideMappingString
from T_BTOrderSideMapping
where TemplateID = @templateID


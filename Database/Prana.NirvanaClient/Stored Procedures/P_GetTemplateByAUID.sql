
CREATE procedure [dbo].[P_GetTemplateByAUID] as
select TemplateID, TemplateName
from T_BTTemplateList

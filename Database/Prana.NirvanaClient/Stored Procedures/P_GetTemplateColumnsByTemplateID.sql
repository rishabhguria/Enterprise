CREATE procedure [dbo].[P_GetTemplateColumnsByTemplateID]
(

@templateID varchar(200)

)
as
select Columns 
from T_BTTemplateList
where TemplateID=@templateID

CREATE procedure [dbo].[P_BTSaveTemplateList] (
@templateID varchar(200),
@templateName varchar(10),
@columns varchar(500),
@isDefaultTemplate varchar(5)
)
as
if((select count(*) from T_BTTemplateList where TemplateID=@templateID)=0)
begin
insert into T_BTTemplateList(TemplateID,TemplateName,Columns)  values(@templateID,@templateName,@columns)
end
else
begin
update T_BTTemplateList set TemplateName =@templateName,Columns = @columns where TemplateID=@templateID
end
if(@isDefaultTemplate = 'TRUE') 
begin
update T_BTTemplateList set IsDefaultTemplate='FALSE'
update T_BTTemplateList set IsDefaultTemplate='TRUE' where TemplateID=@templateID
end
else
begin
update T_BTTemplateList set IsDefaultTemplate='FALSE' where TemplateID=@templateID
end

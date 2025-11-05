

CREATE procedure [dbo].[BTCheckTemplateReference]
(
@templateID varchar(200)
)

as
declare @count int 

select  @count = count(*)   from T_BTSavedBaskets 
where  TemplateID=@templateID

select @count
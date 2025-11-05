
CREATE procedure [dbo].[P_BTGetDefaultTemplate] (

@userID int
)
as
select TemplateID from T_BTTemplateList where IsDefaultTemplate='TRUE'


--select * from T_BTTemplateList

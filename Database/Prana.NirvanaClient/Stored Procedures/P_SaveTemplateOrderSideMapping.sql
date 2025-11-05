


CREATE procedure [dbo].[P_SaveTemplateOrderSideMapping]
(
@templateID varchar(200),
@sidemappingString varchar(200)
)
as
if((select count(*) from T_BTOrderSideMapping where templateID = @templateID) =0)
insert into T_BTOrderSideMapping(TemplateID, SideMappingString)
values(@templateID,@sidemappingString)
else
update T_BTOrderSideMapping
set
SideMappingString = @sidemappingString
where
TemplateID = @templateID



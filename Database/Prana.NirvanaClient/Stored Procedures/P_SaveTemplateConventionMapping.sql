



CREATE procedure [dbo].[P_SaveTemplateConventionMapping]
(
@conventionMappingID varchar(200),
@percentage varchar(10),
@roundLot varchar(10)
)
as
if((select count(*) from T_BTConventionMapping where ConventionMappingID = @conventionMappingID) =0)
insert into T_BTConventionMapping 
values(@conventionMappingID,@percentage,@roundLot)
else
update T_BTConventionMapping
set
Percentage = @percentage,
RoundLot = @roundLot
where 
ConventionMappingID = @conventionMappingID




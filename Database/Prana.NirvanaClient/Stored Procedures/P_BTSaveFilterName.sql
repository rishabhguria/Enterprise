
CREATE proc [dbo].[P_BTSaveFilterName]
(
@filterID varchar(200),
@filterName varchar(50)
)
as
if((select count(*) from T_BTFilters where FilterID = @filterID) =0)
insert into T_BTFilters
values(@filterID,@filterName)
else 
update T_BTFilters
set
FilterName = @filterName
where
FilterID = @filterID



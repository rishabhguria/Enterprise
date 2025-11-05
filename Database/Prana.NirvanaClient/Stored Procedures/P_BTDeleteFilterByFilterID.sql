CREATE proc P_BTDeleteFilterByFilterID
(
@filterID varchar(200)
)
as
delete T_BTFilters
where FilterID = @filterID
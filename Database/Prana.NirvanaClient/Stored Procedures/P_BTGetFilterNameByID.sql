CREATE proc P_BTGetFilterNameByID
(
@filterID varchar(200)
)
AS
select FilterName 
from T_BTFilters
where FilterID = @filterID
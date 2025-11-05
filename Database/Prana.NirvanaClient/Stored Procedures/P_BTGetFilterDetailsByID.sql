
CREATE proc [dbo].[P_BTGetFilterDetailsByID]
(
@filterID varchar(200)
)
AS
select FilterTypesID,BenchMarkID,OperatorID,
Percentage
from T_BTFilterDetails
where FilterID = @filterID

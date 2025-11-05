CREATE proc P_BTGetFilterTypeNameByID
(
@filterTypeID int
)
AS
select FilterName from T_BTFilterTypes
where FilterTypeID = @filterTypeID
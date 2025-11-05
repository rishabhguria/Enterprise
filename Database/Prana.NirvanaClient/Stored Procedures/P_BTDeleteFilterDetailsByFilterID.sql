CREATE procedure P_BTDeleteFilterDetailsByFilterID
(
@filterID varchar(200)
)
as
delete from T_BTFilterDetails 
where FilterID=@filterID
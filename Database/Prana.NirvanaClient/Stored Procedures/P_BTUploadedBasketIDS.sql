CREATE procedure P_BTUploadedBasketIDS
(
@userID int,
@basketIDS varchar(200)

)
as
if(select count(*) from T_BTUpLoadedBasketIDS where UserID=@userID)=0

insert into T_BTUpLoadedBasketIDS(UploadedBasketIDs,UserID)
values(@basketIDS,@userID) 
else
update T_BTUpLoadedBasketIDS 
set UploadedBasketIDs=@basketIDS
where UserID=@userID


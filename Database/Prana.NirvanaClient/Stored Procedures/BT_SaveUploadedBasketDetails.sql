




CREATE procedure [dbo].[BT_SaveUploadedBasketDetails]

(
@basketID varchar(50),
@upLoadedBasketID varchar(50),
@haswaves varchar(5),
@userID int,
@isSaved varchar(5),
@uploadedBasketName varchar(20),
@date dateTime,
@AssetID int,
@UnderLyingID int,
@TemplateID varchar(200),
@benchMarkValue float
)
as

insert into T_BTUploadedBaskets (BasketID,UploadedBasketID,HasWaves,UserID,IsSaved,UploadedBasketName,TimeOfSave,AssetID,UnderLyingID,TemplateID,BenchMark) 
values(@basketID ,@upLoadedBasketID,@haswaves,@userID,@isSaved,@uploadedBasketName,@date,@AssetID,@UnderLyingID,@TemplateID,@benchMarkValue )





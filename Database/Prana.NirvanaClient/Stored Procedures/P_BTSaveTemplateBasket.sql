








CREATE procedure [dbo].[P_BTSaveTemplateBasket]
(
@savedBasketID varchar(200),
@savedBasketName varchar(50),
@templateID varchar(200),
@sharedTradingAccounts varchar(20),
@assetID int,
@underLyingID int,
@userID int ,
--@displayColumnList varchar(500),
@BenchMark float
)
as
if((select count(*) from T_BTSavedBaskets where SavedBasketID = @savedBasketID) =0)
insert T_BTSavedBaskets(SavedBasketID,SavedBasketName,
TemplateID,SharedTradingAccounts,AssetID,UnderLyingID,UserID,BenchMark) 
values(@savedBasketID,@savedBasketName,@templateID,@sharedTradingAccounts,@assetID,
@underLyingID,@userID,@BenchMark)
else 
update T_BTSavedBaskets
set
SavedBasketName = @savedBasketName,
TemplateID = @templateID,
SharedTradingAccounts = @sharedTradingAccounts,
assetID=@assetID,
underLyingID=@underLyingID,
BenchMark =@BenchMark 
where
SavedBasketID = @savedBasketID











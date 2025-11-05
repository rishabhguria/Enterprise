CREATE PROCEDURE [dbo].[P_GetLinkedTab_WatchList]
	@UserId INT
AS
    
Declare @Tabname VARCHAR(50)=''
select @Tabname = [TabName] from T_WatchList_TabNames where [TabId] in
( select TabId from T_WatchList_LinkedTab where UserID=@UserId )
select @Tabname


/****** Object:  Stored Procedure dbo.P_SaveUnderlying    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveUnderlying
(
		@UnderlyingID int,		
		@AssetID int,
		@Name varchar(50),
		@comment text
)
AS 
declare @total int
if(@UnderlyingID > 0)
begin	
	select @total = count(*)
	from T_UnderLying 
	Where UnderLyingName = @Name AND UnderlyingID <> @UnderlyingID
	
	if(@total > 0)
	begin
		Set @UnderlyingID = -1
	end
	else
	begin
		Update T_UnderLying 
		Set AssetID = @AssetID,
			UnderLyingName = @Name,
			Comment = @comment		
		Where UnderlyingID = @UnderlyingID
	end
end
else
begin
	set @total = 0
	select @total = count(*)
	from T_UnderLying
			Where UnderLyingName = @Name --And AssetID = @AssetID ** So that no any new underlying having the same name in the existing be added.
	
	if(@total > 0)
	begin
		Set @UnderlyingID = -1
	end
	else
	begin
		INSERT T_Underlying (UnderLyingName, AssetID, Comment)
		Values(@Name, @AssetID, @comment)  
			
		Set @UnderlyingID = scope_identity()
	end
end
select @UnderlyingID

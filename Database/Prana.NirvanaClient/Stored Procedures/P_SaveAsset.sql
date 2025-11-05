
/****** Object:  Stored Procedure dbo.P_SaveAsset    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_SaveAsset
(
		@AssetID int,		
		@assetName varchar(50),
		@comment text
		
	)
AS 
declare @total int
if(@AssetID > 0)
begin	
	select @total = count(*)
	from T_Asset 
	Where Assetname = @assetName AND AssetID <> @AssetID
	
	if(@total > 0)
	begin
		
		Set @AssetID = -1
	end
	else
	begin
		Update T_Asset 
		Set Assetname = @assetName,
			Comment = @comment		
		Where AssetID = @AssetID	
	end
end
else
begin
	set @total = 0
	select @total = count(*)
	from T_Asset 
	Where Assetname = @assetName
	
	if(@total > 0)
	begin
		
		Set @AssetID = -1
	end
	else
	begin
		INSERT T_Asset (Assetname, Comment)
		Values(@AssetName, @comment)  
			
		Set @AssetID = scope_identity()
	end
end
select @AssetID

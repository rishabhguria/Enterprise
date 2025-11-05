


/****** Object:  Stored Procedure dbo.P_AddUserPermission    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE dbo.P_AddUserPermission
	(
		@userID int,
		@permissionID int,
		@permission int
	)
AS
	declare @permisionID int
	declare @total int
	
	Set @permisionID = -1;
	
	Select @total = count(*)
	from T_UserPermission
	Where userID=@userID and permissionID = @permissionID
	
	if(@total = 0)
	begin 
		if(@permission = 1)
		begin
			Insert into T_UserPermission(userID, permissionID)
			Values(@userID, @permissionID)
			set @permisionID = scope_identity()
		end
		else
		begin
			Delete T_UserPermission
 			Where userID=@userID and permissionID = @permissionID
 			
 			set @permisionID = 0;
		end
	end
	else
	begin 
		set @permisionID = 0;
	end



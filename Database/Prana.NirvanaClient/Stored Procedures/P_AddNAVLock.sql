CREATE PROCEDURE dbo.P_AddNAVLock
(
	@lockDate DateTime,
	@creationDate DateTime,
	@createdBy int
)
AS
BEGIN
	Declare @total int

	Select @total = Count(1) 
	From T_NavLock
	Where LockDate = @lockDate and DeletionDate is null

	if ( @total = 0)
	begin 		
		INSERT INTO T_NavLock 
		(LockDate, CreationDate, CreatedBy)
		Values (@lockDate, @creationDate, @createdBy)
		        
	DECLARE @lockId INT = scope_identity();

	DECLARE @user VARCHAR(50) = (
			SELECT ShortName
			FROM T_CompanyUser
			WHERE UserID = @createdBy
			)

	INSERT INTO T_TradeAudit (
		ActionDate
		,[Action]
		,OriginalValue
		,NewValue
		,Comment
		,[Source]
		)
	VALUES (
		GetDate()
		,138
		,'None'
		,@user
		,'Global Lock for ' + convert(VARCHAR(max), @lockDate, 1) + ' added by ' + @user
		,11
		);

	SELECT @lockId

	end
	else
	begin
		Select -1
	end
END
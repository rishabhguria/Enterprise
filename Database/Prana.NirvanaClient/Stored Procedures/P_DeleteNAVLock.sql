
CREATE PROCEDURE dbo.P_DeleteNAVLock
(   
	@lockID INT,
	@deletionDate DateTime,
	@userId INT
)	
AS
BEGIN
	UPDATE T_NavLock
	SET DeletionDate = @deletionDate, DeletedBy = @userId
	WHERE LockId = @lockID

	DECLARE @oldUserId INT;
	DECLARE @lockDate DATETIME;

	SELECT @oldUserId = CreatedBy
		,@lockDate = LockDate
	FROM T_NavLock
	WHERE LockId = @lockID

	DECLARE @oldUser VARCHAR(50) = (
			SELECT ShortName
			FROM T_CompanyUser
			WHERE UserID = @oldUserId
			)
	DECLARE @newUser VARCHAR(50) = (
			SELECT ShortName
			FROM T_CompanyUser
			WHERE UserID = @userId
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
		,139
		,@oldUser
		,@newUser
		,'Global Lock for ' + convert(VARCHAR(max), @lockDate, 1) + ' deleted by ' + @newUser
		,11
		);
END
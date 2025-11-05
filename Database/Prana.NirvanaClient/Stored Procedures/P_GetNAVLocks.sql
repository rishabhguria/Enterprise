CREATE PROCEDURE dbo.P_GetNAVLocks
AS
	SELECT
	LockId,
	LockDate,
	CreationDate,
	CreatedBy
	FROM T_NavLock
	WHERE DeletionDate IS NULL
	Order by LockId desc

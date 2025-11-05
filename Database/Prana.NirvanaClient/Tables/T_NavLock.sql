CREATE TABLE [dbo].[T_NavLock]
(
	[LockId]			INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	[LockDate]		DATETIME      NOT NULL,
	[CreationDate]	DATETIME      NOT NULL,
	[CreatedBy]		INT			  NOT NULL,
	[DeletionDate]	DATETIME      NULL,
	[DeletedBy]		INT           NULL
)

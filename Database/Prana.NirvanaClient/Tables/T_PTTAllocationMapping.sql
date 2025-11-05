CREATE TABLE [dbo].[T_PTTAllocationMapping]
(
	[Id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY,
	[PTTId] INT NOT NULL,
	[AllocationPrefId] INT NOT NULL,
	INDEX INX_AllocationPrefId (AllocationPrefId) 
)
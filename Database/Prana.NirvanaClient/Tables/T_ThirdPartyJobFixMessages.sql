CREATE TABLE [dbo].[T_ThirdPartyJobFixMessages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[BlockId] INT NOT NULL,
	[AllocationID]  NVARCHAR(50) NULL,
	[TransmissionTime] DATETIME,
	[Direction] NVARCHAR(50) NULL,
	[Description] NVARCHAR(50) NULL,
	[FIXMsg] VARCHAR(MAX) NULL,
)

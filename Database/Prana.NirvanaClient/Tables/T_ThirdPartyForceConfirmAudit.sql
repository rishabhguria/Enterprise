CREATE TABLE [dbo].[T_ThirdPartyForceConfirmAudit]
(
	[CompanyUserId] INT NULL, 
    [ConfirmationDateTime] DATETIME NOT NULL DEFAULT (getdate()),
	[Broker] NVARCHAR(50) NULL,
	[Symbol] VARCHAR (100) NULL,
	[Side] NVARCHAR(50) NULL, 
    [Quantity] NVARCHAR(MAX) NULL,
	[AllocationId] NVARCHAR(50) NOT NULL,
	[ThirdPartyBatchId] INT NOT NULL,
	[Comment] NVARCHAR(MAX) NOT NULL
)

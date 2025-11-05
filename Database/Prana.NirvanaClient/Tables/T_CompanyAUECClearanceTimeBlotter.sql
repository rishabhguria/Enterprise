CREATE TABLE [dbo].[T_CompanyAUECClearanceTimeBlotter] (
	[CompanyAUECClearanceTimeID] INT IDENTITY(1, 1) NOT NULL
	,[CompanyAUECID] INT NOT NULL
	,[ClearanceTime] DATETIME NULL
	,CONSTRAINT [PK_T_CompanyAUECClearanceTime] PRIMARY KEY CLUSTERED ([CompanyAUECClearanceTimeID] ASC)
    ,[PermitRollover] BIT NOT NULL DEFAULT 1
	,[IsSendManualOrderViaFIX] BIT NOT NULL DEFAULT 1
	,[SendManualOrderTriggerTime] DATETIME NULL
	,[LastManualOrderRunTriggerTime] DATETIME NULL
	);
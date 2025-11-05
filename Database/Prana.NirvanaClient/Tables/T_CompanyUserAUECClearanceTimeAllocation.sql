CREATE TABLE [dbo].[T_CompanyUserAUECClearanceTimeAllocation] (
    [CompanyUserAUECClearanceTimeID] INT      IDENTITY (1, 1) NOT NULL,
    [CompanyUserAUECID]              INT      NOT NULL,
    [ClearanceTime]                  DATETIME NULL,
    CONSTRAINT [PK_T_CompanyUserAUECClearanceTimeAllocation] PRIMARY KEY CLUSTERED ([CompanyUserAUECClearanceTimeID] ASC),
    CONSTRAINT [FK_T_CompanyUserAUECClearanceTimeAllocation_T_CompanyUserAUEC] FOREIGN KEY ([CompanyUserAUECID]) REFERENCES [dbo].[T_CompanyUserAUEC] ([CompanyUserAUECID])
);


CREATE TABLE [dbo].[PM_PositionCashFlow] (
    [cashFlowID]      INT              IDENTITY (1, 1) NOT NULL,
    [PositionID]      UNIQUEIDENTIFIER NOT NULL,
    [cashFlowPerUnit] FLOAT (53)       NOT NULL,
    [cashFlowDate]    DATETIME         NOT NULL,
    CONSTRAINT [PK_PM_PositionCashFlow] PRIMARY KEY CLUSTERED ([cashFlowID] ASC),
    CONSTRAINT [FK__PM_Positi__Posit__5111A972] FOREIGN KEY ([PositionID]) REFERENCES [dbo].[PM_NetPositions] ([PositionId])
);


CREATE TABLE [dbo].[PM_SimulatedPositions] (
    [SimulatedPositionID] INT          IDENTITY (1, 1) NOT NULL,
    [Symbol]              NVARCHAR (5) NOT NULL,
    [SideID]              INT          NOT NULL,
    [Quantity]            INT          NOT NULL,
    [AveragePrice]        FLOAT (53)   NOT NULL,
    [AuecID]              INT          NOT NULL,
    [IsActive]            BIT          NULL,
    CONSTRAINT [PK_PM_SimulatedPositions] PRIMARY KEY CLUSTERED ([SimulatedPositionID] ASC)
);


CREATE TABLE [dbo].[PM_MarkPositions] (
    [MarkPositionID] INT           IDENTITY (1, 1) NOT NULL,
    [Symbol]         VARCHAR (100) NOT NULL,
    [MarkPrice]      FLOAT (53)    NOT NULL,
    [MarkDateTime]   DATETIME      NULL,
    [IsActive]       BIT           NULL,
    CONSTRAINT [PK_PM_MarkPositions] PRIMARY KEY CLUSTERED ([MarkPositionID] ASC)
);


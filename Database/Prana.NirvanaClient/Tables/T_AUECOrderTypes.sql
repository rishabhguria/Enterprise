CREATE TABLE [dbo].[T_AUECOrderTypes] (
    [AUECOrderTypeID] INT IDENTITY (1, 1) NOT NULL,
    [AUECID]          INT NOT NULL,
    [OrderTypeID]     INT NOT NULL,
    CONSTRAINT [PK_T_AUECOrderTypes] PRIMARY KEY CLUSTERED ([AUECOrderTypeID] ASC)
);


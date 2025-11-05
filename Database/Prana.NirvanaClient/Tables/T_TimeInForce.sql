CREATE TABLE [dbo].[T_TimeInForce] (
    [TimeInForceID]       INT          IDENTITY (1, 1) NOT NULL,
    [TimeInForce]         VARCHAR (50) NOT NULL,
    [TimeInForceTagValue] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_TimeInForce] PRIMARY KEY CLUSTERED ([TimeInForceID] ASC)
);


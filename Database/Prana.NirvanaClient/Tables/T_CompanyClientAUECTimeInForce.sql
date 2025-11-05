CREATE TABLE [dbo].[T_CompanyClientAUECTimeInForce] (
    [CompanyClientAUECTimeInForceID] INT NOT NULL,
    [CompanyClientAUECID]            INT NOT NULL,
    [TimeInForceID]                  INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientAUECTimeInForce] PRIMARY KEY CLUSTERED ([CompanyClientAUECTimeInForceID] ASC),
    CONSTRAINT [FK_T_CompanyClientAUECTimeInForce_T_TimeInForce] FOREIGN KEY ([TimeInForceID]) REFERENCES [dbo].[T_TimeInForce] ([TimeInForceID])
);


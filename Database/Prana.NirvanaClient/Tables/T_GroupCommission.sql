CREATE TABLE [dbo].[T_GroupCommission] (
    [GroupCommissionId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [GroupId_FK]        VARCHAR (50) NOT NULL,
    [Commission]        FLOAT (53)   DEFAULT ((0.0)) NOT NULL,
    [Fees]              FLOAT (53)   DEFAULT ((0.0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([GroupCommissionId] ASC) WITH (FILLFACTOR = 100)
);


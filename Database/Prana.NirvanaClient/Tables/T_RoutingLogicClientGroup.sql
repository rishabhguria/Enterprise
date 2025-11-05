CREATE TABLE [dbo].[T_RoutingLogicClientGroup] (
    [ClientGroupID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [ApplyRL]       INT          NOT NULL,
    CONSTRAINT [PK_T_ClientGroup] PRIMARY KEY CLUSTERED ([ClientGroupID] ASC)
);


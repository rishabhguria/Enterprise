CREATE TABLE [dbo].[T_RoutingLogicParameter] (
    [ParameterID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_RoutingLogicParamater] PRIMARY KEY CLUSTERED ([ParameterID] ASC)
);


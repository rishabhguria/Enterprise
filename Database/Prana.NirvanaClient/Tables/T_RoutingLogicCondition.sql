CREATE TABLE [dbo].[T_RoutingLogicCondition] (
    [RLConditionID]  INT           IDENTITY (1, 1) NOT NULL,
    [RLID_FK]        INT           NOT NULL,
    [ParameterID_FK] INT           NOT NULL,
    [ParameterValue] VARCHAR (500) NOT NULL,
    [Sequence]       SMALLINT      NOT NULL,
    [JoinCondition]  INT           NOT NULL,
    [OperatorID_FK]  INT           NOT NULL,
    CONSTRAINT [PK_T_RoutingLogicCondition] PRIMARY KEY CLUSTERED ([RLConditionID] ASC)
);


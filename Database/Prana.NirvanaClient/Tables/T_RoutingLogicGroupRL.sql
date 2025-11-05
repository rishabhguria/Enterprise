CREATE TABLE [dbo].[T_RoutingLogicGroupRL] (
    [ClientGroupID_FK] INT      NOT NULL,
    [RLID_FK]          INT      NOT NULL,
    [ApplyRL]          BIT      NOT NULL,
    [Rank]             SMALLINT NOT NULL
);


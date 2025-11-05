CREATE TABLE [dbo].[T_RoutingInstruction] (
    [RoutingInstructionID]   INT          IDENTITY (1, 1) NOT NULL,
    [RoutingInstructionName] VARCHAR (50) NOT NULL,
    [RoutingInstruction]     VARCHAR (1)  NOT NULL,
    CONSTRAINT [PK_T_RoutingInstruction] PRIMARY KEY CLUSTERED ([RoutingInstructionID] ASC)
);


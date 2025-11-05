CREATE TABLE [dbo].[T_Workflows] (
    [WorkflowID] INT            IDENTITY (1, 1) NOT NULL,
    [WorkFlow]   NVARCHAR (100) NOT NULL,
    [Sequence]   INT            NULL
);


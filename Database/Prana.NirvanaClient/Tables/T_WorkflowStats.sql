CREATE TABLE [dbo].[T_WorkflowStats] (
    [WorkflowStatID] INT            IDENTITY (1, 1) NOT NULL,
    [WorkFlowState]  NVARCHAR (100) NOT NULL,
    [Sequence]       INT            NULL
);


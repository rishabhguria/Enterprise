CREATE TABLE [dbo].[T_ExecutionInstructions] (
    [ExecutionInstructionsID]       INT          IDENTITY (1, 1) NOT NULL,
    [ExecutionInstructions]         VARCHAR (50) NOT NULL,
    [ExecutionInstructionsTagValue] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_ExecutionInstructions] PRIMARY KEY CLUSTERED ([ExecutionInstructionsID] ASC)
);


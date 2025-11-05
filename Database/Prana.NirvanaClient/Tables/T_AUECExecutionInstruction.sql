CREATE TABLE [dbo].[T_AUECExecutionInstruction] (
    [AUECExecutionInstructionID] INT IDENTITY (1, 1) NOT NULL,
    [AUECID]                     INT NOT NULL,
    [ExecutionInstructionID]     INT NOT NULL,
    CONSTRAINT [PK_T_AUECExecutionInstruction] PRIMARY KEY CLUSTERED ([AUECExecutionInstructionID] ASC)
);


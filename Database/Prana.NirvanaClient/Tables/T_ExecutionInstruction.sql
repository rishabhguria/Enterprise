CREATE TABLE [dbo].[T_ExecutionInstruction] (
    [ExecutionInstructionID] INT          NOT NULL,
    [Instruction]            VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_ExecutionInstruction] PRIMARY KEY CLUSTERED ([ExecutionInstructionID] ASC)
);


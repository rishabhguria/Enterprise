CREATE TABLE [dbo].[T_CVAUECExecutionInstructions] (
    [CVAUECHandlingInstructionID] BIGINT IDENTITY (1, 1) NOT NULL,
    [CVAUECID]                    BIGINT NOT NULL,
    [ExecutionInstructionsID]     INT    NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueAUECExecutionInstruction] PRIMARY KEY CLUSTERED ([CVAUECHandlingInstructionID] ASC),
    CONSTRAINT [FK_T_CVAUECExecutionInstructions_T_ExecutionInstructions] FOREIGN KEY ([ExecutionInstructionsID]) REFERENCES [dbo].[T_ExecutionInstructions] ([ExecutionInstructionsID])
);


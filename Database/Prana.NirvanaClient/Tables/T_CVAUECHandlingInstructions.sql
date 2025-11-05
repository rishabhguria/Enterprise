CREATE TABLE [dbo].[T_CVAUECHandlingInstructions] (
    [CVAUECHandlingInstructionID] BIGINT IDENTITY (1, 1) NOT NULL,
    [CVAUECID]                    BIGINT NOT NULL,
    [HandlingInstructionsID]      INT    NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueAUECHandlingInstructions] PRIMARY KEY CLUSTERED ([CVAUECHandlingInstructionID] ASC)
);


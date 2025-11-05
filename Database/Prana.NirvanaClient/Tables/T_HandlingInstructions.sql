CREATE TABLE [dbo].[T_HandlingInstructions] (
    [HandlingInstructionsID]       INT          IDENTITY (1, 1) NOT NULL,
    [HandlingInstructions]         VARCHAR (50) NOT NULL,
    [HandlingInstructionsTagValue] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_HandlingInstructions] PRIMARY KEY CLUSTERED ([HandlingInstructionsID] ASC)
);


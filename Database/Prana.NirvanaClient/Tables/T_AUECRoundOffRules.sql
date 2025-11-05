CREATE TABLE [dbo].[T_AUECRoundOffRules] (
    [AUECID]    INT NOT NULL,
    [RoundOff]  INT NULL,
    [IsApplied] BIT NULL,
    CONSTRAINT [PK_T_AUECRoundOffRules] PRIMARY KEY CLUSTERED ([AUECID] ASC)
);


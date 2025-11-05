CREATE TABLE [dbo].[T_PMClearanceTimes] (
    [AUECID]        INT      NOT NULL,
    [ClearanceTime] DATETIME NULL,
    CONSTRAINT [PK_T_PMClearanceTimes] PRIMARY KEY CLUSTERED ([AUECID] ASC)
);


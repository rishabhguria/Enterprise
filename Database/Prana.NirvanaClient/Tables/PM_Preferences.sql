CREATE TABLE [dbo].[PM_Preferences] (
    [UserID]              INT          NULL,
    [useClosingMark]      VARCHAR (10) NULL,
    [XPercentofAvgVolume] FLOAT (53)   NULL,
	[IsShowPMToolbar]	BIT DEFAULT 0,
);


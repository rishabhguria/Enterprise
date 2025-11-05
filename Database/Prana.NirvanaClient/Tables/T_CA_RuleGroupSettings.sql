CREATE TABLE [dbo].[T_CA_RuleGroupSettings] (
    [GroupId]               VARCHAR (50)   NULL,
    [GroupName]             VARCHAR (1000) NULL,
    [PopUpEnabled]          BIT            NULL,
    [EmailEnabled]          BIT            NULL,
    [EmailToList]           VARCHAR (1000) NULL,
    [EmailCCList]           VARCHAR (1000) NULL,
    [LimitFrequencyMinutes] INT            NULL,
    [AlertInTimeRange]      BIT            NULL,
    [StopAlertOnHolidays]   BIT            NULL,
    [StartTime]             DATETIME       NULL,
    [EndTime]               DATETIME       NULL,
	[SendInOneEmail]			BIT            NOT NULL DEFAULT 0,
	[Slot1]					DATETIME		NOT NULL DEFAULT '1800-01-01 00:00:00.000',
    [Slot2]					DATETIME		NOT NULL DEFAULT '1800-01-01 00:00:00.000',
	[Slot3]					DATETIME		NOT NULL DEFAULT '1800-01-01 00:00:00.000',
    [Slot4]					DATETIME		NOT NULL DEFAULT '1800-01-01 00:00:00.000',
	[Slot5]					DATETIME		NOT NULL DEFAULT '1800-01-01 00:00:00.000',
    [EmailSubject]			VARCHAR(1000) NOT NULL DEFAULT ''
);


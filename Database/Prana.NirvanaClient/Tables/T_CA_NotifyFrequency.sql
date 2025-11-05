CREATE TABLE [dbo].[T_CA_NotifyFrequency] (
    [ID]                     INT           IDENTITY (1, 1) NOT NULL,
    [Minutes]                INT           NULL,
    [MeasurementDescription] VARCHAR (100) NULL,
    [SnoozeDescription]      VARCHAR (100) NULL,
    [CronExp]                VARCHAR (200) NULL,
    CONSTRAINT [PK_T_CA_NotifyFrequency] PRIMARY KEY CLUSTERED ([ID] ASC)
);


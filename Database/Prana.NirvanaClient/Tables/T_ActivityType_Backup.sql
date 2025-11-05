CREATE TABLE [dbo].[T_ActivityType_Backup] (
    [ActivityTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Acronym]        VARCHAR (50)   NULL,
    [ActivityType]   VARCHAR (100)  NULL,
    [Description]    VARCHAR (3000) NULL,
    [BalanceType]    INT            NULL,
    [ActivitySource] TINYINT        NOT NULL
);


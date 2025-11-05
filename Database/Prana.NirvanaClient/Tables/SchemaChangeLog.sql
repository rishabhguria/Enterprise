CREATE TABLE [dbo].[SchemaChangeLog] (
    [LogId]        INT           IDENTITY (1, 1) NOT NULL,
    [EventXML]     XML           NULL,
    [DatabaseName] VARCHAR (256) NULL,
    [EventType]    VARCHAR (50)  NULL,
    [ObjectName]   VARCHAR (256) NULL,
    [ObjectType]   VARCHAR (25)  NULL,
    [SqlCommand]   VARCHAR (MAX) NULL,
    [EventDate]    DATETIME      CONSTRAINT [DF_EventsLog_EventDate] DEFAULT (getdate()) NULL,
    [LoginName]    VARCHAR (256) NULL,
    [Workstation]  VARCHAR (256) NULL
);


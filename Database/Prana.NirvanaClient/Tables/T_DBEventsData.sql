CREATE TABLE [dbo].[T_DBEventsData] (
    [EventDate]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [EventType]        NVARCHAR (64)  NULL,
    [EventDDL]         NVARCHAR (MAX) NULL,
    [EventXML]         XML            NULL,
    [DatabaseName]     NVARCHAR (255) NULL,
    [SchemaName]       NVARCHAR (255) NULL,
    [ObjectName]       NVARCHAR (255) NULL,
    [ObjectType]       NVARCHAR (255) NULL,
    [ObjectCreateDate] DATETIME       NULL,
    [ObjectModifyDate] DATETIME       NULL,
    [HostName]         VARCHAR (64)   NULL,
    [IPAddress]        VARCHAR (32)   NULL,
    [ProgramName]      NVARCHAR (255) NULL,
    [LoginName]        NVARCHAR (255) NULL
);


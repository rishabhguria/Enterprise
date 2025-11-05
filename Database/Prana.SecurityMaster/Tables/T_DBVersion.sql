CREATE TABLE [dbo].[T_DBVersion] (
    [Date]       DATETIME     DEFAULT (GETDATE()) NOT NULL,
    [Version]    VARCHAR (20) NULL,
    [Revision]   VARCHAR (150)  NOT NULL,
    [Product]    VARCHAR (50) NULL
);
CREATE TABLE [dbo].[T_FileData] (
    [FileId]       INT           IDENTITY (1, 1) NOT NULL,
    [FileNames]    VARCHAR (500) NOT NULL,
    [FileData]     IMAGE         NOT NULL,
    [LastSaveTime] DATETIME      NOT NULL,
    [FileType]     INT           NOT NULL,
    CONSTRAINT [PK_T_FileData] PRIMARY KEY CLUSTERED ([FileId] ASC)
);


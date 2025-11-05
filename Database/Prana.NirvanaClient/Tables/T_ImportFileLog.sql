CREATE TABLE [dbo].[T_ImportFileLog] (
    [ImportFileID]                  INT            IDENTITY (1, 1) NOT NULL,
    [ImportFileName]                NVARCHAR (500) NOT NULL,
    [ImportFilePath]                NVARCHAR (MAX) NOT NULL,
    [ImportType]                    NVARCHAR (MAX) NOT NULL,
    [UTCTime]                       DATETIME       CONSTRAINT [DF_T_ImportFileLog_UTCTime] DEFAULT (getutcdate()) NOT NULL,
    [ImportFileLastModifiedUTCTime] DATETIME       NULL,
    CONSTRAINT [PK_T_ImportFileLog] PRIMARY KEY CLUSTERED ([ImportFileID] ASC)
);


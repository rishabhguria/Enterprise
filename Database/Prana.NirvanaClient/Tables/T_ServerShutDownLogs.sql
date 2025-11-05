CREATE TABLE [dbo].[T_ServerShutDownLogs] (
    [PK]             INT IDENTITY (1, 1) NOT NULL,
    [NormalShutDown] INT NOT NULL,
    CONSTRAINT [PK_T_ServerShutDownLogs] PRIMARY KEY CLUSTERED ([PK] ASC)
);


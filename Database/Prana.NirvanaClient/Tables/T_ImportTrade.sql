CREATE TABLE [dbo].[T_ImportTrade] (
    [ImportSourceID]   INT          IDENTITY (1, 1) NOT NULL,
    [ImportSourceName] VARCHAR (50) NOT NULL,
    [XSLTFileID]       INT          NOT NULL,
    CONSTRAINT [PK_T_ImportTrade] PRIMARY KEY CLUSTERED ([ImportSourceID] ASC)
);


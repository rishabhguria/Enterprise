CREATE TABLE [dbo].[__SQLVC_Scripts] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [WhenApplied] DATETIME         CONSTRAINT [DF___SQLVC_Scripts_WhenApplied] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK___SQLVC_Scripts] PRIMARY KEY CLUSTERED ([Id] ASC)
);


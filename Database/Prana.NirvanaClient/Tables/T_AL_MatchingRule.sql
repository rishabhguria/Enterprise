CREATE TABLE [dbo].[T_AL_MatchingRule] (
    [Id]           INT            NOT NULL,
    [MatchingRule] NVARCHAR (20)  NOT NULL,
    [Description]  NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_T_AL_MatchingRule_MatchingRule] UNIQUE NONCLUSTERED ([MatchingRule] ASC)
);


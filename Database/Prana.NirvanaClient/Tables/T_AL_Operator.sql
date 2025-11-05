CREATE TABLE [dbo].[T_AL_Operator] (
    [Id]          INT            NOT NULL,
    [Operator]    NVARCHAR (20)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_T_AL_Operator_Operator] UNIQUE NONCLUSTERED ([Operator] ASC)
);


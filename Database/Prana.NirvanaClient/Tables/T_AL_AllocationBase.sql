CREATE TABLE [dbo].[T_AL_AllocationBase] (
    [Id]             INT            NOT NULL,
    [AllocationBase] NVARCHAR (50)  NOT NULL,
    [Description]    NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_T_AL_AllocationBase_AllocationBase] UNIQUE NONCLUSTERED ([AllocationBase] ASC)
);


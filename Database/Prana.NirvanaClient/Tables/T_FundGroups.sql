CREATE TABLE [dbo].[T_FundGroups] (
    [FundGroupID] INT          IDENTITY (1, 1) NOT NULL,
    [GroupName]   VARCHAR (50) NULL,
    CONSTRAINT [IX_T_FundGroups] UNIQUE NONCLUSTERED ([GroupName] ASC)
);


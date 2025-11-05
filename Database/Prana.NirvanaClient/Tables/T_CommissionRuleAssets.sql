CREATE TABLE [dbo].[T_CommissionRuleAssets] (
    [RuleAssetId] BIGINT           IDENTITY (1, 1) NOT NULL,
    [RuleId_FK]   UNIQUEIDENTIFIER NOT NULL,
    [AssetId_FK]  INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([RuleAssetId] ASC) WITH (FILLFACTOR = 100),
    FOREIGN KEY ([RuleId_FK]) REFERENCES [dbo].[T_CommissionRules] ([RuleId]),
    CONSTRAINT [FK__T_Commiss__Asset__4FB370E5] FOREIGN KEY ([AssetId_FK]) REFERENCES [dbo].[T_Asset] ([AssetID])
);


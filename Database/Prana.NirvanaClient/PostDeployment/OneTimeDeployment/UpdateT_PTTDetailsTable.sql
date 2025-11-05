-- Now alter columns of T_PTTDetails table from DECIMAL(32,19) → DECIMAL(38,19)
ALTER TABLE [dbo].[T_PTTDetails]
ALTER COLUMN [StartingPosition] DECIMAL(38,19) NOT NULL;

ALTER TABLE [dbo].[T_PTTDetails]
ALTER COLUMN [StartingValue] DECIMAL(38,19) NOT NULL;

ALTER TABLE [dbo].[T_PTTDetails]
ALTER COLUMN [StartingPercentage] DECIMAL(38,19) NOT NULL;

ALTER TABLE [dbo].[T_PTTDetails]
ALTER COLUMN [PercentageType] DECIMAL(38,19) NOT NULL;

ALTER TABLE [dbo].[T_PTTDetails]
ALTER COLUMN [TradeQuantity] DECIMAL(38,19) NOT NULL;

ALTER TABLE [dbo].[T_PTTDetails]
ALTER COLUMN [EndingPercentage] DECIMAL(38,19) NOT NULL;

ALTER TABLE [dbo].[T_PTTDetails]
ALTER COLUMN [PercentageAllocation] DECIMAL(38,19) NOT NULL;
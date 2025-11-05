SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_W_Funds] ADD
CONSTRAINT [FK_TouchFunds_CompanyFunds] FOREIGN KEY ([PranaFundID]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
GO
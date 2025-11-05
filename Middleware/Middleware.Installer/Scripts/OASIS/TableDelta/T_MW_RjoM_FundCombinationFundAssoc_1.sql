/****** Object:  Table [dbo].[T_MW_RjoM_FundCombinationFundAssoc]    Script Date: 07/18/2014 14:52:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_MW_RjoM_FundCombinationFundAssoc]  WITH CHECK ADD  CONSTRAINT [FK_RjoM_FundCombinationFundAssoc_RjoM_FundCombination] FOREIGN KEY([FundCombinationId])
REFERENCES [dbo].[T_MW_RjoM_FundCombination] ([Id])
GO
ALTER TABLE [dbo].[T_MW_RjoM_FundCombinationFundAssoc] CHECK CONSTRAINT [FK_RjoM_FundCombinationFundAssoc_RjoM_FundCombination]
GO
ALTER TABLE [dbo].[T_MW_RjoM_FundCombinationFundAssoc]  WITH CHECK ADD  CONSTRAINT [FK_RjoM_FundCombinationFundAssoc_T_CompanyMasterFunds] FOREIGN KEY([FundId])
REFERENCES [dbo].[T_CompanyMasterFunds] ([CompanyMasterFundID])
GO
ALTER TABLE [dbo].[T_MW_RjoM_FundCombinationFundAssoc] CHECK CONSTRAINT [FK_RjoM_FundCombinationFundAssoc_T_CompanyMasterFunds]
GO
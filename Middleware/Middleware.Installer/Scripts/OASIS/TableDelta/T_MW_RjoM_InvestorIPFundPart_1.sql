/****** Object:  Table [dbo].[T_MW_RjoM_InvestorIPFundPart]    Script Date: 07/18/2014 14:52:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_MW_RjoM_InvestorIPFundPart]  WITH CHECK ADD  CONSTRAINT [FK_RjoM_InvestorIPFundPart_RjoM_InvestorIP] FOREIGN KEY([InvestorIPId])
REFERENCES [dbo].[T_MW_RjoM_InvestorIP] ([Id])
GO
ALTER TABLE [dbo].[T_MW_RjoM_InvestorIPFundPart] CHECK CONSTRAINT [FK_RjoM_InvestorIPFundPart_RjoM_InvestorIP]
GO
ALTER TABLE [dbo].[T_MW_RjoM_InvestorIPFundPart]  WITH CHECK ADD  CONSTRAINT [FK_RjoM_InvestorIPFundPart_T_CompanyMasterFunds] FOREIGN KEY([FundId])
REFERENCES [dbo].[T_CompanyMasterFunds] ([CompanyMasterFundID])
GO
ALTER TABLE [dbo].[T_MW_RjoM_InvestorIPFundPart] CHECK CONSTRAINT [FK_RjoM_InvestorIPFundPart_T_CompanyMasterFunds]
GO
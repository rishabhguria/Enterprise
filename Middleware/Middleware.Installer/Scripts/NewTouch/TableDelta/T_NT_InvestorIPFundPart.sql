GO
ALTER TABLE [dbo].[T_NT_InvestorIPFundPart]  WITH CHECK ADD  CONSTRAINT [FK_T_NT_InvestorIPFundPart_T_CompanyMasterFunds] FOREIGN KEY([FundId])
REFERENCES [dbo].[T_CompanyMasterFunds] ([CompanyMasterFundID])
GO
ALTER TABLE [dbo].[T_NT_InvestorIPFundPart] CHECK CONSTRAINT [FK_T_NT_InvestorIPFundPart_T_CompanyMasterFunds]
GO
ALTER TABLE [dbo].[T_NT_InvestorIPFundPart]  WITH CHECK ADD  CONSTRAINT [FK_T_NT_InvestorIPFundPart_T_NT_InvestorIP] FOREIGN KEY([InvestorIPId])
REFERENCES [dbo].[T_NT_InvestorIP] ([Id])
GO
ALTER TABLE [dbo].[T_NT_InvestorIPFundPart] CHECK CONSTRAINT [FK_T_NT_InvestorIPFundPart_T_NT_InvestorIP]
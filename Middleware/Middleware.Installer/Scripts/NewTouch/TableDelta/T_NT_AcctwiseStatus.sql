GO
ALTER TABLE [dbo].[T_NT_AcctwiseStatus]  WITH CHECK ADD  CONSTRAINT [FK_T_NT_AcctwiseStatus_T_CompanyFunds] FOREIGN KEY([AcctId])
REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
GO
GO
ALTER TABLE [dbo].[T_NT_AcctwiseStatus] CHECK CONSTRAINT [FK_T_NT_AcctwiseStatus_T_CompanyFunds]
GO
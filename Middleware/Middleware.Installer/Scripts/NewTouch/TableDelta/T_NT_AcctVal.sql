ALTER TABLE [dbo].[T_NT_AcctVal]  WITH CHECK ADD  CONSTRAINT [FK_T_NT_AcctVal_T_CompanyFunds] FOREIGN KEY([AcctId])
REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
GO
GO
ALTER TABLE [dbo].[T_NT_AcctVal] CHECK CONSTRAINT [FK_T_NT_AcctVal_T_CompanyFunds]
GO
GO
ALTER TABLE [dbo].[T_NT_InvestorIP]  WITH CHECK ADD  CONSTRAINT [FK_T_NT_InvestorIP_T_NT_GroupType] FOREIGN KEY([GroupTypeId])
REFERENCES [dbo].[T_NT_GroupType] ([Id])
GO
ALTER TABLE [dbo].[T_NT_InvestorIP] CHECK CONSTRAINT [FK_T_NT_InvestorIP_T_NT_GroupType]
GO
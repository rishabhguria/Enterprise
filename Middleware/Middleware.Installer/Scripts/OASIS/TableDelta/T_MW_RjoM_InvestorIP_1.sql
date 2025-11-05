/****** Object:  Table [dbo].[T_MW_RjoM_InvestorIP]    Script Date: 07/18/2014 14:52:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_MW_RjoM_InvestorIP]  WITH CHECK ADD  CONSTRAINT [FK_RjoM_InvestorIP_RjoM_GroupType] FOREIGN KEY([GroupTypeId])
REFERENCES [dbo].[T_MW_RjoM_GroupType] ([Id])
GO
ALTER TABLE [dbo].[T_MW_RjoM_InvestorIP] CHECK CONSTRAINT [FK_RjoM_InvestorIP_RjoM_GroupType]
GO
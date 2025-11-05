/****** Object:  Table [dbo].[T_MW_RjoM_FundCombination]    Script Date: 07/18/2014 14:52:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_MW_RjoM_FundCombination]  WITH CHECK ADD  CONSTRAINT [FK_RjoM_FundCombination_RjoM_GroupType] FOREIGN KEY([GroupTypeId])
REFERENCES [dbo].[T_MW_RjoM_GroupType] ([Id])
GO
ALTER TABLE [dbo].[T_MW_RjoM_FundCombination] CHECK CONSTRAINT [FK_RjoM_FundCombination_RjoM_GroupType]
GO
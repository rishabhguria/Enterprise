/****** Object:  Table [dbo].[T_MW_RjoM_AcctVal]    Script Date: 07/18/2014 14:52:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_MW_RjoM_AcctVal]  WITH CHECK ADD  CONSTRAINT [FK_RjoM_AcctVal_T_CompanyFunds] FOREIGN KEY([AcctId])
REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID])
GO
ALTER TABLE [dbo].[T_MW_RjoM_AcctVal] CHECK CONSTRAINT [FK_RjoM_AcctVal_T_CompanyFunds]
GO
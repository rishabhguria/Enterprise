SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_W_RegistrationClientMapping]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationClientMapping_Clients] FOREIGN KEY([ClientID])
REFERENCES [dbo].[T_W_Clients] ([ClientID])
GO
ALTER TABLE [dbo].[T_W_RegistrationClientMapping] CHECK CONSTRAINT [FK_RegistrationClientMapping_Clients]
GO
ALTER TABLE [dbo].[T_W_RegistrationClientMapping]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationClientMapping_Registration] FOREIGN KEY([RegistrationID])
REFERENCES [dbo].[T_W_Registration] ([RegistrationID])
GO
ALTER TABLE [dbo].[T_W_RegistrationClientMapping] CHECK CONSTRAINT [FK_RegistrationClientMapping_Registration]
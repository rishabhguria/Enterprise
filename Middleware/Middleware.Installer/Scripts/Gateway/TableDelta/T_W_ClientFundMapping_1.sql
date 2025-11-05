SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[T_W_ClientFundMapping] ADD
CONSTRAINT [FK_T_W_ClientFundMapping_T_W_Clients] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[T_W_Clients] ([ClientID]),
CONSTRAINT [FK_T_W_ClientFundMapping_T_W_Funds] FOREIGN KEY ([TouchFundID]) REFERENCES [dbo].[T_W_Funds] ([TouchFundID])
GO
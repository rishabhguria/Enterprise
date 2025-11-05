CREATE TABLE [dbo].[T_RoutingLogic] (
    [RLID]                              INT          IDENTITY (1, 1) NOT NULL,
    [Name]                              VARCHAR (50) NOT NULL,
    [AUECID_FK]                         INT          NOT NULL,
    [DefaultCompanyTradingAccountID_FK] INT          NOT NULL,
    CONSTRAINT [PK_T_RoutingLogic] PRIMARY KEY CLUSTERED ([RLID] ASC)
);


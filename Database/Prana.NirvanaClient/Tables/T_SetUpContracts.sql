CREATE TABLE [dbo].[T_SetUpContracts] (
    [CompanySetUpContractID] INT           IDENTITY (1, 1) NOT NULL,
    [Symbol]                 VARCHAR (50)  NOT NULL,
    [AuecID]                 INT           NOT NULL,
    [ContractSize]           INT           NOT NULL,
    [ContractMonthID]        INT           NOT NULL,
    [CompanyID]              INT           NOT NULL,
    [Description]            VARCHAR (100) NULL,
    CONSTRAINT [PK_T_SetUpContracts] PRIMARY KEY CLUSTERED ([CompanySetUpContractID] ASC),
    CONSTRAINT [FK_T_SetUpContracts_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);


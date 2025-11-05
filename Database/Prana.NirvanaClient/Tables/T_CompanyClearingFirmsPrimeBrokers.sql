CREATE TABLE [dbo].[T_CompanyClearingFirmsPrimeBrokers] (
    [CompanyClearingFirmsPrimeBrokersID] INT          IDENTITY (1, 1) NOT NULL,
    [ClearingFirmsPrimeBrokersName]      VARCHAR (50) NOT NULL,
    [ClearingFirmsPrimeBrokersShortName] VARCHAR (50) NOT NULL,
    [CompanyID]                          INT          NOT NULL,
    CONSTRAINT [PK_T_CompanyClearingFirmsPrimeBrokers] PRIMARY KEY CLUSTERED ([CompanyClearingFirmsPrimeBrokersID] ASC),
    CONSTRAINT [FK_T_CompanyClearingFirmsPrimeBrokers_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);


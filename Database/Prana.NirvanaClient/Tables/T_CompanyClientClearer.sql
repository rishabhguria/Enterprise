CREATE TABLE [dbo].[T_CompanyClientClearer] (
    [CompanyClientClearerID]        INT          IDENTITY (1, 1) NOT NULL,
    [CompanyClientID]               INT          NOT NULL,
    [CompanyClientClearerName]      VARCHAR (50) NOT NULL,
    [CompanyClientClearerShortName] VARCHAR (50) NOT NULL,
    [ClearingFirmBrokerID]          VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_CompanyClientClearer] PRIMARY KEY CLUSTERED ([CompanyClientClearerID] ASC)
);


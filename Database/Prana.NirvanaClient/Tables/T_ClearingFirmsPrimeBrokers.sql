CREATE TABLE [dbo].[T_ClearingFirmsPrimeBrokers] (
    [ClearingFirmsPrimeBrokersID]        INT          IDENTITY (1, 1) NOT NULL,
    [ClearingFirmsPrimeBrokersName]      VARCHAR (50) NOT NULL,
    [ClearingFirmsPrimeBrokersShortName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_ClearingFirmsPrimeBrokers] PRIMARY KEY CLUSTERED ([ClearingFirmsPrimeBrokersID] ASC)
);


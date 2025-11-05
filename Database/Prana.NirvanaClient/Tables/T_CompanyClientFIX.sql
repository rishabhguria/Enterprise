CREATE TABLE [dbo].[T_CompanyClientFIX] (
    [CompanyClientFIXID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyClientID]    INT          NOT NULL,
    [SenderCompID]       VARCHAR (50) NOT NULL,
    [OnBehalfOfCompID]   VARCHAR (50) NOT NULL,
    [TargetCompID]       VARCHAR (50) NOT NULL,
    [IP]                 VARCHAR (50) NOT NULL,
    [Port]               INT          NOT NULL,
    [ApplyRL]            BIT          CONSTRAINT [DF_T_CompanyClientFIX_ApplyRL] DEFAULT (0) NOT NULL,
    [ApplyValidation]    BIT          CONSTRAINT [DF_T_CompanyClientFIX_ApplyValidation] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_T_CompanyClientFIX] PRIMARY KEY CLUSTERED ([CompanyClientFIXID] ASC)
);


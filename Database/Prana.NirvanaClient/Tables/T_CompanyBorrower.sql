CREATE TABLE [dbo].[T_CompanyBorrower] (
    [CompanyBorrowerID] INT          IDENTITY (1, 1) NOT NULL,
    [BorrowerName]      VARCHAR (50) NOT NULL,
    [BorrowerShortName] VARCHAR (50) NOT NULL,
    [BorrowerFirmID]    VARCHAR (50) NOT NULL,
    [CompanyID]         INT          NOT NULL,
    CONSTRAINT [PK_T_CompanyBorrower] PRIMARY KEY CLUSTERED ([CompanyBorrowerID] ASC)
);


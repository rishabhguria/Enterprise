CREATE TABLE [dbo].[T_CompanyType] (
    [CompanyTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyType]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_CompanyType] PRIMARY KEY CLUSTERED ([CompanyTypeID] ASC)
);


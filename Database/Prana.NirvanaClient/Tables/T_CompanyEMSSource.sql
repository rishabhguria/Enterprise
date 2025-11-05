CREATE TABLE [dbo].[T_CompanyEMSSource] (
    [CompanyEMSSourceID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyID]          INT NOT NULL,
    [EMSSourceID]        INT NOT NULL,
    CONSTRAINT [PK_T_CompanyEMSSource] PRIMARY KEY CLUSTERED ([CompanyEMSSourceID] ASC)
);


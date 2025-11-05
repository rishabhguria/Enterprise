CREATE TABLE [dbo].[T_CompanyMasterFunds] (
    [CompanyMasterFundID] INT           IDENTITY (1, 1) NOT NULL,
    [MasterFundName]      NVARCHAR (200) NOT NULL,
    [MasterFundLogo]      IMAGE         NULL,
    [CompanyID]           INT           CONSTRAINT [DF_T_CompanyMasterFunds_CompanyID] DEFAULT ((0)) NULL,
    [IsActive]            BIT           DEFAULT ((1)) NOT NULL,
    [GroupTypeId]         INT           DEFAULT ((1)) NOT NULL, 
    CONSTRAINT [PK_T_CompanyMasterFunds] PRIMARY KEY CLUSTERED ([CompanyMasterFundID] ASC),
    CONSTRAINT [C_U_MasterFundName] UNIQUE NONCLUSTERED ([MasterFundName] ASC)
);


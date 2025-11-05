CREATE TABLE [dbo].[T_CompanyMasterStrategy] (
    [CompanyMasterStrategyID] INT           IDENTITY (1, 1) NOT NULL,
    [MasterStrategyName]      NVARCHAR (50) NOT NULL,
    [CompanyID]               NCHAR (10)    CONSTRAINT [DF_T_CompanyMasterStrategy_CompanyID] DEFAULT ((0)) NULL,
    [IsActive]                BIT           DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_CompanyMasterStrategy] PRIMARY KEY CLUSTERED ([CompanyMasterStrategyID] ASC),
    CONSTRAINT [C_U_Name] UNIQUE NONCLUSTERED ([MasterStrategyName] ASC)
);


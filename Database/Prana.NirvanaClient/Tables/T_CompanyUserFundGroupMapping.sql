CREATE TABLE [dbo].[T_CompanyUserFundGroupMapping] (
    [UserFundGroupMappingID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyUserID]          INT NOT NULL,
    [FundGroupID]            INT NOT NULL
);


CREATE TABLE [dbo].[T_PTTAccountPercentagePreference]
(
	[AccountId] INT  NOT NULL , 
    [PercentInMasterFund] DECIMAL(18, 6) NULL DEFAULT 0,
	[AccountFactor] FLOAT NOT NULL DEFAULT 1, 
	[PreferenceType] INT NOT NULL Default 0,
    CONSTRAINT FK_T_PTTAccountPercentagePreference_T_CompanyFunds FOREIGN KEY (AccountId) REFERENCES T_CompanyFunds (CompanyFundID)
)

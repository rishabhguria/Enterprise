CREATE TABLE [dbo].[T_RestrictedAllowedSecuritiesList]
(
	[CompanyID] INT NOT NULL  ,
	[Symbol] VARCHAR(MAX) NULL, 
    [RestrictedOrAllowed] INT NULL , 
    [IsTickerSymbology] BIT NOT NULL DEFAULT ('1'), 
    FOREIGN KEY (CompanyID) REFERENCES T_Company(CompanyID)
)

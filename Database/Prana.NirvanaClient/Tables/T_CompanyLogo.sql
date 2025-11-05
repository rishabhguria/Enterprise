CREATE TABLE [dbo].[T_CompanyLogo] (
    [LogoID]   INT           IDENTITY (1, 1) NOT NULL,
    [LogoName] NVARCHAR (50) NOT NULL,
    [Logo]     IMAGE         NOT NULL
);


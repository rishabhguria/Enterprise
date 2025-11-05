CREATE TABLE [dbo].[T_CompanyUserAUEC] (
    [CompanyUserAUECID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyUserID]     INT NOT NULL,
    [CompanyAUECID]     INT NOT NULL,
    CONSTRAINT [PK_T_CompanyUserAUEC] PRIMARY KEY CLUSTERED ([CompanyUserAUECID] ASC),
    CONSTRAINT [FK_T_CompanyUserAUEC_T_CompanyAUEC] FOREIGN KEY ([CompanyAUECID]) REFERENCES [dbo].[T_CompanyAUEC] ([CompanyAUECID])
);


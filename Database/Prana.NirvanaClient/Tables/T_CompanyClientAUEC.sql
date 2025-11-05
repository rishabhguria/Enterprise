CREATE TABLE [dbo].[T_CompanyClientAUEC] (
    [CompanyClientAUECID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyClientID]     INT NOT NULL,
    [CompanyAUECID]       INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientAUEC] PRIMARY KEY CLUSTERED ([CompanyClientAUECID] ASC),
    CONSTRAINT [FK_T_CompanyClientAUEC_T_CompanyAUEC1] FOREIGN KEY ([CompanyAUECID]) REFERENCES [dbo].[T_CompanyAUEC] ([CompanyAUECID])
);


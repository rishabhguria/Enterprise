CREATE TABLE [dbo].[T_CompanyAUEC] (
    [CompanyAUECID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyID]     INT NOT NULL,
    [AUECID]        INT NOT NULL,
    CONSTRAINT [PK_T_CompanyAUEC] PRIMARY KEY CLUSTERED ([CompanyAUECID] ASC),
    CONSTRAINT [FK_T_CompanyAUEC_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);


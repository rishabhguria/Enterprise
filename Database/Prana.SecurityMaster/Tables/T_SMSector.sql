CREATE TABLE [dbo].[T_SMSector] (
    [SectorID]   INT          IDENTITY (1, 1) NOT NULL,
    [SectorName] VARCHAR (20) NULL,
    CONSTRAINT [PK_T_SMSector] PRIMARY KEY CLUSTERED ([SectorID] ASC)
);


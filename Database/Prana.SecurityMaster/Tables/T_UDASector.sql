CREATE TABLE [dbo].[T_UDASector] (
    [SectorName] VARCHAR (100) NULL,
    [SectorID]   INT           NOT NULL,
    CONSTRAINT [T_UDASector_Unique_SectorName] UNIQUE NONCLUSTERED ([SectorName] ASC)
);


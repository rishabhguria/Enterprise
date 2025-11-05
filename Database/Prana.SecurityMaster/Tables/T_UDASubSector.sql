CREATE TABLE [dbo].[T_UDASubSector] (
    [SubSectorName] VARCHAR (100) NULL,
    [SubSectorID]   INT           NOT NULL,
    CONSTRAINT [T_UDASubSector_Unique_SubSectorName] UNIQUE NONCLUSTERED ([SubSectorName] ASC)
);


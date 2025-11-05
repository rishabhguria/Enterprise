CREATE TABLE [dbo].[T_SecondarySortTypes] (
    [SecondarySortID]   INT          IDENTITY (1, 1) NOT NULL,
    [SecondarySortName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_SecondarySortTypes] PRIMARY KEY CLUSTERED ([SecondarySortID] ASC)
);


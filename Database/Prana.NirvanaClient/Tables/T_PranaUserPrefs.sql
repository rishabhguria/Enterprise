CREATE TABLE [dbo].[T_PranaUserPrefs] (
    [UserID]       INT          NOT NULL,
    [FileName]     VARCHAR (40) NOT NULL,
    [FileData]     IMAGE        NULL,
    [LastSaveTime] DATETIME     NOT NULL
);


CREATE TABLE [dbo].[T_Layout] (
    [LayoutID]   INT          IDENTITY (1, 1) NOT NULL,
    [LayoutName] VARCHAR (50) NOT NULL,
    [UserID]     INT          NOT NULL,
    [TimeStamp]  ROWVERSION   NULL,
    [LastUsed]   DATETIME     NULL,
    CONSTRAINT [PK_T_Layout] PRIMARY KEY CLUSTERED ([LayoutID] ASC)
);


CREATE TABLE [dbo].[T_LayoutComponentDetails] (
    [LayoutModuleID] INT          IDENTITY (1, 1) NOT NULL,
    [LayoutID]       INT          NOT NULL,
    [ModuleID]       INT          NOT NULL,
    [LeftX]          INT          NULL,
    [RightY]         INT          NULL,
    [Height]         INT          NULL,
    [Width]          INT          NULL,
    [WindowState]    VARCHAR (50) NULL,
    [IsInUse]        BIT          NULL,
    [UserID]         INT          NULL,
    [QTTIndex]       INT          NULL,
    CONSTRAINT [PK_T_LayoutComponentDetails] PRIMARY KEY CLUSTERED ([LayoutModuleID] ASC)
);


CREATE TABLE [dbo].[T_UnallocatedGroup] (
    [GroupID]        VARCHAR (50) NOT NULL,
    [AllocationType] VARCHAR (10) NULL,
    [DateGrouped]    DATETIME     NULL,
    [UserID]         INT          NULL,
    CONSTRAINT [PK_T_UnallocatedGroup] PRIMARY KEY CLUSTERED ([GroupID] ASC)
);


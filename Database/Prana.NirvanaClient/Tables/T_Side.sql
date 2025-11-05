CREATE TABLE [dbo].[T_Side] (
    [SideID]       INT          IDENTITY (1, 1) NOT NULL,
    [Side]         VARCHAR (50) NOT NULL,
    [SideTagValue] VARCHAR (50) NOT NULL,
    [IsBasicSide]  BIT          CONSTRAINT [DF__T_Side__IsBasicS__122052C0] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_T_Side] PRIMARY KEY CLUSTERED ([SideID] ASC)
);


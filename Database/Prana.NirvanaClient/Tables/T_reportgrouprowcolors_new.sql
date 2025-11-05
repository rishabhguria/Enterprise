CREATE TABLE [dbo].[T_ReportGroupRowColors_New] (
    [PKID]             INT           NOT NULL,
    [Group1BGColor]    NVARCHAR (50) NULL,
    [Group2BGColor]    NVARCHAR (50) NULL,
    [Group3BGColor]    NVARCHAR (50) NULL,
    [Group4BGColor]    NVARCHAR (50) NULL,
    [DetailRowBGColor] NVARCHAR (50) NULL,
	[Group1FontColor] [nvarchar](50) NULL,
	[Group2FontColor] [nvarchar](50) NULL,
	[Group3FontColor] [nvarchar](50) NULL,
	[Group4FontColor] [nvarchar](50) NULL,
	[DetailRowFontColor] [nvarchar](50) NULL,
	[GrandTotalRowFontColor] [nvarchar](50) NULL,
    CONSTRAINT [PK_T_ReportGroupRowColors_New] PRIMARY KEY CLUSTERED ([PKID] ASC)
);


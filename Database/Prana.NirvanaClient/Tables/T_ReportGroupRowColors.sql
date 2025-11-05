CREATE TABLE [dbo].[T_ReportGroupRowColors] (
    [PKID]             INT           NOT NULL,
    [Group1BGColor]    NVARCHAR (50) NULL,
    [Group2BGColor]    NVARCHAR (50) NULL,
    [Group3BGColor]    NVARCHAR (50) NULL,
    [Group4BGColor]    NVARCHAR (50) NULL,
    [DetailRowBGColor] NVARCHAR (50) NULL,
    CONSTRAINT [PK_T_ReportGroupRowColors] PRIMARY KEY CLUSTERED ([PKID] ASC)
);


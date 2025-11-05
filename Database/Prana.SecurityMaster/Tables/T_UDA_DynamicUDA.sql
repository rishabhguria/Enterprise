CREATE TABLE [dbo].[T_UDA_DynamicUDA] (
    [IndexID]       INT           IDENTITY (1, 1) NOT NULL,
    [Tag]           VARCHAR (100) NOT NULL,
    [HeaderCaption] VARCHAR (100) NULL,
    [DefaultValue]  VARCHAR (100) NULL,
    [MasterValues]  XML           NULL,
    PRIMARY KEY CLUSTERED ([Tag] ASC)
);


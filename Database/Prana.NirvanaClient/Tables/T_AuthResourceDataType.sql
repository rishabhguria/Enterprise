CREATE TABLE [dbo].[T_AuthResourceDataType] (
    [ResourceDataTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [ResourceDataTypeName] NVARCHAR (50)  NOT NULL,
    [Description]          NVARCHAR (200) NULL,
    CONSTRAINT [PK_T_AuthResourceDataType] PRIMARY KEY CLUSTERED ([ResourceDataTypeId] ASC)
);


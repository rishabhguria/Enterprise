CREATE TABLE [dbo].[T_UDASecurityType] (
    [SecurityTypeName] NVARCHAR (200) NULL,
    [SecurityTypeID]   INT            NOT NULL,
    CONSTRAINT [T_UDASecurityType_Unique_SecurityTypeName] UNIQUE NONCLUSTERED ([SecurityTypeName] ASC)
);


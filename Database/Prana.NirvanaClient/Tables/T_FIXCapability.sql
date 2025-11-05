CREATE TABLE [dbo].[T_FIXCapability] (
    [FixCapabilityID] INT          IDENTITY (1, 1) NOT NULL,
    [Description]     VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_FIXCapability] PRIMARY KEY CLUSTERED ([FixCapabilityID] ASC)
);


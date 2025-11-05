CREATE TABLE [dbo].[T_RMAlertType] (
    [AlertTypeID]   INT          NOT NULL,
    [AlertTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_RMAlertType] PRIMARY KEY CLUSTERED ([AlertTypeID] ASC)
);


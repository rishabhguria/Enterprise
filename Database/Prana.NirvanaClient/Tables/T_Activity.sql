CREATE TABLE [dbo].[T_Activity] (
    [ActivityID]     INT            IDENTITY (1, 1) NOT NULL,
    [Activity]       VARCHAR (50)   NULL,
    [ActivityTypeID] INT            NULL,
    [Description]    VARCHAR (3000) NULL,
    CONSTRAINT [PK_T_Activity] PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);


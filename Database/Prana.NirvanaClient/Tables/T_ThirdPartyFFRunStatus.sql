CREATE TABLE [dbo].[T_ThirdPartyFFRunStatus] (
    [StatusID] INT           NOT NULL,
    [Status]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_ThirdPartyFFRunStatus] PRIMARY KEY CLUSTERED ([StatusID] ASC)
);


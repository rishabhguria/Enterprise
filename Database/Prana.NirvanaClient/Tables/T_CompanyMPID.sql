CREATE TABLE [dbo].[T_CompanyMPID] (
    [CompanyMPID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyID]   INT          NOT NULL,
    [MPID]        VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_T_CompanyMPID] PRIMARY KEY CLUSTERED ([CompanyMPID] ASC)
);


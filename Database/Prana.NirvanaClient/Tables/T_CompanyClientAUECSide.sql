CREATE TABLE [dbo].[T_CompanyClientAUECSide] (
    [CompanyClientAUECSideID] INT NOT NULL,
    [CompanyClientAUECID]     INT NOT NULL,
    [SideID]                  INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientAUECSide] PRIMARY KEY CLUSTERED ([CompanyClientAUECSideID] ASC),
    CONSTRAINT [FK__T_Company__SideI__14FCBF6B] FOREIGN KEY ([SideID]) REFERENCES [dbo].[T_Side] ([SideID])
);


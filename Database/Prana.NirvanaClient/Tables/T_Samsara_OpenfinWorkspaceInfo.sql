CREATE TABLE [dbo].[T_Samsara_OpenfinWorkspaceInfo](	
	[UserID] [INT] NOT NULL,
	[WorkspaceId] [VARCHAR](100) NOT NULL,
    [WorkspaceName] [VARCHAR](100) NOT NULL,	
    [WorkspaceLayout] [VARBINARY](max) NOT NULL,
	[LastSavedTime] [datetime]
)

/****************************************************************************                                                                                              
Name :   [P_SaveOpenfinWorkspaceInfo]                                                                                              
Purpose:  To Save the Openfin Workspace Layout corresponding to a particular user id passed.  
Author : Karan Joshi    
Module: SamsaraLogout                             
****************************************************************************/   

CREATE PROCEDURE [dbo].[P_Samsara_SaveOpenfinWorkspaceInfo]
    @UserID INT,
    @WorkspaceLayout VARBINARY(MAX),
    @WorkspaceName VARCHAR(100),
    @WorkspaceId VARCHAR(100)
AS
BEGIN
  IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'T_Samsara_OpenfinWorkspaceInfo')
    BEGIN
        -- Table doesn't exist, create table and perform insert
        CREATE TABLE dbo.[T_Samsara_OpenfinWorkspaceInfo] (
            UserID INT PRIMARY KEY,
            WorkspaceLayout VARBINARY(MAX) ,
            WorkspaceName VARCHAR(100),
            WorkspaceId VARCHAR(100),
            LastSavedTime datetime
        )
        INSERT INTO dbo.[T_Samsara_OpenfinWorkspaceInfo] (UserID, WorkspaceLayout, WorkspaceName, WorkspaceId, LastSavedTime)
        VALUES (@UserID, @WorkspaceLayout, @WorkspaceName, @WorkspaceId, GETDATE())
    END
    ELSE
    BEGIN

    IF EXISTS (SELECT 1 FROM [T_Samsara_OpenfinWorkspaceInfo] WHERE UserID = @UserID and WorkspaceId = @WorkspaceId)
    BEGIN
        -- WorkspaceName exists, perform update
        UPDATE dbo.[T_Samsara_OpenfinWorkspaceInfo]
        SET WorkspaceLayout = @WorkspaceLayout , LastSavedTime = GETDATE()
        WHERE WorkspaceName = @WorkspaceName
        AND UserID = @UserID
    END
    ELSE
    BEGIN
        -- User ID doesn't exist, perform insert
        INSERT INTO dbo.[T_Samsara_OpenfinWorkspaceInfo] (UserID, WorkspaceLayout, WorkspaceName, WorkspaceId, LastSavedTime)
        VALUES (@UserID, @WorkspaceLayout, @WorkspaceName, @WorkspaceId,GETDATE())
    END
END
END
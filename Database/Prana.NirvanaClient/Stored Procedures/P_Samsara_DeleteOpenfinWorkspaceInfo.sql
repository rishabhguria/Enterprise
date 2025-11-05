/****************************************************************************                                                                                              
Name :   [P_Samsara_DeleteOpenfinWorkspaceInfo]                                                                                             
Purpose: To Delete the Openfin Workspace Layout corresponding to a particular userId and workspaceId passed.  
Author : Avanish Srivastava
Module: SamsaraWorkspaceManagement                            
****************************************************************************/ 

CREATE PROCEDURE [dbo].[P_Samsara_DeleteOpenfinWorkspaceInfo]
    @userId INT,
    @workspaceId VARCHAR(100)
AS
BEGIN
    -- Delete the entry from T_Samsara_OpenfinWorkspaceInfo table
    DELETE FROM T_Samsara_OpenfinWorkspaceInfo
    WHERE userId = @userId
    AND workspaceId = @workspaceId;
END;

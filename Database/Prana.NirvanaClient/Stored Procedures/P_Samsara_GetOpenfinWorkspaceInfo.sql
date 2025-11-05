/****************************************************************************                                                                                              
Name :   [P_GetOpenfinWorkspaceInfo]                                                                                              
Purpose:  To fetch all the Openfin Workspace Layout along with user ids.  
Author : Karan Joshi    
Module: SamsaraLogout                             
****************************************************************************/  

CREATE PROCEDURE [dbo].[P_Samsara_GetOpenfinWorkspaceInfo]
(
	@userID INT = NULL
)
AS
BEGIN
    IF @userID IS NOT NULL
    BEGIN
        -- If userID is provided, fetch data for that userID
        SELECT [UserID], [WorkspaceLayout], [WorkspaceName], [WorkspaceId]
        FROM [T_Samsara_OpenfinWorkspaceInfo]
        WHERE [UserID] = @userID
    END
    ELSE
    BEGIN
        -- If userID is not provided, fetch all data
        SELECT [UserID], [WorkspaceLayout],  [WorkspaceName], [WorkspaceId]
        FROM [T_Samsara_OpenfinWorkspaceInfo]
    END
END
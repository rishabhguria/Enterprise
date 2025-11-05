/****************************************************************************
Name    : [P_Samsara_LogDeletedPageForUser]
Purpose : Logs information about a deleted page for a user into the 
          T_Samsara_DeletedPagesInfo table.
Module  : SamsaraWorkspaceManagement
****************************************************************************/
CREATE PROCEDURE [dbo].[P_Samsara_LogDeletedPageForUser]
    @userId     INT,
    @pageId     VARCHAR(100),
    @pageName   VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON; -- Prevents returning the count of rows affected

    BEGIN TRY
        -- Insert the deleted page information
        INSERT INTO [dbo].[T_Samsara_DeletedPagesInfo] (
            UserID,
            PageId,
            PageName,
            DeleteTime
        )
        VALUES (
            @userId,
            @pageId,
            @pageName,
            GETDATE()
        );

    END TRY
    BEGIN CATCH
        -- Optionally rollback any open transactions if needed
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW
    END CATCH
END;

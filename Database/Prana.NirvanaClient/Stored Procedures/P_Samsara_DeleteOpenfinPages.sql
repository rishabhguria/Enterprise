/****************************************************************************                                                                                              
Name :   [P_Samsara_DeleteOpenfinPages]                                                                                             
Purpose: To Delete the Openfin Pages corresponding to a particular userId and pageId passed.  
Author : Kishor BG
Module: SamsaraWorkspaceManagement                            
****************************************************************************/ 
CREATE PROCEDURE [dbo].[P_Samsara_DeleteOpenfinPages]
    @userId INT,
    @pageId VARCHAR(100),
    @commaSeparatedViewIds VARCHAR(MAX)
AS
BEGIN

    CREATE TABLE #TempViewIds (ViewId NVARCHAR(MAX));
    
    DECLARE @Position INT;
    DECLARE @NextPosition INT;
    DECLARE @Value NVARCHAR(MAX);
    
    SET @Position = 1;
    SET @commaSeparatedViewIds = @commaSeparatedViewIds + ','; -- Append a comma to the end
    
    WHILE CHARINDEX(',', @commaSeparatedViewIds, @Position) > 0
    BEGIN
        SET @NextPosition = CHARINDEX(',', @commaSeparatedViewIds, @Position);
        SET @Value = SUBSTRING(@commaSeparatedViewIds, @Position, @NextPosition - @Position);
        
        INSERT INTO #TempViewIds (ViewId)
        VALUES (@Value);
        
        SET @Position = @NextPosition + 1;
    END


    -- Delete the entry from T_Samsara_OpenfinPageInfo table
    DELETE FROM T_Samsara_OpenfinPageInfo
    WHERE userId = @userId
    AND pageId = @pageId;

    -- Delete the entry from T_Samsara_CompanyUserLayouts table    
    DELETE FROM T_Samsara_CompanyUserLayouts
    WHERE userId = @userId
    AND ViewId IN (SELECT ViewId FROM #TempViewIds);

    -- Delete the entry from T_RTPNL_UserWidgetConfigDetails table
    DELETE FROM T_RTPNL_UserWidgetConfigDetails
    WHERE userId = @userId
    AND pageId = @pageId;

    DROP TABLE #TempViewIds

END;

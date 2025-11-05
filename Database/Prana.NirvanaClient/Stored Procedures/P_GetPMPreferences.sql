CREATE PROCEDURE [dbo].[P_GetPMPreferences]      
(   
@userID int    
)  
as  
SELECT 
 top 1 useClosingMark, XpercentofAvgVolume, IsShowPMToolbar    from PM_Preferences 
    
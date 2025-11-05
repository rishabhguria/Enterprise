
--=============================================        
-- Author:  Rajat      
-- Create date: <Create Date,,>        
-- Description: Provides the exchangeid and displayname      
--=============================================        
Create PROCEDURE [dbo].[P_GetKeyValuePairAUECMultipliers]        
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
        
    -- Insert statements for procedure here        
 SELECT AUECID, Multiplier from T_AUEC        
END

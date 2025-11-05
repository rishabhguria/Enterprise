-- =============================================    
-- Author:  <Harsh Kumar>    
-- Create date: <Create Date,,>    
-- Description: <Gets AUECIDs for which data exists in trade/taxlots tables>    
-- =============================================    
CREATE PROCEDURE GetInUseAUECs    
    
AS    
BEGIN    
  
 select  distinct AUECID from T_group WITH (NOLOCK)   
    
END 
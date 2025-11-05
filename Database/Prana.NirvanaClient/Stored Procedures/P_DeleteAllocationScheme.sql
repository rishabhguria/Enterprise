/***************************************      
      
CREATED BY: RAHUL GUPTA            
CREATED ON: 2012-05-30      
      
***************************************/      
      
      
      
CREATE PROCEDURE P_DeleteAllocationScheme(      
@schemeID int,      
@schemeName varchar(100)      
)      
AS      
      
      
BEGIN TRY         
       
DECLARE @count int      
SET @count  = (SELECT count(*) FROM T_Group where AllocationSchemeID = @schemeID)      
      
IF(@count <= 0)      
DELETE T_AllocationScheme WHERE AllocationSchemeID = @schemeID       
and AllocationSchemeName = @schemeName      
END TRY          
                                                                                                    
BEGIN CATCH                                                                                                   
END CATCH 
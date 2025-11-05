            
                                                
CREATE PROCEDURE [dbo].[P_SaveAllocationScheme]                                                                              
(                  
  @AllocationSchemeName Varchar(100),              
  @Date DateTime,                                        
  @xml varchar(max),          
  @AllocationSchemeID int,
  @IsPrefVisible BIT,          
  @CreationSource int                           
)                                                
As                                       
                                                                
BEGIN TRY                                                         
                                        
Declare @count Int              
Set @count = (Select Count(*) From T_AllocationScheme          
 Where AllocationSchemeName = @AllocationSchemeName)  
  
Declare @ASID Int              
Set @ASID = (Select AllocationSchemeID From T_AllocationScheme          
 Where AllocationSchemeName = @AllocationSchemeName)                    
              
if (@count <= 0 )              
 Begin              
  Insert into T_AllocationScheme(Date,AllocationSchemeName,AllocationScheme,IsPrefVisible,CreationSource)                                 
  Values(@Date,@AllocationSchemeName,@xml,@IsPrefVisible,@CreationSource)      
Select SCOPE_IDENTITY()                      
 End              
Else              
 Begin               
  Update T_AllocationScheme                              
 Set AllocationScheme = @xml,          
 AllocationSchemeName = @AllocationSchemeName,          
 Date = @Date,
 IsPrefVisible = @IsPrefVisible          
 Where AllocationSchemeName = @AllocationSchemeName              
 Select @ASID        
 End                        
                                              
                                                                                            
END TRY                                                                                                        
BEGIN CATCH                                                                 
-- Drop Table #TempAllocationScheme                                         
END CATCH

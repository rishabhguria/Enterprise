    
CREATE Procedure [dbo].[P_OTC_SaveCustomFields]                                                      
(                                                      
 @xml nText,                                                      
 @ErrorMessage varchar(500) output,                                                                                               
 @ErrorNumber int output                                                                  
)                                                         
As         
                                                                         
--SET @ErrorNumber = 0                                                                          
--SET @ErrorMessage = 'Success'                                                       
                                                      
BEGIN TRY                                   
                                                      
 BEGIN TRAN TRAN1                                                                            
                                                                              
  DECLARE @handle int                                                                            
                                                                                                                           
  exec sp_xml_preparedocument @handle OUTPUT, @Xml                                
  
  CREATE TABLE #TempCustomFields(
	
	
	[InstrumentType] [int],
	[Name] [varchar](100),
	[DefaultValue] [sql_variant],
	[DataType] [varchar](50),
    [UIOrder] INT,
	[ID] int
 )
                                                                                                                   
 Insert Into #TempCustomFields                                                                                                                                                                           
 (                                                                                                                                                                          
	[InstrumentType],
	[Name],
	[DefaultValue],
	[DataType],
    [UIOrder],
	[ID]                         
  )                                                                               
  Select                                  
   [InstrumentType],
	[Name],
	[DefaultValue],
	[DataType],
    [UIOrder],
	[ID]                  
                                                              
  FROM OPENXML(@handle, '//OTCCustomFields', 2)                                                                                                                           
  WITH                                                                                                
  (                                                        
    [InstrumentType] [int],
	[Name] [varchar](100),
	[DefaultValue] [sql_variant],
	[DataType] [varchar](50),
    [UIOrder] INT,
	[ID] int
	                                                    
  )                                                          
     
IF exists(select * from   #TempCustomFields
	where #TempCustomFields.ID not in (select ID from T_OTC_CustomFields) )
Begin	                    
 Insert Into T_OTC_CustomFields                                                                                                                                                                           
 (                                                                                                                                                                          
	[InstrumentType],
	[Name],
	[DefaultValue],
	[DataType],
    [UIOrder] 
	                    
  )                                                                               
  Select                                  
    [InstrumentType],
	[Name],
	[DefaultValue],
	[DataType],
    [UIOrder] 
	from   #TempCustomFields

End
Else
begin

UPDATE [dbo].[T_OTC_CustomFields]
   SET [InstrumentType] = #TempCustomFields.InstrumentType
      ,[Name] = #TempCustomFields.Name
      ,[DefaultValue] = #TempCustomFields.DefaultValue
      ,[DataType] = #TempCustomFields.DataType
      ,[UIOrder] =#TempCustomFields.UIOrder
  from 
  [T_OTC_CustomFields]
  inner join #TempCustomFields on #TempCustomFields.Id = [T_OTC_CustomFields].ID
end


	
                 
Drop Table #TempCustomFields                      
                                                  
EXEC sp_xml_removedocument @handle                                                                                
                                                                                 
COMMIT TRANSACTION TRAN1                                                                                
                                              
END TRY                                                                                
BEGIN CATCH                             
 SET @ErrorMessage = ERROR_MESSAGE();                                                             
 SET @ErrorNumber = Error_number();                                                                                 
 ROLLBACK TRANSACTION TRAN1                                                                                   
END CATCH;
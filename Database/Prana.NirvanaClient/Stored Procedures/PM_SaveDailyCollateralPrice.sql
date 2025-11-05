CREATE proc PM_SaveDailyCollateralPrice      
(                          
   @Xml nText                          
 , @ErrorMessage varchar(500) output                          
 , @ErrorNumber int output                          
)                          
AS                           
                          
SET @ErrorMessage = 'Success'                          
SET @ErrorNumber = 0                          
BEGIN TRAN TRAN1                           
                          
BEGIN TRY                
               
 DECLARE @handle int                             
 exec sp_xml_preparedocument @handle OUTPUT,@Xml                             
                           
 CREATE TABLE #TempCollateralPrice                                                                               
 (                                                                                 
  Symbol varchar(100)                             
  ,Date datetime
  ,FundId int    
  ,CollateralPrice FLOAT
  ,Haircut FLOAT
  ,RebateOnMV FLOAT
  ,RebateOnCollateral FLOAT
 )                                                                                
                                                                                
 INSERT INTO #TempCollateralPrice                          
 (                                                                     
    Symbol                        
   ,Date                        
   ,FundId  
  ,CollateralPrice
  ,Haircut
  ,RebateOnMV
  ,RebateOnCollateral
  )                                                                                
 SELECT                                                                                
   Symbol                        
   ,Date                        
   ,FundId  
  ,CollateralPrice
  ,Haircut        
  ,RebateOnMV
  ,RebateOnCollateral        
 FROM OPENXML(@handle, '//DailyCollateralPrice', 2)                                                                                   
  WITH                                                                                 
  (                                                           
   Symbol varchar(100)                             
  ,Date datetime
  ,FundId int    
  ,CollateralPrice FLOAT
  ,Haircut FLOAT        
  ,RebateOnMV FLOAT
  ,RebateOnCollateral FLOAT       
  )  As CPT where CPT.CollateralPrice is not null                           
                
DELETE PM_DailyCollateralPrice from PM_DailyCollateralPrice                
inner join  #TempCollateralPrice  on DateDiff(d,Convert(VARCHAR(10), #TempCollateralPrice.Date, 110),PM_DailyCollateralPrice.Date)=0               
and #TempCollateralPrice.Symbol = PM_DailyCollateralPrice.Symbol and #TempCollateralPrice.FundId = PM_DailyCollateralPrice.FundId
                          
 INSERT INTO                          
    PM_DailyCollateralPrice                          
 (                          
    Symbol                        
   ,Date                        
   ,FundID 
  ,CollateralPrice
  ,Haircut                           
  ,RebateOnMV
  ,RebateOnCollateral                           
 )                     
  SELECT                           
    Symbol                        
   ,Date                        
   ,FundId  
  ,CollateralPrice
  ,Haircut         
  ,RebateOnMV
  ,RebateOnCollateral      
   FROM                           
    #TempCollateralPrice                  
                          
 DROP TABLE #TempCollateralPrice               
                      
 EXEC sp_xml_removedocument @handle                          
                           
COMMIT TRANSACTION TRAN1                          
                           
END TRY                          
 BEGIN CATCH                           
  SET @ErrorMessage = ERROR_MESSAGE();                          
  print @errormessage                          
  SET @ErrorNumber = Error_number();                           
  ROLLBACK TRANSACTION TRAN1                           
END CATCH;     
    
    
   

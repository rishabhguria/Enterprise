CREATE PROCEDURE [dbo].[SaveDataCollateralInterest]
 (        
  @Xml nText        
  ,@ErrorMessage varchar(500) output        
  ,@ErrorNumber int output    
 )       
AS
SET @ErrorMessage = 'Success'        
SET @ErrorNumber = 0        
BEGIN TRAN TRAN1         
        
BEGIN TRY        
        
DECLARE @handle int           
exec sp_xml_preparedocument @handle OUTPUT,@Xml

CREATE TABLE #TEMPCOLLATERALINTEREST
(
Date date,
FundID int,
BenchmarkName Varchar(50),
BenchmarkRate Varchar(50),
Spread int
)

INSERT INTO #TEMPCOLLATERALINTEREST
(
Date ,
FundID ,
BenchmarkName ,
BenchmarkRate ,
Spread 
)
SELECT
CONVERT(date, getdate()),
Account,
BenchmarkName,
BenchmarkRate,
Spread
FROM         
OPENXML(@handle, '//PositionMaster', 2)           
WITH         
(Date date, Account int, BenchmarkName Varchar(50),BenchmarkRate Varchar(50), Spread int)
INSERT INTO PM_CollateralInterest
	(Date,FundID, BenchmarkName, BenchmarkRate,Spread)
	SELECT Date,FundID, BenchmarkName, BenchmarkRate,Spread
	FROM #TEMPCOLLATERALINTEREST
COMMIT TRANSACTION TRAN1       
END TRY        
BEGIN CATCH         
 SET @ErrorMessage = ERROR_MESSAGE();        
 SET @ErrorNumber = Error_number();         
 ROLLBACK TRANSACTION TRAN1        
         
END CATCH;    
GO
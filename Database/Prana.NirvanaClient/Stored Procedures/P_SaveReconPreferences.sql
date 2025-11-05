-----------------------------------------------------------------
--Created BY: Pranay Deep
--Date: 21 October 2015
--Purpose: save Recon prefrences for check box - ShowCAGeneratedTrades
-----------------------------------------------------------------
CREATE PROCEDURE [dbo].[P_SaveReconPreferences]
(
@xml XML,
@ErrorMessage varchar(500) output,                                                                         
@ErrorNumber int output 
) 
AS
SET @ErrorNumber = 0                                                        
SET @ErrorMessage = 'Success'
BEGIN
      SET NOCOUNT ON;

 DECLARE @handle int                                     
  exec sp_xml_preparedocument @handle OUTPUT,@xml   
--Creating temporary table for storing data from xml   
------------------------------------------------------------------                          

CREATE TABLE #TempTable                                                                                             
(                                 
	ClientID int,                 
	ReconTypeID int,                                                
	TemplateName varchar(100), 
    TemplateKey varchar(100),
	IsShowCAGeneratedTrades bit
)                                                                                                                
  
Insert Into #TempTable                                                                                                                                                   
(                                                                                                                                                  
	ClientID,                 
	ReconTypeID,                    
	TemplateName,                            
	TemplateKey,    
	IsShowCAGeneratedTrades 
)                                                                                                                       
Select                                                                                                                                                   
	ClientID,                 
	ReconTypeID,                    
	TemplateName,                            
	TemplateKey,    
	IsShowCAGeneratedTrades
                               
FROM OPENXML(@handle, '//ArrayOfReconPreference/ReconPreference',2)                                                                                                                                                     
WITH                                                                        
(                                
    ClientID int,                 
	ReconTypeID int,                                                
	TemplateName varchar(100), 
    TemplateKey varchar(100),
	IsShowCAGeneratedTrades bit
)                         
------------------------------------------------------------------                          
------------------------------------------------------------------                          
--Checking if same row exits in the table "T_ReconPreferences"
--If YES then UPDATE the value of the column "IsShowCAGeneratedTrades"
--If NO the INSERT the whole row from the temporary table into table "T_ReconPreferences"                                                                                                                                                                                                          
IF EXISTS(
SELECT * FROM T_ReconPreferences RP
inner join   #TempTable on 
 RP.ClientID = #TempTable .ClientID
AND RP.ReconTypeID = #TempTable .ReconTypeID
AND RP.TemplateName = #TempTable .TemplateName
AND RP.TemplateKey = #TempTable .TemplateKey
--AND RP.IsShowCAGeneratedTrades = #TempTable .IsShowCAGeneratedTrades
) 
BEGIN
UPDATE T_ReconPreferences 
SET T_ReconPreferences.IsShowCAGeneratedTrades = #TempTable .IsShowCAGeneratedTrades
FROM T_ReconPreferences TRP
inner join   #TempTable on
 TRP.ClientID = #TempTable .ClientID
AND TRP.ReconTypeID = #TempTable .ReconTypeID
AND TRP.TemplateName = #TempTable .TemplateName
AND TRP.TemplateKey = #TempTable .TemplateKey
END
--In ELSE condition,INSERT the whole row from the temporary table into table "T_ReconPreferences"     
ELSE
BEGIN
	INSERT INTO T_ReconPreferences (
	ClientID, 
	ReconTypeID, 
	TemplateName, 
	TemplateKey, 
	IsShowCAGeneratedTrades) 
    select  ClientID,	ReconTypeID,TemplateName,TemplateKey,IsShowCAGeneratedTrades
  from #TempTable


END 
drop table #TempTable
 exec sp_xml_removedocument @handle
END 
--For checking the tables 
--select * from T_ReconPreferences
-- DELETE FROM T_ReconPreferences
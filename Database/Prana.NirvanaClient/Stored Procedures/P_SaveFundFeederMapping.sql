
-----------------------------------------------------------------
--Created BY: Bharat Raturi
--Date: 28/02/14
--Purpose: Save the mapping of the funds with the feeder funds
-----------------------------------------------------------------

CREATE procedure [dbo].[P_SaveFundFeederMapping]
(
@xmlDoc ntext,
@xmlAmount ntext,
@compID int
)
as
DECLARE @handle int
DECLARE @handle1 int                           
exec sp_xml_preparedocument @handle OUTPUT,@xmlDoc
exec sp_xml_preparedocument @handle1 OUTPUT,@xmlAmount 

CREATE TABLE #TempMapping                                                                               
(                                                                               
    CompanyFundId int,      
	CompanyFeederFundId int,
    AllocatedAmount decimal(10,2)                    
)        
insert INTO #TempMapping                                                                               
(CompanyFundId , CompanyFeederFundId , AllocatedAmount)
SELECT                                                                               
CompanyFundID, 
CompanyFeederFundID,
AllocatedAmount                                      
FROM OPENXML(@handle, '/dsMapping/dtMapping', 2)                                                                                 
WITH                                                                               
(                                                         
   CompanyFundID INT,                       
   CompanyFeederFundID INT,
   AllocatedAmount decimal(10,2)              
)                          
DELETE FROM T_CompanyFundFeederFundAssociation 
WHERE CompanyFeederFundID IN 
(SELECT feederfundid from T_CompanyFeederFunds where CompanyID=@compID)
Insert into T_CompanyFundFeederFundAssociation 
( 
CompanyFundID,
CompanyFeederFundID,
AllocatedAmount
) 
select CompanyFundId,CompanyFeederFundId,AllocatedAmount from #TempMapping  

     -------------------------------------

CREATE TABLE #TempAmount                                                                               
(                                                                               
    FeederFundId int,
    AllocatedAmount decimal(10,2),
    RemainingAmount decimal(10,2),
)        
insert INTO #TempAmount                                                                               
(FeederFundId , AllocatedAmount , RemainingAmount)
SELECT                                                                               
FeederFundId, 
AllocatedAmount,
RemainingAmount                                      
FROM OPENXML(@handle1, '/dsAmount/dtAmount', 2)                                                                                 
WITH                                                                               
(                                                         
   FeederFundID INT,                       
   AllocatedAmount decimal(10,2),
   RemainingAmount decimal(10,2)
)                          

--select * from #TempAmount

update T_CompanyFeederFunds 
SET RemainingAmount=#TempAmount.RemainingAmount,
AllocatedAmount=#TempAmount.AllocatedAmount
from
--T_CompanyFeederFunds f inner JOIN #TempAmount t on t.FeederFundId=f.FeederFundID 
#TempAmount
where  
#TempAmount.FeederFundId=T_CompanyFeederFunds.FeederFundID
EXEC sp_xml_removedocument @handle

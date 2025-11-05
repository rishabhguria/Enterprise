
-----------------------------------------------------------------

--modified BY: omshiv
--Date:26/sep 14
--Purpose: if AutoApprove is true then setting prices approved in markprice tables
--http://jira.nirvanasolutions.com:8080/browse/CHMW-1334

--modified BY: omshiv
--Date:8/Aug 14
--Purpose: Save the fund-symbol wise mark price details

--Created BY: Bharat Raturi
--Date: 12-4-14
--Purpose: Save the fund-symbol wise mark price details
--usage PM_SaveMarkPrices_Updated 
-----------------------------------------------------------------
 CREATE Proc [dbo].[PM_SaveMarkPrices_Updated]                                    
(                                    
   @Xml nText,
   @isAutoApproved bit    
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
                                     
 CREATE TABLE #TempMarkPrices                                             
 (                                                                                           
  Symbol varchar(100)                  
  ,Date datetime                                  
  ,MarkPrice float                  
  ,MarkPriceImportType Varchar(5)
  ,ForwardPoints float
  ,FundID int
  ,Source int
                                
 )                                                 
                                                                                          
 INSERT INTO #TempMarkPrices                                    
 (                                                                               
    Symbol                                  
   ,Date                                  
   ,MarkPrice                  
   ,MarkPriceImportType
   ,ForwardPoints
   ,FundID
   ,Source                
  )                                                                                          
 SELECT                                                                                          
    Symbol                                  
   ,Date                                  
   ,MarkPrice                   
   ,MarkPriceImportType
   ,isnull(ForwardPoints,0) as ForwardPoints
   ,AccountID
   ,Source
 FROM OPENXML(@handle, '//MarkPriceImport', 2)                                                                                             
  WITH                                                                                           
  (                                                                     
   Symbol varchar(100)                                       
  ,Date datetime                                  
  ,MarkPrice float                   
  ,MarkPriceImportType Varchar(5) 
  ,ForwardPoints float
  ,AccountID int  
  ,Source int                                       
  )         


update PM_DayMarkPrice 
SET 
PM_DayMarkPrice.AmendedMarkPrice=#TempMarkPrices.MarkPrice,
PM_DayMarkPrice.ForwardPoints=#TempMarkPrices.ForwardPoints,
PM_DayMarkPrice.FinalMarkPrice=
  Case                
  When @isAutoApproved='True'  
  Then #TempMarkPrices.MarkPrice                   
  ELSE PM_DayMarkPrice.FinalMarkPrice  -- for Live feed Mark Price                    
 End ,
PM_DayMarkPrice.IsApproved= 
case 
WHEN PM_DayMarkPrice.AmendedMarkPrice = #TempMarkPrices.MarkPrice
then PM_DayMarkPrice.IsApproved
else @isAutoApproved
end,
SourceID = #TempMarkPrices.Source

--select *
from   
PM_DayMarkPrice                          
inner join  #TempMarkPrices on DateDiff(d,#TempMarkPrices.Date,PM_DayMarkPrice.Date)=0                         
and (#TempMarkPrices.Symbol = PM_DayMarkPrice.Symbol and #TempMarkPrices.FundID=PM_DayMarkPrice.FundID)
 and #TempMarkPrices.MarkPrice != 0

 INSERT INTO          
 PM_DayMarkPrice                                    
 (                                    
   ApplicationMarkPrice,                                    
   PrimeBrokerMarkPrice,                                    
   AmendedMarkPrice, 
   FinalMarkPrice,                             
   Symbol,                                    
   IsActive,                                    
   Date,
   ForwardPoints,
   FundID,
   SourceID,
   IsApproved
 )                               

 SELECT  Distinct                                   
 0 as ApplicationMarkPrice,                  
 Case MarkPriceImportType                
  When 'P' -- for Prime Broker Mark Price                     
  Then MarkPrice                   
  When 'L'  -- for Live feed Mark Price                    
  Then 0                  
 Else 0                  
 End as PrimeBrokerMarkPrice,  
                
 Temp.MarkPrice,

 Case             
  WHEN @isAutoApproved='True'                    
  Then MarkPrice  
  ELSE 0  -- for Live feed Mark Price                    
                  
 End as FinalMarkPrice,
 
                                 
 Temp.Symbol,                                    
 1 as IsActive,                                  
 Temp.Date ,
 Temp.ForwardPoints,
 isnull(Temp.fundID,0) as FundID ,                          
 Temp.Source,
 @isAutoApproved as IsApproved
 FROM                                     
#TempMarkPrices Temp
left  join  PM_DayMarkPrice on DateDiff(d,Temp.Date,PM_DayMarkPrice.Date)=0                         
and (Temp.Symbol = PM_DayMarkPrice.Symbol and Temp.fundID=PM_DayMarkPrice.FundID)
where  PM_DayMarkPrice.DayMarkPriceID is NULL   

Delete dayMP from 
    PM_DayMarkPrice dayMP                       
    inner join  #TempMarkPrices temp 
    on DateDiff(d,temp.Date,dayMP.Date)=0                         
    and (temp.Symbol = dayMP.Symbol and temp.FundID=dayMP.FundID) and temp.MarkPrice = 0  
                
 DROP TABLE #TempMarkPrices                         
                                
 EXEC sp_xml_removedocument @handle                                    
                                     
COMMIT TRANSACTION TRAN1                                    
                               
END TRY
 BEGIN CATCH                                     
  SET @ErrorMessage = ERROR_MESSAGE();                                    
  print @errormessage                                    
  SET @ErrorNumber = Error_number();                                     
  ROLLBACK TRANSACTION TRAN1                                     
END CATCH;

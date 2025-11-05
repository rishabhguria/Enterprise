

/****************************************************************************                      
Name :   [PMGetFundWiseConversionRate]  
Purpose:  Returns all fund wise conversion forex rate for the date range passed.  
Module: MarkPriceAndForexConversion/PM                          
****************************************************************************/      
    
CREATE Procedure [dbo].[PMGetFundWiseConversionRate] (
@fundIDs varchar(max),                
@fromDate DateTime,                  
@ToDate DateTime,                  
@Type int, -- 0 for Same Date,1 for Week , 2 for Month
@filter int,                  
@ErrorMessage varchar(500) output,                            
@ErrorNumber int output                     
)                  
As                  
DECLARE @Dates varchar(2000)                  
DECLARE @FirstDateofMonth varchar(50)                  
DECLARE @LastDateofMonth varchar(50)                  
                  
If(@Type=0)                  
Begin                  
Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                   
Set @LastDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                   
end                  
Else If(@Type=1)                  
Begin                  
Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                   
Set @LastDateofMonth=CONVERT(VARCHAR(25),@ToDate,101)                   
End                  
Else If(@Type=2)                  
Begin                  
Set @FirstDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@fromDate)-1),@fromDate),101)      
Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)                 
END 
Else If(@Type=3) -- Month range                                                                      
Begin                                                                               
 Set @FirstDateofMonth=CONVERT(VARCHAR(25),@fromDate,101)                                                                      
 Set @LastDateofMonth=CONVERT(VARCHAR(25),@ToDate,101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                                                
END            
-- if DATEDIFF(m,CONVERT(VARCHAR(25),GetUTCDate(),101),CONVERT(VARCHAR(25),@fromDate,101)) = 0               
--  begin              
--   Set @LastDateofMonth=CONVERT(VARCHAR(25),GetUTcDate(),101)              
--  end              
-- else              
--  begin              
--   Set @LastDateofMonth=CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,@fromDate))),DATEADD(mm,1,@fromDate)),101)-- CONVERT(VARCHAR(25),GetUTcDate(),101)--                  
-- end                  
--End  
                  
SET @ErrorMessage = 'Success'                            
SET @ErrorNumber = 0                     
                  
BEGIN TRY  

declare @handle int  
exec sp_xml_preparedocument @handle output, @FundIDs  
  
create table #FundIDs  
(fundID int)  
  
insert INTO #FundIDs(fundID)  
  
select fundID from openXML(@handle,'dsFund/dtFund',2)  
with  
(FundID int)        
                  
SET @Dates = ''                  
SELECT @Dates = @Dates + '[' + convert(varchar(12),Items,101) + '],'                  
FROM (select Top 35 AllDates.Items                       
from dbo.GetDateRange(@FirstDateofMonth, @LastDateofMonth) as AllDates Order By AllDates.Items Desc) ForexDate                  
SET @Dates = LEFT(@Dates, LEN(@Dates) - 1) 

exec ('select *                  
from (Select distinct CCR.Date,CSP.FromCurrencyID,CSP.ToCurrencyID,CSP.eSignalSymbol as Symbol,'' '' as Summary,CSP.CurrencyPairID,
(select top(1) ConversionRate from T_CurrencyConversionRate where FundID=tempFund.fundID and Date=CCR.Date and CurrencyPairID_FK=CCR.CurrencyPairID_FK) as ConversionRate,
(select top(1) NULLIF(CCR.SourceID,0) from T_CurrencyConversionRate where FundID=tempFund.fundID and Date=CCR.Date and CurrencyPairID_FK=CCR.CurrencyPairID_FK) as SourceID,
(select NULLIF(CCR.IsApproved,0) from T_CurrencyConversionRate where FundID=tempFund.fundID and Date=CCR.Date and CurrencyPairID_FK=CCR.CurrencyPairID_FK) as IsApproved,
CASE WHEN CSP.BloombergSymbol='''' or CSP.BloombergSymbol is null THEN (select CurrencySymbol from T_Currency where currencyID=CSP.FromCurrencyID)+
(select CurrencySymbol from T_Currency where currencyID=CSP.ToCurrencyID) + '' CURNCY'' ELSE
CSP.BloombergSymbol END as BloombergSymbol,
tempFund.fundID,f.FundName
from T_CurrencyStandardPairs CSP
left outer Join  T_CurrencyConversionRate CCR On
CSP.CurrencyPairID=CCR.CurrencyPairID_FK
cross join #FundIDs tempFund
left outer JOIN (SELECT CompanyFundID,FundName from T_CompanyFunds Where IsActive=1)AS f   
on f.CompanyFundID=tempFund.fundID
where f.CompanyFundID in (select fundID from #FundIDs)
)
AS DMP PIVOT (MAX(ConversionRate) FOR Date IN (' + @Dates + ')) AS pvt; ')                
                 
exec sp_xml_removedocument @handle             
                 
END TRY                            
BEGIN CATCH                    
                              
 SET @ErrorMessage = ERROR_MESSAGE();                            
 SET @ErrorNumber = Error_number();
                             
END CATCH;


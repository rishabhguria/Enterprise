/*                
            
Modified by :<SAndeep>                    
Date : <27-May-2008>                    
purpose: <Added one more Field Full Security Name>            
          
Modified by :<SAndeep>                    
Date : <05-July-2008>                    
purpose: <Added 2 more Fields PBUniqueID and AllocationSeqNo>                 
*/                
                  
                
CREATE Procedure [dbo].[P_FFSaveThirdPartyDetailXML]        
 (                          
 @companyid int,                  
 @thirdPartyID int,                  
 @selectedFundIds varchar(100),                  
 @tradeDate DateTime ,                  
 @xmlDoc Varchar(max),                  
 @statusID int,                  
 @userID int,                  
 @filePath varchar(200),                  
 @FormatId int -- Added on 16-Aug-2007                  
)                  
As                  
                  
                  
----a temp table created to store the xml data.                   
Create Table #T_XmlData4                  
(                  
 [Side][varchar](50), [Symbol][varchar](50),[ExecQty][float],[AveragePrice][float],[OrderTypeTag][varchar](100),                  
 [OrderType][varchar](50),[AssetID][int],[Asset][varchar](50),[UnderLyingID][int],[UnderLying][varchar](50),[ExchangeID][int],[Exchange][varchar](50),                  
 [CurrencyID][int],[Currency][varchar](50),[AUECID][int],[SecFees][float],[CommissionRate][float],[CommissionCharged][float],[GrossAmt][float],[NetAmt][float],                  
 [SecurityIDType][varchar](50),[CommissionRateTypeID][int],[CommissionRateType][varchar](50),[CompanyCVID][int],[CVName][varchar](50),[CVIdentifier][varchar](50),                  
 [Percentage][float],[OrderQty][float],[EntityID][varchar](50),[CounterPartyID][int],[CounterParty][varchar](50),                  
 [AlllocQty][float],[TotalQty][float],[FundName][varchar](50),[FundMappedName][varchar](50),[FundAccountNo][varchar](50),[FundTypeID][int],[FundType][varchar](10),                  
 [CompanyFundID][int],[OtherBrokerFee][float],[PutOrCall][int],[CurrencySymbol][varchar](50),                  
 [StrikePrice][Decimal](18,4),[ExpirationDate][varchar](50),[SettlementDate][varchar](50),                
 [VenueID][int],[VenueName][varchar](50),[CUSIP][varchar](100),[ISIN][varchar](100),              
 [SEDOL][varchar](100),[RIC][varchar](100),[BBCode][varchar](100),[FullSecurityName][varchar](200),[PBUniqueID][bigint],[AllocationSeqNo][int],[TaxLotStateID][int],[ClearingBrokerFee][float]                  
)                  
                  
-- a temp table that contains the data copied from above table alongwith the 'T_ThirdPartyFFRunReport'table PKID.                  
Create Table #T_XmlDataNew4                  
(                  
 [ID] int identity(1,1) NOT NULL,[ThirdPartyFFRunID] [int] ,[Side][varchar](50), [Symbol][varchar](50),[ExecQty][float],[AveragePrice][float],[OrderTypeTag][varchar](100),                  
 [OrderType][varchar](50),[AssetID][int],[Asset][varchar](50),[UnderLyingID][int],[UnderLying][varchar](50),[ExchangeID][int],[Exchange][varchar](50),                  
 [CurrencyID][int],[Currency][varchar](50),[AUECID][int],[SecFees][float],[CommissionRate][float],[CommissionCharged][float],[GrossAmt][float],[NetAmt][float],                  
 [SecurityIDType][varchar](50),[CommissionRateTypeID][int],[CommissionRateType][varchar](50),[CompanyCVID][int],[CVName][varchar](50),[CVIdentifier][varchar](50),                  
 [Percentage][float],[OrderQty][float],[EntityID][varchar](50),[CounterPartyID][int],[CounterParty][varchar](50),                  
 [AlllocQty][float],[TotalQty][float],[FundName][varchar](50),[FundMappedName][varchar](50),[FundAccountNo][varchar](50),[FundTypeID][int],[FundType][varchar](10),                  
 [CompanyFundID][int],[OtherBrokerFee][float],[PutOrCall][int],[CurrencySymbol][varchar](50),                  
 [StrikePrice][Decimal](18,4),[ExpirationDate][varchar](50),[SettlementDate][varchar](50),                
 [VenueID][int],[VenueName][varchar](50),[CUSIP][varchar](100),[ISIN][varchar](100),              
 [SEDOL][varchar](100),[RIC][varchar](100),[BBCode][varchar](100),[FullSecurityName][varchar](200),[PBUniqueID][bigint],[AllocationSeqNo][int],[TaxLotStateID][int],[ClearingBrokerFee][float]                       
                  
)             
                  
DECLARE @handle int                    
Declare @total int                  
Declare @thirdPartyFFRunID int                
Declare @pKID int                  
set @pKID = 0                  
set @thirdPartyFFRunID = 0                  
set @total = 0     
                 
Declare @FundIDTable Table                                                              
(                                                              
FundID int                                                            
)        
declare @UpdateIds table     
(    
  ID int    
)             
                  
Begin Tran     
    
Insert Into @FundIDTable                     
Select * from  dbo.Split(@selectedFundIds,',')      
     
SELECT    @thirdPartyFFRunID= ThirdPartyFFRunID                  
FROM   T_ThirdPartyFFRunReport Where CompanyID=@companyid and CompanyThirdPartyID=@thirdPartyID    
And dbo.GetFormattedDatePart(TradeDate)=dbo.GetFormattedDatePart(@tradeDate)    
And FormatId=@FormatId    
    
if(@thirdPartyFFRunID > 0)    
Begin    
 Update T_ThirdPartyFFRunReport    
 Set     
 FFUserID=@userID,    
 CompanyID=@companyid,    
 CompanyThirdPartyID=@thirdPartyID,    
 TradeDate=@tradeDate,    
 FormatId=@FormatId,    
 StatusID=@statusID,    
 FilePathName=@filePath    
 Where T_ThirdPartyFFRunReport.ThirdPartyFFRunID=@thirdPartyFFRunID    
End    
    
Else    
 Begin    
 --insert the 'T_ThirdPartyFFRunReport' details and fetch the generated PKID                  
 Insert into T_ThirdPartyFFRunReport (FFUserID, CompanyID,CompanyThirdPartyID,TradeDate,StatusID,FilePathName,FormatId)                  
 values (@userID,@companyID,@thirdPartyID,@tradeDate,@statusID,@filePath,@FormatId)                  
 Set @thirdPartyFFRunID = scope_identity()     
End    
    
SELECT    @total= count(*)                  
FROM  T_ThirdPartyFFRunFunds Where T_ThirdPartyFFRunFunds.ThirdPartyFFRunID_FK= @thirdPartyFFRunID      
if(@total > 0)    
 Begin    
 Insert into T_ThirdPartyFFRunFunds(CompanyThirdPartyID_FK,ThirdPartyFFRunID_FK,CompanyFundID)                  
 select @thirdPartyID,@thirdPartyFFRunID,FundIDTable.FundID from @FundIDTable FundIDTable     
 Where FundIDTable.FundID  not in     
 (Select T_ThirdPartyFFRunFunds.CompanyFundID from T_ThirdPartyFFRunFunds )    
 End    
Else    
 Begin    
 --insert the 'T_ThirdPartyFFRunFunds'details alongwith the T_ThirdPartyFFRunReport PKID                  
 Insert into T_ThirdPartyFFRunFunds(CompanyThirdPartyID_FK,ThirdPartyFFRunID_FK,CompanyFundID)                  
 select @thirdPartyID,@thirdPartyFFRunID,FundIDTable.FundID from @FundIDTable FundIDTable     
-- Where FundIDTable.FundID  not in     
-- (Select T_ThirdPartyFFRunFunds.CompanyFundID from T_ThirdPartyFFRunFunds )    
 End     
                
----count the existing rows for the given parameters.                  
--SELECT    @total= count(*)                  
--FROM         T_ThirdPartyFFRunFunds INNER JOIN                  
--                      T_ThirdPartyFFRunReport ON T_ThirdPartyFFRunFunds.ThirdPartyFFRunID_FK = T_ThirdPartyFFRunReport.ThirdPartyFFRunID                  
--WHERE     (T_ThirdPartyFFRunReport.CompanyThirdPartyID = @thirdPartyID) AND (T_ThirdPartyFFRunReport.TradeDate = @tradeDate) AND                   
--                     (T_ThirdPartyFFRunReport.FormatId=@FormatId) AND (T_ThirdPartyFFRunFunds.CompanyFundID in (Select Items from dbo.Split(@selectedFundIds,',')))                  
--                  
--if @total > 0                  
--begin                  
-- -- select the primary key of the master table.                  
-- SELECT    @pKID = T_ThirdPartyFFRunFunds.ThirdPartyFFRunID_FK                  
-- FROM         T_ThirdPartyFFRunFunds INNER JOIN           
--        T_ThirdPartyFFRunReport ON T_ThirdPartyFFRunFunds.ThirdPartyFFRunID_FK = T_ThirdPartyFFRunReport.ThirdPartyFFRunID                  
-- WHERE     (T_ThirdPartyFFRunFunds.CompanyFundID in (Select Items from dbo.Split(@selectedFundIds,','))) AND (T_ThirdPartyFFRunReport.TradeDate = @tradeDate) AND                   
--    (T_ThirdPartyFFRunReport.FormatId=@FormatId) AND (T_ThirdPartyFFRunReport.CompanyThirdPartyID = @thirdPartyID)                  
--                  
-- --delete the data from 'T_ThirdPartyFFRunFunds' table                  
-- DELETE FROM T_ThirdPartyFFRunFunds                  
-- FROM         T_ThirdPartyFFRunFunds INNER JOIN                  
--        T_ThirdPartyFFRunReport ON T_ThirdPartyFFRunFunds.ThirdPartyFFRunID_FK = T_ThirdPartyFFRunReport.ThirdPartyFFRunID                  
-- WHERE     (T_ThirdPartyFFRunFunds.CompanyFundID in (Select Items from dbo.Split(@selectedFundIds,','))) AND (T_ThirdPartyFFRunReport.TradeDate = @tradeDate) AND                   
--    (T_ThirdPartyFFRunReport.FormatId=@FormatId) AND (T_ThirdPartyFFRunReport.CompanyThirdPartyID = @thirdPartyID)                  
--                  
-- --delete from the 'T_ThirdPartyFFRunFundDetails' table                  
---- DELETE FROM T_ThirdPartyFFRunFundDetails                  
----FROM         T_ThirdPartyFFRunFundDetails INNER JOIN                  
----                      T_ThirdPartyFFRunReport ON T_ThirdPartyFFRunFundDetails.ThirdPartyFFRunID = T_ThirdPartyFFRunReport.ThirdPartyFFRunID                  
----WHERE     (T_ThirdPartyFFRunFundDetails.CompanyFundID in (Select Items from dbo.Split(@selectedFundIds,','))) AND (T_ThirdPartyFFRunReport.TradeDate = @tradeDate) AND                   
----                    (T_ThirdPartyFFRunReport.FormatId=@FormatId) AND (T_ThirdPartyFFRunReport.CompanyThirdPartyID = @thirdPartyID)                  
----                  
-- --select row count from 'T_ThirdPartyFFRunFunds' table                  
-- SELECT   @total= count(*)                  
-- FROM         T_ThirdPartyFFRunFunds                  
-- WHERE     (ThirdPartyFFRunID_FK = @pKID)                  
--                  
-- -- in case of no other funds existing for the given data, it will delete data frm 'T_ThirdPartyFFRunReport' table                  
-- if @total = 0                  
-- begin                  
--  Delete from T_ThirdPartyFFRunReport where T_ThirdPartyFFRunReport.ThirdPartyFFRunID = @pKID                  
-- end                  
--                  
--end                  
--                  
----insert the 'T_ThirdPartyFFRunReport' details and fetch the generated PKID                  
--Insert into T_ThirdPartyFFRunReport (FFUserID, CompanyID,CompanyThirdPartyID,TradeDate,StatusID,FilePathName,FormatId)                  
--values (@userID,@companyID,@thirdPartyID,@tradeDate,@statusID,@filePath,@FormatId)                  
--Set @thirdPartyFFRunID = scope_identity()                  
--                  
----insert the 'T_ThirdPartyFFRunFunds'details alongwith the T_ThirdPartyFFRunReport PKID                  
--Insert into T_ThirdPartyFFRunFunds(CompanyThirdPartyID_FK,ThirdPartyFFRunID_FK,CompanyFundID)                  
--select @thirdPartyID,@thirdPartyFFRunID,Items from dbo.Split(@selectedFundIds,',')                  
--             
EXEC sp_xml_preparedocument @handle OUTPUT, @xmlDoc                  
                  
--This code inserts new data.                  
Insert Into #T_XmlData4                  
 (Side, Symbol,ExecQty,AveragePrice,OrderTypeTag,                  
 OrderType,AssetID,Asset,UnderLyingID,UnderLying,ExchangeID,Exchange,                  
 CurrencyID,Currency,AUECID,SecFees,CommissionRate,CommissionCharged,GrossAmt,NetAmt,                  
 SecurityIDType,CommissionRateTypeID,CommissionRateType,CompanyCVID,CVName,CVIdentifier,               
 Percentage,OrderQty,EntityID,CounterPartyID,CounterParty,                  
 AlllocQty,TotalQty,FundName,FundMappedName,FundAccountNo,FundTypeID,FundType,CompanyFundID,OtherBrokerFee,                  
 PutOrCall,CurrencySymbol,StrikePrice,ExpirationDate,SettlementDate,VenueID,VenueName,CUSIP,ISIN,SEDOL,RIC,BBCode,          
 FullSecurityName,PBUniqueID,AllocationSeqNo,TaxLotStateID,ClearingBrokerFee)                  
SELECT  Side, Symbol,ExecutedQty,AveragePrice,OrderTypeTag,                  
 OrderType,AssetID,Asset,UnderLyingID,UnderLying,ExchangeID,Exchange,              
 CurrencyID,CurrencyName,AUECID,SecFees,Commission,CommissionCharged,GrossAmount,NetAmount,                  
 SecurityIDType,CommissionRateTypeID,CommissionRateType,CompanyCVID,CVName,CVIdentifier,                  
 Percentage,OrderQty,EntityID,CounterPartyID,CounterParty,                  
 AllocatedQty,TotalQty,FundName,FundMappedName,FundAccountNo,CompanyFundTypeID,CompanyFundType,CompanyFundID,OtherBrokerFee,                  
 PutOrCall,CurrencySymbol,StrikePrice,ExpirationDate,SettlementDate,VenueID,VenueName,CUSIP,ISIN,SEDOL,RIC,BBCode,          
 FullSecurityName,PBUniqueID,AllocationSeqNo,TaxLotStateID,ClearingBrokerFee    
FROM OPENXML (@handle, '//ThirdPartyFlatFileDetail',2)                  
WITH ( Side varchar(50), Symbol varchar(50),ExecutedQty float,AveragePrice float,OrderTypeTag varchar(100),                  
 OrderType varchar(50),AssetID int,Asset varchar(50),UnderLyingID int,UnderLying varchar(50),ExchangeID int,Exchange varchar(50),                  
 CurrencyID int,CurrencyName varchar(50),AUECID int,SecFees float,Commission float,CommissionCharged float,GrossAmount float,NetAmount float,                  
 SecurityIDType varchar(50),CommissionRateTypeID int,CommissionRateType varchar(50),CompanyCVID int,CVName varchar(50),CVIdentifier varchar(50),                  
 Percentage float,OrderQty float,EntityID varchar(50),CounterPartyID int,CounterParty varchar(50),                  
 AllocatedQty float,TotalQty float,FundName varchar(50),FundMappedName varchar(50),FundAccountNo varchar(50),CompanyFundTypeID int,CompanyFundType varchar(10),CompanyFundID int,OtherBrokerFee float,                  
 PutOrCall int,CurrencySymbol varchar(50),                  
 StrikePrice Decimal(18,4),ExpirationDate varchar(50),SettlementDate varchar(50),VenueID int,VenueName varchar(50),              
 CUSIP varchar(100),ISIN varchar(100),SEDOL varchar(100),RIC varchar(100),BBCode varchar(100),          
 FullSecurityName varchar(200),PBUniqueID bigint,AllocationSeqNo int,TaxLotStateID int,ClearingBrokerFee float)                  
                  
--insert the data alongwith the T_ThirdPartyFFRunReport PKID into #T_XmlDataNew4                  
insert into #T_XmlDataNew4 (ThirdPartyFFRunID,Side, Symbol,ExecQty,AveragePrice,OrderTypeTag,                  
 OrderType,AssetID,Asset,UnderLyingID,UnderLying,ExchangeID,Exchange,                  
 CurrencyID,Currency,AUECID,SecFees,CommissionRate,CommissionCharged,GrossAmt,NetAmt,                  
 SecurityIDType,CommissionRateTypeID,CommissionRateType,CompanyCVID,CVName,CVIdentifier,                  
 Percentage,OrderQty,EntityID,CounterPartyID,CounterParty,                  
 AlllocQty,TotalQty,FundName,FundMappedName,FundAccountNo,FundTypeID,FundType,CompanyFundID,OtherBrokerFee,                  
 PutOrCall,CurrencySymbol,StrikePrice,ExpirationDate,SettlementDate,VenueID,VenueName,CUSIP,ISIN,SEDOL,RIC,BBCode,          
 FullSecurityName,PBUniqueID,AllocationSeqNo,TaxLotStateID,ClearingBrokerFee )                  
SELECT  @thirdPartyFFRunID,Side, Symbol,ExecQty,AveragePrice,OrderTypeTag,                  
 OrderType,AssetID,Asset,UnderLyingID,UnderLying,ExchangeID,Exchange,                  
 CurrencyID,Currency,AUECID,SecFees,CommissionRate,CommissionCharged,GrossAmt,NetAmt,           
 SecurityIDType,CommissionRateTypeID,CommissionRateType,CompanyCVID,CVName,CVIdentifier,                  
 Percentage,OrderQty,EntityID,CounterPartyID,CounterParty,                  
 AlllocQty,TotalQty,FundName,FundMappedName,FundAccountNo,FundTypeID,FundType,CompanyFundID,OtherBrokerFee,                  
 PutOrCall,CurrencySymbol,StrikePrice,ExpirationDate,SettlementDate,VenueID,VenueName,CUSIP,ISIN,SEDOL,RIC,BBCode,          
 FullSecurityName,PBUniqueID,AllocationSeqNo,TaxLotStateID,ClearingBrokerFee          
FROM #T_XmlData4                  
        
    
Insert into @UpdateIds    
select ID from #T_XmlDataNew4 temp    
left outer join T_ThirdPartyFFRunFundDetails abc    
on temp.EntityID = abc.EntityID    
where temp.EntityID = abc.EntityID and temp.TaxlotStateID = abc.TaxlotStateID              
--finally , insert into 'T_ThirdPartyFFRunFundDetails'table .     
    
update T_ThirdPartyFFRunFundDetails     
set    
T_ThirdPartyFFRunFundDetails.ThirdPartyFFRunID= #T_XmlDataNew4.ThirdPartyFFRunID,    
T_ThirdPartyFFRunFundDetails.Side= #T_XmlDataNew4.Side,    
T_ThirdPartyFFRunFundDetails.Symbol= #T_XmlDataNew4.Symbol,    
T_ThirdPartyFFRunFundDetails.ExecQty= #T_XmlDataNew4.ExecQty,    
T_ThirdPartyFFRunFundDetails.AveragePrice= #T_XmlDataNew4.AveragePrice,    
T_ThirdPartyFFRunFundDetails.OrderTypeTag= #T_XmlDataNew4.OrderTypeTag,    
T_ThirdPartyFFRunFundDetails.OrderType= #T_XmlDataNew4.OrderType,    
T_ThirdPartyFFRunFundDetails.AssetID= #T_XmlDataNew4.AssetID,    
T_ThirdPartyFFRunFundDetails.Asset= #T_XmlDataNew4.Asset,    
T_ThirdPartyFFRunFundDetails.UnderLyingID= #T_XmlDataNew4.UnderLyingID,    
T_ThirdPartyFFRunFundDetails.UnderLying= #T_XmlDataNew4.UnderLying,    
T_ThirdPartyFFRunFundDetails.ExchangeID= #T_XmlDataNew4.ExchangeID,    
T_ThirdPartyFFRunFundDetails.Exchange= #T_XmlDataNew4.Exchange,    
T_ThirdPartyFFRunFundDetails.CurrencyID= #T_XmlDataNew4.CurrencyID,    
T_ThirdPartyFFRunFundDetails.Currency= #T_XmlDataNew4.Currency,    
T_ThirdPartyFFRunFundDetails.AUECID= #T_XmlDataNew4.AUECID,    
T_ThirdPartyFFRunFundDetails.SecFees= #T_XmlDataNew4.SecFees,    
T_ThirdPartyFFRunFundDetails.CommissionRate= #T_XmlDataNew4.CommissionRate,    
T_ThirdPartyFFRunFundDetails.CommissionCharged= #T_XmlDataNew4.CommissionCharged,    
T_ThirdPartyFFRunFundDetails.GrossAmt= #T_XmlDataNew4.GrossAmt,    
T_ThirdPartyFFRunFundDetails.NetAmt= #T_XmlDataNew4.NetAmt,    
T_ThirdPartyFFRunFundDetails.SecurityIDType= #T_XmlDataNew4.SecurityIDType,    
T_ThirdPartyFFRunFundDetails.CommissionRateTypeID= #T_XmlDataNew4.CommissionRateTypeID,    
T_ThirdPartyFFRunFundDetails.CommissionRateType= #T_XmlDataNew4.CommissionRateType,    
T_ThirdPartyFFRunFundDetails.CompanyCVID= #T_XmlDataNew4.CompanyCVID,    
T_ThirdPartyFFRunFundDetails.CVName= #T_XmlDataNew4.CVName,    
T_ThirdPartyFFRunFundDetails.CVIdentifier= #T_XmlDataNew4.CVIdentifier,    
T_ThirdPartyFFRunFundDetails.Percentage= #T_XmlDataNew4.Percentage,    
T_ThirdPartyFFRunFundDetails.OrderQty= #T_XmlDataNew4.OrderQty,    
--T_ThirdPartyFFRunFundDetails.EntityID= #T_XmlDataNew4.EntityID,    
T_ThirdPartyFFRunFundDetails.CounterPartyID= #T_XmlDataNew4.CounterPartyID,    
T_ThirdPartyFFRunFundDetails.CounterParty= #T_XmlDataNew4.CounterParty,    
T_ThirdPartyFFRunFundDetails.AlllocQty= #T_XmlDataNew4.AlllocQty,    
T_ThirdPartyFFRunFundDetails.TotalQty= #T_XmlDataNew4.TotalQty,    
T_ThirdPartyFFRunFundDetails.FundName= #T_XmlDataNew4.FundName,    
T_ThirdPartyFFRunFundDetails.FundMappedName= #T_XmlDataNew4.FundMappedName,    
T_ThirdPartyFFRunFundDetails.FundAccountNo= #T_XmlDataNew4.FundAccountNo,    
T_ThirdPartyFFRunFundDetails.FundTypeID= #T_XmlDataNew4.FundTypeID,    
T_ThirdPartyFFRunFundDetails.FundType= #T_XmlDataNew4.FundType,    
T_ThirdPartyFFRunFundDetails.CompanyFundID= #T_XmlDataNew4.CompanyFundID,    
T_ThirdPartyFFRunFundDetails.OtherBrokerFee= #T_XmlDataNew4.OtherBrokerFee,    
T_ThirdPartyFFRunFundDetails.PutOrCall= #T_XmlDataNew4.PutOrCall,    
T_ThirdPartyFFRunFundDetails.CurrencySymbol= #T_XmlDataNew4.CurrencySymbol,    
T_ThirdPartyFFRunFundDetails.StrikePrice= #T_XmlDataNew4.StrikePrice,    
T_ThirdPartyFFRunFundDetails.ExpirationDate= #T_XmlDataNew4.ExpirationDate,    
T_ThirdPartyFFRunFundDetails.SettlementDate= #T_XmlDataNew4.SettlementDate,    
T_ThirdPartyFFRunFundDetails.VenueID= #T_XmlDataNew4.VenueID,    
T_ThirdPartyFFRunFundDetails.VenueName= #T_XmlDataNew4.VenueName,    
T_ThirdPartyFFRunFundDetails.CUSIP= #T_XmlDataNew4.CUSIP,    
T_ThirdPartyFFRunFundDetails.ISIN= #T_XmlDataNew4.ISIN,    
T_ThirdPartyFFRunFundDetails.SEDOL= #T_XmlDataNew4.SEDOL,    
T_ThirdPartyFFRunFundDetails.RIC= #T_XmlDataNew4.RIC,    
T_ThirdPartyFFRunFundDetails.BBCode= #T_XmlDataNew4.BBCode,    
T_ThirdPartyFFRunFundDetails.FullSecurityName= #T_XmlDataNew4.FullSecurityName,    
T_ThirdPartyFFRunFundDetails.PBUniqueID= #T_XmlDataNew4.PBUniqueID,    
T_ThirdPartyFFRunFundDetails.AllocationSeqNo= #T_XmlDataNew4.AllocationSeqNo,    
T_ThirdPartyFFRunFundDetails.ClearingBrokerFee= #T_XmlDataNew4.ClearingBrokerFee    
--T_ThirdPartyFFRunFundDetails.TaxLotStateID= #T_XmlDataNew4.TaxLotStateID    
-- FROM #T_XmlDataNew4 where  #T_XmlDataNew4.ID  in     
--(select TempIDs.ID from @UpdateIds TempIDs )    
FROM #T_XmlDataNew4 Where T_ThirdPartyFFRunFundDetails.EntityID = #T_XmlDataNew4.EntityID     
And T_ThirdPartyFFRunFundDetails.TaxLotStateID = #T_XmlDataNew4.TaxLotStateID    
    
                 
Insert into T_ThirdPartyFFRunFundDetails(ThirdPartyFFRunID,Side, Symbol,ExecQty,AveragePrice,OrderTypeTag,                  
 OrderType,AssetID,Asset,UnderLyingID,UnderLying,ExchangeID,Exchange,                  
 CurrencyID,Currency,AUECID,SecFees,CommissionRate,CommissionCharged,GrossAmt,NetAmt,                  
 SecurityIDType,CommissionRateTypeID,CommissionRateType,CompanyCVID,CVName,CVIdentifier,                  
 Percentage,OrderQty,EntityID,CounterPartyID,CounterParty,                  
 AlllocQty,TotalQty,FundName,FundMappedName,FundAccountNo,FundTypeID,FundType,CompanyFundID,OtherBrokerFee,                  
 PutOrCall,CurrencySymbol,StrikePrice,ExpirationDate,SettlementDate,VenueID,VenueName,CUSIP,ISIN,SEDOL,RIC,BBCode,          
 FullSecurityName,PBUniqueID,AllocationSeqNo,TaxLotStateID,ClearingBrokerFee)                   
select ThirdPartyFFRunID,Side, Symbol,ExecQty,AveragePrice,OrderTypeTag,                  
 OrderType,AssetID,Asset,UnderLyingID,UnderLying,ExchangeID,Exchange,                  
 CurrencyID,Currency,AUECID,SecFees,CommissionRate,CommissionCharged,GrossAmt,NetAmt,                  
 SecurityIDType,CommissionRateTypeID,CommissionRateType,CompanyCVID,CVName,CVIdentifier,                  
 Percentage,OrderQty,EntityID,CounterPartyID,CounterParty,                  
 AlllocQty,TotalQty,FundName,FundMappedName,FundAccountNo,FundTypeID,FundType,CompanyFundID,OtherBrokerFee,                  
 PutOrCall,CurrencySymbol,StrikePrice,ExpirationDate,SettlementDate,VenueID,VenueName,CUSIP,ISIN,SEDOL,RIC,BBCode,          
 FullSecurityName,PBUniqueID,AllocationSeqNo,TaxLotStateID,ClearingBrokerFee            
from #T_XmlDataNew4      
where  #T_XmlDataNew4.ID not in     
(select TempIDs.ID from @UpdateIds TempIDs )--where #T_XmlDataNew4.TaxLotStateID)    
                
IF @@ERROR <> 0                  
   begin                  
                   
 rollback tran                  
end                  
else                  
begin                   
                   
 commit tran                  
end                  
-- Remove the internal representation.                  
EXEC sp_xml_removedocument @handle                  
                  
drop table #T_XmlDataNew4                   
drop table #T_XmlData4 


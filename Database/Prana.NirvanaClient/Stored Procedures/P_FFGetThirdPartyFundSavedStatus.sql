/*             
            
Modified by : <Sandeep>                  
Date : <27-05-2008>                  
Purpose : <Fetch FullSecurityName field>            
          
Last Modify By Sandeep as on 05-july-2008 th fetch the PBUniqueID and AllocationSeqNo           
                
Execute                
[P_FFGetThirdPartyFundSavedStatus] 17,1,'25-May-2008',20,1                
                
*/                  
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundSavedStatus]      
 (                  
 @thirdPartyID int ,                  
 @companyFundID int ,                  
 @inputDate DateTime,                  
 @FormatId int,                
 @companyID int,  
 @auecIDs varchar(max)                  
 )                  
                   
AS                  
                  
SELECT                      
                      T_ThirdPartyFFRunFundDetails.ThirdPartyFFRunID, T_ThirdPartyFFRunReport.FFUserID,T_ThirdPartyFFRunReport.CompanyID,                  
                      T_ThirdPartyFFRunReport.CompanyThirdPartyID, T_ThirdPartyFFRunReport.TradeDate,                   
                      Isnull(T_ThirdPartyFFRunReport.StatusID,-1), isnull(T_ThirdPartyFFRunReport.FilePathName,''),T_ThirdPartyFFRunFundDetails.CompanyFundID,                  
                      isnull(T_ThirdPartyFFRunStatus.Status,''), isnull(T_ThirdPartyFFRunFundDetails.Side,''),isnull(T_ThirdPartyFFRunFundDetails.Symbol,''),isnull(T_ThirdPartyFFRunFundDetails.ExecQty,0),                  
                      isnull(T_ThirdPartyFFRunFundDetails.OrderType,''),isnull(T_ThirdPartyFFRunFundDetails.OrderTypeTag,0),isnull(T_ThirdPartyFFRunFundDetails.AveragePrice,0),                   
                      isnull(T_ThirdPartyFFRunFundDetails.Asset,''), isnull(T_ThirdPartyFFRunFundDetails.AssetID,0),                   
                      isnull(T_ThirdPartyFFRunFundDetails.UnderLyingID,0), isnull(T_ThirdPartyFFRunFundDetails.UnderLying,''),isnull(T_ThirdPartyFFRunFundDetails.CurrencyID,0),                  
                      isnull(T_ThirdPartyFFRunFundDetails.Exchange,''), isnull(T_ThirdPartyFFRunFundDetails.ExchangeID,0),  isnull(T_ThirdPartyFFRunFundDetails.Currency,''),                  
                      isnull(T_ThirdPartyFFRunFundDetails.CommissionRate,0),isnull(T_ThirdPartyFFRunFundDetails.SecFees,0),isnull(T_ThirdPartyFFRunFundDetails.AUECID,0), isnull(T_ThirdPartyFFRunFundDetails.NetAmt,0),                  
                      isnull(T_ThirdPartyFFRunFundDetails.GrossAmt,0), isnull(T_ThirdPartyFFRunFundDetails.CommissionCharged,0),                   
                      isnull(T_ThirdPartyFFRunFundDetails.SecurityIDType,''), isnull(T_ThirdPartyFFRunFundDetails.CommissionRateTypeID,0),                                         
                      isnull(T_ThirdPartyFFRunFundDetails.CommissionRateType,''), isnull(T_ThirdPartyFFRunFundDetails.CompanyCVID,0),isnull(T_ThirdPartyFFRunFundDetails.CVName,''),                                         
                      isnull(T_ThirdPartyFFRunFundDetails.CVIdentifier,''), isnull(T_ThirdPartyFFRunFundDetails.Percentage,0), isnull(T_ThirdPartyFFRunFundDetails.OrderQty,0),                   
                      isnull(T_ThirdPartyFFRunFundDetails.EntityID,''), isnull(T_ThirdPartyFFRunFundDetails.CounterPartyID,0),                   
                                        
                      isnull(T_ThirdPartyFFRunFundDetails.CounterParty,''), isnull(T_ThirdPartyFFRunFundDetails.AlllocQty,0),                   
                      isnull(T_ThirdPartyFFRunFundDetails.TotalQty,0), isnull(T_ThirdPartyFFRunFundDetails.FundName,''), isnull(T_ThirdPartyFFRunFundDetails.FundMappedName,''),                   
                      isnull(T_ThirdPartyFFRunFundDetails.FundAccountNo,''), isnull(T_ThirdPartyFFRunFundDetails.FundTypeID,0), isnull(T_ThirdPartyFFRunFundDetails.FundType,''),                  
                      isnull(T_ThirdPartyFFRunFundDetails.OtherBrokerFee,0) ,                  
       isnull(T_ThirdPartyFFRunFundDetails.PutOrCall,-2147483648) ,isnull(T_ThirdPartyFFRunFundDetails.CurrencySymbol,'') ,                  
       isnull(T_ThirdPartyFFRunFundDetails.StrikePrice,0),isnull(T_ThirdPartyFFRunFundDetails.ExpirationDate,''),                  
       isnull(T_ThirdPartyFFRunFundDetails.SettlementDate,''),                
       isnull(T_CompanyThirdPartyFlatFileSaveDetails.CompanyIdentifier,''),                
       isnull(T_ThirdPartyType.ThirdPartyTypeID,0) as ThirdPartyTypeID,                
       isnull(T_ThirdPartyType.ThirdPartyTypeName,'') as ThirdPartyType,                
       isnull(T_ThirdPartyFFRunFundDetails.VenueID,0) as VenueID,                
       isnull(T_ThirdPartyFFRunFundDetails.VenueName,'') as VenueName,                
       isnull(T_ThirdParty.ThirdPartyName,'') as ThirdPartyName,              
       isnull(T_ThirdPartyFFRunFundDetails.CUSIP,'') as CUSIP,                 
       isnull(T_ThirdPartyFFRunFundDetails.ISIN,'') as ISIN,              
       isnull(T_ThirdPartyFFRunFundDetails.SEDOL,'') as SEDOL,              
       isnull(T_ThirdPartyFFRunFundDetails.RIC,'') as RIC,              
       isnull(T_ThirdPartyFFRunFundDetails.BBCode,'') as BBCode,            
       isnull(T_ThirdPartyFFRunFundDetails.FullSecurityName,'') as FullSecurityName,          
          isnull(T_ThirdPartyFFRunFundDetails.PBUniqueID,0) as PBUniqueID  ,          
          isnull(T_ThirdPartyFFRunFundDetails.AllocationSeqNo,0) as AllocationSeqNo,      
       isnull(T_ThirdPartyFFRunFundDetails.TaxLotStateID,0) as TaxLotStateID,                  
        isnull(T_ThirdPartyFFRunFundDetails.ClearingBrokerFee,0)    
                                        
FROM         T_ThirdPartyFFRunFundDetails INNER JOIN                  
             T_ThirdPartyFFRunReport ON T_ThirdPartyFFRunFundDetails.ThirdPartyFFRunID = T_ThirdPartyFFRunReport.ThirdPartyFFRunID                 
    INNER JOIN                  
             T_ThirdPartyFFRunStatus ON T_ThirdPartyFFRunReport.StatusID = T_ThirdPartyFFRunStatus.StatusID                  
    INNER JOIN                 
             T_CompanyThirdPartyFlatFileSaveDetails ON T_ThirdPartyFFRunReport.CompanyThirdPartyID=T_CompanyThirdPartyFlatFileSaveDetails.CompanyThirdPartyID                
 INNER JOIN                
   T_CompanyThirdParty on T_CompanyThirdParty.CompanyThirdPartyID=T_ThirdPartyFFRunReport.CompanyThirdPartyID                 
 INNER JOIN                 
   T_ThirdParty on T_ThirdParty.ThirdPartyId=T_CompanyThirdParty.ThirdPartyId AND  T_CompanyThirdParty.CompanyID=@companyID                   
 INNER JOIN                 
   T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeID=T_ThirdParty.ThirdPartyTypeID                
                
WHERE     (T_ThirdPartyFFRunReport.TradeDate = @inputDate) AND (T_ThirdPartyFFRunReport.CompanyThirdPartyID = @thirdPartyID) AND                   
          (T_ThirdPartyFFRunFundDetails.CompanyFundID = @companyFundID) AND (T_ThirdPartyFFRunReport.FormatId=@FormatId) And  
    T_ThirdPartyFFRunFundDetails.AUECID in (select Cast(Items as int) from dbo.Split(@auecIDs,',')) 


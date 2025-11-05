-----------------------------------------------------------------  
  
--modified BY: Omshiv  
--Date: 1/11/14  
--Purpose: Get Fund Wise import BatchID and recon IDs

-----------------------------------------------------------------  
CREATE  Proc [dbo].[P_GetFundWiseBatchData]                                    
as             
SELECT
F.CompanyFundID,
FS.FormatName,
FormatType
FROM T_CompanyFunds as F  
inner JOIN T_ImportFileSettings as I ON I.FundID = F.CompanyFundID
inner JOIN T_BatchSchedulers as FS ON FS.BatchSchedulerID = I.ImportFileSettingID
WHERE F.IsActive = 1 AND I.IsActive=1 -- and I.FormatType=0



CREATE Procedure [dbo].[P_GetThirdPartyForceConfirmAuditData]  
(  
 @thirdPartyBatchId INT,  
 @runDate DATETIME  
)  
AS  
BEGIN  
 SELECT CompanyUserId, ConfirmationDateTime, Broker, Symbol, Side, Quantity, AllocationId,ThirdPartyBatchId, Comment, ShortName AS UserName FROM T_ThirdPartyForceConfirmAudit  FCA
 JOIN T_CompanyUser CU
 ON FCA.CompanyUserId = CU.UserID
 WHERE DATEDIFF(d, ConfirmationDateTime, @runDate) = 0 AND ThirdPartyBatchId = @thirdPartyBatchId  
END
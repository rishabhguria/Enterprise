-----------------------------------------------------------------  
  
--modified BY: Omshiv  
--Date: 1/11/14  
--Purpose: Get Fund Wise import BatchID and recon IDs

-----------------------------------------------------------------  
CREATE proc [dbo].[P_GetFundWiseUDAData]
as
SELECT 
F.FundID,
F.Symbol_pk,
F.UDAAssetClassID,
F.UDASecurityTypeID,
F.UDASectorID,
F.UDASubSectorID,
F.UDACountryID,
F.IsApproved,
F.PrimarySymbol,
F.CreatedBy,
F.ModifiedBy,
F.ApprovedBy,
F.CreationDate,
F.ModifiedDate
from T_FundWiseUDAData F
left JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = F.Symbol_pk


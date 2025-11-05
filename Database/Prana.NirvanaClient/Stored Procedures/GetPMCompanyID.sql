
CREATE Procedure [dbo].[GetPMCompanyID] (
@companyID int
)
as

Select 
PMCompanyID 
from PM_Company Where NOMSCompanyID=@companyID

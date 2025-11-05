
Create Proc GetPMModulePermission
(
@CompanyID int
)
as
Select
Read_WriteID 
from T_CompanyModule Where ModuleID=8  and CompanyID=@CompanyID

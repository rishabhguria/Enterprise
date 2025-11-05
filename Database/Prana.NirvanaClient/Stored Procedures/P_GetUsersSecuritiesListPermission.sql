CREATE PROCEDURE [dbo].[P_GetUsersSecuritiesListPermission]
(
@companyUserID int 
)
AS
Select Read_WriteID from T_UsersSecuritiesListPermission  WHERE CompanyUserID = @companyUserID


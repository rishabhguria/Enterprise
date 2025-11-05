CREATE PROCEDURE [dbo].[P_GetBreakOrderPreferenceFromDB]
(
	@companyID int 
)

AS

select IsBreakOrderEnabled from T_BreakOrderBasedOnPreference  WHERE CompanyID = @companyID



CREATE PROCEDURE dbo.P_GetAllUsersbyRMAdmin
(
	@companyID int	
)
AS
	SELECT   T_RMCompanyUsersOverall.CompanyUserID,T_CompanyUser.LastName, T_CompanyUser.FirstName, T_CompanyUser.ShortName,T_CompanyUser.Title,
				 T_CompanyUser.EMail,  T_CompanyUser.TelphoneWork,  T_CompanyUser.TelphoneHome,  T_CompanyUser.TelphoneMobile, T_CompanyUser.Fax,
				 T_CompanyUser.Login, T_CompanyUser.Password, T_CompanyUser.TelphonePager,  T_CompanyUser.Address1,  T_CompanyUser.Address2, 
				 T_CompanyUser.CountryID,T_CompanyUser.StateID, T_CompanyUser.Zip, T_RMCompanyUsersOverall.CompanyID,T_CompanyUser.TradingPermission
	                
	                     
	                     
	FROM         T_CompanyUser INNER JOIN
	                      T_RMCompanyUsersOverall ON T_CompanyUser.UserID = T_RMCompanyUsersOverall.CompanyUserID
	WHERE     (T_RMCompanyUsersOverall.CompanyID = @companyID) 


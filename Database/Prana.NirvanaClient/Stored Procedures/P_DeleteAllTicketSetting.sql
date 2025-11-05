



CREATE   procedure [dbo].[P_DeleteAllTicketSetting]
(
		@companyUserID int
)
as
delete from T_TicketSettings
where CompanyUserID = @companyUserID





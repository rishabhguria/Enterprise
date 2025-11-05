

CREATE procedure [dbo].[P_GetTradingTicketPrefrencesPopUps]
(
		@companyUserID int
		
	)

as
select * from T_ConfirmationPopUp
where CompanyUserID = @companyUserID




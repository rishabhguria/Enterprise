
/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientTraderByID    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientTraderByID
(
	@companyClientTraderID int	
)
AS

/*Declare @Total int
Set @Total = 0
Select @Total = Count(*) FROM T_CompanyClientTradingAccount Where ClientTraderID = @companyClientTraderID
if(@Total = 0)
begin */
	Delete T_CompanyClientTrader
	Where TraderID = @companyClientTraderID
/*end*/
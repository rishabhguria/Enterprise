
CREATE Procedure [dbo].[P_GetTransferTradeRules]
(
@companyID int 
)
as
Select * from T_TransferTradeRules  WHERE CompanyID = @companyID

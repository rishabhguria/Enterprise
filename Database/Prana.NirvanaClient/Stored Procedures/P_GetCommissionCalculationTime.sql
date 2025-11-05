
Create Procedure [dbo].[P_GetCommissionCalculationTime] as
Select IsPostAllocatedCalculation from T_CommissionCalculationTime


/****** Object:  Stored Procedure dbo.P_GetOperater  Script Date: 2/21/2006 2:30:21 PM ******/  
 CREATE PROCEDURE [dbo].[P_GetOperater] AS  
 Select OperatorID,Name  
 From T_Operator  
 Order By Name Asc

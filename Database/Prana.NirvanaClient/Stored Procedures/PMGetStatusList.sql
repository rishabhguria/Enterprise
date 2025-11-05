



/****** Object:  Stored Procedure dbo.P_GetCountries    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE [dbo].[PMGetStatusList]
AS
	Select StatusID, Name
	From PM_DataSourceStatusRef
	Order By Name Asc





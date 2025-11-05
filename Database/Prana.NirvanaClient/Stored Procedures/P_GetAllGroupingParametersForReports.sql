
/****** Object:  Stored Procedure dbo.P_GetAllGroupingParametersForReports    Script Date: 08/28/2008 8:25:22 PM ******/
CREATE PROCEDURE [dbo].[P_GetAllGroupingParametersForReports]
	
AS
	
	select * from T_ReportGroupParameters
--	select 'Select' AS [-Select-], 
--			'FundName' AS [FundName], 
--			'Symbol' AS [Symbol], 
--			'Position_Type' AS [Side], 
--			'MasterFund' AS [MasterFund]

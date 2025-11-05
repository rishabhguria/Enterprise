
/****** Object:  Stored Procedure dbo.P_GetGroupingParametersForReports    Script Date: 08/28/2008 8:25:22 PM ******/
CREATE PROCEDURE [dbo].[P_GetGroupingParametersForReports]
(
	@grpParam1 VARCHAR(50),
	@grpParam2 VARCHAR(50),
	@grpParam3 VARCHAR(50)
)
AS
	
	IF(@grpParam1 = 'Select')
		select * from T_ReportGroupParameters
	ELSE IF(@grpParam2 = 'Select')
		select * from T_ReportGroupParameters WHERE GroupParameter NOT IN(@grpParam1)
	ELSE IF(@grpParam3 = 'Select')
		select * from T_ReportGroupParameters WHERE GroupParameter NOT IN(@grpParam1, @grpParam2)
	ELSE
		select * from T_ReportGroupParameters WHERE GroupParameter NOT IN(@grpParam1, @grpParam2, @grpParam3)
	--AND (@grpParam1 <> 'Select' AND @grpParam2 <> 'Select' AND @grpParam3 <> 'Select')
--	select 'Select' AS [-Select-], 
--			'FundName' AS [FundName], 
--			'Symbol' AS [Symbol], 
--			'Position_Type' AS [Side], 
--			'MasterFund' AS [MasterFund]

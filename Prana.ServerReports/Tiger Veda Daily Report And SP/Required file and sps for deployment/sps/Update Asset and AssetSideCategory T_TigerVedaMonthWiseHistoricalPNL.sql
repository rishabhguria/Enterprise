------------------------------------------------------------------
--Update Asset on the basis of Investment Type of Historical Data
------------------------------------------------------------------
Update T_TigerVedaMonthWiseHistoricalPNL
Set Asset = 
CASE 

	WHEN InvestmentType = '<<Unassigned>>'
	THEN NULL
	WHEN InvestmentType = 'American Depository Receipt'
	THEN 'Equity'
	WHEN InvestmentType = 'Common Stock'
	THEN 'Equity'
	WHEN InvestmentType = 'Currency'
	THEN 'FX Hedge'
	WHEN InvestmentType = 'Currency Forward'
	THEN 'FX Hedge'
	WHEN InvestmentType = 'Equity Call Option'
	THEN 'EquityOption'
	WHEN InvestmentType = 'Equity Put Option'
	THEN 'EquityOption'
	WHEN InvestmentType = 'Equity Variance Swap'
	THEN 'Equity'
	WHEN InvestmentType = 'FF Equity SWAP (ES)'
	THEN 'Equity'
	WHEN InvestmentType = 'Fully Funded Equity SWAP'
	THEN 'Equity'
	WHEN InvestmentType = 'Mutual Fund'
	THEN 'Equity'
	WHEN InvestmentType = 'Private Placement - EQ'
	THEN 'PrivateEquity'
	WHEN InvestmentType = 'Real Estate'
	THEN 'Equity'
	ELSE 'Equity'

END 

-------------------------------------------------------------------------------------------
--Update AssetSideCategory on the basis of Investment Type and Direction of Historical Data
-------------------------------------------------------------------------------------------

Update T_TigerVedaMonthWiseHistoricalPNL
Set AssetSideCategory= 
CASE 
	WHEN Asset<> 'FX Hedge'
	THEN Asset+' '+Direction
	ELSE Asset
END
--------------------------------------------------------------------
--Remove NULL from PNL Fields to avoid NULL values while adding PNL
--------------------------------------------------------------------
Update T_TigerVedaMonthWiseHistoricalPNL
Set JanPNL =		ISNULL(JanPNL,0),
	JanPNLPercent=	ISNULL(JanPNLPercent,0),
	FebPNL=			ISNULL(FebPNL,0),
	FebPNLPercent=	ISNULL(FebPNLPercent,0),
	MarPNL=			ISNULL(MarPNL,0),
	MarPNLPercent=	ISNULL(MarPNLPercent,0),
	AprPNL=			ISNULL(AprPNL,0),
	AprPNLPercent=	ISNULL(AprPNLPercent,0),
	MayPNL=			ISNULL(MayPNL,0),
	MayPNLPercent=	ISNULL(MayPNLPercent,0),
	JunPNL=			ISNULL(JunPNL,0),
	JunPNLPercent=	ISNULL(JunPNLPercent,0),
	JulPNL=			ISNULL(JulPNL,0),
	JulPNLPercent=	ISNULL(JulPNLPercent,0),
	AugPNL=			ISNULL(AugPNL,0),
	AugPNLPercent=	ISNULL(AugPNLPercent,0)
---------------------------------------------------------------------
--Required Modification for Mapping Funds to Correspoding Broker Code
---------------------------------------------------------------------

Update T_TigerVedaMonthWiseHistoricalPNL
Set InvestmentCodeInFile=InvestmentCode,
	AccountInFile=Account

Update T_TigerVedaMonthWiseHistoricalPNL
Account=
CASE 

	WHEN Account = 'CS_DBX' THEN 'DBX - CSFB'
	WHEN Account = 'SWP_CS' THEN 'DBX - CSSW'
	WHEN Account = 'SWP_GS' THEN 'DBX - GSSW'
	WHEN Account = 'VB_DBCC' THEN 'VB - DBCC'
	WHEN Account = 'VB_GSCO' THEN 'VB - GSCO'
	WHEN Account = 'VB_GSSW' THEN 'VB - GSSW'
	WHEN Account = 'VB_MSSW' THEN 'VB - MSSW'
	WHEN Account = 'VB_SIDE' THEN 'VB - SIDE'
	WHEN Account = 'VM_AGENT' THEN 'VM - AGENT'
	WHEN Account = 'VM_CHASE' THEN 'VM - JPMCASH'
	WHEN Account = 'VM_CSFB' THEN 'VM - CSFB'
	WHEN Account = 'VM_CSSW' THEN 'VM - CSSW'
	WHEN Account = 'VM_GSCO' THEN 'VM - GSCO'
	WHEN Account = 'VM_GSSW' THEN 'VM - GSSW'
	WHEN Account = 'VM_JPM' THEN 'VM - JPMSW'
	WHEN Account = 'VM_MLSW' THEN 'VM - MLSW'
	WHEN Account = 'VM_MSSW' THEN 'VM - MSSW'
	WHEN Account = 'VM_NOMURA' THEN 'VM - NMSW'
	WHEN Account = 'VM_SIDE 1' THEN 'VM - SIDE 1'
	WHEN Account = 'VM_SIDE 2' THEN 'VM - SIDE 2'
	WHEN Account = 'VM_SIDE 3' THEN 'VM - SIDE 3'
	WHEN Account = 'VM_UBS' THEN 'VM - UBSSW'
	WHEN Account = 'VB_SIDE' THEN 'VB - SIDE'
	WHEN Account = 'VM_CAP' THEN ''
	ELSE NULL

END
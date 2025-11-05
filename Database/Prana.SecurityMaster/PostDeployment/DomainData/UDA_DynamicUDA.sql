
GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'RiskCurrency')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'RiskCurrency',    
	 @HeaderCaption = 'Risk Currency',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL
END
 
 GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'Issuer')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'Issuer',   
	 @HeaderCaption = 'Issuer',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL  
END

 GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CountryOfRisk')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'CountryOfRisk',   
	 @HeaderCaption = 'Country Of Risk',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL  
END

 GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'Region')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'Region',   
	 @HeaderCaption = 'Region',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL  
END

 GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'Analyst')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'Analyst',   
	 @HeaderCaption = 'Analyst',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL  
END

 GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'UCITSEligibleTag')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'UCITSEligibleTag',   
	 @HeaderCaption = 'UCITS Eligible Tag',  
	 @DefaultValue = 'Yes', 
	 @MasterValues='<MasterUDAValue><Value key= "0">Yes</Value><Value key="1">No</Value><Value key="2">Yes - Restricted</Value></MasterUDAValue>',
	 @RenamedKeys = NULL
END
 
GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'LiquidTag')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'LiquidTag',   
	 @HeaderCaption = 'Liquid Tag',  
	 @DefaultValue = 'Yes',  
	 @MasterValues='<MasterUDAValue><Value key="0">Yes</Value><Value key="1">No</Value></MasterUDAValue>',
	 @RenamedKeys = NULL                             
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'MarketCap')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'MarketCap',   
	@HeaderCaption	= 'Market Cap',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA1')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'CustomUDA1',   
	@HeaderCaption	= 'Custom UDA1',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA2')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'CustomUDA2',   
	@HeaderCaption	= 'Custom UDA2',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA3')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'CustomUDA3',   
	@HeaderCaption	= 'Custom UDA3',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA4')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'CustomUDA4',   
	@HeaderCaption	= 'Custom UDA4',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA5')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'CustomUDA5',   
	@HeaderCaption	= 'Custom UDA5',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA6')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'CustomUDA6',   
	@HeaderCaption	= 'Custom UDA6',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA7')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	@Tag			= 'CustomUDA7',   
	@HeaderCaption	= 'Custom UDA7',  
	@DefaultValue	= NULL,  
	@MasterValues	= NULL,
	@RenamedKeys = NULL                            
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA8')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'CustomUDA8',    
	 @HeaderCaption = 'Custom UDA8',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA9')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'CustomUDA9',    
	 @HeaderCaption = 'Custom UDA9',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA10')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'CustomUDA10',    
	 @HeaderCaption = 'Custom UDA10',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA11')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'CustomUDA11',    
	 @HeaderCaption = 'Custom UDA11',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL
END

GO
IF NOT EXISTS(SELECT 1 FROM T_UDA_DynamicUDA WHERE Tag = 'CustomUDA12')
BEGIN
	EXEC P_UDA_SaveDynamicUDA  
	 @Tag = 'CustomUDA12',    
	 @HeaderCaption = 'Custom UDA12',  
	 @DefaultValue = NULL,  
	 @MasterValues=NULL,
	 @RenamedKeys = NULL
END
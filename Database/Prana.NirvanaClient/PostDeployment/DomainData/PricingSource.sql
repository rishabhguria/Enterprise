SET IDENTITY_INSERT T_PricingSource ON;
truncate table T_PricingSource

	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(-2147483648,'None',			0);
	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(1,			'ESignal',		0);
	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(2,			'SAPI',			0);
	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(3,			'BPIPE',		0);
	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(4,			'Google',		0);
	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(5,			'Yahoo',		0);
	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(6,			'BloomBerg',	0);
	Insert Into T_PricingSource(SourceId, SourceName, PricingSourceType) Values(7,			'Prime Broker',	0);

SET IDENTITY_INSERT T_PricingSource OFF;
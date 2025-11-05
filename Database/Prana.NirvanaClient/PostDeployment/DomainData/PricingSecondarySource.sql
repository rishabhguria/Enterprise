SET IDENTITY_INSERT T_PricingSecondarySource ON;
truncate table T_PricingSecondarySource

	Insert Into T_PricingSecondarySource(SourceId, SourceName, Description) Values(1,	'CMPN',	'NY Composite');
	Insert Into T_PricingSecondarySource(SourceId, SourceName, Description) Values(2,	'CMPL',	'London Composite');
	Insert Into T_PricingSecondarySource(SourceId, SourceName, Description) Values(3,	'CMPT',	'Tokyo Composite');
	Insert Into T_PricingSecondarySource(SourceId, SourceName, Description) Values(4,	'BGN',	'Bloomberg');
	Insert Into T_PricingSecondarySource(SourceId, SourceName, Description) Values(5,	'BVAL',	'BVAL');
	

SET IDENTITY_INSERT T_PricingSecondarySource OFF;
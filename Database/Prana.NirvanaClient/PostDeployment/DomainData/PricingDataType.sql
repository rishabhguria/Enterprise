SET IDENTITY_INSERT T_PricingDataType ON;
truncate table T_PricingDataType

		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(0,'None');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(1,'Ask');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(2,'Bid');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(3,'Last');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(4,'MID');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(5,'IMID');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(6,'Open');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(7,'Close');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(8,'High');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(9,'Low');
		Insert Into T_PricingDataType(DataTypeID, DataTypeName) Values(10,'Avg_AskBid');

SET IDENTITY_INSERT T_PricingDataType OFF;
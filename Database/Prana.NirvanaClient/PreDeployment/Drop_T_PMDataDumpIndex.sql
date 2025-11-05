IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('schema.T_PMDataDump') AND NAME ='IDX_CreatedOn')
BEGIN
    DROP INDEX IDX_CreatedOn ON T_PMDataDump;
END

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('schema.T_PMDataDump') AND NAME ='IDX_Fund_Symbol')
BEGIN
    DROP INDEX IDX_Fund_Symbol ON T_PMDataDump;
END

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('schema.T_PMDataDump') AND NAME ='IDX_PricingSource')
BEGIN
    DROP INDEX IDX_PricingSource ON T_PMDataDump;
END
CREATE PROCEDURE P_SaveAUECMapping
(
@xml XML
)
AS
BEGIN
	DECLARE @handle INT
	EXEC sp_xml_preparedocument @handle OUTPUT, @xml
	CREATE TABLE #XmlAUECMapping
	(
		AUECID INT,        
		ExchangeIdentifier VARCHAR(MAX),        
		[Year] VARCHAR(20),       
		[Month] VARCHAR(20),      
		[Day] VARCHAR(20),     
		[Type] VARCHAR(20),    
		Strike VARCHAR(20),
		ExchangeToken VARCHAR(MAX),
		PSRootToken VARCHAR(MAX),
		PSFormatString VARCHAR(MAX),
		TranslateRoot BIT,
		TranslateType BIT,
		ExerciseStyle VARCHAR(MAX),
		EsignalFormatString VARCHAR(MAX),
		EsignalOptionFormatString VARCHAR(MAX),
		BloombergOptionFormatString VARCHAR(MAX),
		EsignalRootToken VARCHAR(MAX),
		BloombergRootToken VARCHAR(MAX),
		EsignalExchangeCode VARCHAR(20),
		FactSetExchangeCode VARCHAR(20),
		FactSetRegionCode VARCHAR(20),
		FactSetFormatString VARCHAR(MAX),
		ActivFormatString VARCHAR(MAX),
		BloombergCompositeCode VARCHAR(20),
		BloombergExchangeCode VARCHAR(20),
		BloombergFormatString VARCHAR(MAX)
	)

	INSERT INTO #XmlAUECMapping
	(
		AUECID,
		ExchangeIdentifier,
		[Year],
		[Month],
		[Day],
		[Type],
		Strike,
		ExchangeToken,
		PSRootToken,
		PSFormatString,
		TranslateRoot,
		TranslateType,
		ExerciseStyle,
		EsignalFormatString,
		EsignalOptionFormatString,
		BloombergOptionFormatString,
		EsignalRootToken,
		BloombergRootToken,
		EsignalExchangeCode,
		FactSetExchangeCode,
		FactSetRegionCode,
		FactSetFormatString,
		ActivFormatString,
		BloombergCompositeCode,
		BloombergExchangeCode,
		BloombergFormatString
	)
	SELECT
		AUECID,
		ExchangeIdentifier,
		[Year],
		[Month],
		[Day],
		[Type],
		Strike,
		ExchangeToken,
		PSRootToken,
		PSFormatString,
		TranslateRoot,
		TranslateType,
		ExerciseStyle,
		EsignalFormatString,
		EsignalOptionFormatString,
		BloombergOptionFormatString,
		EsignalRootToken,
		BloombergRootToken,
		EsignalExchangeCode,
		FactSetExchangeCode,
		FactSetRegionCode,
		FactSetFormatString,
		ActivFormatString,
		BloombergCompositeCode,
		BloombergExchangeCode,
		BloombergFormatString
	FROM
	OPENXML(@handle, '//Table1', 2)

	WITH
	(
		AUECID INT,        
		ExchangeIdentifier VARCHAR(MAX),        
		[Year] VARCHAR(20),       
		[Month] VARCHAR(20),      
		[Day] VARCHAR(20),     
		[Type] VARCHAR(20),    
		Strike VARCHAR(20),
		ExchangeToken VARCHAR(MAX),
		PSRootToken VARCHAR(MAX),
		PSFormatString VARCHAR(MAX),
		TranslateRoot bit,
		TranslateType bit,
		ExerciseStyle VARCHAR(MAX),		
		EsignalFormatString VARCHAR(MAX),
		EsignalOptionFormatString VARCHAR(MAX),
		BloombergOptionFormatString VARCHAR(MAX),
		EsignalRootToken VARCHAR(MAX),
		BloombergRootToken VARCHAR(MAX),
		EsignalExchangeCode VARCHAR(20),
		FactSetExchangeCode VARCHAR(20),
		FactSetRegionCode VARCHAR(20),
		FactSetFormatString VARCHAR(MAX),
		ActivFormatString VARCHAR(MAX),
		BloombergCompositeCode VARCHAR(20),
		BloombergExchangeCode VARCHAR(20),
		BloombergFormatString VARCHAR(MAX)
	)

	UPDATE map
	SET
	map.[Year] = xmlMap.[Year],
	map.[Month] = xmlMap.[Month],
	map.[Day] = xmlMap.[Day],
	map.[Type] = xmlMap.[Type],
	map.Strike = xmlMap.Strike,
	map.ExchangeToken = xmlMap.ExchangeToken,
	map.PSRootToken = xmlMap.PSRootToken,
	map.PSFormatString = xmlMap.PSFormatString,
	map.TranslateRoot = xmlMap.TranslateRoot,
	map.TranslateType = xmlMap.TranslateType,
	map.ExerciseStyle = xmlMap.ExerciseStyle,
	map.EsignalFormatString = xmlMap.EsignalFormatString,
	map.EsignalOptionFormatString = xmlMap.EsignalOptionFormatString,
	map.BloombergOptionFormatString = xmlMap.BloombergOptionFormatString,
	map.EsignalExchangeCode = xmlMap.EsignalExchangeCode,
	map.FactSetExchangeCode = xmlMap.FactSetExchangeCode,
	map.FactSetRegionCode = xmlMap.FactSetRegionCode,
	map.FactSetFormatString = xmlMap.FactSetFormatString,
	map.ActivFormatString = xmlMap.ActivFormatString,
	map.BloombergCompositeCode = xmlMap.BloombergCompositeCode,
	map.BloombergExchangeCode = xmlMap.BloombergExchangeCode,
	map.BloombergFormatString = xmlMap.BloombergFormatString
	FROM T_AUECMapping map
	INNER JOIN #XmlAUECMapping xmlMap ON map.AUECID = xmlMap.AUECID

	EXEC sp_xml_removedocument @handle
END


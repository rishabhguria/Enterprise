<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>



	<!--<xsl:template name="temp_MonthExpireCode">
		<xsl:param name="param_MonthExpireCode"/>
		<xsl:choose>
			<xsl:when test ="$param_MonthExpireCode='01'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='02'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='03'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='04'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='05'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='06'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='07'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='08'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='09'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='10'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='11'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$param_MonthExpireCode='12'">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="$param_MonthExpireCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>-->

	<!--<xsl:template name="tempCurrencyCode">
		<xsl:param name="paramCurrencySymbol"/>
		--><!-- 1 characters for metal code --><!--
		--><!--  e.g. A represents A = aluminium--><!--
		<xsl:choose>
			<xsl:when test ="$paramCurrencySymbol='USD'">
				<xsl:value-of select ="'1'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='HKD'">
				<xsl:value-of select ="'2'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='JPY'">
				<xsl:value-of select ="'3'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GBP'">
				<xsl:value-of select ="'4'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='AED'">
				<xsl:value-of select ="'5'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BRL'">
				<xsl:value-of select ="'6'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CAD'">
				<xsl:value-of select ="'7'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='EUR'">
				<xsl:value-of select ="'8'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='NOK'">
				<xsl:value-of select ="'9'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SGD'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MUL'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ZAR'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SEK'">
				<xsl:value-of select ="'13'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='AUD'">
				<xsl:value-of select ="'14'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CNY'">
				<xsl:value-of select ="'15'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='KRW'">
				<xsl:value-of select ="'16'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BDT'">
				<xsl:value-of select ="'17'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='THB'">
				<xsl:value-of select ="'18'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='dong'">
				<xsl:value-of select ="'19'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GBX'">
				<xsl:value-of select ="'20'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='INR'">
				<xsl:value-of select ="'21'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CHF'">
				<xsl:value-of select ="'23'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CLP'">
				<xsl:value-of select ="'24'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='COP'">
				<xsl:value-of select ="'25'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CZK'">
				<xsl:value-of select ="'26'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='DKK'">
				<xsl:value-of select ="'27'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GHS'">
				<xsl:value-of select ="'28'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='HUF'">
				<xsl:value-of select ="'29'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='IDR'">
				<xsl:value-of select ="'30'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ILS'">
				<xsl:value-of select ="'31'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ISK'">
				<xsl:value-of select ="'32'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='KZT'">
				<xsl:value-of select ="'33'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='LVL'">
				<xsl:value-of select ="'34'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MXN'">
				<xsl:value-of select ="'35'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='NZD'">
				<xsl:value-of select ="'36'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PEN'">
				<xsl:value-of select ="'37'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PLN'">
				<xsl:value-of select ="'38'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='RON'">
				<xsl:value-of select ="'40'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='RUB'">
				<xsl:value-of select ="'41'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SKK'">
				<xsl:value-of select ="'42'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='TRY'">
				<xsl:value-of select ="'43'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ARS'">
				<xsl:value-of select ="'44'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='UYU'">
				<xsl:value-of select ="'45'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='TWD'">
				<xsl:value-of select ="'46'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BMD'">
				<xsl:value-of select ="'47'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='EEK'">
				<xsl:value-of select ="'48'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GEL'">
				<xsl:value-of select ="'49'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MYR'">
				<xsl:value-of select ="'51'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SIT'">
				<xsl:value-of select ="'52'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='XAF'">
				<xsl:value-of select ="'53'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='XOF'">
				<xsl:value-of select ="'54'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='AZN'">
				<xsl:value-of select ="'55'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PKR'">
				<xsl:value-of select ="'56'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PHP'">
				<xsl:value-of select ="'57'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>-->

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">D:\NirvanaCode\SourceCode\Dev\Prana_CA\Application\Prana.Client\Prana\bin\Debug\MappingFiles\PranaXSD\OptionModelInputs.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 != 'Symbol'">
					<PositionMaster>
						<!--  Symbol Region -->
						<!--
						-->
						<!--<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>-->
						<!--

						<xsl:variable name="varException">
							<xsl:choose>
								<xsl:when test="contains(COL28,'XXXXXXX')!= false">
									<xsl:value-of select="'XXXXXXX'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varCOL35">
							<xsl:value-of select ="COL1"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) = 1 and number(substring-before(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' ')) and string-length(substring-before(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' ')) &lt; 5">
									<xsl:value-of select="'FOPT'"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) != 1 and number(substring-before(substring-after(normalize-space($varCOL35),' '),' ') and string-length(substring-before(substring-after(normalize-space($varCOL35),' '),' ')) &lt; 5)">
									<xsl:value-of select="'FOPT'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'FUT'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varAssetCode">
							<xsl:choose>
								<xsl:when test ="substring(substring-before(normalize-space($varCOL35),' '),string-length(substring-before(normalize-space($varCOL35),' ')) - 1,1) = 'C'">
									<xsl:value-of select ="'OP'"/>
								</xsl:when>
								<xsl:when test ="substring(substring-before(normalize-space($varCOL35),' '),string-length(substring-before(normalize-space($varCOL35),' ')) - 1,1) = 'P'">
									<xsl:value-of select ="'OP'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'EQ'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						-->
						<!--<xsl:variable name="varBBCode">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
									<xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(substring-before(normalize-space($varCOL35),' '),1,string-length(substring-before(normalize-space($varCOL35),' '))-2)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->
						<!--

						<xsl:variable name="varBBCode">
							<xsl:choose>
								<xsl:when test="$varAssetType='FUT'">
									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
											<xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(substring-before(normalize-space($varCOL35),' '),1,string-length(substring-before(normalize-space($varCOL35),' '))-2)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
											<xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring(substring-before(normalize-space($varCOL35),' '),1,string-length(substring-before(normalize-space($varCOL35),' '))-3)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						-->
						<!--<xsl:variable name ="varBBKey">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
									<xsl:value-of select="substring-after(substring-after(normalize-space($varCOL35),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(normalize-space($varCOL35),' ')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->
						<!--

						<xsl:variable name ="varBBKey">
							<xsl:choose>
								<xsl:when test="$varAssetType='FUT'">
									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
											<xsl:value-of select="translate(substring-after(substring-after(normalize-space($varCOL35),' '),' '),$varSmall,$varCapital)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="translate(substring-after(normalize-space($varCOL35),' '),$varSmall,$varCapital)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
											<xsl:value-of select="translate(substring-after(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' '),$varSmall,$varCapital)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="translate(substring-after(substring-after(normalize-space($varCOL35),' '),' '),$varSmall,$varCapital)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CODEDISPLAY>
							<xsl:value-of select ="concat($varBBCode,' Key ' , $varBBKey)"/>
						</CODEDISPLAY>

						-->
						<!--<Bloomberg>
							<xsl:value-of select="COL1"/>
							
						</Bloomberg>-->
						<!--
						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@UnderlyingCode"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@UnderlyingCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varStrikeMul">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@StrikeMul"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@StrikeMul"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExpFlag">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@ExpFlag"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExpFlag"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMulFlag">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@MulFlag"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@MulFlag"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExchangeCode">
							<xsl:choose>
								<xsl:when test="$varException != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@Exception=$varException]/@ExchangeCode"/>
								</xsl:when>
								<xsl:when test="$varBBCode != '' and $varBBKey != ''">
									<xsl:value-of select="document('../ReconMappingXml/JPM_UnderlyingMapping.xml')/UnderlyingMapping/Exchange[@Name='ALL']/SymbolData[@BBCode=$varBBCode and @BBKey = $varBBKey]/@ExchangeCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varBBSymbolBeforeKey">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' '))=1">
									<xsl:value-of select="substring-before(substring-after(normalize-space($varCOL35),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(normalize-space($varCOL35),' ')"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="varStrike">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) = 1">
									<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space($varCOL35),' '),' '),' ')"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(normalize-space($varCOL35),' ')) != 1">
									<xsl:value-of select="substring-before(substring-after(normalize-space($varCOL35),' '),' ')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varStrikeOPT">
							<xsl:choose>
								<xsl:when test="$varStrikeMul = ''">
									<xsl:value-of select="$varStrike"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varStrike*($varStrikeMul)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMonthExpireCode">
							<xsl:value-of select ="substring($varBBSymbolBeforeKey,((string-length($varBBSymbolBeforeKey) - 2) + 1),2)"/>
						</xsl:variable>

						<xsl:variable name="varMonthExpireCodeOPT">
							<xsl:value-of select ="substring($varBBSymbolBeforeKey,((string-length($varBBSymbolBeforeKey) - 3) + 1),3)"/>
						</xsl:variable> -->
						<Bloomberg>
							<xsl:value-of select="COL1"/>
						</Bloomberg>

						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>

						<!--<xsl:variable name="varVolatility">
							<xsl:choose>
								<xsl:when  test="boolean(number(COL8))">
									<xsl:value-of select="COL8"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose >
								
						</xsl:variable>-->

						<Volatility>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL8))">
									<xsl:value-of select="COL8"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</Volatility>
						
						<xsl:choose>
							<xsl:when test="boolean(number(COL8))">
								<VolatilityUsed>
									<xsl:value-of select="'1'"/>
								</VolatilityUsed>
							</xsl:when >
							<xsl:otherwise>
								<VolatilityUsed>
									<xsl:value-of select="'0'"/>
								</VolatilityUsed>
							</xsl:otherwise>
						</xsl:choose >
						

						<!--<xsl:choose>
							<xsl:when test="translate(COL7,$varSmall,$varCapital)  = 'FALSE' or boolean(number(COL8)) = 'FALSE'">
								<VolatilityUsed>
									<xsl:value-of select="'0'"/>
								</VolatilityUsed>
							</xsl:when >
							<xsl:otherwise>
								<VolatilityUsed>
									<xsl:value-of select="'1'"/>
								</VolatilityUsed>
							</xsl:otherwise>
						</xsl:choose >-->

						<!--<xsl:variable name="varIntRate">
							<xsl:choose>
								<xsl:when  test="boolean(number(COL11))">
									<xsl:value-of select="COL11"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose >
						</xsl:variable>-->
						
						<IntRate>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL11))">
									<xsl:value-of select="COL11"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</IntRate>

						<IntRateUsed>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL11))">
									<xsl:value-of select="'1'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose >
						</IntRateUsed>

						<!--<IntRateUsed>
							<xsl:choose>
								<xsl:when  test="translate(COL10,$varSmall,$varCapital)  = 'FALSE' or boolean(number(COL11)) = 'FALSE'">
									<xsl:value-of select="'0'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose >
						</IntRateUsed>-->

						<!--<xsl:variable name="varDividend">
							<xsl:choose>
								<xsl:when  test="boolean(number(COL14))">
									<xsl:value-of select="COL14"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose >
						</xsl:variable>-->

						<Dividend>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL14))">
									<xsl:value-of select="COL14"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</Dividend>

						<DividendUsed>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL14))">
									<xsl:value-of select="'1'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose >
						</DividendUsed>

						<!--<DividendUsed>
							<xsl:choose>
								<xsl:when  test="translate(COL13,$varSmall,$varCapital)  = 'FALSE' or boolean(number(COL14))= 'FALSE'">
									<xsl:value-of select="'0'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose >
						</DividendUsed>-->

						<!--<xsl:variable name="varDelta">
							<xsl:choose>
								<xsl:when  test="boolean(number(COL17))">
									<xsl:value-of select="COL17"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose >
						</xsl:variable>-->

						<Delta>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL17))">
									<xsl:value-of select="COL17"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</Delta>
						
						<DeltaUsed>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL17))">
									<xsl:value-of select="'1'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose >
						</DeltaUsed>

						<!--<DeltaUsed>
							<xsl:choose>
								<xsl:when  test="translate(COL16,$varSmall,$varCapital)  = 'FALSE' or boolean(number(COL17)) ='FALSE' ">
									<xsl:value-of select="'0'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose >
						</DeltaUsed>-->

						<!--<xsl:variable name="varLastPrice">
							<xsl:choose>
								<xsl:when  test="boolean(number(COL2))">
									<xsl:value-of select="COL2"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose >
						</xsl:variable>-->
						
						<LastPrice>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL2))">
									<xsl:value-of select="COL2"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</LastPrice>
						
						<LastPriceUsed>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL2))">
									<xsl:value-of select="1"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</LastPriceUsed>

						<!--<LastPriceUsed>
							<xsl:choose>
								<xsl:when  test="translate(COL19,$varSmall,$varCapital)  = 'FALSE' or boolean(number(COL2)) = 'FALSE' ">
									<xsl:value-of select="'0'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose >
						</LastPriceUsed>-->

						<!--<xsl:variable name="varForwardPoints">
							<xsl:choose>
								<xsl:when  test="boolean(number(COL21))">
									<xsl:value-of select="COL21"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="-2147483648"/>
								</xsl:otherwise>
							</xsl:choose >
						</xsl:variable>-->
						
						<ForwardPoints>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL21))">
									<xsl:value-of select="COL21"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</ForwardPoints>

						<ForwardPointsUsed>
							<xsl:choose>
								<xsl:when  test=" boolean(number(COL21))">
									<xsl:value-of select="'1'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose >
						
						</ForwardPointsUsed>
						

						<!--<ForwardPointsUsed>
							<xsl:choose>
								<xsl:when  test="translate(COL20,$varSmall,$varCapital)  = 'FALSE' or boolean(number(COL21)) = 'FALSE'">
									<xsl:value-of select="'0'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose >

						</ForwardPointsUsed>-->
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>

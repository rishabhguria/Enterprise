<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>



	<xsl:template name="temp_MonthExpireCode">
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
	</xsl:template>

	<xsl:template name="tempCurrencyCode">
		<xsl:param name="paramCurrencySymbol"/>
		<!-- 1 characters for metal code -->
		<!--  e.g. A represents A = aluminium-->
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
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth='1' or $varMonth='01'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='2' or $varMonth='02'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='3' or $varMonth='03'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='4' or $varMonth='04'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = '5' or $varMonth='05'">
				<xsl:value-of select = "'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='6' or $varMonth='06'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='7' or $varMonth='07'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='8' or $varMonth='08'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='9' or $varMonth='09'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' ">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12'">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">D:\NirvanaCode\SourceCode\Dev\Prana_CA\Application\Prana.Client\Prana\bin\Debug\MappingFiles\PranaXSD\OptionModelInputs.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 != 'Symbol'">
					<PositionMaster>
						<!--  Symbol Region -->
						<!--<Symbol>
							<xsl:value-of select="COL1"/>
						</Symbol>-->

						<CODEDISPLAY>
							<xsl:value-of select ="''"/>
						</CODEDISPLAY>
						<xsl:variable name="varContractCode">
							<xsl:choose>
								<xsl:when test="COL15 = 89">
									<xsl:value-of select="'PL'"/>
								</xsl:when>
								<xsl:when test="COL15 = 90">
									<xsl:value-of select="'PA'"/>
								</xsl:when>
								<xsl:when test="COL15 = 37">
									<xsl:value-of select="'GC'"/>
								</xsl:when>
								<xsl:when test="COL15 = 39">
									<xsl:value-of select="'SI'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL15"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varCUSIP">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varRIC">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varBloomberg">
							<xsl:value-of select="COL39"/>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varOSISymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varEquitySymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>


						<xsl:variable name="varExchange">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<xsl:variable name="varExYear">
							<xsl:value-of select="COL43"/>
						</xsl:variable>

						<xsl:variable name="varExMonth">
							<xsl:value-of select="COL32"/>
						</xsl:variable>

						<xsl:variable name="varImportDate">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<xsl:variable name="varOptionExpiry">
							<xsl:value-of select="COL23"/>
						</xsl:variable>

						<xsl:variable name="varCurrency">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varPBSymbol">
							<xsl:value-of select="COL39"/>
						</xsl:variable>

						<xsl:variable name="varDescription">
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<xsl:variable name="varMarkPrice">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<xsl:variable name="varFXConversionMethodOperator">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varFXRate">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varPutCall">
							<xsl:value-of select="COL36"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:value-of select="COL29"/>
						</xsl:variable>

						<xsl:variable name="varCommission">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varSecFees">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varMiscFee">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varBBTicker">
							<xsl:value-of select="COL38"/>
						</xsl:variable>

						<xsl:variable name="varInitial">
							<xsl:choose>
								<xsl:when test="COL15 = 'CP'">
									<xsl:value-of select="'CAD'"/>
								</xsl:when>
								<xsl:when test="COL15 = 'LD'">
									<xsl:value-of select="'PBD'"/>
								</xsl:when>
								<xsl:when test="COL15 = 'BN'">
									<xsl:value-of select="'NID'"/>
								</xsl:when>
								<xsl:when test="COL15 = 'L8'">
									<xsl:value-of select="'ZSD'"/>
								</xsl:when>
								<xsl:when test="COL15 = 'AU'">
									<xsl:value-of select="'AHD'"/>
								</xsl:when>
								<xsl:when test="COL15 = 'L7'">
									<xsl:value-of select="'SND'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<!--<xsl:variable name="PB_Currency_Name">
              <xsl:value-of select="COL14"/>
            </xsl:variable>
            <xsl:variable name="PB_Suffix">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name = $varPBName]/SymbolData[@TickerSuffixCode = $PB_Currency_Name]/@PBSuffixCode"/>
            </xsl:variable>-->


						<xsl:variable name="varStrike">
							<xsl:value-of select="format-number(COL38,'#')"/>
						</xsl:variable>

						<xsl:variable name="varExpiryMonth">
							<xsl:value-of select="substring-before(COL4,'/')"/>
						</xsl:variable>

						<xsl:variable name="varMonthCode">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varExMonth"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varExpiryYear">
							<xsl:value-of select="substring(substring-after(substring-after(COL4,'/'),'/'),4,1)"/>
						</xsl:variable>

						<xsl:variable name="varExpiryDay">
							<xsl:value-of select="substring-before(substring-after(COL4,'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varFutExDay">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL9),'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varFutExDayMod">
							<xsl:choose>
								<xsl:when test="string-length($varFutExDay) = 1">
									<xsl:value-of select="concat('0',$varFutExDay)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varFutExDay"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varOptExDay">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL34),'/'),'/')"/>
						</xsl:variable>

						<xsl:variable name="varOptExDayMod">
							<xsl:choose>
								<xsl:when test="string-length($varOptExDay) = 1">
									<xsl:value-of select="concat('0',$varOptExDay)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varOptExDay"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<PBSymbol>
							<xsl:value-of  select="translate($varBloomberg,$varSmall, $varCapital)"/>
						</PBSymbol>


						<Volatility>
							<xsl:value-of select="COL2"/>
						</Volatility>



						<VolatilityUsed>
							<xsl:value-of select="'1'"/>
						</VolatilityUsed>


						<IntRateUsed>
							<xsl:value-of select="'0'"/>
						</IntRateUsed>

						<DividendUsed>
							<xsl:value-of select="'0'"/>
						</DividendUsed>

						<DeltaUsed>
							<xsl:value-of select="'0'"/>
						</DeltaUsed>

						<LastPriceUsed>
							<xsl:value-of select="'1'"/>
						</LastPriceUsed>

						<IntRate>
							<xsl:value-of select="0"/>
						</IntRate>

						<Dividend>
							<xsl:value-of select="0"/>
						</Dividend>

						<Delta>
							<xsl:value-of select="0"/>
						</Delta>

						<Symbol>
							
									<xsl:value-of select="COL1"/>
								
						</Symbol>

						<LastPrice>
							<xsl:choose>
								<xsl:when  test="boolean(number(COL7))">
									<xsl:value-of select="COL7"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</LastPrice>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>


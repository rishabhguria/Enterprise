<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>
	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:if test="normalize-space(COL4) !=''">

					<PositionMaster>


						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="normalize-space(COL4) != '' or normalize-space(COL4) != '*'">
									<xsl:value-of select="normalize-space(COL4)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varISINSymbol">
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>
						
						<ISINSymbol>
							<xsl:choose>
								<xsl:when test="$varISINSymbol!=''">
									<xsl:value-of select="$varISINSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ISINSymbol>
						
						<xsl:variable name="varCusipSymbol">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>

						<CusipSymbol>
							<xsl:choose>
								<xsl:when test="$varCusipSymbol!=''">
									<xsl:value-of select="$varCusipSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CusipSymbol>

						<TickerSymbol>							
							<xsl:value-of select="$varSymbol"/>								
						</TickerSymbol>						

						<Multiplier>
							<xsl:value-of select="'1'"/>
						</Multiplier>

						<UnderLyingSymbol>
							<xsl:value-of select="normalize-space(COL2)"/>
						</UnderLyingSymbol>
						
						<xsl:variable name="varBloombergSymbol">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>

						<BloombergSymbol>
							<xsl:choose>
								<xsl:when test="$varBloombergSymbol!=''">
									<xsl:value-of select="$varBloombergSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</BloombergSymbol>

						<xsl:variable name="varSedol">
							<xsl:value-of select="normalize-space(COL8)"/>
						</xsl:variable>

						<SedolSymbol>
							<xsl:choose>
								<xsl:when test="$varSedol!=''">
									<xsl:value-of select="$varSedol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SedolSymbol>

						<xsl:variable name="PB_CURRENCY_NAME">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_CURRENCY_NAME">
							<xsl:value-of select="document('../ReconMappingXML/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
						</xsl:variable>

						<CurrencyID>
							<xsl:choose>
							<xsl:when test ="$PRANA_CURRENCY_NAME !=''">
								<xsl:value-of select="$PRANA_CURRENCY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						</CurrencyID>
						
						<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXML/ExchangeMapping.xml')/ExchnageMapping/PB[@Name=$PB_NAME]/ExchangeData[@ExchangeName=$PB_EXCHANGE_NAME]/@PranaExchangeCode"/>
						</xsl:variable>

						<ExchangeID>
							<xsl:choose>
							<xsl:when test ="$PRANA_EXCHANGE_NAME !=''">
								<xsl:value-of select="$PRANA_EXCHANGE_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						</ExchangeID>
						
						<xsl:variable name="PRANA_Underlying_NAME">
							<xsl:value-of select="document('../ReconMappingXML/UnderlyingMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/UnderlyingData[@ExchangeName=$PB_EXCHANGE_NAME]/@UnderlyingID"/>
						</xsl:variable>

						<UnderLyingID>
							<xsl:choose>
							<xsl:when test ="$PRANA_Underlying_NAME !=''">
								<xsl:value-of select="$PRANA_Underlying_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
						</UnderLyingID>
						
						<AssetID>
							<xsl:value-of select="1"/>
						</AssetID>					
							
						<UDASector>
						   <xsl:value-of select="'Undefined'"/>
						 </UDASector>

						 <UDASubSector>
								<xsl:value-of select="'Undefined'"/>
						 </UDASubSector>

						 <UDASecurityType>
						   <xsl:value-of select="'Undefined'"/>
						 </UDASecurityType>

						 <UDAAssetClass>
						   <xsl:value-of select="'Undefined'"/>
						 </UDAAssetClass>

						 <UDACountry>
						   <xsl:value-of select="'Undefined'"/>
						 </UDACountry>
						
						<LongName>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</LongName>
						
						<!--<AUECID>
							<xsl:value-of select="43"/>
						</AUECID>-->
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>

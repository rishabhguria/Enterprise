<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">

		<DocumentElement>


			<xsl:for-each select ="//Comparision">

				<xsl:variable name ="NetPosition">
					<xsl:value-of select ="number(COL5)"/>
				</xsl:variable>	

				<xsl:if test ="number($NetPosition)">

					<Comparision>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GreenOwl'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL17"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL28='P' or COL28='C'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="COL16"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="COL28='P' or COL28='C'">
									<xsl:value-of select="concat(COL30,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<TradeDate>
							<xsl:value-of select="COL3"/>
						</TradeDate>

						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(COL14)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="varFX">
							<xsl:value-of select="number(COL7) div number(COL15)"/>
						</xsl:variable>

						<ForexPrice>
							<xsl:choose>
								<xsl:when test ="number($varFX)">
									<xsl:value-of select="$varFX"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</ForexPrice>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>					

						<Quantity>

							<xsl:choose>

								<xsl:when test ="number($NetPosition)">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>							

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>														

						<PutOrCall>
							<xsl:choose>
								<xsl:when test="COL28='P'">
									<xsl:value-of select="'Put'"/>
								</xsl:when>
								<xsl:when test="COL28='C'">
									<xsl:value-of select="'Call'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>

						<SEDOL>
							<xsl:value-of select="COL20"/>
						</SEDOL>

						<ISIN>
							<xsl:value-of select="COL19"/>
						</ISIN>

						<CUSIP>
							<xsl:value-of select="COL18"/>
						</CUSIP>

						<Bloomberg>
							<xsl:value-of select="COL21"/>
						</Bloomberg>

						<CurrencySymbol>
							<xsl:value-of select="COL13"/>
						</CurrencySymbol>						

						<OSI>
							<xsl:value-of select="COL30"/>
						</OSI>

						<xsl:variable name ="MarketValueBase">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>

								<xsl:when test ="$MarketValueBase &lt;0">
									<xsl:value-of select ="$MarketValueBase*-1"/>
								</xsl:when>

								<xsl:when test ="$MarketValueBase &gt;0">
									<xsl:value-of select ="$MarketValueBase"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValueBase>

						<xsl:variable name ="MarketValue">
							<xsl:value-of select="COL15"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>

								<xsl:when test ="$MarketValue &lt;0">
									<xsl:value-of select ="$MarketValue*-1"/>
								</xsl:when>

								<xsl:when test ="$MarketValue &gt;0">
									<xsl:value-of select ="$MarketValue"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>


					</Comparision>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>


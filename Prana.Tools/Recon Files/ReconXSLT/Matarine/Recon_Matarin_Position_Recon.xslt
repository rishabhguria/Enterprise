<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position" select="COL4"/>

				<xsl:if test="number($Position) and contains(COL1,'Cash')!='true'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'SS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select="normalize-space(COL3)"/>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test ="$Symbol!='*'">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="COL9"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<Side>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<Quantity>
							<xsl:value-of select="$Position"/>
						</Quantity>

						<xsl:variable name="Cost" select="number(COL8)"/>

						<AvgPX>
							<xsl:choose>

								<xsl:when test="$Cost &gt; 0">
									<xsl:value-of select="$Cost"/>
								</xsl:when>

								<xsl:when test="$Cost &lt; 0">
									<xsl:value-of select="$Cost * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

						<xsl:variable name="MarkPrice" select="number(COL6)"/>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>

								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<TradeDate>
							<xsl:value-of select="COL2"/>
						</TradeDate>

						<CurrencySymbol>
							<xsl:value-of select="COL7"/>
						</CurrencySymbol>

						<xsl:variable name="MarketValue" select="number(COL10)"/>

						<MarketValue>
							<xsl:choose>

								<xsl:when test="$MarketValue &gt; 0">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>

								<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>

						<xsl:variable name="NetNotionalValue" select="number(COL11)"/>

						<NetNotionalValue>
							<xsl:choose>

								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>

								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetNotionalValue>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
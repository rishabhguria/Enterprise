<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="number($Position)">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="PB_FUND_NAME" select="''"/>

							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>

							<FundName>
								<xsl:choose>

									<xsl:when test ="$PRANA_FUND_NAME!=''">
										<xsl:value-of select ="$PRANA_FUND_NAME"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="$PB_FUND_NAME"/>
									</xsl:otherwise>

								</xsl:choose>
							</FundName>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="COL4"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="Symbol">
								<xsl:value-of select="normalize-space(COL4)"/>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="$Symbol!=''">
										<xsl:value-of select="$Symbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							<Quantity>
								<xsl:choose>
									<xsl:when test="number($Position)">
										<xsl:value-of select="$Position"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</Quantity>

							<Side>
								<xsl:choose>
									<xsl:when test="$Position &gt; 0">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Position &lt; 0">
										<xsl:value-of select="'Sell short'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>


							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>


							<xsl:variable name="varOriginalDate">
								<xsl:value-of select="COL6"/>
							</xsl:variable>

							<OriginalPurchaseDate>
								<xsl:value-of select="$varOriginalDate"/>
							</OriginalPurchaseDate>

							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL9"/>
								</xsl:call-template>
							</xsl:variable>

							<MarkPrice>
								<xsl:choose>
									<xsl:when test="$varMarkPrice &gt; 0">
										<xsl:value-of select="$varMarkPrice"/>
									</xsl:when>
									<xsl:when test="$varMarkPrice &lt; 0">
										<xsl:value-of select="$varMarkPrice * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarkPrice>

							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
								</xsl:call-template>
							</xsl:variable>
							<AvgPX>
								<xsl:choose>
									<xsl:when test="$AvgPrice &gt; 0">
										<xsl:value-of select="$AvgPrice"/>
									</xsl:when>
									<xsl:when test="$AvgPrice &lt; 0">
										<xsl:value-of select="$AvgPrice * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</AvgPX>

							<xsl:variable name="varUnitCost">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL8"/>
								</xsl:call-template>
							</xsl:variable>

							<UnitCost>
								<xsl:choose>
									<xsl:when test="$varUnitCost &gt; 0">
										<xsl:value-of select="$varUnitCost"/>
									</xsl:when>

									<xsl:when test="$varUnitCost &lt; 0">
										<xsl:value-of select="$varUnitCost * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnitCost>


							<xsl:variable name="NetNotional">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
								</xsl:call-template>
							</xsl:variable>

							<NetNotionalValue>
								<xsl:choose>
									<xsl:when test="number($NetNotional)">
										<xsl:value-of select="$NetNotional"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValue>

							<xsl:variable name="varNetNotionalBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL10"/>
								</xsl:call-template>
							</xsl:variable>
							<NetNotionalValueBase>
								<xsl:choose>
									<xsl:when test="number($varNetNotionalBase)">
										<xsl:value-of select="$varNetNotionalBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValueBase>

							<xsl:variable name="varMarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
								</xsl:call-template>
							</xsl:variable>



							<xsl:variable name="varMarketValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL11"/>
								</xsl:call-template>
							</xsl:variable>

							<MarketValue>
								<xsl:choose>
									<xsl:when test="$varMarketValue &gt; 0">
										<xsl:value-of select="$varMarketValue"/>
									</xsl:when>
									<xsl:when test="$varMarketValue &lt; 0">
										<xsl:value-of select="$varMarketValue * (1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>

							<MarketValueBase>
								<xsl:choose>
									<xsl:when test="$varMarketValueBase &gt; 0">
										<xsl:value-of select="$varMarketValueBase"/>
									</xsl:when>
									<xsl:when test="$varMarketValueBase &lt; 0">
										<xsl:value-of select="$varMarketValueBase * (1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>


							<PBSymbol>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</CompanyName>

						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>

							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>

							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>


							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>


							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<OriginalPurchaseDate>
								<xsl:value-of select ="''"/>
							</OriginalPurchaseDate>

							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



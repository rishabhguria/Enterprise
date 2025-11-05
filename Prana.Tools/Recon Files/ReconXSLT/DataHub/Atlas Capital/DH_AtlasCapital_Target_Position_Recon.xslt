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

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL13"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="COL12"/>
							</xsl:variable>

							<xsl:variable name="varSEDOL">
								<xsl:value-of select="COL7"/>								
							</xsl:variable>
							
							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="$varSEDOL = '' or $varSEDOL = '*'">
										<xsl:value-of select="COL6"/>
									</xsl:when>
									<xsl:when test="$varSEDOL != '' or $varSEDOL != '*'">
										<xsl:value-of select="''"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							<SEDOL>
								<xsl:choose>								
									<xsl:when test="$varSEDOL != '' or $varSEDOL != '*'">
										<xsl:value-of select="$varSEDOL"/>
									</xsl:when>								
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEDOL>

							<xsl:variable name="PB_FUND_NAME" select="COL1"/>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name='State Street']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>
							<Quantity>
								<xsl:choose>
									<xsl:when test="number($Quantity)">
										<xsl:value-of select="$Quantity"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>

							<xsl:variable name="varMarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
								</xsl:call-template>
							</xsl:variable>
							<MarketValue>
								<xsl:choose>
									<xsl:when test="$varMarketValue &gt; 0">
										<xsl:value-of select="$varMarketValue"/>
									</xsl:when>
									<xsl:when test="$varMarketValue &lt; 0">
										<xsl:value-of select="$varMarketValue * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>

							<xsl:variable name="varMarketValuebase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL17"/>
								</xsl:call-template>
							</xsl:variable>
							<MarketValueBase>
								<xsl:choose>
									<xsl:when test="$varMarketValuebase &gt; 0">
										<xsl:value-of select="$varMarketValuebase"/>
									</xsl:when>
									<xsl:when test="$varMarketValuebase &lt; 0">
										<xsl:value-of select="$varMarketValuebase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="0"/>
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
							<xsl:variable name="varMarkPriceB">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL14"/>
								</xsl:call-template>
							</xsl:variable>
							<MarkPriceBase>
								<xsl:choose>
									<xsl:when test="$varMarkPriceB &gt; 0">
										<xsl:value-of select="$varMarkPriceB"/>
									</xsl:when>
									<xsl:when test="$varMarkPriceB &lt; 0">
										<xsl:value-of select="$varMarkPriceB * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarkPriceBase>
							
				<xsl:variable name="NetNotionalValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL18"/>
				  </xsl:call-template>
			  </xsl:variable>
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

			  <xsl:variable name="NetNotionalValueBase">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="0"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test="$NetNotionalValueBase &gt; 0">
						  <xsl:value-of select="$NetNotionalValueBase"/>
					  </xsl:when>
					  <xsl:when test="$NetNotionalValueBase &lt; 0">
						  <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValueBase>

							<PBSymbol>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</CompanyName>

							<FXRate>
								<xsl:value-of select="COL16"/>
							</FXRate>
							
							<SMRequest>
								<xsl:value-of select="'True'"/>							
							</SMRequest>

						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<SEDOL>
								<xsl:value-of select="''"/>
							</SEDOL>
							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>
							
							<MarkPriceBase>
								<xsl:value-of select="0"/>
							</MarkPriceBase>
							
							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>
								
							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

							<FXRate>
								<xsl:value-of select="''"/>
							</FXRate>

							<SMRequest>
								<xsl:value-of select="''"/>							
							</SMRequest>
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>


			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



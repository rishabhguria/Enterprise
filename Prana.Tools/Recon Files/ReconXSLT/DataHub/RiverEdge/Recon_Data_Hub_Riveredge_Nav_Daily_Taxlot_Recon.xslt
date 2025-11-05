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
						<xsl:with-param name="Number" select="COL28"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'DataHub'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="normalize-space(COL5)"/>
							</xsl:variable>



							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="substring-after(COL7,' ')"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
							</xsl:variable>

							<xsl:variable name="varSymbol">
								<xsl:choose>
									<xsl:when test="contains(COL7,' ')">
										<xsl:value-of select="substring-before(COL7,' ')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL7"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<xsl:variable name="varSedol">
								<xsl:value-of select="COL10"/>
							</xsl:variable>
							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
									</xsl:when>
									<xsl:when test="$varSedol!=''">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>


							<SEDOL>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:when test="$varSedol!=''">
										<xsl:value-of select="$varSedol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEDOL>

							<xsl:variable name="PB_FUND_NAME" select="COL2"/>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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



							<xsl:variable name="varSide">
								<xsl:value-of select="COL23"/>
							</xsl:variable>
							<Side>

								<xsl:choose>
									<xsl:when test ="contains($varSide,'Long')">
										<xsl:value-of select ="'Sell'"/>
									</xsl:when>

									<xsl:when test ="contains($varSide,'Short')">
										<xsl:value-of select ="'Buy'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="0"/>
									</xsl:otherwise>

								</xsl:choose>
							</Side>
							<Quantity>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="$Quantity"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="$Quantity * -1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>



							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL31"/>
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


							<xsl:variable name="NetNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL33"/>
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
									<xsl:with-param name="Number" select="COL34"/>
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



							<xsl:variable name="varMarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL36"/>
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

							<xsl:variable name="varMarketValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL37"/>
								</xsl:call-template>
							</xsl:variable>
							<MarketValueBase>
								<xsl:choose>
									<xsl:when test="$varMarketValueBase &gt; 0">
										<xsl:value-of select="$varMarketValueBase"/>
									</xsl:when>
									<xsl:when test="$varMarketValueBase &lt; 0">
										<xsl:value-of select="$varMarketValueBase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>

							<TradeDate>
								<xsl:value-of select ="COL30"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>


							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="$varMarketValue div $Quantity"/>
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



							<xsl:variable name="UnitCost">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL31"/>
								</xsl:call-template>
							</xsl:variable>
							<UnitCost>
								<xsl:choose>
									<xsl:when test="$UnitCost &gt; 0">
										<xsl:value-of select="$UnitCost"/>
									</xsl:when>
									<xsl:when test="$UnitCost &lt; 0">
										<xsl:value-of select="$UnitCost * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnitCost>

							<xsl:variable name="varFXRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL33 div COL34"/>
								</xsl:call-template>
							</xsl:variable>
							<FXRate>
								<xsl:choose>
									<xsl:when test="$varFXRate &gt; 0">
										<xsl:value-of select="$varFXRate"/>
									</xsl:when>
									<xsl:when test="$varFXRate &lt; 0">
										<xsl:value-of select="$varFXRate * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</FXRate>



							<CompanyName>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</CompanyName>

							<SMRequest>
							<xsl:value-of select="'true'"/>
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


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>



							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>



						
							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>

							
							<FXRate>
								<xsl:value-of select="0"/>
							</FXRate>



							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

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



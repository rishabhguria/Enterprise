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
						<xsl:with-param name="Number" select="substring(COL1,32,20)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'SS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="varCusip" select="substring(COL1,300,12)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$varCusip!=''">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="$varCusip!=''">
									<xsl:value-of select="$varCusip"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</CUSIP>

						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,9,12)"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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



						<xsl:variable name="varSide">
							<xsl:value-of select="''"/>
						</xsl:variable>
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

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<!--<xsl:with-param name="Number" select="format-number(substring(COL1,52,16),'##.###')"/>-->
								<xsl:with-param name="Number" select="''"/>
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
								<xsl:with-param name="Number" select="substring(COL1,180,16)"/>
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
								<xsl:with-param name="Number" select="substring(COL1,624,16)"/>
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
								<xsl:with-param name="Number" select="''"/>
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
								<xsl:with-param name="Number" select="substring(COL1,586,22)"/>
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

						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(COL1,5,2)"/>
						</xsl:variable>

						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL1,7,2)"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL1,1,4)"/>
						</xsl:variable>
						<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>




						<SettlementDate>
							<xsl:value-of select ="''"/>
						</SettlementDate>



						<xsl:variable name="varMarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,467,15)"/>
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
								<xsl:with-param name="Number" select="substring(COL1,691,17)"/>
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

						<xsl:variable name="varFxRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring(COL1,329,27)"/>
							</xsl:call-template>
						</xsl:variable>
						<FXRate>
							<xsl:choose>
								<xsl:when test="$varFxRate &gt; 0">
									<xsl:value-of select="$varFxRate"/>
								</xsl:when>
								<xsl:when test="$varFxRate &lt; 0">
									<xsl:value-of select="$varFxRate * (-1)"/>
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
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



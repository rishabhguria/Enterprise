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



	<xsl:template name="Date">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='Jan'">
				<xsl:value-of select="1"/>
			</xsl:when>
			<xsl:when test="$Month='Feb'">
				<xsl:value-of select="2"/>
			</xsl:when>
			<xsl:when test="$Month='Mar'">
				<xsl:value-of select="3"/>
			</xsl:when>
			<xsl:when test="$Month='Apr'">
				<xsl:value-of select="4"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="5"/>
			</xsl:when>
			<xsl:when test="$Month='Jun'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:when test="$Month='Jul'">
				<xsl:value-of select="7"/>
			</xsl:when>
			<xsl:when test="$Month='Aug'">
				<xsl:value-of select="8"/>
			</xsl:when>
			<xsl:when test="$Month='Sep'">
				<xsl:value-of select="9"/>
			</xsl:when>
			<xsl:when test="$Month='Oct'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$Month='Nov'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$Month='Dec'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="FutureCode">
		<xsl:param name="FMonth"/>
		<xsl:choose>
			<xsl:when test="$FMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$FMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$FMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$FMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$FMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$FMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$FMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$FMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$FMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$FMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$FMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$FMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

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



	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>



	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL31"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and COL10='F'">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'Barclays'"/>
							</xsl:variable>

							<xsl:variable name = "PB_SYMBOL_NAME" >
								<xsl:value-of select ="COL37"/>
							</xsl:variable>


							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="''"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
							</xsl:variable>




							<xsl:variable name="PB_PBContract">
								<xsl:value-of select="COL36"/>
							</xsl:variable>


							<xsl:variable name="PRANA_UNDERLYING_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/UnderlyingMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_PBContract]/@UnderlyingCode"/>
							</xsl:variable>

							<xsl:variable name="varFutureSymbol">
								<xsl:value-of select="substring-before(substring-after(normalize-space(COL3),' '),' ')"/>
							</xsl:variable>

							<xsl:variable name="varFutureYear">
								<xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL37),' '),' '),2,1)"/>
							</xsl:variable>

							<xsl:variable name ="varFutureMonthCode">
								<xsl:call-template name="FutureCode">
									<xsl:with-param name="FMonth" select="substring-before(normalize-space(COL37),' ')"/>
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="PRANA_UNDERLYING">
								<xsl:choose>
									<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
										<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="$PB_PBContract"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varSymbol">
								<xsl:value-of select="concat($PRANA_UNDERLYING,'',$varFutureMonthCode,$varFutureYear)"/>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>


									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							

							<SEDOL>
								<xsl:value-of select="''"/>
							</SEDOL>

							<ISIN>
								<xsl:value-of select="''"/>
							</ISIN>


							<xsl:variable name="PB_FUND_NAME" select="COL5"/>
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

							<xsl:variable name="varSide">
								<xsl:value-of select="COL30"/>
							</xsl:variable>

							<Side>
								<xsl:choose>
									<xsl:when test="$varSide ='1'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>

									<xsl:when test="$varSide ='2'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>

								</xsl:choose>
							</Side>

							<CurrencySymbol>
								<xsl:value-of select="COL41"/>
							</CurrencySymbol>

							<Quantity>
								<xsl:choose>

									<xsl:when test="number($Quantity) and COL30='2'">
										<xsl:value-of select="$Quantity * (-1)"/>
									</xsl:when>
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
									<xsl:with-param name="Number" select="COL28"/>
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
									<xsl:with-param name="Number" select="COL15"/>
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
									<xsl:with-param name="Number" select="''"/>
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

							<xsl:variable name="MarketValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>

							<MarketValueBase>
								<xsl:choose>
									<xsl:when test="number($MarketValueBase)">
										<xsl:value-of select="$MarketValueBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>

							<xsl:variable name="MarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL48"/>
								</xsl:call-template>
							</xsl:variable>
							<MarketValue>
								<xsl:choose>
									<xsl:when test="number($MarketValue)">
										<xsl:value-of select="$MarketValue"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>


							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>



							<xsl:variable name="MarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL42"/>
								</xsl:call-template>
							</xsl:variable>
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


							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<MarkPriceBase>
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
							</MarkPriceBase>



							<xsl:variable name="UnitCost">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
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

							<xsl:variable name ="FXRate">
								<xsl:value-of select ="''"/>
							</xsl:variable>

							<FXRate>
								<xsl:choose>
									<xsl:when test ="$FXRate ">
										<xsl:value-of select ="$FXRate"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="0"/>
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


							<CUSIP>
								<xsl:value-of select="''"/>
							</CUSIP>

							<ISIN>
								<xsl:value-of select="''"/>
							</ISIN>

							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>

							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<FXRate>
								<xsl:value-of select="'0'"/>
							</FXRate>

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


							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>
							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<MarkPriceBase>
								<xsl:value-of select="0"/>
							</MarkPriceBase>
							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>


							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>


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

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



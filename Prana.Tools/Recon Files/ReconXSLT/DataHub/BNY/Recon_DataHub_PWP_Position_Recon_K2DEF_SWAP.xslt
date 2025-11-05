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
						<xsl:with-param name="Number" select="COL3"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity)and COL2!='Cash and Equivalents' and COL2!='Currency' ">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'BAML'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="COL5"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="varDate">
								<xsl:value-of select="normalize-space(substring-after(COL12,'-'))"/>
							</xsl:variable>


							<xsl:variable name = "varSymbol" >
								<xsl:choose>
									<xsl:when test="contains(COL4,'_')">
										<xsl:value-of select="translate(COL4,'_',' ')"/>
									</xsl:when>

									<xsl:when test="COL2 ='FX Forward'">
										<xsl:value-of select="concat(COL4,'  ',$varDate)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="COL4"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="substring-after(COL4,'_')"/>
							</xsl:variable>


							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PranaSuffixCode=$PB_SUFFIX_CODE]/@TickerSuffixCode"/>
							</xsl:variable>


							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="COL4!=''">
										<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>


							<Asset>
								<xsl:value-of select="COL2"/>
							</Asset>



							<xsl:variable name="PB_FUND_NAME" select="'K2DEF_BNY'"/>
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



							<xsl:variable name="Side">
								<xsl:value-of select="''"/>
							</xsl:variable>
							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select ="'Buy'"/>
									</xsl:when>

									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select ="'Sell short'"/>
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

							<xsl:variable name="MarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL7"/>
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



							<xsl:variable name="varMarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL9"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varFXFWDMarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL10"/>
								</xsl:call-template>
							</xsl:variable>

							<MarketValue>
								<xsl:choose>
									<xsl:when test="COL2='FX Forward'">
										<xsl:choose>
											<xsl:when test="$varFXFWDMarketValue &gt; 0">
												<xsl:value-of select="$varFXFWDMarketValue"/>
											</xsl:when>
											<xsl:when test="$varFXFWDMarketValue &lt; 0">
												<xsl:value-of select="$varFXFWDMarketValue "/>
											</xsl:when>

										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$varMarketValue &gt; 0">
												<xsl:value-of select="$varMarketValue"/>
											</xsl:when>
											<xsl:when test="$varMarketValue &lt; 0">
												<xsl:value-of select="$varMarketValue "/>
											</xsl:when>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>


							</MarketValue>


							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>


							<CompanyName>
								<xsl:value-of select="COL5"/>
							</CompanyName>

						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<Asset>
								<xsl:value-of select="''"/>
							</Asset>

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

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



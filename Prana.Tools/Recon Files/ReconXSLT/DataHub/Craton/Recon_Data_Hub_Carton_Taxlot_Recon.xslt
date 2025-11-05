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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="MonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=01">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=02">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=03">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=04">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=05">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=06">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=07">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=08">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=09">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL18,'/')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL9,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),2,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),4,2)"/>
			</xsl:variable>

			<!--<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),12,1)"/>
			</xsl:variable>-->

			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL9),' '),8),'##.##')"/>
			</xsl:variable>


			<!--<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
				-->
			<!--<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,1)"/>-->
			<!--
			</xsl:variable>-->

			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<!--<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>-->

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$ExpiryMonth,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>



	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and COL9!='VPWF8'">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'DataHub'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="normalize-space(COL4)"/>
							</xsl:variable>



							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="substring-after(COL3,'.')"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
							</xsl:variable>

							<xsl:variable name="varSymbol">
								<xsl:choose>
									<xsl:when test="contains(COL3,'.')">
										<xsl:value-of select="substring-before(COL3,'.')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL3"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							<xsl:variable name="PB_FUND_NAME" select="''"/>
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
								<xsl:value-of select="''"/>
							</xsl:variable>
							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="'Sell short'"/>
									</xsl:when>
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
									<xsl:with-param name="Number" select="COL9"/>
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
									<xsl:with-param name="Number" select="COL8"/>
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

							<TradeDate>
								<xsl:value-of select ="COL1"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>


							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
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



							<xsl:variable name="varNetprice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL8"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varFXRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="($Quantity * $AvgPrice) div $varNetprice"/>
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

							<!--<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>-->

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

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>



							<MarkPriceBase>
								<xsl:value-of select="0"/>
							</MarkPriceBase>

							<FXRate>
								<xsl:value-of select="0"/>
							</FXRate>



							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

							<!--<SMRequest>
								<xsl:value-of select="''"/>
							</SMRequest>-->

						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>


			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



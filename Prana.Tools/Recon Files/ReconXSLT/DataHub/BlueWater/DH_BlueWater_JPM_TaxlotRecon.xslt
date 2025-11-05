<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
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


	<xsl:template name="FormatDate">
		<xsl:param name="DateTime"/>
		<!--  converts date time double number to 18/12/2009  -->
		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019"/>
		</xsl:variable>
		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))"/>
		</xsl:variable>
		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
		</xsl:variable>
		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
		</xsl:variable>
		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
		</xsl:variable>
		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))"/>
		</xsl:variable>
		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
		</xsl:variable>
		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))"/>
		</xsl:variable>
		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
		</xsl:variable>
		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
		</xsl:variable>
		<xsl:variable name="varMonthUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nMonth) = 1">
					<xsl:value-of select="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="nDayUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nDay) = 1">
					<xsl:value-of select="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>

	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and COL2='Equity'">

						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'JP Morgan'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="normalize-space(COL7)"/>
							</xsl:variable>

							<xsl:variable name="varSEDOL">
								<xsl:value-of select="normalize-space(COL34)"/>
							</xsl:variable>

							<xsl:variable name="varCUSIP">
								<xsl:value-of select="normalize-space(COL28)"/>
							</xsl:variable>

							<xsl:variable name="varISIN">
								<xsl:value-of select="normalize-space(COL31)"/>
							</xsl:variable>

							<xsl:variable name="varSymbol">
								<xsl:value-of select="normalize-space(COL29)"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>

									<xsl:when test="$varSEDOL!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varCUSIP!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varISIN!=''">
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

									<xsl:when test="$varSEDOL!=''">
										<xsl:value-of select="$varSEDOL"/>
									</xsl:when>

									<xsl:when test="$varCUSIP!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varISIN!=''">
										<xsl:value-of select="''"/>
									</xsl:when>


									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SEDOL>

							<CUSIP>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varCUSIP!=''">
										<xsl:value-of select="$varCUSIP"/>
									</xsl:when>

									<xsl:when test="$varSEDOL!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varCUSIP!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varISIN!=''">
										<xsl:value-of select="''"/>
									</xsl:when>


									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</CUSIP>

							<ISIN>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>


									<xsl:when test="$varISIN!=''">
										<xsl:value-of select="$varISIN"/>
									</xsl:when>

									<xsl:when test="$varSEDOL!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varCUSIP!=''">
										<xsl:value-of select="''"/>
									</xsl:when>



									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ISIN>

							<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>
							<FundName>
								<xsl:choose>
									<xsl:when test="$PRANA_FUND_NAME!=''">
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$PB_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</FundName>

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

							<xsl:variable name="UnitCost">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>

							<UnitCost>
								<xsl:choose>
									<xsl:when test="number($UnitCost)">
										<xsl:value-of select="$UnitCost"/>
									</xsl:when>


									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>

								</xsl:choose>
							</UnitCost>

							<xsl:variable name="varAvgPX">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL13)"/>
								</xsl:call-template>
							</xsl:variable>
							<AvgPX>
								<xsl:choose>
									<xsl:when test="$varAvgPX &gt; 0">
										<xsl:value-of select="$varAvgPX"/>
									</xsl:when>

									<xsl:when test="$varAvgPX &lt; 0">
										<xsl:value-of select="$varAvgPX * -1"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</AvgPX>

							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL13)"/>
								</xsl:call-template>
							</xsl:variable>
							<MarkPrice>
								<xsl:choose>
									<xsl:when test="$varMarkPrice &gt; 0">
										<xsl:value-of select="$varMarkPrice"/>
									</xsl:when>

									<xsl:when test="$varMarkPrice &lt; 0">
										<xsl:value-of select="$varMarkPrice "/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarkPrice>

							<xsl:variable name="NetNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL17)"/>
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


							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="'Sell short'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>

							<CurrencySymbol>
								<xsl:value-of select="normalize-space(COL7)"/>
							</CurrencySymbol>

							<OriginalPurchaseDate>
								<xsl:value-of select="COL8"/>
							</OriginalPurchaseDate>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<PBSymbol>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</PBSymbol>

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

							<FundName>
								<xsl:value-of select ="''"/>
							</FundName>

							<CounterParty>
								<xsl:value-of select="''"/>
							</CounterParty>

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>



							<Quantity>
								<xsl:value-of select="''"/>
							</Quantity>

							<Side>
								<xsl:value-of select="''"/>
							</Side>

							<TradeDate>
								<xsl:value-of select="' '"/>
							</TradeDate>

							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>

							<TotalCost>
								<xsl:value-of select="0"/>
							</TotalCost>



							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>

							<SMRequest>
								<xsl:value-of select="'true'"/>
							</SMRequest>



						</PositionMaster>

					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>

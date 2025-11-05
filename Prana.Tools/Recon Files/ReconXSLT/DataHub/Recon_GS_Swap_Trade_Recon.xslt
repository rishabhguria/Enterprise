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

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


	<xsl:template name ="varFutMonthCode">
		<xsl:param name="varFutMonth"/>

		<xsl:choose>

			<xsl:when  test ="$varFutMonth=1">
				<xsl:value-of select ="'F'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=2">
				<xsl:value-of select ="'G'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=3">
				<xsl:value-of select ="'H'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=4">
				<xsl:value-of select ="'J'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=5">
				<xsl:value-of select ="'K'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=6">
				<xsl:value-of select ="'M'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=7">
				<xsl:value-of select ="'N'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=8">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=9">
				<xsl:value-of select ="'U'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=10">
				<xsl:value-of select ="'v'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=11">
				<xsl:value-of select ="'X'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=12">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
		</xsl:choose>

	</xsl:template>




	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL22"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity) and COL9!='Dividends' and COL10!='Cancel' and COL7!='*'">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Goldman Sachs'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select ="COL30"/>
						</xsl:variable>



						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL23,' ')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@TickerSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="Symbol">

							<xsl:choose>
								<xsl:when test="contains(COL23,' ')">
									<xsl:value-of select="substring-before(COL23,' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL23"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>



						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$PRANA_SUFFIX_NAME!=''">
									<xsl:value-of select="concat($Symbol,$PRANA_SUFFIX_NAME,'/SWAP')"/>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="concat($Symbol,'/SWAP')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

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
						
						<PBSymbol>
							
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>


						<Date>
							<xsl:value-of select="COL15"/>
						</Date>

						<SettleDate>
							<xsl:value-of select="COL16"/>
						</SettleDate>

						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL42"/>
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

						<Side>
							<xsl:choose>

								<xsl:when test="COL10='CANCEL'">

									<xsl:choose>
										<xsl:when test="COL7='Buy' and COL8='Open'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>

										<xsl:when test="COL7='Buy' and COL8='Close'">
											<xsl:value-of select="'Sell Short'"/>
										</xsl:when>

										<xsl:when test="COL7='Sell' and COL8='Open'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>

										<xsl:when test="COL7='Sell' and COL8='Close'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL7='Buy' and COL8='Open'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>

										<xsl:when test="COL7='Buy' and COL8='Close'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>

										<xsl:when test="COL7='Sell' and COL8='Open'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>

										<xsl:when test="COL7='Sell' and COL8='Close'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL51"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="COL10='Cancel'">

									<xsl:choose>

										<xsl:when test="$Commission &lt; 0">
											<xsl:value-of select="$Commission"/>
										</xsl:when>

										<xsl:when test="$Commission &gt; 0">
											<xsl:value-of select="$Commission*-1"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>

									<xsl:choose>

										<xsl:when test="$Commission &gt; 0">
											<xsl:value-of select="$Commission"/>
										</xsl:when>

										<xsl:when test="$Commission &lt; 0">
											<xsl:value-of select="$Commission*-1"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>

								</xsl:otherwise>

							</xsl:choose>

						</Commission>
						
						<xsl:variable name="PRANA_STRATEGY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$PB_NAME]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
						</xsl:variable>
						<Strategy>
							<xsl:value-of select="concat(normalize-space(COL10),' , ',normalize-space(COL11))"/>
						</Strategy>
						
						
						

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL45"/>
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

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



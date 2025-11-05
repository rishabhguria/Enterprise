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
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">

					<PositionMaster>

						<xsl:variable name ="varPBName">
							<xsl:value-of select ="'Pershing'"/>
						</xsl:variable>
						
						<xsl:variable name ="Symbol">
							<xsl:value-of select ="COL9"/>
						</xsl:variable>
						<xsl:variable name ="PB_COMPANY">
							<xsl:value-of select ="COL11"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test ="$Symbol!=''">
									<xsl:value-of select ="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_COMPANY"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>

						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="COL2"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($Quantity)&gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:when test="number($Quantity)&lt; 0">
									<xsl:value-of select="(-1) * $Quantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>


						<SettlementCurrencyAveragePrice>
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
						</SettlementCurrencyAveragePrice>



						<xsl:variable name="Side" select="COL7"/>

						<Side>
							<xsl:choose>
								<xsl:when test ="$Side='S'and COL35='Margin'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>
								<xsl:when test ="$Side='S'and COL35='Short'">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:when test ="$Side='B'and COL35='Short'">
									<xsl:value-of select ="'Buy to Close'"/>
								</xsl:when>
								<xsl:when test ="$Side='B'and COL35='Margin'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY"/>
						</PBSymbol>


						<TradeDate>
							<xsl:value-of select="COL5"/>
							<!--<xsl:value-of select ="concat($Month,'/',$Date,'/',$Year)"/>-->
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="COL6"/>
							<!--<xsl:value-of select ="concat($Month,'/',$Date,'/',$Year)"/>-->
						</SettlementDate>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL20"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test =" $Commission &gt; 0">
									<xsl:value-of select ="$Commission"/>
								</xsl:when>
								<xsl:when test ="$Commission &lt; 0">
									<xsl:value-of select ="$Commission * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>
						
						<xsl:variable name="NetNotionalValue">
							<xsl:value-of select="COL16"/>
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
							<xsl:value-of select="COL15"/>
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

						<SettlementCurrencyTotalCost>
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
						</SettlementCurrencyTotalCost>
						
						
						<xsl:variable name="Misefees">
							<xsl:value-of select="COL21"/>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when test="$Misefees &gt; 0">
									<xsl:value-of select="$Misefees"/>
								</xsl:when>
								<xsl:when test="$Misefees &lt; 0">
									<xsl:value-of select="$Misefees* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>


						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
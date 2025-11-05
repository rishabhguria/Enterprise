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
				<xsl:value-of select="$varNumber * (-1)"/>
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
						<xsl:with-param name="Number" select="COL4"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test ="number($Quantity) and COL1!='Cash'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_CODE" >
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL12"/>
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



						<xsl:variable name="varSymbol">
							<xsl:value-of select="translate(COL3,'*','')"/>
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
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

					
						<Quantity>
							<xsl:value-of select ="$Quantity"/>
						</Quantity>

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


						<xsl:variable name ="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPriceBase>
							<xsl:choose>
								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarkPriceBase>


						<xsl:variable name ="NetMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>

								<xsl:when test ="$NetMarketValue">
									<xsl:value-of select ="$NetMarketValue"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValue>

						<xsl:variable name ="varNetMarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>

								<xsl:when test ="$varNetMarketValueBase">
									<xsl:value-of select ="$varNetMarketValueBase"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValueBase>

						
						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

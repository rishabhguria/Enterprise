<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
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

			<xsl:variable name="FundName" select="substring-before(substring-after(//Comparision[contains(COL1,'Account')]/COL1, ':'),'-')"/>

			<xsl:variable name="Date" select="substring-after(//Comparision[contains(COL1,'As of Date')]/COL1, ':')"/>

			<xsl:for-each select ="//Comparision">
				<xsl:if test ="number(COL4) and number(COL4) and not(contains(COL3,'EVOLVE BANK'))">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Evolve'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL1!='*'">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="normalize-space($FundName)"/>
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

						<TradeDate>
							<xsl:value-of select ="''"/>
						</TradeDate>

						<xsl:variable name ="Quantity">
							<xsl:value-of select ="number(COL4)"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="$Quantity ">
									<xsl:value-of select ="$Quantity"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

					

						<Side>
							<xsl:choose>

								<xsl:when test ="$Quantity &lt;0">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>

								<xsl:when test ="$Quantity &gt;0">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Side>



						<xsl:variable name ="COL6">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name ="COL4">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL4"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name ="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$COL6 div $COL4"/>
							</xsl:call-template>
						</xsl:variable>
						<MarkPrice>
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
						</MarkPrice>

						<xsl:variable name ="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValue>
							<xsl:choose>
								<xsl:when test ="$MarketValue &lt;0">
									<xsl:value-of select ="$MarketValue*-1"/>
								</xsl:when>
								<xsl:when test ="$MarketValue &gt;0">
									<xsl:value-of select ="$MarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>						
						

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>

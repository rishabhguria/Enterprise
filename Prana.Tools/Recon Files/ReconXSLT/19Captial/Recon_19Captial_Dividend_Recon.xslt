<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>



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

			<xsl:for-each select="//Comparision">

				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'HellerHouse'"/>
				</xsl:variable>

				<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL7)"/>

				<xsl:variable name="PRANA_SYMBOL_NAME">
					<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
				</xsl:variable>


				<!--<xsl:variable name ="varSymbol">
					<xsl:value-of select ="normalize-space(substring-before(translate(COL27, $varSmall,$varCapital),' EQUITY'))"/>
				</xsl:variable>-->
				
				<xsl:variable name="varDividend">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL13"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varDividend)">


				<xsl:variable name="Symbol" select="normalize-space(COL6)"/>

				<!--<xsl:if test="number(COL5) and normalize-space(COL21)='STOCK'">-->

					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						
						<Dividend>
							<xsl:choose>
								<xsl:when  test="number($varDividend)">
									<xsl:value-of select="$varDividend"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Dividend>

						<PayoutDate>
							<xsl:value-of select="COL5"/>
						</PayoutDate>

						<ExDate>
							<xsl:value-of select="COL4"/>
						</ExDate>

						<Description>
							<xsl:choose>
								<xsl:when test ="$varDividend &gt; 0">
									<xsl:value-of select="'Dividend Received'"/>
								</xsl:when>
								<xsl:when test ="$varDividend &lt; 0">
									<xsl:value-of select="'Dividend Charged'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>

						<xsl:variable name="Currency">
							<xsl:choose>
								<xsl:when test="contains(COL2,'U.S. DOLLAR')">
									<xsl:value-of select="'USD'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<Currency>
							<xsl:value-of select="$Currency"/>
						</Currency>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
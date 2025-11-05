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

			<xsl:for-each select ="//Comparision">
				

				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="substring(COL1,19,14)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="$varQuantity and substring(COL1,236,4)='JPMC' and substring(COL1,224,1)!='0'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Pershing_PMP'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="substring(COL1,236,4)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="Symbol" select="normalize-space(substring(COL1,42,6))"/>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,1,8)"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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


						<xsl:variable name="Quantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="format-number(substring(COL1,19,14),'#.####')"/>
							</xsl:call-template>
						</xsl:variable>
						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($Quantity) &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:when test="number($Quantity) &lt; 0">
									<xsl:value-of select="$Quantity* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="varAvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="format-number(substring(COL1,103,16),'#.##') div $Quantity"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<Side>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>


						<xsl:variable name="Year">
							<xsl:value-of select="substring(COL1,57,4)"/>
						</xsl:variable>
						<xsl:variable name="Month">
							<xsl:value-of select="substring(COL1,61,2)"/>
						</xsl:variable>
						<xsl:variable name="Day">
							<xsl:value-of select="substring(COL1,63,2)"/>
						</xsl:variable>
						<Date>
							<xsl:value-of select ="concat($Month,'/',$Day,'/',$Year)"/>
						</Date>


						<xsl:variable name="CurrencySymbol">
							<xsl:value-of select="substring(COL1,73,3)"/>
						</xsl:variable>
						<Currency>
							<xsl:value-of select="$CurrencySymbol"/>
						</Currency>


						<xsl:variable name="Year1">
							<xsl:value-of select="substring(COL1,65,4)"/>
						</xsl:variable>
						<xsl:variable name="Month1">
							<xsl:value-of select="substring(COL1,69,2)"/>
						</xsl:variable>
						<xsl:variable name="Day1">
							<xsl:value-of select="substring(COL1,71,2)"/>
						</xsl:variable>
						<SettlementDate>
							<xsl:value-of select ="concat($Month1,'/',$Day1,'/',$Year1)"/>
						</SettlementDate>
						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>


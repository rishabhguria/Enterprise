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
				<xsl:value-of select="$varNumber * (-1)"/>
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

	<xsl:template name="MonthCode">
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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL14)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'NAV'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL29"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_Underlyn_NAME">
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name="PRANA_UNDERLYNE_NAME">
							<xsl:value-of select="document('../ReconMappingXML/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@UnderlyingCode=$PB_SYMBOL_NAME]/@PBCode"/>
						</xsl:variable>
						
						<xsl:variable name="MonthsCode">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="substring(COL12,5,2)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="UnderlyingSymbol">
							<xsl:choose>
								<xsl:when test="$PRANA_UNDERLYNE_NAME!=''">
									<xsl:value-of select="$PRANA_UNDERLYNE_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_Underlyn_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="Year">
							<xsl:value-of select="substring(COL12,4,1)"/>
						</xsl:variable>
						<xsl:variable name="Future">
							<xsl:value-of select="concat($MonthsCode,$Year)"/>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:value-of select="concat(COL6 , COL12)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
								<xsl:when test="COL6!=''">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',$Future)"/>
								</xsl:when>



								<!--<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>-->
								
							
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="COL4"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

					

						<xsl:variable name="PB_Root_Name" select="COL6"/>

						<xsl:variable name ="PRANA_Prise_Multeplier">
							<xsl:value-of select ="document('../ReconMappingXml/PriceMulMapping.xml')/PriceMulMapping/PB[@Name=$PB_NAME]/MultiplierData[@PranaRoot=$PB_Root_Name]/@Multiplier"/>
						</xsl:variable>
						
						<xsl:variable name="varPrice">
							<xsl:value-of select="COL17"/>
						</xsl:variable>
						<xsl:variable name="MarkPrice">
							
							<xsl:choose>
								<xsl:when test="number($PRANA_Prise_Multeplier)">
									<xsl:value-of select="$varPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varPrice"/>
								</xsl:otherwise>
							</xsl:choose>
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



					

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Year1" select="substring(COL2,1,4)"/>
						<xsl:variable name="Month" select="substring(COL2,5,2)"/>
						<xsl:variable name="Day" select="substring(COL2,8,2)"/>

						<Date>
							<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>
						</Date>

						<!--<BaseCurrency>
							<xsl:value-of select="''"/>
						</BaseCurrency>
						<SettlCurrency>
							<xsl:value-of select="''"/>
						</SettlCurrency>-->

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
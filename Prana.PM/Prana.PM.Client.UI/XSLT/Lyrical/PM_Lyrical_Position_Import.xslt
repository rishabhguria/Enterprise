<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
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

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="substring(COL1,19,14)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and substring(COL1,236,4)='JPMC'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Goodnow'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="substring(COL1,236,4)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="Symbol" select="substring(COL1,33,9)"/>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,13,6)"/>
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

						<xsl:variable name="varPosition">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="format-number(substring(COL1,19,14),'#.####')"/>
							</xsl:call-template>
						</xsl:variable>
						<NetPosition>
							<xsl:choose>
								<xsl:when test="$varPosition &gt; 0">
									<xsl:value-of select="$varPosition"/>
								</xsl:when>
								<xsl:when test="$varPosition &lt; 0">
									<xsl:value-of select="$varPosition * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>



						<xsl:variable name="Costbasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="format-number(substring(COL1,103,16),'#.##')"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$Costbasis &gt; 0">
									<xsl:value-of select="$Costbasis"/>
								</xsl:when>
								<xsl:when test="$Costbasis &lt; 0">
									<xsl:value-of select="$Costbasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="CurrencySymbol">
							<xsl:value-of select="substring(COL1,73,3)"/>
						</xsl:variable>
						<CurrencySymbol>
							<xsl:value-of select="$CurrencySymbol"/>
						</CurrencySymbol>
					
						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Year">
							<xsl:value-of select="substring(COL1,57,4)"/>
						</xsl:variable>
						<xsl:variable name="Month">
							<xsl:value-of select="substring(COL1,61,2)"/>
						</xsl:variable>
						<xsl:variable name="Day">
							<xsl:value-of select="substring(COL1,63,2)"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select ="concat($Month,'/',$Day,'/',$Year)"/>
						</PositionStartDate>

						<xsl:variable name="Year1">
							<xsl:value-of select="substring(COL1,65,4)"/>
						</xsl:variable>
						<xsl:variable name="Month1">
							<xsl:value-of select="substring(COL1,69,2)"/>
						</xsl:variable>
						<xsl:variable name="Day1">
							<xsl:value-of select="substring(COL1,71,2)"/>
						</xsl:variable>
						<PositionSettlementDate>
							<xsl:value-of select ="concat($Month1,'/',$Day1,'/',$Year1)"/>
						</PositionSettlementDate>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
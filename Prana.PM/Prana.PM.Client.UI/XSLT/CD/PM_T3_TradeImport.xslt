<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL4 != 'quantity_executed' and number(COL4)">
					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol">
							<xsl:value-of select = "COL3"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>


						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>




						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME=''">
									<xsl:value-of select="$PB_Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number(normalize-space(COL4)) &gt; 0">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:when test="number(normalize-space(COL4)) &lt; 0">
									<xsl:value-of select="COL4 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test="translate(COL5, $vLowercaseChars_CONST, $vUppercaseChars_CONST)='BUY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL5= 'COVER'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="COL5= 'SELL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL5= 'SHORT'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<CostBasis>
							<xsl:choose>
								<xsl:when  test="number(COL7)">
									<xsl:value-of select="COL7"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</CostBasis>

						<!--<Commission>
					<xsl:value-of select="COL8"/>
				</Commission>-->
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>


</xsl:stylesheet>

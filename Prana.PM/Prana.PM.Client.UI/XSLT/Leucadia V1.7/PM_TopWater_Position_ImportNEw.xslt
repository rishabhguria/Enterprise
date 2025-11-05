<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<DocumentElement>
		<xsl:for-each select="//PositionMaster">
			<xsl:if test="number(COL4)">
				<PositionMaster>

					<xsl:variable name="PBSuffixCode">
						<xsl:value-of select = "COL2"/>
					</xsl:variable>

					<xsl:variable name="PB_ExchangeCODE">
						<xsl:value-of select="COL3"/>
					</xsl:variable>

					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
					</xsl:variable>
					
					<Symbol>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME=''">
								<xsl:value-of select="COL2"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(COL2,$PRANA_SYMBOL_NAME)"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<AccountName>
						<xsl:value-of select="''"/>
					</AccountName>
					
					<NetPosition>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL4)) &gt; 0">
								<xsl:value-of select="COL4"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL4)) &lt; 0">
								<xsl:value-of select="COL4* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPosition>

					<CostBasis>
						<xsl:choose>
							<xsl:when  test="$PRANA_SYMBOL_NAME = '-JSE' or $PRANA_SYMBOL_NAME = '-LON'">
								<xsl:value-of select="COL5 div 100"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL5))">
								<xsl:value-of select="COL5"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CostBasis>

					<PositionStartDate>
						<xsl:value-of select="''"/>
					</PositionStartDate>

					<SideTagValue>
						<xsl:choose>
							<xsl:when test="COL1 = 'BUY'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="COL1 = 'SELL'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SideTagValue>

					<PBSymbol>
						<xsl:value-of select="COL2"/>
					</PBSymbol>

					<TradeAttribute1>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME=''">
								<xsl:value-of select="COL2"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(COL2,$PRANA_SYMBOL_NAME)"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeAttribute1>
				</PositionMaster>
			</xsl:if>
		</xsl:for-each>
	</DocumentElement>
</xsl:template>

</xsl:stylesheet> 

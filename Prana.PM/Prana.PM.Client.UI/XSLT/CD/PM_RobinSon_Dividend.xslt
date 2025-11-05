<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
	<DocumentElement>

		<xsl:for-each select="//PositionMaster">
			<xsl:if test="number(COL7)">
				<PositionMaster>

					<xsl:variable name="PB_Symbol">
						<xsl:value-of select="COL2"/>
					</xsl:variable>

					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
					</xsl:variable>

					<AccountName>
						<xsl:value-of select="''"/>
					</AccountName>

					<PBSymbol>
						<xsl:value-of select="COL2"/>
					</PBSymbol>

					<Symbol>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME = ''">
								<xsl:value-of select="COL1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<Dividend>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL14)) ">
								<xsl:value-of select="COL14"/>
							</xsl:when>						
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Dividend>

					<PayoutDate>
						<xsl:value-of select="COL4"/>
					</PayoutDate>

					<ExDate>
						<xsl:value-of select="COL3"/>
					</ExDate>

					<RecordDate>
						<xsl:value-of select="COL5"/>
					</RecordDate>
					
				</PositionMaster>
			</xsl:if>
		</xsl:for-each>
	</DocumentElement>
</xsl:template>

</xsl:stylesheet> 

<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="COL14 != 'Current Local Price'">

					<PositionMaster>
						<Date>
							<xsl:value-of select="''"/>
						</Date>

						<xsl:variable name="PBSuffixCode">
							<xsl:value-of select = "COL5"/>
						</xsl:variable>

						<xsl:variable name="PB_ExchangeCODE">
							<xsl:value-of select="substring-after(COL5, ' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
						</xsl:variable>



						<xsl:variable name="PB_SymbolName">
							<xsl:value-of select="substring-before(COL5,' ')"/>
						</xsl:variable>
						
						<xsl:variable name="PB_Symbol" select="COL5"/>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'JPM']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<!--<xsl:when test ="$PRANA_Exchange = ''">
									<xsl:value-of select ="COL5"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<MarkPrice>
							<xsl:choose>
								<xsl:when  test="number(normalize-space(COL14))">
									<xsl:value-of select="COL14"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<PBSymbol>
							<xsl:value-of select="COL10"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

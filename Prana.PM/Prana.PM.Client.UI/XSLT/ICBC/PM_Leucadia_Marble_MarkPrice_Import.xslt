<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				
				<xsl:if test="number(COL3)">
					
					<PositionMaster>
						
						<xsl:variable name="varPBName">
							<xsl:value-of select="'BOFA'"/>
						</xsl:variable>				

						<xsl:variable name="varMarkPrice">
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name="PBSuffixCode">
							<xsl:value-of select = "COL4"/>
						</xsl:variable>

						<xsl:variable name="PB_ExchangeCODE">
							<xsl:value-of select="substring-after(COL4, ' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name ="PB_SymbolName">
							<xsl:value-of select ="substring-before(COL4, ' ')"/>
						</xsl:variable>

						<xsl:variable name="PB_Symbol" select="COL4"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'JPM']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>				

						<Date>
							<xsl:value-of select="''"/>
						</Date>					

						<!--<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME=''">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($PB_SymbolName,$PRANA_SYMBOL_NAME)"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>-->							

						<MarkPrice>
							<xsl:choose>
								<xsl:when  test="number($varMarkPrice)">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</MarkPrice>
						
						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>
						
					</PositionMaster>
					
				</xsl:if>
				
			</xsl:for-each>

		</DocumentElement>
		
	</xsl:template>



</xsl:stylesheet>

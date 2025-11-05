<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="COL8 != 'Cash and Equivalents' and COL8 != 'Asset Class' ">
					<PositionMaster>

						<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(substring(COL6,2))"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Jefferies']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								<PBSymbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</PBSymbol>
							</xsl:when>
							<xsl:when test ="COL8 = 'Options'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(substring(COL1,1,21),'U')"/>
								</IDCOOptionSymbol>
								<PBSymbol>
									<xsl:value-of select="COL1"/>
								</PBSymbol>
							</xsl:when>
							<!--<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="starts-with(COL6,'Q') and $varSymbol != 'QQ'">
										<xsl:variable name = "varLength" >
											<xsl:value-of select="string-length(normalize-space(COL6))"/>
										</xsl:variable>
										<Symbol>
											<xsl:value-of select="concat(substring(COL6,2,($varLength - 3)),' ',substring(COL6,($varLength - 1),$varLength))"/>
										</Symbol>
									</xsl:when>-->
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL6"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
								<PBSymbol>
									<xsl:value-of select="COL6"/>
								</PBSymbol>
							</xsl:otherwise>
							<!--</xsl:choose>
							</xsl:otherwise>-->
						</xsl:choose>


						<xsl:choose>
							<xsl:when  test="COL7='CAD' and boolean(number(COL11))">
								<MarkPrice>
									<xsl:value-of select="(COL15 div COL11)"/>
								</MarkPrice>
							</xsl:when >						
							<xsl:when  test="boolean(number(COL14))">
								<MarkPrice>
									<xsl:value-of select="COL14"/>
								</MarkPrice>							
							</xsl:when >							
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >
						
						<Date>
							<xsl:value-of select="''"/>
						</Date>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

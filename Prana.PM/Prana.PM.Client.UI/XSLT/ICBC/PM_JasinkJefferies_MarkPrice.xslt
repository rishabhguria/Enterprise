<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="COL9 != 'Cash and Equivalents' and  number(COL15)">
					<PositionMaster>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'Jefferies'"/>
						</xsl:variable>
						
						<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(substring(COL8,2))"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL9 = 'Options' and COL4 != ''">
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="concat(COL2, 'U')"/>
										</IDCOOptionSymbol>
									</xsl:when>
									<xsl:when test ="COL9 = 'Options' and COL4 = ''">
										<Symbol>
											<xsl:value-of select="concat(substring(COL3,1,2), ' ',substring(COL3,3,2),substring(COL6,1,1), number(substring(COL5,4))*1000)"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="COL7"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="COL7='CAD' and boolean(number(COL11)) and COL11 &gt; 0">
								<MarkPrice>
									<xsl:value-of select="(COL15 div COL11)"/>
								</MarkPrice>
							</xsl:when >
							<xsl:when  test="boolean(number(COL15))">
								<MarkPrice>
									<xsl:value-of select="COL15"/>
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

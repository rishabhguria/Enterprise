<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<PositionMaster>
					<!--  Symbol Region -->
					<!--<xsl:variable name="PB_COMPANY_NAME" select="translate(COL4,'&quot;','')"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">								
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="starts-with(COL5,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL5)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL5,2,($varLength - 3)),' ',substring(COL5,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL17"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>-->

					<Symbol>
						<xsl:value-of select="COL9"/>
					</Symbol>

					<xsl:choose>
						<xsl:when  test="boolean(number(COL12))">
							<MarkPrice>
								<xsl:value-of select="COL12"/>
							</MarkPrice>
						</xsl:when >
						<xsl:otherwise>
							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:variable name ="varYear">
						<xsl:value-of select ="substring(COL1,1,4)"/>
					</xsl:variable>

					<xsl:variable name ="varMonth">
						<xsl:value-of select ="substring(COL1,5,2)"/>
					</xsl:variable>

					<xsl:variable name ="varDay">
						<xsl:value-of select ="substring(COL1,7,2)"/>
					</xsl:variable>

					<Date>
						<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
					</Date>
				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:variable name = "varDate">
				<xsl:value-of select="PositionMaster[substring-before(COL1,'of')='Positions as ']/COL1"/>
			</xsl:variable>

			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL2"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='US Equity' or $varInstrumentType ='Non-US Equity'">
					<PositionMaster>
						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Barclays']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="starts-with(COL4,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL4)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL4,2,($varLength - 3)),' ',substring(COL4,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL4"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<!--<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>-->

						<xsl:choose>
							<xsl:when  test="boolean(number(COL6))">
								<MarkPrice>
									<xsl:value-of select="COL6"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

						<Date>
							<xsl:value-of select="substring-after($varDate,'of')"/>
						</Date>
						
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

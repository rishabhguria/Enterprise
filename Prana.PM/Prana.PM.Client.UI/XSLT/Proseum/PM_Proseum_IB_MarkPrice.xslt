<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test ="COL1='POST'">
					<PositionMaster>
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Proseum']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<!--  Symbol Region -->						
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL6"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="COL15='MarkPrice' or COL15='*'">
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="number(translate(COL15,',',''))"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >
						<!-- Position Date mapped with the column 1 -->
						<xsl:choose>
							<xsl:when test ="COL8='*' or COL8='ReportDate'">
								<Date>
									<xsl:value-of select="''"/>
								</Date>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name = "varYR" >
									<xsl:value-of select="translate(substring(COL8,1,4),'&quot;','')"/>
								</xsl:variable>
								<xsl:variable name = "varMth" >
									<xsl:value-of select="translate(substring(COL8,5,2),'&quot;','')"/>
								</xsl:variable>
								<xsl:variable name = "varDt" >
									<xsl:value-of select="translate(substring(COL8,7,2),'&quot;','')"/>
								</xsl:variable>
								<Date>
									<xsl:value-of select="translate(concat($varYR,'/',$varMth,'/',$varDt),'&quot;','')"/>
								</Date>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>

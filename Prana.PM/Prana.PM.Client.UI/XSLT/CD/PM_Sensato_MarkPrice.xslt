<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL3 !='L/S'">

					<PositionMaster>

						<!--Trade Date-->

						<xsl:choose>
							<xsl:when test="string-length(COL2)= 8">
								<Date>
									<xsl:value-of select ="concat(substring(COL2,5,2),'/',substring(COL2,7,2),'/',substring(COL2,1,4))"/>
								</Date>
							</xsl:when>
							<xsl:otherwise>
								<Date>
									<xsl:value-of select ="''"/>
								</Date>
							</xsl:otherwise>
						</xsl:choose>


						<!--SYMBOL-->
						<!--<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL17)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>-->

						<!--<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="normalize-space(COL17)"/>
								</PBSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:when test="COL15 != ''">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="concat('SEDOL - ',COL15, ', Description - ',COL15)"/>
								</PBSymbol>-->
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
						
								<SEDOL>
									<xsl:value-of select="substring-before(COL4,'CFDUSD')"/>
								</SEDOL>
							<!--</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="''"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>-->
						
						<!--Mark Price-->


						<xsl:choose>
							<xsl:when  test="number(COL7) and number(COL7) &lt; 0">
								<MarkPrice>
									<xsl:value-of select="COL7 * (-1)"/>
								</MarkPrice>
							</xsl:when >
							<xsl:when  test="number(COL7) and number(COL7) &gt; 0">
								<MarkPrice>
									<xsl:value-of select="COL7"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

						

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	
</xsl:stylesheet>

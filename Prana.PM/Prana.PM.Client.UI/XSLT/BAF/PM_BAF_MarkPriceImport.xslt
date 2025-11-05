<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="string-length(COL1) &lt; 4 and COL1!='*'">
					<PositionMaster>
						<xsl:variable name ="varPBSymbol" select="COL2" />
						<xsl:variable name ="varIsOption">
							<xsl:choose>
								<xsl:when test ="COL26='Options'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test ="$varIsOption !=''">
								<IDCOOptionSymbol>
									<xsl:value-of select="concat($varPBSymbol,'U')"/>
								</IDCOOptionSymbol>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:when>

							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="$varPBSymbol"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
								<xsl:when  test="boolean(number(COL11))">
									<MarkPrice>
										<xsl:value-of select="COL11"/>
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



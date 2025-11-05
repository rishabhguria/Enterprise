<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

		
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				
					<PositionMaster>
						<xsl:variable name="varPBSymbol" select="COL1"/>
						<xsl:choose>
							<xsl:when test ="$varPBSymbol!=''">
								<Symbol>
									<xsl:value-of select ="$varPBSymbol"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL7))">
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
					
						
								<Date>
									<xsl:value-of select="''"/>
								</Date>
							

					</PositionMaster>
				<!--</xsl:if>-->
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>



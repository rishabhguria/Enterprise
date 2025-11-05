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
				<xsl:if test="boolean(number(COL8))">
				
				
					<PositionMaster>

						<Symbol>
							<xsl:value-of select ="COL2"/>
						</Symbol>

						<PBSymbol>
							<xsl:value-of select ="COL2"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL8))">
								<MarkPrice>
									<xsl:value-of select="COL8"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >
						<!--Date Field does not come in the Position file, so user will select from the UI -->
						<Date>
							<xsl:value-of select="COL66"/>
						</Date>

					</PositionMaster>
				</xsl:if>
				
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

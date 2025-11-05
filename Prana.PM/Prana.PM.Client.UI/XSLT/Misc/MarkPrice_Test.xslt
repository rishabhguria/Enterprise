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
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<Symbol>
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</Symbol>

						<xsl:choose>
							<xsl:when test ="COL3 &lt; 0 or COL3 &gt; 0 or COL3 = 0">
								<MarkPrice>
									<xsl:value-of select="COL3"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>								
							</xsl:otherwise>
						</xsl:choose >

						<!-- Position Date mapped with the column 10 -->
						<Date>
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</Date>						
					</PositionMaster>				
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

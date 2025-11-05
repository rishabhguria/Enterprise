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
					<xsl:choose>
						<xsl:when test ="COL3 != ''">
							<Symbol>
								<xsl:value-of select="translate(COL3,'&quot;','')"/>
							</Symbol>
						</xsl:when>
						<xsl:when test ="COL3 = ''">
							<Symbol>
								<xsl:value-of select="translate(COL1,'&quot;','')"/>
							</Symbol>
						</xsl:when>
						<xsl:when test ="COL1 = ''">
							<xsl:variable name = "varLength" >
								<xsl:value-of select="string-length(translate(translate(COL5,'&quot;',''),' ',''))"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test ="$varLength &gt; 0 ">
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring(COL5,($varLength)-1,2)"/>
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring(COL5,1,($varLength)-2)"/>
									</xsl:variable>
									<Symbol>
										<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
									</Symbol>
								</xsl:when>
								<xsl:otherwise>
									<Symbol>
										<xsl:value-of select="''"/>
									</Symbol>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							
						</xsl:otherwise>
					</xsl:choose>
					
					

					<xsl:choose>
						<xsl:when test ="COL10 &lt; 0 or COL10 &gt; 0 or COL10 = 0">
							<MarkPrice>
								<xsl:value-of select="COL10"/>
							</MarkPrice>
						</xsl:when >
						<xsl:otherwise>
							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>
						</xsl:otherwise>
					</xsl:choose >
					
					<xsl:choose>
						<xsl:when test="COL7 = '*' or COL7 = 'Trade/Settlement/Value Date'">
							<Date>
								<xsl:value-of select="''"/>
							</Date>
						</xsl:when>
						<xsl:otherwise>
							<Date>
								<xsl:value-of select="translate(COL7,'&quot;','')"/>
							</Date>
						</xsl:otherwise>
					</xsl:choose >

				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

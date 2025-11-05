<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				
					<PositionMaster>

						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

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
							<xsl:when test ="$varPBSymbol!=''">
								<PBSymbol>
									<xsl:value-of select ="$varPBSymbol"/>
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<PBSymbol>
									<xsl:value-of select="''"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="boolean(number(COL5))">
								<CostBasis>
									<xsl:value-of select="COL5"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >


								<PositionStartDate>
									<xsl:value-of select="''"/>
								</PositionStartDate>
											

						<xsl:choose>
							<xsl:when  test="boolean(number(COL4))and number(COL4) &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL4 * -1"/>
								</NetPosition>
							</xsl:when>
							<xsl:when  test="boolean(number(COL4))and number(COL4) &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL4"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when  test="COL3='Long'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when  test="COL3='Short'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>

						
					
					</PositionMaster>
				</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>



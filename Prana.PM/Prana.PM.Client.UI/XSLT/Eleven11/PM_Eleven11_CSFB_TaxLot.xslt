<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL3"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType ='Common Stock'">
					<PositionMaster>
						<!--   Fund -->
						<FundName>
							<xsl:value-of select="' '"/>
						</FundName>

						<Symbol>
							<xsl:value-of select="COL6"/>
						</Symbol>
						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<PBAssetType>
							<xsl:value-of select="COL3"/>
						</PBAssetType>
						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when test ="COL4 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL4*(-1)"/>
								</NetPosition>
							</xsl:when >
							<xsl:when test ="COL4 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL4"/>
								</NetPosition>
							</xsl:when >
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:choose>
							<xsl:when test ="boolean(number(COL12) and COL12 != 0 and COL4 != 0)">
								<CostBasis>
									<xsl:value-of select="COL12 div COL4"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Position Date mapped with the column 9 -->
						<xsl:choose>
							<xsl:when test ="COL9='Lot Date' or COL9='*'">
								<PositionStartDate>
									<xsl:value-of select="''"/>
								</PositionStartDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<xsl:value-of select="COL9"/>
								</PositionStartDate>
							</xsl:otherwise>
						</xsl:choose>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

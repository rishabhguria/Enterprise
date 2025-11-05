<?xml version="1.0" encoding="utf-8"?>
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

				<xsl:if test="COL1 !='SYMBOL'">
					<PositionMaster>
						<!--fundname section-->
						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

						<PBAssetType>
							<xsl:value-of select="COL2"/>
						</PBAssetType>

						<xsl:choose>
							<xsl:when test="boolean(number(COL8)) and COL8 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL8 * (-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="boolean(number(COL8)) and COL8 &gt; 0">
								<NetPosition>
									<xsl:value-of select="COL8"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<!--Side-->
						<xsl:choose>
							<xsl:when test="COL3='BUY'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL3='SELL'">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL3='SHORT SELL'">
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

						<!-- Position Date mapped with the column 9 -->
						<xsl:choose>
							<xsl:when test ="COL9 != 'Trade Date'">
								<PositionStartDate>
									<xsl:value-of select="COL9"/>
								</PositionStartDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<xsl:value-of select="''"/>
								</PositionStartDate>
							</xsl:otherwise>
						</xsl:choose>
						

						<xsl:choose>
							<xsl:when test="boolean(number(COL11))">
								<CostBasis>
									<xsl:value-of select="COL11"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="COL4 = 'Symbol'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL4"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL13))">
								<Commission>
									<xsl:value-of select ="(COL13 + COL16 + COL17) * (-1)"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select ="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
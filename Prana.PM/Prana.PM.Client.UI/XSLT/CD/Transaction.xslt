<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/Nirvana/SensatoV1.7_Allocation/Client/MappingFiles/PranaXSD/ImportPositions.xsd</xsl:attribute>
	<xsl:for-each select="//PositionMaster">
		<PositionMaster>

      <Symbol>
        <xsl:value-of select ="COL4"/>
      </Symbol>
      <PBSymbol>
        <xsl:value-of select ="COL4"/>
      </PBSymbol>
	  <AccountName>
			<xsl:value-of select="''"/>
		</AccountName>
		<xsl:choose>
						<xsl:when test="COL5='BUY'">
							<SideTagValue>
								<xsl:value-of select="1"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test="COL5='SELL'">
							<SideTagValue>
								<xsl:value-of select="2"/>
							</SideTagValue>
						</xsl:when>
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="0"/>
							</SideTagValue>
						</xsl:otherwise>
		</xsl:choose>

					<xsl:choose>
						<xsl:when test="number(COL7) and number(COL7) &lt; 0 ">
							<NetPosition>
								<xsl:value-of select="normalize-space(COL17) *(-1)"/>
							</NetPosition>
						</xsl:when>
						<xsl:when test=" number(COL7) and number(COL7) &gt; 0">
							<NetPosition>
								<xsl:value-of select="normalize-space(COL17)"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="'0'"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>
					<PositionStartDate>
						<xsl:value-of select="COL2"/>
					</PositionStartDate>
					<CostBasis>
						<xsl:value-of select="COL8"/>
					</CostBasis>
					
					

      <PBAssetType>
        <xsl:value-of select ="Equity"/>
      </PBAssetType>

		
		</PositionMaster>


			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
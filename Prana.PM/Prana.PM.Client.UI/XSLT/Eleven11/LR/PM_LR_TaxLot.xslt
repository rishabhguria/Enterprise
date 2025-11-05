<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL21, ' ' , ''),'&quot;','')"/>
				</xsl:variable>
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='5295005'">								
									<FundName>
									<xsl:value-of select="'LR F LTD'"/>
									</FundName>	
							</xsl:when>
							<xsl:when test="$varPortfolioID='5296005'">
								<FundName>
									<xsl:value-of select="'LR LP'"/>
								</FundName>								
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="' '"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >

						<!--   CUSIP -->
						<!-- Column 2 mapped with CUSIP-->
						<!--<CUSIP>
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</CUSIP>
						<SEDOL>
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</SEDOL>-->
						<Bloomberg>
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</Bloomberg>
						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</PBSymbol>
						<PBAssetType>
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</PBAssetType>
						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when test ="COL5 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL5*(-1)"/>
								</NetPosition>
							</xsl:when >
							<xsl:when test ="COL5 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL5"/>
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
							<xsl:when test ="COL9 &lt; 0 or COL9 &gt; 0 or COL9 = 0">
								<CostBasis>
									<xsl:value-of select="COL9"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>
						
						<!-- Position Date mapped with the column 6 -->
						<PositionStartDate>
							<xsl:value-of select="translate(COL6,'&quot;','')"/>
						</PositionStartDate>
					</PositionMaster>
				</xsl:for-each>
			</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8"?>
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
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='21000000'">
								<FundName>
									<xsl:value-of select="'Letrp Value'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='000000005296005'">
								<FundName>
									<xsl:value-of select="'Letrol Value'"/>
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
						<CUSIP>
							<xsl:value-of select="translate(COL10,'&quot;','')"/>
						</CUSIP>
						<SEDOL>
							<xsl:value-of select="translate(COL9,'&quot;','')"/>
						</SEDOL>
						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="translate(COL13,'&quot;','')"/>
						</PBSymbol>
						<PBAssetType>
							<xsl:value-of select="translate(COL16,'&quot;','')"/>
						</PBAssetType>						
						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when test="COL18 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL18*(-1)"/>
								</NetPosition>	
							</xsl:when>
							<xsl:when test ="COL18 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL18"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="COL14 &lt; 0 or COL14 &gt; 0 or COL14=0 ">
								<CostBasis>
									<xsl:value-of select="COL14"/>
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
							<xsl:value-of select="translate(COL5,'&quot;','')"/>
						</PositionStartDate>
					</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>	
</xsl:stylesheet>

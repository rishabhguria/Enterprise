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
						<!--<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>-->

						<xsl:choose>
							<xsl:when test="COL7 ='MONROE MODERATE'">								
								<FundName>
									<xsl:value-of select="'Monroe Moderate'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="COL7 ='MONROE CONSERVATIVE'">								
								<FundName>
									<xsl:value-of select="'Monroe Conservative'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="COL7"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<PBAssetType>
							<xsl:value-of select="''"/>
						</PBAssetType>

						<xsl:choose>
							<xsl:when test="boolean(number(COL3))">
								<NetPosition>
									<xsl:value-of select="COL3"/>
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
							<xsl:when test="COL2='BOUGHT'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL2='SOLD'">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL2='SOLD SHORT'">
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

						<!-- Position Date mapped with the column 16 -->
						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<xsl:choose>
							<xsl:when test="boolean(number(COL4))">
								<CostBasis>
									<xsl:value-of select="COL4"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="COL1 = 'SYMBOL'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL1"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
						<PBSymbol>
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</PBSymbol>

						<CounterPartyID>
							<xsl:value-of select ="1"/>
						</CounterPartyID>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
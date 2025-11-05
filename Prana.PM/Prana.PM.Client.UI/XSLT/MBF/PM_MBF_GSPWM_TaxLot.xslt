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

				<xsl:if test="COL3 !='SYMBOL' and COL3 !='*'">
					<PositionMaster>
						<!--fundname section-->
						<!--<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>-->

						<xsl:choose>
							<xsl:when test="COL7 ='028-21850-1 GS/IB'">
								<FundName>
									<xsl:value-of select="'028-21850-1 Conservative GS/IB MFisher GS'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="COL7 ='028-21851-9 GS/IB'">
								<FundName>
									<xsl:value-of select="'028-21851-9 Moderate GS/IB MFisher GS'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<PBAssetType>
							<xsl:value-of select="''"/>
						</PBAssetType>

						<xsl:choose>
							<xsl:when test="boolean(number(COL4))">
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

						<!--Side-->
						<xsl:choose>
							<xsl:when test="COL2='B'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL2='SS'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL2='S'">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Position Date mapped with the column 1 -->
						<xsl:choose>
							<xsl:when test="COL1 ='GSPW' or COL5='DATE' or COL5='*'">
								<PositionStartDate>
									<xsl:value-of select="''"/>
								</PositionStartDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<xsl:value-of select="COL1"/>
								</PositionStartDate>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:choose>
							<xsl:when test="boolean(number(COL5))">
								<CostBasis>
									<xsl:value-of select="COL5"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="COL3 = 'SYMBOL'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL3"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
						<PBSymbol>
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when test ="COL6='GSPW'">
								<CounterPartyID>
									<xsl:value-of select ="2"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select ="0"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
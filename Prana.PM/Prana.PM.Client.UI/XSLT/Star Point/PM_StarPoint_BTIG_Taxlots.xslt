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

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL15,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="($varInstrumentType='Equity')">
					<PositionMaster>
						<!--   Fund -->
						<xsl:choose>
							<xsl:when test ="COL2=''">
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:when>
						</xsl:choose>
						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

						<!--  Symbol Region -->
						<Symbol>
							<xsl:value-of select="COL3"/>
						</Symbol>

						<xsl:choose>
							<xsl:when test ="COL10='BTIG'">
								<CounterPartyID>
									<xsl:value-of select ="'1'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select ="'0'"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</PBSymbol>

						<PBAssetType>
							<xsl:value-of select="$varInstrumentType"/>
						</PBAssetType>

						<!-- Column 4 mapped with Side-->
						<xsl:variable name = "varSide" >
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varSide='BL'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SL'">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='SS'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='BC'">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Column 8 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when  test="COL12 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL12*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL12 &gt; 0 or COL12 = 0">
								<NetPosition>
									<xsl:value-of select="COL12"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<!--  Average Price Region -->
						<xsl:choose>
							<xsl:when  test="boolean(number(COL9))">
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
						
						<xsl:choose>
							<xsl:when  test="COL12 &lt; 0">
								<Commission>
									<xsl:value-of select="COL12*(-1) * COL14"/>
								</Commission>
							</xsl:when>
							<xsl:when test="COL12 &gt; 0 or COL12 = 0">
								<Commission>
									<xsl:value-of select="COL12 * COL14"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Position Date mapped with the column 6 -->
						<!--<xsl:variable name = "varYear"  select="substring(COL6,1,4)"/>
						<xsl:variable name = "varMonth"  select="substring(COL6,5,2)"/>
						<xsl:variable name = "varDay"  select="substring(COL6,7)"/>

						<PositionStartDate>
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</PositionStartDate>-->
						<PositionStartDate>
							<xsl:value-of select="COL6"/>
						</PositionStartDate>
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>

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
					<xsl:value-of select="translate(COL21,'&quot;','')"/>
				</xsl:variable>

				<!--<xsl:if test="($varInstrumentType='Cash Equity')">-->
					<PositionMaster>
						<!--   Fund -->
						<xsl:choose>
							<xsl:when test ="COL1='038C25676'">
								<FundName>
									<xsl:value-of select="'SC INTERNATIONAL FUND'"/>
								</FundName>
							</xsl:when>
							<xsl:when test ="COL1='038C25502'">
								<FundName>
									<xsl:value-of select="'SC OPPORTUNITY FUND'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>					

						<!--  Symbol Region -->
						<Symbol>
							<xsl:value-of select="translate(COL3, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
						</Symbol>

						<xsl:choose>
							<xsl:when test ="COL7='MSPB'">
								<CounterPartyID>
									<xsl:value-of select ="'1'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:when test ="COL7='CUTE'">
								<CounterPartyID>
									<xsl:value-of select ="'2'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:when test ="COL7='BERN'">
								<CounterPartyID>
									<xsl:value-of select ="'5'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:when test ="COL7='WEEE'">
								<CounterPartyID>
									<xsl:value-of select ="'6'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:when test ="COL7='WTCO'">
								<CounterPartyID>
									<xsl:value-of select ="'7'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select ="'-1'"/>
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
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varSide='L'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='S'">
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

						<!-- Column 8 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when  test="COL8 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL8*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0 or COL8 = 0">
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

						<!--  Average Price Region -->
						<xsl:choose>
							<xsl:when  test="boolean(number(COL15))">
								<CostBasis>
									<xsl:value-of select="COL15"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<PositionStartDate>
							<xsl:value-of select="COL6"/>
						</PositionStartDate>
					</PositionMaster>
					<!--</xsl:if>-->
				</xsl:for-each>
			</DocumentElement>
		</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>

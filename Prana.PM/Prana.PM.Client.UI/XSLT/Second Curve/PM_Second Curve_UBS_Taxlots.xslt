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
					<xsl:value-of select="translate(COL4,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="($varInstrumentType !='Cash')">
					<PositionMaster>
						<!--   Fund -->
						<xsl:choose>
							<xsl:when test ="COL1='3167982'">
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:when>							
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>					

						<!--  Symbol Region -->
						<xsl:choose>
							<xsl:when test ="$varInstrumentType='Equity'">
								<Symbol>
									<xsl:value-of select="translate(COL5, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='Option'">
								<xsl:variable name="varAfterQ" >
									<xsl:value-of select="translate(substring-after(COL5,'Q'),' ','')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterQ)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="translate(COL5, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>


						<!--<xsl:choose>
							<xsl:when test ="COL7='BTIG'">
								<CounterPartyID>
									<xsl:value-of select ="'1'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:when test ="COL7='MSPB'">
								<CounterPartyID>
									<xsl:value-of select ="'46'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:when test ="COL7='BASF'">
								<CounterPartyID>
									<xsl:value-of select ="'47'"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<CounterPartyID>
									<xsl:value-of select ="'0'"/>
								</CounterPartyID>
							</xsl:otherwise>
						</xsl:choose>-->

						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="translate(COL5,'&quot;','')"/>
						</PBSymbol>

						<PBAssetType>
							<xsl:value-of select="$varInstrumentType"/>
						</PBAssetType>

						<!-- Column 4 mapped with Side-->
						<xsl:variable name = "varSide" >
							<xsl:value-of select="translate(COL6,'&quot;','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varSide='Long Positions'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='Short Positions'">
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

						<!-- Column 10 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when  test="COL10 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL10*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL10 &gt; 0 or COL10 = 0">
								<NetPosition>
									<xsl:value-of select="COL10"/>
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
							<xsl:when  test="boolean(number(COL13))">
								<CostBasis>
									<xsl:value-of select="COL13"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<PositionStartDate>
							<xsl:value-of select="COL9"/>
						</PositionStartDate>
					</PositionMaster>
					</xsl:if>
				</xsl:for-each>
			</DocumentElement>
		</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>

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
					<xsl:value-of select="translate(translate(COL16,' ',''),'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="($varInstrumentType='Equities')">
					<PositionMaster>
						<!--   Fund -->
						<!--
						-->
						<!-- Column 2 mapped with Fund-->
						<!--
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='038C54502'">
								<FundName>
									<xsl:value-of select="'LP C/O'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >-->

						<FundName>
							<xsl:value-of select="''"/>
						</FundName>
						<!--  Symbol Region -->
						<Symbol>
							<xsl:value-of select="translate(COL3, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
						</Symbol>

						<!--<xsl:choose>
							<xsl:when test ="COL60='MS'">
								<CounterPartyID>
									<xsl:value-of select ="1"/>
								</CounterPartyID>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>-->


						<!-- Symbol Region Ends -->

						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</PBSymbol>

						<PBAssetType>
							<xsl:value-of select="$varInstrumentType"/>
						</PBAssetType>

						<!-- Column 29 mapped with Side-->
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

						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when  test="COL5 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL5*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL5 &gt; 0 or COL5 = 0">
								<NetPosition>
									<xsl:value-of select="COL5"/>
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
							<xsl:when  test="boolean(number(COL34)) and boolean(number(COL5)) and COL5 != 0 and COL5 != '*' ">
								<CostBasis>
									<xsl:value-of select="COL34 div COL5"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<!-- Position Date mapped with the column 1 -->
						<PositionStartDate>
							<xsl:value-of select="translate(COL51,'&quot;','')"/>
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

<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL21, ' ' , ''),'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType='EquityOptions' or $varInstrumentType='CommonStock' or $varInstrumentType='FXForward'">
				<!--<xsl:if test="$varInstrumentType='FXForward'">-->

					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='038C35642'">
								<FundName>
									<xsl:value-of select="'LP C/O'"/>
								</FundName>
							</xsl:when>
							<xsl:when  test="$varPortfolioID='038C35741'">
								<FundName>
									<xsl:value-of select="'OFFSHORE'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="' '"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >

						<!--  Symbol Region -->

						<!--<Symbol>
							<xsl:value-of select="Specify Column Name"/>
						</Symbol>-->

						<!-- Symbol Region Ends -->

						<!--   CUSIP -->
						<!-- Column 2 mapped with CUSIP-->
						<CUSIP>
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</CUSIP>

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
							<xsl:when test="$varSide='L' and COL8 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='L' and COL8 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>								
							</xsl:when>
							<xsl:when test="$varSide='S' and COL8 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'B'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="$varSide='S' and COL8 &lt; 0">
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
							<xsl:when test="COL8 &lt; 0">
								<NetPosition>
									<xsl:value-of select="COL8*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL8 &gt; 0">
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
						<!--<xsl:if test="$varInstrumentType='FXForward'">
							<xsl:variable name = "varSymbol" >
								<xsl:value-of select="substring(COL20,0,11)"/>
							</xsl:variable>
							<Symbol>
								<xsl:value-of select="translate(translate($varSymbol, ' ' , ''),'TO','-')"/>
							</Symbol>
						</xsl:if>-->
						<!--  Average Price Region -->
						<xsl:choose>
							<xsl:when test="$varInstrumentType='EquityOptions' or $varInstrumentType='CommonStock'">
								<xsl:choose>
									<xsl:when test ="COL15 &lt; 0 or COL15 &gt; 0 or COL15 = 0">
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
							</xsl:when>
							<xsl:when test="$varInstrumentType='FXForward'">
								<xsl:variable name = "varMDIndicator" >
									<xsl:value-of select="translate(COL23, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</xsl:variable>
								<xsl:choose>
									<xsl:when test="$varMDIndicator='M'">
										<CostBasis>
											<xsl:value-of select="COL22"/>
										</CostBasis>
									</xsl:when>
									<xsl:when test="$varMDIndicator='D'">
										<CostBasis>
											<xsl:value-of select="1 div COL22"/>
										</CostBasis>
									</xsl:when>
									<xsl:otherwise>
										<CostBasis>
											<xsl:value-of select="0"/>
										</CostBasis>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>
						
						
						<xsl:if test="$varInstrumentType='EquityOptions' or $varInstrumentType='CommonStock'">
							<CostBasis>
								<xsl:value-of select="COL15"/>
							</CostBasis>
						</xsl:if>
						<xsl:if test="$varInstrumentType='FXForward'">
							<xsl:variable name = "varMDIndicator" >
								<xsl:value-of select="translate(COL23, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
							</xsl:variable>
							<xsl:if test="$varMDIndicator='M'">
								<CostBasis>
									<xsl:value-of select="COL22"/>
								</CostBasis>
							</xsl:if>
							<xsl:if test="$varMDIndicator='D'">
								<CostBasis>
									<xsl:value-of select="1 div COL22"/>
								</CostBasis>
							</xsl:if>
						</xsl:if>

						<!--<CostBasis>
							<xsl:value-of select="Specify Column Name"/>
						</CostBasis>-->

						<!-- Symbol Region  -->

						<SEDOL>
							<xsl:value-of select="translate(COL19,'&quot;','')"/>
						</SEDOL>

						<!-- Position Date mapped with the column 6 -->
						<PositionStartDate>
							<xsl:value-of select="translate(COL6,'&quot;','')"/>
						</PositionStartDate>
						<xsl:if test="$varInstrumentType='EquityOptions'">
							<!--<xsl:variable name = "varBefSym" >
								<xsl:value-of select="substring-before(COL3,'/')"/>
							</xsl:variable>
							<xsl:variable name = "varAftSym" >
								<xsl:value-of select="substring-after(COL3,'/')"/>
							</xsl:variable>
							<Symbol>
								<xsl:value-of select="translate(concat($varBefSym,' ',$varAftSym),'&quot;','')"/>
							</Symbol>-->
							<Symbol>
								<xsl:value-of select="translate(translate(COL3, '/' , ' '),'&quot;','')"/>
							</Symbol>
						</xsl:if>
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

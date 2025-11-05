<?xml version="1.0" encoding="utf-8"?>
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
					<xsl:value-of select="COL11"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType='50' or $varInstrumentType='60'">
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 2 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='11817245' or $varPortfolioID='11802555'">
								<FundName>
									<xsl:value-of select="'LETRP.11817245'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802079'">
								<FundName>
									<xsl:value-of select="'LETRP.11802079(HOT)'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802597'">
								<FundName>
									<xsl:value-of select="'LETRP2.11802597'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802852'">
								<FundName>
									<xsl:value-of select="'LETRP2.11802852(HOT)'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="' '"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >

						<!-- Prime Broker Symbol -->
						<PBSymbol>
							<xsl:value-of select="translate(COL27,'&quot;','')"/>
						</PBSymbol>
						<xsl:if test ="COL11='1'">
							<PBAssetType>
								<xsl:value-of select="'Cash and cash equivalents'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='10'">
							<PBAssetType>
								<xsl:value-of select="'Bonds'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='50'">
							<PBAssetType>
								<xsl:value-of select="'Equities'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='55'">
							<PBAssetType>
								<xsl:value-of select="'Private Investments'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='60'">
							<PBAssetType>
								<xsl:value-of select="'Options'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='70'">
							<PBAssetType>
								<xsl:value-of select="'Futures'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='90'">
							<PBAssetType>
								<xsl:value-of select="'Programs'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='99'">
							<PBAssetType>
								<xsl:value-of select="'Indexes'"/>
							</PBAssetType>
						</xsl:if >
						<!-- Column 7 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when test ="COL7 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL7*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test ="COL7 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>									
									<xsl:value-of select="COL7"/>
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
							<xsl:when test ="COL7 != 0 and COL8 != 0">
								<CostBasis>
									<xsl:value-of select="COL8 div COL7"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>
						<!--<CUSIP>
							<xsl:value-of select="translate(COL9,'&quot;','')"/>
						</CUSIP>
						<ISIN>
							<xsl:value-of select="translate(COL23,'&quot;','')"/>
						</ISIN>
						<SEDOL>
							<xsl:value-of select="translate(COL24,'&quot;','')"/>
						</SEDOL>-->
						
						<!-- Position Date mapped with the column 4 -->
						<xsl:variable name = "varYR" >
							<xsl:value-of select="translate(substring(COL4,1,4),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varMth" >
							<xsl:value-of select="translate(substring(COL4,5,2),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varDt" >
							<xsl:value-of select="translate(substring(COL4,7,2),'&quot;','')"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="translate(concat($varYR,'/',$varMth,'/',$varDt),'&quot;','')"/>
						</PositionStartDate>
						<!-- for Options -->

						<xsl:variable name="varForBBCode" >
							<xsl:value-of select="substring-before(COL27,'EQUITY')"/>
						</xsl:variable>
						<xsl:if test ="COL11='50'">
							<Bloomberg>
								<xsl:value-of select="$varForBBCode"/>
								<!--<xsl:value-of select="substring-before(translate(COL27,'&quot;',''),'EQUITY'"/>-->
							</Bloomberg>
							<Symbol>
								<xsl:value-of select ="translate(COL32,' ','')"/>
							</Symbol>
						</xsl:if >
						<xsl:if test ="COL11='60'">
							<Symbol>
								<xsl:value-of select ="translate($varForBBCode,'+',' ')"/>
							</Symbol>							
						</xsl:if >
							
						<!-- length of Option Symbol--><!--
							<xsl:variable name = "varLength" >
								<xsl:value-of select="string-length(COL5)"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test ="$varLength=6">
									<xsl:variable name="varLen5" >
										<xsl:value-of select="substring(COL5,2,$varLength)"/>
									</xsl:variable>
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring($varLen5,($varLength)-2,2)"/>
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring($varLen5,1,($varLength)-3)"/>
									</xsl:variable>
									<Symbol>
										<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
									</Symbol>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring(COL5,($varLength)-1,2)"/>
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring(COL5,1,($varLength)-2)"/>
									</xsl:variable>
									<Symbol>
										<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
									</Symbol>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if >-->
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

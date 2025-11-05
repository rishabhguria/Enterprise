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
				<PositionMaster>
					<!--   Fund -->
					<!-- Column 1 mapped with Fund-->
					<xsl:variable name = "varPortfolioID" >
						<xsl:value-of select="translate(COL1,'&quot;','')"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$varPortfolioID='4RWX0409' or $varPortfolioID = '4RWXF909' or $varPortfolioID = '4RWX1209' or $varPortfolioID = '4RWXF919'">
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

					<PositionStartDate>
						<xsl:value-of select="translate(COL2,'&quot;','')"/>
					</PositionStartDate>

					<xsl:choose>
						<xsl:when test ="COL3 &gt; 0">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
							<NetPosition>
								<xsl:value-of select="COL3"/>
							</NetPosition>
						</xsl:when>
						<xsl:when test ="COL3 &lt; 0">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
							<NetPosition>
								<xsl:value-of select="COL3 * (-1)"/>
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

					<xsl:variable name = "varInstrumentType" >
						<xsl:value-of select="translate(translate(COL15, ' ' , ''),'&quot;','')"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test ="$varInstrumentType='EQUITY'">
							<Symbol>
								<xsl:value-of select="translate(COL12,'&quot;','')"/>
							</Symbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='OPTION'">
							<Symbol>
								<xsl:value-of select="translate(COL14,'&quot;','')"/>
							</Symbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='FUTURE'">
							<xsl:variable name = "varLength" >
								<xsl:value-of select="string-length(translate(translate(COL13,'&quot;',''),' ',''))"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test ="$varLength &gt; 0 ">
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring(COL13,($varLength)-1,2)"/>
										<!--<xsl:value-of select="'After'"/>-->
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring(COL13,1,($varLength)-2)"/>
										<!--<xsl:value-of select="'Before'"/>-->
									</xsl:variable>
									<Symbol>
										<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
									</Symbol>
								</xsl:when>
								<xsl:otherwise>
									<Symbol>
										<xsl:value-of select="''"/>
									</Symbol>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose>

					<PBAssetType>
						<xsl:value-of select="$varInstrumentType"/>
					</PBAssetType>
					<xsl:choose>
						<xsl:when test ="$varInstrumentType='EQUITY'">
							<PBSymbol>
								<xsl:value-of select="translate(COL12,'&quot;','')"/>
							</PBSymbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='OPTION'">
							<PBSymbol>
								<xsl:value-of select="translate(COL14,'&quot;','')"/>
							</PBSymbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='FUTURE'">
							<PBSymbol>
								<xsl:value-of select="COL13"/>
							</PBSymbol>
						</xsl:when>
						<xsl:otherwise>
							<PBSymbol>
								<xsl:value-of select="COL12"/>
							</PBSymbol>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="COL5 &lt; 0 or COL5 &gt; 0 or COL5=0">
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

				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

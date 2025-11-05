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
				<xsl:if test ="COL1='POST'">
					<PositionMaster>
						<!--   Fund -->
						
						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

						<PositionStartDate>
							<xsl:value-of select="translate(COL8,'&quot;','')"/>
						</PositionStartDate>
						<!--<AUECLocalDate>
							<xsl:value-of select="translate(COL8,'&quot;','')"/>
						</AUECLocalDate>-->

						<xsl:choose>
							<xsl:when test ="COL22='Short'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>								
							</xsl:when>
							<xsl:when test ="COL22='Long'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>								
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>								
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="COL13 &lt; 0 ">
								<NetPosition>
									<xsl:value-of select="COL13 * (-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test ="COL13 &gt; 0 or COL13=0">
								<NetPosition>
									<xsl:value-of select="COL13"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>						
						
						<xsl:variable name = "varInstrumentType" >
							<xsl:value-of select="translate(translate(COL5, ' ' , ''),'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test ="$varInstrumentType='STK'">
								<Symbol>
									<xsl:value-of select="translate(COL6,'&quot;','')"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='OPT'">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL6)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL6,1,($varLength - 2)),' ',substring(COL6,($varLength - 1),$varLength))"/>
								</Symbol>
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

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL18))">
								<CostBasis>
									<xsl:value-of select="COL18"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

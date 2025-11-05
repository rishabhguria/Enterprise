<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL5,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="$varInstrumentType='EQUITY' or $varInstrumentType='OPTION' or $varInstrumentType='CFD' ">
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 2 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varPortfolioID='LUCAS TOP'">
								<FundName>
									<xsl:value-of select="'LR F Dom'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='000000005296005'">
								<FundName>
									<xsl:value-of select="'LR Dom'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="'LR Dom '"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >

						<PBAssetType>
							<xsl:value-of select="translate(COL5,'&quot;','')"/>
						</PBAssetType>

						<!-- Column 5 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when test="COL10 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
								<NetPosition>
									<!--<xsl:value-of select='format-number(COL17*(-1),"###")'/>-->
									<xsl:value-of select="COL10*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL10 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL10"/>
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
							<xsl:when test ="COL21 &lt; 0 or COL21 &gt; 0 or COL21 = 0">
								<CostBasis>
									<xsl:value-of select="COL21"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>


						<!-- Position Date mapped with the column 16 -->
						<PositionStartDate>
							<xsl:value-of select="translate(COL16,'&quot;','')"/>
						</PositionStartDate>

						<!-- Prime Broker Symbol -->
						<xsl:if test="$varInstrumentType='EQUITY' or $varInstrumentType='CFD' ">
							<Symbol>
								<xsl:value-of select="translate(COL19,'&quot;','')"/>
							</Symbol>
							<PBSymbol>
								<xsl:value-of select="translate(COL19,'&quot;','')"/>
							</PBSymbol>
						</xsl:if >

						<xsl:if test="$varInstrumentType='OPTION'">
							<!-- $PXPHO-->
							<xsl:variable name="varAfterDollar" >
								<xsl:value-of select="substring-after(COL19,'$')"/>
							</xsl:variable>
							<xsl:variable name = "varLength" >
								<xsl:value-of select="string-length($varAfterDollar)"/>
							</xsl:variable>
							<xsl:variable name = "varAfter" >
								<xsl:value-of select="substring($varAfterDollar,($varLength)-1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varBefore" >
								<xsl:value-of select="substring($varAfterDollar,1,($varLength)-2)"/>
							</xsl:variable>

							<Symbol>
								<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
							</Symbol>

							<PBSymbol>
								<xsl:value-of select="translate(COL19,'&quot;','')"/>
							</PBSymbol>
						</xsl:if >
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
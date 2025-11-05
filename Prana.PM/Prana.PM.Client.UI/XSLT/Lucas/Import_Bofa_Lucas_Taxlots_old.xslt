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
					<xsl:value-of select="translate(translate(COL21,' ' , ''),'&quot;','')"/>
				</xsl:variable>

				<PositionMaster>
					<!--   Fund -->
					<!-- Column 1 mapped with Fund-->
					<xsl:variable name = "varPortfolioID" >
						<xsl:value-of select="COL2"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$varPortfolioID='11810000'">
							<FundName>
								<xsl:value-of select="'Bofa_Fund1'"/>
							</FundName>
						</xsl:when>
						<xsl:when test="$varPortfolioID='31354321'">
							<FundName>
								<xsl:value-of select="'Bofa_Fund2'"/>
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
						<xsl:value-of select="translate(translate(COL8,' ' , ''),'&quot;','')"/>
					</PBSymbol>

					<xsl:if test ="$varInstrumentType='NSTK'">
						<PBAssetType>
							<xsl:value-of select="'Equity'"/>
						</PBAssetType>
					</xsl:if >
					<xsl:if test ="$varInstrumentType='COMM'">
						<PBAssetType>
							<xsl:value-of select="'Equity'"/>
						</PBAssetType>
					</xsl:if >
					<xsl:if test ="$varInstrumentType='EQUT'">
						<PBAssetType>
							<xsl:value-of select="'Equity'"/>
						</PBAssetType>
					</xsl:if >
					<xsl:if test ="$varInstrumentType='FSTK'">
						<PBAssetType>
							<xsl:value-of select="'Equity'"/>
						</PBAssetType>
					</xsl:if >
					<xsl:if test ="$varInstrumentType='IACT'">
						<PBAssetType>
							<xsl:value-of select="'Equity'"/>
						</PBAssetType>
					</xsl:if >

					<!--
					<xsl:if test ="COL21='10'">
						<PBAssetType>
							<xsl:value-of select="'Bonds'"/>
						</PBAssetType>
					</xsl:if >

					<xsl:if test ="COL21='50'">
						<PBAssetType>
							<xsl:value-of select="'Equities'"/>
						</PBAssetType>
					</xsl:if >

					<xsl:if test ="COL21='55'">
						<PBAssetType>
							<xsl:value-of select="'Private Investments'"/>
						</PBAssetType>
					</xsl:if >

					<xsl:if test ="COL21='60'">
						<PBAssetType>
							<xsl:value-of select="'Options'"/>
						</PBAssetType>
					</xsl:if >

					<xsl:if test ="COL21='70'">
						<PBAssetType>
							<xsl:value-of select="'Futures'"/>
						</PBAssetType>
					</xsl:if >

					<xsl:if test ="COL21='90'">
						<PBAssetType>
							<xsl:value-of select="'Programs'"/>
						</PBAssetType>
					</xsl:if >

					<xsl:if test ="COL21='99'">
						<PBAssetType>
							<xsl:value-of select="'Indexes'"/>
						</PBAssetType>
					</xsl:if >

					-->
					<!-- Column 9 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
					<xsl:if test="COL9 &lt; 0">
						<SideTagValue>
							<xsl:value-of select="'5'"/>
						</SideTagValue>
						<NetPosition>
							<xsl:value-of select="COL9*(-1)"/>
						</NetPosition>

					</xsl:if>
					<xsl:if test="COL9 &gt; 0">
						<SideTagValue>
							<xsl:value-of select="'1'"/>
						</SideTagValue>
						<NetPosition>
							<xsl:value-of select="COL9"/>
						</NetPosition>

					</xsl:if>
					<CostBasis>
						<xsl:value-of select="COL10"/>
					</CostBasis>

					<CUSIP>
						<xsl:value-of select="COL19"/>
					</CUSIP>
					<ISIN>
						<xsl:value-of select="COL35"/>
					</ISIN>
					<SEDOL>
						<xsl:value-of select="COL36"/>
					</SEDOL>
					<Bloomberg>
						<xsl:value-of select="COL39"/>
					</Bloomberg>
					<!-- Position Date mapped with the column 4 -->
					<xsl:variable name = "varYR" >
						<xsl:value-of select="translate(substring(COL5,1,4),'&quot;','')"/>
					</xsl:variable>
					<xsl:variable name = "varMth" >
						<xsl:value-of select="translate(substring(COL5,5,2),'&quot;','')"/>
					</xsl:variable>
					<xsl:variable name = "varDt" >
						<xsl:value-of select="translate(substring(COL5,7,2),'&quot;','')"/>
					</xsl:variable>
					<PositionStartDate>
						<xsl:value-of select="translate(concat($varYR,'/',$varMth,'/',$varDt),'&quot;','')"/>
					</PositionStartDate>

				</PositionMaster>
				<!--
				</xsl:if>
				-->
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

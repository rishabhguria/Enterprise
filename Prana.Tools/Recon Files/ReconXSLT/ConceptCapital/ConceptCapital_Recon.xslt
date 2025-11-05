<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="Comparision">
				<PositionMaster>
					<!--FundName Section-->
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="translate(COL1,'&quot;','')"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='BOFA']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME=''">
							<FundName>
								<xsl:value-of select='$PB_FUND_NAME'/>
							</FundName>
						</xsl:when>
						<xsl:otherwise>
							<FundName>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</FundName>
						</xsl:otherwise>
					</xsl:choose>

					
					<!--AssetName Section-->
					<!--<xsl:variable name = "PB_ASSET_NAME" >
						<xsl:value-of select="translate(COL15,'&quot;','')"/>
					</xsl:variable>

					<xsl:variable name="PRANA_ASSET_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='BOFA']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_ASSET_NAME=''">
							<PBAssetName>
								<xsl:value-of select='$PB_ASSET_NAME'/>
							</PBAssetName>
						</xsl:when>
						<xsl:otherwise>
							<PBAssetName>
								<xsl:value-of select='$PRANA_ASSET_NAME'/>
							</PBAssetName>
						</xsl:otherwise>
					</xsl:choose>-->

					<!--Side, SideTagValue and Qunatity Section-->
					<xsl:choose>
						<xsl:when  test="COL2 &lt; 0">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
							<Side>
								<xsl:value-of select="'Sell'"/>
							</Side>
							<Quantity>
								<xsl:value-of select="COL2*(-1)"/>
							</Quantity>
						</xsl:when >
						<xsl:when test="COL2 &gt; 0">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
							<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>
							<Quantity>
								<xsl:value-of select="COL12*(-1)"/>
							</Quantity>
						</xsl:when>
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:variable name = "varInstrumentType" >
						<xsl:value-of select="translate(translate(COL15, ' ' , ''),'&quot;','')"/>
					</xsl:variable>

					<!--Prana Symbol Section -->
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
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring(COL13,1,($varLength)-2)"/>									
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

					<!--Prime broker Asset Type Section -->
					<PBAssetName>
						<xsl:value-of select="$varInstrumentType"/>
					</PBAssetName>

					<!--Prime broker Symbol Section -->
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

					<!--Average Price Section-->
					<CostBasis>
						<xsl:value-of select="COL5"/>
					</CostBasis>
				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>



</xsl:stylesheet>

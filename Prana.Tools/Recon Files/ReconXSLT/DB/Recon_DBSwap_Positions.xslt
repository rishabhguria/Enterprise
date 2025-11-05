<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">				
				<xsl:if test="COL2!='Parent'">
					<PositionMaster>
						<!--Need To Add Fund -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SENSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</FundName>
							</xsl:when>
							
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose >
						<!--  Symbol Region -->

						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>

						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>

						<SEDOL>
							<xsl:value-of select="COL18"/>
						</SEDOL>
						
						<xsl:choose>
							<xsl:when test="COL24='Short'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL24='Long'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="COL23 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL23*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="COL23 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL23"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!--Need To Ask-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL27) &lt; 0 )">
								<AvgPX>
									<xsl:value-of select="COL27 * (-1)"/>
								</AvgPX>
							</xsl:when>
							<xsl:when test ="boolean(number(COL27) &gt; 0 )">
								<AvgPX>
									<xsl:value-of select="COL27"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>																								
							
						<!--AssetName section-->
						<!--<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='Abundance']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
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
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

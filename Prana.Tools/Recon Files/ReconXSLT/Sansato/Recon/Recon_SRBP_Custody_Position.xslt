<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="COL1='Synthetic'">
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
						</xsl:choose>
						
						<!--  Symbol Region -->
						
						<Symbol>
							<xsl:value-of select="''"/>
						</Symbol>
						
						<PBSymbol>
							<xsl:value-of select="''"/>
						</PBSymbol>
						
						<SEDOL>
							<xsl:value-of select="substring-before(COL4,'CFDUSD')"/>
						</SEDOL>
						
						<xsl:choose>
							<xsl:when test="contains(COL6,'(') !=false  or contains(COL6,'-') !=false">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="contains(COL6,'(') =false  or contains(COL6,'-') =false">
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
							<xsl:when test="contains(COL6,'(') !=false  or contains(COL6,'-') !=false">
								<Quantity>
									<xsl:value-of select="COL6*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="contains(COL6,'(') =false  or contains(COL6,'-') =false">
								<Quantity>
									<xsl:value-of select="COL6"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="contains(COL7,'(') !=false  or contains(COL7,'-') !=false">
								<AvgPX>
									<xsl:value-of select="COL7 *(-1)"/>
								</AvgPX>
							</xsl:when>
							<xsl:when test ="contains(COL7,'(') =false  or contains(COL7,'-') =false">
								<AvgPX>
									<xsl:value-of select="COL7 *(-1)"/>
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

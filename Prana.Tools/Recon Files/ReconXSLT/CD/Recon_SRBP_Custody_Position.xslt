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
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
						<xsl:choose>
							<xsl:when test ="$PB_FUND_NAME= 'Sensato AP Master'">
								<xsl:value-of select ="'013-425764-SWAP'"/>
							</xsl:when>
							<xsl:when test ="$PB_FUND_NAME= 'Sensato S1 AP'">
								<xsl:value-of select ="'S1_013-101969_SWAP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:otherwise>
						</xsl:choose>													
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</AccountName>
							</xsl:when>

							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</AccountName>
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

						<!--<xsl:choose>
							<xsl:when test="contains(COL6,'(') !=false  or contains(COL6,'-') !=false">
								<Quantity>
									<xsl:value-of select="COL6"/>
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
						</xsl:choose>-->
						<xsl:choose>
							<xsl:when test ="number(COL6)">
								<Quantity>
									<xsl:value-of select ="COL6"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
								<xsl:value-of select ="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="contains(COL7,'(') !=false  or contains(COL7,'-') !=false">
								<MarkPrice>
									<xsl:value-of select="COL7 *(-1)"/>
								</MarkPrice>
							</xsl:when>
							<xsl:when test ="contains(COL7,'(') =false  or contains(COL7,'-') =false">
								<MarkPrice>
									<xsl:value-of select="COL7 *(-1)"/>
								</MarkPrice>

							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>

							</xsl:otherwise>
						</xsl:choose>
						
						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>
					
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

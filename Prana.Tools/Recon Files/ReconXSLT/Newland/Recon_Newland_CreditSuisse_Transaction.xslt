<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL18 != 0 and COL12 = 'STOCK' and COL12 = 'OPTION' ">
					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Newland_CS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>


						<!--fundname section-->

						<!--<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='GS']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>-->

						<!--<xsl:choose>
							<xsl:when test="$PB_ASSET_NAME='WARRANTS'">
								<PBAssetName>
									<xsl:value-of select="'EQUITY'"/>
								</PBAssetName>
							</xsl:when>
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
						<PBAssetName>
							<xsl:value-of select="COL12"/>
						</PBAssetName>

						<xsl:choose>
							<xsl:when test ="COL18 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL18*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL18 &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL18"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL19))">
								<AvgPX>
									<xsl:value-of select="COL19"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL11,'&quot;','')"/>

						<PBSymbol>
							<xsl:value-of select="COL9"/>
						</PBSymbol>

						<!--<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Newland_GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>-->
						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<!--<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select='$PRANA_SYMBOL_NAME'/>
								</Symbol>
							</xsl:when>
							<xsl:when test="starts-with(COL5,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL5)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL5,2,($varLength - 3)),' ',substring(COL5,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select='COL5'/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>-->

						
						
						<xsl:choose>
							<xsl:when test ="COL12='STOCK'">
								<Symbol>
									<xsl:value-of select="COL9"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="COL12='OPTION'">
								<xsl:variable name = "varBeforeDot" >
									<xsl:value-of select="substring-before(COL9,'.')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varBeforeDot)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varBeforeDot,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varBeforeDot,1,($varLength)-2)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
							</xsl:when >
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL9"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>				
						

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>

<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test ="COL1 != 'account' and normalize-space(COL7) != normalize-space(COL8)">
					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varSymbol">
							<xsl:value-of select ="normalize-space(substring(COL6,2))"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Jefferies']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<xsl:when test ="COL8 = 'Options'">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(substring(COL1,1,21),'U')"/>
								</IDCOOptionSymbol>
							</xsl:when>
							<!--<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="starts-with(COL6,'Q') and $varSymbol != 'QQ'">
										<xsl:variable name = "varLength" >
											<xsl:value-of select="string-length(normalize-space(COL6))"/>
										</xsl:variable>
										<Symbol>
											<xsl:value-of select="concat(substring(COL6,2,($varLength - 3)),' ',substring(COL6,($varLength - 1),$varLength))"/>
										</Symbol>
									</xsl:when>-->
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="COL6"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
							<!--</xsl:choose>
							</xsl:otherwise>-->
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<xsl:variable name ="varSecurity">
							<xsl:value-of select ="normalize-space(COL8)"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="COL11 &lt; 0 and $varSecurity!='Options'">
								<Quantity>
									<xsl:value-of select="COL11*(1)"/>
								</Quantity>
								<Side>
									<xsl:value-of select="'Sell Short'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL11 &gt; 0 and $varSecurity!='Options'">
								<Quantity>
									<xsl:value-of select="COL11"/>
								</Quantity>
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							
							<xsl:when test="COL11 &gt; 0 and $varSecurity='Options'">
								<Quantity>
									<xsl:value-of select="COL11"/>
								</Quantity>
								<Side>
									<xsl:value-of select="'Buy to Open'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL11 &lt; 0 and $varSecurity='Options'">
								<Quantity>
									<xsl:value-of select="COL11"/>
								</Quantity>
								<Side>
									<xsl:value-of select="'Sell to open'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL12))">
								<AvgPX>
									<xsl:value-of select="COL12"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test ="boolean(number(COL15))">
								<MarketValue>
									<xsl:value-of select="COL15"/>
								</MarketValue>
							</xsl:when>
							<xsl:otherwise>
								<MarketValue>
									<xsl:value-of select="0"/>
								</MarketValue>
							</xsl:otherwise>
						</xsl:choose>

						<CompanyName>
							<xsl:value-of select="COL9"/>
						</CompanyName>

						<!--AssetName section-->
						<!--<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='SCUBS']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>-->
						<PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>

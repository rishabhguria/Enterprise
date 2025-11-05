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
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select="normalize-space(COL20)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:choose>
								<!--<xsl:when test ="$PB_FUND_NAME='32577'">
									<xsl:value-of select ="'S1_32577_SWAP'"/>
								</xsl:when>-->
								<xsl:when test="$PB_FUND_NAME='31291' or $PB_FUND_NAME='32577'">
									<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME and @Currency = $PB_CURRENCY_NAME]/@PranaFund"/>
								</xsl:otherwise>
							</xsl:choose>
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
						</xsl:choose >
						<!--SYMBOL-->


						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL15)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/Recon_SymbolMapping.xml')/SymbolMapping/PB[@Name='Sensato_DB']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL15"/>
								</PBSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:when>
							<xsl:when test="COL18 != ''">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL15"/>
								</PBSymbol>
								<SEDOL>
									<xsl:value-of select="COL18"/>
								</SEDOL>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL15"/>
								</PBSymbol>
								<SEDOL>
									<xsl:value-of select="''"/>
								</SEDOL>
							</xsl:otherwise>
						</xsl:choose>

						
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
							<xsl:when test="number(COL23)">
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
							<xsl:when test ="boolean(number(COL29) &lt; 0 )">
								<MarkPrice>
									<xsl:value-of select="COL29 * (-1)"/>
								</MarkPrice>
							</xsl:when>
							<xsl:when test ="boolean(number(COL29) &gt; 0 )">
								<MarkPrice>
									<xsl:value-of select="COL29"/>
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

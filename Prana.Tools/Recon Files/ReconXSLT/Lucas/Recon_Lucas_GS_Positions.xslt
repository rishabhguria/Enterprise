<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:variable name = "PB_ASSET_NAME" >
					<xsl:value-of select="translate(COL5,'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$PB_ASSET_NAME != 'CASH AND FX-CONTRACTS' and translate(COL2,'&quot;','') != 'Fund' and COL10 != '' and COL10 != 0 and contains(COL7, 'EUROPEAN OPTION') = false">

					<PositionMaster>

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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


						<!--fundname section-->



						<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='GS']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>

						<xsl:choose>
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
						</xsl:choose>




						<xsl:if test="COL10 &lt; 0">
							<Side>
								<xsl:value-of select="'Sell'"/>
							</Side>
						</xsl:if>
						<xsl:if test="COL10 &gt; 0">
							<Side>
								<xsl:value-of select="'Buy'"/>
							</Side>
						</xsl:if>
						<xsl:if test="COL10 &lt; 0">
							<Quantity>
								<xsl:value-of select="COL10*(-1)"/>
							</Quantity>
						</xsl:if>
						<xsl:if test="COL10 &gt; 0">
							<Quantity>
								<xsl:value-of select="COL10"/>
							</Quantity>
						</xsl:if>

						<AvgPX>
							<xsl:value-of select="COL11"/>
						</AvgPX>

						<!-- Symbol Section-->
						<xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>

						<PBSymbol>
							<xsl:value-of select="COL8"/>
						</PBSymbol>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<CompanyName>
							<xsl:value-of select='COL7'/>
						</CompanyName>
						<xsl:choose>

							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select='$PRANA_SYMBOL_NAME'/>
								</Symbol>
							</xsl:when>

							<xsl:when test="starts-with(COL8,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL8)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL8,2,($varLength - 3)),' ',substring(COL8,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>


							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select='COL8'/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
						<!-- Symbol Section ends-->
						<Commissions>
							<xsl:value-of select="COL6"/>
						</Commissions>

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>

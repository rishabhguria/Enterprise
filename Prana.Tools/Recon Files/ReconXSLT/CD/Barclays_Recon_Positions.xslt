<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test ="COL2 != 'REPORT RUN' and COL2 != 'distribution_id' and COL5 != ''">				
				<PositionMaster>

					<!--fundname section-->
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="translate(COL3,'&quot;','')"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='BARCLAYS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_FUND_NAME=''">
							<AccountName>
								<xsl:value-of select='$PB_FUND_NAME'/>
							</AccountName>
						</xsl:when>
						<xsl:otherwise>
							<AccountName>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</AccountName>

						</xsl:otherwise>
					</xsl:choose>


					<!--fundname section-->

					<!--<xsl:variable name = "PB_ASSET_NAME" >
						<xsl:value-of select="translate(COL5,'&quot;','')"/>
					</xsl:variable>

					<xsl:variable name="PRANA_ASSET_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='BARCLAYS']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
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
					
					<xsl:if test="COL15 &lt; 0">
						<Side>
							<xsl:value-of select="'Sell'"/>
						</Side>
						<Quantity>
							<xsl:value-of select="COL15*(-1)"/>
						</Quantity>
					</xsl:if>
					<xsl:if test="COL15 &gt; 0">
						<Side>
							<xsl:value-of select="'Buy'"/>
						</Side>
						<Quantity>
							<xsl:value-of select="COL15"/>
						</Quantity>
					</xsl:if>

					<AvgPX>
						<xsl:value-of select="COL14"/>
					</AvgPX>


						<!-- Symbol Section-->
					<xsl:variable name="PB_COMPANY_NAME" select="translate(COL12,'&quot;','')"/>

					<PBSymbol>
							<xsl:value-of select="COL7"/>
						</PBSymbol>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BARCLAYS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<CompanyName>
							<xsl:value-of select='COL12'/>
						</CompanyName>
						<xsl:choose>

							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<Symbol>
									<xsl:value-of select='$PRANA_SYMBOL_NAME'/>
								</Symbol>
							</xsl:when>
							
							<xsl:when test="starts-with(COL7,'$')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(COL7)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(COL7,2,($varLength - 3)),' ',substring(COL7,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
							
							
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select='COL7'/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
					<!-- Symbol Section ends-->

				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>

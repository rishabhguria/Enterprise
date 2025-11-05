<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">

		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="Comparision">


				<PositionMaster>
					<!--fundname section-->
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="translate(col001,'&quot;','')"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/FundData[@GSFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
					<Samplecol>
						<xsl:value-of select="col000"/>
					</Samplecol>

					<xsl:variable name = "PB_ASSET_NAME" >
						<xsl:value-of select="translate(col010,'&quot;','')"/>
					</xsl:variable>

					<xsl:variable name="PRANA_ASSET_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/AssetData[@GSAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
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
					</xsl:choose>

					<xsl:if test="col006 &lt; 0">
						<SideTagValue>
							<xsl:value-of select="'5'"/>
						</SideTagValue>
						<Side>
							<xsl:value-of select="'Sell'"/>
						</Side>
						<Quantity>
							<xsl:value-of select="round(col006*(-1))"/>
						</Quantity>
					</xsl:if>
					<xsl:if test="col006 &gt; 0">
						<SideTagValue>
							<xsl:value-of select="'1'"/>
						</SideTagValue>
						<Side>
							<xsl:value-of select="'Buy'"/>
						</Side>
						<Quantity>
							<xsl:value-of select="round(col006)"/>
						</Quantity>
					</xsl:if>

					<xsl:variable name="PB_COMPANY_NAME" select="translate(col012,'&quot;','')"/>
					<PBSymbol>
						<xsl:value-of select="col004"/>
					</PBSymbol>

					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
					</xsl:variable>
					<CompanyName>
						<xsl:value-of select='col012'/>
					</CompanyName>
					<xsl:choose>
						<xsl:when test="$PRANA_SYMBOL_NAME!=''">
				<Symbol>
								<xsl:value-of select='$PRANA_SYMBOL_NAME'/>
							</Symbol>

						</xsl:when>
						<xsl:when test="starts-with(col004,'Q')">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(col004)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat(substring(col004,2,($varLength - 3)),' ',substring(col004,($varLength - 1),$varLength))"/>
								</Symbol>
							</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select='col004'/>
							</Symbol>
						</xsl:otherwise>
						
							
							
					</xsl:choose>
					<xsl:if test="col006 != 0 and col007 != 0">
						<CostBasis>
							<!--<xsl:value-of select="col007 div col006"/>-->
							<xsl:value-of select='format-number(col007 div col006,"###.00")'/>
						</CostBasis>
					</xsl:if >
				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>



</xsl:stylesheet>

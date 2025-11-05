<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<!--COL2 != 'SEDOL'-->
				<xsl:if test ="COL9!='Cash and Equivalents'and number(COL12)">
					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
							<xsl:value-of select ="normalize-space(substring(COL8,2))"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
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
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="COL9 = 'Options' and COL4 != ''">
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="concat(COL2, 'U')"/>
										</IDCOOptionSymbol>
									</xsl:when>
									<xsl:when test ="COL9 = 'Options' and COL4 = ''">
										<Symbol>
											<xsl:value-of select="concat(substring(COL3,1,2), ' ',substring(COL3,3,2),substring(COL6,1,1), number(substring(COL5,4))*1000)"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:when>
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="COL7"/>
										</Symbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

            <Quantity>
              <xsl:value-of select="COL12"/>
            </Quantity>

            <xsl:choose>
              <xsl:when test="COL12 &lt; 0">
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
              </xsl:when>
              <xsl:when test="COL12 &gt; 0">
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
							<xsl:when test ="boolean(number(COL15))">
								<MarkPrice>
									<xsl:value-of select="COL15"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

						<CompanyName>
							<xsl:value-of select="COL6"/>
						</CompanyName>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="COL16 &lt; 0">
                  <xsl:value-of select="COL16"/>
                </xsl:when>
                <xsl:when test="COL16 &gt; 0">
                    <xsl:value-of select="COL16"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test="COL20 &lt; 0">
                  <xsl:value-of select="COL20"/>
                </xsl:when>
                <xsl:when test="COL20 &gt; 0">
                  <xsl:value-of select="COL20"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>

						<!--AssetName section-->
						<!--<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='SCUBS']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>-->
						<PBAssetName>
							<xsl:value-of select="COL9"/>
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

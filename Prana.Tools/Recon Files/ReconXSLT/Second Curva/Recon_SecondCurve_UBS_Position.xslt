<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL2,'&quot;',''),' ','')"/>
				</xsl:variable>

				<xsl:if test="($varInstrumentType != 'Cash' and COL1 != 'Account Name')">
					<PositionMaster>
						<!-- Symbol Section-->
						<!--<xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>
			  
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>-->


						<!--  Symbol Region -->
						<xsl:choose>
							<xsl:when test ="$varInstrumentType='Equity'">
								<Symbol>
									<xsl:value-of select="translate(COL5, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL5,'&quot;','')"/>
								</PBSymbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='Option'">
								<xsl:variable name="varAfterQ" >
									<xsl:value-of select="translate(substring-after(COL5,'Q'),' ','')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterQ)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL5,'&quot;','')"/>
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="translate(COL5, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL5,'&quot;','')"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>


						<!-- Column 4 mapped with Side-->
						<xsl:variable name = "varSide" >
							<xsl:value-of select="translate(COL23,'&quot;','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varSide='S'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='L'">
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
							<xsl:when test="COL7 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL7*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="COL7 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL7"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL8))">
								<MarkPrice>
									<xsl:value-of select="COL8"/>
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

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,' ','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='UBS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<!--AssetName section-->
						<!--<xsl:variable name="PRANA_ASSET_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='SCUBS']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
						</xsl:variable>-->
						<PBAssetName>
							<xsl:value-of select='$varInstrumentType'/>
						</PBAssetName>
						<MarketValue>
							<xsl:value-of select="COL14"/>
						</MarketValue>
						<MarketValueBase>
							<xsl:value-of select="COL15"/>
						</MarketValueBase>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->

</xsl:stylesheet>

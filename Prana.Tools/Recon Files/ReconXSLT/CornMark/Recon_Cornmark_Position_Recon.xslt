<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="PositionMaster">
				<!--<xsl:variable name = "PB_ASSET_NAME" >
					<xsl:value-of select="translate(COL2,'&quot;','')"/>
				</xsl:variable>-->
				<!--<xsl:variable name="MARKET_VALUE" select="translate(translate(COL12,'&quot;',''),' ','')"/>-->

				<!--<xsl:if test="$PB_ASSET_NAME != 'Investment Type' and $PB_ASSET_NAME != 'CURRENCY' and COL8 != 0 and COL8 != 'QUANTITY'">-->
					<PositionMaster>
						<!-- Symbol Section-->
						<!--<xsl:variable name="PB_COMPANY_NAME" select="translate(COL7,'&quot;','')"/>
			  
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>-->

						<!--  Symbol Region -->
						<!--<xsl:variable name = "varInstrumentType" >
							<xsl:value-of select="translate(translate(COL2, ' ' , ''),'&quot;','')"/>
						</xsl:variable>-->

            <!--<xsl:variable name="OptionUnderlyingSymbol">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='OPTION'">
                  <xsl:variable name="OpraCode" select="normalize-space(COL3)"/>
                  <xsl:value-of select="document('../ReconMappingXml/UnderlyingSymbolMapping.xml')/SymbolMapping/PB[@Name='ABUNDANCE']/SymbolData[@OPRASymbol=$OpraCode]/@UnderlyingSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="OptionMonth">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='OPTION'">
                  <xsl:value-of select ="substring(COL5,string-length(COL5) - 1,1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Strike">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='OPTION'">
                  <xsl:variable name ="varStr" select ="normalize-space(substring(COL4,19,11))"/>
                  <xsl:variable name ="varStrikeDecimal" select ="substring-after($varStr,'.')"/>
                  <xsl:variable name ="varStrikeInt" select ="substring-before($varStr,'.')"/>
                  <xsl:choose>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
                      <xsl:value-of select ="concat($varStr,'0')"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
                      <xsl:value-of select ="$varStr"/>
                    </xsl:when>
                    <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
                      <xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select ="concat($varStr,'.00')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="ExpYear">
              <xsl:choose>
                <xsl:when test="$varInstrumentType='OPTION'">
                  <xsl:value-of select ="substring(COL4,17,2)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->
            
            
						<xsl:choose>
							<xsl:when test ="boolean(COL2)">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL2,'&quot;','')"/>
								</PBSymbol>
								<CUSIP>
									<xsl:value-of select="translate(COL2,'&quot;','')"/>
								</CUSIP>
					            </xsl:when>
							<!--<xsl:when test ="$varInstrumentType='OPTION'">
								<Symbol>
									--><!--<xsl:value-of select="translate(COL5,'&quot;','')"/>--><!--
									--><!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>--><!--
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="COL17"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat(COL17,'U')"/>
								</IDCOOptionSymbol>
							</xsl:when>-->
							<!--<xsl:when test ="$varInstrumentType='FUTURE'">
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length(translate(translate(COL6,'&quot;',''),' ',''))"/>
								</xsl:variable>
								<xsl:choose>
									<xsl:when test ="$varLength &gt; 0 ">
										<xsl:variable name = "varAfter" >
											<xsl:value-of select="substring(COL6,($varLength)-1,2)"/>
										</xsl:variable>
										<xsl:variable name = "varBefore" >
											<xsl:value-of select="substring(COL6,1,($varLength)-2)"/>
										</xsl:variable>
										<Symbol>
											<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
										</Symbol>
										<PBSymbol>
											<xsl:value-of select="translate(COL6,'&quot;','')"/>
										</PBSymbol>
										<IDCOOptionSymbol>
											<xsl:value-of select="''"/>
										</IDCOOptionSymbol>
									</xsl:when>-->
									<xsl:otherwise>
										<Symbol>
											<xsl:value-of select="''"/>
										</Symbol>
										<PBSymbol>
											<xsl:value-of select="''"/>
										</PBSymbol>
										<CUSIP>
											<xsl:value-of select="''"/>
										</CUSIP>
						</xsl:otherwise>
								</xsl:choose>
							<!--</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="''"/>
								</PBSymbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>-->


						<xsl:choose>
							<xsl:when test="COL7 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL7 &gt; 0">
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
							<xsl:when test ="boolean(number(COL11))">
								<AvgPX>
									<xsl:value-of select="COL11"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<!--<CompanyName>
              <xsl:value-of select='COL7'/>
            </CompanyName>-->

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CORMARK']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<!--AssetName section--><!--
						<xsl:variable name="PRANA_ASSET_NAME">
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

						<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>

					</PositionMaster>
				<!--</xsl:if>-->
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>

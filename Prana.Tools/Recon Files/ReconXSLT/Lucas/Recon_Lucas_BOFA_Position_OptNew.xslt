<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/DocumentElement">

    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="Comparision">
        <xsl:if test ="COL7 != '' and COL7 != 0 and COL11 != 1 and COL11 != 10 and COL11 != 55 and COL11 != 90 and COL11 != 99">
          <xsl:if test ="COL2 = 11817245 or COL2 = 11802597">
            <PositionMaster>
              <!--FundName Section-->
              <xsl:variable name = "PB_FUND_NAME" >
                <xsl:value-of select="translate(COL2,'&quot;','')"/>
              </xsl:variable>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='BOFA']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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


              <!--Sample Section-->
              <Samplecol>
                <xsl:value-of select="COL1"/>
              </Samplecol>

              <!--AssetName Section-->
              <xsl:variable name = "PB_ASSET_NAME" >
                <xsl:value-of select="translate(COL11,'&quot;','')"/>
              </xsl:variable>

              <xsl:variable name="PRANA_ASSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/AssetMapping.xml')/AssetMapping/PB[@Name='BOFA']/AssetData[@PBAssetCode=$PB_ASSET_NAME]/@PranaAsset"/>
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


              <xsl:choose>
                <xsl:when  test="COL7 &lt; 0">
                  <SideTagValue>
                    <xsl:value-of select="'5'"/>
                  </SideTagValue>
                  <Side>
                    <xsl:value-of select="'Sell'"/>
                  </Side>
                  <Quantity>
                    <xsl:value-of select='format-number((COL7)*(-1), "###,###,###.#")'/>
                  </Quantity>
                </xsl:when>
                <xsl:when  test="COL7 &gt; 0">
                  <SideTagValue>
                    <xsl:value-of select="'1'"/>
                  </SideTagValue>
                  <Side>
                    <xsl:value-of select="'Buy'"/>
                  </Side>
                  <Quantity>
                    <xsl:value-of select='format-number((COL7), "###,###,###.#")'/>
                  </Quantity>
                </xsl:when>
                <xsl:otherwise>
                  <SideTagValue>
                    <xsl:value-of select="''"/>
                  </SideTagValue>
                  <Side>
                    <xsl:value-of select="''"/>
                  </Side>
                  <Quantity>
                    <xsl:value-of select="0"/>
                  </Quantity>
                </xsl:otherwise>
              </xsl:choose>

              <!--Side, SideTagValue and Qunatity Section-->
              <!--<xsl:if test="COL7 &lt; 0">
						<SideTagValue>
							<xsl:value-of select="'5'"/>
						</SideTagValue>
						<Side>
							<xsl:value-of select="'Sell'"/>
						</Side>
						<Quantity>
							<xsl:value-of select='format-number((COL7)*(-1), "###,###,###.#")'/>
						</Quantity>
					</xsl:if>
					<xsl:if test="COL7 &gt; 0">
						<SideTagValue>
							<xsl:value-of select="'1'"/>
						</SideTagValue>
						<Side>
							<xsl:value-of select="'Buy'"/>
						</Side>
						<Quantity>
							<xsl:value-of select='format-number((COL7), "###,###,###.#")'/>
						</Quantity>
					</xsl:if>-->

              <!--PBSymbol Section-->
              <xsl:variable name="PB_COMPANY_NAME" select="translate(COL13,'&quot;','')"/>
              <PBSymbol>
                <xsl:value-of select="COL5"/>
              </PBSymbol>

              <!--<xsl:variable name="OptionUnderlyingSymbol">
            <xsl:choose>
              <xsl:when test="COL11='60'">
                <xsl:variable name="varAfterQ" select="substring-after(COL5,'Q')"/>
				  <xsl:variable name="OpraCode" select="substring($varAfterQ,1,3)"/>
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingSymbolMapping.xml')/SymbolMapping/PB[@Name='BOFA']/SymbolData[@OPRASymbol=$OpraCode]/@UnderlyingSymbol"/>
			  </xsl:when>
			  <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="OptionMonth">
            <xsl:choose>
              <xsl:when test="COL11='60'">
                <xsl:value-of select ="substring(COL5,string-length(COL5) - 1,1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="Strike">
            <xsl:choose>
              <xsl:when test="COL11='60'">
                <xsl:variable name ="varStrikeDecimal" select ="substring-after(COL18,'.')"/>
                <xsl:variable name ="varStrikeInt" select ="substring-before(COL18,'.')"/>
                <xsl:choose>
                  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 1">
                    <xsl:value-of select ="concat(COL18,'0')"/>
                  </xsl:when>
                  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) = 2">
                    <xsl:value-of select ="COL18"/>
                  </xsl:when>
                  <xsl:when test ="$varStrikeDecimal != '' and string-length($varStrikeDecimal) &gt; 2">
                    <xsl:value-of select ="concat($varStrikeInt,'.',substring($varStrikeDecimal,1,2))"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="concat(COL18,'.00')"/>
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
              <xsl:when test="COL11='60'">
                <xsl:value-of select ="substring(COL16,3,2)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>-->


              <!--CompanyName and Symbol Section-->
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BOFA']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
              </xsl:variable>
              <CompanyName>
                <xsl:value-of select='COL13'/>
              </CompanyName>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                  <Symbol>
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </Symbol>
                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>
                </xsl:when>
                <xsl:when test="COL11='60'">
                  <!--<xsl:variable name="varAfterQ" >
									<xsl:value-of select="substring-after(COL5,'Q')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterQ)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
								</xsl:variable>-->
                  <Symbol>
                    <!--<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>-->
                    <!--<xsl:value-of select ="concat('O:',$OptionUnderlyingSymbol,' ',$ExpYear,$OptionMonth,$Strike)"/>-->
                    <xsl:value-of select ="''"/>
                  </Symbol>
                  <IDCOOptionSymbol>
                    <xsl:value-of select="concat(COL5,'U')"/>
                  </IDCOOptionSymbol>
                </xsl:when>
                <xsl:otherwise>
                  <Symbol>
                    <xsl:value-of select="COL5"/>
                  </Symbol>
                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>
                </xsl:otherwise>
              </xsl:choose>

              <!--CostBasis Section-->
              <xsl:if test="COL7 != 0 and COL8 != 0">
                <AvgPX>
                  <!--<xsl:value-of select="col007 div col006"/>-->
                  <xsl:value-of select='format-number(COL8 div COL7,"###.00")'/>
                </AvgPX>
              </xsl:if >

              <!--MarkPrice Section-->
              <xsl:if test="COL7 != 0 and COL8 != 0">
                <MarkPrice>
                  <!--<xsl:value-of select="col007 div col006"/>-->
                  <xsl:value-of select='format-number(COL21,"###.00")'/>
                </MarkPrice>
              </xsl:if >

              <SMRequest>
                <xsl:value-of select ="'TRUE'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:if>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>



</xsl:stylesheet>

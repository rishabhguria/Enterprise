<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/DocumentElement">
    <DocumentElement>
      <xsl:for-each select="Comparision">

        <xsl:variable name = "varInstrumentType" >
          <xsl:value-of select="translate(translate(COL51,'&quot;',''),' ','')"/>
        </xsl:variable>

        <xsl:if test="($varInstrumentType !='CASH' and $varInstrumentType != 'FX FORWARDS')">

          <PositionMaster>
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="translate(COL2,'&quot;','')"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

            <xsl:choose>
              <xsl:when test ="number(translate(COL27,',','')) &lt; 0">
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
                <Quantity>
                  <xsl:value-of select="number(translate(COL27,',',''))*(-1)"/>
                </Quantity>
              </xsl:when>
              <xsl:otherwise>
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
                <Quantity>
                  <xsl:value-of select="number(translate(COL27,',',''))"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:choose>
              <xsl:when test ="boolean(number(COL30))">
                <AvgPX>
                  <xsl:value-of select="COL30"/>
                </AvgPX>
              </xsl:when>
              <xsl:otherwise>
                <AvgPX>
                  <xsl:value-of select="0"/>
                </AvgPX>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test ="boolean(number(COL30))">
                <MarkPrice>
                  <xsl:value-of select="COL30"/>
                </MarkPrice>
              </xsl:when>
              <xsl:otherwise>
                <MarkPrice>
                  <xsl:value-of select="0"/>
                </MarkPrice>
              </xsl:otherwise>
            </xsl:choose>

            <PBAssetName>
              <xsl:value-of select='$varInstrumentType'/>
            </PBAssetName>

            <xsl:choose>
              <xsl:when test ="number(translate(COL32,',','')) &lt; 0">
                <MarketValue>
                  <xsl:value-of select="COL32 *(-1)"/>
                </MarketValue>
              </xsl:when>
              <xsl:when test ="number(translate(COL32,',','')) &gt; 0">
                <MarketValue>
                  <xsl:value-of select="COL32"/>
                </MarketValue>
              </xsl:when>
              <xsl:otherwise>
                <MarketValue>
                  <xsl:value-of select="0"/>
                </MarketValue>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:choose>
              <xsl:when test ="number(translate(COL33,',','')) &lt; 0">
                <MarketValueBase>
                  <xsl:value-of select="COL33 *(-1)"/>
                </MarketValueBase>
              </xsl:when>
              <xsl:when test ="number(translate(COL33,',','')) &gt; 0">
                <MarketValueBase>
                  <xsl:value-of select="COL33"/>
                </MarketValueBase>
              </xsl:when>
              <xsl:otherwise>
                <MarketValueBase>
                  <xsl:value-of select="0"/>
                </MarketValueBase>
              </xsl:otherwise>
            </xsl:choose>


            <!-- Symbol Section-->

            <PBSymbol>
              <xsl:value-of select="COL8"/>
            </PBSymbol>

            <xsl:variable name="PB_COMPANY_NAME" select="COL6"/>

            <CompanyName>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </CompanyName>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test ="COL51='Futures'">
                    <xsl:variable name = "varLength" >
                      <xsl:value-of select="string-length(translate(translate(COL8,'&quot;',''),' ',''))"/>
                    </xsl:variable>
                    <xsl:choose>
                      <xsl:when test ="$varLength &gt; 0 ">
                        <xsl:variable name = "varAfter" >
                          <xsl:value-of select="substring(COL8,($varLength)-1,2)"/>
                        </xsl:variable>
                        <xsl:variable name = "varBefore" >
                          <xsl:value-of select="substring(COL8,1,($varLength)-2)"/>
                        </xsl:variable>
                        <Symbol>
                          <xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
                        </Symbol>
                      </xsl:when>
                      <xsl:otherwise>
                        <Symbol>
                          <xsl:value-of select="COL8"/>
                        </Symbol>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test ="COL51='Call - Listed' or COL51='Put - Listed'">
                    <Symbol>
                      <xsl:value-of select="translate(COL8,'/',' ')"/>
                    </Symbol>
                  </xsl:when>
                  <xsl:otherwise>
                    <Symbol>
                      <xsl:value-of select="COL8"/>
                    </Symbol>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>

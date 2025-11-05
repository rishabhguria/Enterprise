<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">
        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL7)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Position) and normalize-space(COL3)!='SpotFX'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name = "PB_COMPANY_NAME">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:choose>

                <xsl:when test ="contains(normalize-space(COL4),'USD')">
                  <xsl:value-of select ="normalize-space(substring-before(COL4,'USD'))"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="normalize-space(COL4)"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>
            
            <AccountName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccountName>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL8)"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>
                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL7)"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>
                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="Side">
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Side='Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$Side='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$Side='SellShort'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$Side='CoverShort'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL9)"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varSecFee &gt; 0">
                  <xsl:value-of select="$varSecFee"/>
                </xsl:when>
                <xsl:when test="$varSecFee &lt; 0">
                  <xsl:value-of select="$varSecFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <xsl:variable name="varOtherFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL18)"/>
              </xsl:call-template>
            </xsl:variable>
            <OtherBrokerFees>
              <xsl:choose>
                <xsl:when test="$varOtherFee &gt; 0">
                  <xsl:value-of select="$varOtherFee"/>
                </xsl:when>
                <xsl:when test="$varOtherFee &lt; 0">
                  <xsl:value-of select="$varOtherFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OtherBrokerFees>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL15)"/>
            </xsl:variable>
            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>
            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>

            <xsl:variable name="varODate">
              <xsl:choose>
                <xsl:when test="contains(COL1,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL1),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL1,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL1,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL1,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL1),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL1,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL1,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varOMonth">
              <xsl:choose>
                <xsl:when test="contains(COL1,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL1),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL1,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL1,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL1,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL1),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL1,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL1,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varOyear">
              <xsl:choose>
                <xsl:when test="contains(COL1,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL1,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL1,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL1,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <PositionStartDate>
              <xsl:value-of select="concat($varOMonth,'/',$varODate,'/',$varOyear)"/>
            </PositionStartDate>


            <xsl:variable name="varSDate">
              <xsl:choose>
                <xsl:when test="contains(COL2,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL2),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL2,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL2,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL2,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL2),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL2,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL2,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varSMonth">
              <xsl:choose>
                <xsl:when test="contains(COL2,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL2),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL2,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL2,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL2,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL2),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL2,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL2,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varSyear">
              <xsl:choose>
                <xsl:when test="contains(COL2,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL2,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL2,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL2,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <PositionSettlementDate>
              <xsl:value-of select="concat($varSMonth,'/',$varSDate,'/',$varSyear)"/>
            </PositionSettlementDate>

            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </PBSymbol>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



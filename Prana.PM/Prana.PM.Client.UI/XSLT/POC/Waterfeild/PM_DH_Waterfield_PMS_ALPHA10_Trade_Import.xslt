<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month='January'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$Month='February'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$Month='March'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$Month='April'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$Month='June'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$Month='July'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$Month=August">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$Month='September'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$Month='October'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$Month='November'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$Month='December'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="/">

    <DocumentElement>


      <xsl:for-each select ="//PositionMaster">
        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test ="number($Position) and COL2 !='*' and COL9 !='*'">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Reliance'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              
            </xsl:variable>

            <FundName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </FundName>

            <PositionStartDate>
              <xsl:value-of select ="''"/>
            </PositionStartDate>


            <PositionSettlementDate>
              <xsl:value-of select ="''"/>
            </PositionSettlementDate>


            <NetPosition>
              <xsl:choose>
                <xsl:when test ="$Position &gt;0">
                  <xsl:value-of select ="number($Position)"/>
                </xsl:when>
                <xsl:when test ="$Position &lt;0">
                  <xsl:value-of select ="number($Position)*-1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test ="$Position &gt;0">
                  <xsl:value-of select ="'1'"/>
                </xsl:when>
                <xsl:when test ="$Position &lt;0">
                  <xsl:value-of select ="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </SideTagValue>

            <xsl:variable name="varCoupon">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>
            <NetAmount>
              <xsl:value-of select="$varCoupon"/>
            </NetAmount>

            <xsl:variable name="CostValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>

                <xsl:when test ="$CostValue &lt;0">
                  <xsl:value-of select ="$CostValue*-1"/>
                </xsl:when>

                <xsl:when test ="$CostValue &gt;0">
                  <xsl:value-of select ="$CostValue"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="COL47 !=''">
                  <xsl:choose>
                    <xsl:when test="$Commission &gt; 0">
                      <xsl:value-of select="$Commission * (-1)"/>
                    </xsl:when>
                    <xsl:when test="$Commission &lt; 0">
                      <xsl:value-of select="$Commission "/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
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
                </xsl:otherwise>
              </xsl:choose>
            </Commission>


            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="COL47 !=''">
                  <xsl:choose>
                    <xsl:when test="$varSecFee &gt; 0">
                      <xsl:value-of select="$varSecFee * (-1)"/>
                    </xsl:when>
                    <xsl:when test="$varSecFee &lt; 0">
                      <xsl:value-of select="$varSecFee "/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
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
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>

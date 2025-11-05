<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>

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


  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL11)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Cash) and (contains(COL18,'Cash and Cash Equivalents')='true')">
            
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Northern Trust'"/>
              </xsl:variable>

              <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

              <xsl:variable name ="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>

              <Symbol>
                <xsl:value-of select="normalize-space(COL14)"/>
              </Symbol>
              
              <Currency>
                <xsl:choose>
                  <xsl:when test="normalize-space(COL14) = ''">
                    <xsl:value-of select="normalize-space(COL13)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="normalize-space(COL14)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Currency>

              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PortfolioAccount>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <OpeningBalanceDR>
                <xsl:choose>
                  <xsl:when test="number($Cash)">
                    <xsl:value-of select="$Cash"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceDR>

              <OpeningBalanceBaseDR>
                <xsl:choose>
                  <xsl:when test="number($Cash)">
                    <xsl:value-of select="$Cash"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceBaseDR>

              <xsl:variable name="CashBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL5)"/>
                </xsl:call-template>
              </xsl:variable>

              <OpeningBalanceCR>
                <xsl:choose>
                  <xsl:when test="number($CashBase)">
                    <xsl:value-of select="$CashBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceCR>

              <OpeningBalanceBaseCR>
                <xsl:choose>
                  <xsl:when test="number($CashBase)">
                    <xsl:value-of select="$CashBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceBaseCR>

              <!--<Currency>
								<xsl:value-of select="''"/>
							</Currency>-->

              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>

              <PortfolioAccount>
                <xsl:value-of select="''"/>
              </PortfolioAccount>

              <Currency>
                <xsl:value-of select="''"/>
              </Currency>

              <OpeningBalanceDR>
                <xsl:value-of select="0"/>
              </OpeningBalanceDR>

              <OpeningBalanceCR>
                <xsl:value-of select="0"/>
              </OpeningBalanceCR>

              <OpeningBalanceBaseDR>
                <xsl:value-of select="0"/>
              </OpeningBalanceBaseDR>

              <OpeningBalanceBaseCR>
                <xsl:value-of select="0"/>
              </OpeningBalanceBaseCR>

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
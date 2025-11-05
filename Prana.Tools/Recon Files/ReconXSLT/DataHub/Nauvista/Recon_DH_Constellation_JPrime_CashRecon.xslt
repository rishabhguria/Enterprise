<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
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
  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select="//Comparision">

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test ="contains(COL6,'Cash')and contains(COL4,'Div')!='true'and contains(COL4,'W/HTax')!='true'">

            <PositionMaster>

              <xsl:variable name = "PB_FUND_NAME" >
                <xsl:value-of select="normalize-space(COL3)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>

              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME !=''">
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PortfolioAccount>

              <Currency>
                <xsl:value-of select="normalize-space(COL5)"/>
              </Currency>

              <xsl:variable name="CashBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL10"/>
                </xsl:call-template>
              </xsl:variable>

              <OpeningBalanceDR>
                <xsl:choose>
                  <xsl:when test="number($CashBase)">
                    <xsl:value-of select="$CashBase"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceDR>

              <OpeningBalanceCR>
                <xsl:choose>
                  <xsl:when test="number($Cash)">
                    <xsl:value-of select="$Cash"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceCR>

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

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>


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

        <xsl:variable name="CashBase">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL13"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test ="number($CashBase)  and contains(COL1,'CASH')='true'">

            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Morgan Stanley and Co. International plc'"/>
              </xsl:variable>

				<xsl:variable name="PB_FUND_NAME">
					<xsl:value-of select="COL8"/>
				</xsl:variable>
				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
                <xsl:value-of select="normalize-space(COL3)"/>
              </Currency>

            
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

              <xsl:variable name="CashLocal">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL12"/>
                </xsl:call-template>
              </xsl:variable>

              <OpeningBalanceDR>
                <xsl:choose>
                  <xsl:when test="number($CashLocal)">
                    <xsl:value-of select="$CashLocal"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceDR>

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


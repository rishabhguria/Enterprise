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

        <xsl:choose>
          <xsl:when test="COL7='CASH EQUIVALENT' or COL7='CASH'">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'DHPB'"/>
              </xsl:variable>

              <xsl:variable name = "PB_FUND_NAME">
                <xsl:value-of select="COL1"/>
              </xsl:variable>

              <xsl:variable name ="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
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
              <xsl:variable name="PB_CURRENCY_NAME" select="COL8"/>

              <!-- <xsl:variable name ="PRANA_CURRENCY_NAME"> -->
                <!-- <xsl:value-of select ="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyDesc=$PB_CURRENCY_NAME]/@CurrencyName"/> -->
              <!-- </xsl:variable> -->

              <Currency>
                <!-- <xsl:choose> -->

                  <!-- <xsl:when test ="$PRANA_CURRENCY_NAME!=''"> -->
                    <!-- <xsl:value-of select ="$PRANA_CURRENCY_NAME"/> -->
                  <!-- </xsl:when> -->

                  <!-- <xsl:otherwise> -->
                    <!-- <xsl:value-of select ="$PB_CURRENCY_NAME"/> -->
					<xsl:value-of select ="'USD'"/>
                  <!-- </xsl:otherwise> -->

                <!-- </xsl:choose> -->
              </Currency>

              <xsl:variable name="Cash" select="number(COL23)"/>
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

              <xsl:variable name="OpeningBalanceCR">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <OpeningBalanceCR>
                <xsl:choose>
                  <xsl:when test="number($OpeningBalanceCR)">
                    <xsl:value-of select="$OpeningBalanceCR"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceCR>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

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

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>


            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
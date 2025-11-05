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
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varPosition) and (COL4='JAPANESE YEN' or COL4='HONG KONG DOLLAR') and COL3='Equity'">
          <PositionMaster>

            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <xsl:variable name="varSettlCurrency">
              <xsl:choose>
                <xsl:when test ="normalize-space(COL4)='JAPANESE YEN'">
                  <xsl:value-of select ="'JPY'"/>
                </xsl:when>

                <xsl:when test ="normalize-space(COL4)='HONG KONG DOLLAR'">
                  <xsl:value-of select ="'HKG'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <SettlementCurrency>
              <xsl:value-of select ="$varSettlCurrency"/>
            </SettlementCurrency>

            <xsl:variable name="varCOL9">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varCOL14">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="FXRate">
              <xsl:choose>
                <xsl:when test ="$varSettlCurrency='JPY' or $varSettlCurrency='HKG'">
                  <xsl:value-of select="($varPosition * $varCOL9) div $varCOL14"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <ForexPrice>
              <xsl:choose>
                <xsl:when test="$FXRate &gt; 0">
                  <xsl:value-of select="$FXRate"/>
                </xsl:when>
                
                <xsl:when test="$FXRate &lt; 0">
                  <xsl:value-of select="$FXRate * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </ForexPrice>

            <!--<FXConversionMethodOperator>
              <xsl:choose>
                <xsl:when test ="$varSettlCurrency='GBP' or $varSettlCurrency='EUR' or $varSettlCurrency='AUD' or $varSettlCurrency='NZD'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'M'"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXConversionMethodOperator>-->

            <xsl:variable name="varDate">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <Date>
              <xsl:value-of select="$varDate"/>
            </Date>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
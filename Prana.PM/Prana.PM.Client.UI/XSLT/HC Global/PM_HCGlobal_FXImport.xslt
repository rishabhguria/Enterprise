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


        <xsl:if test="contains(COL2,'Cash')">
          <PositionMaster>

            <xsl:variable name="varBaseCurrency">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <xsl:variable name="varSettlCurrency">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>
            <SettlementCurrency>
              <xsl:value-of select="$varSettlCurrency"/>
            </SettlementCurrency>
            
            <xsl:variable name="var1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>
            
              <xsl:variable name="var2">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL14"/>
                </xsl:call-template>
              </xsl:variable>
            
            <xsl:variable name="varFXRate">
              <xsl:value-of select="$var2 div $var1"/>
            </xsl:variable>

            <xsl:variable name="FXRate">
              <xsl:choose>
                <xsl:when test ="$varSettlCurrency='GBP' or $varSettlCurrency='EUR' or $varSettlCurrency='AUD' or $varSettlCurrency='NZD'">
                  <xsl:value-of select="1 div $varFXRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varFXRate"/>
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
                  <xsl:value-of select="1"/>
                </xsl:otherwise>

              </xsl:choose>
            </ForexPrice>

            <Date>
              <xsl:value-of select="''"/>
            </Date>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
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

        <xsl:if test="normalize-space(COL3)='Cash'">

          <PositionMaster>

            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name = "PB_CURRENCY_NAME" >
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            <xsl:variable name="PRANA_CURRENCY_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SettlementCurrencyMapping.xml')/SettleCurrencyMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSettleCurrencyName=$PB_CURRENCY_NAME]/@SettleCurrency"/>
            </xsl:variable>

            <SettlementCurrency>
              <xsl:choose>
                <xsl:when test="$PRANA_CURRENCY_NAME!=''">
                  <xsl:value-of select="$PRANA_CURRENCY_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_CURRENCY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </SettlementCurrency>

            <xsl:variable name="FXRate">
              <xsl:choose>
                <xsl:when test ="$PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD'">
                  <xsl:value-of select="number(COL6) div number(COL10)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="number(COL10) div number(COL6)"/>
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
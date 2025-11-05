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

        <xsl:variable name="varFXRate">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>



        <xsl:if test="number($varFXRate) and COL42='FX Forward' and not(starts-with(COL20,'USD'))">

          <PositionMaster>
            
            <xsl:variable name="varCurrency" select="COL17"/>
            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MSFS'"/>
            </xsl:variable>
            <xsl:variable name = "PB_CURRENCY_NAME" >
              <xsl:value-of select="normalize-space($varCurrency)"/>
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

            <ForexPrice>
              <xsl:choose>
                <xsl:when test="COL17='AUD'">
                  <xsl:value-of select="$varFXRate"/>
                </xsl:when>
                <xsl:when test="COL17='GBP'">
                  <xsl:value-of select="$varFXRate"/>
                </xsl:when>
                <xsl:when test="COL17='EUR'">
                  <xsl:value-of select="$varFXRate"/>
                </xsl:when>
                <xsl:when test="COL17='NZD'">
                  <xsl:value-of select="$varFXRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1 div $varFXRate"/>
                </xsl:otherwise>

              </xsl:choose>
            </ForexPrice>

            <xsl:variable name="Date" select="''"/>
            <Date>
              <xsl:value-of select="$Date"/>
            </Date>
            
          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
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

        <xsl:variable name="varMVBase">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL20)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="varMVLocal">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL19)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="varFXRate">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="$varMVBase div $varMVLocal"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test ="number($varFXRate)">

          <PositionMaster>
            <!--<xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>-->
           
            
            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable> 
            
            <SettlementCurrency>
              <xsl:value-of select="$PB_CURRENCY_NAME"/>
            </SettlementCurrency>

            <ForexPrice>
              <xsl:choose>
                <xsl:when test="$varFXRate &gt; 0">
                  <xsl:value-of select="$varFXRate"/>

                </xsl:when>
                <xsl:when test="$varFXRate &lt; 0">
                  <xsl:value-of select="$varFXRate * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </ForexPrice>
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
</xsl:stylesheet>
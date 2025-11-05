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


		
		 <xsl:if test ="number(translate(COL18,'+','')) and ((contains(COL5,'DABRA CAPITAL MASTER FUND I') or 
          contains(COL5,'RAZMAR BLUESTONE FUND LP') or contains(COL5,'ADK SOHO FUND LP') or contains(COL5,'KAZAZIAN CAPITAL MASTER FUND') or 
          contains(COL5,'IBIS SPECIAL OPP FD LP') or contains(COL5,'LEGACY WORLDWIDE INVESTMENTS') or contains(COL5,'ACG ADVISORS (UK) LLP') or 
          contains(COL5,'GSREF  L.P.')))">

          <PositionMaster>

            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name = "PB_CURRENCY_NAME" >
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <SettlementCurrency>
                  <xsl:value-of select="$PB_CURRENCY_NAME"/>
            </SettlementCurrency>

            <xsl:variable name="FXRate">
              <xsl:choose>
                <xsl:when test ="$PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD'">
                  <xsl:value-of select="1 div number(translate(COL27,'+','')) "/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select=" 1 div number(translate(COL27,'+',''))"/>
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
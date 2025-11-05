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


        <xsl:variable name="varMarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varMarkPrice) and COL42='FX Forward' and not(starts-with(COL20,'USD'))">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MSFS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL20"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <Symbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>

            </Symbol>

            <xsl:variable name="MarkPrice">
              <xsl:choose>
                <xsl:when test="COL17='AUD'">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:when test="COL17='GBP'">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:when test="COL17='EUR'">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:when test="COL17='NZD'">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1 div $varMarkPrice"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <MarkPrice>
              <xsl:value-of select="$MarkPrice"/>
            </MarkPrice>

            <xsl:variable name="varFXPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varForwardPrice">
              <xsl:choose>
                <xsl:when test="COL17='AUD'">
                  <xsl:value-of select="$varFXPrice"/>
                </xsl:when>
                <xsl:when test="COL17='GBP'">
                  <xsl:value-of select="$varFXPrice"/>
                </xsl:when>
                <xsl:when test="COL17='EUR'">
                  <xsl:value-of select="$varFXPrice"/>
                </xsl:when>
                <xsl:when test="COL17='NZD'">
                  <xsl:value-of select="$varFXPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1 div $varFXPrice"/>
                </xsl:otherwise>
              </xsl:choose>   
            </xsl:variable>


            <xsl:variable name="varForwardPoints">
              <xsl:value-of select="($MarkPrice - $varForwardPrice)"/>
            </xsl:variable>

            <ForwardPoints>
              <xsl:value-of select="$varForwardPoints"/>
            </ForwardPoints>
            
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
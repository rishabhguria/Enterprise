<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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
      <xsl:for-each select ="//PositionMaster">
        <xsl:variable name="varMarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL13"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test ="number($varMarkPrice)">
          <PositionMaster>

            <xsl:variable name = "PB_NAME" >
              <xsl:value-of select ="'Test'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL4"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "varCusip" >
              <xsl:value-of select ="COL20"/>
            </xsl:variable>

            <xsl:variable name = "varSedol" >
              <xsl:value-of select ="COL19"/>
            </xsl:variable>
            
            <xsl:variable name = "varSymbol" >
              <xsl:value-of select ="COL19"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                
                <xsl:when test ="$varSedol!='*' or $varSedol!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                
                <xsl:when test ="$varCusip!='*' or $varCusip!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                <xsl:when test ="$varSymbol!=''">
                  <xsl:value-of select ="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:when test ="$varSedol!='*' or $varSedol!=''">
                  <xsl:value-of select ="$varSedol"/>
                </xsl:when>

                <xsl:when test ="$varCusip!='*' or $varCusip!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                <xsl:when test ="$varSymbol!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>


            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:when test ="$varCusip!='*' or $varCusip!=''">
                  <xsl:value-of select ="$varCusip"/>
                </xsl:when>
                
                <xsl:when test ="$varSedol!='*' or $varSedol!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:when test ="$varSymbol!=''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="$varMarkPrice &lt;0">
                  <xsl:value-of select ="$varMarkPrice*-1"/>
                </xsl:when>

                <xsl:when test ="$varMarkPrice &gt;0">
                  <xsl:value-of select ="$varMarkPrice"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

            <Date>
              <xsl:value-of select ="''"/>
            </Date>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>

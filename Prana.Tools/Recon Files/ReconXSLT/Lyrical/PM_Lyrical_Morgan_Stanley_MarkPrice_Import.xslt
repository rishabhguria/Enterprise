<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>



      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="MarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="substring(COL1,211,14)"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="substring(COL1,65,19)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Cost">
          <xsl:value-of select="$MarkPrice div $Quantity"/>
        </xsl:variable>
 
        <xsl:if test="number($MarkPrice)  and contains(normalize-space(substring(COL1,28,36)),'BANK DEPOSIT PROGRAM')!='true'">



          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'LYRICAL'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="substring(COL1,28,36)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="Symbol" select="substring(COL1,189,21)"/>

            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="normalize-space($Symbol)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>

            </Symbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$MarkPrice &gt; 0">
                  <xsl:value-of select="$MarkPrice"/>

                </xsl:when>
                <xsl:when test="$MarkPrice &lt; 0">
                  <xsl:value-of select="$MarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>


            <!--<xsl:variable name="Side" select="COL1"/>


            <SideTagValue>
              <xsl:choose>

                <xsl:when test="$Side='Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                <xsl:when test="$Side='Long'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </SideTagValue>-->


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>



            <xsl:variable name="Year" select="substring(COL1,378,4)"/>
            <xsl:variable name="Month" select="substring(COL1,383,2)"/>
            <xsl:variable name="Day" select="substring(COL1,386,2)"/>

            <xsl:variable name="Date" select="substring(COL1,378,10)"/>

            <Date>

              <xsl:choose>
                <xsl:when test="contains(substring(COL1,378,10),'-')">
                  <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$Date"/>
                </xsl:otherwise>
              </xsl:choose>


            </Date>



          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
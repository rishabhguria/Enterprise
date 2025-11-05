<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='C' or substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='P'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space(COL3),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL3),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),3,2)"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),'##.00')"/>
      </xsl:variable>
      <xsl:variable name="MonthCodVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="$ExpiryMonth"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">
        <xsl:variable name="MarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL13"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($MarkPrice)">
          <PositionMaster>


            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>



            <xsl:variable name = "varAsset" >
              <xsl:choose>
                <xsl:when test="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='C' or substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='P'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="Symbol">
              <xsl:value-of select="normalize-space(COL15)"/>
            </xsl:variable>

            <xsl:variable name="varDoubleQuote">"</xsl:variable>
            <xsl:variable name="varCusip">
              <xsl:value-of select="translate(translate(normalize-space(COL17),'=',''),$varDoubleQuote,'')"/>
            </xsl:variable>

            
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL3)"/>
                    <xsl:with-param name="Suffix" select="''"/>
                  </xsl:call-template>
                </xsl:when>
                 <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
                <xsl:when test="$varCusip!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>       
               
                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <CUSIP>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varCusip!=''">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>


            
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

            <xsl:variable name="varDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <Date>
              <xsl:value-of select="$varDate"/>
            </Date>

            <PBSymbol>
              <xsl:value-of select ="$PB_COMPANY"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>
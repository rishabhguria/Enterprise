<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">
        <xsl:if test="COL3='Equity'">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'FCM'"/>
            </xsl:variable>
            
            <xsl:variable name="varSymbol">
              <xsl:value-of select="translate(substring-before(COL5,' '),'/','.')"/>
            </xsl:variable>

            <xsl:variable name="Exchange" select="substring-before(substring-after(COL5,' '),' ')"/>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="$Exchange"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            
            <TickerSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varSymbol"/>
                </xsl:otherwise>
              </xsl:choose>            
            </TickerSymbol>

            <UnderLyingSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varSymbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </UnderLyingSymbol>



            <xsl:variable name="varBloomberg">
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            <BloombergSymbol>
              <xsl:value-of select="$varBloomberg"/>
            </BloombergSymbol>

            <xsl:variable name="Currency" select="normalize-space(COL6)"/>

            <AUECID>
              <xsl:choose>
                <xsl:when test="$Currency='AUD'">
                  <xsl:value-of select="63"/>
                </xsl:when>
                <xsl:when test="$Currency='SEK'">
                  <xsl:value-of select="59"/>
                </xsl:when>
                <xsl:when test="$Currency='PHP'">
                  <xsl:value-of select="161"/>
                </xsl:when>
                <xsl:when test="$Currency='EUR'">
                  <xsl:choose>
                    <xsl:when test="$Exchange='GR'or $Exchange='GY'">
                      <xsl:value-of select="'34'"/>
                    </xsl:when>
                    <xsl:when test="$Exchange='BB' or $Exchange='FP' or $Exchange='PL' or $Exchange='NA'">
                      <xsl:value-of select="'44'"/>
                    </xsl:when>
                    <xsl:when test="$Exchange='SM'">
                      <xsl:value-of select="'53'"/>
                    </xsl:when>
                    <xsl:when test="$Exchange='IM'">
                      <xsl:value-of select="'56'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>                  
                </xsl:when>

                <xsl:when test="$Currency='NOK'">
                  <xsl:value-of select="31"/>
                </xsl:when>

                <xsl:when test="$Currency='JPY'">
                  <xsl:value-of select="21"/>
                </xsl:when>
                
                <xsl:when test="$Currency='GBP'">
                  <xsl:value-of select="43"/>
                </xsl:when>

                <xsl:when test="$Currency='HKD'">
                  <xsl:value-of select="20"/>
                </xsl:when>
                
                <xsl:when test="$Currency='CHF'">
                  <xsl:value-of select="53"/>
                </xsl:when>

                <xsl:when test="$Currency='CAD'">
                  <xsl:value-of select="71"/>
                </xsl:when>
                
                <xsl:when test="$Currency='USD'">
                  <xsl:value-of select="1"/>
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AUECID>
            

            <Multiplier>
              <xsl:value-of select="'1'"/>
            </Multiplier>
            
            <xsl:variable name="varDescription">
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <LongName>
              <xsl:value-of select="$varDescription"/>
            </LongName>

            <UDASector>
              <xsl:value-of select="'Undefined'"/>
            </UDASector>

            <UDASubSector>
              <xsl:value-of select="'Undefined'"/>
            </UDASubSector>

            <UDASecurityType>
              <xsl:value-of select="'Undefined'"/>
            </UDASecurityType>

            <UDAAssetClass>
              <xsl:value-of select="'Undefined'"/>
            </UDAAssetClass>

            <UDACountry>
              <xsl:value-of select="'Undefined'"/>
            </UDACountry>

          
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>

<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
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
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Date">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="1"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="2"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="3"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="4"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="5"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="6"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="7"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="8"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="9"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="10"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="11"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="12"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>

    <xsl:if test="COL41='Equity Option'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(COL1,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL1),'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL1),' '),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL1),'/'),'/'),' ')"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL1),'/'),'/'),' '),' '),1,1)"/>
      </xsl:variable>

      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL1),'/'),'/'),' '),' '),2),'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
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

      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

    </xsl:if>
  </xsl:template>


  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="MarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="varOptionMarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:variable name="varAsset">
          <xsl:choose>
            <xsl:when test="COL41='Equity Option'">
              <xsl:value-of select="'EqityOption'"/>
            </xsl:when>
            <xsl:when test="contains(COL1,' Equity') and COL41 !='Equity Option'">
              <xsl:value-of select="'Equity'"/>
            </xsl:when>
            <xsl:when test="substring-after(normalize-space(COL1),' ')='Corp' or substring-after(normalize-space(COL1),' ')='Pfd'">
              <xsl:value-of select="'FixedIncom'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="''"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="varMarkPrice">
          <xsl:choose>
            <xsl:when test="$varAsset='Equity' or $varAsset='FixedIncom'">
              <xsl:value-of select="$MarkPrice"/>
            </xsl:when>
            <xsl:when test="$varAsset='EqityOption'">
              <xsl:value-of select="$varOptionMarkPrice"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

       

        <xsl:if test="number($varMarkPrice)  and substring-after(normalize-space(COL1),' ') !='Curncy' and substring-after(normalize-space(COL1),' ') !='Index'">
        <!--<xsl:if test="number($varMarkPrice)">-->

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Wells Fargo'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Symbol">
              <xsl:value-of select="substring-before(COL1,' Equity')"/>
            </xsl:variable>
          

            <xsl:variable name="varSymbol">
              <xsl:choose>
                <xsl:when test="contains($Symbol,'/') and COL41 !='Equity Option'">
                  <xsl:value-of select="translate($Symbol,'/','.')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$Symbol"/>
                </xsl:otherwise>
              </xsl:choose>             
            </xsl:variable>

            <xsl:variable name="varBloombergSymbol">
              <xsl:value-of select="concat($varSymbol,' ','EQUITY')"/>
            </xsl:variable>

        <xsl:variable name="CUSIP">
             <xsl:choose>
               <xsl:when test="substring-after(normalize-space(COL1),' ')='Corp'">
                 <xsl:value-of select="substring-before(normalize-space(COL1),'Corp')"/>
               </xsl:when>
               <xsl:when test="substring-after(normalize-space(COL1),' ')='pfd'">
                 <xsl:value-of select="substring-before(normalize-space(COL1),'pfd')"/>
               </xsl:when>
               <xsl:otherwise>
                 <xsl:value-of select="''"/>
               </xsl:otherwise>
             </xsl:choose>
			 </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL1"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="$varAsset='FixedIncom'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varCurrency !='USD'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSymbol !=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='FixedIncom'">
                  <xsl:value-of select="$CUSIP"/>
                </xsl:when>

                <xsl:when test="$varCurrency !='USD'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varSymbol !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <Bloomberg>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varAsset='EqityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varCurrency !='USD'">
                  <xsl:value-of select="$varBloombergSymbol"/>
                </xsl:when>
                
                <xsl:when test="$varAsset='FixedIncom'">
                  <xsl:value-of select="''"/>
                </xsl:when>



                <xsl:when test="$varSymbol !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Bloomberg>

            
            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$varMarkPrice &gt; 0">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:when test="$varMarkPrice &lt; 0">
                  <xsl:value-of select="$varMarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>

            <Date>
              <xsl:value-of select="''"/>
            </Date>

            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>
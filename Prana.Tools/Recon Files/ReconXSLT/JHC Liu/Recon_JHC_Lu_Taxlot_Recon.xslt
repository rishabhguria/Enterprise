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
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>

    <xsl:variable name ="varMonthUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nMonth) = 1">
          <xsl:value-of select ="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="nDayUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nDay) = 1">
          <xsl:value-of select ="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>



  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='CALL'">
      <xsl:choose>
        <xsl:when test="$Month=01">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test=" $Month=06">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='PUT'">
      <xsl:choose>
        <xsl:when test="$Month=01">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12">
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
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDate">
      <xsl:value-of select="normalize-space(substring-before(substring-after(substring-after($Symbol,' '),' '),' '))"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring-before(substring-after($ExpiryDate,'/'),'/')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="normalize-space(substring-before($ExpiryDate,'/'))"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring-after(substring-after($ExpiryDate,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name="PutORCall">
      <xsl:value-of select="normalize-space(substring-before(COL6,' '))"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring-before(substring-after(COL6,' '),' '),'##.00')"/>
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
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL31"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Quantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'JP Morgan'"/>
            </xsl:variable>

            <xsl:variable name ="PB_FUND_NAME">
              <xsl:value-of select ="COL1"/>
            </xsl:variable>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccountName>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL11)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL9)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            
            <Quantity>
              <xsl:choose>
                <xsl:when test="number($Quantity)">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>


            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$NetNotionalValue &gt; 0">
                  <xsl:value-of select="$NetNotionalValue"/>

                </xsl:when>
                <xsl:when test="$NetNotionalValue &lt; 0">
                  <xsl:value-of select="$NetNotionalValue"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="COL6"/>
                </xsl:otherwise>

              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL32"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$NetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$NetNotionalValueBase"/>

                </xsl:when>
                <xsl:when test="$NetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$NetNotionalValueBase"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </NetNotionalValueBase>



            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL35"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>

              <xsl:choose>

                <xsl:when test="number($MarketValue)">
                  <xsl:value-of select="$MarketValue"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="COL8"/>
                </xsl:otherwise>

              </xsl:choose>

            </MarketValue>


            <xsl:variable name="FXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL27"/>
              </xsl:call-template>
            </xsl:variable>

            <FXRate>
              <xsl:choose>
                <xsl:when test="number($FXRate)">
                  <xsl:value-of select="$FXRate"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="COL8"/>
                </xsl:otherwise>

              </xsl:choose>
            </FXRate>
            

            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL34"/>
              </xsl:call-template>
            </xsl:variable>
            <MarketValueBase>

              <xsl:choose>
                <xsl:when test="number($MarketValueBase)">
                  <xsl:value-of select="$MarketValueBase"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </MarketValueBase>




            <xsl:variable name="varOrgDay">
              <xsl:value-of select="substring(COL29,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varOrgMonth">
              <xsl:value-of select="substring(COL29,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varOrgYear">
              <xsl:value-of select="substring(COL29,1,4)"/>
            </xsl:variable>
            <OriginalPurchaseDate>
              <xsl:value-of select="concat($varOrgMonth,'/',$varOrgDay,'/',$varOrgYear)"/>
            </OriginalPurchaseDate>


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL29,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL29,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL29,1,4)"/>
            </xsl:variable>
            <TradeDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </TradeDate>

            <CurrencySymbol>
              <xsl:value-of select="COL20"/>
            </CurrencySymbol>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
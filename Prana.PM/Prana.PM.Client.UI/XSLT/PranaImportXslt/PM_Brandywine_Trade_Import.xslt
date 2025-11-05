<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
  <xsl:output method="xml" indent="yes" />
  <xsl:template name="Translate">
    <xsl:param name="Number" />
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="MonthCode">
    <xsl:param name="Month" />
    <xsl:param name="PutOrCall" />
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'" />
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'" />
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'" />
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'" />
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'" />
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'" />
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'" />
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'" />
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'" />
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'" />
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'" />
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'X'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>



  <xsl:template name="Option">
    <xsl:param name="varSymbol" />
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($varSymbol,' ')" />
    </xsl:variable>
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space($varSymbol),' ')" />
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),5,2)" />
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),3,2)" />
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),1,2)" />
    </xsl:variable>
    <xsl:variable name="PutORCall">

      <xsl:value-of select="translate($var, '0123456789.', '')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(translate($var,translate($var, '0123456789.', ''), ''),8,7) div 1000,'##.00')" />
    </xsl:variable>
    <xsl:variable name="MonthCodVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$ExpiryMonth" />
        <xsl:with-param name="PutOrCall" select="$PutORCall" />
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Day">
      <xsl:choose>
        <xsl:when test="substring($ExpiryDay,1,1)='0'">
          <xsl:value-of select="substring($ExpiryDay,2,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ExpiryDay" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="concat('O:',$UnderlyingSymbol,$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)" />
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
  
    <xsl:template name="FormatDates">
    <xsl:param name="varFullDate" />
    <xsl:variable name="varYear">
      <xsl:value-of select="substring($varFullDate, string-length($varFullDate) - 3 , 4)"/>
    </xsl:variable>
    <xsl:variable name="varWithoutYear">
      <xsl:value-of select="substring($varFullDate, 1, string-length($varFullDate) - 4)"/>
    </xsl:variable>
    <xsl:variable name="varDay">
      <xsl:choose>
        <xsl:when test="$varWithoutYear &lt; 100">
          <xsl:value-of select="concat('0',substring($varWithoutYear, string-length($varWithoutYear) - 0, 1))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($varWithoutYear, string-length($varWithoutYear) - 1, string-length($varWithoutYear))"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="$varWithoutYear &lt; 999">
          <xsl:value-of select="concat('0',substring($varWithoutYear, 1, 1))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($varWithoutYear, 1, 2)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="varNetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL90)" />
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($varNetPosition)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''" />
            </xsl:variable>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''" />
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol" />
            </xsl:variable>
            <xsl:variable name="varSymbol">
              <xsl:value-of select="(normalize-space(COL33))" />
            </xsl:variable>
            <xsl:variable name="varCurrency">
              <xsl:value-of select="(normalize-space(COL55))" />
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="(normalize-space(COL33))" />
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="(normalize-space(COL33))" />
            </xsl:variable>
            

            <xsl:variable name="PB_FUND_NAME" select="COL11" />
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME" />
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>
            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varNetPosition &gt; 0">
                  <xsl:value-of select="$varNetPosition" />
                </xsl:when>
                <xsl:when test="$varNetPosition &lt; 0">
                  <xsl:value-of select="$varNetPosition * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL78)" />
            </xsl:variable>
            <SideTagValue>
             <xsl:choose>
                <xsl:when test="$varSide='SALE'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide='PURCHASE'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
              
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>
            
            <Symbol>
              <xsl:choose>
                
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>
                <xsl:when test="$varCurrency='USD' and (string-length($varSymbol) = 9)">
                  <xsl:value-of select="''" />
                </xsl:when>
                 <xsl:when test="$varCurrency!='USD' and  (string-length($varSymbol) = 7)">
                  <xsl:value-of select="''" />
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="''" />
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSymbol!='*' and $varCurrency='USD'">
                  <xsl:value-of select="$varSymbol" />
                </xsl:when>
                <xsl:when test="$varSymbol!='*' and $varCurrency!='USD'">
                  <xsl:value-of select="''" />
                </xsl:when>
                <xsl:when test="$varSymbol!='*'">
                  <xsl:value-of select="''" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>
            <SEDOL>
               <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSymbol!='*' and $varCurrency!='USD'">
                  <xsl:value-of select="$varSymbol" />
                </xsl:when>
                <xsl:when test="$varSymbol!='*' and $varCurrency='USD'">
                  <xsl:value-of select="''" />
                </xsl:when>
                <xsl:when test="$varSymbol!='*'">
                  <xsl:value-of select="''" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>
             <xsl:variable name="varPositionStartDate">
              <xsl:call-template name ="FormatDate">
                <xsl:with-param name="DateTime" select="COL75">
                </xsl:with-param>
              </xsl:call-template>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate" />
            </PositionStartDate>

             
            <xsl:variable name="varSDate">
              <xsl:call-template name ="FormatDate">
                <xsl:with-param name="DateTime" select="COL74">
                </xsl:with-param>
              </xsl:call-template>
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="$varSDate" />
            </PositionSettlementDate>

           
               <xsl:variable name="varPriceBase">
            <xsl:call-template name="Translate">
              <xsl:with-param name="Number" select="COL67" />
            </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL68" />
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis" />
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * -1" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>
            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="COL55"/>
            </xsl:variable>

            <xsl:variable name="PRANA_CURRENCY_ID">
              <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
            </xsl:variable>

            <CurrencyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_CURRENCY_ID)">
                  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyID>

        
            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL47" />
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission" />
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </Commission>
            
            <xsl:variable name="varNetAmount">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL66" />
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varFXRate">
               <xsl:choose>
                <xsl:when test="$varCostBasis!= 0">
                  <xsl:value-of select="($varPriceBase div $varCostBasis)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test="$varFXRate &gt; 0">
                  <xsl:value-of select="$varFXRate" />
                </xsl:when>
                <xsl:when test="$varFXRate &lt; 0">
                  <xsl:value-of select="$varFXRate * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
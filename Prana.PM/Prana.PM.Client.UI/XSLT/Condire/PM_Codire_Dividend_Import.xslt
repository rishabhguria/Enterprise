<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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
  
  

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL21"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varDividend) and (COL2='038CACXU1' or COL2='038CACXT4' or COL2='038CDKU41' or COL2='038CDKTN1' 
                or COL2='038CADTV2' or COL2='038CACXV9' or COL2='038CAD183' or COL2='06178JZ20' or COL2='038CACAH5' or COL2='038CDOQE6'
                or COL2='06178YD96' or COL2='06178Y5A2' or COL2='0581CP811' or COL2='0581CPVY3' or COL2='0581CU0B6' or COL2='026DACP57' or COL2='026DACP40' or COL2='026DACP32')">

          <PositionMaster>

            <xsl:variable name="PB_Name">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="''"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
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

            <xsl:variable name="varCusip">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>

            <xsl:variable name="varSedol">
              <xsl:value-of select="normalize-space(COL11)"/>
            </xsl:variable>
         
            <xsl:variable name="Symbol">
              <xsl:value-of select="normalize-space(COL9)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSedol !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varCusip !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_Symbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varCusip !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSedol !=''">
                  <xsl:value-of select="$varSedol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varCusip !=''">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>
                <xsl:when test="$varSedol !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <Amount>
              <xsl:choose>
                <xsl:when test="number($varDividend)">
                  <xsl:value-of select="$varDividend"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Amount>


            <!--<xsl:variable name="varPSDay">
              <xsl:value-of select="substring-before(COL9,'-')"/>
            </xsl:variable>

            <xsl:variable name="varPSMonth">
              <xsl:value-of select="substring-before(substring-after(COL9,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varPSYear">
              <xsl:value-of select="substring-after(substring-after(COL9,'-'),'-')"/>
            </xsl:variable>-->
            
            <PayoutDate>
              <!--<xsl:value-of select="concat($varPSMonth,'/',$varPSDay,'/',$varPSYear)"/>-->
              <xsl:value-of select="COL18"/>
            </PayoutDate>

            <!--<xsl:variable name="varExDay">
              <xsl:value-of select="substring-before(COL8,'-')"/>
            </xsl:variable>

            <xsl:variable name="varExMonth">
              <xsl:value-of select="substring-before(substring-after(COL8,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varExYear">
              <xsl:value-of select="substring-after(substring-after(COL8,'-'),'-')"/>
            </xsl:variable>-->

          
            <ExDate>
              <xsl:value-of select="COL17"/>
            </ExDate>

         
            <RecordDate>
              <xsl:value-of select="''"/>
            </RecordDate>



            <xsl:variable name="varCurrencyName" select="COL13"/>
            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>

            <xsl:variable name="varDescription">
              <xsl:choose>
                <xsl:when test="(COL6 ='DividendGross' or COL6='DividendSubstitute') and $varDividend &lt; 0">
                  <xsl:value-of select ="'Dividend Charged'"/>
                </xsl:when>
                <xsl:when test="(COL6 ='DividendGross' or COL6='DividendSubstitute') and $varDividend &gt; 0">
                  <xsl:value-of select="'Dividend Received'"/>
                </xsl:when>
                <xsl:when test="COL6 ='DividendTax' and $varDividend &lt; 0">
                  <xsl:value-of select="'Withholding Tax'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <Description>            
              <xsl:value-of select="$varDescription"/>
            </Description>


            <xsl:variable name="varActivityType">
              <xsl:choose>
                <xsl:when test="(COL6 ='DividendGross' or COL6='DividendSubstitute') and $varDividend &gt; 0">
                  <xsl:value-of select="'DividendExpense'"/>
                </xsl:when>
                <xsl:when test="(COL6 ='DividendGross' or COL6='DividendSubstitute') and $varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
                <xsl:when test="COL6 ='DividendTax' and $varDividend &lt; 0">
                  <xsl:value-of select="'WithholdingTax'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <ActivityType>
              <xsl:value-of select="$varActivityType"/>
            </ActivityType>
            
            

            <xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test="$varCurrencyName ='USD'">
                  <xsl:value-of select="1"/>
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
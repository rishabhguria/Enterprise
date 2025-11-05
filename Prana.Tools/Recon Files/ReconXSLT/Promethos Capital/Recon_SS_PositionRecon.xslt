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
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01' ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='06' ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01' ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02' ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03' ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
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
      <xsl:value-of select="normalize-space(COL9)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDate">
      <xsl:value-of select="normalize-space(substring-after(substring-before(COL12,' '),' '))"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring-before(substring-after(normalize-space(COL12),'/'),'/')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring-before(substring-after(normalize-space(COL12),' '),'/')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL12),'/'),'/'),' ')"/>
    </xsl:variable>

    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(substring-before(normalize-space(COL12),' '),1,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring-after(substring-after(normalize-space(COL12),'@'),' '),'##.00')"/>
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
            <xsl:with-param name="Number" select="COL206"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Quantity) and COL4='EQUITY'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'StateStreet'"/>
            </xsl:variable>

            <xsl:variable name ="PB_FUND_NAME">
              <xsl:value-of select ="COL85"/>
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
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccountName>


           

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL203)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="substring-before(normalize-space(COL3),' ')"/>
            </xsl:variable>
			
			<xsl:variable name="varSedol">
             <xsl:value-of select="substring(COL204,2)"/>
           </xsl:variable>

        <xsl:variable name="varISIN">
             <xsl:value-of select="COL100"/>
           </xsl:variable>
		   
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
				
				<xsl:when test="$varSedol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
				<xsl:when test="$varISIN!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
			
			 <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
				<xsl:when test="$varSedol!=''">
                  <xsl:value-of select="$varSedol"/>
                </xsl:when>
				
				<xsl:when test="$varISIN!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>
			
			 <!-- <ISIN> -->
              <!-- <xsl:choose> -->
                <!-- <xsl:when test="$PRANA_SYMBOL_NAME!=''"> -->
                  <!-- <xsl:value-of select="''"/> -->
                <!-- </xsl:when> -->
				
				<!-- <xsl:when test="$varISIN!=''"> -->
                  <!-- <xsl:value-of select="$varISIN"/> -->
                <!-- </xsl:when> -->
				
				<!-- <xsl:when test="$varSedol!=''"> -->
                  <!-- <xsl:value-of select="''"/> -->
                <!-- </xsl:when> -->
                
                <!-- <xsl:when test="$varSymbol!=''"> -->
                  <!-- <xsl:value-of select="''"/> -->
                <!-- </xsl:when> -->

                <!-- <xsl:otherwise> -->
                  <!-- <xsl:value-of select="''"/> -->
                <!-- </xsl:otherwise> -->
              <!-- </xsl:choose> -->
            <!-- </ISIN> -->

 <ISINSymbol>
 <xsl:value-of select="$varISIN"/>
 </ISINSymbol>

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

            <Side>              
                  <xsl:choose>
                    <xsl:when test="$Quantity &gt; 0">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="$Quantity &lt; 0">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>                
            </Side>


            <xsl:variable name="varNetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL137"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="number($varNetNotionalValue)">
                  <xsl:value-of select="$varNetNotionalValue"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="varNetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="number($varNetNotionalValueBase)">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>

			
			<xsl:variable name="varCostbasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL148"/>
              </xsl:call-template>
            </xsl:variable>
			
			<xsl:variable name="varCostbasis2">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>
			
			<xsl:variable name="Costbasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL35"/>
              </xsl:call-template>
            </xsl:variable>
			
			<UnitCost>
              <xsl:choose>
                <xsl:when test="$Costbasis &gt; 0">
                  <xsl:value-of select="$Costbasis"/>
                </xsl:when>
                <xsl:when test="$Costbasis &lt; 0">
                  <xsl:value-of select="$Costbasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </UnitCost>
			
			<xsl:variable name="FXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="1 div COL14"/>
              </xsl:call-template>
			 </xsl:variable>
			
			<FXRate>
				<!-- <xsl:value-of select="format-number($FXRate,'##.0000')"/> -->
				<xsl:value-of select="''"/>
			</FXRate>

            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL141"/>
              </xsl:call-template>

            </xsl:variable>
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


				<xsl:variable name="MarkPriceBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="$MarkPrice * $FXRate"/>
              </xsl:call-template>
			  </xsl:variable>
			  
			<MarkPriceBase>
              <xsl:choose>
                <xsl:when test="$MarkPriceBase &gt; 0">
                  <xsl:value-of select="$MarkPriceBase"/>
                </xsl:when>
                <xsl:when test="$MarkPriceBase &lt; 0">
                  <xsl:value-of select="$MarkPriceBase * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPriceBase>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL132"/>
              </xsl:call-template>
            </xsl:variable>
            <MarketValue>
              <xsl:choose>
                <xsl:when test="number($MarketValue)">
                  <xsl:value-of select="$MarketValue"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>


            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL21"/>
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

            <xsl:variable name="varOrgDate">
              <xsl:value-of select="COL27"/>
            </xsl:variable>
            <OriginalPurchaseDate>
              <xsl:value-of select="$varOrgDate"/>
            </OriginalPurchaseDate>
			
			

			<CurrencySymbol>
				 <xsl:value-of select="COL215"/>
			</CurrencySymbol>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

           
            <TradeDate>
              <xsl:value-of select="''"/>
            </TradeDate>

            

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
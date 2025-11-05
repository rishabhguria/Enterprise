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
        <xsl:when test="$Month=07 ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08 ">
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


  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>

    <xsl:variable name="varUSymbol">
      <xsl:value-of select="substring-before($varBBCode,' ')"/>
    </xsl:variable>

    <xsl:variable name="varPutORCall">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),7,1)"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),1,2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring(substring-after(normalize-space($varBBCode),' '),8) div 1000,'#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),5,2)"/>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring(substring-after(normalize-space($varBBCode),' '),3,2)"/>
    </xsl:variable>


    <xsl:variable name="MonthCodeVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$varMonthCode"/>
        <xsl:with-param name="PutOrCall" select="$varPutORCall"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="varExpiryDay">
      <xsl:choose>
        <xsl:when test="substring($varExDay,1,1)= '0'">
          <xsl:value-of select="substring($varExDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varExDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="normalize-space(concat('O:',$varUSymbol,' ',$varExYear,$MonthCodeVar,$varStrike,'D',$varExpiryDay))"/>
  </xsl:template>

  <xsl:template name="varMonthTemplate">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Mar'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Apr'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$varMonth='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test=" $varMonth='Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Jul'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="DateFormate">
    <xsl:param name="Date"/>
    <xsl:variable name="varMonthConvert">
      <xsl:call-template name="varMonthTemplate">
        <xsl:with-param name="varMonth" select="substring-before(substring-after($Date,'-'),'-')"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Date,'-')">
        <xsl:value-of select="concat($varMonthConvert,'/',substring-before($Date,'-'),'/',substring-after(substring-after($Date,'-'),'-'))"/>
      </xsl:when>
      <xsl:when test="contains($Date,'/')">
        <xsl:value-of select="concat(substring-before($Date,'/'),'/',substring-before(substring-after($Date,'/'),'/'),'/',substring-after(substring-after($Date,'/'),'/'))"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Apex'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY_NAME" >
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="string-length(COL8) &gt; 10">
                  <xsl:call-template name="ConvertBBCodetoTicker">
                    <xsl:with-param name="varBBCode" select="COL8"/>
                  </xsl:call-template>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <!--<xsl:variable name="PB_FUND_NAME" select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25))"/>-->

            <xsl:variable name="PB_FUND_NAME">
              <xsl:choose>
                <xsl:when test="normalize-space(COL26)='' or normalize-space(COL26)='*'">
                  <xsl:value-of select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat(normalize-space(COL22),'-',normalize-space(COL23),'-',normalize-space(COL24),'-',normalize-space(COL25),'-',normalize-space(COL26))"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

              <!--<xsl:value-of select ="'Papyrus Capital Fund LP'"/>-->
            </AccountName>

            <Quantity>
              <xsl:choose>
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>

                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * -1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>
            <AvgPX>
              <xsl:choose>
                <xsl:when test="$AvgPrice &gt; 0">
                  <xsl:value-of select="$AvgPrice"/>

                </xsl:when>
                <xsl:when test="$AvgPrice &lt; 0">
                  <xsl:value-of select="$AvgPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </AvgPX>

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>

                </xsl:when>
                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Commission>


            <!--<xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <MiscFees>
              <xsl:choose>
                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>
                </xsl:when>
                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>-->

            <!--<xsl:variable name="varOtherBrokerFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
              </xsl:call-template>
            </xsl:variable>
            <OtherBrokerFees>
              <xsl:choose>
                <xsl:when test="$varOtherBrokerFees &gt; 0">
                  <xsl:value-of select="$varOtherBrokerFees"/>

                </xsl:when>
                <xsl:when test="$varOtherBrokerFees &lt; 0">
                  <xsl:value-of select="$varOtherBrokerFees * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </OtherBrokerFees>-->

            <xsl:variable name="SecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>

            <SecFee>
              <xsl:choose>
                <xsl:when test="$SecFee &gt; 0">
                  <xsl:value-of select="$SecFee"/>

                </xsl:when>
                <xsl:when test="$SecFee &lt; 0">
                  <xsl:value-of select="$SecFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </SecFee>

            <!--<xsl:variable name="varClearingFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <ClearingFee>
              <xsl:choose>
                <xsl:when test="$varClearingFee &gt; 0">
                  <xsl:value-of select="$varClearingFee"/>
                </xsl:when>
                <xsl:when test="$varClearingFee &lt; 0">
                  <xsl:value-of select="$varClearingFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </ClearingFee>-->

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate"> 
                <xsl:with-param name="Number" select="COL21"/> 
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$NetNotionalValue &gt; 0">
                  <xsl:value-of select="$NetNotionalValue"/> 
                </xsl:when>
                <xsl:when test="$NetNotionalValue &lt; 0">
                  <xsl:value-of select="$NetNotionalValue * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>


            <!--<xsl:variable name="varNetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL21"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>-->

            <!--<xsl:variable name="varCurrency">
              <xsl:value-of select="COL10"/>
            </xsl:variable>
            <CurrencySymbol>
              <xsl:value-of select="$varCurrency"/>
            </CurrencySymbol>-->

            <!--<xsl:variable name="varTradeDateYear" select="substring(COL10,1,4)"/>
            <xsl:variable name="varTradeDateMonth" select="substring(COL10,5,2)"/>
            <xsl:variable name="varTradeDateDay" select="substring(COL10,7,2)"/>-->
            <!-- <TradeDate> -->
              <!-- <xsl:value-of select="COL5"/> -->
            <!-- </TradeDate> -->

            <!--<xsl:variable name="varSettlementDateYear" select="substring(COL11,1,4)"/>
            <xsl:variable name="varSettlementDateMonth" select="substring(COL11,5,2)"/>
            <xsl:variable name="varSettlementDateDay" select="substring(COL11,7,2)"/>-->
			
			<xsl:variable name="varSDay">
              <xsl:value-of select="substring-before(substring-after(COL6,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varSYear">
              <xsl:value-of select="substring-after(substring-after(COL6,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varSMonth">
              <xsl:value-of select="substring-before(COL6,'/')"/>
            </xsl:variable>
            <SettlementDate>
              <xsl:value-of select="concat($varSMonth,'/',$varSDay,'/',$varSYear)"/>
            </SettlementDate>
			
			<xsl:variable name="varDay">
              <xsl:value-of select="substring-before(substring-after(COL5,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring-after(substring-after(COL5,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring-before(COL5,'/')"/>
            </xsl:variable>
            <TradeDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </TradeDate>
			
			

            <Side>
              <xsl:choose>
                <xsl:when  test="COL4='B'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>

                <xsl:when  test="COL4='OS'">
                  <xsl:value-of select="'Sell to Open'"/>
                </xsl:when>

                <xsl:when  test="COL4='SS'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>

                <xsl:when  test="COL4='CB'">
                  <xsl:value-of select="'Buy to Close'"/>
                </xsl:when>
				 <xsl:when  test="COL4='CS'">
                  <xsl:value-of select="'Sell to Close'"/>
                </xsl:when>
				 <xsl:when  test="COL4='OB'">
                  <xsl:value-of select="'Buy to Open'"/>
                </xsl:when>
				 <xsl:when  test="COL4='S'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
				 <xsl:when  test="COL4='SB'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when> 
				<xsl:when  test="COL4='SSP'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
				
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>
			
			<xsl:variable name="PB_CountnerParty" select="normalize-space(COL27)"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>
            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="contains(COL27,'ARCA')">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="contains(COL27,'CHBC')">
                  <xsl:value-of select="'7'"/>
                </xsl:when>
				<xsl:when test="contains(COL27,'DEAN')">
                  <xsl:value-of select="'9'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'DFIN')">
                  <xsl:value-of select="'11'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'ETRS')">
                  <xsl:value-of select="'12'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'FOGS')">
                  <xsl:value-of select="'14'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'NON')">
                  <xsl:value-of select="'15'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'OCC')">
                  <xsl:value-of select="'16'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'PUMX')">
                  <xsl:value-of select="'18'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'QNTX')">
                  <xsl:value-of select="'19'"/>
                </xsl:when>
				<xsl:when test="contains(COL27,'WEXM')">
                  <xsl:value-of select="'20'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'RDBN')">
                  <xsl:value-of select="'13'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'ISIG')">
                  <xsl:value-of select="'17'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL27,'WELX')">
                  <xsl:value-of select="'19'"/>
                </xsl:when>
				
			
                <xsl:when test="$PRANA_CounterPartyID !=''">
                  <xsl:value-of select ="$PRANA_CounterPartyID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>

            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>



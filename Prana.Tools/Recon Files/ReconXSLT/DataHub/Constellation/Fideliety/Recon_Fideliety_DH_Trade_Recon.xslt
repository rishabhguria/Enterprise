<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">

    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }

  </msxsl:script>

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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
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
        <xsl:when test="$Month='06'">
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
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
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
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(substring(COL1,874,1),'P') or contains(substring(COL1,874,1),'C')">

      <xsl:variable name="varUnderlying">
        <xsl:value-of select="normalize-space(substring(COL1,862,5))"/>
      </xsl:variable>

      <xsl:variable name="varExYear">
        <xsl:value-of select="substring(COL1,868,2)"/>
      </xsl:variable>

      <xsl:variable name="varExDay">
        <xsl:value-of select="substring(COL1,872,2)"/>
      </xsl:variable>

      <xsl:variable name="varMonthCode">
        <xsl:value-of select="substring(COL1,870,2)"/>
      </xsl:variable>

      <xsl:variable name="varStrike">
        <xsl:value-of select="format-number(substring(COL1,875,7) div 100, '#.00')"/>
      </xsl:variable>


      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(COL1,874,1)"/>
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

      <xsl:variable name="varMonth">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="$varMonthCode"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>

      <xsl:value-of select="normalize-space(concat('O:', $varUnderlying, ' ', $varExYear,$varMonth,$varStrike,'D',$varExpiryDay))"/>
    </xsl:if>
  </xsl:template>



  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>



  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="substring(COL1,219,16)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Fidelity'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="substring(COL1,399,19)"/>
              </xsl:variable>


              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>


              <Asset>
                <xsl:value-of select="''"/>
              </Asset>

              <xsl:variable name="Symbol" select="normalize-space(substring(COL1,100,15))"/>

              <xsl:variable name="varSEDOL">
                <xsl:value-of select="normalize-space(substring(COL1,1262,14))"/>
              </xsl:variable>

              <xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="substring(COL1,874,1) ='C' or substring(COL1,874,1) ='P'">
                    <xsl:value-of select="'EquityOption'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:call-template name="Option">
                      <xsl:with-param name="Symbol" select="normalize-space(substring(COL1,862,5))"/>
                      <xsl:with-param name="Suffix" select="''"/>
                    </xsl:call-template>
                  </xsl:when>

                  <xsl:when test="$Symbol =''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$Symbol!=''">
                    <xsl:value-of select="$Symbol"/>
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

                  <xsl:when test="$Asset='EquityOption'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$Symbol =''">
                    <xsl:value-of select="$varSEDOL"/>
                  </xsl:when>

                  <xsl:when test="$Symbol!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>





              <xsl:variable name="PB_FUND_NAME" select="substring(COL1,31,6)"/>

              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <xsl:variable name="PB_BROKER_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>
              <xsl:variable name="PRANA_BROKER_ID">
                <xsl:value-of select="document('../../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
              </xsl:variable>

              <CounterPartyID>
                <xsl:choose>
                  <xsl:when test="number($PRANA_BROKER_ID)">
                    <xsl:value-of select="$PRANA_BROKER_ID"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
                <!--<xsl:value-of select="1"/>-->
              </CounterPartyID>


              <xsl:variable name="varQuantitye">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="$Quantity div 10000"/>
                </xsl:call-template>
              </xsl:variable>
              <Quantity>
                <xsl:choose>
                  <xsl:when test="$varQuantitye &gt; 0">
                    <xsl:value-of select="$varQuantitye"/>
                  </xsl:when>

                  <xsl:when test="$varQuantitye &lt; 0">
                    <xsl:value-of select="$varQuantitye * -1"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <xsl:variable name="AvgPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,236,17) div 100000000"/>
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
                  <xsl:with-param name="Number" select="substring(COL1,319,10) div 100"/>
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

              <xsl:variable name="OrfFee">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,337,7)  div 100"/>
                </xsl:call-template>
              </xsl:variable>
              <SecFee>
                <xsl:choose>
                  <xsl:when test="$OrfFee &gt; 0">
                    <xsl:value-of select="$OrfFee"/>
                  </xsl:when>
                  <xsl:when test="$OrfFee &lt; 0">
                    <xsl:value-of select="$OrfFee * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SecFee>

              <xsl:variable name="varFXRate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,1464,15) div 10000000"/>
                </xsl:call-template>
              </xsl:variable>
              <xsl:variable name="varNFXRate">
                <xsl:choose>
                  <xsl:when test="substring(COL1,1277,3) !='USD'">
                    <xsl:value-of select="$varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <FxRate>
                <xsl:choose>
                  <xsl:when test="number($varNFXRate)">
                    <xsl:value-of select="$varNFXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FxRate>

              <xsl:variable name="AdditionalFee1">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="AdditionalFee2">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>


              <xsl:variable name="OtherBrokerFees">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <Fees>
                <xsl:choose>
                  <xsl:when test="OtherBrokerFees &gt; 0">
                    <xsl:value-of select="OtherBrokerFees"/>
                  </xsl:when>
                  <xsl:when test="OtherBrokerFees &lt; 0">
                    <xsl:value-of select="OtherBrokerFees * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Fees>

              <xsl:variable name="ClearingFee">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,1057,6) div 10"/>
                </xsl:call-template>
              </xsl:variable>
              <ClearingFee>
                <xsl:choose>
                  <xsl:when test="$ClearingFee &gt; 0">
                    <xsl:value-of select="$ClearingFee"/>
                  </xsl:when>
                  <xsl:when test="$ClearingFee &lt; 0">
                    <xsl:value-of select="$ClearingFee * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingFee>


              <xsl:variable name="AUECFee1">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <AUECFee1>
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
              </AUECFee1>

              <xsl:variable name="AUECFee2">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <AUECFee2>
                <xsl:choose>
                  <xsl:when test="$AUECFee2 &gt; 0">
                    <xsl:value-of select="$AUECFee2"/>
                  </xsl:when>
                  <xsl:when test="$AUECFee2 &lt; 0">
                    <xsl:value-of select="$AUECFee2 * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AUECFee2>

              <xsl:variable name="StampDuty">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <StampDuty>
                <xsl:choose>
                  <xsl:when test="$StampDuty &gt; 0">
                    <xsl:value-of select="$StampDuty"/>
                  </xsl:when>
                  <xsl:when test="$StampDuty &lt; 0">
                    <xsl:value-of select="$StampDuty * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </StampDuty>



              <UnderlyingSymbol>
                <xsl:value-of select="''"/>
              </UnderlyingSymbol>

              <Bloomberg>
                <xsl:value-of select="''"/>
              </Bloomberg>

              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>

              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>



              <xsl:variable name="GrossNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,299,9) div 10000"/>
                </xsl:call-template>
              </xsl:variable>
              <GrossNotionalValue>
                <xsl:choose>
                  <xsl:when test="$GrossNotionalValue &gt; 0">
                    <xsl:value-of select="$GrossNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$GrossNotionalValue &lt; 0">
                    <xsl:value-of select="$GrossNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </GrossNotionalValue>

              <xsl:variable name="GrossNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <GrossNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$GrossNotionalValueBase &gt; 0">
                    <xsl:value-of select="$GrossNotionalValueBase"/>
                  </xsl:when>
                  <xsl:when test="$GrossNotionalValueBase &lt; 0">
                    <xsl:value-of select="$GrossNotionalValueBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </GrossNotionalValueBase>



              <xsl:variable name="NetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,363,14) div 10"/>
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

              <xsl:variable name="NetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValueBase &gt; 0">
                    <xsl:value-of select="$NetNotionalValueBase"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValueBase &lt; 0">
                    <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>


              <xsl:variable name="TotalCommissionandFees">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="$Commission"/>
                </xsl:call-template>
              </xsl:variable>

              <TotalCommissionandFees>
                <xsl:choose>
                  <xsl:when test="$TotalCommissionandFees &gt; 0">
                    <xsl:value-of select="$TotalCommissionandFees"/>
                  </xsl:when>
                  <xsl:when test="$TotalCommissionandFees &lt; 0">
                    <xsl:value-of select="$TotalCommissionandFees * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCommissionandFees>

              <xsl:variable name="TotalCommissionandFeesBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>

              <TotalCommissionandFeesBase>
                <xsl:choose>
                  <xsl:when test="$TotalCommissionandFeesBase &gt; 0">
                    <xsl:value-of select="$TotalCommissionandFeesBase"/>
                  </xsl:when>
                  <xsl:when test="$TotalCommissionandFeesBase &lt; 0">
                    <xsl:value-of select="$TotalCommissionandFeesBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCommissionandFeesBase>


              <xsl:variable name="ClearingBrokerFeeBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <ClearingBrokerFeeBase>
                <xsl:choose>
                  <xsl:when test="$ClearingBrokerFeeBase &gt; 0">
                    <xsl:value-of select="$ClearingBrokerFeeBase"/>
                  </xsl:when>
                  <xsl:when test="$ClearingBrokerFeeBase &lt; 0">
                    <xsl:value-of select="$ClearingBrokerFeeBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingBrokerFeeBase>


              <xsl:variable name="SoftCommission">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SoftCommission>
                <xsl:choose>
                  <xsl:when test="$SoftCommission &gt; 0">
                    <xsl:value-of select="$SoftCommission"/>
                  </xsl:when>
                  <xsl:when test="$SoftCommission &lt; 0">
                    <xsl:value-of select="$SoftCommission * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SoftCommission>


              <xsl:variable name="TaxOnCommissions">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <TaxOnCommissions>
                <xsl:choose>
                  <xsl:when test="$TaxOnCommissions &gt; 0">
                    <xsl:value-of select="$TaxOnCommissions"/>
                  </xsl:when>
                  <xsl:when test="$TaxOnCommissions &lt; 0">
                    <xsl:value-of select="$TaxOnCommissions * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </TaxOnCommissions>


              <xsl:variable name="UnitCost">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <UnitCost>
                <xsl:choose>
                  <xsl:when test="$UnitCost &gt; 0">
                    <xsl:value-of select="$UnitCost"/>
                  </xsl:when>
                  <xsl:when test="$UnitCost &lt; 0">
                    <xsl:value-of select="$UnitCost * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </UnitCost>


              <BaseCurrency>
                <xsl:value-of select="'USD'"/>
              </BaseCurrency>


              <SettlCurrency>
                <xsl:value-of select="''"/>
              </SettlCurrency>


              <xsl:variable name="SettlCurrFxRate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlCurrFxRate>
                <xsl:choose>
                  <xsl:when test="number($SettlCurrFxRate)">
                    <xsl:value-of select="$SettlCurrFxRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SettlCurrFxRate>


              <xsl:variable name="SettlCurrAmt">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlCurrAmt>

                <xsl:choose>
                  <xsl:when test="number($SettlCurrAmt)">
                    <xsl:value-of select="$SettlCurrAmt"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlCurrAmt>


              <xsl:variable name="SettlPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <SettlPrice>

                <xsl:choose>
                  <xsl:when test="number($SettlPrice)">
                    <xsl:value-of select="$SettlPrice"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlPrice>

              <xsl:variable name="MiscFees">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <MiscFees>
                <xsl:choose>
                  <xsl:when test="$MiscFees &gt; 0">
                    <xsl:value-of select="$MiscFees"/>
                  </xsl:when>
                  <xsl:when test="$MiscFees &lt; 0">
                    <xsl:value-of select="$MiscFees * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MiscFees>

              <xsl:variable name="SecFees">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="substring(COL1,337,7)  div 10"/>
                </xsl:call-template>
              </xsl:variable>
              <SecFees>
                <xsl:choose>
                  <xsl:when test="$SecFees &gt; 0">
                    <xsl:value-of select="$SecFees"/>
                  </xsl:when>
                  <xsl:when test="$SecFees &lt; 0">
                    <xsl:value-of select="$SecFees * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SecFees>



              <OriginalPurchaseDate>
                <xsl:value-of select ="''"/>
              </OriginalPurchaseDate>


              <xsl:variable name="Year">
                <xsl:value-of select="substring(COL1,48,4)"/>
              </xsl:variable>
              <xsl:variable name="Month">
                <xsl:value-of select="substring(COL1,52,2)"/>
              </xsl:variable>
              <xsl:variable name="Day">
                <xsl:value-of select="substring(COL1,54,2)"/>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select ="concat($Month,'/',$Day,'/',$Year)"/>
              </TradeDate>

              <xsl:variable name="Year1">
                <xsl:value-of select="substring(COL5,1,4)"/>
              </xsl:variable>
              <xsl:variable name="Month1">
                <xsl:value-of select="substring(COL5,5,2)"/>
              </xsl:variable>
              <xsl:variable name="Day1">
                <xsl:value-of select="substring(COL5,7,2)"/>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select ="''"/>
              </SettlementDate>


              <ExpirationDate>
                <xsl:value-of select ="''"/>
              </ExpirationDate>

              <ProcessDate>
                <xsl:value-of select ="''"/>
              </ProcessDate>


              <CurrencySymbol>
                <xsl:value-of select ="''"/>
              </CurrencySymbol>



				<xsl:variable name="Side">
					<xsl:value-of select="substring(COL1,936,2)"/>
				</xsl:variable>
              <Side>
				  <xsl:choose>
					  <xsl:when test="$Asset='EquityOption'">
						  <xsl:choose>
							  <xsl:when test="contains(substring(COL1,479,7),'CLOSING')">
								  <xsl:choose>
									  <xsl:when test="$Side='BU'">
										  <xsl:value-of select="'Buy to Close'"/>
									  </xsl:when>
									  <xsl:when test="$Side='SL'">
										  <xsl:value-of select="'Sell to Close'"/>
									  </xsl:when>
								  </xsl:choose>
							  </xsl:when>
							  <xsl:when test="contains(substring(COL1,479,7),'OPENING')">
								  <xsl:choose>
									  <xsl:when test="$Side='BU'">
										  <xsl:value-of select="'Buy to Open'"/>
									  </xsl:when>
									  <xsl:when test="$Side='SL'">
										  <xsl:value-of select="'Sell to Open'"/>
									  </xsl:when>
								  </xsl:choose>
							  </xsl:when>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$Side='BU' ">
								  <xsl:value-of select="'Buy'"/>
							  </xsl:when>
							  <xsl:when test="$Side='SL'">
								  <xsl:value-of select="'Sell'"/>
							  </xsl:when>
							  <!--<xsl:when test="$Side='BU'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='SL'and COL13='3'">
											<xsl:value-of select="'5'"/>
										</xsl:when>-->
							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>

			  </Side>

			  <PBSymbol>
				  <xsl:value-of select="$PB_SYMBOL_NAME"/>
			  </PBSymbol>

			  <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>



            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>

              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>



              <FundName>
                <xsl:value-of select="''"/>
              </FundName>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>


              <Commission>
                <xsl:value-of select="0"/>
              </Commission>

              <SecFees>
                <xsl:value-of select="0"/>
              </SecFees>
              <FxRate>
                <xsl:value-of select="0"/>
              </FxRate>


              <Fees>
                <xsl:value-of select="0"/>
              </Fees>


              <ClearingFee>
                <xsl:value-of select="0"/>
              </ClearingFee>


              <AUECFee1>
                <xsl:value-of select="0"/>
              </AUECFee1>


              <AUECFee2>
                <xsl:value-of select="0"/>
              </AUECFee2>


              <StampDuty>
                <xsl:value-of select="0"/>
              </StampDuty>


              <UnderlyingSymbol>
                <xsl:value-of select="''"/>
              </UnderlyingSymbol>

              <Bloomberg>
                <xsl:value-of select="''"/>
              </Bloomberg>

              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>

              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>

              <Asset>
                <xsl:value-of select="''"/>
              </Asset>


              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>


              <GrossNotionalValue>
                <xsl:value-of select="0"/>
              </GrossNotionalValue>

              <GrossNotionalValueBase>
                <xsl:value-of select="0"/>
              </GrossNotionalValueBase>

              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>

              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>

              <TotalCommissionandFees>
                <xsl:value-of select="0"/>
              </TotalCommissionandFees>

              <TotalCommissionandFeesBase>
                <xsl:value-of select="0"/>
              </TotalCommissionandFeesBase>

              <ClearingBrokerFeeBase>
                <xsl:value-of select="0"/>
              </ClearingBrokerFeeBase>

              <SoftCommission>
                <xsl:value-of select="0"/>
              </SoftCommission>

              <TaxOnCommissions>
                <xsl:value-of select="0"/>
              </TaxOnCommissions>


              <UnitCost>
                <xsl:value-of select="0"/>
              </UnitCost>

              <BaseCurrency>
                <xsl:value-of select="''"/>
              </BaseCurrency>

              <SettlCurrency>
                <xsl:value-of select="''"/>
              </SettlCurrency>

              <SettlCurrFxRate>
                <xsl:value-of select="0"/>
              </SettlCurrFxRate>


              <SettlCurrAmt>
                <xsl:value-of select="0"/>
              </SettlCurrAmt>

              <SettlPrice>
                <xsl:value-of select="0"/>
              </SettlPrice>

              <MiscFees>
                <xsl:value-of select="0"/>
              </MiscFees>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

              <OriginalPurchaseDate>
                <xsl:value-of select="''"/>
              </OriginalPurchaseDate>

              <SettlementDate>
                <xsl:value-of select="''"/>
              </SettlementDate>

              <ExpirationDate>
                <xsl:value-of select="''"/>
              </ExpirationDate>

              <ProcessDate>
                <xsl:value-of select="''"/>
              </ProcessDate>

              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>

              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>


            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>



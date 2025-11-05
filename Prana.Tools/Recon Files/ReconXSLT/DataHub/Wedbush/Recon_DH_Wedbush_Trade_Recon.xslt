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

  <xsl:template name="MonthNo">
    <xsl:param name="varMonth"/>

    <xsl:choose>
      <xsl:when test ="$varMonth= 'A' or $varMonth= 'M'">
        <xsl:value-of select ="1"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'B' or $varMonth= 'N'">
        <xsl:value-of select ="2"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'C' or $varMonth= 'O'">
        <xsl:value-of select ="3"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'D' or $varMonth= 'P'">
        <xsl:value-of select ="4"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'E' or $varMonth= 'Q'">
        <xsl:value-of select ="5"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'F' or $varMonth= 'R'">
        <xsl:value-of select ="6"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'G' or $varMonth= 'S'">
        <xsl:value-of select ="7"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'H' or $varMonth= 'T'">
        <xsl:value-of select ="8"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'I' or $varMonth= 'U'">
        <xsl:value-of select ="9"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'J' or $varMonth= 'V'">
        <xsl:value-of select ="10"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'K' or $varMonth= 'W'">
        <xsl:value-of select ="11"/>
      </xsl:when>
      <xsl:when test ="$varMonth= 'L' or $varMonth= 'X'">
        <xsl:value-of select ="12"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="ConvertBBCodetoTicker">
    <xsl:param name="varBBCode"/>

    <xsl:variable name="varRoot">
      <xsl:choose>
        <xsl:when test="substring(COL5,2,1) = '1'">
          <xsl:value-of select="substring-before(COL5,'1')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,2,1) = '2'">
          <xsl:value-of select="substring-before(COL5,'2')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,3,1) = '1'">
          <xsl:value-of select="substring-before(COL5,'1')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,3,1) = '2'">
          <xsl:value-of select="substring-before(COL5,'2')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,4,1) = '1'">
          <xsl:value-of select="substring-before(COL5,'1')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,4,1) = '2'">
          <xsl:value-of select="substring-before(COL5,'2')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,5,1) = '1'">
          <xsl:value-of select="substring-before(COL5,'1')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,5,1) = '2'">
          <xsl:value-of select="substring-before(COL5,'2')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,6,1) = '1'">
          <xsl:value-of select="substring-before(COL5,'1')"/>
        </xsl:when>
        <xsl:when test="substring(COL5,6,1) = '2'">
          <xsl:value-of select="substring-before(COL5,'2')"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varUnderlyingLength">
      <xsl:value-of select="string-length($varRoot)"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +6)), '#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring($varBBCode,($varUnderlyingLength +3),2)"/>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
    </xsl:variable>

    <xsl:variable name="varExMonth">
      <xsl:call-template name="MonthNo">
        <xsl:with-param name="varMonth" select="$varMonthCode"/>
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

    <xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>


  </xsl:template>

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



  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity) and  COL23='0' and (COL2='11520755' or COL2='24222066')">
            <PositionMaster>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL18"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name ="varCallPut">
                <xsl:choose>
                  <xsl:when test="COL19 = '*' and COL5 != ''">
                    <xsl:value-of select="1"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <xsl:variable name="varUnderlying">
                <xsl:choose>
                  <xsl:when test="COL5 != '' and COL5 != '*' and $varCallPut = 1">
                    <xsl:value-of select="substring-before(COL5,'1')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="normalize-space(COL5)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varUnderlyingLength">
                <xsl:value-of select="string-length($varUnderlying)"/>
              </xsl:variable>

              <xsl:variable name="varMonthCode">
                <xsl:if test ="$varCallPut = 1">
                  <xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
                </xsl:if>
              </xsl:variable>


              <Symbol>
                <xsl:choose>
                  <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                    <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varCallPut = 1">
                    <xsl:call-template name="ConvertBBCodetoTicker">
                      <xsl:with-param name="varBBCode" select="COL5"/>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varUnderlying"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>


              <PBSymbol>
                <xsl:if test="COL5 != '' and COL5 != '*'">
                  <xsl:value-of select="COL5"/>
                </xsl:if>
              </PBSymbol>




              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>

              <xsl:variable name="PB_FUND_NAME" select="COL2"/>

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
                  <xsl:with-param name="Number" select="COL6"/>
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
                  <xsl:with-param name="Number" select="COL15"/>
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

              <xsl:variable name="FxRate">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <FxRate>
                <xsl:choose>
                  <xsl:when test="number($FxRate)">
                    <xsl:value-of select="$FxRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FxRate>

              <xsl:variable name="OtherBrokerFee">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <Fees>
                <xsl:choose>
                  <xsl:when test="$OtherBrokerFee &gt; 0">
                    <xsl:value-of select="$OtherBrokerFee"/>
                  </xsl:when>
                  <xsl:when test="$OtherBrokerFee &lt; 0">
                    <xsl:value-of select="$OtherBrokerFee * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Fees>

              <xsl:variable name="ClearingFee">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
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
                  <xsl:with-param name="Number" select="COL12"/>
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



              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>



              <xsl:variable name="GrossNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
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
                  <xsl:with-param name="Number" select="COL17"/>
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
                  <xsl:with-param name="Number" select="''"/>
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

              <xsl:variable name ="Date" select="COL9"/>
              <TradeDate>
                <xsl:value-of select="$Date"/>
              </TradeDate>

              <OriginalPurchaseDate>
                <xsl:value-of select ="''"/>
              </OriginalPurchaseDate>


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



              <xsl:variable name="varSide">
                <xsl:value-of select="COL11"/>
              </xsl:variable>

              <Side>
                <xsl:choose>
                  <xsl:when test="COL3 = 'P' and COL34 = '2' and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'P' and COL34 = '6'  and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
                    <xsl:value-of select="'Buy to Close'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'S' and COL34 = '2'  and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'S'and COL34 = '6'   and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
                    <xsl:value-of select="'Sell short'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'SS' and COL34 = '2' and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'SS'and COL34 = '6'  and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
                    <xsl:value-of select="'Sell short'"/>
                  </xsl:when>
                  <!--For option trades-->
                  <xsl:when test="COL3 = 'P' and COL34 = '2' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
                    <xsl:value-of select="'Buy to Open'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'P' and COL34 = '6' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
                    <xsl:value-of select="'Buy to Close'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'S' and COL34 = '2' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
                    <xsl:value-of select="'Sell to Close'"/>
                  </xsl:when>
                  <xsl:when test="COL3 = 'S' and COL34 = '6' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
                    <xsl:value-of select="'Sell to Open'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              
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
                <xsl:value-of select="COL22"/>
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
                <xsl:value-of select="0"/>
              </BaseCurrency>

              <SettlCurrency>
                <xsl:value-of select="0"/>
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



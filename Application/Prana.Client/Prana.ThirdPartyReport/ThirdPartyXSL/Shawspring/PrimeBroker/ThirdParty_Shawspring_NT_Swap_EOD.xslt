<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public int RoundOff(double Qty)
    {

    return (int)Math.Round(Qty,0);
    }
  </msxsl:script>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before($Date,'/'),'/',substring-before(substring-after($Date,'/'),'/'),'/',substring-after(substring-after($Date,'/'),'/'))"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
    
      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>


        <FileHeader>
          <xsl:value-of select="'salse'"/>
        </FileHeader>

        <FileFooter>
          <xsl:value-of select="'true'"/>
        </FileFooter>


        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        
        <Instruction>
          <xsl:value-of select="'Instruction'"/>
        </Instruction>

        <NTAccoutnName>
          <xsl:value-of select="'NT Accoutn Name'"/>
        </NTAccoutnName>


        <NTAccount>
          <xsl:value-of select="'NT Account #'"/>
        </NTAccount>


        <SecurityDescription>
          <xsl:value-of select="'Security Description'"/>
        </SecurityDescription>


        <CUSIP>
          <xsl:value-of select="'Cusip/ISIN'"/>
        </CUSIP>

        <BuyorSell>
          <xsl:value-of select="'Buy or Sell'"/>
        </BuyorSell>


        <Units>
          <xsl:value-of select="'Units'"/>
        </Units>


        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>


        <SettleDate>
          <xsl:value-of select="'Settle Date'"/>
        </SettleDate>

        <Price>
          <xsl:value-of select="'Price(HKD)'"/>
        </Price>


        <Principal>
          <xsl:value-of select="'Principal(HKD)'"/>
        </Principal>


        <NetAmount>
          <xsl:value-of select="'NetAmount(HKD)'"/>
        </NetAmount>

        <InternalNetNotional>
          <xsl:value-of select ="0"/>
        </InternalNetNotional>
        <comm>
          <xsl:value-of select="'comm(HKD)'"/>
        </comm>


        <OtherFees>
          <xsl:value-of select="'Other Fees(HKD)'"/>
        </OtherFees>


       
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
      
      </ThirdPartyFlatFileDetail>

     
      <xsl:for-each select="ThirdPartyFlatFileDetail[CurrencySymbol='HKD']">

        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>


          <FileHeader>
            <xsl:value-of select="'salse'"/>
          </FileHeader>

          <FileFooter>
            <xsl:value-of select="'true'"/>
          </FileFooter>


          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <Instruction>
            <xsl:value-of select="'EQUITY INSTRUCTIONS'"/>
          </Instruction>
          
          
          <NTAccoutnName>
            <xsl:value-of select="concat(concat('=&quot;',AccountName),'&quot;')"/>
          </NTAccoutnName>


          <NTAccount>
            <xsl:value-of select="AccountNo"/>
          </NTAccount>


          <SecurityDescription>
            <xsl:value-of select="FullsecurityName"/>
          </SecurityDescription>


          <CUSIP>
            <xsl:choose>
              <xsl:when test="CUSIP!=''">
                <!--<xsl:value-of select="concat(concat('=&quot;',CUSIP),'&quot;')"/>-->
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </CUSIP>

          <BuyorSell>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SELL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'Sell Short'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'Buy to Close'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>

          </BuyorSell>

          <xsl:variable name ="Qty">
            <xsl:value-of select="my:RoundOff(AllocatedQty)"/>
          </xsl:variable>
          <Units>
            <xsl:choose>
              <xsl:when test="number($Qty)">
                <xsl:value-of select="$Qty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Units>


          <xsl:variable name="varTradeDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="TradeDate"/>
            </xsl:call-template>
          </xsl:variable>
          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <xsl:variable name="varSettlDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate"/>
            </xsl:call-template>
          </xsl:variable>
          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>



          <xsl:variable name="varSettFxAmt">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Trade ='M'">
                    <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AveragePrice"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varAvgPrice">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Price>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="$varAvgPrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Price>



          <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="Principal">
            <xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
          </xsl:variable>

          <xsl:variable name="PRINCIPAL">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Principal"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$Principal * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$Principal div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Principal>
            <xsl:choose>
              <xsl:when test="number($PRINCIPAL)">
                <xsl:value-of select="$PRINCIPAL"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Principal>


          <xsl:variable name ="varNetAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="NetAmount * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="NetAmount div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <NetAmount>
            <xsl:choose>
              <xsl:when test="number($varNetAmount)">
                <xsl:value-of select="$varNetAmount"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </NetAmount>

          <InternalNetNotional>
            <xsl:value-of select="$varNetAmount"/>
          </InternalNetNotional>


          <xsl:variable name="Commission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <xsl:variable name="COMMAMNT">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Commission"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$Commission * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$Commission div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <comm>
            <xsl:choose>
              <xsl:when test="number($COMMAMNT)">
                <xsl:value-of select="$COMMAMNT"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </comm>

          <xsl:variable name = "OthFees">
            <xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee)"/>
          </xsl:variable>

          <xsl:variable name ="varOtherFees">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$OthFees"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$OthFees * $varFXRate"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$OthFees div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <OtherFees>
            <xsl:choose>
              <xsl:when test="number($varOtherFees)">
                <xsl:value-of select="$varOtherFees"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </OtherFees>


          <EntityID>
            <xsl:value-of select="''"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>   
   
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>

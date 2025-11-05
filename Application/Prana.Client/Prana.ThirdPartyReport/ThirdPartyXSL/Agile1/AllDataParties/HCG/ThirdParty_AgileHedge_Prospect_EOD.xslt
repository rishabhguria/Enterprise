<?xml version="1.0" encoding="UTF-8"?>
<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


      <xsl:for-each select="ThirdPartyFlatFileDetail">


        <ThirdPartyFlatFileDetail>
          
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="true"/>
          </RowHeader>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>



          <xsl:variable name="PB_NAME" select="'AgileHedge'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="FundName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <!--Exercise / Assign Need To Ask-->

          <AccountNumber>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountNumber>

          <Strategy>
            <xsl:value-of select="''"/>
          </Strategy>


          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <BuySellIndicator>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'Buy'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CoverShort'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'SellShort'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close' or Side='Sell'">
                <xsl:value-of select="'Sell'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BuySellIndicator>

          <Ticker>
            <xsl:value-of select="Symbol"/>

          </Ticker>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <Issuer>
            <xsl:value-of select="''"/>
          </Issuer>

          <OtherDescription>
            <xsl:value-of select="FullSecurityName"/>
          </OtherDescription>

          <Coupon>
            <xsl:value-of select="Coupon"/>
          </Coupon>

          <xsl:variable name="Maturity">
            <xsl:choose>
              <xsl:when test="substring-after(substring-after(ExpirationDate,'/'),'/')='1800'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="ExpirationDate"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Maturity>
            <xsl:value-of select="$Maturity"/>
          </Maturity>

          <OriginalFace>
            <xsl:value-of select="''"/>
          </OriginalFace>

          <TradeQuantity>
            <xsl:value-of select="AllocatedQty"/>
          </TradeQuantity>

          <Factor>
            <xsl:value-of select="''"/>
          </Factor>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <xsl:variable name="FXRate">
            <xsl:choose>
              <xsl:when test="number(FXRate_Taxlot)">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>

              <xsl:when test="number(ForexRate)">
                <xsl:value-of select="ForexRate"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <TradeFXRate>
            <xsl:value-of select="$FXRate"/>
          </TradeFXRate>

          <TradeCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </TradeCurrency>

          <SettleCurrency>
            <xsl:value-of select="SettlCurrency"/>
          </SettleCurrency>

          <EffectiveYield>
            <xsl:value-of select="''"/>
          </EffectiveYield>



          <Commission>

            <xsl:choose>

              <xsl:when test="CommissionCharged &gt; 0">
                <xsl:value-of select="CommissionCharged"/>
              </xsl:when>

              <xsl:when test="CommissionCharged &lt; 0">
                <xsl:value-of select="CommissionCharged * (-1)"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>

            </xsl:choose>

          </Commission>

          <SecFee>
            <xsl:choose>

              <xsl:when test="StampDuty &gt; 0">
                <xsl:value-of select="StampDuty"/>
              </xsl:when>

              <xsl:when test="StampDuty &lt; 0">
                <xsl:value-of select="StampDuty * (-1)"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>

            </xsl:choose>
          </SecFee>

          <xsl:variable name="OtherFees" select="OtherBrokerFee + ClearingBrokerFee  + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>

          <OtherFees>
            <xsl:choose>

              <xsl:when test="$OtherFees &gt; 0">
                <xsl:value-of select="$OtherFees"/>
              </xsl:when>

              <xsl:when test="$OtherFees &lt; 0">
                <xsl:value-of select="$OtherFees * (-1)"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>

            </xsl:choose>
          </OtherFees>

          <AccruedInterest>
            <xsl:value-of select="AccruedInterest"/>
          </AccruedInterest>

          <NetTradeCash>
            <xsl:choose>

              <xsl:when test="NetAmount &gt; 0">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>

              <xsl:when test="NetAmount &lt; 0">
                <xsl:value-of select="NetAmount * (-1)"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>

            </xsl:choose>
          </NetTradeCash>

          <xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_BROKER_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <CounterPartyBroker>
            <!--<xsl:choose>
                <xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
                  <xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_BROKER_NAME"/>
                </xsl:otherwise>
              </xsl:choose>-->
            <xsl:value-of select="CounterParty"/>
          </CounterPartyBroker>


          <ClosingMethod>
            <xsl:value-of select="'HighCostBook'"/>
          </ClosingMethod>





          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>

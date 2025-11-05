<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


    <!--<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'SSP Fund Cowen PHW006826']">-->
      <xsl:for-each select="ThirdPartyFlatFileDetail">
    
        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
            <ThirdPartyFlatFileDetail>


              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>
          

              <xsl:variable name="PB_NAME" select="'SS&amp;C'"/>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>


              <TradeType>
                <xsl:choose>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select ="'Buy to Cover'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="Side"/>
                  </xsl:otherwise>
                </xsl:choose>

              </TradeType>

              <xsl:variable name="varTaxlotStateTx">
                <xsl:choose>
                  <xsl:when test="TaxLotState='Allocated'">
                    <xsl:value-of select ="'New'"/>
                  </xsl:when>
                  <xsl:when test="TaxLotState='Amended'">
                    <xsl:value-of select ="'Amended'"/>
                  </xsl:when>
                  <xsl:when test="TaxLotState='Deleted'">
                    <xsl:value-of select ="'Cancel'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="'Sent'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <Action>
                <xsl:value-of select="$varTaxlotStateTx"/>
              </Action>

              <Portfolio>
                <xsl:value-of select="'SHA'"/>
              </Portfolio>

              <LocationAccount>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </LocationAccount>

              <Strategy>
                <xsl:value-of select="'NONE'"/>
              </Strategy>


              <Investment>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-before(BBCode,' Equity')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Investment>


              <xsl:variable name="TradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <EventDate>
                <xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
              </EventDate>


              <xsl:variable name="TradeSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettleDate>
                <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'/',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'/',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
              </SettleDate>

              <ActualSettleDate>
                <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'/',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'/',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
              </ActualSettleDate>

              <Comments>
                <xsl:value-of select="''"/>
              </Comments>

              <UserTranID1>
                <xsl:value-of select="EntityID"/>
              </UserTranID1>

              <Quantity>
                <xsl:value-of select="OrderQty"/>
              </Quantity>


              <xsl:variable name="varSettFxAmt">
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:choose>
                      <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                        <xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varPrice">
                <xsl:choose>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varSettFxAmt"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <Price>
                <xsl:choose>
                  <xsl:when test="number($varPrice)">
                    <xsl:value-of select="format-number($varPrice,'##.######')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </Price>

              <NetCounterAmount>
                <xsl:value-of select="'CALC'"/>
              </NetCounterAmount>

              <xsl:variable name="varFXRate">
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Commission">
                <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </xsl:variable>

              <xsl:variable name="varCommission">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$Commission"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="$Commission * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='D'">
                    <xsl:value-of select="$Commission div $varFXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <TotCommission>
                <xsl:choose>
                  <xsl:when test="number($varCommission)">
                    <xsl:value-of select="format-number($varCommission,'##.####')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotCommission>

              <xsl:variable name="Sec">
                <xsl:value-of select="number(SecFee)"/>
              </xsl:variable>

              <xsl:variable name="varSec">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$Sec"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="$Sec * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='D'">
                    <xsl:value-of select="$Sec div $varFXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SecFeeAmount>
                <xsl:choose>
                  <xsl:when test="number(SecFee)">
                    <xsl:value-of select="format-number($varSec,'##.####')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SecFeeAmount>

              <PriceDenomination>
                <xsl:value-of select="SettlCurrency"/>
              </PriceDenomination>

              <CounterInvestment>
                <xsl:value-of select="CurrencySymbol"/>
              </CounterInvestment>

              <TradeFx>
                <xsl:choose>
                  <xsl:when test="FXRate_Taxlot='0'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="FXRate_Taxlot='1'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>

              </TradeFx>

              <xsl:variable name = "PB_CounterParty">
                <xsl:value-of select="CounterParty"/>
              </xsl:variable>

              <xsl:variable name="PRANA_BrokerCode">
                <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CounterParty]/@PBBroker"/>
              </xsl:variable>

              <Broker>
                <xsl:choose>
                  <xsl:when test="$PRANA_BrokerCode!=''">
                    <xsl:value-of select="concat($PRANA_BrokerCode,'-ALGO-AMER')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="concat($PB_CounterParty,'-ALGO-AMER')"/>
                  </xsl:otherwise>
                </xsl:choose>

              </Broker>

              <ExpenseAmt>
                <xsl:value-of select="''"/>
              </ExpenseAmt>

              <ExpenseCode>
                <xsl:value-of select="''"/>
              </ExpenseCode>

              <Trader>
                <xsl:value-of select="'cps-shawspring1'"/>
              </Trader>

              <NetTradeAmount>
                <xsl:value-of select="'CALC'"/>
              </NetTradeAmount>

              <AssetType>
                <xsl:choose>
                  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select ="'EQT'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Asset"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AssetType>

              <!--<xsl:variable name="NetAmount">
                <xsl:value-of select="NetAmount"/>
              </xsl:variable>-->

              <xsl:variable name="varNetAmount">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$varNetamount"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="$varNetamount * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='D'">
                    <xsl:value-of select="$varNetamount div $varFXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <USDSettlementAmount>
                <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
              </USDSettlementAmount>

              <Cusip>
                <xsl:value-of select="CUSIP"/>
              </Cusip>

              <Sedol>
                <xsl:value-of select="SEDOL"/>
              </Sedol>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>


                <TaxLotState>
                  <xsl:value-of select="'Deleted'"/>
                </TaxLotState>
                
                <xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(Side,'Buy')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                    </xsl:when>
                    <xsl:when test="contains(Side,'Sell')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
               
                <xsl:variable name="PB_NAME" select="'SS&amp;C'"/>

                <xsl:variable name = "PRANA_FUND_NAME">
                  <xsl:value-of select="AccountName"/>
                </xsl:variable>

                <xsl:variable name ="THIRDPARTY_FUND_CODE">
                  <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
                </xsl:variable>


                <TradeType>
                  <xsl:choose>
                    <xsl:when test="OldSide='Buy to Close'">
                      <xsl:value-of select ="'Buy to Cover'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="OldSide"/>
                    </xsl:otherwise>
                  </xsl:choose>

                </TradeType>

               

                <Action>
                  <xsl:value-of select ="'Cancel'"/>
                </Action>

                <Portfolio>
                  <xsl:value-of select="'SHA'"/>
                </Portfolio>

                <LocationAccount>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                      <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_FUND_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </LocationAccount>

                <Strategy>
                  <xsl:value-of select="'NONE'"/>
                </Strategy>




                <Investment>
                  <xsl:choose>
                    <xsl:when test="OldTransactionType='USD'">
                      <xsl:value-of select="Symbol"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(BBCode,' Equity')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Investment>


                <xsl:variable name="varOldTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <EventDate>
                  <xsl:value-of select="concat(substring-before($varOldTradeDate,'/'),'/',substring-before(substring-after($varOldTradeDate,'/'),'/'),'/',substring-after(substring-after($varOldTradeDate,'/'),'/'))"/>
                </EventDate>
                
                <xsl:variable name="varOldSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <SettleDate>
                  <xsl:value-of select="concat(substring-before($varOldSettlementDate,'/'),'/',substring-before(substring-after($varOldSettlementDate,'/'),'/'),'/',substring-after(substring-after($varOldSettlementDate,'/'),'/'))"/>
                </SettleDate>

                <ActualSettleDate>
                  <xsl:value-of select="concat(substring-before($varOldSettlementDate,'/'),'/',substring-before(substring-after($varOldSettlementDate,'/'),'/'),'/',substring-after(substring-after($varOldSettlementDate,'/'),'/'))"/>
                </ActualSettleDate>

                <Comments>
                  <xsl:value-of select="''"/>
                </Comments>

                <UserTranID1>
                  <xsl:value-of select="EntityID"/>
                </UserTranID1>

                <Quantity>
                  <xsl:value-of select="OldExecutedQuantity"/>
                </Quantity>


                <xsl:variable name="varSettFxAmt">
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != OldTransactionType">
                      <xsl:choose>
                        <xsl:when test="OldFXConversionMethodOperator ='M'">
                          <xsl:value-of select="OldAvgPrice * OldFXRate"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="OldAvgPrice div OldFXRate"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="OldAvgPrice"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="varPrice">
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency = OldTransactionType">
                      <xsl:value-of select="OldAvgPrice"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$varSettFxAmt"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <Price>
                  <xsl:choose>
                    <xsl:when test="number($varPrice)">
                      <xsl:value-of select="format-number($varPrice,'##.######')"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>

                </Price>

                <NetCounterAmount>
                  <xsl:value-of select="'CALC'"/>
                </NetCounterAmount>

                <xsl:variable name="varFXRate">
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != OldTransactionType">
                      <xsl:value-of select="OldFXRate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="Commission">
                  <xsl:value-of select="OldSoftCommission + OldSoftCommissionAmount"/>
                </xsl:variable>

                <xsl:variable name="varCommission">
                  <xsl:choose>
                    <xsl:when test="$varFXRate=0">
                      <xsl:value-of select="$Commission"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator ='M'">
                      <xsl:value-of select="$Commission * $varFXRate"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator ='D'">
                      <xsl:value-of select="$Commission div $varFXRate"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <TotCommission>
                  <xsl:choose>
                    <xsl:when test="number($varCommission)">
                      <xsl:value-of select="format-number($varCommission,'##.####')"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </TotCommission>

                <xsl:variable name="Sec">
                  <xsl:value-of select="number(OldSecFee)"/>
                </xsl:variable>

                <xsl:variable name="varSec">
                  <xsl:choose>
                    <xsl:when test="$varFXRate=0">
                      <xsl:value-of select="$Sec"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator ='M'">
                      <xsl:value-of select="$Sec * $varFXRate"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator ='D'">
                      <xsl:value-of select="$Sec div $varFXRate"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <SecFeeAmount>
                  <xsl:choose>
                    <xsl:when test="number(SecFee)">
                      <xsl:value-of select="format-number($varSec,'##.####')"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>

                </SecFeeAmount>

                <PriceDenomination>
                  <xsl:value-of select="OldSettlCurrency"/>
                </PriceDenomination>

                <CounterInvestment>
                  <xsl:value-of select="OldTransactionType"/>
                </CounterInvestment>

                <TradeFx>
                  <xsl:choose>
                    <xsl:when test="OldFXRate='0'">
                      <xsl:value-of select="''"/>
                    </xsl:when>

                    <xsl:when test="OldFXRate='1'">
                      <xsl:value-of select="''"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="OldFXRate"/>
                    </xsl:otherwise>
                  </xsl:choose>

                </TradeFx>

                <xsl:variable name = "PB_CounterParty">
                  <xsl:value-of select="CounterParty"/>
                </xsl:variable>

                <xsl:variable name="PRANA_BrokerCode">
                  <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CounterParty]/@PBBroker"/>
                </xsl:variable>

                <Broker>
                  <xsl:choose>
                    <xsl:when test="$PRANA_BrokerCode!=''">
                      <xsl:value-of select="concat($PRANA_BrokerCode,'-ALGO-AMER')"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="concat($PB_CounterParty,'-ALGO-AMER')"/>
                    </xsl:otherwise>
                  </xsl:choose>

                </Broker>

                <ExpenseAmt>
                  <xsl:value-of select="''"/>
                </ExpenseAmt>

                <ExpenseCode>
                  <xsl:value-of select="''"/>
                </ExpenseCode>

                <Trader>
                  <xsl:value-of select="'cps-shawspring1'"/>
                </Trader>

                <NetTradeAmount>
                  <xsl:value-of select="'CALC'"/>
                </NetTradeAmount>

                <AssetType>
                  <xsl:choose>
                    <xsl:when test="Asset='Equity'">
                      <xsl:value-of select ="'EQT'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="Asset"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </AssetType>

            
                <xsl:variable name="varNetAmount">
                  <xsl:choose>
                    <xsl:when test="$varFXRate=0">
                      <xsl:value-of select="$varOldNetAmount"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator ='M'">
                      <xsl:value-of select="$varOldNetAmount * $varFXRate"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator ='D'">
                      <xsl:value-of select="$varOldNetAmount div $varFXRate"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>


                <USDSettlementAmount>
                  <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
                </USDSettlementAmount>

                <Cusip>
                  <xsl:value-of select="CUSIP"/>
                </Cusip>

                <Sedol>
                  <xsl:value-of select="SEDOL"/>
                </Sedol>


                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail>
            </xsl:if>
            <ThirdPartyFlatFileDetail>


              <TaxLotState>
                <xsl:value-of select="'Allocated'"/>
              </TaxLotState>

            

              <xsl:variable name="PB_NAME" select="'SS&amp;C'"/>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>


              <TradeType>
                <xsl:choose>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select ="'Buy to Cover'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="Side"/>
                  </xsl:otherwise>
                </xsl:choose>

              </TradeType>

              <xsl:variable name="varTaxlotStateTx">
                <xsl:choose>
                  <xsl:when test="TaxLotState='Allocated'">
                    <xsl:value-of select ="'New'"/>
                  </xsl:when>
                  <xsl:when test="TaxLotState='Amended'">
                    <xsl:value-of select ="'Amended'"/>
                  </xsl:when>
                  <xsl:when test="TaxLotState='Deleted'">
                    <xsl:value-of select ="'Cancel'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <Action>
                <xsl:value-of select ="'New'"/>
              </Action>

              <Portfolio>
                <xsl:value-of select="'SHA'"/>
              </Portfolio>

              <LocationAccount>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </LocationAccount>

              <Strategy>
                <xsl:value-of select="'NONE'"/>
              </Strategy>


              <Investment>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-before(BBCode,' Equity')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Investment>


              <xsl:variable name="TradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <EventDate>
                <xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
              </EventDate>


              <xsl:variable name="TradeSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettleDate>
                <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'/',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'/',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
              </SettleDate>

              <ActualSettleDate>
                <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'/',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'/',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
              </ActualSettleDate>

              <Comments>
                <xsl:value-of select="''"/>
              </Comments>

              <UserTranID1>
                <xsl:value-of select="EntityID"/>
              </UserTranID1>

              <Quantity>
                <xsl:value-of select="OrderQty"/>
              </Quantity>


              <xsl:variable name="varSettFxAmt">
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:choose>
                      <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                        <xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varPrice">
                <xsl:choose>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varSettFxAmt"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <Price>
                <xsl:choose>
                  <xsl:when test="number($varPrice)">
                    <xsl:value-of select="format-number($varPrice,'##.######')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </Price>

              <NetCounterAmount>
                <xsl:value-of select="'CALC'"/>
              </NetCounterAmount>

              <xsl:variable name="varFXRate">
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Commission">
                <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </xsl:variable>

              <xsl:variable name="varCommission">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$Commission"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="$Commission * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='D'">
                    <xsl:value-of select="$Commission div $varFXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <TotCommission>
                <xsl:choose>
                  <xsl:when test="number($varCommission)">
                    <xsl:value-of select="format-number($varCommission,'##.####')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotCommission>

              <xsl:variable name="Sec">
                <xsl:value-of select="number(SecFee)"/>
              </xsl:variable>

              <xsl:variable name="varSec">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$Sec"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="$Sec * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='D'">
                    <xsl:value-of select="$Sec div $varFXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SecFeeAmount>
                <xsl:choose>
                  <xsl:when test="number(SecFee)">
                    <xsl:value-of select="format-number($varSec,'##.####')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </SecFeeAmount>

              <PriceDenomination>
                <xsl:value-of select="SettlCurrency"/>
              </PriceDenomination>

              <CounterInvestment>
                <xsl:value-of select="CurrencySymbol"/>
              </CounterInvestment>

              <TradeFx>
                <xsl:choose>
                  <xsl:when test="FXRate_Taxlot='0'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="FXRate_Taxlot='1'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>

              </TradeFx>

              <xsl:variable name = "PB_CounterParty">
                <xsl:value-of select="CounterParty"/>
              </xsl:variable>

              <xsl:variable name="PRANA_BrokerCode">
                <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CounterParty]/@PBBroker"/>
              </xsl:variable>

              <Broker>
                <xsl:choose>
                  <xsl:when test="$PRANA_BrokerCode!=''">
                    <xsl:value-of select="concat($PRANA_BrokerCode,'-ALGO-AMER')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="concat($PB_CounterParty,'-ALGO-AMER')"/>
                  </xsl:otherwise>
                </xsl:choose>

              </Broker>

              <ExpenseAmt>
                <xsl:value-of select="''"/>
              </ExpenseAmt>

              <ExpenseCode>
                <xsl:value-of select="''"/>
              </ExpenseCode>

              <Trader>
                <xsl:value-of select="'cps-shawspring1'"/>
              </Trader>

              <NetTradeAmount>
                <xsl:value-of select="'CALC'"/>
              </NetTradeAmount>

              <AssetType>
                <xsl:choose>
                  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select ="'EQT'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Asset"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AssetType>

              <!--<xsl:variable name="NetAmount">
                <xsl:value-of select="NetAmount"/>
              </xsl:variable>-->

              <xsl:variable name="varNetAmount">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$varNetamount"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="$varNetamount * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot ='D'">
                    <xsl:value-of select="$varNetamount div $varFXRate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <USDSettlementAmount>
                <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
              </USDSettlementAmount>

              <Cusip>
                <xsl:value-of select="CUSIP"/>
              </Cusip>

              <Sedol>
                <xsl:value-of select="SEDOL"/>
              </Sedol>



              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>

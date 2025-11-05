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

           
              <xsl:variable name="PB_NAME" select="'WellsFargo'"/>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountNo"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>


              <Symbol>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="SEDOL"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <Account>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Account>

              <Side>
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy_cover'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                    <xsl:value-of select="'Sell_Short'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close' or Side='Sell'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number(OrderQty)">
                    <xsl:value-of select="OrderQty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </Quantity>

              <Price>
                <xsl:choose>
                  <xsl:when test="number(AvgPrice)">
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Price>

              <NetMoney>
                <xsl:value-of select="$varNetamount"/>
              </NetMoney>

              <Currency>
                <xsl:value-of select="CurrencySymbol"/>
              </Currency>

              <ExecutionTime>
                <xsl:value-of select="''"/>
              </ExecutionTime>

              <Underlying>
                <xsl:choose>
                  <xsl:when test="(CurrencySymbol ='USD') and (Asset='EquityOption')">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol!='USD' and (Asset='EquityOption')">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Underlying>

              <Expiration>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="ExpirationDate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Expiration>

              <StrikePrice>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="StrikePrice"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </StrikePrice>

              <CallPutIndicator>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="PutOrCall"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CallPutIndicator>

              <RootCode>
                <xsl:value-of select="''"/>
              </RootCode>

              <SecurityType>
                <xsl:choose>

                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select="'Swap'"/>
                  </xsl:when>
                  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select="'Equity'"/>
                  </xsl:when>
                  <xsl:when test="Asset='Future'">
                    <xsl:value-of select="'Future'"/>
                  </xsl:when>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'Option'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FXForward'">
                    <xsl:value-of select="'FX Forward'"/>
                  </xsl:when>
                  <xsl:when test="contains(Asset,'FX')">
                    <xsl:value-of select="'Currency'"/>
                  </xsl:when>

                  <xsl:when test="contains(Asset,'FixedIncome')">
                    <xsl:value-of select="'Bond'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Asset"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SecurityType>

              <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

              <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
              </xsl:variable>

              <xsl:variable name="Broker">
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <ExecBroker>
                <xsl:value-of select="$Broker"/>
              </ExecBroker>

              <BookingCategory>
                <xsl:value-of select="$Broker"/>
              </BookingCategory>


              <xsl:variable name="varBrokerrate">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' ">
                    <xsl:value-of select ="format-number(((CommissionCharged + SoftCommissionCharged) div OrderQty),'0.####')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <CommissionCentsPerShare>
                <xsl:value-of select="$varBrokerrate"/>
              </CommissionCentsPerShare>

              <xsl:variable name="varCommissionFee">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' ">
                    <xsl:value-of select ="format-number(((SecFee + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee) ),'0.####')"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol!='USD' ">
                    <xsl:value-of select ="format-number(((SecFee + OtherBrokerFees + CommissionCharged + SoftCommissionCharged + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee) ),'0.####')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <CommissionFlatFee>
                <xsl:value-of select="$varCommissionFee"/>
              </CommissionFlatFee>

              <xsl:variable name="TradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
              </TradeDate>

              <xsl:variable name="TradeSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'/',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'/',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
              </SettlementDate>


          
              <SecuritySubType>
                <xsl:value-of select="''"/>
              </SecuritySubType>

              <LocateID>
                <xsl:value-of select="LotId"/>
              </LocateID>

              <PositionEffect>
                <xsl:value-of select="''"/>
              </PositionEffect>

              <AllocID>
                <xsl:value-of select="EntityID"/>
              </AllocID>

              <ClOrdID>
                <xsl:value-of select="EntityID"/>
              </ClOrdID>

              <CoveredOrUncovered>
                <xsl:value-of select="''"/>
              </CoveredOrUncovered>

              <LastMarket>
                <xsl:value-of select="''"/>
              </LastMarket>

              <Exchange>
                <xsl:value-of select="Exchange"/>
              </Exchange>

              <ExecutionState>
                <xsl:value-of select="''"/>
              </ExecutionState>

              <ISIN>
                <xsl:value-of select="ISIN"/>
              </ISIN>

              <SEDOL>
                <xsl:value-of select="SEDOL"/>
              </SEDOL>

              <GroupExecutionID>
                <xsl:value-of select="''"/>
              </GroupExecutionID>

              <ClientID>
                <xsl:value-of select="AccountNo"/>
              </ClientID>

              <Ticker>
                <xsl:value-of select="''"/>
              </Ticker>

              <TickerType>
                <xsl:value-of select="''"/>
              </TickerType>

              <Interest>
                <xsl:value-of select="''"/>
              </Interest>

              <Country>
                <xsl:value-of select="Country"/>
              </Country>

              <Fee_SEC>
                <xsl:value-of select="''"/>
              </Fee_SEC>

              <Fee_Stamp>
                <xsl:value-of select="''"/>
              </Fee_Stamp>

              <Fee_Other>
                <xsl:value-of select="''"/>
              </Fee_Other>

              <Yield>
                <xsl:value-of select="''"/>
              </Yield>

              <Principal>
                <xsl:value-of select="''"/>
              </Principal>

              <PSET>
                <xsl:value-of select="''"/>
              </PSET>


              <SettlInstID>
                <xsl:value-of select="''"/>
              </SettlInstID>

              <Fee_PTM>
                <xsl:value-of select="''"/>
              </Fee_PTM>

              <Fee_Tax>
                <xsl:value-of select="''"/>
              </Fee_Tax>

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

              <SettlementPrice>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol!='USD'">
                    <xsl:choose>
                      <xsl:when test="SettlCurrency!= CurrencySymbol">
                        <xsl:value-of select="format-number($varPrice,'##.######')"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="0"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                </xsl:choose>
                                
                <!--<xsl:choose>
                  --><!--<xsl:when test="SettlCurrency!= CurrencySymbol and CurrencySymbol!='USD'">--><!--
                  <xsl:when test="SettlCurrency!= CurrencySymbol">
                    <xsl:value-of select="format-number($varPrice,'##.######')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>-->
              </SettlementPrice>

              <SettlementCcy>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="SettlCurrency"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementCcy>


              <SettlementFxRate>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementFxRate>

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
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <SettlementNetAmount>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementNetAmount>



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

              <SettlementCommission>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="format-number($varCommission,'##.######')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementCommission>

              <xsl:variable name="varFeee23">

                <xsl:value-of select="SecFee + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee"/>
              </xsl:variable>

              <SettlementFee>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="format-number($varFeee23,'##.######')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementFee>

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
                
                <xsl:variable name="PB_NAME" select="'WellsFargo'"/>

                <xsl:variable name = "PRANA_FUND_NAME">
                  <xsl:value-of select="AccountNo"/>
                </xsl:variable>

                <xsl:variable name ="THIRDPARTY_FUND_CODE">
                  <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
                </xsl:variable>


                <Symbol>
                  <xsl:choose>
                    <xsl:when test="OldTransactionType ='USD'">
                      <xsl:value-of select="Symbol"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="SEDOL"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Symbol>

                <Account>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                      <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_FUND_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Account>

                <Side>
                  <xsl:choose>
                    <xsl:when test="OldSide='Buy' or OldSide='Buy to Open'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Buy to Close'">
                      <xsl:value-of select="'Buy_cover'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Sell short' or OldSide='Sell to Open'">
                      <xsl:value-of select="'Sell_Short'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Sell to Close' or OldSide='Sell'">
                      <xsl:value-of select="'Sell'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Side>

                <Quantity>
                  <xsl:choose>
                    <xsl:when test="number(OldExecutedQuantity)">
                      <xsl:value-of select="OldExecutedQuantity"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>

                </Quantity>

                <Price>
                  <xsl:choose>
                    <xsl:when test="number(OldAvgPrice)">
                      <xsl:value-of select="OldAvgPrice"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Price>

                <NetMoney>
                  <xsl:value-of select="$varOldNetAmount"/>
                </NetMoney>

                <Currency>
                  <xsl:value-of select="OldTransactionType"/>
                </Currency>

                <ExecutionTime>
                  <xsl:value-of select="''"/>
                </ExecutionTime>

                <Underlying>
                  <xsl:choose>
                    <xsl:when test="(OldTransactionType ='USD') and (Asset='EquityOption')">
                      <xsl:value-of select="Symbol"/>
                    </xsl:when>
                    <xsl:when test="OldTransactionType!='USD' and (Asset='EquityOption')">
                      <xsl:value-of select="SEDOL"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Underlying>

                <Expiration>
                  <xsl:choose>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="ExpirationDate"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Expiration>

                <StrikePrice>
                  <xsl:choose>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="StrikePrice"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </StrikePrice>

                <CallPutIndicator>
                  <xsl:choose>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="PutOrCall"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </CallPutIndicator>

                <RootCode>
                  <xsl:value-of select="''"/>
                </RootCode>

                <SecurityType>
                  <xsl:choose>

                    <xsl:when test="Asset='Equity' and IsSwapped='true'">
                      <xsl:value-of select="'Swap'"/>
                    </xsl:when>
                    <xsl:when test="Asset='Equity'">
                      <xsl:value-of select="'Equity'"/>
                    </xsl:when>
                    <xsl:when test="Asset='Future'">
                      <xsl:value-of select="'Future'"/>
                    </xsl:when>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="'Option'"/>
                    </xsl:when>
                    <xsl:when test="Asset='FXForward'">
                      <xsl:value-of select="'FX Forward'"/>
                    </xsl:when>
                    <xsl:when test="contains(Asset,'FX')">
                      <xsl:value-of select="'Currency'"/>
                    </xsl:when>

                    <xsl:when test="contains(Asset,'FixedIncome')">
                      <xsl:value-of select="'Bond'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="Asset"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </SecurityType>

                <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

                <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
                </xsl:variable>

                <xsl:variable name="Broker">
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <ExecBroker>
                  <xsl:value-of select="$Broker"/>
                </ExecBroker>

                <BookingCategory>
                  <xsl:value-of select="$Broker"/>
                </BookingCategory>


                <xsl:variable name="varBrokerrate">
                  <xsl:choose>
                    <xsl:when test="OldTransactionType='USD' ">
                      <xsl:value-of select ="format-number(((OldSoftCommission + OldSoftCommissionAmount) div OldExecutedQuantity),'0.####')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <CommissionCentsPerShare>
                  <xsl:value-of select="$varBrokerrate"/>
                </CommissionCentsPerShare>

                <xsl:variable name="varCommissionFee">
                  <xsl:choose>
                    <xsl:when test="OldTransactionType='USD' ">
                      <xsl:value-of select ="format-number(((OldSecFee + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldOccFee + OldOrfFee) ),'0.####')"/>
                    </xsl:when>
                    <xsl:when test="OldTransactionType!='USD' ">
                      <xsl:value-of select ="format-number(((OldSecFee + OldOtherBrokerFees + OldSoftCommission + OldSoftCommissionAmount + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldOccFee + OldOrfFee) ),'0.####')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <CommissionFlatFee>
                  <xsl:value-of select="$varCommissionFee"/>
                </CommissionFlatFee>


                <xsl:variable name="varOldTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <TradeDate>
                  <xsl:value-of select="concat(substring-before($varOldTradeDate,'/'),'/',substring-before(substring-after($varOldTradeDate,'/'),'/'),'/',substring-after(substring-after($varOldTradeDate,'/'),'/'))"/>
                </TradeDate>


                <xsl:variable name="varOldSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <SettlementDate>
                  <xsl:value-of select="concat(substring-before($varOldSettlementDate,'/'),'/',substring-before(substring-after($varOldSettlementDate,'/'),'/'),'/',substring-after(substring-after($varOldSettlementDate,'/'),'/'))"/>
                </SettlementDate>
              

                <SecuritySubType>
                  <xsl:value-of select="''"/>
                </SecuritySubType>

                <LocateID>
                  <xsl:value-of select="''"/>
                </LocateID>

                <PositionEffect>
                  <xsl:value-of select="''"/>
                </PositionEffect>

                <AllocID>
                  <xsl:value-of select="EntityID"/>
                </AllocID>

                <ClOrdID>
                  <xsl:value-of select="EntityID"/>
                </ClOrdID>

                <CoveredOrUncovered>
                  <xsl:value-of select="''"/>
                </CoveredOrUncovered>

                <LastMarket>
                  <xsl:value-of select="''"/>
                </LastMarket>

                <Exchange>
                  <xsl:value-of select="Exchange"/>
                </Exchange>

                <ExecutionState>
                  <xsl:value-of select="''"/>
                </ExecutionState>

                <ISIN>
                  <xsl:value-of select="ISIN"/>
                </ISIN>

                <SEDOL>
                  <xsl:value-of select="SEDOL"/>
                </SEDOL>

                <GroupExecutionID>
                  <xsl:value-of select="''"/>
                </GroupExecutionID>

                <ClientID>
                  <xsl:value-of select="AccountNo"/>
                </ClientID>

                <Ticker>
                  <xsl:value-of select="''"/>
                </Ticker>

                <TickerType>
                  <xsl:value-of select="''"/>
                </TickerType>

                <Interest>
                  <xsl:value-of select="''"/>
                </Interest>

                <Country>
                  <xsl:value-of select="''"/>
                </Country>

                <Fee_SEC>
                  <xsl:value-of select="''"/>
                </Fee_SEC>

                <Fee_Stamp>
                  <xsl:value-of select="''"/>
                </Fee_Stamp>

                <Fee_Other>
                  <xsl:value-of select="''"/>
                </Fee_Other>

                <Yield>
                  <xsl:value-of select="''"/>
                </Yield>

                <Principal>
                  <xsl:value-of select="''"/>
                </Principal>

                <PSET>
                  <xsl:value-of select="''"/>
                </PSET>


                <SettlInstID>
                  <xsl:value-of select="''"/>
                </SettlInstID>

                <Fee_PTM>
                  <xsl:value-of select="''"/>
                </Fee_PTM>

                <Fee_Tax>
                  <xsl:value-of select="''"/>
                </Fee_Tax>

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

                <SettlementPrice>
                  <xsl:choose>
                    <xsl:when test="OldTransactionType!='USD'">
                      <xsl:choose>
                        <!--<xsl:when test="OldSettlCurrency!= OldCurrencySymbol and OldCurrencySymbol!='USD'">-->
                        <xsl:when test="OldSettlCurrency!= OldTransactionType">
                          <xsl:value-of select="format-number($varPrice,'##.######')"/>
                        </xsl:when>

                        <xsl:otherwise>
                          <xsl:value-of select="''"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>                 
                 
                </SettlementPrice>

                <SettlementCcy>
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != OldTransactionType">
                      <xsl:value-of select="OldSettlCurrency"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SettlementCcy>


                <SettlementFxRate>
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != OldTransactionType">
                      <xsl:value-of select="OldFXRate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SettlementFxRate>

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


                <SettlementNetAmount>
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != OldTransactionType">
                      <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SettlementNetAmount>



                <xsl:variable name="Commission">
                  <xsl:value-of select="OldSoftCommissionAmount + OldSoftCommission"/>
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


                <SettlementCommission>
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != OldTransactionType">
                      <xsl:value-of select="format-number($varCommission,'##.######')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SettlementCommission>

                <xsl:variable name="varFeee23">

                  <xsl:value-of select="OldSecFee + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldOccFee + OldOrfFee"/>
                </xsl:variable>

                <SettlementFee>
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != OldTransactionType">
                      <xsl:value-of select="format-number($varFeee23,'##.######')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SettlementFee>

                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail>
            </xsl:if>
            <ThirdPartyFlatFileDetail>

              <TaxLotState>
                <xsl:value-of select="'Allocated'"/>
              </TaxLotState>



              <xsl:variable name="PB_NAME" select="'WellsFargo'"/>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountNo"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>


              <Symbol>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="SEDOL"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <Account>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Account>

              <Side>
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy_cover'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                    <xsl:value-of select="'Sell_Short'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close' or Side='Sell'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number(OrderQty)">
                    <xsl:value-of select="OrderQty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>

              </Quantity>

              <Price>
                <xsl:choose>
                  <xsl:when test="number(AvgPrice)">
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Price>

              <NetMoney>
                <xsl:value-of select="$varNetamount"/>
              </NetMoney>

              <Currency>
                <xsl:value-of select="CurrencySymbol"/>
              </Currency>

              <ExecutionTime>
                <xsl:value-of select="''"/>
              </ExecutionTime>

              <Underlying>
                <xsl:choose>
                  <xsl:when test="(CurrencySymbol ='USD') and (Asset='EquityOption')">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol!='USD' and (Asset='EquityOption')">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Underlying>

              <Expiration>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="ExpirationDate"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Expiration>

              <StrikePrice>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="StrikePrice"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </StrikePrice>

              <CallPutIndicator>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="PutOrCall"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CallPutIndicator>

              <RootCode>
                <xsl:value-of select="''"/>
              </RootCode>

              <SecurityType>
                <xsl:choose>

                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select="'Swap'"/>
                  </xsl:when>
                  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select="'Equity'"/>
                  </xsl:when>
                  <xsl:when test="Asset='Future'">
                    <xsl:value-of select="'Future'"/>
                  </xsl:when>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'Option'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FXForward'">
                    <xsl:value-of select="'FX Forward'"/>
                  </xsl:when>
                  <xsl:when test="contains(Asset,'FX')">
                    <xsl:value-of select="'Currency'"/>
                  </xsl:when>

                  <xsl:when test="contains(Asset,'FixedIncome')">
                    <xsl:value-of select="'Bond'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Asset"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SecurityType>

              <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

              <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
              </xsl:variable>

              <xsl:variable name="Broker">
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <ExecBroker>
                <xsl:value-of select="$Broker"/>
              </ExecBroker>

              <BookingCategory>
                <xsl:value-of select="$Broker"/>
              </BookingCategory>


              <xsl:variable name="varBrokerrate">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' ">
                    <xsl:value-of select ="format-number(((CommissionCharged + SoftCommissionCharged) div OrderQty),'0.####')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <CommissionCentsPerShare>
                <xsl:value-of select="$varBrokerrate"/>
              </CommissionCentsPerShare>

              <xsl:variable name="varCommissionFee">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' ">
                    <xsl:value-of select ="format-number(((SecFee + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee) ),'0.####')"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol!='USD' ">
                    <xsl:value-of select ="format-number(((SecFee + OtherBrokerFees + CommissionCharged + SoftCommissionCharged + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee) ),'0.####')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <CommissionFlatFee>
                <xsl:value-of select="$varCommissionFee"/>
              </CommissionFlatFee>

              <xsl:variable name="TradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
              </TradeDate>

              <xsl:variable name="TradeSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'/',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'/',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
              </SettlementDate>



              <SecuritySubType>
                <xsl:value-of select="''"/>
              </SecuritySubType>

              <LocateID>
                <xsl:value-of select="LotId"/>
              </LocateID>

              <PositionEffect>
                <xsl:value-of select="''"/>
              </PositionEffect>

              <AllocID>
                <xsl:value-of select="EntityID"/>
              </AllocID>

              <ClOrdID>
                <xsl:value-of select="EntityID"/>
              </ClOrdID>

              <CoveredOrUncovered>
                <xsl:value-of select="''"/>
              </CoveredOrUncovered>

              <LastMarket>
                <xsl:value-of select="''"/>
              </LastMarket>

              <Exchange>
                <xsl:value-of select="Exchange"/>
              </Exchange>

              <ExecutionState>
                <xsl:value-of select="''"/>
              </ExecutionState>

              <ISIN>
                <xsl:value-of select="ISIN"/>
              </ISIN>

              <SEDOL>
                <xsl:value-of select="SEDOL"/>
              </SEDOL>

              <GroupExecutionID>
                <xsl:value-of select="''"/>
              </GroupExecutionID>

              <ClientID>
                <xsl:value-of select="AccountNo"/>
              </ClientID>

              <Ticker>
                <xsl:value-of select="''"/>
              </Ticker>

              <TickerType>
                <xsl:value-of select="''"/>
              </TickerType>

              <Interest>
                <xsl:value-of select="''"/>
              </Interest>

              <Country>
                <xsl:value-of select="Country"/>
              </Country>

              <Fee_SEC>
                <xsl:value-of select="''"/>
              </Fee_SEC>

              <Fee_Stamp>
                <xsl:value-of select="''"/>
              </Fee_Stamp>

              <Fee_Other>
                <xsl:value-of select="''"/>
              </Fee_Other>

              <Yield>
                <xsl:value-of select="''"/>
              </Yield>

              <Principal>
                <xsl:value-of select="''"/>
              </Principal>

              <PSET>
                <xsl:value-of select="''"/>
              </PSET>


              <SettlInstID>
                <xsl:value-of select="''"/>
              </SettlInstID>

              <Fee_PTM>
                <xsl:value-of select="''"/>
              </Fee_PTM>

              <Fee_Tax>
                <xsl:value-of select="''"/>
              </Fee_Tax>

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

              <SettlementPrice>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol!='USD'">
                    <xsl:choose>
                      <xsl:when test="SettlCurrency!= CurrencySymbol">
                        <xsl:value-of select="format-number($varPrice,'##.######')"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="0"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                </xsl:choose>

                <!--<xsl:choose>
                  -->
                <!--<xsl:when test="SettlCurrency!= CurrencySymbol and CurrencySymbol!='USD'">-->
                <!--
                  <xsl:when test="SettlCurrency!= CurrencySymbol">
                    <xsl:value-of select="format-number($varPrice,'##.######')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>-->
              </SettlementPrice>

              <SettlementCcy>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="SettlCurrency"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementCcy>


              <SettlementFxRate>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementFxRate>

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
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <SettlementNetAmount>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementNetAmount>



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

              <SettlementCommission>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="format-number($varCommission,'##.######')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementCommission>

              <xsl:variable name="varFeee23">

                <xsl:value-of select="SecFee + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + OccFee + OrfFee"/>
              </xsl:variable>

              <SettlementFee>
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:value-of select="format-number($varFeee23,'##.######')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementFee>

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

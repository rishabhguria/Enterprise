<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <xsl:variable name="varNetamountCalc">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * Multiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * Multiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>



          <TradeType>
            <xsl:value-of select="Side"/>
          </TradeType>

          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amemded'">
                <xsl:value-of select ="'MODIFY'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CANCEL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Action>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </Action>

          <Portfolio>
            <xsl:value-of select="'HELM'"/>
          </Portfolio>

          <LocationAccount>
            <xsl:choose>
              <xsl:when test="AccountName='Helm Investment Partners'">
                <xsl:value-of select="'PW7007035'"/>
              </xsl:when>
              <xsl:when test="AccountName='Helm Investment partner-IB'">
                <xsl:value-of select="'U3606843'"/>
              </xsl:when>

              <xsl:when test="AccountName='Helm Investment-BTG Cayman'">
                <xsl:value-of select="'28036'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </LocationAccount>

          <Strategy>
            <xsl:value-of select="'NONE'"/>
          </Strategy>

          <Investment>
            <xsl:value-of select="Symbol"/>
          </Investment>

 <CallPut>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="PutOrCall"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </CallPut>
          <UnderlyingTicker>
            <xsl:choose>
              <xsl:when test="Asset='PrivateEquity' or Asset='EquityOption'">
                <xsl:value-of select="UnderlyingSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingTicker>
          <UnderlyingISIN>
            <xsl:choose>
              <xsl:when test="Asset='PrivateEquity' or Asset='EquityOption'">
                <xsl:value-of select="UnderlyingISINSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingISIN>

          <UnderlyingCusip>
            <xsl:choose>
              <xsl:when test="Asset='PrivateEquity' or Asset='EquityOption'">
                <xsl:value-of select="UnderlyingCUSIPSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingCusip>
          <UnderlyingSedol>
            <xsl:choose>
              <xsl:when test="Asset='PrivateEquity' or Asset='EquityOption'">
                <xsl:value-of select="UnderlyingSEDOLSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingSedol>
          <OTCPriceDenomination>
            <xsl:choose>
              <xsl:when test="Asset='PrivateEquity'">
                <xsl:value-of select="CurrencySymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </OTCPriceDenomination>
          <UnderlyingCcy>
            <xsl:choose>
              <xsl:when test="Asset='PrivateEquity' or Asset='EquityOption'">
                <xsl:value-of select="UnderlyingCurrencySymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingCcy>
          <UnderlyingStrikePrice>
            <xsl:value-of select="''"/>
          </UnderlyingStrikePrice>
          <UnderlyingExpirationDate>
            <xsl:value-of select="''"/>
          </UnderlyingExpirationDate>
          
          <EventDate>
            <xsl:value-of select="concat(substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'),'-',substring-after(substring-after(TradeDate,'/'),'/'))"/>
          </EventDate>

          <xsl:variable name="TradeSettlementDate">
            <xsl:call-template name="DateFormat">
              <xsl:with-param name="Date" select="SettlementDate">
              </xsl:with-param>
            </xsl:call-template>
          </xsl:variable>
          <SettleDate>
            <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'-',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'-',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
          </SettleDate>
        

          <ActualSettleDate>
            <xsl:value-of select="concat(substring-before($TradeSettlementDate,'/'),'-',substring-before(substring-after($TradeSettlementDate,'/'),'/'),'-',substring-after(substring-after($TradeSettlementDate,'/'),'/'))"/>
          </ActualSettleDate>

          <Comments>
            <xsl:value-of select="''"/>
          </Comments>

          <UserTranID1>
            <xsl:value-of select="EntityID"/>
          </UserTranID1>

          <Quantity>
            <xsl:value-of select="Quantity"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AvgPrice"/>
          </Price>

          <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="0"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varGrossamount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="format-number($varNetamountCalc,'##.00')"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($varNetamountCalc * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($varNetamountCalc div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>

              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <NetCounterAmount>
            <xsl:value-of select="$varNetamountCalc"/>
          </NetCounterAmount>



          <xsl:variable name="Commission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>


          <xsl:variable name="varTotalCommission">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="format-number($Commission,'##.00')"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($Commission * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($Commission div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>

              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <TotCommission>
            <xsl:value-of select="$varTotalCommission"/>
          </TotCommission>

          <SecFeeAmount>
            <xsl:value-of select="SecFee"/>
          </SecFeeAmount>

          <PriceDenomination>
            <xsl:value-of select="CurrencySymbol"/>
          </PriceDenomination>

          <CounterInvestment>
            <xsl:value-of select="CurrencySymbol"/>
          </CounterInvestment>

          <TradeFx>
            <xsl:value-of select="FXRate_Taxlot"/>
          </TradeFx>

          <Broker>
            <xsl:choose>
              <xsl:when test="AccountName='Helm Investment Partners'">
                <xsl:value-of select="'PERSHING'"/>
              </xsl:when>
              <xsl:when test="AccountName='Helm Investment partner-IB'">
                <xsl:value-of select="'IB'"/>
              </xsl:when>
              <xsl:when test="AccountName='Helm Investment-BTG Cayman'">
                <xsl:value-of select="'BTG PACTUAL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Broker>
          <xsl:variable name="varFees">
            <xsl:value-of select ="MiscFees + ClearingBrokerFee + OtherBrokerFees + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + OccFee + OrfFee"/>
          </xsl:variable>

          <ExpenseAmt>
            <xsl:value-of select="$varFees"/>
          </ExpenseAmt>

          <ExpenseCode>
            <xsl:value-of select="'MiscellaneousCharges'"/>
          </ExpenseCode>

          <Trader>
            <xsl:value-of select="''"/>
          </Trader>




          <xsl:variable name="varNetamount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="format-number($varNetamountCalc,'##.00')"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($varNetamountCalc * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($varNetamountCalc div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>

              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <NetTradeAmount>
            <xsl:value-of select="$varNetamountCalc"/>
          </NetTradeAmount>

          <AssetType>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'EQT'"/>
              </xsl:when>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="'SpotFXTrade'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>
            </xsl:choose>
          </AssetType>


			<xsl:variable name="varFXRate_taxlot">
				<xsl:choose>
					<xsl:when test="CurrencySymbol!='USD'">
						<xsl:value-of select="0"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			
          <xsl:variable name="varNetAmount">
			  <xsl:choose>
				  <xsl:when test="CurrencySymbol!='USD'">
					  <xsl:choose>
						  <!--<xsl:when test="FXRate_Taxlot=0">
							  <xsl:value-of select="format-number($varNetamountCalc,'##.00')"/>
						  </xsl:when>-->
						  <xsl:when test="FXConversionMethodOperator='M'">
							  <xsl:value-of select="format-number($varNetamountCalc * FXRate_Taxlot,'##.00')"/>
						  </xsl:when>

						  <xsl:when test="FXConversionMethodOperator='D'">
							  <xsl:value-of select="format-number($varNetamountCalc div FXRate_Taxlot,'##.00')"/>
						  </xsl:when>

						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="$varNetamountCalc"/>
				  </xsl:otherwise>
			  </xsl:choose>
           
          </xsl:variable>

          <USDSettlementAmount>
			  <xsl:value-of select="format-number($varNetAmount,'##.00')"/>
		  </USDSettlementAmount>

		  <Cusip>
			  <xsl:value-of select="CUSIPSymbol"/>
		  </Cusip>

		  <Sedol>
            <xsl:value-of select="SEDOLSymbol"/>
          </Sedol>
          <ISIN>
            <xsl:value-of select ="ISINSymbol"/>
          </ISIN>
          <OCC>
            <xsl:value-of select="OSISymbol"/>
          </OCC>


          <xsl:variable name="varTransactionType">
            <xsl:choose>
              <xsl:when test ="TransactionType='SellShort'">
                <xsl:value-of select ="'Sell Short'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='BuytoClose'">
                <xsl:value-of select ="'Buy to Close'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='BuytoOpen'">
                <xsl:value-of select ="'Buy to Open'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='SelltoClose'">
                <xsl:value-of select ="'Sell to Close'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='SelltoOpen'">
                <xsl:value-of select ="'Sell to Open'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='ShortAddition'">
                <xsl:value-of select ="'Short Addition'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='ShortWithdrawal'">
                <xsl:value-of select ="'Short Withdrawal'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='ShortWithdrawalCashInLieu'">
                <xsl:value-of select ="'Short Withdrawal Cash In Lieu'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongWithdrawalCashInLieu'">
                <xsl:value-of select ="'Long Withdrawal Cash In Lieu'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongWithdrawal'">
                <xsl:value-of select ="'Long Withdrawal'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongCostAdj'">
                <xsl:value-of select ="'Long Cost Adj'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='LongAddition'">
                <xsl:value-of select ="'Long Addition'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='DLCostAndPNL'">
                <xsl:value-of select ="'DL Cost And PNL'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSClosingPx'">
                <xsl:value-of select ="'Cash Settle At Closing Date Spot PX'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='DLCostAndPNL'">
                <xsl:value-of select ="'DL Cost And PNL'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSCost'">
                <xsl:value-of select ="'Cash Settle At Cost'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSSwp'">
                <xsl:value-of select ="'Swap Expire'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSSwpRl'">
                <xsl:value-of select ="'Swap Expire and Rollover'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='CSZero'">
                <xsl:value-of select ="'Cash Settle At Zero Price'"/>
              </xsl:when>
              <xsl:when test ="TransactionType='DLCost'">
                <xsl:value-of select ="'Deliver FX At Cost'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="TransactionType"/>
              </xsl:otherwise>

            </xsl:choose>
          </xsl:variable>

          <tradeCcyBuy>
			  <xsl:choose>
				  <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
					  <xsl:value-of select ="VsCurrency"/>
				  </xsl:when>
				  <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
					  <xsl:value-of select="LeadCurrency"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </tradeCcyBuy>

          <tradeCcyBuyQuantity>
            <xsl:choose>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
                <xsl:value-of select="ExecutedQty" />
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
                <!--<xsl:value-of select="ExecutedQty * AveragePrice" />-->
                <xsl:value-of select="$varNetamountCalc"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </tradeCcyBuyQuantity>

          <tradeCcySell>
            <xsl:choose>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
                <xsl:value-of select ="LeadCurrency"/>
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
                <xsl:value-of select="VsCurrency"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </tradeCcySell>

          <tradeCcySellQuantity>
            <xsl:choose>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
                <!--<xsl:value-of select ="ExecutedQty * AveragePrice"/>-->
                <xsl:value-of select="$varNetamountCalc"/>
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
                <xsl:value-of select="ExecutedQty" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </tradeCcySellQuantity>

			<SettlementCurrency>
				<xsl:value-of select="SettlCurrency"/>
			</SettlementCurrency>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>
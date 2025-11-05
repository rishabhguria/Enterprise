<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>		
			
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='LCM Global Growth Fund' and (Asset='FX' or Asset='FXForward' or Asset='Equity')]">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					
					<TradeUpdateType>
						<xsl:choose>
					        <xsl:when test="TaxLotState='Allocated'">
						<xsl:value-of select ="'0'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Amended'">
						<xsl:value-of select ="'1'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Deleted'">
						<xsl:value-of select ="'2'"/>
					</xsl:when>					
				</xsl:choose>
					</TradeUpdateType>

					<TradeTransactionType>
						<xsl:value-of select="'1'"/>
					</TradeTransactionType>

					<xsl:variable name="PB_NAME" select="''"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<TradeAccount>					
						<xsl:value-of select="'LCM Growth Cowen PB'"/>
					</TradeAccount>


					<TradeFund>
						<xsl:value-of select="'LCM Global Growth Fund-Makalu'"/>
					</TradeFund>

					<TradeStrategyName>
						<xsl:value-of select="'LCM Default'"/>
					</TradeStrategyName>

					<TradeShortCounterpartyName>
						<xsl:value-of select="'Cowen'"/>
					</TradeShortCounterpartyName>

					
					<InstStaticTicker>
						<xsl:value-of select="BBCode"/>
					</InstStaticTicker>

					<TradeBuySell>
						<xsl:choose>
							<xsl:when test ="Side='Buy' or Side ='Buy to Open'">
								<xsl:value-of select ="'B'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell'or Side ='Sell to Close'">
								<xsl:value-of select ="'S'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell to Open'">
								<xsl:value-of select ="'SS'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell short'">
								<xsl:value-of select ="'SS'"/>
							</xsl:when>
							<xsl:when test ="Side='Buy to Cover' or Side='Buy to Close'">
								<xsl:value-of select ="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeBuySell>

					<TradeCcy>
						<xsl:value-of select="CurrencySymbol"/>
					</TradeCcy>

					<TradeDisplayPrice>
						<xsl:value-of select="AveragePrice"/>
					</TradeDisplayPrice>

					<TradeDisplayQuantity>
						<xsl:value-of select="AllocatedQty"/>
					</TradeDisplayQuantity>
          
					<xsl:variable name="varTradeTax">
						<xsl:value-of select="OtherBrokerFee + MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee + TaxOnCommissions + TransactionLevy + StampDuty + ClearingFee"/>
					</xsl:variable>
					<TradeTax>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="$varTradeTax"/>
              </xsl:when>
              
              <!--<xsl:when test="Asset='FX' or Asset='FXForward'">
                <xsl:value-of select=""/>
              </xsl:when>-->
              
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>         
						
					</TradeTax>
          
					<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					<TradeCommissionAmount>
						<xsl:value-of select="$varCommission"/>
					</TradeCommissionAmount>

					<TradeExecutionDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeExecutionDate>

					<TradeSettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</TradeSettlementDate>


					<TradeTrader>
						<xsl:value-of select ="'LighthouseAdmin'"/>
					</TradeTrader>

					<TradeExternalRef>
						<xsl:value-of select="EntityID"/>
					</TradeExternalRef>


					<TradeTaxUseRateTable>
						<xsl:value-of select="'0'"/>
					</TradeTaxUseRateTable>

					<TradeCommissionUseRateTable>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'1'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>           
					</TradeCommissionUseRateTable>

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
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
                <xsl:value-of select="LeadCurrencyName"/>
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
                <xsl:value-of select="VsCurrencyName"/>
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
                <xsl:value-of select="NetAmount" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </tradeCcyBuyQuantity>

          <tradeCcySell>
            <xsl:choose>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
                <xsl:value-of select ="LeadCurrencyName"/>
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
                <xsl:value-of select="VsCurrencyName"/>
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
                <xsl:value-of select="NetAmount" />
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
                <xsl:value-of select="ExecutedQty" />
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </tradeCcySellQuantity>

          <xsl:variable name="NDFDeliveryCCY">
            <xsl:choose>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Buy')">
                <xsl:value-of select="LeadCurrencyName"/>
              </xsl:when>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward') and contains($varTransactionType,'Sell')">
                <xsl:value-of select="VsCurrencyName"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <tradeNdfDeliveryCcy>
            <xsl:choose>
              <xsl:when test="(contains(Asset,'FX') or Asset='FXForward')">
                <xsl:value-of select="$NDFDeliveryCCY"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>       

          </tradeNdfDeliveryCcy>
          
          <Type>
            <xsl:choose>
              <xsl:when test="Asset='FX' or Asset='FXForward'">
                <xsl:value-of select="'FX'"/>
              </xsl:when>

              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'Equity'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Type>
          
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
         

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
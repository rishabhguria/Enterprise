<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <TradeUpdateType>
          <xsl:value-of select ="'TradeUpdateType'"/>
        </TradeUpdateType>

        <tradeDate>
          <xsl:value-of select="'tradeDate'"/>
        </tradeDate>

        <tradeNdf>
          <xsl:value-of select="'tradeNdf'"/>
        </tradeNdf>

        <tradeNdfFixingDate>
          <xsl:value-of select="'tradeNdfFixingDate'"/>
        </tradeNdfFixingDate>

        <tradeCcyBuy>
          <xsl:value-of select="'tradeCcyBuy'"/>
        </tradeCcyBuy>

        <tradeCcyBuyQuantity>
          <xsl:value-of select="'tradeCcyBuyQuantity'"/>
        </tradeCcyBuyQuantity>

        <tradeCcySell>
          <xsl:value-of select="'tradeCcySell'"/>
        </tradeCcySell>

        <tradeCcySellQuantity>
          <xsl:value-of select="'tradeCcySellQuantity'"/>
        </tradeCcySellQuantity>

        <tradeSettlementDate>
          <xsl:value-of select="'tradeSettlementDate'"/>
        </tradeSettlementDate>

        <tradeNdfDeliveryCcy>
          <xsl:value-of select="'tradeNdfDeliveryCcy'"/>
        </tradeNdfDeliveryCcy>

        <tradeExternalRef>
          <xsl:value-of select="'tradeExternalRef'"/>
        </tradeExternalRef>

        <tradeCounterparty>
          <xsl:value-of select="'tradeCounterparty'"/>
        </tradeCounterparty>

        

        <tradeCommission1Amount>
          <xsl:value-of select="'tradeCommission1Amount'"/>
        </tradeCommission1Amount>

        <tradeCommission1AmountCcy>
          <xsl:value-of select="'tradeCommission1AmountCcy'"/>
        </tradeCommission1AmountCcy>

        <tradeAccount>
          <xsl:value-of select="'tradeAccount'"/>
        </tradeAccount>

        <tradeStrategy>
          <xsl:value-of select="'tradeStrategy'"/>
        </tradeStrategy>



        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>


      </ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='LCM Global Growth Fund' and (Asset='FX' or Asset='FXForward')]">		

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

					<tradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</tradeDate>

					<tradeNdf>
						<xsl:value-of select="'N'"/>
					</tradeNdf>

					<tradeNdfFixingDate>
						<xsl:value-of select="''"/>
					</tradeNdfFixingDate>

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

					<tradeSettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</tradeSettlementDate>
					
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
						<xsl:value-of select="$NDFDeliveryCCY"/> 
						
					</tradeNdfDeliveryCcy>

					<tradeExternalRef>
						<xsl:value-of select="EntityID"/>
					</tradeExternalRef>

					<tradeCounterparty>
						<xsl:value-of select="'Cowen'"/>
					</tradeCounterparty>

					<!--<xsl:variable name="varTradeCommAmnt">
						<xsl:value-of select=""/>
					</xsl:variable>-->
					
					<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					
					<tradeCommission1Amount>
						<xsl:value-of select="$varCommission"/>
					</tradeCommission1Amount>

					<tradeCommission1AmountCcy>
						<xsl:value-of select="CurrencySymbol"/>
					</tradeCommission1AmountCcy>

					<tradeAccount>
						<xsl:value-of select="'LCM Growth Cowen PB'"/>
					</tradeAccount>

					<tradeStrategy>
						<xsl:value-of select="'LCM Default'"/>
					</tradeStrategy>

					

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>


				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
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

        <TradeTransactionType>
          <xsl:value-of select="'TradeTransactionType'"/>
        </TradeTransactionType>

       
        <TradeAccount>        
          <xsl:value-of select="'TradeAccount'"/>
        </TradeAccount>


        <TradeFund>
          <xsl:value-of select="'TradeFund'"/>
        </TradeFund>

        <tradeStrategyName>
          <xsl:value-of select="'tradeStrategyName'"/>
        </tradeStrategyName>

        <tradeShortCounterpartyName>
          <xsl:value-of select="'tradeShortCounterpartyName'"/>
        </tradeShortCounterpartyName>

        <instISIN>
          <xsl:value-of select="'instISIN'"/>
        </instISIN>

        <tradeBuySell>
          <xsl:value-of select="'tradeBuySell'"/>
        </tradeBuySell>

        <tradeCcy>
          <xsl:value-of select="'tradeCcy'"/>
        </tradeCcy>

        <tradeDisplayPrice>
          <xsl:value-of select="'tradeDisplayPrice'"/>
        </tradeDisplayPrice>

        <tradeDisplayQuantity>
          <xsl:value-of select="'tradeDisplayQuantity'"/>
        </tradeDisplayQuantity>

        <tradeTax>
          <xsl:value-of select="'tradeTax'"/>
        </tradeTax>
        
        <tradeCommissionAmount>
          <xsl:value-of select="'tradeCommissionAmount'"/>
        </tradeCommissionAmount>

        <tradeExecutionDate>
          <xsl:value-of select="'tradeExecutionDate'"/>
        </tradeExecutionDate>

        <tradeSettlementDate>
          <xsl:value-of select="'tradeSettlementDate'"/>
        </tradeSettlementDate>


        <tradeTrader>
          <xsl:value-of select ="'tradeTrader'"/>
        </tradeTrader>

        <tradeExternalRef>
          <xsl:value-of select="'tradeExternalRef'"/>
        </tradeExternalRef>


        <tradeTaxUseRateTable>
          <xsl:value-of select="'tradeTaxUseRateTable'"/>
        </tradeTaxUseRateTable>

        <tradeCommission4UseRateTable>
          <xsl:value-of select="'tradeCommission4UseRateTable'"/>
        </tradeCommission4UseRateTable>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset='FixedIncome']">
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
						<!--<xsl:choose>
							<xsl:when test="AccountName ='LCM Growth Cowen PB'">
								<xsl:value-of select="'LCM Growth Cowen PB'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:value-of select="'LCM Growth Cowen PB'"/>
					</TradeAccount>


					<TradeFund>
						<xsl:value-of select="'LCM Global Growth Fund-Makalu'"/>
					</TradeFund>

					<tradeStrategyName>
						<xsl:value-of select="'LCM Default'"/>
					</tradeStrategyName>

					<tradeShortCounterpartyName>
						<xsl:value-of select="CounterParty"/>
					</tradeShortCounterpartyName>

					<xsl:variable name="varInsTricker">
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="BBCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<instISIN>
						<xsl:value-of select="ISIN"/>
					</instISIN>

					<tradeBuySell>
						<xsl:choose>
							<xsl:when test ="Side='Buy'">
								<xsl:value-of select ="'B'"/>
							</xsl:when>
							<xsl:when test ="Side='Sell'">
								<xsl:value-of select ="'S'"/>
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
					</tradeBuySell>

					<tradeCcy>
						<xsl:value-of select="CurrencySymbol"/>
					</tradeCcy>

					<tradeDisplayPrice>
						<xsl:value-of select="AveragePrice"/>
					</tradeDisplayPrice>

					<tradeDisplayQuantity>
						<xsl:value-of select="AllocatedQty"/>
					</tradeDisplayQuantity>

					<tradeTax>
						<xsl:value-of select="''"/>
					</tradeTax>
					<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					<tradeCommissionAmount>
						<xsl:value-of select="$varCommission"/>
					</tradeCommissionAmount>

					<tradeExecutionDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</tradeExecutionDate>

					<tradeSettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</tradeSettlementDate>


					<tradeTrader>
						<xsl:value-of select ="'LighthouseAdmin'"/>
					</tradeTrader>

					<tradeExternalRef>
						<xsl:value-of select="PBUniqueID"/>
					</tradeExternalRef>


					<tradeTaxUseRateTable>
						<xsl:value-of select="'1'"/>
					</tradeTaxUseRateTable>

					<tradeCommission4UseRateTable>
						<xsl:value-of select="'1'"/>
					</tradeCommission4UseRateTable>
					
					


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>


				
			</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
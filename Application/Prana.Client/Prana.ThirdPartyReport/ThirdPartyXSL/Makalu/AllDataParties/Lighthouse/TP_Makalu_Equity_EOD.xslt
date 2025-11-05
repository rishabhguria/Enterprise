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

        <TradeStrategyName>
          <xsl:value-of select="'TradeStrategyName'"/>
        </TradeStrategyName>

        <TradeShortCounterpartyName>
          <xsl:value-of select="'TradeShortCounterpartyName'"/>
        </TradeShortCounterpartyName>


        <InstStaticTicker>
          <xsl:value-of select="'InstStaticTicker'"/>
        </InstStaticTicker>

        <TradeBuySell>
          <xsl:value-of select="'TradeBuySell'"/>
        </TradeBuySell>

        <TradeCcy>
          <xsl:value-of select="'TradeCcy'"/>
        </TradeCcy>

        <TradeDisplayPrice>
          <xsl:value-of select="'TradeDisplayPrice'"/>
        </TradeDisplayPrice>

        <TradeDisplayQuantity>
          <xsl:value-of select="'TradeDisplayQuantity'"/>
        </TradeDisplayQuantity>
       
        <TradeTax>
          <xsl:value-of select="'TradeTax'"/>
        </TradeTax>
       
        <TradeCommissionAmount>
          <xsl:value-of select="'TradeCommissionAmount'"/>
        </TradeCommissionAmount>

        <TradeExecutionDate>
          <xsl:value-of select="'TradeExecutionDate'"/>
        </TradeExecutionDate>

        <TradeSettlementDate>
          <xsl:value-of select="'TradeSettlementDate'"/>
        </TradeSettlementDate>


        <TradeTrader>
          <xsl:value-of select ="'TradeTrader'"/>
        </TradeTrader>

        <TradeExternalRef>
          <xsl:value-of select="'TradeExternalRef'"/>
        </TradeExternalRef>


        <TradeTaxUseRateTable>
          <xsl:value-of select="'TradeTaxUseRateTable'"/>
        </TradeTaxUseRateTable>

        <TradeCommissionUseRateTable>
          <xsl:value-of select="'TradeCommissionUseRateTable'"/>
        </TradeCommissionUseRateTable>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = 'LCM Global Growth Fund' and Asset !=' FixedIncome'  and Asset != 'FX' and Asset != 'FXForward' and Asset != 'FXOption' and Asset!='ConvertibleBond']">
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

					<TradeStrategyName>
						<xsl:value-of select="'LCM Default'"/>
					</TradeStrategyName>

					<TradeShortCounterpartyName>
						<xsl:value-of select="'Cowen'"/>
					</TradeShortCounterpartyName>

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
						<xsl:value-of select="$varTradeTax"/>
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
						<xsl:value-of select="'1'"/>
					</TradeCommissionUseRateTable>
					
					


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>


				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
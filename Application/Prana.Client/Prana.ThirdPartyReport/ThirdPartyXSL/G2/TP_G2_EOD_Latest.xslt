<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>     

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <AccountNo>
          <xsl:value-of select="'Account No'"/>
        </AccountNo>

        <ProductType>
          <xsl:value-of select="'Product Type'"/>
        </ProductType>

        <Cusip>
          <xsl:value-of select="'Cusip'"/>
        </Cusip>

        <BLOOMBERG>
          <xsl:value-of select="'Bloomberg Figi'"/>
        </BLOOMBERG>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <TradePrice>
          <xsl:value-of select="'Trade Price'"/>
        </TradePrice>

        <InterestRate>
          <xsl:value-of select="'Interest Rate'"/>
        </InterestRate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <TradeDirection>
          <xsl:value-of select="'Trade Direction'"/>
        </TradeDirection>

        <Amount>
          <xsl:value-of select="'Amount'"/>
        </Amount>

        <InterestAmount>
          <xsl:value-of select="'Interest Amount'"/>
        </InterestAmount>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <Taxes>
          <xsl:value-of select="'Taxes'"/>
        </Taxes>

        <ExchangeFee>
          <xsl:value-of select="'Exchange Fee'"/>
        </ExchangeFee>

        <TradeId>
          <xsl:value-of select="'Trade Id'"/>
        </TradeId>

        <TradeStatus>
          <xsl:value-of select="'Trade Status'"/>
        </TradeStatus>
   
        <ExecutingBroker>
          <xsl:value-of select="'Executing Broker'"/>
        </ExecutingBroker>

        <SEDOL>
          <xsl:value-of select="'SEDOL'"/>
        </SEDOL>

        <Currency>
          <xsl:value-of select="'Local Currency'"/>
        </Currency>

        <SettleCurrency>
          <xsl:value-of select="'Settle Currency'"/>
        </SettleCurrency> 
		
		<TradePriceInLocalCurrency>
         <xsl:value-of select="concat('Trade Price','(','In Local Currency',')')"/>
        </TradePriceInLocalCurrency> 

        <FXrate>
          <xsl:value-of select="'FX rate'"/>
        </FXrate>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

   <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
		

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
			
          <xsl:variable name="Trade_Day" select="substring(TradeDate,4,2)"/>
          <xsl:variable name="Trade_Month" select="substring(TradeDate,1,2)"/>
          <xsl:variable name="Trade_Year" select="substring(TradeDate,7,4)"/>
			
          <TradeDate>
            <xsl:value-of select="concat($Trade_Year,$Trade_Month,$Trade_Day)"/>
          </TradeDate>

          <xsl:variable name="PB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>
			
          <xsl:variable name ="PB_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

          <AccountNo>
            <xsl:choose>
              <xsl:when test ="$PB_FUND_NAME!= ''">
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountNo>
		  
		  <!-- <xsl:variable name="varExchange">
		            <xsl:choose>
						<xsl:when test="Exchange='BasketSwap'">
						  <xsl:value-of select ="'Equity Swap'"/>
					    </xsl:when>
					</xsl:choose>
		  </xsl:variable> -->
	<xsl:variable name="varAsset">
				<xsl:choose>
					<xsl:when test="Asset='FX'">
						<xsl:value-of select ="'FX Spot'"/>
					</xsl:when>
					<xsl:when test="Asset='EquityOption'">
						<xsl:value-of select ="'Option'"/>
					</xsl:when>
					<xsl:when test="ExchangeID='251'">
						<xsl:value-of select ="'Equity Swap'"/>
					</xsl:when>
					<xsl:when test="Asset='Equity'">
						<xsl:value-of select ="'Equity'"/>
					</xsl:when> 
					<xsl:when test="Asset='FixedIncome'">
						<xsl:value-of select ="'Bond'"/>
					</xsl:when>
					
					<!-- <xsl:when test="ExchangeID='251'">
						<xsl:value-of select ="'Equity Swap'"/>
					</xsl:when> -->
					 <xsl:otherwise>
					    <xsl:value-of select ="Asset"/>
					</xsl:otherwise> 
				</xsl:choose>
		</xsl:variable>
		  
		  <ProductType>
		  	<xsl:value-of select="$varAsset"/>
		  </ProductType>

			<Cusip>
				<xsl:value-of select="CUSIP"/>
			</Cusip>
			
			<BLOOMBERG>
				<xsl:value-of select="BBCode"/>
			</BLOOMBERG>
			
			 <Symbol>
             <xsl:value-of select="Symbol"/>
        </Symbol>

			<ISIN>
				<xsl:value-of select="ISIN"/>
			</ISIN>

			<TradePrice>
				<xsl:value-of select="AveragePrice"/>
			</TradePrice>

			<InterestRate>
				<xsl:value-of select="''"/>
			</InterestRate>

			<xsl:variable name="STrade_Day" select="substring(SettlementDate,4,2)"/>
			<xsl:variable name="STrade_Month" select="substring(SettlementDate,1,2)"/>
			<xsl:variable name="STrade_Year" select="substring(SettlementDate,7,4)"/>

			<SettlementDate>
				<xsl:value-of select="concat($STrade_Year,$STrade_Month,$STrade_Day)"/>
			</SettlementDate>

			<Quantity>
				<xsl:value-of select="AllocatedQty"/>
			</Quantity>

			<xsl:variable name="Side1">
				<xsl:choose>
					<xsl:when test="Side='Buy to Open' or Side='Buy' ">
						<xsl:value-of select ="'B'"/>
					</xsl:when>
					<xsl:when test="Side='Sell'">
						<xsl:value-of select ="'S'"/>
					</xsl:when>
					<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
						<xsl:value-of select ="'SS'"/>
					</xsl:when>
					<xsl:when test="Side='Buy to Close'">
						<xsl:value-of select ="'BC'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="Side"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>


			<TradeDirection>
				<xsl:value-of select="$Side1"/>
			</TradeDirection>

			<Amount>
				<xsl:value-of select="NetAmount"/>
			</Amount>

			<InterestAmount>
				<xsl:value-of select="AccruedInterest"/>
			</InterestAmount>

			<Commission>
				<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
			</Commission>

			<Taxes>
				<xsl:value-of select="''"/>
			</Taxes>

			<ExchangeFee>
				<xsl:value-of select="TaxOnCommissions + OtherBrokerFee + ClearingBrokerFee + ClearingFee + MiscFees + SecFee + StampDuty + TransactionLevy"/>
			</ExchangeFee>

			<TradeId>
				<xsl:value-of select="PBUniqueID"/>
			</TradeId>

			<xsl:variable name="varTaxlotStateGrp">
				<xsl:choose>
					<xsl:when test="TaxLotState='Allocated'">
						<xsl:value-of select ="'N'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Amended'">
						<xsl:value-of select ="'A'"/>
					</xsl:when>
					<xsl:when test="TaxLotState='Deleted'">
						<xsl:value-of select ="'C'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:variable>

			<TradeStatus>
				<xsl:value-of select="$varTaxlotStateGrp"/>
			</TradeStatus>

			<xsl:variable name="PRANA_COUNTERPARTY">
				<xsl:value-of select="CounterParty"/>
			</xsl:variable>

			<xsl:variable name="PB_COUNTERPARTY">
				<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = 'GS']/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@PBBroker"/>
			</xsl:variable>

			<xsl:variable name="varCounterParty">
				<xsl:choose>
					<xsl:when test="$PB_COUNTERPARTY = ''">
						<xsl:value-of select="$PRANA_COUNTERPARTY"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PB_COUNTERPARTY"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<ExecutingBroker>
				<xsl:value-of select="$varCounterParty"/>
			</ExecutingBroker>

			<SEDOL>
				<xsl:value-of select="SEDOL"/>
			</SEDOL>

			<Currency>
            <xsl:value-of select="CurrencySymbol"/>
      </Currency>

			<SettleCurrency>
				<xsl:value-of select="SettlCurrency"/>
			</SettleCurrency>
			
			<TradePriceInLocalCurrency>
            <xsl:value-of select="''"/>
            </TradePriceInLocalCurrency> 

			
			<FXrate>
				<xsl:value-of select="FXRate_Taxlot"/>
			</FXrate>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>
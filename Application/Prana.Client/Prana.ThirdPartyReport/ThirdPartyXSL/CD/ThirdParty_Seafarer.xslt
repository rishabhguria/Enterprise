<?xml version="1.0" encoding="UTF-8"?>
											
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

		<ThirdPartyFlatFileDetail>
			<!--for system internal use-->
			<RowHeader>
				<xsl:value-of select ="'true'"/>
			</RowHeader>
			
			<!--for system use only-->
			<IsCaptionChangeRequired>
				  <xsl:value-of select ="'true'"/>
			 </IsCaptionChangeRequired>

			<!--for system internal use-->
			<TaxLotState>
				<xsl:value-of select ="'TaxLotState'"/>
			</TaxLotState>

			<Side>
				<xsl:value-of select ="'Side'"/>
			</Side>

			<Ticker>
				<xsl:value-of select ="'Ticker'"/>
			</Ticker>

			<CUSIP>
				<xsl:value-of select ="'CUSIP'"/>
			</CUSIP>

			<RIC>
				<xsl:value-of select ="'RIC'"/>
			</RIC>

			<BBCode>
				<xsl:value-of select ="'BBCode'"/>
			</BBCode>

			<ISIN>
				<xsl:value-of select ="'ISIN'"/>
			</ISIN>

			<Sedol>
				<xsl:value-of select ="'Sedol'"/>
			</Sedol>

			<OrderID>
				<xsl:value-of select="'OrderID'"/>
			</OrderID>

		   <OrderQuantity>
				  <xsl:value-of select ="'Order Quantity'"/>
			</OrderQuantity>

			<TradeDate>
				  <xsl:value-of select ="'Trade Date'"/>
			 </TradeDate>

			<Settlement_Date>
				  <xsl:value-of select ="'Settlement Date'"/>
			</Settlement_Date>

			<ExecutionPrice_AvgPrice>
				  <xsl:value-of select ="'Execution Price (Avg. Price)'"/>
			</ExecutionPrice_AvgPrice>

			<ExecutingBrokerCode>
				  <xsl:value-of select="'Executing Broker Code'"/>
		    </ExecutingBrokerCode>
				
			<Account>
				<xsl:value-of select ="'Account'"/>
			</Account>

			<TradeCommission>
				  <xsl:value-of select ="'Trade Commission'"/>
			</TradeCommission>

			<SECfees>
				<xsl:value-of select ="'SEC fees'"/>
			</SECfees>

			<othermiscellaneousfees>
				<xsl:value-of select ="'other miscellaneous fees'"/>
			</othermiscellaneousfees>
			
			<StrikePrice>
				<xsl:value-of select ="'Strike Price'"/>
			</StrikePrice>

			<ExpirationDate>
				<xsl:value-of select ="'Expiration Date'"/>
			</ExpirationDate>

			<PutorCall>
				<xsl:value-of select ="'Put or Call'"/>
			</PutorCall>
				
			<UnderlyingSymbol>
				<xsl:value-of select ="'Underlying Symbol'"/>
			</UnderlyingSymbol>

			<Exchange>
				<xsl:value-of select ="'Exchange'"/>
			</Exchange>

			<TradedCurrency>
				<xsl:value-of select ="'Traded Currency'"/>
			</TradedCurrency>
			 
			<!-- system use only-->
			<EntityID>
				<xsl:value-of select="'EntityID'"/>
			</EntityID>
		</ThirdPartyFlatFileDetail>
		
      <xsl:for-each select="ThirdPartyFlatFileDetail">
		  <ThirdPartyFlatFileDetail>
			  <!--for system internal use-->
			  <RowHeader>
				  <xsl:value-of select ="'true'"/>
			  </RowHeader>

			  <!--for system use only-->
			  <IsCaptionChangeRequired>
				  <xsl:value-of select ="'false'"/>
			  </IsCaptionChangeRequired>

			  <!--for system internal use-->
			  <TaxLotState>
				  <xsl:value-of select ="TaxLotState"/>
			  </TaxLotState>

			  <xsl:variable name="varSide">
				  <xsl:choose>
					  <xsl:when test="Side='Buy' or Side='Buy to Open'">
						  <xsl:value-of select="'B'"/>
					  </xsl:when>
					  <xsl:when test="Side='Sell' or Side='Sell to Close'">
						  <xsl:value-of select="'S'"/>
					  </xsl:when>
					  <xsl:when test="Side='Sell short' or Side='Sell to Open'">
						  <xsl:value-of select="'SS'"/>
					  </xsl:when>					 
					  <xsl:when test="Side='Buy to Close'">
						  <xsl:value-of select="'BTC'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="Side"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
			  <Side>
				  <xsl:value-of select ="$varSide"/>
			  </Side>

			  <Ticker>
				  <xsl:value-of select ="Symbol"/>
			  </Ticker>

			  <CUSIP>
				  <xsl:value-of select ="CUSIP"/>
			  </CUSIP>

			  <RIC>
				  <xsl:value-of select ="RIC"/>
			  </RIC>

			  <BBCode>
				  <xsl:value-of select ="BBCode"/>
			  </BBCode>

			  <ISIN>
				  <xsl:value-of select ="ISIN"/>
			  </ISIN>

			  <Sedol>
				  <xsl:value-of select ="SEDOL"/>
			  </Sedol>
			  
			  <!--Need To Ask Below Tag-->
			  <OrderID>
				  <xsl:value-of select="''"/>
			  </OrderID>

			 
			  <OrderQuantity>
				  <xsl:value-of select ="AllocatedQty"/>
			  </OrderQuantity>

			  <TradeDate>
				  <xsl:value-of select ="TradeDate"/>
			  </TradeDate>

			  <Settlement_Date>
				  <xsl:value-of select ="SettlementDate"/>
			  </Settlement_Date>

			  <ExecutionPrice_AvgPrice>
				  <xsl:value-of select ="AveragePrice"/>
			  </ExecutionPrice_AvgPrice>

		
			  <ExecutingBrokerCode>
				  <xsl:value-of select="CounterParty"/>
			  </ExecutingBrokerCode>

			 
			  <Account>
				  <xsl:value-of select ="AccountNo"/>
			  </Account>

			
			  <TradeCommission>
				  <xsl:value-of select ="CommissionCharged"/>
			  </TradeCommission>

			  <SECfees>
				  <xsl:value-of select ="SecFees"/>
			  </SECfees>

			  <othermiscellaneousfees>
				  <xsl:value-of select ="MiscFees"/>
			  </othermiscellaneousfees>

			  <StrikePrice>
				  <xsl:value-of select ="StrikePrice"/>
			  </StrikePrice>
			  <xsl:choose>
				  <xsl:when test="PutOrCall!= ' '">
					  <ExpirationDate>
						  <xsl:value-of select ="ExpirationDate"/>
					  </ExpirationDate>
				  </xsl:when>
				  <xsl:otherwise>
					  <ExpirationDate>
						  <xsl:value-of select ="''"/>
					  </ExpirationDate>
				  </xsl:otherwise>
			  </xsl:choose>

			  <!---->
			  <PutorCall>
				  <xsl:value-of select ="PutOrCall"/>
			  </PutorCall>

			  <UnderlyingSymbol>
				  <xsl:value-of select ="UnderlyingSymbol"/>
			  </UnderlyingSymbol>

			  <Exchange>
				  <xsl:value-of select ="Exchange"/>
			  </Exchange>

			  <TradedCurrency>
				  <xsl:value-of select ="CurrencySymbol"/>
			  </TradedCurrency>

			  <!-- system use only-->
			  <EntityID>
				  <xsl:value-of select="EntityID"/>
			  </EntityID>
		  </ThirdPartyFlatFileDetail>
	  </xsl:for-each>
  </ThirdPartyFlatFileDetailCollection>
</xsl:template>
</xsl:stylesheet>

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

			
			<TransactionPortfolioCode>
				<xsl:value-of select ="'Transaction Portfolio Code'"/>
			</TransactionPortfolioCode>

			<RelationshipType>
				<xsl:value-of select ="'Relationship Type'"/>
			</RelationshipType>

			<TransType>
				<xsl:value-of select ="'Trans Type'"/>
			</TransType>

			<ActionType>
				<xsl:value-of select ="'Action Type'"/>
			</ActionType>
			
			<ProductType>
				<xsl:value-of select="'Product Type'"/>
			</ProductType>

			<SecurityCode>
				  <xsl:value-of select ="'Security Code'"/>
			</SecurityCode>

			<SecurityCodeType>
				  <xsl:value-of select ="'Security Code Type'"/>
			 </SecurityCodeType>

			<Exchange>
				  <xsl:value-of select ="'Exchange'"/>
			</Exchange>

			<NominalorUnderlyingQuantity>
				  <xsl:value-of select ="'Nominal or Underlying Quantity'"/>
			</NominalorUnderlyingQuantity>

			<Price>
				  <xsl:value-of select="'Price'"/>
		    </Price>


			<Currency>
				<xsl:value-of select ="'Currency'"/>
			</Currency>

			<AmountNetMoney>
				  <xsl:value-of select ="'Amount ( Net Money)'"/>
			</AmountNetMoney>

			<ClientCounterParty>
				  <xsl:value-of select ="'Client Counter Party'"/>
			</ClientCounterParty>

			<TradeDate>
				<xsl:value-of select ="'Trade Date'"/>
			</TradeDate>

			<SettleDate>
				  <xsl:value-of select ="'Settle Date'"/>
			</SettleDate>

			<AccruedInterest>
				  <xsl:value-of select ="'Accrued Interest'"/>
			</AccruedInterest>

			<Commission>
				  <xsl:value-of select ="'Commission'"/>
			</Commission>

			<CommissionTypeCode>
				  <xsl:value-of select ="'Commission Type Code'"/>
			</CommissionTypeCode>

			<SecFees>
				  <xsl:value-of select ="'Sec Fees'"/>
			</SecFees>

			<ClientCouponRateStrikePrice>
				<xsl:value-of select ="'Client Coupon Rate/Strike Price'"/>
			</ClientCouponRateStrikePrice>


			<ClientMaturityDateExpirationDate>
					<xsl:value-of select ="'Client Maturity Date/Expiration Date'"/>
			</ClientMaturityDateExpirationDate>

			<Blotter>
				  <xsl:value-of select ="'Blotter'"/>
			</Blotter>

		  <ClientID>
				<xsl:value-of select ="'Client ID'"/>
		  </ClientID>

			<Trailer>
				  <xsl:value-of select ="'Trailer'"/>
			</Trailer>

			 
			<!-- system use only-->
			<EntityID>
				<xsl:value-of select="'EntityID'"/>
			</EntityID>
		</ThirdPartyFlatFileDetail>
		
      <xsl:for-each select="ThirdPartyFlatFileDetail">
			  <xsl:if test ="AccountNo!='441885' and AccountNo!='32577' and AccountNo!='465568' and AccountNo!='465569'and AccountNo!='465572' and AccountNo!='465573'and AccountNo!='465574' and AccountNo!='465575' and AccountNo!='465576' and AccountNo!='465577'  and AccountNo!='472993'  and AccountNo!='472989'  and AccountNo!='472987' and AccountNo!='472991' and AccountNo!='472990'  and AccountNo!='472988'  and AccountNo!='472986' and AccountNo!='472985'  and AccountNo!='472992'">
		  <ThirdPartyFlatFileDetail>
			  <!--for system internal use-->
			  <RowHeader>
				  <xsl:value-of select ="'true'"/>
			  </RowHeader>

			  <xsl:variable name ="varCurrency">
				  <xsl:value-of select ="CurrencySymbol"/>
			  </xsl:variable>

			  <!--<xsl:variable name="SwapFxRate">
				  <xsl:value-of select="document('../ReconMappingXml/SwapFxRates.xml')/FxRate/Rate[@Name='SENSATO']/FxData[@CurrSymbol=$varCurrency]/@FxPrice"/>
			  </xsl:variable>-->

			  <!-- Non Swap Accounts does not need to be converted in to USD so normalized rate should be 1 -->
			  <xsl:variable name="NormalizedFxRate">
				  <xsl:choose>
					  <xsl:when test="ForexRate_Trade='' or ForexRate_Trade='0' or AccountNo = 'SAPMFLP' or AccountNo = 'SSAPMF' or AccountNo = 'SENS2'">
						  <xsl:value-of select="1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="ForexRate_Trade"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
			  <!--for system use only-->
			  <IsCaptionChangeRequired>
				  <xsl:value-of select ="'true'"/>
			  </IsCaptionChangeRequired>
			  <!--for system internal use-->
			  
			  <TaxLotState>
				  <xsl:value-of select ="TaxLotState"/>
			  </TaxLotState>
			
			  <xsl:choose>
				  <xsl:when test="AccountNo='441885'">
					  <TransactionPortfolioCode>
						  <xsl:value-of select="'SEASPA1'"/>
					  </TransactionPortfolioCode>
				  </xsl:when>
				  <xsl:when test="AccountNo = '32577'">
					  <TransactionPortfolioCode>
						  <xsl:value-of select="'SEAPMF1'"/>
					  </TransactionPortfolioCode>
				  </xsl:when>
				  <!--<xsl:when test="AccountNo='SAPMFLP'">
					  <TransactionPortfolioCode>
						  <xsl:value-of select="'SAPMFLP'"/>
					  </TransactionPortfolioCode>
				  </xsl:when>
				  <xsl:when test="AccountNo = 'SSAPMF'">
					  <TransactionPortfolioCode>
						  <xsl:value-of select="'SSAPMF'"/>
					  </TransactionPortfolioCode>
				  </xsl:when>-->
				  <xsl:otherwise>
					  <TransactionPortfolioCode>
						  <xsl:value-of select="AccountNo"/>
					  </TransactionPortfolioCode>
				  </xsl:otherwise>
			  </xsl:choose>
			  
			  <xsl:choose>
				  <xsl:when test="AccountNo='441885' or AccountNo = '32577' or AccountNo='472993'  or AccountNo='472989'  or AccountNo='472987' or AccountNo='472991' or AccountNo='472990'  or AccountNo='472988'  or AccountNo='472986' or AccountNo='472985'  or AccountNo='472992'">
					  <RelationshipType>
						  <xsl:value-of select="'S'"/>
					  </RelationshipType>
				  </xsl:when>
				  <xsl:otherwise>
					  <RelationshipType>
						  <xsl:value-of select="'I'"/>
					  </RelationshipType>
				  </xsl:otherwise>
			  </xsl:choose>
			  

        <xsl:choose>
          <xsl:when test="Side='Sell short' or Side = 'Sell to Open'">
            <TransType>
              <xsl:value-of select="'SS'"/>
            </TransType>
          </xsl:when>
          <xsl:when test="Side='Buy' or Side='Buy to Open'">
            <TransType>
              <xsl:value-of select="'BL'"/>
            </TransType>
          </xsl:when>
          <xsl:when test="Side='Sell' or Side= 'Sell to Close'">
            <TransType>
              <xsl:value-of select="'SL'"/>
            </TransType>
          </xsl:when>
          <xsl:when test="Side='Buy to Close'">
            <TransType>
              <xsl:value-of select="'BS'"/>
            </TransType>
          </xsl:when>
          <xsl:otherwise>
            <TransType>
              <xsl:value-of select="Side"/>
            </TransType>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
				  <xsl:when test ="TaxLotState= 'Allocated'">
					  <ActionType>
						  <xsl:value-of select ="'N'"/>
					  </ActionType>
				  </xsl:when>
				  <xsl:when test ="TaxLotState= 'Deleted'">
					  <ActionType>
						  <xsl:value-of select ="'C'"/>
					  </ActionType>
				  </xsl:when>
				  <xsl:when test ="TaxLotState= 'Amemded'">
					  <ActionType>
						  <xsl:value-of select ="'A'"/>
					  </ActionType>
				  </xsl:when>
				  <xsl:otherwise>
					  <ActionType>
						  <xsl:value-of select ="'N'"/>
					  </ActionType>
				  </xsl:otherwise>
			  </xsl:choose>

			  <xsl:choose>
				  <xsl:when test="Asset='Equity'">
					  <ProductType>
						  <xsl:value-of select="'EQ'"/>
					  </ProductType>
				  </xsl:when>
				  <xsl:when test="Asset='EquityOption'">
					  <ProductType>
						  <xsl:value-of select="'OP'"/>
					  </ProductType>
				  </xsl:when>
				  <xsl:when test="Asset='FixedIncome'">
					  <ProductType>
						  <xsl:value-of select="'FI'"/>
					  </ProductType>
				  </xsl:when>
				  <xsl:otherwise>
					  <ProductType>
						  <xsl:value-of select="Asset"/>
					  </ProductType>
				  </xsl:otherwise>
			  </xsl:choose>

        <xsl:choose>
          <xsl:when test ="Asset='Equity'">
            <SecurityCode>
              <xsl:value-of select ="SEDOL"/>
            </SecurityCode>
          </xsl:when>
          <xsl:otherwise>
            <SecurityCode>
              <xsl:value-of select ="Symbol"/>
            </SecurityCode>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test ="Asset='Equity'">
            <SecurityCodeType>
              <xsl:value-of select ="'SE'"/>
            </SecurityCodeType>
          </xsl:when>
          <xsl:otherwise>
            <SecurityCodeType>
              <xsl:value-of select ="'SY'"/>
            </SecurityCodeType>
          </xsl:otherwise>
        </xsl:choose>        
			
			  <Exchange>
				  <xsl:value-of select ="Exchange"/>
			  </Exchange>

			  <NominalorUnderlyingQuantity>
				  <xsl:value-of select ="AllocatedQty"/>
			  </NominalorUnderlyingQuantity>

			  <!--<Price>
				  <xsl:value-of select="AveragePrice*$NormalizedFxRate"/>
			  </Price>-->


					  <!--<Price>
						  <xsl:value-of select='format-number(AveragePrice, "0.0000")'/>
					  </Price>-->

			  <xsl:choose>
				  <xsl:when test ="CounterParty='GSPrg' or CounterParty='GSElec'">
					  <Price>
						  <xsl:value-of select='format-number(AveragePrice, "0.000000")'/>
					  </Price>
				  </xsl:when>
				  <xsl:otherwise >
					  <Price>
						  <xsl:value-of select='format-number(AveragePrice, "0.0000")'/>
					  </Price>
				  </xsl:otherwise>
			  </xsl:choose >
			
			  <!--<xsl:variable name="varCurrency">
				  <xsl:choose>
					  <xsl:when test="contains(CurrencySymbol,'MUL') =false">
						  <xsl:value-of select="CurrencySymbol"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>-->

			  <xsl:choose>
				  <xsl:when test="AccountNo='441885' or AccountNo = '32577' or AccountNo='472993'  or AccountNo='472989'  or AccountNo='472987' or AccountNo='472991' or AccountNo='472990'  or AccountNo='472988'  or AccountNo='472986' or AccountNo='472985'  or AccountNo='472992'">
					  <Currency>
						  <xsl:value-of select="'USD'"/>
					  </Currency>
				  </xsl:when>
				  <xsl:otherwise>
					  <Currency>
						  <xsl:value-of select="CurrencySymbol"/>
					  </Currency>
				  </xsl:otherwise>
			  </xsl:choose>

			  <!--<AmountNetMoney>
				  <xsl:value-of select ="NetAmount*$NormalizedFxRate"/>
			  </AmountNetMoney>-->


			  <AmountNetMoney>
				  <xsl:choose>
					  
					
							  <xsl:when test ="CurrencySymbol = 'JPY'">
								  <xsl:value-of select='format-number(NetAmount, "###")'/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select='format-number(NetAmount, "###.00")'/>
							  </xsl:otherwise>
					
				  </xsl:choose>
			  </AmountNetMoney>

						<!--<xsl:variable name="varCounterParty">
				  <xsl:choose>
					  <xsl:when test="CounterParty = 'Undefined'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="CounterParty"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>-->
			  
			  <xsl:choose>
				  <xsl:when test ="CounterParty='GSPrg' or CounterParty='GSElec'">
					  <ClientCounterParty>
						  <xsl:value-of select="'GSCO'"/>
					  </ClientCounterParty>
				  </xsl:when>
				  <xsl:when test ="CounterParty='UBS Program' or CounterParty='UBS Electronic'">
					  <ClientCounterParty>
						  <xsl:value-of select="'UBSW'"/>
					  </ClientCounterParty>
				  </xsl:when>
				  <xsl:when test ="CounterParty='DBPrg' or CounterParty='DBElec'">
					  <ClientCounterParty>
						  <xsl:value-of select="'DBSI'"/>
					  </ClientCounterParty>
				  </xsl:when>
				  <xsl:when test ="CounterParty='INSTElec' or CounterParty='DBElec'">
					  <ClientCounterParty>
						  <xsl:value-of select="'INSTINET'"/>
					  </ClientCounterParty>
				  </xsl:when>
				   <xsl:when test ="CounterParty='CSElect' or CounterParty='CSPrg'">
					  <ClientCounterParty>
						  <xsl:value-of select="'CSELECT'"/>
					  </ClientCounterParty>
				  </xsl:when>
				  <xsl:otherwise >
					  <ClientCounterParty>
						  <xsl:value-of select="CounterParty"/>
					  </ClientCounterParty>
				  </xsl:otherwise>
			  </xsl:choose >
			  
			  <TradeDate>
				  <xsl:value-of select ="TradeDate"/>
			  </TradeDate>

			  <SettleDate>
				  <xsl:value-of select ="SettlementDate"/>
			  </SettleDate>

			  <AccruedInterest>
				  <xsl:value-of select ="''"/>
			  </AccruedInterest>

			  <!--<Commission>
				  <xsl:value-of select ="CommissionCharged*$NormalizedFxRate"/>
			  </Commission>-->

			  <Commission>
				  <xsl:choose>
					  <xsl:when test ="CurrencySymbol = 'JPY'">
						  <xsl:value-of select='format-number(CommissionCharged, "###")'/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select='format-number(CommissionCharged, "###.00")'/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Commission>

			  <CommissionTypeCode>
				  <xsl:value-of select ="'D'"/>
			  </CommissionTypeCode>
			  
			  <xsl:variable name="varSecFees">
				  <xsl:value-of select="(StampDuty + TransactionLevy + SecFees + OtherBrokerFee + ClearingFee + MiscFees + TaxOnCommissions)*$NormalizedFxRate"/>
			  </xsl:variable>
			  
			  <SecFees>
				  <xsl:value-of select ="$varSecFees*$NormalizedFxRate"/>
			  </SecFees>

			  <xsl:choose>
				  <xsl:when test ="Asset='EquityOption'">
					  <ClientCouponRateStrikePrice>
						  <xsl:value-of select ="StrikePrice"/>
					  </ClientCouponRateStrikePrice>
				  </xsl:when>
				  <xsl:otherwise>
					  <ClientCouponRateStrikePrice>
						  <xsl:value-of select ="''"/>
					  </ClientCouponRateStrikePrice>
				  </xsl:otherwise>
			  </xsl:choose>
			  
			
			  
			  <xsl:variable name="varExpirationDate">
				  <xsl:choose>
					  <xsl:when test="StrikePrice =0">
						  <xsl:value-of select ="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="ExpirationDate"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:choose>
				  <xsl:when test ="Asset='Equity'">
					  <ClientMaturityDateExpirationDate>
						  <xsl:value-of select ="''"/>
					  </ClientMaturityDateExpirationDate>
				  </xsl:when>
				  <xsl:otherwise>
					  <ClientMaturityDateExpirationDate>
						  <xsl:value-of select ="ExpirationDate"/>
					  </ClientMaturityDateExpirationDate>
				  </xsl:otherwise>
			  </xsl:choose>		

			  <Blotter>
				  <xsl:value-of select ="''"/>
			  </Blotter>

			  <ClientID>
				  <xsl:value-of select ="TradeRefID"/>
			  </ClientID>

			  <Trailer>
				  <xsl:value-of select ="''"/>
			  </Trailer>

			  <!-- system use only-->
			  <EntityID>
				  <xsl:value-of select="EntityID"/>
			  </EntityID>

		  </ThirdPartyFlatFileDetail>
	  </xsl:if>
	  </xsl:for-each>
  </ThirdPartyFlatFileDetailCollection>
</xsl:template>
</xsl:stylesheet>

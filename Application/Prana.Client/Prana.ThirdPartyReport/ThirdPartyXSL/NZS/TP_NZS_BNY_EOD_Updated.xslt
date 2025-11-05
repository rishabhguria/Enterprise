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

          <ClientName>
            <xsl:value-of select ="'Client Name'"/>
          </ClientName>

          <AccountID>
            <xsl:value-of select ="'Account ID'"/>
          </AccountID>

          <ClientTradeRefNo>
            <xsl:value-of select ="'Client Trade Ref No.'"/>
          </ClientTradeRefNo>

          <ConfirmedUnconfirmedAmended>
            <xsl:value-of select ="'Confirmed/Unconfirmed/Amended'"/>
          </ConfirmedUnconfirmedAmended>

          <CancelledYN>
            <xsl:value-of select ="'Cancelled Y/N'"/>
          </CancelledYN>

          <BrokerDescription>
            <xsl:value-of select ="'Broker Description'"/>
          </BrokerDescription>

          <BrokerCode>
            <xsl:value-of select ="'Broker Code'"/>
          </BrokerCode>

          <TradeDate>
            <xsl:value-of select ="'Trade Date'"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select ="'Settle Date'"/>
          </SettleDate>

          <AssetType>
            <xsl:value-of select ="'Asset Type'"/>
          </AssetType>

          <SecurityIdentifierCUSIP>
            <xsl:value-of select ="'SecurityIdentifier CUSIP'"/>
          </SecurityIdentifierCUSIP>

          <SecurityIdentifierSEDOL>
            <xsl:value-of select ="'SecurityIdentifier SEDOL'"/>
          </SecurityIdentifierSEDOL>

          <SecurityIdentifierISIN>
            <xsl:value-of select ="'SecurityIdentifier ISIN'"/>
          </SecurityIdentifierISIN>

          <SecurityDescription>
            <xsl:value-of select ="'Security Description'"/>
          </SecurityDescription>

          <FactoredBondYN>
            <xsl:value-of select ="'Factored Bond Y/N'"/>
          </FactoredBondYN>

          <IPOPlacingYN>
            <xsl:value-of select ="'IPO/Placing Y/N'"/>
          </IPOPlacingYN>

          <IPOPlacingtobebookedatcostYN>
            <xsl:value-of select ="'IPO / Placing to be booked at cost Y/N'"/>
          </IPOPlacingtobebookedatcostYN>

          <CouponRate>
            <xsl:value-of select ="'Coupon Rate'"/>
          </CouponRate>

          <IssueDate>
            <xsl:value-of select ="'Issue Date'"/>
          </IssueDate>

          <MaturityDate>
            <xsl:value-of select ="'Maturity Date'"/>
          </MaturityDate>

          <TransactionCode>
            <xsl:value-of select ="'Transaction Code'"/>
          </TransactionCode>

          <OriginalFace>
            <xsl:value-of select ="'Original Face'"/>
          </OriginalFace>

          <CurrentFace>
            <xsl:value-of select ="'Current Face'"/>
          </CurrentFace>

          <Price>
            <xsl:value-of select ="'Price'"/>
          </Price>

          <GrossPrincipallocal>
            <xsl:value-of select ="'Gross Principal (local)'"/>
          </GrossPrincipallocal>

          <Commissionlocal>
            <xsl:value-of select ="'Commission (local)'"/>
          </Commissionlocal>

          <Interestlocal>
            <xsl:value-of select ="'Interest (local)'"/>
          </Interestlocal>

          <FeesTradeExpenselocal>
            <xsl:value-of select ="'Fees Trade Expense (local)'"/>
          </FeesTradeExpenselocal>

          <TradeCurrency>
            <xsl:value-of select ="'Trade Currency'"/>
          </TradeCurrency>

          <SettleCurrency>
            <xsl:value-of select ="'Settle Currency'"/>
          </SettleCurrency>
          

          <NetSettleAmount>
            <xsl:value-of select ="'Net Settle Amount'"/>
          </NetSettleAmount>

          <UnderlyingsecurityISIN>
            <xsl:value-of select ="'Underlying security ISIN'"/>
          </UnderlyingsecurityISIN>

          <CurrencyReceived>
            <xsl:value-of select ="'Currency Received'"/>
          </CurrencyReceived>

          <AmountReceived>
            <xsl:value-of select ="'Amount Received'"/>
          </AmountReceived>
          
          <CurrencyDelivered>
            <xsl:value-of select ="'Currency Delivered'"/>
          </CurrencyDelivered>

          <AmountDelivered>
            <xsl:value-of select ="'Amount Delivered'"/>
          </AmountDelivered>

          <FXRate>
            <xsl:value-of select ="'FXRate'"/>
          </FXRate>

          <ShareClass>
            <xsl:value-of select ="'Share Class'"/>
          </ShareClass>

          <CurrencyContractType>
            <xsl:value-of select ="'Currency Contract Type'"/>
          </CurrencyContractType>

          <NDF>
            <xsl:value-of select ="'NDF'"/>
          </NDF>

          <Ticker>
            <xsl:value-of select ="'Ticker'"/>
          </Ticker>

          <BloombergGlobalID>
            <xsl:value-of select ="'Bloomberg Global ID'"/>
          </BloombergGlobalID>

          <CallPut>
            <xsl:value-of select ="'Call/Put'"/>
          </CallPut>
          
          <ContractSize>
            <xsl:value-of select ="'Contract Size'"/>
          </ContractSize>

          <StrikePrice>
            <xsl:value-of select ="'Strike Price'"/>
          </StrikePrice>

          <ExpiryDate>
            <xsl:value-of select ="'Expiry Date'"/>
          </ExpiryDate>

          <SecurityIdentifierBLID>
            <xsl:value-of select ="'Security Identifier BL ID'"/>
          </SecurityIdentifierBLID>

          <SecurityIdentifierLXID>
            <xsl:value-of select ="'Security Identifier LX ID'"/>
          </SecurityIdentifierLXID>

          <FirstPaymentCouponDate>
            <xsl:value-of select ="'First Payment/Coupon Date'"/>
          </FirstPaymentCouponDate>
          
          <PreviousCouponRate>
            <xsl:value-of select ="'Previous Coupon Rate'"/>
          </PreviousCouponRate>
          
          <NextCouponRate>
            <xsl:value-of select ="'Next Coupon Rate'"/>
          </NextCouponRate>
          
          <PaymentFrequency>
            <xsl:value-of select ="'Payment Frequency'"/>
          </PaymentFrequency>
          
          <ResetFrequencyType>
            <xsl:value-of select ="'Reset Frequency Type'"/>
          </ResetFrequencyType>
          
          <AnnualRateType>
            <xsl:value-of select ="'Annual Rate Type'"/>
          </AnnualRateType>
          
          <FixedFloatingFlag>
            <xsl:value-of select ="'Fixed / FloatingFlag'"/>
          </FixedFloatingFlag>
          
          <IssuerName>
            <xsl:value-of select ="'Issuer Name'"/>
          </IssuerName>
          
          <Country>
            <xsl:value-of select ="'Country'"/>
          </Country>
          
          <ETFOrderID>
            <xsl:value-of select ="'ETF Order ID'"/>
          </ETFOrderID>
          
          <EntityID>
            <xsl:value-of select="'EntityID'"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName ='Challenge Technology EQ Evolution3']">

        <ThirdPartyFlatFileDetail>
          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <ClientName>
            <xsl:value-of select ="AccountName"/>
          </ClientName>

          <AccountID>
            <xsl:value-of select ="AccountNo"/>
          </AccountID>
          <xsl:variable name="varClientTradeRefNo">            
          <xsl:choose>
            <xsl:when test="string-length(position())=1">
              <xsl:value-of select=" position()"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="position()"/>
            </xsl:otherwise>
          </xsl:choose>
          </xsl:variable>
          <ClientTradeRefNo>
            <xsl:value-of select ="PBUniqueID"/>
          </ClientTradeRefNo>

          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'Confirmed'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'Amended'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'Confirmed'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

         
          <ConfirmedUnconfirmedAmended>
            <xsl:value-of select="$varTaxlotState"/>
          </ConfirmedUnconfirmedAmended>

          <xsl:variable name="varCancelledYN">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'N'"/>
              </xsl:when>
            
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'Y'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <CancelledYN>
            <xsl:value-of select ="$varCancelledYN"/>
          </CancelledYN>
          
          <xsl:variable name="PB_NAME">
            <xsl:value-of select="'Northern Trust'"/>
          </xsl:variable>
		  
		  <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
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

					
       
          
          <BrokerDescription>
             <xsl:value-of select="$Broker"/>
          </BrokerDescription>

          <BrokerCode>
            <xsl:value-of select ="CounterParty"/>
          </BrokerCode>

          <TradeDate>
            <xsl:value-of select ="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select ="SettlementDate"/>
          </SettleDate>
          <xsl:variable name="varAsset">
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'EQ'"/>
              </xsl:when>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="'FX'"/>
              </xsl:when>
              <xsl:when test="Asset='FXForward'">
                <xsl:value-of select="'Forward'"/>
              </xsl:when>
             
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'O'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          

          <AssetType>
            <xsl:value-of select ="$varAsset"/>
          </AssetType>

          <SecurityIdentifierCUSIP>
            <xsl:choose>
              <xsl:when test="Asset='Equity' or Asset='Option'">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityIdentifierCUSIP>

          <SecurityIdentifierSEDOL>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityIdentifierSEDOL>

          <SecurityIdentifierISIN>
            <xsl:choose>
              <xsl:when test="Asset='Equity' or Asset='EquityOption'">
                <xsl:value-of select="ISIN"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityIdentifierISIN>

          <SecurityDescription>
            <xsl:value-of select ="FullSecurityName"/>
          </SecurityDescription>

          <FactoredBondYN>
            <xsl:choose>
              <xsl:when test="Asset='Equity' or Asset='EquityOption'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </FactoredBondYN>

          <IPOPlacingYN>
            <xsl:choose>
              <xsl:when test="Asset='Equity' or Asset='EquityOption'">
                <xsl:value-of select="'N'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </IPOPlacingYN>

          <IPOPlacingtobebookedatcostYN>
            <xsl:choose>
              <xsl:when test="Asset='Equity' or Asset='EquityOption'">
                <xsl:value-of select="'N'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </IPOPlacingtobebookedatcostYN>

          <CouponRate>
            <xsl:value-of select ="''"/>
          </CouponRate>

          <IssueDate>
            <xsl:value-of select ="''"/>
          </IssueDate>

          <MaturityDate>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </MaturityDate>

          <TransactionCode>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Cover'">
                    <xsl:value-of select="'BUYCVR'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'SHORTSELL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>

            </xsl:choose>

            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'BUYCVR'"/>
                  </xsl:when>

                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'SHORTSELL'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              </xsl:choose>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="'OPCCT'"/>
              </xsl:when>
            </xsl:choose>
          
          </TransactionCode>

          <OriginalFace>

            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="AllocatedQty"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </OriginalFace>

          <CurrentFace>

            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="AllocatedQty"/>
              </xsl:otherwise>
            </xsl:choose>
          </CurrentFace>

          <Price>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="AveragePrice"/>
              </xsl:otherwise>
            </xsl:choose>           
          </Price>
          
          <xsl:variable name = "varGrossPrinLocal">
            <xsl:value-of select="AveragePrice * AllocatedQty"/>
          </xsl:variable>
          <GrossPrincipallocal>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="GrossAmount"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </GrossPrincipallocal>


          <xsl:variable name = "varTotalComm">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
          <Commissionlocal>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="$varTotalComm"/>
              </xsl:otherwise>
            </xsl:choose>           
          </Commissionlocal>

          <Interestlocal>          
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="AccruedInterest"/>
              </xsl:otherwise>
            </xsl:choose>
          </Interestlocal>

          <FeesTradeExpenselocal>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee + SecFee)"/>
              </xsl:otherwise>
            </xsl:choose>
          
          </FeesTradeExpenselocal>

          <TradeCurrency>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="CurrencySymbol"/>
              </xsl:otherwise>
            </xsl:choose>          
          </TradeCurrency>

          <SettleCurrency>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="SettlCurrency"/>
              </xsl:otherwise>
            </xsl:choose>           
          </SettleCurrency>


          <NetSettleAmount>
		  <xsl:choose>
              <xsl:when test="Asset='Equity'">
               <xsl:value-of select ="NetAmount"/>
              </xsl:when>            
            </xsl:choose>
          </NetSettleAmount>

          <UnderlyingsecurityISIN>
            <xsl:value-of select ="ISIN"/>
          </UnderlyingsecurityISIN>

          <CurrencyReceived>
            <xsl:choose>
              <xsl:when test="Side='Buy' and Asset='FX'">
                <xsl:value-of select="substring-before(Symbol,'/')"/>
              </xsl:when>
              <xsl:when test="Side='Sell' and Asset='FX' ">
                <xsl:value-of select="substring-after(Symbol,'/')"/>
              </xsl:when>
              
            </xsl:choose>
         
          </CurrencyReceived>  

          <AmountReceived>
            <xsl:choose>
              <xsl:when test="Side='Buy' and Asset='FX'">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:when test="Side='Sell'  and Asset='FX'">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>
             
            </xsl:choose>
          </AmountReceived>

          <CurrencyDelivered>
            <xsl:choose>
              <xsl:when test="Side='Sell' and Asset='FX'">
                <xsl:value-of select="substring-before(Symbol,'/')"/>
              </xsl:when>
              <xsl:when test="Side='Buy' and Asset='FX'">
                <xsl:value-of select="substring-after(Symbol,'/')"/>
              </xsl:when>
             
            </xsl:choose>
          </CurrencyDelivered>

          <AmountDelivered>
            <xsl:choose>
              <xsl:when test="Side='Sell' and Asset='FX'">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:when test="Side='Buy' and Asset='FX'">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>
              
            </xsl:choose>
          </AmountDelivered>

          <FXRate>
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
			  <xsl:when test="Asset='Equity'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXRate>

          <ShareClass>
            <xsl:value-of select ="''"/>
          </ShareClass>

          <xsl:variable name="varCurrencyContractType">
            <xsl:choose>
              <xsl:when test="Asset='FX'">
                <xsl:value-of select="'SPOT'"/>
              </xsl:when>
              <xsl:when test="Asset='FXForward'">
                <xsl:value-of select="'FORWARD'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
         
          <CurrencyContractType>
            <xsl:value-of select="$varCurrencyContractType"/>       
          </CurrencyContractType>

          <NDF>
            <xsl:choose>
              <xsl:when test="Asset='FX' or Asset='FXForward'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </NDF>

          <Ticker>
            <xsl:value-of select ="Symbol"/>
          </Ticker>

          <BloombergGlobalID>
            <xsl:choose>
              <xsl:when test="Asset='Future' or Asset='EquityOption'">
                <xsl:value-of select="BBCode"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BloombergGlobalID>

          <CallPut>
            <xsl:choose>
              <xsl:when test="PutOrCall='Call'">
                <xsl:value-of select="'CALL'"/>
              </xsl:when>
              <xsl:when test="PutOrCall='Put'">
                <xsl:value-of select="'PUT'"/>
              </xsl:when>
            </xsl:choose>
          </CallPut>

          <ContractSize>
            <xsl:value-of select ="''"/>
          </ContractSize>

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

          <ExpiryDate>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExpiryDate>

          <SecurityIdentifierBLID>
            <xsl:value-of select ="''"/>
          </SecurityIdentifierBLID>

          <SecurityIdentifierLXID>
            <xsl:value-of select ="''"/>
          </SecurityIdentifierLXID>

          <FirstPaymentCouponDate>
            <xsl:value-of select ="''"/>
          </FirstPaymentCouponDate>

          <PreviousCouponRate>
            <xsl:value-of select ="''"/>
          </PreviousCouponRate>

          <NextCouponRate>
            <xsl:value-of select ="''"/>
          </NextCouponRate>

          <PaymentFrequency>
            <xsl:value-of select ="''"/>
          </PaymentFrequency>

          <ResetFrequencyType>
            <xsl:value-of select ="''"/>
          </ResetFrequencyType>

          <AnnualRateType>
            <xsl:value-of select ="''"/>
          </AnnualRateType>

          <FixedFloatingFlag>
            <xsl:value-of select ="''"/>
          </FixedFloatingFlag>

          <IssuerName>
            <xsl:value-of select ="''"/>
          </IssuerName>

          <Country>
            <xsl:value-of select ="''"/>
          </Country>

          <ETFOrderID>
            <xsl:value-of select ="''"/>
          </ETFOrderID>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>
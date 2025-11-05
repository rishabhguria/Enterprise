<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>		
			
			<xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOX Collapse' and CounterParty!='SwapReset' and CounterParty!='Swap Reset']">
				
				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>					

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<ProductCode>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and IsSwapped='true'">
								<xsl:value-of select="'SWAP'"/>
							</xsl:when>
							<xsl:when test="Asset = 'Equity'" >
								<xsl:value-of select="'Equity'"/>
							</xsl:when>
              <xsl:when test="Asset = 'EquityOption'" >
                <xsl:value-of select="'EquityOption'"/>
              </xsl:when>
              <xsl:when test="Asset = 'FixedIncome'" >
                <xsl:value-of select="'FixedIncome'"/>
              </xsl:when>
              <xsl:when test="Asset='FX'">
								<xsl:value-of select="'FX'"/>
							</xsl:when>
							 <xsl:when test="Asset='FXForward'">
								<xsl:value-of select="'FXForward'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ProductCode>

					<xsl:variable name="PB_NAME" select="'UBS'"/>

					<xsl:variable name="PRANA_FUND_NAME" select="AccountNo"/>

					<xsl:variable name="PB_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund = $PRANA_FUND_NAME]/@PBFundName"/>
					</xsl:variable>

					<AccountID>
						<xsl:choose>
							<xsl:when test="$PB_FUND_NAME!=''">
								<xsl:value-of select="$PB_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountID>

					<TradeRefID>
						<xsl:value-of select="TradeRefID"/>
					</TradeRefID>

					<TradeDate>
						<xsl:value-of select="concat(substring(TradeDate,7,4), substring(TradeDate,1,2), substring(TradeDate,4,2))"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="concat(substring(SettlementDate,7,4), substring(SettlementDate,1,2), substring(SettlementDate,4,2))"/>
					</SettlementDate>

					<ActionCode>
						<xsl:choose>
							<xsl:when test ="TaxLotState='Amended'">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'AB'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'ABC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'AS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'ASHS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test ="TaxLotState='Deleted'">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'XB'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'XBC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'XS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'XSS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'BC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</ActionCode>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>
					
					<xsl:variable name="varFxrate">
					<xsl:choose>
							<xsl:when test="contains(Asset,'FX')">
								<xsl:choose>
									<xsl:when test="Asset='FX'">
										<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number(ForexRate,'#.####')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						

						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="number(ForexRate)">
									<xsl:value-of select="ForexRate"/>
								</xsl:when>

								<xsl:when test="number(FXRate_Taxlot)">
									<xsl:value-of select="FXRate_Taxlot"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<xsl:variable name="varSettFxAmt">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
										<xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AveragePrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="Price">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varSettFxAmt"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<Price>
						
								<xsl:value-of select="$Price"/>
							
					</Price>

					<xsl:variable name="varSettFxNetAmount">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
										<xsl:value-of select="NetAmount * FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="NetAmount div FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="NetAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="NetAmount">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varSettFxNetAmount"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<NetAmount>
					
										<xsl:value-of select="$NetAmount"/>
								
						
					</NetAmount>

					<SettlementCcy>
				  	  <xsl:value-of select="SettlCurrency"/>
					</SettlementCcy>
					
					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>
				

          <SEDOL>            
           <xsl:value-of select="SEDOL"/>
          </SEDOL>


          

           <xsl:variable name="varTotalCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

					<ExecBrokerCode>
                           <xsl:value-of select="CounterParty"/>
					</ExecBrokerCode>
					
					<xsl:variable name="varSettCommission">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
							<xsl:choose>
								<xsl:when test="FXConversionMethodOperator_Trade ='M'">
									<xsl:value-of select="$varTotalCommission * FXRate_Taxlot"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varTotalCommission div FXRate_Taxlot"/>
								</xsl:otherwise>
							</xsl:choose>
							</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$varTotalCommission"/>
						</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<CommissionAmount>
						<xsl:choose>
							<xsl:when test ="number($varSettCommission)">
								<xsl:value-of select="$varSettCommission"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionAmount>
					
					<OtherCharges>
						<xsl:value-of select ="OtherBrokerFee + SecFee + OccFee + OrfFee + ClearingFee + StampDuty + TransactionLevy + TaxOnCommissions + MiscFees"/>
					</OtherCharges>

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

					<BuyCurrency>
						<xsl:choose>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Buy')">
								<xsl:value-of select="LeadCurrencyName"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Sell')">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BuyCurrency>

					<SellCurrency>
						<xsl:choose>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Sell')">
								<xsl:value-of select ="LeadCurrencyName"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Buy')">
								<xsl:value-of select="VsCurrencyName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SellCurrency>

					<BuyAmount>
						<xsl:choose>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Buy')">
								<xsl:value-of select="AllocatedQty" />
							</xsl:when>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Sell')">
								<xsl:value-of select="AllocatedQty * AveragePrice" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BuyAmount>

					<SellAmount>
						<xsl:choose>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Buy')">
								<xsl:value-of select ="AllocatedQty * AveragePrice"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'FX') and contains($varTransactionType,'Sell')">
								<xsl:value-of select="AllocatedQty" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SellAmount>

					<FXRate>
						<xsl:choose>
							<xsl:when test="contains(Asset,'FX')">
								<xsl:choose>
									<xsl:when test="Asset='FX'">
										<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>					

						<xsl:otherwise>
							<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
									<xsl:value-of select="FXRate_Taxlot"/>
								</xsl:when>
								 <!-- <xsl:when test="number(ForexRate)">  -->
									<!-- <xsl:value-of select="ForexRate"/>  -->
								<!-- </xsl:when>  -->

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
						</xsl:choose>
						
					</FXRate>

          <Description>
            <xsl:value-of select ="FullSecurityName"/>
          </Description>


          <Symbol>
            <xsl:choose>
              <xsl:when test ="Asset = 'EquityOption'">
                <xsl:value-of select ="OSIOptionSymbol"/>
              </xsl:when>
            
              <xsl:otherwise>
                <xsl:value-of select ="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>            
          </Symbol>



          <CUSIP>            
           <xsl:value-of select="CUSIP"/>
          </CUSIP>




          <ISIN>            
           <xsl:value-of select="ISIN"/>
          </ISIN>


          <Interest>
            <xsl:value-of select ="AccruedInterest"/>
          </Interest>
		  
					
					
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
		  
		   <Direction>
            <xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="PutOrCall"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
          </Direction>
		  
		  <NewCancelAmend>
		  <xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended'">
								<xsl:value-of select="'A'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
		  </NewCancelAmend>
		  
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
					
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
			
		</ThirdPartyFlatFileDetailCollection>
		
	</xsl:template>

</xsl:stylesheet>
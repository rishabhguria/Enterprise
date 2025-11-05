<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<!--for system internal use-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<FileHeader>
					<xsl:value-of select ="'true'"/>
				</FileHeader>

				<FileFooter>
					<xsl:value-of select ="'false'"/>
				</FileFooter>

				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<RecordAction>
					<xsl:value-of select="'RecordAction'"/>
				</RecordAction>

				<RecordType>
					<xsl:value-of select="'RecordType'"/>
				</RecordType>

				<Portfolio>
					<xsl:value-of select="'Portfolio'"/>
				</Portfolio>

				<Investment>
					<xsl:value-of select="'Investment'"/>
				</Investment>

				<LocationAccount>
					<xsl:value-of select="'LocationAccount'"/>
				</LocationAccount>

				<Strategy>
					<xsl:value-of select="'Strategy'"/>
				</Strategy>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<EventDate>
					<xsl:value-of select="'EventDate'"/>
				</EventDate>

				<SettleDate>
					<xsl:value-of select="'SettleDate'"/>
				</SettleDate>

				<ActualSettleDate>
					<xsl:value-of select="'ActualSettleDate'"/>
				</ActualSettleDate>

				<SecFeeAmount>
					<xsl:value-of select="'SecFeeAmount'"/>
				</SecFeeAmount>

				<NetCounterAmount>
					<xsl:value-of select="'NetCounterAmount'"/>
				</NetCounterAmount>

				<NetInvestmentAmount>
					<xsl:value-of select="'NetInvestmentAmount'"/>
				</NetInvestmentAmount>

				<TotCommission>
					<xsl:value-of select="'TotCommission'"/>
				</TotCommission>

				<UserTranId1>
					<xsl:value-of select="'UserTranId1'"/>
				</UserTranId1>

				<PriceDenomination>
					<xsl:value-of select="'PriceDenomination'"/>
				</PriceDenomination>

				<CounterInvestment>
					<xsl:value-of select="'CounterInvestment'"/>
				</CounterInvestment>

				<CounterFXDenomination>
					<xsl:value-of select="'CounterFXDenomination'"/>
				</CounterFXDenomination>

				<TradeFX>
					<xsl:value-of select="'TradeFX'"/>
				</TradeFX>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SecurityType>
					<xsl:value-of select="'Security Type'"/>
				</SecurityType>

				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>

				<OrfFee>
					<xsl:value-of select="'OrfFee'"/>
				</OrfFee>

				<BuyCurrency>
					<xsl:value-of select="'Buy Currency'"/>
				</BuyCurrency>

				<SellCurrency>
					<xsl:value-of select ="'Sell Currency'"/>
				</SellCurrency>

				<BuyQuantity>
					<xsl:value-of select="'Buy Quantity'" />
				</BuyQuantity>

				<SellQuantity>
					<xsl:value-of select ="'Sell Quantity'"/>
				</SellQuantity>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[TransactionType!='LongWithdrawal' and TransactionType!='ShortAddition' and TransactionType!='ShortWithdrawal' and TransactionType!='LongAddition']">

				<ThirdPartyFlatFileDetail>

					<!--for system internal use-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<FileHeader>
						<xsl:value-of select ="'false'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select ="'false'"/>
					</FileFooter>

					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name ="PB_NAME">
						<xsl:value-of select="GS"/>
					</xsl:variable>

					<xsl:variable name="Prana_Fund_Name">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name="PRANA_MasterFund_Name">
						<xsl:value-of select="document('../ReconMappingXml/MasterFundMapping.xml')/MasterFundMapping/PB[@Name= 'GS']/MasterFundData[@FundName=$Prana_Fund_Name]/@MasterFundName"/>
					</xsl:variable>

					<xsl:variable name="PB_Fund_Name">
					<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$Prana_Fund_Name]/@PranaFund"/>
					</xsl:variable>

					<RecordAction>
						<xsl:value-of select="'New'"/>
					</RecordAction>

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

					<RecordType>
						<xsl:value-of select="$varTransactionType"/>
					</RecordType>




					<Portfolio>
						<xsl:choose>
							<xsl:when test ="AccountName = 'Maple Rock MF: GS'">

								<xsl:value-of select="'GS'"/>

							</xsl:when>

							<xsl:when test ="AccountName = 'Maple Rock MF: UBS'">

								<xsl:value-of select="'UBS'"/>

							</xsl:when>

							<xsl:when test="AccountName = 'Maple Rock MF: Scotia'">
								<xsl:value-of select="'Scotia'"/>
							</xsl:when>

							<xsl:when test="AccountName = 'Maple Rock MF: Fidelity'">
								<xsl:value-of select="'FIDL'"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Portfolio>
					
					
					
					
					
					<Investment>
						<xsl:choose>
							<xsl:when test="Asset = 'FixedIncome'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test ="contains(Symbol, '-') != false">
								<xsl:value-of select ="BBCode"/>
							</xsl:when>
							<xsl:when test ="Asset = 'EquityOption'">
								<xsl:value-of select ="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Investment>

					<xsl:variable name ="varFundName">
						<xsl:choose>
							<xsl:when test ="AccountName = 'MS Main'">
								<xsl:value-of select ="'038CDFPK2'"/>
							</xsl:when>
							<xsl:when test ="AccountName = 'MS Swap'">
								<xsl:value-of select ="'06178F8Q5'"/>
							</xsl:when>
							<xsl:when test ="AccountName = 'MS FX Option'">
								<xsl:value-of select ="'058D17U04'"/>
							</xsl:when>
							<xsl:when test ="AccountName = 'MS FX Spot'">
								<xsl:value-of select ="'0581CB0P7'"/>
							</xsl:when>
							<xsl:when test ="AccountName = 'GS Main'">
								<xsl:value-of select ="'002506988'"/>
							</xsl:when>
							<xsl:when test ="AccountName = 'GS FX Option'">
								<xsl:value-of select ="'044455921'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>

					<LocationAccount>
						<xsl:value-of select="concat($PRANA_MasterFund_Name, '_', $varFundName)"/>
					</LocationAccount>

					<Strategy>
						<xsl:value-of select="Strategy"/>
					</Strategy>

					<Quantity>
						<xsl:value-of select="format-number(AllocatedQty,'#.####')"/>
					</Quantity>

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
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'FX')">
								<xsl:value-of select="format-number(AveragePrice,'#.######')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($Price,'#.####')"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:choose>
							<xsl:when test="IsSwapped ='true'">
								<xsl:value-of select="format-number($Price,'#.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
					</xsl:otherwise>
						</xsl:choose>
					</Price>

					<Broker>
						<xsl:value-of select="CounterParty"/>
					</Broker>

					<EventDate>
						<xsl:value-of select="TradeDate"/>
					</EventDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<ActualSettleDate>
						<xsl:value-of select="SettlementDate"/>
					</ActualSettleDate>

					<xsl:variable name="varSettFxSecFee">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
										<xsl:value-of select="StampDuty * FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="StampDuty div FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="StampDuty"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="StampDuty">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="StampDuty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varSettFxSecFee"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>
					
					<SecFeeAmount>
						<xsl:value-of select="format-number($StampDuty,'#.####')"/>
					</SecFeeAmount>

					<NetCounterAmount>
						<xsl:value-of select="''"/>
					</NetCounterAmount>

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

					<NetInvestmentAmount>
						<!--<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="format-number(NetAmount div 100,'#.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(NetAmount,'#.####')"/>
							</xsl:otherwise>						
						</xsl:choose>-->

						<xsl:value-of select="format-number($NetAmount,'#.####')"/>
						
					</NetInvestmentAmount>

					<xsl:variable name="varSettFxCommissionCharged">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
										<xsl:value-of select="CommissionCharged * FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CommissionCharged div FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="CommissionCharged">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varSettFxCommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>

					<TotCommission>
						<xsl:choose>
							<xsl:when test="IsSwapped ='true'">
								<xsl:value-of select="format-number($CommissionCharged,'#.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(CommissionCharged,'#.####')"/>
					</xsl:otherwise>
						</xsl:choose>
					</TotCommission>

					<UserTranId1>
						<xsl:value-of select="TradeRefID"/>
					</UserTranId1>

					<PriceDenomination>
						<xsl:choose>
							<xsl:when test="IsSwapped ='true'">
								
								<xsl:value-of select="'USD'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:otherwise>
						</xsl:choose>

					</PriceDenomination>

					<CounterInvestment>
						<xsl:value-of select="SettlCurrency"/>
					</CounterInvestment>

					<CounterFXDenomination>
						<xsl:value-of select="SettlCurrency"/>
					</CounterFXDenomination>

					<xsl:variable name="FxRate">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>
					</xsl:variable>

				

					<TradeFX>

<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select ='1'/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Asset='FX'">
										<xsl:value-of select="format-number(AveragePrice,'#.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($FxRate,'#.####')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>			
						
</TradeFX>

					<CUSIP>
						<xsl:value-of select="CUSIP"/>
					</CUSIP>

					<SecurityType>
						<xsl:choose>
							<xsl:when test ="IsSwapped='true' and Asset='Equity'">
								<xsl:value-of select="'SWAP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityType>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<xsl:variable name="varSettFxOrfFee">
						<xsl:choose>
							<xsl:when test="SettlCurrency != CurrencySymbol">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator_Trade ='M'">
										<xsl:value-of select="OrfFee * FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="OrfFee div FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="OrfFee"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="OrfFee">
						<xsl:choose>
							<xsl:when test="SettlCurrency = CurrencySymbol">
								<xsl:value-of select="OrfFee"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varSettFxOrfFee"/>
							</xsl:otherwise>
						</xsl:choose>


					</xsl:variable>


						<OrfFee>
						<xsl:value-of select="$OrfFee"/>
					</OrfFee>

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

					<BuyQuantity>
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
					</BuyQuantity>

					<SellQuantity>
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
					</SellQuantity>

					

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

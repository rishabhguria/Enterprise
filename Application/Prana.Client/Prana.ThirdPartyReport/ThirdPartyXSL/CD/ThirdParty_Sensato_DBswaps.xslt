<?xml version="1.0" encoding="UTF-8"?>

								<!--
								 Description -		ThirdParty file for DB Swaps
								 Date Modified-     30-01-2012
								-->

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



				
				<stratName>
					<xsl:value-of select ="'stratName'"/>
				</stratName>

				<tradeType>
					<xsl:value-of select ="'tradeType'"/>
				</tradeType>

				<execBroker>
					<xsl:value-of select ="'execBroker'"/>
				</execBroker>

				<primeAcct>
					<xsl:value-of select ="'primeAcct'"/>
				</primeAcct>

				<TradeCcy>
					<xsl:value-of select ="'TradeCcy'"/>
				</TradeCcy>

				<SettleCcy>
					<xsl:value-of select="'SettleCcy'"/>
				</SettleCcy>

				<Sedol>
					<xsl:value-of select ="'Sedol'"/>
				</Sedol>

				<ticker>
					<xsl:value-of select ="'ticker'"/>
				</ticker>

				<SensatoNetQty>
					<xsl:value-of select ="'SensatoNetQty'"/>
				</SensatoNetQty>

				<grossPriceLocal>
					<xsl:value-of select ="'grossPriceLocal'"/>
				</grossPriceLocal>

				<grossAmountLocal>
					<xsl:value-of select="'grossAmountLocal'"/>
				</grossAmountLocal>

				<ProrataCommissionLocal>
					<xsl:value-of select ="'ProrataCommissionLocal'"/>
				</ProrataCommissionLocal>

				<ProrataFeesLocal>
					<xsl:value-of select ="'ProrataFeesLocal'"/>
				</ProrataFeesLocal>

				<ProrataCommFeesLocal>
					<xsl:value-of select ="'ProrataCommFeesLocal'"/>
				</ProrataCommFeesLocal>

				<netPriceLocal>
					<xsl:value-of select ="'netPriceLocal'"/>
				</netPriceLocal>

				<netAmountLocal>
					<xsl:value-of select ="'netAmountLocal'"/>
				</netAmountLocal>

				<CommFeesRate>
					<xsl:value-of select ="'CommFeesRate'"/>
				</CommFeesRate>

				<swapFXrate>
					<xsl:value-of select ="'swapFXrate'"/>
				</swapFXrate>

				<SwapFXrateReciprocal>
					<xsl:value-of select ="'SwapFXrateReciprocal'"/>
				</SwapFXrateReciprocal>

				<grossPriceUSD>
					<xsl:value-of select ="'grossPriceUSD'"/>
				</grossPriceUSD>

				<grossAmountUSD>
					<xsl:value-of select ="'grossAmountUSD'"/>
				</grossAmountUSD>

				<ProrataCommissionUSD>
					<xsl:value-of select ="'ProrataCommissionUSD'"/>
				</ProrataCommissionUSD>


				<ProrataFeesUSD>
					<xsl:value-of select ="'ProrataFeesUSD'"/>
				</ProrataFeesUSD>

				<ProrataCommFeesUSD>
					<xsl:value-of select ="'ProrataCommFeesUSD'"/>
				</ProrataCommFeesUSD>

				<netPriceUSD>
					<xsl:value-of select ="'netPriceUSD'"/>
				</netPriceUSD>

				<netAmountUSD>
					<xsl:value-of select ="'netAmountUSD'"/>
				</netAmountUSD>
			
				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<!--<xsl:if test ="Asset!='Equity'">-->
				<!--<xsl:if test ="FundName ='SAPDB_441885' or FundName ='466055' or FundName ='S1DB_32577'  or FundName ='465568' or FundName ='465569'or FundName ='465572' or FundName ='465573'or FundName ='465574' or FundName ='465575' or FundName ='465576' or FundName ='465577' ">-->
				<xsl:if test ="AccountName ='SAPDB_SWAP' or AccountName ='466055' or AccountName ='S1DB_SWAP'  or AccountName ='SP29DB_SWAP_TWD' or AccountName ='SP29DB_SWAP_AUD'or AccountName ='465572' or AccountName ='465573'or AccountName ='SP29DB_SWAP_MYR' or AccountName ='SP29DB_SWAP_IDR' or AccountName ='465576' or AccountName ='SP29DB_SWAP_KRW'or AccountName ='S2DB_SWAPAUD' or AccountName ='S2DB_SWAPIDR' or AccountName ='S2DB_SWAPINR' or AccountName ='S2DB_SWAPKRW (L)' or AccountName ='S2DB_SWAPKRW (S)' or AccountName ='S2DB_SWAPMYR' or AccountName ='S2DB_SWAPPHP' or AccountName ='S2DB_SWAPTWD' or AccountName ='S2DB_SWAPHKD' ">
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
							<xsl:value-of select ="TaxLotState"/>
						</TaxLotState>


						<xsl:choose>
							<xsl:when test="AccountName='SAPDB_SWAP'">
								<stratName>
									<xsl:value-of select="'strat_asia_live'"/>
								</stratName>
							</xsl:when>
							<xsl:when test="AccountName='S1DB_SWAP'">
								<stratName>
									<xsl:value-of select="'strat_s1'"/>
								</stratName>
							</xsl:when>
							<!--<xsl:when test="FundName ='466055' or FundName ='465568' or FundName ='465569'or FundName ='465572' or FundName ='465573'or FundName ='465574' or FundName ='465575' or FundName ='465576' or FundName ='465577' ">-->
								<xsl:when test="AccountName ='466055' or AccountName ='SP29DB_SWAP_TWD' or AccountName ='SP29DB_SWAP_AUD'or AccountName ='465572' or AccountName ='465573'or AccountName ='SP29DB_SWAP_MYR' or AccountName ='SP29DB_SWAP_IDR' or AccountName ='465576' or AccountName ='SP29DB_SWAP_KRW' ">
								<stratName>
									<xsl:value-of select="'strat_s29'"/>
								</stratName>
							</xsl:when>

							<xsl:when test="AccountName ='S2DB_SWAPAUD' or AccountName ='S2DB_SWAPIDR' or AccountName ='S2DB_SWAPINR' or AccountName ='S2DB_SWAPKRW (L)' or AccountName ='S2DB_SWAPKRW (S)' or AccountName ='S2DB_SWAPMYR' or AccountName ='S2DB_SWAPPHP' or AccountName ='S2DB_SWAPTWD' or AccountName ='S2DB_SWAPHKD'">
								<stratName>
									<xsl:value-of select="'strat_s2'"/>
								</stratName>
							</xsl:when>
							
							<xsl:otherwise>
								<stratName>
									<xsl:value-of select="'none'"/>
								</stratName>
							</xsl:otherwise>
						</xsl:choose>
						
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<tradeType>
									<xsl:value-of select ="'BL'"/>
								</tradeType>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<tradeType>
									<xsl:value-of select ="'BC'"/>
								</tradeType>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side = 'Sell to Open'">
								<tradeType>
									<xsl:value-of select ="'SS'"/>
								</tradeType>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side= 'Sell to Close'">
								<tradeType>
									<xsl:value-of select ="'SL'"/>
								</tradeType>
							</xsl:when>
							<xsl:otherwise>
								<tradeType>
									<xsl:value-of select ="Side"/>
								</tradeType>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="CounterParty='CSElect' or CounterParty='CSPrg'">
								<execBroker>
									<xsl:value-of select="'CSELECT'"/>
								</execBroker>
							</xsl:when>
							
							<xsl:otherwise>
								<execBroker>
									<xsl:value-of select="CounterParty"/>
								</execBroker>
							</xsl:otherwise>
							
						</xsl:choose>	
						
						
						<!--<execBroker>
							<xsl:value-of select ="CounterParty"/>
						</execBroker>-->
						
						<primeAcct>
							<xsl:value-of select ="ThirdParty"/>
						</primeAcct>

						<TradeCcy>
							<xsl:value-of select ="CurrencySymbol"/>
						</TradeCcy>

						<SettleCcy>
							<xsl:value-of select="'USD'"/>
						</SettleCcy>

						<Sedol>
							<xsl:value-of select ="SEDOL"/>
						</Sedol>

						<ticker>
							<xsl:value-of select ="BBCode"/>
						</ticker>


						<xsl:variable name ="varSideMultiplier">
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close'">
									<xsl:value-of select ="1"/>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell short' or Side='Sell to Open' or Side= 'Sell to Close'">
									<xsl:value-of select ="-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="varNetQty">
							<xsl:value-of select ="AllocatedQty * $varSideMultiplier"/>
						</xsl:variable>

						<SensatoNetQty>
							<xsl:value-of select ="$varNetQty "/>
						</SensatoNetQty>
						
						<grossPriceLocal>
							<xsl:value-of select="AveragePrice"/>
						</grossPriceLocal>
						
						<!--<xsl:choose>
							<xsl:when test ="number(AveragePrice)">
								<grossPriceLocal>
									<xsl:value-of select="AveragePrice"/>
								</grossPriceLocal>		
							</xsl:when>
							<xsl:otherwise>
								<grossPriceLocal>
									<xsl:value-of select="0"/>
								</grossPriceLocal>
							</xsl:otherwise>
						</xsl:choose>-->


						<xsl:choose>
							<xsl:when test ="number(GrossAmount)">
								<grossAmountLocal>
									<xsl:value-of select ="GrossAmount"/>
								</grossAmountLocal>
							</xsl:when>
							<xsl:otherwise>
								<grossAmountLocal>
									<xsl:value-of select ="0"/>
								</grossAmountLocal>
							</xsl:otherwise>
						</xsl:choose>
						
						
						<ProrataCommissionLocal>
							<xsl:value-of select ="CommissionCharged"/>
						</ProrataCommissionLocal>

						<xsl:variable name="varFees">
							<xsl:value-of select="MiscFees + ClearingFee + SecFees +OtherBrokerFee + StampDuty + TransactionLevy + TaxOnCommissions"/>
						</xsl:variable>

						<ProrataFeesLocal>
							<xsl:value-of select ="$varFees"/>
						</ProrataFeesLocal>

						<xsl:variable name="varCommFees">							
								<xsl:value-of select="CommissionCharged + $varFees"/>							
						</xsl:variable>
						<ProrataCommFeesLocal>
							<xsl:value-of select ="$varCommFees"/>
						</ProrataCommFeesLocal>

						<xsl:variable name="varNetPrice">
							<xsl:choose>
								<xsl:when test ="number(AllocatedQty) and AllocatedQty != 0 and number(NetAmount) and NetAmount != 0">
									<xsl:value-of select="NetAmount div AllocatedQty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>													
						</xsl:variable>

						<netPriceLocal>
							<xsl:value-of select ="$varNetPrice"/>
						</netPriceLocal>

						<xsl:choose>
							<xsl:when test ="number(NetAmount)">
								<netAmountLocal>
									<xsl:value-of select ="NetAmount * $varSideMultiplier"/>
								</netAmountLocal>
							</xsl:when>
							<xsl:otherwise>
								<netAmountLocal>
									<xsl:value-of select ="0"/>
								</netAmountLocal>
							</xsl:otherwise>
						</xsl:choose>
						

						<xsl:variable name="varCommFeeRate">
							<xsl:choose>
								<xsl:when test ="number(GrossAmount) and GrossAmount !=0">
									<xsl:value-of select="$varCommFees div GrossAmount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
								
						</xsl:variable>

						<CommFeesRate>
							<xsl:value-of select ="$varCommFeeRate"/>
						</CommFeesRate>

						<swapFXrate>
							<xsl:value-of select ="FXRate_Taxlot"/>
						</swapFXrate>

						<xsl:choose>
							<xsl:when test="FXRate_Taxlot != 0">
								<SwapFXrateReciprocal>
									<xsl:value-of select ="1 div FXRate_Taxlot "/>
								</SwapFXrateReciprocal>
							</xsl:when>
							<xsl:otherwise>
								<SwapFXrateReciprocal>
									<xsl:value-of select ="0"/>
								</SwapFXrateReciprocal>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="vargrossPriceUSD">
							<xsl:choose>
								<xsl:when test ="number(AveragePrice)">
									<xsl:value-of select="FXRate_Taxlot * AveragePrice"/>		
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>							
						</xsl:variable>

						<grossPriceUSD>
							<xsl:value-of select ="$vargrossPriceUSD"/>
						</grossPriceUSD>

						<xsl:choose>
							<xsl:when test ="number(GrossAmount)">
								<grossAmountUSD>
									<xsl:value-of select ="GrossAmount * FXRate_Taxlot * $varSideMultiplier"/>
								</grossAmountUSD>
							</xsl:when>
							<xsl:otherwise>
								<grossAmountUSD>
									<xsl:value-of select ="0"/>
								</grossAmountUSD>
							</xsl:otherwise>
						</xsl:choose>
						

						<ProrataCommissionUSD>
							<xsl:value-of select ="CommissionCharged * FXRate_Taxlot"/>
						</ProrataCommissionUSD>

						<ProrataFeesUSD>
							<xsl:value-of select ="FXRate_Taxlot * $varFees"/>
						</ProrataFeesUSD>

						<ProrataCommFeesUSD>
							<xsl:value-of select ="FXRate_Taxlot* $varCommFees"/>
						</ProrataCommFeesUSD>

						<netPriceUSD>
							<xsl:value-of select ="FXRate_Taxlot * $varNetPrice"/>
						</netPriceUSD>

						<xsl:choose>
							<xsl:when test ="number(NetAmount)">
								<netAmountUSD>
									<xsl:value-of select ="FXRate_Taxlot * NetAmount * $varSideMultiplier"/>
								</netAmountUSD>
							</xsl:when>
							<xsl:otherwise>
								<netAmountUSD>
									<xsl:value-of select ="0"/>
								</netAmountUSD>
							</xsl:otherwise>
						</xsl:choose>

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

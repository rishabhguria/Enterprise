<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

					<!-- this field use internal purpose-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<TradeNumber>
						<xsl:value-of select =" TradeRefID"/>
					</TradeNumber>

					<!--Allocation Number or number of allocations-->
					<xsl:choose>
						<xsl:when test ="Asset='FX'">
							<!--change this number if testing FX-->
							<AllocationNumber>
								<xsl:value-of select ="'0'"/>
							</AllocationNumber>
						</xsl:when >
						<xsl:otherwise>
							<AllocationNumber>
								<xsl:value-of select ="'0'"/>
							</AllocationNumber>
						</xsl:otherwise>
					</xsl:choose >

					<!--define security type-->
					<xsl:choose>
						<xsl:when test="Asset ='EquityOption' ">
							<SecurityType>
								<xsl:value-of select ="'OPTION'"/>
							</SecurityType>
						</xsl:when>
						<xsl:when test="Asset ='Futures' ">
							<SecurityType>
								<xsl:value-of select ="'FUTURE'"/>
							</SecurityType>
						</xsl:when>
						<xsl:when test="Asset ='Equity' ">
							<SecurityType>
								<xsl:value-of select ="'STOCK'"/>
							</SecurityType>
						</xsl:when>
						<xsl:otherwise>
							<ActionCode>
								<xsl:value-of select="''"/>
							</ActionCode>
						</xsl:otherwise>
					</xsl:choose>

					<!--Define the prefix for side based on Tax lot state-->
					<xsl:variable name = "var_Cancel_Correct_Identifier" >
						<xsl:choose>
							<xsl:when test ="TaxLotState='Amended'">
								<xsl:value-of select ="'C'"/>
							</xsl:when >
							<xsl:when test ="TaxLotState='Deleted'">
								<xsl:value-of select ="'X'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<!--Side Identifier-->
					<xsl:choose>
						
						<xsl:when test="Asset = 'Equity' and Side='Buy to Open' or Side='Buy' ">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'BUY')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>

						<xsl:when test="Asset = 'EquityOption' and Side='Buy to Open' or Side='Buy' ">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'OP')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>

						<xsl:when test=" Asset = 'Equity' and Side='Buy to Cover' or Side='Buy to Close' ">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'BC')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>

						<xsl:when test="Asset = 'EquityOption' and  Side='Buy to Cover' or Side='Buy to Close' ">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'CP')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>

						<!--<xsl:when test="Side='Buy to Close' and Asset = 'Equity'">
              <Buy_Sell_Sht_Cvr>
                <xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'BC')"/>
              </Buy_Sell_Sht_Cvr>
            </xsl:when>

            <xsl:when test="Side='Buy to Close' and Asset = 'EquityOption'">
              <Buy_Sell_Sht_Cvr>
                <xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'CP')"/>
              </Buy_Sell_Sht_Cvr>
            </xsl:when>-->

						<xsl:when test="Asset = 'EquityOption' and Side='Sell' or Side='Sell to Close' ">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'CS')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
						<xsl:when test="Asset = 'Equity' and Side='Sell' or Side='Sell to Close'  ">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'SELL')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
						<xsl:when test="Side='Sell to Open'">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'OS')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
						
						<xsl:when test="Side='Sell short'  ">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="concat ($var_Cancel_Correct_Identifier, 'SS')"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
					
					


						<xsl:otherwise>
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select="Side"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:otherwise>
					</xsl:choose>

					<Counterparty>
						<xsl:value-of select ="CounterParty"/>
					</Counterparty>

					<AccountNumber>
						<xsl:value-of select="'NEWLAND'"/>
					</AccountNumber>

					<xsl:choose>

						<xsl:when test="Asset = 'Equity' and Side='Buy to Cover' or Side='Buy to Close' or Side='Sell short' or Side='Sell to Open'">
							<AccountType>
								<xsl:value-of select="'SHORT'"/>
							</AccountType>
						</xsl:when>
						<xsl:otherwise>
							<AccountType>
								<xsl:value-of select="'MARGIN'"/>
							</AccountType>
						</xsl:otherwise>
					</xsl:choose>

	

					<ISIN>
						<xsl:value-of select ="ISIN"/>
					</ISIN>

					<SEDOL>
						<xsl:value-of select ="SEDOL"/>
					</SEDOL>

					<QUIK>
						<xsl:value-of select ="''"/>
					</QUIK>

					<CUSIP>
						<xsl:value-of select ="CUSIP"/>
					</CUSIP>

					<xsl:choose>
						<xsl:when test="Asset = 'EquityOption' ">
							<Ticker>
								<xsl:value-of select ="translate(Symbol, ' ', '+')"/>
							</Ticker>
						</xsl:when>
						<xsl:otherwise>
							<Ticker>
								<xsl:value-of select ="Symbol"/>
							</Ticker>
						</xsl:otherwise>
					</xsl:choose>

					<!--<Ticker>
            <xsl:value-of select ="Symbol"/>
          </Ticker>-->


					<LocalCurrencyCode>
						<xsl:value-of select ="CurrencySymbol"/>
					</LocalCurrencyCode>

					<!-- company base currency-->
					<SettleCurrency>
						<xsl:value-of select ="'USD'"/>
					</SettleCurrency>
					<!--Mapping required, FX Rate = 1 for USD local, empty for international-->
					<xsl:choose>
						<xsl:when test="CurrencySymbol = 'USD'">
							<FXRate>
								<xsl:value-of select ="'1'"/>
							</FXRate>
						</xsl:when>
						<xsl:otherwise>
							<FXRate>
								<xsl:value-of select ="''"/>
							</FXRate>
						</xsl:otherwise>
					</xsl:choose>

					<PricingFactor>
						<xsl:value-of select ="''"/>
					</PricingFactor>

					<TradingFactor>
						<xsl:value-of select ="''"/>
					</TradingFactor>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<PriceLocal>
						<xsl:value-of select="AveragePrice"/>
					</PriceLocal>

					<AccruedInterestLocal>
						<xsl:value-of select ="''"/>
					</AccruedInterestLocal>

					<PrincipalLocal>
						<xsl:value-of select ="''"/>
					</PrincipalLocal>

					<!-- only commission and taxes on commission-->
					<CommissionLocal>
						<xsl:value-of select="CommissionCharged  + TaxOnCommissions"/>
					</CommissionLocal>

					<FeesLocal>
						<xsl:value-of select ="OtherBrokerFee + MiscFees"/>
					</FeesLocal>

					<ChargesLocal>
						<xsl:value-of select ="ClearingFee "/>
					</ChargesLocal>

					<LeviesLocal>
						<xsl:value-of select ="TransactionLevy "/>
					</LeviesLocal>

					<SEC_Fees_Local>
						<xsl:value-of select ="''"/>
					</SEC_Fees_Local>

					<NetProceeds_Local>
						<xsl:value-of select ="''"/>
					</NetProceeds_Local>

					<PriceSettlement>
						<xsl:value-of select ="''"/>
					</PriceSettlement>

					<AccruedINterestSettlement>
						<xsl:value-of select ="''"/>
					</AccruedINterestSettlement>

					<PrincipalSettlement>
						<xsl:value-of select ="''"/>
					</PrincipalSettlement>

					<CommissionSettlement>
						<xsl:value-of select ="''"/>
					</CommissionSettlement>

					<Fees_Settlement>
						<xsl:value-of select ="''"/>
					</Fees_Settlement>

					<Charges_Settlement>
						<xsl:value-of select ="''"/>
					</Charges_Settlement>

					<Levies_Settlement>
						<xsl:value-of select ="''"/>
					</Levies_Settlement>

					<SEC_Fees__Settlement>
						<xsl:value-of select ="''"/>
					</SEC_Fees__Settlement>

					<NetProceeds__Settlement>
						<xsl:value-of select ="''"/>
					</NetProceeds__Settlement>

					<xsl:variable name = "varTradeMth" >
						<xsl:value-of select="substring(TradeDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeDay" >
						<xsl:value-of select="substring(TradeDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeYR" >
						<xsl:value-of select="substring(TradeDate,7,4)"/>
					</xsl:variable>
					<TradeDate>
						<xsl:value-of select="concat($varTradeYR,'',$varTradeMth,'',$varTradeDay)"/>
					</TradeDate>


					<xsl:variable name = "varSettleMth" >
						<xsl:value-of select="substring(SettlementDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleDay" >
						<xsl:value-of select="substring(SettlementDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleYR" >
						<xsl:value-of select="substring(SettlementDate,7,4)"/>
					</xsl:variable>
					<SettlementDate>
						<xsl:value-of select="concat($varSettleYR,'',$varSettleMth,'',$varSettleDay)"/>
					</SettlementDate>

					<Reserved>
						<xsl:value-of select ="''"/>
					</Reserved>

					<Comments>
						<xsl:value-of select ="''"/>
					</Comments>
					<Yield>
						<xsl:value-of select ="''"/>
					</Yield>
					<ClearingBroker>
						<xsl:value-of select ="''"/>
					</ClearingBroker>
					<Strategy>
						<xsl:value-of select ="''"/>
					</Strategy>

					<ClearingLocation>
						<xsl:value-of select ="''"/>
					</ClearingLocation>



					<ProductDescription>
						<xsl:value-of select ="FullSecurityName"/>
					</ProductDescription>

					<Delivery_Instruction>
						<xsl:value-of select ="''"/>
					</Delivery_Instruction>

					<Settlement_Instruction>
						<xsl:value-of select ="''"/>
					</Settlement_Instruction>


					<Off_Date_Delivery_date>
						<xsl:value-of select ="''"/>
					</Off_Date_Delivery_date>

					<Rate>
						<xsl:value-of select ="''"/>
					</Rate>

					<Rerate_FixDate>
						<xsl:value-of select ="''"/>
					</Rerate_FixDate>

					<Repo_Basis>
						<xsl:value-of select ="''"/>
					</Repo_Basis>

					<PutCall_Type>
						<xsl:value-of select ="PutOrCall"/>
					</PutCall_Type>

					<Option_Contract_Type>
						<xsl:value-of select ="''"/>
					</Option_Contract_Type>


					<Expiry_Time>
						<xsl:value-of select ="''"/>
					</Expiry_Time>

					<Expiry_Location>
						<xsl:value-of select ="''"/>
					</Expiry_Location>

					<FX_Option_CCY1>
						<xsl:value-of select ="''"/>
					</FX_Option_CCY1>

					<FX_Option_CCY2>
						<xsl:value-of select ="''"/>
					</FX_Option_CCY2>

					<NotionalAmount_CCY1>
						<xsl:value-of select ="''"/>
					</NotionalAmount_CCY1>

					<NotionalAmount_CCY2>
						<xsl:value-of select ="''"/>
					</NotionalAmount_CCY2>

					<Payout_Amount>
						<xsl:value-of select ="''"/>
					</Payout_Amount>

					<Payout_CCY>
						<xsl:value-of select ="''"/>
					</Payout_CCY>

					<Rebate_Quote_Mode>
						<xsl:value-of select ="''"/>
					</Rebate_Quote_Mode>

					<Rebate_1>
						<xsl:value-of select ="''"/>
					</Rebate_1>

					<Barrier_1>
						<xsl:value-of select ="''"/>
					</Barrier_1>

					<Barrier_1_StartDate>
						<xsl:value-of select ="''"/>
					</Barrier_1_StartDate>


					<Barrier_1_EndDate>
						<xsl:value-of select ="''"/>
					</Barrier_1_EndDate>

					<Rebate_2>
						<xsl:value-of select ="''"/>
					</Rebate_2>

					<Barrier_2>
						<xsl:value-of select ="''"/>
					</Barrier_2>


					<Barrier_2_StartDate>
						<xsl:value-of select ="''"/>
					</Barrier_2_StartDate>


					<Barrier_2_EndDate>
						<xsl:value-of select ="''"/>
					</Barrier_2_EndDate>


					<Far_Amount>
						<xsl:value-of select ="''"/>
					</Far_Amount>

					<Barrier_Direction>
						<xsl:value-of select ="''"/>
					</Barrier_Direction>

					<Link_Indicator>
						<xsl:value-of select ="''"/>
					</Link_Indicator>

					<!-- Order Status-->
					<!--
					<xsl:choose>
						<xsl:when test ="TaxLotState='Amended'">
							<OrdStatus>
								<xsl:value-of select ="'R'"/>
							</OrdStatus>
						</xsl:when >
						<xsl:when test ="TaxLotState='Deleted'">
							<OrdStatus>
								<xsl:value-of select ="'D'"/>
							</OrdStatus>
						</xsl:when>
						<xsl:otherwise>
							<OrdStatus>
								<xsl:value-of select ="'N'"/>
							</OrdStatus>
						</xsl:otherwise>
					</xsl:choose >

					-->
					<!-- Exec Trans Type-->
					<!--
					<xsl:choose>
						<xsl:when test ="TaxLotState='Amended' or TaxLotState='Deleted'">
							<ExecTransType>
								<xsl:value-of select ="'0'"/>
							</ExecTransType>
						</xsl:when >
						<xsl:otherwise>
							<ExecTransType>
								<xsl:value-of select ="'2'"/>
							</ExecTransType>
						</xsl:otherwise>
					</xsl:choose >-->


					<!--<Put_Call>
						<xsl:value-of select ="PutOrCall"/>
					</Put_Call>-->

					<!-- this is also for internal purpose-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

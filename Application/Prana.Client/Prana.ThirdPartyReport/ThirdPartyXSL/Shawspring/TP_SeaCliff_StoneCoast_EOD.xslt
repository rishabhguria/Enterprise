<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			


			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

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

					<xsl:variable name ="varExpirationDate">
						<xsl:if test ="Asset = 'EquityOption' or Asset = 'FutureOption' or Asset = 'Future' or Asset = 'FixedIncome'">
							<xsl:value-of select ="ExpirationDate"/>
						</xsl:if>
					</xsl:variable>


					<xsl:variable name="PB_NAME" select="'EOD'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<Client_Acct_No>
						<xsl:value-of select="AccountNo"/>
					</Client_Acct_No>

					<Fund_Account>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</Fund_Account>

					<Ticker>
						<xsl:choose>
							<xsl:when test="Asset='Equity'and CurrencySymbol!='USD'">
								<xsl:value-of select="concat(Symbol,'.',substring(Exchange,1,2))"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='PrivateEquity'and CurrencySymbol='USD'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>

							<xsl:when test="Asset='Future'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>

							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Ticker>


					<Cusip>
						<xsl:value-of select="CUSIP"/>
					</Cusip>
					
					<Sedol>
						<xsl:value-of select="SEDOL"/>
					</Sedol>

					<Isin>
						<xsl:value-of select="ISIN"/>
					</Isin>

					<Bloomberg_Global_ID>
						<xsl:value-of select="''"/>
					</Bloomberg_Global_ID>

					<Bloomberg_Yellow_Key>
						<xsl:value-of select="Asset"/>
					</Bloomberg_Yellow_Key>

					<Description>
						<xsl:value-of select="translate(FullSecurityName,',','')"/>
					</Description>


					<Tran_Type>
						<xsl:choose>
							<xsl:when test="Asset='Future'">
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'BCOV'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SSEL'"/>
									</xsl:when>

									<!--<xsl:when test="Side='Buy' and TaxLotState='Amended'">
										<xsl:value-of select="'AMENDMENTBUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell' and TaxLotState='Amended'">
										<xsl:value-of select="'AMENDMENTSELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' and TaxLotState='Amended'">
										<xsl:value-of select="'AMENDMENTCOVER'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short' and TaxLotState='Amended'">
										<xsl:value-of select="'AMENDMENTSell Short'"/>
									</xsl:when>-->

									<xsl:when test="Side='Buy' and TaxLotState='Deleted'">
										<xsl:value-of select="'XBUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell' and TaxLotState='Deleted'">
										<xsl:value-of select="'XSELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' and TaxLotState='Deleted'">
										<xsl:value-of select="'XCOV'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short' and TaxLotState='Deleted'">
										<xsl:value-of select="'XSSEL'"/>
									</xsl:when>

								</xsl:choose>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="(Side='Buy to Open' or Side='Buy') ">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell to Open' ">
										<xsl:value-of select="'SSEL'"/>
									</xsl:when>


									<xsl:when test="Side='Sell to Close' or Side='Sell'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'BCOV'"/>
									</xsl:when>

									<!--<xsl:when test="(Side='Buy to Open' or Side='Buy') and TaxLotState='Amended'">
										<xsl:value-of select="'AMENDMENTBUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell to Open' and TaxLotState='Amended' ">
										<xsl:value-of select="'AMENDMENTSell Short'"/>
									</xsl:when>


									<xsl:when test="Side='Sell to Close' or Side='Sell' and TaxLotState='Amended'">
										<xsl:value-of select="'AMENDMENTSELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' and TaxLotState='Amended'">
										<xsl:value-of select="'AMENDMENTCOVER'"/>
									</xsl:when>-->

									<xsl:when test="(Side='Buy to Open' or Side='Buy') and TaxLotState='Deleted'">
										<xsl:value-of select="'XBUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell to Open' and TaxLotState='Deleted' ">
										<xsl:value-of select="'XSSEL'"/>
									</xsl:when>


									<xsl:when test="Side='Sell to Close' or Side='Sell' and TaxLotState='Deleted'">
										<xsl:value-of select="'XSELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' and TaxLotState='Deleted'">
										<xsl:value-of select="'XCOV'"/>
									</xsl:when>

								</xsl:choose>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
										<xsl:value-of select="'BCOV'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'SSEL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy' and TaxLotState='Amended'">
										<xsl:value-of select="'CORRECTIONBUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell' and TaxLotState='Amended'">
										<xsl:value-of select="'CORRECTIONSELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' and TaxLotState='Amended'">
										<xsl:value-of select="'CORRECTIONCOVER'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short' and TaxLotState='Amended'">
										<xsl:value-of select="'CORRECTIONSell Short'"/>
									</xsl:when>

									<xsl:when test="Side='Buy' and TaxLotState='Deleted'">
										<xsl:value-of select="'CANCELBUY'"/>
									</xsl:when>

									<xsl:when test="Side='Sell' and TaxLotState='Deleted'">
										<xsl:value-of select="'CANCELSELL'"/>
									</xsl:when>

									<xsl:when test="Side='Buy to Close' and TaxLotState='Deleted'">
										<xsl:value-of select="'CANCELCOV'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short' and TaxLotState='Deleted'">
										<xsl:value-of select="'CANCELSSEL Short'"/>
									</xsl:when>

								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						
					</Tran_Type>


					<Broker>
						<xsl:value-of select="CounterParty"/>
					</Broker>

					<Trade_Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Trade_Currency>

					
					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</SettleDate>

					<Quantity_Current_Face>
						<!--<xsl:value-of select="ExecutedQty"/>-->
						<xsl:value-of select="AllocatedQty"/>
					</Quantity_Current_Face>

					<Original_Face>
						<xsl:value-of select="''"/>
					</Original_Face>


					<Local_Price>
						<xsl:value-of select="AveragePrice"/>
					</Local_Price>

					<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>					
						
					<Commission>
						<xsl:value-of select="$varCommission"/>
					</Commission>
					<xsl:variable name="varTradeTax">
						<xsl:value-of select="OtherBrokerFee + MiscFees + OccFee + OrfFee + ClearingBrokerFee + TaxOnCommissions + TransactionLevy  + ClearingFee"/>
					</xsl:variable>
					<Misc_Fee>
						<xsl:value-of select="$varTradeTax"/>
					</Misc_Fee>
					
					<Accrued_Interest>
						<xsl:value-of select ="AccruedInterest"/>
					</Accrued_Interest>

					<SEC_Fees>
						<xsl:value-of select="SecFee + StampDuty"/>
					</SEC_Fees>


					<xsl:variable name ="UDA_Country">
						<xsl:value-of select ="UDACountryName"/>
					</xsl:variable>
					<xsl:variable name ="PB_SettleCurrency">
						<xsl:value-of select="document('../ReconMappingXml/SettlementCurrencyMapping.xml')/SettleCurrencyMapping/PB[@Name='ALL']/SymbolData[@UDACountry=$UDA_Country]/@SettleCurrency"/>
					</xsl:variable>
					<Settle_Currency>
						<xsl:value-of select="$PB_SettleCurrency"/>
					</Settle_Currency>

					<Net_Settle_Amount>
						<xsl:value-of select="NetAmount"/>
					</Net_Settle_Amount>

					<Security_Type>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="'Option'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</Security_Type>

					<New_Issue>
						<xsl:value-of select="''"/>
					</New_Issue>

					<Strategy>
						<xsl:value-of select="Strategy"/>
					</Strategy>

					<Trader>
						<xsl:value-of select="''"/>
					</Trader>

					<Manager>
						<xsl:value-of select="''"/>
					</Manager>

					<Analyst>
						<xsl:value-of select="''"/>
					</Analyst>

					<Expiry_Date>
						<xsl:value-of select="$varExpirationDate"/>
					</Expiry_Date>

					<FX_Rate>
						<xsl:value-of select="FXRate_Taxlot"/>
					</FX_Rate>

					<Put_Call>
						<xsl:value-of select="PutOrCall"/>
					</Put_Call>


					<UnderlyingTicker>
						<xsl:value-of select="UnderlyingSymbol"/>
					</UnderlyingTicker>


					<Strike_Price>
						<xsl:value-of select="StrikePrice"/>
					</Strike_Price>


					<LotRelief>
						<xsl:value-of select="''"/>
					</LotRelief>

					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>

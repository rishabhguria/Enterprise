<?xml version="1.0" encoding="UTF-8"?>
<!--Description -ThirdParty file for Credit Suisse
								 Date Created - 02-07-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

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

				<Clienttradereferencenumber>
					<xsl:value-of select ="'Client trade reference number'"/>
				</Clienttradereferencenumber>

				<TradeSequenceNumber>
					<xsl:value-of select ="'Trade Sequence Number'"/>
				</TradeSequenceNumber>

				<AssetType>
					<xsl:value-of select ="'Asset Type'"/>
				</AssetType>

				<TradeAction>
					<xsl:value-of select ="'Trade Action'"/>
				</TradeAction>

				<CounterPartyorExecutingBroker>
					<xsl:value-of select ="'Counter Party or Executing Broker'"/>
				</CounterPartyorExecutingBroker>

				<AccountNumber>
					<xsl:value-of select ="'Account Number'"/>
				</AccountNumber>

				<AccountType>
					<xsl:value-of select="'Account Type'"/>
				</AccountType>

				<ISIN>
					<xsl:value-of select ="'ISIN'"/>
				</ISIN>

				<SEDOL>
					<xsl:value-of select ="'SEDOL'"/>
				</SEDOL>

				<QUIK>
					<xsl:value-of select ="'QUIK'"/>
				</QUIK>

				<CUSIP>
					<xsl:value-of select ="'CUSIP'"/>
				</CUSIP>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<LocalCurrencyCode>
					<xsl:value-of select ="'Local Currency Code'"/>
				</LocalCurrencyCode>

				<SettlementCurrencyCode>
					<xsl:value-of select ="'Settlement Currency Code'"/>
				</SettlementCurrencyCode>

				<MainFXRate>
					<xsl:value-of select ="'Main FX Rate'"/>
				</MainFXRate>

				<PricingFactor>
					<xsl:value-of select ="'Pricing Factor'"/>
				</PricingFactor>

				<Filler>
					<xsl:value-of select ="'Filler'"/>
				</Filler>

				<Quantity>
					<xsl:value-of select ="'Quantity'"/>
				</Quantity>

				<PriceLocal>
					<xsl:value-of select ="'Price Local'"/>
				</PriceLocal>

				<AccruedInterestLocal>
					<xsl:value-of select ="'Accrued Interest Local'"/>
				</AccruedInterestLocal>

				<PrincipalLocal>
					<xsl:value-of select ="'Principal Local'"/>
				</PrincipalLocal>

				<CommissionLocal>
					<xsl:value-of select ="'Commission Local'"/>
				</CommissionLocal>

				<OtherFeesLocal>
					<xsl:value-of select ="'Other Fees Local'"/>
				</OtherFeesLocal>


				<OtherLocalCharges>
					<xsl:value-of select ="'Other Local Charges'"/>
				</OtherLocalCharges>

				<LeviesLocal>
					<xsl:value-of select ="'Levies Local'"/>
				</LeviesLocal>

				<SECFeesLocal>
					<xsl:value-of select ="'SEC Fees Local'"/>
				</SECFeesLocal>

				<NetProceedsLocal>
					<xsl:value-of select ="'Net Proceeds Local'"/>
				</NetProceedsLocal>


				<SettlementPrice>
					<xsl:value-of select ="'Settlement Price'"/>
				</SettlementPrice>

				<AccruedInterestSettlement>
					<xsl:value-of select ="'Accrued Interest Settlement'"/>
				</AccruedInterestSettlement>

				<PrincipalSettlement>
					<xsl:value-of select ="'Principal Settlement'"/>
				</PrincipalSettlement>

				<CommissionSettlement>
					<xsl:value-of select ="'Commission Settlement'"/>
				</CommissionSettlement>

				<FeesSettlement>
					<xsl:value-of select ="'Fees Settlement'"/>
				</FeesSettlement>

				<ChargesSettlement>
					<xsl:value-of select ="'Charges Settlement'"/>
				</ChargesSettlement>

				<LeviesSettlement>
					<xsl:value-of select ="'Levies Settlement'"/>
				</LeviesSettlement>

				<SECFeesSettlement>
					<xsl:value-of select ="'SEC Fees Settlement'"/>
				</SECFeesSettlement>

				<NetProceedsSettlement>
					<xsl:value-of select ="'Net Proceeds Settlement'"/>
				</NetProceedsSettlement>

				<TradeDate>
					<xsl:value-of select ="'Trade Date'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select ="'Settlement Date'"/>
				</SettlementDate>

				<Reserved>
					<xsl:value-of select ="'Reserved'"/>
				</Reserved>

				<Notes>
					<xsl:value-of select ="'Notes'"/>
				</Notes>

				<Yield>
					<xsl:value-of select ="'Yield'"/>
				</Yield>

				<ClearingBroker>
					<xsl:value-of select ="'Clearing Broker'"/>
				</ClearingBroker>

				<Strategy>
					<xsl:value-of select ="'Strategy'"/>
				</Strategy>

				<ClearingLocation>
					<xsl:value-of select ="'Clearing Location'"/>
				</ClearingLocation>

				<Productdescription>
					<xsl:value-of select ="'Product description'"/>
				</Productdescription>

				<Deliveryinstruction>
					<xsl:value-of select ="'Delivery instruction'"/>
				</Deliveryinstruction>

				<Settlementinstruction>
					<xsl:value-of select ="'Settlement instruction'"/>
				</Settlementinstruction>

				<OffDateDeliverydate>
					<xsl:value-of select ="'Off Date/Delivery date'"/>
				</OffDateDeliverydate>

				<Rate>
					<xsl:value-of select ="'Rate'"/>
				</Rate>

				<RerateFixdate>
					<xsl:value-of select ="'Rerate/Fix date'"/>
				</RerateFixdate>

				<Repobasis>
					<xsl:value-of select ="'Repo basis'"/>
				</Repobasis>

				<PutCalltype>
					<xsl:value-of select ="'PutCall type'"/>
				</PutCalltype>

				<Optioncontracttype>
					<xsl:value-of select ="'Option contract type'"/>
				</Optioncontracttype>

				<Expirytime>
					<xsl:value-of select ="'Expiry time'"/>
				</Expirytime>

				<Expirylocation>
					<xsl:value-of select ="'Expiry location'"/>
				</Expirylocation>

				<FXoptionCCY1>
					<xsl:value-of select ="'FX option CCY1'"/>
				</FXoptionCCY1>

				<FXoptionCCY2>
					<xsl:value-of select ="'FX option CCY2'"/>
				</FXoptionCCY2>

				<Notionalamount1>
					<xsl:value-of select ="'Notional amount1'"/>
				</Notionalamount1>

				<Notionalamount2>
					<xsl:value-of select ="'Notional amount2'"/>
				</Notionalamount2>

				<PayoutAmount>
					<xsl:value-of select ="'Payout Amount'"/>
				</PayoutAmount>

				<PayoutCCY>
					<xsl:value-of select ="'Payout CCY'"/>
				</PayoutCCY>

				<RebateQuoteMode>
					<xsl:value-of select ="'Rebate Quote Mode'"/>
				</RebateQuoteMode>

				<Rebate1>
					<xsl:value-of select ="'Rebate1'"/>
				</Rebate1>

				<Barrier1>
					<xsl:value-of select ="'Barrier1'"/>
				</Barrier1>

				<Barrier1startdate>
					<xsl:value-of select ="'Barrier1 start date'"/>
				</Barrier1startdate>

				<Barrier1enddate>
					<xsl:value-of select ="'Barrier1 end date'"/>
				</Barrier1enddate>

				<Rebate2>
					<xsl:value-of select ="'Rebate2'"/>
				</Rebate2>

				<Barrier2>
					<xsl:value-of select ="'Barrier2'"/>
				</Barrier2>

				<Barrier2startdate>
					<xsl:value-of select ="'Barrier2 start date'"/>
				</Barrier2startdate>

				<Barrier2enddate>
					<xsl:value-of select ="'Barrier2 end date'"/>
				</Barrier2enddate>

				<Faramount>
					<xsl:value-of select ="'Far amount'"/>
				</Faramount>

				<Barrierdirection>
					<xsl:value-of select ="'Barrier direction'"/>
				</Barrierdirection>

				<Linkindicator>
					<xsl:value-of select ="'Link indicator'"/>
				</Linkindicator>-->

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test ="AccountName = 'Tracker Account' or (AccountName = 'SSARIS' and Asset != 'Future' and Asset != 'FutureOption')">			
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>


					<Clienttradereferencenumber>
						<xsl:value-of select ="TradeRefID"/>
					</Clienttradereferencenumber>


					<TradeSequenceNumber>
						<xsl:value-of select ="''"/>
					</TradeSequenceNumber>


					<xsl:variable name="varAsset">
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'STOCK'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'DEBT'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="Asset='FXForward'">
								<xsl:value-of select="'FX'"/>
							</xsl:when>
							<xsl:when test="Asset='FX'">
								<xsl:value-of select="'FXSPOT'"/>
							</xsl:when>
							<xsl:when test="Asset='FXOption'">
								<xsl:value-of select="'FXOTC'"/>
							</xsl:when>
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="'FUTURE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>


					<!--<xsl:variable name="varAsset">
						<xsl:choose>
							<xsl:when test="AssetID=1">
								<xsl:value-of select="'STOCK'"/>
							</xsl:when>
							<xsl:when test="Asset= 8">
								<xsl:value-of select="'DEBT'"/>
							</xsl:when>
							<xsl:when test="Asset= 2">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="Asset=11">
								<xsl:value-of select="'FX'"/>
							</xsl:when>
							<xsl:when test="Asset=10">
								<xsl:value-of select="'FXOTC'"/>
							</xsl:when>
							<xsl:when test="Asset=3">
								<xsl:value-of select="'FUTURE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>-->

					<AssetType>
						<xsl:value-of select ="$varAsset"/>
					</AssetType>


					<xsl:variable name="varSide">
						<xsl:choose>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:choose>
									<xsl:when test="Side='Buy' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'XBUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'XSELL'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'XBC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'XSS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='EquityOption')">
										<xsl:value-of select="'XOP'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Open' and (Asset='EquityOption')">
										<xsl:value-of select="'XCP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Close' and (Asset='EquityOption')">
										<xsl:value-of select="'XCS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open' and (Asset='EquityOption')">
										<xsl:value-of select="'XOS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy' and (Asset='FutureOption')">
										<xsl:value-of select="'XOP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' and (Asset='FutureOption')">
										<xsl:value-of select="'XCS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='FutureOption')">
										<xsl:value-of select="'XCP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' and (Asset='FutureOption')">
										<xsl:value-of select="'XOS'"/>
									</xsl:when>
									<xsl:when test="(Side='Buy' or Side='Buy to Open') and (Asset='Future')">
										<xsl:value-of select="'XBUY'"/>
									</xsl:when>
									<xsl:when test="(Side='Sell' or Side='Sell to Close') and (Asset='Future')">
										<xsl:value-of select="'XSELL'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='Future')">
										<xsl:value-of select="'XBC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open' and (Asset='Future')">
										<xsl:value-of select="'XSS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Side"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:choose>
									<xsl:when test="Side='Buy' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'CBUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'CSELL'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'CBC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'CSS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Open' and (Asset='EquityOption')">
										<xsl:value-of select="'COP'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='EquityOption')">
										<xsl:value-of select="'CCP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Close' and (Asset='EquityOption')">
										<xsl:value-of select="'CCS'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open' and (Asset='EquityOption')">
										<xsl:value-of select="'COS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy' and (Asset='FutureOption')">
										<xsl:value-of select="'COP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' and (Asset='FutureOption')">
										<xsl:value-of select="'CCS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='FutureOption')">
										<xsl:value-of select="'CCP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' and (Asset='FutureOption')">
										<xsl:value-of select="'COS'"/>
									</xsl:when>
									<xsl:when test="(Side='Buy' or Side='Buy to Open') and (Asset='Future')">
										<xsl:value-of select="'CBUY'"/>
									</xsl:when>
									<xsl:when test="(Side='Sell' or Side='Sell to Close') and (Asset='Future')">
										<xsl:value-of select="'CSELL'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='Future')">
										<xsl:value-of select="'CBC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open' and (Asset='Future')">
										<xsl:value-of select="'CSS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Side"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side='Buy' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'BC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' and (Asset='Equity' or Asset='FixedIncome' or Asset='FX' or Asset='FXForward')">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Open' and (Asset='EquityOption')">
										<xsl:value-of select="'OP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Close' and (Asset='EquityOption')">
										<xsl:value-of select="'CS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='EquityOption')">
										<xsl:value-of select="'CP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open' and (Asset='EquityOption')">
										<xsl:value-of select="'OS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy' and (Asset='FutureOption')">
										<xsl:value-of select="'OP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' and (Asset='FutureOption')">
										<xsl:value-of select="'CS'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='FutureOption')">
										<xsl:value-of select="'CP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' and (Asset='FutureOption')">
										<xsl:value-of select="'OS'"/>
									</xsl:when>
									<xsl:when test="(Side='Buy' or Side='Buy to Open') and (Asset='Future')">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side=('Sell' or Side='Sell to Close') and (Asset='Future')">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close' and (Asset='Future')">
										<xsl:value-of select="'BC'"/>
									</xsl:when>
									<xsl:when test="Side='Sell to Open' and (Asset='Future')">
										<xsl:value-of select="'SS'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Side"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<TradeAction>
						<xsl:value-of select ="$varSide"/>
					</TradeAction>

					<xsl:variable name ="varCounterParty">
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>

					<xsl:variable name="varPBCounterParty">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='CS']/BrokerData [@PranaBroker = $varCounterParty]/@MLPBroker"/>
					</xsl:variable>


					<CounterPartyorExecutingBroker>
					<xsl:choose>
						<xsl:when test ="$varPBCounterParty != ''">
							<xsl:value-of select ="$varPBCounterParty"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="CounterParty"/>
						</xsl:otherwise>
					</xsl:choose>
				</CounterPartyorExecutingBroker>





				<AccountNumber>
						<xsl:value-of select ="AccountNo"/>
					</AccountNumber>

					<xsl:variable name="varAccountType">
						<xsl:choose>
							<xsl:when test="Asset = 'Equity' and (Side = 'Sell short' or Side = 'Buy to Close')">
								<xsl:value-of select="'SHORT'"/>
							</xsl:when>
							<xsl:when test="Asset = 'Equity' and (Side != 'Sell short' and Side != 'Buy to Close')">
								<xsl:value-of select="'MARGIN'"/>
							</xsl:when>
							<xsl:when test="Asset = 'EquityOption'">
								<xsl:value-of select="'MARGIN'"/>
							</xsl:when>
							<xsl:when test="Asset = 'FX' or Asset = 'FXForward'">
								<xsl:value-of select="'CASH'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<AccountType>
						<xsl:value-of select="$varAccountType"/>
					</AccountType>

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

					<!-- For Equity Option OSI Symbology-->

					<xsl:variable name="varOptionUnderlying">
						<xsl:value-of select="substring-after(substring-before(Symbol,' '),':')"/>
					</xsl:variable>

					<xsl:variable name = "BlankCount_Root" >
						<xsl:call-template name="noofBlanks">
							<xsl:with-param name="count1" select="(6) - string-length($varOptionUnderlying)" />
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="varFormattedStrikePrice">
						<xsl:value-of select="format-number(StrikePrice,'00000.000')"/>
					</xsl:variable>

					<xsl:variable name="varOSIOptionSymbol">
						<xsl:value-of select="concat($varOptionUnderlying,$BlankCount_Root,substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2),substring(PutOrCall,1,1),translate($varFormattedStrikePrice,'.',''))"/>
					</xsl:variable>



					<xsl:choose>
						<xsl:when test ="(Asset='FX' or Asset = 'FXForward')">
							<xsl:choose>
								<xsl:when test ="(LeadCurrencyName = 'AUD' or LeadCurrencyName = 'NZD' or LeadCurrencyName = 'EUR' or LeadCurrencyName = 'GBP')">
									<TICKER>
										<xsl:value-of select="concat(LeadCurrencyName,'/',VsCurrencyName)"/>
									</TICKER>
								</xsl:when>
								<xsl:otherwise>
									<TICKER>
										<xsl:value-of select="concat(VsCurrencyName,'/',LeadCurrencyName)"/>
									</TICKER>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:when test ="Asset='EquityOption'">
							<TICKER>
								<xsl:choose>
									<xsl:when test="OSIOptionSymbol != ''">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varOSIOptionSymbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</TICKER>
						</xsl:when >
						<xsl:otherwise>
							<TICKER>
								<xsl:value-of select="Symbol"/>
							</TICKER>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test ="Asset='FX' or Asset = 'FXForward'">
							<LocalCurrencyCode>
								<xsl:value-of select ="LeadCurrencyName"/>
							</LocalCurrencyCode>
							<SettlementCurrencyCode>
								<xsl:value-of select ="VsCurrencyName"/>
							</SettlementCurrencyCode>
							<MainFXRate>
								<xsl:choose>
									<xsl:when test ="AveragePrice != 0">
										<xsl:value-of select ="1 div AveragePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="AveragePrice"/>
									</xsl:otherwise>
								</xsl:choose>								
							</MainFXRate>
						</xsl:when>
						<xsl:otherwise>
							<LocalCurrencyCode>
								<xsl:value-of select ="CurrencySymbol"/>
							</LocalCurrencyCode>
							<SettlementCurrencyCode>
								<xsl:value-of select ="CurrencySymbol"/>
							</SettlementCurrencyCode>
							<MainFXRate>
								<!--<xsl:value-of select ="ForexRate_Trade"/>-->
								<xsl:value-of select ="1"/>
							</MainFXRate>
						</xsl:otherwise>
					</xsl:choose>

					<PricingFactor>
						<xsl:value-of select ="''"/>
					</PricingFactor>

					<Filler>
						<xsl:value-of select ="''"/>
					</Filler>

					<Quantity>
						<xsl:value-of select ="AllocatedQty"/>
					</Quantity>

					<PriceLocal>
						<xsl:value-of select ="AveragePrice"/>
					</PriceLocal>

					<xsl:variable name="varAccruedInterest">
						<xsl:choose>
							<xsl:when test="number(AccruedInterest)">
								<xsl:value-of select="AccruedInterest"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<AccruedInterestLocal>
						<xsl:value-of select ="$varAccruedInterest"/>
					</AccruedInterestLocal>

					<!--<xsl:variable name="varPrincipalLocal">
						<xsl:choose>
							<xsl:when test="number(AllocatedQty) and number(AveragePrice) ">
								<xsl:value-of select="AllocatedQty * AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>-->

					<PrincipalLocal>
						<xsl:value-of select ="GrossAmount"/>
					</PrincipalLocal>

					<xsl:variable name ="varUnderlying">
						<xsl:value-of select="substring-before(Symbol,' ')"/>
					</xsl:variable>

					<xsl:variable name ="varAssetCategory">
						<xsl:value-of select="Asset"/>
					</xsl:variable>

					<xsl:variable name ="varExchange">
						<xsl:value-of select="Exchange"/>
					</xsl:variable>

					<xsl:variable name="varcommissionRate">
						<xsl:value-of select="document('../ReconMappingXml/CommissionRate.xml')/CommissionRateMapping/PB[@Name='CS']/SymbolData [@Asset = $varAssetCategory and @Underlying = $varUnderlying and @Exchange = $varExchange]/@CommRate"/>
					</xsl:variable>

					<CommissionLocal>
						<xsl:choose>
							<xsl:when test ="$varcommissionRate != '' and number($varcommissionRate)">
								<xsl:value-of select="AllocatedQty * $varcommissionRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionLocal>

					<!--<CommissionLocal>
						<xsl:value-of select ="CommissionCharged"/>
					</CommissionLocal>-->

					<OtherFeesLocal>
						<xsl:value-of select ="StampDuty  + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions"/>
					</OtherFeesLocal>

					<OtherLocalCharges>
						<xsl:value-of select ="0"/>
					</OtherLocalCharges>

					<LeviesLocal>
						<xsl:value-of select ="TransactionLevy"/>
					</LeviesLocal>

					<SECFeesLocal>
						<xsl:value-of select ="SecFees"/>
					</SECFeesLocal>

					<xsl:variable name="varNetProceedsLocal">
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="NetAmount + $varAccruedInterest"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="NetAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NetProceedsLocal>
						<xsl:value-of select ="$varNetProceedsLocal"/>
					</NetProceedsLocal>


					<SettlementPrice>
						<xsl:value-of select ="''"/>
					</SettlementPrice>


					<AccruedInterestSettlement>
						<xsl:value-of select ="$varAccruedInterest * ForexRate_Trade"/>
					</AccruedInterestSettlement>

					<PrincipalSettlement>
						<xsl:value-of select ="GrossAmount * ForexRate_Trade"/>
					</PrincipalSettlement>

					<CommissionSettlement>
						<xsl:choose>
							<xsl:when test ="$varcommissionRate != ''">
								<xsl:value-of select="AllocatedQty * $varcommissionRate * ForexRate_Trade"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged * ForexRate_Trade"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionSettlement>

					<!--<CommissionSettlement>
						<xsl:value-of select ="(CommissionCharged) * ForexRate_Trade"/>
					</CommissionSettlement>-->

					<FeesSettlement>
						<xsl:value-of select ="(StampDuty  + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions) * ForexRate_Trade"/>
					</FeesSettlement>

					<ChargesSettlement>
						<xsl:value-of select ="0"/>
					</ChargesSettlement>

					<LeviesSettlement>
						<xsl:value-of select ="TransactionLevy * ForexRate_Trade"/>
					</LeviesSettlement>

					<SECFeesSettlement>
						<xsl:value-of select ="SecFees * ForexRate_Trade"/>
					</SECFeesSettlement>

					<NetProceedsSettlement>
						<xsl:value-of select ="$varNetProceedsLocal * ForexRate_Trade"/>
					</NetProceedsSettlement>


					<TradeDate>
						<xsl:value-of select ="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>


					<SettlementDate>
						<xsl:choose>
							<xsl:when test="Asset = 'FX' or Asset = 'FXForward'">
								<xsl:value-of select ="concat(substring(ExpirationDate,7,4),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettlementDate>

					<Reserved>
						<xsl:value-of select ="''"/>
					</Reserved>

					<Notes>
						<xsl:value-of select ="''"/>
					</Notes>

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

					<Productdescription>
						<xsl:value-of select ="''"/>
					</Productdescription>

					<Deliveryinstruction>
						<xsl:value-of select ="''"/>
					</Deliveryinstruction>

					<Settlementinstruction>
						<xsl:value-of select ="''"/>
					</Settlementinstruction>

					<OffDateDeliverydate>
						<xsl:value-of select ="''"/>
					</OffDateDeliverydate>

					<Rate>
						<xsl:value-of select ="''"/>
					</Rate>

					<!--<RerateFixdate>
						<xsl:choose>
							<xsl:when test="(Asset = 'FXForward' or Asset= 'FX') and ExpirationDate != '01/01/1800'">
								<xsl:value-of select ="concat(substring(ExpirationDate,7,4),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2))"/>
						</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</RerateFixdate>-->

					<xsl:choose>
						<xsl:when test ="Asset = 'FXForward' and ExpirationDate != '01/01/1800'">
							<xsl:choose>
								<xsl:when test ="(LeadCurrencyName = 'PHP' or LeadCurrencyName = 'RUB' or LeadCurrencyName = 'KZT')">
									<RerateFixdate>
										<xsl:value-of select ="concat(substring(ProcessDate,7,4),substring(ProcessDate,1,2),substring(ProcessDate,4,2))"/>
									</RerateFixdate>
								</xsl:when>
								<xsl:otherwise>
									<RerateFixdate>
										<xsl:value-of select ="concat(substring(OriginalPurchaseDate,7,4),substring(OriginalPurchaseDate,1,2),substring(OriginalPurchaseDate,4,2))"/>
									</RerateFixdate>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<RerateFixdate>
								<xsl:value-of select="''"/>
							</RerateFixdate>
						</xsl:otherwise>
					</xsl:choose >
					
					
					<Repobasis>
						<xsl:value-of select ="''"/>
					</Repobasis>

					<PutCalltype>
						<xsl:value-of select ="''"/>
					</PutCalltype>

					<Optioncontracttype>
						<xsl:value-of select ="''"/>
					</Optioncontracttype>

					<Expirytime>
						<xsl:value-of select ="''"/>
					</Expirytime>

					<Expirylocation>
						<xsl:value-of select ="''"/>
					</Expirylocation>

					<FXoptionCCY1>
						<xsl:value-of select ="''"/>
					</FXoptionCCY1>

					<FXoptionCCY2>
						<xsl:value-of select ="''"/>
					</FXoptionCCY2>

					<Notionalamount1>
						<xsl:value-of select ="''"/>
					</Notionalamount1>

					<Notionalamount2>
						<xsl:value-of select ="''"/>
					</Notionalamount2>

					<PayoutAmount>
						<xsl:value-of select ="''"/>
					</PayoutAmount>

					<PayoutCCY>
						<xsl:value-of select ="''"/>
					</PayoutCCY>

					<RebateQuoteMode>
						<xsl:value-of select ="''"/>
					</RebateQuoteMode>

					<Rebate1>
						<xsl:value-of select ="''"/>
					</Rebate1>

					<Barrier1>
						<xsl:value-of select ="''"/>
					</Barrier1>

					<Barrier1startdate>
						<xsl:value-of select ="''"/>
					</Barrier1startdate>

					<Barrier1enddate>
						<xsl:value-of select ="''"/>
					</Barrier1enddate>

					<Rebate2>
						<xsl:value-of select ="''"/>
					</Rebate2>

					<Barrier2>
						<xsl:value-of select ="''"/>
					</Barrier2>

					<Barrier2startdate>
						<xsl:value-of select ="''"/>
					</Barrier2startdate>

					<Barrier2enddate>
						<xsl:value-of select ="''"/>
					</Barrier2enddate>

					<Faramount>
						<xsl:value-of select ="''"/>
					</Faramount>

					<Barrierdirection>
						<xsl:value-of select ="''"/>
					</Barrierdirection>

					<Linkindicator>
						<xsl:value-of select ="''"/>
					</Linkindicator>-->

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

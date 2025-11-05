<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="ScientificToNumber">
		<xsl:param name="ScientificN"/>
		<xsl:variable name="vExponent" select="substring-after($ScientificN,'E')"/>
		<xsl:variable name="vMantissa" select="substring-before($ScientificN,'E')"/>
		<xsl:variable name="vFactor"
				 select="substring('100000000000000000000000000000000000000000000',
                              1, substring($vExponent,2) + 1)"/>
		<xsl:choose>
			<xsl:when test="starts-with($vExponent,'-')">
				<xsl:value-of select="$vMantissa div $vFactor"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$vMantissa * $vFactor"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
	
			
			<xsl:for-each select="ThirdPartyFlatFileDetail[(contains(AccountName,'Diamond Growth Fund')='true' or contains(AccountName,'Diamond Neutral Fund')='true')]">


				<xsl:variable name="varCommissionCharged_Expo">
					<xsl:choose>
						<xsl:when test="contains(CommissionCharged,'E')">
							<xsl:call-template name="ScientificToNumber">
								<xsl:with-param name="ScientificN" select="CommissionCharged"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="CommissionCharged"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="varSoftCommissionCharged_Expo">
					<xsl:choose>
						<xsl:when test="contains(SoftCommissionCharged,'E')">
							<xsl:call-template name="ScientificToNumber">
								<xsl:with-param name="ScientificN" select="SoftCommissionCharged"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="SoftCommissionCharged"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
				<xsl:variable name="varOtherBrokerFees_Expo">
					<xsl:choose>
						<xsl:when test="contains(OtherBrokerFees,'E')">
							<xsl:call-template name="ScientificToNumber">
								<xsl:with-param name="ScientificN" select="OtherBrokerFees"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="OtherBrokerFees"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
			

				<xsl:variable name="varNetamount">
					<xsl:choose>
						<xsl:when test="contains(Side,'Buy')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + $varCommissionCharged_Expo + $varSoftCommissionCharged_Expo + $varOtherBrokerFees_Expo + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Sell')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - ($varCommissionCharged_Expo + $varSoftCommissionCharged_Expo + $varOtherBrokerFees_Expo + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="TaxLotState!='Amemded'">
						<ThirdPartyFlatFileDetail>


							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							<FileFooter>
								<xsl:value-of select="'true'"/>
							</FileFooter>
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>


							<RecordTypeCode>
								<xsl:value-of select="'TRN'"/>
							</RecordTypeCode>

							<AssetType>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="'OPTION'"/>
									</xsl:when>
									<xsl:when test="Asset='FxForward'">
										<xsl:value-of select="'Forward'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="Asset='FixedIncome'">
												<xsl:value-of select="'BOND'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</AssetType>

							<xsl:variable name ="varAllocationState">
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEW'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'CANC'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Amendment'">
										<xsl:value-of select ="'CORC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<TransactionStatus>
								<xsl:value-of select ="'NEW'"/>
							</TransactionStatus>

							<LongShortIndicator>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Open'">
										<xsl:value-of select="'L'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'S'"/>
									</xsl:otherwise>
								</xsl:choose>
							</LongShortIndicator>

							<TransactionCode>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SHORT'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'COVER'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</TransactionCode>

							<ClientTradeRefNo>
								<xsl:value-of select="EntityID"/>
							</ClientTradeRefNo>

							<xsl:variable name="PB_NAME" select="'NT'"/>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="AccountName"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!='' or $THIRDPARTY_FUND_CODE!='*'">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AccountId>
								<xsl:value-of select="$varAccountName"/>
							</AccountId>

							<PrimeBrokerCustodianAccountID>
								<xsl:choose>
									<xsl:when test="AccountName='Diamond Growth Fund'">
										<xsl:value-of select="'6014588'"/>
									</xsl:when>
									<xsl:when test="AccountName='Diamond Neutral Fund'">
										<xsl:value-of select="'6014589'"/>
									</xsl:when>						

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
					
							</PrimeBrokerCustodianAccountID>

							<BrokerCounterparty>
								<xsl:choose>
									<xsl:when test="CounterParty='RAJA'">
										<xsl:value-of select="'RAYJ'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="CounterParty"/>
									</xsl:otherwise>
								</xsl:choose>
							</BrokerCounterparty>

							<BrokerDescription>
								<xsl:choose>
									<xsl:when test="CounterParty='RAJA'">
										<xsl:value-of select="'RAYJ'"/>
									</xsl:when>
								
									<xsl:otherwise>
										<xsl:value-of select="CounterParty"/>
									</xsl:otherwise>
								</xsl:choose>
							
							</BrokerDescription>

							<ClearingBroker>
								<xsl:value-of select="''"/>
							</ClearingBroker>

							<ClearingBrokerdescription>
								<xsl:value-of select="''"/>
							</ClearingBrokerdescription>

							<SecurityIdentifierType>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'SEDOL'"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'ISIN'"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="'TICKER'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIdentifierType>

							<SecurityIdentifier>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="ISIN"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="Symbol"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIdentifier>

							<UnderlyingSecurityIdentifierType>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityIdentifierType>

							<UnderlyingSecurityIdentifier>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityIdentifier>

							<SecurityDescription>
								<xsl:value-of select="CompanyName"/>
							</SecurityDescription>

							<xsl:variable name="TradeDatea">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDatea">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<TradeDate>
								<xsl:value-of select="concat(substring-after(substring-after($TradeDatea,'/'),'/'),substring-before($TradeDatea,'/'),substring-before(substring-after($TradeDatea,'/'),'/'))"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select="concat(substring-after(substring-after($SettlementDatea,'/'),'/'),substring-before($SettlementDatea,'/'),substring-before(substring-after($SettlementDatea,'/'),'/'))"/>
							</SettlementDate>

							<Settlementcurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</Settlementcurrency>

							<LocalCCY>
								<xsl:value-of select="CurrencySymbol"/>
							</LocalCCY>

							<QuantityOriginalFace>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</QuantityOriginalFace>

							<CurrentFace>
								<xsl:value-of select="''"/>
							</CurrentFace>

							<Price>
								<xsl:choose>
									<xsl:when test="number(AvgPrice)">
										<xsl:value-of select="format-number(AvgPrice,'0.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Price>

							<Principal>
								<xsl:value-of select="format-number(OrderQty * AvgPrice,'0.##')"/>
							</Principal>

							<xsl:variable name="SoftSoftCommission_Expo">
								<xsl:choose>
									<xsl:when test="contains(SoftCommissionCharged,'E')">
										<xsl:call-template name="ScientificToNumber">
											<xsl:with-param name="ScientificN" select="SoftCommissionCharged"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="SoftCommissionCharged"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<xsl:variable name="varCommissionCharged">
								<xsl:choose>
									<xsl:when test="contains(CommissionCharged,'E')">
										<xsl:call-template name="ScientificToNumber">
											<xsl:with-param name="ScientificN" select="CommissionCharged"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CommissionCharged"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varCommissions">
								<xsl:value-of select="$varCommissionCharged + $SoftSoftCommission_Expo"/>
							</xsl:variable>
							
							<!--<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>-->

							<CommissionAmount>
								<xsl:value-of select="format-number($varCommissions,'0.####')"/>
							</CommissionAmount>

							<Taxorfees>
								<xsl:value-of select="format-number(SecFee + OtherBrokerFees,'0.####')"/>
							</Taxorfees>

							<Tax2>
								<xsl:value-of select="''"/>
							</Tax2>

							<Interest>
								<xsl:value-of select="''"/>
							</Interest>

							<NegativeInterest>
								<xsl:value-of select="''"/>
							</NegativeInterest>

							<ConsiderationConstant>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'0.01'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="1"/>
									</xsl:otherwise>
								</xsl:choose>
							</ConsiderationConstant>

							<xsl:variable name="varNetamount_Expo">
								<xsl:choose>
									<xsl:when test="contains($varNetamount,'E')">
										<xsl:call-template name="ScientificToNumber">
											<xsl:with-param name="ScientificN" select="$varNetamount"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varNetamount"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<NetAmountLocal>
								<xsl:value-of select="format-number($varNetamount_Expo,'0.##')"/>
							</NetAmountLocal>

							<InternalNetNotional>
								<xsl:value-of select="format-number($varNetamount_Expo,'0.##')"/>
							</InternalNetNotional>

							<NetAmountSettled>
								<xsl:value-of select="''"/>
							</NetAmountSettled>

							<FXdealingrate>
								<xsl:value-of select="1"/>
							</FXdealingrate>

							<FXcurrencyReceived>
								<xsl:value-of select="''"/>
							</FXcurrencyReceived>

							<FXcurrencyDelivered>
								<xsl:value-of select="''"/>
							</FXcurrencyDelivered>

							<ClassSpecificHedge>
								<xsl:value-of select="''"/>
							</ClassSpecificHedge>

							<NotionalAmountReceived>
								<xsl:value-of select="''"/>
							</NotionalAmountReceived>

							<NotionalAmountdelivered>
								<xsl:value-of select="''"/>
							</NotionalAmountdelivered>

							<StrategyCodes>
								<xsl:value-of select="''"/>
							</StrategyCodes>


							<StrikePrice>
								<xsl:value-of select="''"/>
							</StrikePrice>

							<ExpireDate>
								<xsl:value-of select="''"/>
							</ExpireDate>

							<CountryofQuotation>
								<xsl:value-of select="''"/>
							</CountryofQuotation>

							<Null>
								<xsl:value-of select="''"/>
							</Null>

							<Null1>
								<xsl:value-of select="''"/>
							</Null1>

							<Null2>
								<xsl:value-of select="''"/>
							</Null2>

							<Null3>
								<xsl:value-of select="''"/>
							</Null3>

							<Exchange>
								<xsl:value-of select="Exchange"/>
							</Exchange>

							<AdditionalInfo1>
								<xsl:value-of select="''"/>
							</AdditionalInfo1>

							<AdditionalInfo2>
								<xsl:value-of select="''"/>
							</AdditionalInfo2>

							<AdditionalInfo3>
								<xsl:value-of select="''"/>
							</AdditionalInfo3>

							<AdditionalInfo4>
								<xsl:value-of select="''"/>
							</AdditionalInfo4>

							<AdditionalInfo5>
								<xsl:value-of select="''"/>
							</AdditionalInfo5>

							<AdditionaInfo6>
								<xsl:value-of select="''"/>
							</AdditionaInfo6>




							<!-- system use only-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						<xsl:if test ="number(OldExecutedQuantity)">
							<ThirdPartyFlatFileDetail>

								<!--<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>-->

								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>

								<FileHeader>
									<xsl:value-of select="'true'"/>
								</FileHeader>

								<FileFooter>
									<xsl:value-of select="'true'"/>
								</FileFooter>
								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>


								<RecordTypeCode>
									<xsl:value-of select="'TRN'"/>
								</RecordTypeCode>

								<AssetType>
									<xsl:choose>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'STOCK'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="Asset='FixedIncome'">
													<xsl:value-of select="'BOND'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="''"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</AssetType>

								<xsl:variable name="varTaxlotState">
									<xsl:choose>
										<xsl:when test="TaxLotState='Allocated'">
											<xsl:value-of select ="'NEW'"/>
										</xsl:when>
										<xsl:when test="TaxLotState='Amended'">
											<xsl:value-of select ="'Corrected'"/>
										</xsl:when>
										<xsl:when test="TaxLotState='Deleted'">
											<xsl:value-of select ="'Cancelled'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<TransactionStatus>
									<xsl:value-of select ="'CANC'"/>
								</TransactionStatus>

								<LongShortIndicator>
									<xsl:choose>
										<xsl:when test="OldSide='Buy' or OldSide='Buy to Open' or OldSide='Sell short' or OldSide='Sell to Open'">
											<xsl:value-of select="'L'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'S'"/>
										</xsl:otherwise>
									</xsl:choose>
								</LongShortIndicator>

								<TransactionCode>
									<xsl:choose>
										<xsl:when test="OldSide='Buy' or OldSide='Buy to Open'">
											<xsl:value-of select="'BUY'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell' or OldSide='Sell to Close'">
											<xsl:value-of select="'SELL'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell short' or OldSide='Sell to Open'">
											<xsl:value-of select="'SHORT'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'COVER'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</TransactionCode>

								<ClientTradeRefNo>
									<xsl:value-of select="EntityID"/>
								</ClientTradeRefNo>

								<xsl:variable name="PB_NAME" select="'NT'"/>

								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="AccountName"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>


								<xsl:variable name="varAccountName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_FUND_CODE!='' or $THIRDPARTY_FUND_CODE!='*'">
											<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<AccountId>
									<xsl:value-of select="$varAccountName"/>
								</AccountId>

								<PrimeBrokerCustodianAccountID>
									<xsl:choose>
										<xsl:when test="AccountName='Diamond Growth Fund'">
											<xsl:value-of select="'6014589'"/>
										</xsl:when>
										<xsl:when test="AccountName='Diamond Neutral Fund'">
											<xsl:value-of select="'6014588'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</PrimeBrokerCustodianAccountID>

								<BrokerCounterparty>
									<xsl:value-of select="OldCounterparty"/>
								</BrokerCounterparty>

								<BrokerDescription>
									<xsl:value-of select="OldCounterparty"/>
								</BrokerDescription>

								<ClearingBroker>
									<xsl:value-of select="''"/>
								</ClearingBroker>

								<ClearingBrokerdescription>
									<xsl:value-of select="''"/>
								</ClearingBrokerdescription>

								<SecurityIdentifierType>
									<xsl:choose>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'SEDOL'"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="'ISIN'"/>
										</xsl:when>
										<xsl:when test="Asset='Future'">
											<xsl:value-of select="'TICKER'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityIdentifierType>

								<SecurityIdentifier>
									<xsl:choose>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="SEDOL"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="ISIN"/>
										</xsl:when>
										<xsl:when test="Asset='Future'">
											<xsl:value-of select="Symbol"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityIdentifier>

								<UnderlyingSecurityIdentifierType>
									<xsl:value-of select="''"/>
								</UnderlyingSecurityIdentifierType>

								<UnderlyingSecurityIdentifier>
									<xsl:value-of select="''"/>
								</UnderlyingSecurityIdentifier>

								<SecurityDescription>
									<xsl:value-of select="CompanyName"/>
								</SecurityDescription>

								<xsl:variable name="OldTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<xsl:variable name="OldSettleDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>



								<TradeDate>
									<xsl:value-of select="concat(substring-after(substring-after($OldTradeDate,'/'),'/'),substring-before($OldTradeDate,'/'),substring-before(substring-after($OldTradeDate,'/'),'/'))"/>
								</TradeDate>

								<SettlementDate>
									<xsl:value-of select="concat(substring-after(substring-after($OldSettleDate,'/'),'/'),substring-before($OldSettleDate,'/'),substring-before(substring-after($OldSettleDate,'/'),'/'))"/>
								</SettlementDate>
								<!--<TradeDate>
									<xsl:value-of select="concat(substring-before($OldTradeDate,'/'),'/',substring-before(substring-after($OldTradeDate,'/'),'/'),'/',substring-after(substring-after($OldTradeDate,'/'),'/'))"/>
								</TradeDate>

								<SettlementDate>
									<xsl:value-of select="concat(substring-before($OldSettleDate,'/'),'/',substring-before(substring-after($OldSettleDate,'/'),'/'),'/',substring-after(substring-after($OldSettleDate,'/'),'/'))"/>
								</SettlementDate>-->

								<Settlementcurrency>
									<xsl:value-of select="Currency"/>
								</Settlementcurrency>

								<LocalCCY>
									<xsl:value-of select="CurrencySymbol"/>
								</LocalCCY>

								<QuantityOriginalFace>
									<xsl:choose>
										<xsl:when test="number(OldExecutedQuantity)">
											<xsl:value-of select="OldExecutedQuantity"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</QuantityOriginalFace>

								<CurrentFace>
									<xsl:value-of select="''"/>
								</CurrentFace>

								<Price>
									<xsl:choose>
										<xsl:when test="number(OldAvgPrice)">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</Price>

								<Principal>
									<xsl:value-of select="OldExecutedQuantity * OldAvgPrice"/>
								</Principal>

								<xsl:variable name="varTotalCommission">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>

								<CommissionAmount>
									<xsl:value-of select="$varTotalCommission"/>
								</CommissionAmount>

								<Taxorfees>
									<xsl:value-of select="SecFee + OtherBrokerFees"/>
								</Taxorfees>

								<Tax2>
									<xsl:value-of select="''"/>
								</Tax2>

								<Interest>
									<xsl:value-of select="''"/>
								</Interest>

								<NegativeInterest>
									<xsl:value-of select="''"/>
								</NegativeInterest>

								<ConsiderationConstant>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="'0.01'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="1"/>
										</xsl:otherwise>
									</xsl:choose>
								</ConsiderationConstant>

								<xsl:variable name="varOldNetAmount">
									<xsl:choose>
										<xsl:when test="contains(Side,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
										</xsl:when>
										<xsl:when test="contains(Side,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>


								<xsl:variable name="varOldNetamount_Expo">
									<xsl:choose>
										<xsl:when test="contains($varOldNetAmount,'E')">
											<xsl:call-template name="ScientificToNumber">
												<xsl:with-param name="ScientificN" select="$varOldNetAmount"/>
											</xsl:call-template>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varOldNetAmount"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>


								<NetAmountLocal>
									<xsl:value-of select="$varOldNetamount_Expo"/>
								</NetAmountLocal>

								<InternalNetNotional>
									<xsl:value-of select="$varOldNetamount_Expo"/>
								</InternalNetNotional>

								<NetAmountSettled>
									<xsl:value-of select="''"/>
								</NetAmountSettled>

								<FXdealingrate>
									<xsl:value-of select="1"/>
								</FXdealingrate>

								<FXcurrencyReceived>
									<xsl:value-of select="''"/>
								</FXcurrencyReceived>

								<FXcurrencyDelivered>
									<xsl:value-of select="''"/>
								</FXcurrencyDelivered>

								<ClassSpecificHedge>
									<xsl:value-of select="''"/>
								</ClassSpecificHedge>

								<NotionalAmountReceived>
									<xsl:value-of select="''"/>
								</NotionalAmountReceived>

								<NotionalAmountdelivered>
									<xsl:value-of select="''"/>
								</NotionalAmountdelivered>

								<StrategyCodes>
									<xsl:value-of select="''"/>
								</StrategyCodes>


								<StrikePrice>
									<xsl:value-of select="''"/>
								</StrikePrice>

								<ExpireDate>
									<xsl:value-of select="''"/>
								</ExpireDate>

								<CountryofQuotation>
									<xsl:value-of select="''"/>
								</CountryofQuotation>

								<Null>
									<xsl:value-of select="''"/>
								</Null>

								<Null1>
									<xsl:value-of select="''"/>
								</Null1>

								<Null2>
									<xsl:value-of select="''"/>
								</Null2>

								<Null3>
									<xsl:value-of select="''"/>
								</Null3>

								<Exchange>
									<xsl:value-of select="Exchange"/>
								</Exchange>

								<AdditionalInfo1>
									<xsl:value-of select="''"/>
								</AdditionalInfo1>

								<AdditionalInfo2>
									<xsl:value-of select="''"/>
								</AdditionalInfo2>

								<AdditionalInfo3>
									<xsl:value-of select="''"/>
								</AdditionalInfo3>

								<AdditionalInfo4>
									<xsl:value-of select="''"/>
								</AdditionalInfo4>

								<AdditionalInfo5>
									<xsl:value-of select="''"/>
								</AdditionalInfo5>

								<AdditionaInfo6>
									<xsl:value-of select="''"/>
								</AdditionaInfo6>




								<!-- system use only-->
								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>

						<ThirdPartyFlatFileDetail>

							<!--<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>-->

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<FileHeader>
								<xsl:value-of select="'true'"/>
							</FileHeader>

							<FileFooter>
								<xsl:value-of select="'true'"/>
							</FileFooter>
							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>


							<RecordTypeCode>
								<xsl:value-of select="'TRN'"/>
							</RecordTypeCode>

							<AssetType>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'STOCK'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="Asset='FixedIncome'">
												<xsl:value-of select="'BOND'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</AssetType>

							<xsl:variable name="varTaxlotState">
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEW'"/>
									</xsl:when>
									<xsl:when test="TaxLotState='Amended'">
										<xsl:value-of select ="'Corrected'"/>
									</xsl:when>
									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'Cancelled'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<TransactionStatus>
								<xsl:value-of select ="'NEW'"/>
							</TransactionStatus>

							<LongShortIndicator>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'L'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'S'"/>
									</xsl:otherwise>
								</xsl:choose>
							</LongShortIndicator>

							<TransactionCode>
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SHORT'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'COVER'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</TransactionCode>

							<ClientTradeRefNo>
								<xsl:value-of select="EntityID"/>
							</ClientTradeRefNo>

							<xsl:variable name="PB_NAME" select="'NT'"/>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="AccountName"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!='' or $THIRDPARTY_FUND_CODE!='*'">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AccountId>
								<xsl:value-of select="$varAccountName"/>
							</AccountId>

							<PrimeBrokerCustodianAccountID>
								<xsl:choose>
									<xsl:when test="AccountName='Diamond Growth Fund'">
										<xsl:value-of select="'6014589'"/>
									</xsl:when>
									<xsl:when test="AccountName='Diamond Neutral Fund'">
										<xsl:value-of select="'6014588'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</PrimeBrokerCustodianAccountID>

							<BrokerCounterparty>
								<xsl:value-of select="CounterParty"/>
							</BrokerCounterparty>

							<BrokerDescription>
								<xsl:value-of select="CounterParty"/>
							</BrokerDescription>

							<ClearingBroker>
								<xsl:value-of select="''"/>
							</ClearingBroker>

							<ClearingBrokerdescription>
								<xsl:value-of select="''"/>
							</ClearingBrokerdescription>

							<SecurityIdentifierType>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'SEDOL'"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'ISIN'"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="'TICKER'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIdentifierType>

							<SecurityIdentifier>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="ISIN"/>
									</xsl:when>
									<xsl:when test="Asset='Future'">
										<xsl:value-of select="Symbol"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIdentifier>

							<UnderlyingSecurityIdentifierType>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityIdentifierType>

							<UnderlyingSecurityIdentifier>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityIdentifier>

							<SecurityDescription>
								<xsl:value-of select="CompanyName"/>
							</SecurityDescription>

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<TradeDate>
								<xsl:value-of select="concat(substring-after(substring-after($TradeDate,'/'),'/'),substring-before($TradeDate,'/'),substring-before(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select="concat(substring-after(substring-after($SettlementDate,'/'),'/'),substring-before($SettlementDate,'/'),substring-before(substring-after($SettlementDate,'/'),'/'))"/>
							</SettlementDate>
							
							<Settlementcurrency>
								<xsl:value-of select="Currency"/>
							</Settlementcurrency>

							<LocalCCY>
								<xsl:value-of select="CurrencySymbol"/>
							</LocalCCY>

							<QuantityOriginalFace>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</QuantityOriginalFace>

							<CurrentFace>
								<xsl:value-of select="''"/>
							</CurrentFace>

							<Price>
								<xsl:choose>
									<xsl:when test="number(AvgPrice)">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Price>

							<Principal>
								<xsl:value-of select="CumQty * AvgPrice"/>
							</Principal>
							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<CommissionAmount>
								<xsl:value-of select="$varCommission"/>
							</CommissionAmount>

							<Taxorfees>
								<xsl:value-of select="SecFee + OtherBrokerFees"/>
							</Taxorfees>

							<Tax2>
								<xsl:value-of select="''"/>
							</Tax2>

							<Interest>
								<xsl:value-of select="''"/>
							</Interest>

							<NegativeInterest>
								<xsl:value-of select="''"/>
							</NegativeInterest>

							<ConsiderationConstant>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'0.01'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="1"/>
									</xsl:otherwise>
								</xsl:choose>
							</ConsiderationConstant>

							<xsl:variable name="varNetamount_SExpo">
								<xsl:choose>
									<xsl:when test="contains($varNetamount,'E')">
										<xsl:call-template name="ScientificToNumber">
											<xsl:with-param name="ScientificN" select="$varNetamount"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varNetamount"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<NetAmountLocal>
								<xsl:value-of select="$varNetamount_SExpo"/>
							</NetAmountLocal>

							<InternalNetNotional>
								<xsl:value-of select="$varNetamount_SExpo"/>
							</InternalNetNotional>

							<NetAmountSettled>
								<xsl:value-of select="''"/>
							</NetAmountSettled>

							<FXdealingrate>
								<xsl:value-of select="1"/>
							</FXdealingrate>

							<FXcurrencyReceived>
								<xsl:value-of select="''"/>
							</FXcurrencyReceived>

							<FXcurrencyDelivered>
								<xsl:value-of select="''"/>
							</FXcurrencyDelivered>

							<ClassSpecificHedge>
								<xsl:value-of select="''"/>
							</ClassSpecificHedge>

							<NotionalAmountReceived>
								<xsl:value-of select="''"/>
							</NotionalAmountReceived>

							<NotionalAmountdelivered>
								<xsl:value-of select="''"/>
							</NotionalAmountdelivered>

							<StrategyCodes>
								<xsl:value-of select="''"/>
							</StrategyCodes>


							<StrikePrice>
								<xsl:value-of select="''"/>
							</StrikePrice>

							<ExpireDate>
								<xsl:value-of select="''"/>
							</ExpireDate>

							<CountryofQuotation>
								<xsl:value-of select="''"/>
							</CountryofQuotation>

							<Null>
								<xsl:value-of select="''"/>
							</Null>

							<Null1>
								<xsl:value-of select="''"/>
							</Null1>

							<Null2>
								<xsl:value-of select="''"/>
							</Null2>

							<Null3>
								<xsl:value-of select="''"/>
							</Null3>

							<Exchange>
								<xsl:value-of select="Exchange"/>
							</Exchange>

							<AdditionalInfo1>
								<xsl:value-of select="''"/>
							</AdditionalInfo1>

							<AdditionalInfo2>
								<xsl:value-of select="''"/>
							</AdditionalInfo2>

							<AdditionalInfo3>
								<xsl:value-of select="''"/>
							</AdditionalInfo3>

							<AdditionalInfo4>
								<xsl:value-of select="''"/>
							</AdditionalInfo4>

							<AdditionalInfo5>
								<xsl:value-of select="''"/>
							</AdditionalInfo5>

							<AdditionaInfo6>
								<xsl:value-of select="''"/>
							</AdditionaInfo6>




							<!-- system use only-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:otherwise>
				</xsl:choose>
				</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

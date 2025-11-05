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

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='PACE - Longs - State Street' or AccountName='PACE - Shorts - Morgan Stanley']">
				
				<xsl:variable name="varNetamount">
					<xsl:choose>
						<xsl:when test="contains(Side,'Buy')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Sell')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="TaxLotState!='Amemded'">
						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'True'"/>
							</RowHeader>

							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<TransactionType>
							<xsl:choose>
								<xsl:when test="contains(Side,'Buy')">
									<xsl:value-of select="'BUY'"/>
								</xsl:when>
								<xsl:when test="contains(Side,'Sell')">
									<xsl:value-of select="'SELL'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
							</TransactionType>

						
							<MessageFunction>
								<xsl:choose> 
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEWM'"/>
									</xsl:when>
									
									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'CANC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>
							</MessageFunction>

							<TransactionReference>
								<xsl:value-of select="EntityID"/>
							</TransactionReference>

							<RelatedReferenceNumber>
								<xsl:value-of select="''"/>
							</RelatedReferenceNumber>

							<xsl:variable name="PB_NAME" select="'US Bank'"/>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="'CFFW'"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<FundID>
								<xsl:value-of select="$varAccountName"/>
							</FundID>

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
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>

						
							<SettlementDate>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
							</SettlementDate>

							<LateDeliveryDate>
								<xsl:value-of select="''"/>
							</LateDeliveryDate>

							<SecurityIDType>
								<xsl:choose>
									<xsl:when test="contains(Asset,'EquityOption')">
										<xsl:value-of select="'TS'"/>
									</xsl:when>
									<xsl:when test="contains(Asset,'Future')">
										<xsl:value-of select="'TS'"/>
									</xsl:when>
									<xsl:when test="SEDOL!='*'">
										<xsl:value-of select="'GB'"/>
									</xsl:when>
									<xsl:when test="CUSIP!='*'">
										<xsl:value-of select="'US'"/>
									</xsl:when>
									<xsl:when test="ISIN!='*'">
										<xsl:value-of select="'ISIN'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIDType>

							<SecurityID>
								<xsl:choose>
									<xsl:when test="contains(Asset,'EquityOption')">
										<xsl:value-of select="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:when test="contains(Asset,'Future')">
										<xsl:value-of select="BBCode"/>
									</xsl:when>
									<xsl:when test="SEDOL!='*'">
										<xsl:value-of select="SEDOL"/>
									</xsl:when>
									<xsl:when test="CUSIP!='*'">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>

									<xsl:when test="ISIN!='*'">
										<xsl:value-of select="ISIN"/>
									</xsl:when>
									<xsl:when test="Symbol!='*'">
										<xsl:value-of select="Symbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityID>


							<SecurityDescription>
								<xsl:value-of select="CompanyName"/>
							</SecurityDescription>

							<SecurityType>
								<xsl:choose>

									<xsl:when test="contains(Asset,'Option')">
										<xsl:value-of select="'OPT'"/>
									</xsl:when>
									<xsl:when test="UDAAssetName='ETF'">
										<xsl:value-of select="'ETF'"/>
									</xsl:when>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'CS'"/>
									</xsl:when>
									<xsl:when test="contains(Asset,'Future')">
										<xsl:value-of select="'FUT'"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'CORP'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityType>

							<CurrencyOfDenomination>
								<xsl:value-of select="''"/>
							</CurrencyOfDenomination>
							
							<OptionStyle>
								<xsl:value-of select="''"/>
							</OptionStyle>

							<OptionType>
								<xsl:value-of select="''"/>
							</OptionType>

							<ContractSize>
								<xsl:value-of select="''"/>
							</ContractSize>

							<StrikePrice>
								<xsl:value-of select="''"/>
							</StrikePrice>

							<ExpirationDate>
								<xsl:value-of select="''"/>
							</ExpirationDate>

							<UnderlyingSecurityIDType>
								<xsl:value-of select="''"/>	
							</UnderlyingSecurityIDType>

							<UnderlyingSecurityID>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityID>

							<UnderlyingSecurityDesc>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityDesc>

							<MaturityDate>
								<xsl:value-of select="''"/>
							</MaturityDate>

							<IssueDate>
								<xsl:value-of select="''"/>
							</IssueDate>

							<InterestRate>							
								<xsl:value-of select="''"/>							
							</InterestRate>

							<OriginalFace>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="OrderQty * AssetMultiplier"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</OriginalFace>


							<Quantity>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>

							<TradeCurrency>
								<xsl:value-of select="SettlCurrency"/>
							</TradeCurrency>

							<DealPriceCode>
								<xsl:value-of select="'ACTU'"/>
							</DealPriceCode>

							<xsl:variable name="AvgPrice1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<DealPrice>
								<xsl:choose>
									<xsl:when test="number($AvgPrice1)">
										<xsl:value-of select="format-number($AvgPrice1,'0.######')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</DealPrice>


							<xsl:variable name="Principal" select="OrderQty * AvgPrice * AssetMultiplier"/>

							<xsl:variable name="Principal1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$Principal"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$Principal * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$Principal div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<PrincipalAmount>
								<xsl:choose>
									<xsl:when test="number($Principal1)">
										<xsl:value-of select="format-number($Principal1,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</PrincipalAmount>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varCommission * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varCommission div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<CommissionsAmount>
								<xsl:choose>
									<xsl:when test="number($varCommission1)">
										<xsl:value-of select="format-number($varCommission1,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</CommissionsAmount>

							<xsl:variable name="OtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="OtherFees1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$OtherFees"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$OtherFees * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$OtherFees div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ChargesFeesAmount>
								<xsl:choose>
									<xsl:when test="number($OtherFees1)">
										<xsl:value-of select="format-number($OtherFees1,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</ChargesFeesAmount>

							<OtherAmount>
								<xsl:value-of select="''"/>
							</OtherAmount>

							<xsl:variable name="AccruedInterest1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AccruedInterest"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AccruedInterest * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AccruedInterest div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AccruedInterestAmount>
								<xsl:choose>
									<xsl:when test="number(AccruedInterest1)">
										<xsl:value-of select="format-number(AccruedInterest1,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</AccruedInterestAmount>

							<TaxesAmount>
								<xsl:value-of select="''"/>
							</TaxesAmount>

							<StampDutyExemptionAmount>
								<xsl:value-of select="''"/>
							</StampDutyExemptionAmount>

							<SettlementCurrency>
								<xsl:value-of select="SettlCurrency"/>
							</SettlementCurrency>

							
							
						
							<xsl:variable name = "NETAMNT">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varNetamount * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varNetamount div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<SettlementAmount>
								<xsl:value-of select="format-number($NETAMNT,'0.##')"/>
							</SettlementAmount>


							<TransactionSubType>
								<xsl:value-of select="'TRAD'"/>
							</TransactionSubType>

							<SettlementTransactionConditionIndicator>
								<xsl:choose>

									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'SHOR'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'BUTC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlementTransactionConditionIndicator>


							<SettlementTransactionConditionIndicator2>
								<xsl:choose>

									<xsl:when test="Side='Sell short' or Side='Sell to Open'">
										<xsl:value-of select="'RPTO'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'RPTO'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlementTransactionConditionIndicator2>

							<ProcessingIndicator>
								<xsl:choose>
									<xsl:when test="contains(Asset,'Future')">
										<xsl:choose>
											<xsl:when test="contains(Side,'Open')">
												<xsl:value-of select="'OPEP'"/>
											</xsl:when>
											<xsl:when test="contains(Side,'Close')">
												<xsl:value-of select="'CLOP'"/>
											</xsl:when>

											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test="contains(Asset,'EquityOption')">
										<xsl:choose>
											<xsl:when test="contains(Side,'Open')">
												<xsl:value-of select="'OPEP'"/>
											</xsl:when>
											<xsl:when test="contains(Side,'Close')">
												<xsl:value-of select="'CLOP'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ProcessingIndicator>
							
							<TrackingIndicator>
								<xsl:value-of select="''"/>
							</TrackingIndicator>

							<SettlementLocation>
								<xsl:choose>
									<xsl:when test="CurrencySymbol='USD'">
										<xsl:value-of select="'DTCYUS33'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='AUSTRIA'">
										<xsl:value-of select="'OCSDATWWXXX'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='GERMANY'">
										<xsl:value-of select="'DAKVDEFFXXX'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='JAPAN'">
										<xsl:value-of select="'JJSDJPJT'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='UNITED KINGDOM'">
										<xsl:value-of select="'CRSTGB22'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='FRANCE'">
										<xsl:value-of select="'SICVFRPPXXX'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='CANADA'">
										<xsl:value-of select="'CDSLCATT'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='DENMARK'">
										<xsl:value-of select="'VPDKDKKKXXX'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='ITALY'">
										<xsl:value-of select="'MOTIITMM'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='BRAZIL'">
										<xsl:value-of select="'SSCSBRR1'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='AUSTRALIA'">
										<xsl:value-of select="'CAETAU21'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='SWEDEN'">
										<xsl:value-of select="'VPCSSESS'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='SPAIN'">
										<xsl:value-of select="'IBRCESMMXXX'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='SWITZERLAND'">
										<xsl:value-of select="'INSECHZZXXX'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlementLocation>

							<PlaceOfTrade>
								<xsl:value-of select="''"/>
							</PlaceOfTrade>

							<PlaceOfSafekeeping>
								<xsl:value-of select="''"/>
							</PlaceOfSafekeeping>

							<FXContraCurrency>
								<xsl:value-of select="''"/>
							</FXContraCurrency>

							<FXOrderCXLIndicator>
								<xsl:value-of select="''"/>
							</FXOrderCXLIndicator>


							<ExecutingBrokerIDType>
								<xsl:choose>
									<xsl:when test="CurrencySymbol='USD'">
										<xsl:value-of select="'DTCYID'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'BIC'"/>
									</xsl:otherwise>
								</xsl:choose>
							</ExecutingBrokerIDType>

							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<ExecutingBrokerID>
								<xsl:choose>
									<xsl:when test="CurrencySymbol!='USD'">
										<xsl:value-of select="'MSNYUS33'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$THIRDPARTY_BROKER!= ''">
												<xsl:value-of select="$THIRDPARTY_BROKER"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</ExecutingBrokerID>

							<ExecutingBrokerAcct>
								<xsl:value-of select="''"/>
							</ExecutingBrokerAcct>

							<ClearingBrokerAgentIDType>
								<xsl:choose>
									<xsl:when test="CurrencySymbol='USD'">
										<xsl:value-of select="'DTCYID'"/>
									</xsl:when>
									<xsl:when test="UDACountryName='UNITED KINGDOM'">
										<xsl:value-of select="'CRST'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="'BIC'"/>
									</xsl:otherwise>
								</xsl:choose>
							</ClearingBrokerAgentIDType>


							<xsl:variable name="THIRDPARTY_COUNTERPARTY">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
							</xsl:variable>
							<xsl:variable name="BrokerName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
										<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ClearingBrokerAgentID>
								<xsl:choose>
								<xsl:when test="UDACountryName='CANADA'">
									<xsl:value-of select="'ROYCCAT2'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='JAPAN'">
									<xsl:value-of select="'MSTKJPJX'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='UNITED KINGDOM'">
									<xsl:value-of select="'50708'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='AUSTRIA'">
									<xsl:value-of select="'BKAUATWW'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='GERMANY'">
									<xsl:value-of select="'MSFFDEFX'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='FRANKURT'">
									<xsl:value-of select="'MSFFDEFX'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='FRANCE'">
									<xsl:value-of select="'PARBFRPPXXX'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='DENMARK'">
									<xsl:value-of select="'ESSEDKKK'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='ITALY'">
									<xsl:value-of select="'CITIITM1022 '"/>
								</xsl:when>
								<xsl:when test="UDACountryName='BRAZIL'">
									<xsl:value-of select="'ITAUBRSPINT'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='AUSTRALIA'">
									<xsl:value-of select="'HKBAAU2S'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='SWEDEN'">
									<xsl:value-of select="'ESSESESS'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='SPAIN'">
									<xsl:value-of select="'PARBESMX'"/>
								</xsl:when>
								<xsl:when test="UDACountryName='SWITZERLAND'">
									<xsl:value-of select="'MSNYUS33ISL'"/>
								</xsl:when>
									<xsl:when test="CurrencySymbol='USD'">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ClearingBrokerAgentID>

							<ExposureTypeIndicator>
								<xsl:value-of select="''"/>
							</ExposureTypeIndicator>

							<NetMovementIndicator>
								<xsl:value-of select="''"/>
							</NetMovementIndicator>

							<NetMovementAmount>
								<xsl:value-of select="''"/>
							</NetMovementAmount>

							<IntermediaryIDType>
								<xsl:value-of select="''"/>
							</IntermediaryIDType>

							<IntermediaryID>
								<xsl:value-of select="''"/>
							</IntermediaryID>

							<AcctWithInstitutionIDType>
								<xsl:value-of select="''"/>
							</AcctWithInstitutionIDType>

							<AcctWithInstitutionID>
								<xsl:value-of select="''"/>
							</AcctWithInstitutionID>

							<PayingInstitution>
								<xsl:value-of select="''"/>
							</PayingInstitution>

							<BeneficiaryOfMoney>
								<xsl:value-of select="''"/>
							</BeneficiaryOfMoney>

							<CashAcct>
								<xsl:value-of select="''"/>
							</CashAcct>

							<CBO>
								<xsl:value-of select="''"/>
							</CBO>

							<StampDutyExemption>
								<xsl:value-of select="''"/>
							</StampDutyExemption>

							<StampCode>
								<xsl:value-of select="''"/>
							</StampCode>

							<TRADDETNarrative>
								<xsl:value-of select="''"/>
							</TRADDETNarrative>

							<FIANarrative>
								<xsl:value-of select="''"/>
							</FIANarrative>

							<Processing>
								<xsl:value-of select="''"/>
							</Processing>

							<Reference>
								<xsl:value-of select="''"/>
							</Reference>

							<Clearing>
								<xsl:value-of select="''"/>
							</Clearing>

							<Broker>
								<xsl:value-of select="''"/>
							</Broker>

							<Account>
								<xsl:value-of select="''"/>
							</Account>

							<Restrictions>
								<xsl:value-of select="''"/>
							</Restrictions>

							<RepoTermOpenInd>
								<xsl:value-of select="''"/>
							</RepoTermOpenInd>

							<RepoTermDate>
								<xsl:value-of select="''"/>
							</RepoTermDate>

							<RepoRateType>
								<xsl:value-of select="''"/>
							</RepoRateType>

							<RepoRate>
								<xsl:value-of select="''"/>
							</RepoRate>

							<RepoReference>
								<xsl:value-of select="''"/>
							</RepoReference>

							<RepoTotalTermAmt>
								<xsl:value-of select="''"/>
							</RepoTotalTermAmt>

							<RepoAccrueAmt>
								<xsl:value-of select="''"/>
							</RepoAccrueAmt>

							<RepoTotalCollCnt>
								<xsl:value-of select="''"/>
							</RepoTotalCollCnt>

							<RepoCollNumb>
								<xsl:value-of select="''"/>
							</RepoCollNumb>

							<RepoTypeInd>
								<xsl:value-of select="''"/>
							</RepoTypeInd>

							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>
						</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						<xsl:if test ="number(OldExecutedQuantity)">
							<ThirdPartyFlatFileDetail>

								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>

								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>

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


								<TransactionType>
								<xsl:choose>
									<xsl:when test="contains(OldSide,'Buy')">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="contains(OldSide,'Sell')">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
								</TransactionType>

								
								<MessageFunction>
									<xsl:value-of select="'CANC'"/>
								</MessageFunction>

								<TransactionReference>
									<xsl:value-of select="EntityID"/>
								</TransactionReference>

								<RelatedReferenceNumber>
									<xsl:value-of select="''"/>
								</RelatedReferenceNumber>

								<xsl:variable name="PB_NAME" select="'US Bank'"/>

								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="'CFFW'"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>


								<xsl:variable name="varAccountName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<FundID>
									<xsl:value-of select="$varAccountName"/>
								</FundID>


								<TradeDate>
									<xsl:value-of select="concat(substring-before($OldTradeDate,'/'),'/',substring-before(substring-after($OldTradeDate,'/'),'/'),'/',substring-after(substring-after($OldTradeDate,'/'),'/'))"/>
								</TradeDate>

								<SettlementDate>
									<xsl:value-of select="concat(substring-before($OldSettleDate,'/'),'/',substring-before(substring-after($OldSettleDate,'/'),'/'),'/',substring-after(substring-after($OldSettleDate,'/'),'/'))"/>
								</SettlementDate>

								<LateDeliveryDate>
									<xsl:value-of select="''"/>
								</LateDeliveryDate>

								<SecurityIDType>
									<xsl:choose>
										<xsl:when test="contains(Asset,'EquityOption')">
											<xsl:value-of select="'TS'"/>
										</xsl:when>
										<xsl:when test="contains(Asset,'Future')">
											<xsl:value-of select="'TS'"/>
										</xsl:when>
										<xsl:when test="SEDOL!='*'">
											<xsl:value-of select="'GB'"/>
										</xsl:when>
										<xsl:when test="CUSIP!='*'">
											<xsl:value-of select="'US'"/>
										</xsl:when>
										<xsl:when test="ISIN!='*'">
											<xsl:value-of select="'ISIN'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityIDType>

								<SecurityID>
									<xsl:choose>
										<xsl:when test="contains(Asset,'EquityOption')">
											<xsl:value-of select="OSIOptionSymbol"/>
										</xsl:when>
										<xsl:when test="contains(Asset,'Future')">
											<xsl:value-of select="BBCode"/>
										</xsl:when>
										<xsl:when test="SEDOL!='*'">
											<xsl:value-of select="SEDOL"/>
										</xsl:when>
										<xsl:when test="CUSIP!='*'">
											<xsl:value-of select="CUSIP"/>
										</xsl:when>

										<xsl:when test="ISIN!='*'">
											<xsl:value-of select="ISIN"/>
										</xsl:when>
										<xsl:when test="Symbol!='*'">
											<xsl:value-of select="Symbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityID>


								<SecurityDescription>
									<xsl:value-of select="CompanyName"/>
								</SecurityDescription>

								<SecurityType>
									<xsl:choose>

										<xsl:when test="contains(Asset,'Option')">
											<xsl:value-of select="'OPT'"/>
										</xsl:when>
										<xsl:when test="UDAAssetName='ETF'">
											<xsl:value-of select="'ETF'"/>
										</xsl:when>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'CS'"/>
										</xsl:when>
										<xsl:when test="contains(Asset,'Future')">
											<xsl:value-of select="'FUT'"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="'CORP'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityType>

								<CurrencyOfDenomination>
									<xsl:value-of select="''"/>
								</CurrencyOfDenomination>

								<OptionStyle>
									<xsl:value-of select="''"/>
								</OptionStyle>

								<OptionType>
									<xsl:value-of select="''"/>
								</OptionType>

								<ContractSize>
									<xsl:value-of select="''"/>
								</ContractSize>

								<StrikePrice>
									<xsl:value-of select="''"/>
								</StrikePrice>

								<ExpirationDate>
									<xsl:value-of select="''"/>
								</ExpirationDate>

								<UnderlyingSecurityIDType>
									<xsl:value-of select="''"/>
								</UnderlyingSecurityIDType>

								<UnderlyingSecurityID>
									<xsl:value-of select="''"/>									
								</UnderlyingSecurityID>

								<UnderlyingSecurityDesc>
									<xsl:value-of select="''"/>									
								</UnderlyingSecurityDesc>

								<MaturityDate>
									<xsl:value-of select="''"/>
								</MaturityDate>

								<IssueDate>
									<xsl:value-of select="''"/>
								</IssueDate>

								<InterestRate>									
									<xsl:value-of select="''"/>						

								</InterestRate>

								<OriginalFace>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="OldExecutedQuantity * AssetMultiplier"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</OriginalFace>


								<Quantity>
									<xsl:choose>
										<xsl:when test="number(OldExecutedQuantity)">
											<xsl:value-of select="OldExecutedQuantity"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</Quantity>

								<TradeCurrency>
									<xsl:value-of select="OldSettlCurrency"/>
								</TradeCurrency>

								<DealPriceCode>
									<xsl:value-of select="'ACTU'"/>
								</DealPriceCode>

								<xsl:variable name="AvgPrice2">
									<xsl:choose>
										<xsl:when test="OldSettlCurrFxRate=0">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='M'">
											<xsl:value-of select="OldAvgPrice * OldSettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='D'">
											<xsl:value-of select="OldAvgPrice div OldSettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								

								<DealPrice>
									<xsl:choose>
										<xsl:when test="number($AvgPrice2)">
											<xsl:value-of select="format-number($AvgPrice2,'0.######')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</DealPrice>


								<xsl:variable name="Principal2" select="OldExecutedQuantity * OldAvgPrice * AssetMultiplier"/>
								

								<xsl:variable name="Principal3">
									<xsl:choose>
										<xsl:when test="OldSettlCurrFxRate=0">
											<xsl:value-of select="$Principal2"/>
										</xsl:when>
										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='M'">
											<xsl:value-of select="$Principal2 * OldSettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='D'">
											<xsl:value-of select="$Principal2 div OldSettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<PrincipalAmount>
									<xsl:choose>
										<xsl:when test="number($Principal3)">
											<xsl:value-of select="format-number($Principal3,'0.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</PrincipalAmount>

								<xsl:variable name="varCommission2">
									<xsl:value-of select="(OldCommission + OldSoftCommission)"/>
								</xsl:variable>

	

								<xsl:variable name="varCommission3">
									<xsl:choose>
										<xsl:when test="OldSettlCurrFxRate=0">
											<xsl:value-of select="$varCommission2"/>
										</xsl:when>
										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varCommission2 * OldSettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varCommission2 div OldSettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<CommissionsAmount>
									<xsl:choose>
										<xsl:when test="number($varCommission3)">
											<xsl:value-of select="format-number($varCommission3,'0.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</CommissionsAmount>

								<xsl:variable name="OldOtherFees">
									<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
								</xsl:variable>


								<xsl:variable name="OtherFees3">
									<xsl:choose>
										<xsl:when test="OldSettlCurrFxRate=0">
											<xsl:value-of select="$OldOtherFees"/>
										</xsl:when>
										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='M'">
											<xsl:value-of select="$OldOtherFees * OldSettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='D'">
											<xsl:value-of select="$OldOtherFees div OldSettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<ChargesFeesAmount>
									<xsl:choose>
										<xsl:when test="number($OtherFees3)">
											<xsl:value-of select="format-number($OtherFees3,'0.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</ChargesFeesAmount>

								<OtherAmount>
									<xsl:value-of select="''"/>
								</OtherAmount>

								<xsl:variable name="AccruedInterest2">
									<xsl:choose>
										<xsl:when test="OldSettlCurrFxRate=0">
											<xsl:value-of select="OldAccruedInterest"/>
										</xsl:when>
										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='M'">
											<xsl:value-of select="OldAccruedInterest * OldSettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='D'">
											<xsl:value-of select="OldAccruedInterest div OldSettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<AccruedInterestAmount>
									<xsl:choose>
										<xsl:when test="number($AccruedInterest2)">
											<xsl:value-of select="format-number($AccruedInterest2,'0.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</AccruedInterestAmount>

								<TaxesAmount>
									<xsl:value-of select="''"/>
								</TaxesAmount>

								<StampDutyExemptionAmount>
									<xsl:value-of select="''"/>
								</StampDutyExemptionAmount>

								<SettlementCurrency>
									<xsl:value-of select="OldSettlCurrency"/>
								</SettlementCurrency>

							
								<!--<xsl:variable name="varNetAmmount">
									<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice) + OldCommissionCharged + OldSoftCommissionCharged + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
									<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice) + $OtherFees"/>
								</xsl:variable>-->





								<xsl:variable name="varNetAmmount">
									<xsl:choose>
										<xsl:when test="contains(Side,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
										</xsl:when>
										<xsl:when test="contains(Side,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>


								<xsl:variable name = "varNETAMNT">
									<xsl:choose>
										<xsl:when test="OldSettlCurrFxRate=0">
											<xsl:value-of select="$varNetAmmount"/>
										</xsl:when>
										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varNetAmmount * OldSettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="OldSettlCurrFxRate!=0 and OldSettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varNetAmmount div OldSettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<SettlementAmount>
									<xsl:value-of select="format-number($varNETAMNT,'0.##')"/>
								</SettlementAmount>


								<TransactionSubType>
									<xsl:value-of select="'TRAD'"/>
								</TransactionSubType>

								<SettlementTransactionConditionIndicator>
									<xsl:choose>

										<xsl:when test="OldSide='Sell short' or OldSide='Sell to Open'">
											<xsl:value-of select="'SHOR'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'BUTC'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SettlementTransactionConditionIndicator>


								<SettlementTransactionConditionIndicator2>
									<xsl:choose>

										<xsl:when test="OldSide='Sell short' or OldSide='Sell to Open'">
											<xsl:value-of select="'RPTO'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'RPTO'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SettlementTransactionConditionIndicator2>

								<ProcessingIndicator>
									<xsl:choose>
										<xsl:when test="contains(Asset,'Future')">
											<xsl:choose>
												<xsl:when test="contains(OldSide,'Open')">
													<xsl:value-of select="'OPEP'"/>
												</xsl:when>
												<xsl:when test="contains(OldSide,'Close')">
													<xsl:value-of select="'CLOP'"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select="''"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="contains(Asset,'EquityOption')">
											<xsl:choose>
												<xsl:when test="contains(OldSide,'Open')">
													<xsl:value-of select="'OPEP'"/>
												</xsl:when>
												<xsl:when test="contains(OldSide,'Close')">
													<xsl:value-of select="'CLOP'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="''"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</ProcessingIndicator>

								<TrackingIndicator>
									<xsl:value-of select="''"/>
								</TrackingIndicator>


								<SettlementLocation>
									<xsl:choose>
										<xsl:when test="CurrencySymbol='USD'">
											<xsl:value-of select="'DTCYUS33'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='AUSTRIA'">
											<xsl:value-of select="'OCSDATWWXXX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='GERMANY'">
											<xsl:value-of select="'DAKVDEFFXXX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='JAPAN'">
											<xsl:value-of select="'JJSDJPJT'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='UNITED KINGDOM'">
											<xsl:value-of select="'CRSTGB22'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='FRANCE'">
											<xsl:value-of select="'SICVFRPPXXX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='CANADA'">
											<xsl:value-of select="'CDSLCATT'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='DENMARK'">
											<xsl:value-of select="'VPDKDKKKXXX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='ITALY'">
											<xsl:value-of select="'MOTIITMM'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='BRAZIL'">
											<xsl:value-of select="'SSCSBRR1'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='AUSTRALIA'">
											<xsl:value-of select="'CAETAU21'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='SWEDEN'">
											<xsl:value-of select="'VPCSSESS'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='SPAIN'">
											<xsl:value-of select="'IBRCESMMXXX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='SWITZERLAND'">
											<xsl:value-of select="'INSECHZZXXX'"/>
										</xsl:when>


										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SettlementLocation>

								<PlaceOfTrade>
									<xsl:value-of select="''"/>
								</PlaceOfTrade>

								<PlaceOfSafekeeping>
									<xsl:value-of select="''"/>
								</PlaceOfSafekeeping>

								<FXContraCurrency>
									<xsl:value-of select="''"/>
								</FXContraCurrency>

								<FXOrderCXLIndicator>
									<xsl:value-of select="''"/>
								</FXOrderCXLIndicator>


								<ExecutingBrokerIDType>
									<xsl:choose>
										<xsl:when test="CurrencySymbol='USD'">
											<xsl:value-of select="'DTCYID'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'BIC'"/>
										</xsl:otherwise>
									</xsl:choose>
								</ExecutingBrokerIDType>

								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
								<xsl:variable name="THIRDPARTY_BROKER">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
								</xsl:variable>


								<ExecutingBrokerID>
									<xsl:choose>
										<xsl:when test="CurrencySymbol!='USD'">
											<xsl:value-of select="'MSNYUS33'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="$THIRDPARTY_BROKER!= ''">
													<xsl:value-of select="$THIRDPARTY_BROKER"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>

									</xsl:choose>
								</ExecutingBrokerID>

								<ExecutingBrokerAcct>
									<xsl:value-of select="''"/>
								</ExecutingBrokerAcct>

								<ClearingBrokerAgentIDType>
									<xsl:choose>
										<xsl:when test="CurrencySymbol='USD'">
											<xsl:value-of select="'DTCYID'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='UNITED KINGDOM'">
											<xsl:value-of select="'CRST'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'BIC'"/>
										</xsl:otherwise>
									</xsl:choose>



								</ClearingBrokerAgentIDType>


								<xsl:variable name="THIRDPARTY_COUNTERPARTY">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
								</xsl:variable>

								<xsl:variable name="BrokerName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
											<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<ClearingBrokerAgentID>
									<xsl:choose>
										<xsl:when test="UDACountryName='CANADA'">
											<xsl:value-of select="'ROYCCAT2'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='JAPAN'">
											<xsl:value-of select="'MSTKJPJX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='UNITED KINGDOM'">
											<xsl:value-of select="'50708'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='AUSTRIA'">
											<xsl:value-of select="'BKAUATWW'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='GERMANY'">
											<xsl:value-of select="'MSFFDEFX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='FRANKURT'">
											<xsl:value-of select="'MSFFDEFX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='FRANCE'">
											<xsl:value-of select="'PARBFRPPXXX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='DENMARK'">
											<xsl:value-of select="'ESSEDKKK'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='ITALY'">
											<xsl:value-of select="'CITIITM1022 '"/>
										</xsl:when>
										<xsl:when test="UDACountryName='BRAZIL'">
											<xsl:value-of select="'ITAUBRSPINT'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='AUSTRALIA'">
											<xsl:value-of select="'HKBAAU2S'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='SWEDEN'">
											<xsl:value-of select="'ESSESESS'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='SPAIN'">
											<xsl:value-of select="'PARBESMX'"/>
										</xsl:when>
										<xsl:when test="UDACountryName='SWITZERLAND'">
											<xsl:value-of select="'MSNYUS33ISL'"/>
										</xsl:when>
										<xsl:when test="CurrencySymbol='USD'">
											<xsl:value-of select="$THIRDPARTY_BROKER"/>
										</xsl:when>

										<xsl:otherwise>

											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>


								</ClearingBrokerAgentID>

								<ExposureTypeIndicator>
									<xsl:value-of select="''"/>
								</ExposureTypeIndicator>

								<NetMovementIndicator>
									<xsl:value-of select="''"/>
								</NetMovementIndicator>

								<NetMovementAmount>
									<xsl:value-of select="''"/>
								</NetMovementAmount>

								<IntermediaryIDType>
									<xsl:value-of select="''"/>
								</IntermediaryIDType>

								<IntermediaryID>
									<xsl:value-of select="''"/>
								</IntermediaryID>

								<AcctWithInstitutionIDType>
									<xsl:value-of select="''"/>
								</AcctWithInstitutionIDType>

								<AcctWithInstitutionID>
									<xsl:value-of select="''"/>
								</AcctWithInstitutionID>

								<PayingInstitution>
									<xsl:value-of select="''"/>
								</PayingInstitution>

								<BeneficiaryOfMoney>
									<xsl:value-of select="''"/>
								</BeneficiaryOfMoney>

								<CashAcct>
									<xsl:value-of select="''"/>
								</CashAcct>

								<CBO>
									<xsl:value-of select="''"/>
								</CBO>

								<StampDutyExemption>
									<xsl:value-of select="''"/>
								</StampDutyExemption>

								<StampCode>
									<xsl:value-of select="''"/>
								</StampCode>

								<TRADDETNarrative>
									<xsl:value-of select="''"/>
								</TRADDETNarrative>

								<FIANarrative>
									<xsl:value-of select="''"/>
								</FIANarrative>

								<Processing>
									<xsl:value-of select="''"/>
								</Processing>

								<Reference>
									<xsl:value-of select="''"/>
								</Reference>

								<Clearing>
									<xsl:value-of select="''"/>
								</Clearing>

								<Broker>
									<xsl:value-of select="''"/>
								</Broker>

								<Account>
									<xsl:value-of select="''"/>
								</Account>

								<Restrictions>
									<xsl:value-of select="''"/>
								</Restrictions>

								<RepoTermOpenInd>
									<xsl:value-of select="''"/>
								</RepoTermOpenInd>

								<RepoTermDate>
									<xsl:value-of select="''"/>
								</RepoTermDate>

								<RepoRateType>
									<xsl:value-of select="''"/>
								</RepoRateType>

								<RepoRate>
									<xsl:value-of select="''"/>
								</RepoRate>

								<RepoReference>
									<xsl:value-of select="''"/>
								</RepoReference>

								<RepoTotalTermAmt>
									<xsl:value-of select="''"/>
								</RepoTotalTermAmt>

								<RepoAccrueAmt>
									<xsl:value-of select="''"/>
								</RepoAccrueAmt>

								<RepoTotalCollCnt>
									<xsl:value-of select="''"/>
								</RepoTotalCollCnt>

								<RepoCollNumb>
									<xsl:value-of select="''"/>
								</RepoCollNumb>

								<RepoTypeInd>
									<xsl:value-of select="''"/>
								</RepoTypeInd>
								
								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>							
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="'Allocated'"/>
					</TaxLotState>

					<TransactionType>
						<xsl:choose>
							
							<xsl:when test="contains(Side,'Buy')">
								<xsl:value-of select="'BUY'"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="'SELL'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TransactionType>

					<MessageFunction>
						<xsl:value-of select="'NEWM'"/>
					</MessageFunction>

					<TransactionReference>
						<xsl:value-of select="substring(AmendTaxLotId1,1,15)"/>
					</TransactionReference>

					<RelatedReferenceNumber>
						<xsl:value-of select="EntityID"/>
					</RelatedReferenceNumber>

					<xsl:variable name="PB_NAME" select="'US Bank'"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="'CFFW'"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>


					<xsl:variable name="varAccountName">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<FundID>
						<xsl:value-of select="$varAccountName"/>
					</FundID>

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
						<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
					</TradeDate>


					<SettlementDate>
						<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
					</SettlementDate>

					<LateDeliveryDate>
						<xsl:value-of select="''"/>
					</LateDeliveryDate>

					<SecurityIDType>
						<xsl:choose>
							<xsl:when test="contains(Asset,'EquityOption')">
								<xsl:value-of select="'TS'"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:value-of select="'TS'"/>
							</xsl:when>
							<xsl:when test="SEDOL!='*'">
								<xsl:value-of select="'GB'"/>
							</xsl:when>
							<xsl:when test="CUSIP!='*'">
								<xsl:value-of select="'US'"/>
							</xsl:when>
							<xsl:when test="ISIN!='*'">
								<xsl:value-of select="'ISIN'"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityIDType>

					<SecurityID>
						<xsl:choose>
							<xsl:when test="contains(Asset,'EquityOption')">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test="SEDOL!='*'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:when test="CUSIP!='*'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
						
							<xsl:when test="ISIN!='*'">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:when test="Symbol!='*'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>


					<SecurityDescription>
						<xsl:value-of select="CompanyName"/>
					</SecurityDescription>

					<SecurityType>
						<xsl:choose>
						
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="'OPT'"/>
							</xsl:when>
							<xsl:when test="UDAAssetName='ETF'">
								<xsl:value-of select="'ETF'"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:value-of select="'FUT'"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="'CORP'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityType>

					<CurrencyOfDenomination>						
								<xsl:value-of select="''"/>							
					</CurrencyOfDenomination>

					<OptionStyle>
						<xsl:value-of select="''"/>
					</OptionStyle>

					<OptionType>
						<xsl:value-of select="''"/>
					</OptionType>

					<ContractSize>
						<xsl:value-of select="''"/>
					</ContractSize>

					<StrikePrice>						
								<xsl:value-of select="''"/>							
					</StrikePrice>

					<ExpirationDate>						
								<xsl:value-of select="''"/>							
					</ExpirationDate>

					<UnderlyingSecurityIDType>						
							<xsl:value-of select="''"/>			
					</UnderlyingSecurityIDType>

					<UnderlyingSecurityID>
						<xsl:value-of select="''"/>						
					</UnderlyingSecurityID>

					<UnderlyingSecurityDesc>
						<xsl:value-of select="''"/>						
					</UnderlyingSecurityDesc>

					<MaturityDate>					
								<xsl:value-of select="''"/>							
					</MaturityDate>
					<IssueDate>
						<xsl:value-of select="''"/>
					</IssueDate>

					<InterestRate>			

						<xsl:value-of select="''"/>					

					</InterestRate>

					<OriginalFace>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="OrderQty * AssetMultiplier"/>
							</xsl:when>
							<xsl:otherwise>
						<xsl:value-of select="''"/>
						</xsl:otherwise>
						</xsl:choose>
					</OriginalFace>


					<Quantity>
						<xsl:choose>
							<xsl:when test="number(OrderQty)">
								<xsl:value-of select="OrderQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<TradeCurrency>
						<xsl:value-of select="SettlCurrency"/>
					</TradeCurrency>

					<DealPriceCode>
						<xsl:value-of select="'ACTU'"/>
					</DealPriceCode>

					<xsl:variable name="AvgPrice3">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="AvgPrice"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<DealPrice>
						<xsl:choose>
							<xsl:when test="number($AvgPrice3)">
								<xsl:value-of select="format-number($AvgPrice3,'0.######')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</DealPrice>


					<xsl:variable name="Principal4" select="OrderQty * AvgPrice * AssetMultiplier"/>



					<xsl:variable name="Principal5">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$Principal4"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$Principal4 * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$Principal4 div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<PrincipalAmount>
						<xsl:choose>
							<xsl:when test="number($Principal5)">
								<xsl:value-of select="format-number($Principal5,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</PrincipalAmount>
					
					<xsl:variable name="varCommission4">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<xsl:variable name="varCommission5">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$varCommission4"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$varCommission4 * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$varCommission4 div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<CommissionsAmount>
						<xsl:choose>
							<xsl:when test="number($varCommission5)">
								<xsl:value-of select="format-number($varCommission5,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionsAmount>
					


					<xsl:variable name="OtherFees4">
						<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
					</xsl:variable>

					


					<xsl:variable name="OtherFees5">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$OtherFees4"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$OtherFees4 * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$OtherFees4 div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<ChargesFeesAmount>
						<xsl:choose>
							<xsl:when test="number($OtherFees5)">
								<xsl:value-of select="format-number($OtherFees5,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</ChargesFeesAmount>

					<OtherAmount>
						<xsl:value-of select="''"/>
					</OtherAmount>

					<xsl:variable name="AccruedInterest4">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="AccruedInterest"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="AccruedInterest * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="AccruedInterest div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<AccruedInterestAmount>
						<xsl:choose>
							<xsl:when test="number($AccruedInterest4)">
								<xsl:value-of select="format-number($AccruedInterest4,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccruedInterestAmount>

					<TaxesAmount>
						<xsl:value-of select="''"/>
					</TaxesAmount>

					<StampDutyExemptionAmount>
						<xsl:value-of select="''"/>
					</StampDutyExemptionAmount>

					<SettlementCurrency>
						<xsl:value-of select="SettlCurrency"/>
					</SettlementCurrency>					



					<xsl:variable name = "NETAMNT">
						<xsl:choose>
							<xsl:when test="SettlCurrFxRate=0">
								<xsl:value-of select="$varNetamount"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="$varNetamount * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="$varNetamount div SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SettlementAmount>
						<xsl:value-of select="format-number($NETAMNT,'0.##')"/>
					</SettlementAmount>


					<TransactionSubType>
						<xsl:value-of select="'TRAD'"/>
					</TransactionSubType>

					<SettlementTransactionConditionIndicator>
						<xsl:choose>

							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SHOR'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BUTC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettlementTransactionConditionIndicator>


					<SettlementTransactionConditionIndicator2>
						<xsl:choose>
							
								<xsl:when test="Side='Sell short' or Side='Sell to Open'">
									<xsl:value-of select="'RPTO'"/>
								</xsl:when>
								<xsl:when test="Side='Buy to Close'">
									<xsl:value-of select="'RPTO'"/>
								</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettlementTransactionConditionIndicator2>

					<ProcessingIndicator>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:choose>
									<xsl:when test="contains(Side,'Open')">
										<xsl:value-of select="'OPEP'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Close')">
										<xsl:value-of select="'CLOP'"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="contains(Asset,'EquityOption')">
								<xsl:choose>
									<xsl:when test="contains(Side,'Open')">
										<xsl:value-of select="'OPEP'"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Close')">
										<xsl:value-of select="'CLOP'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ProcessingIndicator>

					<TrackingIndicator>
						<xsl:value-of select="''"/>
					</TrackingIndicator>


					<SettlementLocation>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="'DTCYUS33'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRIA'">
								<xsl:value-of select="'OCSDATWWXXX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='GERMANY'">
								<xsl:value-of select="'DAKVDEFFXXX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'JJSDJPJT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='UNITED KINGDOM'">
								<xsl:value-of select="'CRSTGB22'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='FRANCE'">
								<xsl:value-of select="'SICVFRPPXXX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='CANADA'">
								<xsl:value-of select="'CDSLCATT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='DENMARK'">
								<xsl:value-of select="'VPDKDKKKXXX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='ITALY'">
								<xsl:value-of select="'MOTIITMM'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='BRAZIL'">
								<xsl:value-of select="'SSCSBRR1'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRALIA'">
								<xsl:value-of select="'CAETAU21'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWEDEN'">
								<xsl:value-of select="'VPCSSESS'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SPAIN'">
								<xsl:value-of select="'IBRCESMMXXX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWITZERLAND'">
								<xsl:value-of select="'INSECHZZXXX'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettlementLocation>

					<PlaceOfTrade>
						<xsl:value-of select="''"/>
					</PlaceOfTrade>

					<PlaceOfSafekeeping>
						<xsl:value-of select="''"/>
					</PlaceOfSafekeeping>

					<FXContraCurrency>
						<xsl:value-of select="''"/>
					</FXContraCurrency>

					<FXOrderCXLIndicator>
						<xsl:value-of select="''"/>
					</FXOrderCXLIndicator>


					<ExecutingBrokerIDType>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="'DTCYID'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'BIC'"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutingBrokerIDType>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
					<xsl:variable name="THIRDPARTY_BROKER">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
					</xsl:variable>					
					<ExecutingBrokerID>
						<xsl:choose>
							<xsl:when test="CurrencySymbol!='USD'">
								<xsl:value-of select="'MSNYUS33'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>

						</xsl:choose>
					</ExecutingBrokerID>

					<ExecutingBrokerAcct>
						<xsl:value-of select="''"/>
					</ExecutingBrokerAcct>

					<ClearingBrokerAgentIDType>
						<xsl:choose>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="'DTCYID'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='UNITED KINGDOM'">
								<xsl:value-of select="'CRST'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="'BIC'"/>
							</xsl:otherwise>
						</xsl:choose>

					</ClearingBrokerAgentIDType>


					<xsl:variable name="THIRDPARTY_COUNTERPARTY">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="BrokerName">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<ClearingBrokerAgentID>
						<xsl:choose>
							<xsl:when test="UDACountryName='CANADA'">
								<xsl:value-of select="'ROYCCAT2'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='JAPAN'">
								<xsl:value-of select="'MSTKJPJX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='UNITED KINGDOM'">
								<xsl:value-of select="'50708'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRIA'">
								<xsl:value-of select="'BKAUATWW'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='GERMANY'">
								<xsl:value-of select="'MSFFDEFX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='FRANKURT'">
								<xsl:value-of select="'MSFFDEFX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='FRANCE'">
								<xsl:value-of select="'PARBFRPPXXX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='DENMARK'">
								<xsl:value-of select="'ESSEDKKK'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='ITALY'">
								<xsl:value-of select="'CITIITM1022 '"/>
							</xsl:when>
							<xsl:when test="UDACountryName='BRAZIL'">
								<xsl:value-of select="'ITAUBRSPINT'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='AUSTRALIA'">
								<xsl:value-of select="'HKBAAU2S'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWEDEN'">
								<xsl:value-of select="'ESSESESS'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SPAIN'">
								<xsl:value-of select="'PARBESMX'"/>
							</xsl:when>
							<xsl:when test="UDACountryName='SWITZERLAND'">
								<xsl:value-of select="'MSNYUS33ISL'"/>
							</xsl:when>
							<xsl:when test="CurrencySymbol='USD'">
								<xsl:value-of select="$THIRDPARTY_BROKER"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
					
					</ClearingBrokerAgentID>

					<ExposureTypeIndicator>
						<xsl:value-of select="''"/>
					</ExposureTypeIndicator>

					<NetMovementIndicator>
						<xsl:value-of select="''"/>
					</NetMovementIndicator>

					<NetMovementAmount>
						<xsl:value-of select="''"/>
					</NetMovementAmount>

					<IntermediaryIDType>
						<xsl:value-of select="''"/>
					</IntermediaryIDType>

					<IntermediaryID>
						<xsl:value-of select="''"/>
					</IntermediaryID>

					<AcctWithInstitutionIDType>
						<xsl:value-of select="''"/>
					</AcctWithInstitutionIDType>

					<AcctWithInstitutionID>
						<xsl:value-of select="''"/>
					</AcctWithInstitutionID>

					<PayingInstitution>
						<xsl:value-of select="''"/>
					</PayingInstitution>

					<BeneficiaryOfMoney>
						<xsl:value-of select="''"/>
					</BeneficiaryOfMoney>

					<CashAcct>
						<xsl:value-of select="''"/>
					</CashAcct>

					<CBO>
						<xsl:value-of select="''"/>
					</CBO>

					<StampDutyExemption>
						<xsl:value-of select="''"/>
					</StampDutyExemption>

					<StampCode>
						<xsl:value-of select="''"/>
					</StampCode>

					<TRADDETNarrative>
						<xsl:value-of select="''"/>
					</TRADDETNarrative>

					<FIANarrative>
						<xsl:value-of select="''"/>
					</FIANarrative>

					<Processing>
						<xsl:value-of select="''"/>
					</Processing>

					<Reference>
						<xsl:value-of select="''"/>
					</Reference>

					<Clearing>
						<xsl:value-of select="''"/>
					</Clearing>

					<Broker>
						<xsl:value-of select="''"/>
					</Broker>

					<Account>
						<xsl:value-of select="''"/>
					</Account>

					<Restrictions>
						<xsl:value-of select="''"/>
					</Restrictions>

					<RepoTermOpenInd>
						<xsl:value-of select="''"/>
					</RepoTermOpenInd>

					<RepoTermDate>
						<xsl:value-of select="''"/>
					</RepoTermDate>

					<RepoRateType>
						<xsl:value-of select="''"/>
					</RepoRateType>

					<RepoRate>
						<xsl:value-of select="''"/>
					</RepoRate>

					<RepoReference>
						<xsl:value-of select="''"/>
					</RepoReference>

					<RepoTotalTermAmt>
						<xsl:value-of select="''"/>
					</RepoTotalTermAmt>

					<RepoAccrueAmt>
						<xsl:value-of select="''"/>
					</RepoAccrueAmt>

					<RepoTotalCollCnt>
						<xsl:value-of select="''"/>
					</RepoTotalCollCnt>

					<RepoCollNumb>
						<xsl:value-of select="''"/>
					</RepoCollNumb>

					<RepoTypeInd>
						<xsl:value-of select="''"/>
					</RepoTypeInd>


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

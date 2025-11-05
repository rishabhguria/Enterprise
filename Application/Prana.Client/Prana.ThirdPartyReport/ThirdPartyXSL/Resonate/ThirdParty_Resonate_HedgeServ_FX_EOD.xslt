<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthNamefromCode">
		<xsl:param name="MonthCode"/>

		<xsl:choose>
			<xsl:when test="$MonthCode='1'">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='2'">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='3'">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='4'">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='5'">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='6'">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='7'">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='8'">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='9'">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='10'">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='11'">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$MonthCode='12'">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Conversion">
		<xsl:param name="Value"/>
		<xsl:param name="Curr"/>

		<xsl:choose>
			<xsl:when test="Asset='Equity' and IsSwapped='true'">
				<xsl:choose>
					<xsl:when test="FXConversionMethodOperator_Taxlot='M'">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value div FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select="$Value"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>



			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test="contains(AccountName,'Star')">

				<!--<xsl:if test="FundAccountNo!='038CAAPD2' and FundAccountNo!='0581CHFY9' and FundAccountNo!='06178WSG8'">-->

					<ThirdPartyFlatFileDetail>

						<!--for system internal use-->

						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<!--for system use only-->

						<!--<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>-->

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<xsl:variable name="PB_NAME" select="'HedgeServ'"/>

						<xsl:variable name="flag">
							<xsl:choose>
								<xsl:when test="contains(Symbol,'EUR') and LeadCurrencyName='EUR'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'EUR') and VsCurrencyName='EUR'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'GBP') and LeadCurrencyName='GBP'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'GBP') and VsCurrencyName='GBP'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'AUD') and LeadCurrencyName='AUD'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'AUD') and VsCurrencyName='AUD'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'NZD') and LeadCurrencyName='NZD'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'NZD') and VsCurrencyName='NZD'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'USD') and LeadCurrencyName='USD'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'USD') and VsCurrencyName='USD'">
									<xsl:value-of select ="'0'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="InsType">
							<xsl:choose>
								<xsl:when test="contains(Asset,'EquityOption')">
									<xsl:value-of select="'Option'"/>
								</xsl:when>

								<xsl:when test="Asset ='FX'">
									<xsl:value-of select="'FX'"/>
								</xsl:when>
								<xsl:when test="Asset ='FXForward'">
									<xsl:value-of select="'FX Forward'"/>
								</xsl:when>
								
								<xsl:when test="contains(Asset,'Equity') or (Asset='Equity' and IsSwapped='true')">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<InstrumentType>
							<xsl:value-of select="$InsType"/>
						</InstrumentType>

						<xsl:variable name="InsSubType">
							<xsl:choose>								
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:value-of select="'CFD'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<InstrumentSubType>
							<xsl:value-of select="$InsSubType"/>
						</InstrumentSubType>

						<FXType>
							<xsl:choose>
								<xsl:when test="Asset='FX'">
									<xsl:value-of select="'N'"/>
								</xsl:when>
								<xsl:when test="Asset ='FXForward'">
									<xsl:value-of select="'F'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXType>

						<HSTradeID>
							<xsl:value-of select="concat(EntityID,'c')"/>
						</HSTradeID>

						<TransactionType>
							<xsl:choose>
								<xsl:when test="TaxLotState='Allocated'">
									<xsl:value-of select="'N'"/>
								</xsl:when>
								<xsl:when test="TaxLotState='Amended'">
									<xsl:value-of select="'E'"/>
								</xsl:when>
								<xsl:when test="TaxLotState='Deleted'">
									<xsl:value-of select="'V'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionType>

						<Action>
							<xsl:choose>
								<xsl:when test="$flag='1'">
									<xsl:choose>
										<xsl:when test="Side='Buy' or Side='Buy to Open'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
											<xsl:value-of select="'BC'"/>
										</xsl:when>
										<xsl:when test="Side='Sell' or Side='Sell to Close'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
										<xsl:when test="Side='Sell short' or Side='Sell to Open'">
											<xsl:value-of select="'SS'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$flag='0'">
									<xsl:choose>
										<xsl:when test="Side='Buy' or Side='Buy to Open'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
										<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
											<xsl:value-of select="'SS'"/>
										</xsl:when>
										<xsl:when test="Side='Sell' or Side='Sell to Close'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="Side='Sell short' or Side='Sell to Open'">
											<xsl:value-of select="'BC'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>

						</Action>

						<StreetIDType>
							<xsl:choose>
								<xsl:when test="contains(Asset,'FX')">
									<xsl:value-of select="'Z'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'B'"/>
								</xsl:otherwise>
							</xsl:choose>
						</StreetIDType>

						<xsl:variable name="Symbol_FX">
							<xsl:choose>
								<xsl:when test="contains(Symbol,'EUR') and LeadCurrencyName='EUR'">
									<xsl:value-of select ="concat(LeadCurrencyName,'/',VsCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'EUR') and VsCurrencyName='EUR'">
									<xsl:value-of select ="concat(VsCurrencyName,'/',LeadCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'GBP') and LeadCurrencyName='GBP'">
									<xsl:value-of select ="concat(LeadCurrencyName,'/',VsCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'GBP') and VsCurrencyName='GBP'">
									<xsl:value-of select ="concat(VsCurrencyName,'/',LeadCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'AUD') and LeadCurrencyName='AUD'">
									<xsl:value-of select ="concat(LeadCurrencyName,'/',VsCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'AUD') and VsCurrencyName='AUD'">
									<xsl:value-of select ="concat(VsCurrencyName,'/',LeadCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'NZD') and LeadCurrencyName='NZD'">
									<xsl:value-of select ="concat(LeadCurrencyName,'/',VsCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'NZD') and VsCurrencyName='NZD'">
									<xsl:value-of select ="concat(VsCurrencyName,'/',LeadCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'USD') and LeadCurrencyName='USD'">
									<xsl:value-of select ="concat(LeadCurrencyName,'/',VsCurrencyName)"/>
								</xsl:when>
								<xsl:when test="contains(Symbol,'USD') and VsCurrencyName='USD'">
									<xsl:value-of select ="concat(VsCurrencyName,'/',LeadCurrencyName)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<StreetID>
							<xsl:choose>
								<xsl:when test="Asset='EquityOption' and CurrencySymbol != 'USD'">
									<xsl:value-of select="BBCode"/>
								</xsl:when>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>
								</xsl:when>
								<xsl:when test="contains(Asset,'FX')">
									<xsl:value-of select="$Symbol_FX"/>
								</xsl:when>
								<xsl:when test="BBCode!=''">
									<xsl:value-of select="BBCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="SEDOL"/>
								</xsl:otherwise>
							</xsl:choose>
						</StreetID>

						<ClearingBroker>
							<xsl:value-of select="'Morgan Stanley'"/>
						</ClearingBroker>

						<Currency>
							<xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:value-of select="SettlCurrency"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CurrencySymbol"/>
								</xsl:otherwise>
							</xsl:choose>							
						</Currency>


						<xsl:variable name="IsOTC">
							<xsl:choose>								
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:value-of select="'Y'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'N'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<IsOTC>
							<xsl:value-of select="$IsOTC"/>
						</IsOTC>

						<QuoteType>
							<xsl:value-of select="''"/>
						</QuoteType>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number(AllocatedQty)">
									<xsl:value-of select="AllocatedQty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="Primary">
							<xsl:choose>
								<xsl:when test="contains(Asset,'FX')">
									<xsl:choose>
										<xsl:when test="$flag='1'">
											<xsl:value-of select="AllocatedQty"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="AllocatedQty*AveragePrice"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Secondary">
							<xsl:choose>
								<xsl:when test="contains(Asset,'FX')">
									<xsl:choose>
										<xsl:when test="$flag='1'">
											<xsl:value-of select="AllocatedQty*AveragePrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="AllocatedQty"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<PrimaryAmount>
							<xsl:value-of select="$Primary"/>
						</PrimaryAmount>

						<SecondaryAmount>
							<xsl:value-of select="$Secondary"/>
						</SecondaryAmount>

						<DealFxRate>
							<xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:choose>
										<xsl:when test="number(FXRate_Taxlot)">
											<xsl:value-of select="FXRate_Taxlot"/>
										</xsl:when>
										<xsl:when test="number(ForexRate)">
											<xsl:value-of select="ForexRate"/>
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
						</DealFxRate>

						<Price>							
							<xsl:call-template name="Conversion">
								<xsl:with-param name="Value" select="AveragePrice"/>
								<xsl:with-param name="Curr" select="CurrencySymbol"/>
							</xsl:call-template>
						</Price>

						<xsl:variable name="Trade">
							<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
						</xsl:variable>

						<TradeDate>
							<xsl:value-of select="$Trade"/>
						</TradeDate>

						<xsl:variable name="Settle">
							<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
						</xsl:variable>

						<SettleDate>
							<xsl:choose>
								<xsl:when test="contains(Asset,'FXForward')">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Settle"/>
								</xsl:otherwise>
							</xsl:choose>
						</SettleDate>

						<xsl:variable name="Expiry">
							<xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
						</xsl:variable>

						<ExpirationDate>
							<xsl:choose>
								<xsl:when test="contains(Asset,'FXForward')">
									<xsl:value-of select="$Expiry"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExpirationDate>

						<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

						<xsl:variable name="PB_COUNTERPARTY_NAME_A">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PBBroker"/>
						</xsl:variable>

						<xsl:variable name="Cpty">
							<xsl:choose>
								<xsl:when test="$PB_COUNTERPARTY_NAME_A!=''">
									<xsl:value-of select="$PB_COUNTERPARTY_NAME_A"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CounterParty"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<ExecutingBroker>
							<xsl:value-of select="$Cpty"/>
						</ExecutingBroker>

						<ClearerAccountNumber>
							<xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:value-of select="'038CDLWG0'"/>
								</xsl:when>
								<xsl:when test="contains(Asset,'FX')">
									<xsl:value-of select="'038CDLWG0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'038CDLWG0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearerAccountNumber>

						<Fund>
							<xsl:value-of select="'Star V Partners'"/>
						</Fund>

						
						<xsl:variable name="Comm">
							<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
						</xsl:variable>
						<Commission>							
							<xsl:call-template name="Conversion">
								<xsl:with-param name="Value" select="$Comm"/>
								<xsl:with-param name="Curr" select="CurrencySymbol"/>
							</xsl:call-template>
						</Commission>

						<CommissionType>
							<xsl:value-of select="'T'"/>
						</CommissionType>

						<ExecutionFee>
							<xsl:value-of select="''"/>
						</ExecutionFee>

						<ExecutionFeeCurrency>
							<xsl:value-of select="''"/>
						</ExecutionFeeCurrency>

						<StampDuty>
							<xsl:value-of select="''"/>
						</StampDuty>

						<ClearingFee>
							<xsl:choose>
								<xsl:when test="number(OrfFee)">
									<xsl:value-of select="OrfFee"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingFee>

						<ClearingFeeCurrency>					

							<xsl:value-of select="CurrencySymbol"/>
						</ClearingFeeCurrency>

						<SecFee>
							<xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number(StampDuty)">
											<xsl:value-of select="StampDuty"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>				
						</SecFee>

						<SecFeeCurrency>							
							<xsl:value-of select="CurrencySymbol"/>
						</SecFeeCurrency>

						<InstrumentDescription>
							<xsl:choose>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>
								</xsl:when>
								<xsl:when test="contains(Asset,'FX')">
									<xsl:value-of select="$Symbol_FX"/>
								</xsl:when>
								<xsl:when test="BBCode!=''">
									<xsl:value-of select="BBCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="SEDOL"/>
								</xsl:otherwise>
							</xsl:choose>
						</InstrumentDescription>


						<xsl:variable name="varNetAmnt">
							<xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:choose>
										<xsl:when test="Side='Buy' or Side='Buy to Close'">
											<xsl:value-of select="NetAmount - SecFee"/>
										</xsl:when>
										<xsl:when test="Side='Sell' or Side='Sell short'">
											<xsl:value-of select="NetAmount + SecFee"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="NetAmount"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="NetMoney">							
									<xsl:choose>
										<xsl:when test="number($varNetAmnt)">
											<xsl:value-of select="$varNetAmnt"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>							

						</xsl:variable>

						<xsl:variable name ="Curr" select="CurrencySymbol"/>
						<NetMoney>
							<xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped='true'">
									<xsl:choose>
										<xsl:when test="FXConversionMethodOperator_Taxlot='M'">
											<xsl:choose>
												<xsl:when test="number(FXRate_Taxlot)">
													<xsl:value-of select="$NetMoney * FXRate_Taxlot"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:choose>
														<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
															<xsl:value-of select="$NetMoney * ForexRate"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="$NetMoney div ForexRate"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="number(FXRate_Taxlot)">
													<xsl:value-of select="$NetMoney div FXRate_Taxlot"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:choose>
														<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
															<xsl:value-of select="$NetMoney * ForexRate"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="$NetMoney div ForexRate"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$NetMoney"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetMoney>

						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>

				</xsl:if>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>
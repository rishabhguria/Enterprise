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

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<!--<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxlotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxlotState>



				<Account>

					<xsl:value-of select="'Account'"/>

				</Account>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select="'SettleDate'"/>
				</SettleDate>

				<QtyParValue>

					<xsl:value-of select="'QtyParValue'"/>

				</QtyParValue>

				<OrigFaceAmt>
					<xsl:value-of select="'OrigFaceAmt'"/>
				</OrigFaceAmt>

				<ExecPrice>

					<xsl:value-of select="'ExecPrice'"/>


				</ExecPrice>

				<CommissionAmt>

					<xsl:value-of select="'CommissionAmt'"/>

				</CommissionAmt>


				<OtherFees>

					<xsl:value-of select="'OtherFees'"/>

				</OtherFees>

				<ExchangeFees>

					<xsl:value-of select="'ExchangeFees'"/>

				</ExchangeFees>

				<SECFees>

					<xsl:value-of select="'SECFees'"/>

				</SECFees>

				<NetSettlementAmount>

					<xsl:value-of select="'NetSettlementAmount'"/>

				</NetSettlementAmount>

				<Principal>

					<xsl:value-of select="'Principal'"/>

				</Principal>

				<AccruedInterest>

					<xsl:value-of select="'AccruedInterest'"/>


				</AccruedInterest>

				<TranCode>

					<xsl:value-of select="'TranCode'"/>



				</TranCode>





				<ExecutingBroker>

					<xsl:value-of select="'ExecutingBroker'"/>

				</ExecutingBroker>



				<ExecutingBrokerName>

					<xsl:value-of select="'ExecutingBrokerName'"/>


				</ExecutingBrokerName>





				<ExecutingBrokerAC>

					<xsl:value-of select="'Executing Broker A/C'"/>

				</ExecutingBrokerAC>

				<ClearingBroker>

					<xsl:value-of select="'ClearingBroker'"/>

				</ClearingBroker>

				<ClearingBrokerAC>

					<xsl:value-of select="'Clearing Broker A/C'"/>

				</ClearingBrokerAC>



				<PlaceOfSettlement>
					<xsl:value-of select="'PlaceOfSettlement'"/>

				</PlaceOfSettlement>


				<ExchRateInd>


					<xsl:value-of select="'ExchRateInd'"/>

				</ExchRateInd>



				<ExecuteFX>

					<xsl:value-of select="'ExecuteFX'"/>

				</ExecuteFX>



				<ForeignCCYISOOfTrade>

					<xsl:value-of select="'ForeignCCYISOOfTrade'"/>




				</ForeignCCYISOOfTrade>

				<SecurityDescription>
					<xsl:value-of select="'SecurityDescription'"/>
				</SecurityDescription>

				<Comments>

					<xsl:value-of select="'Comments'"/>


				</Comments>

				<SecurityType>


					<xsl:value-of select="'SecurityType'"/>



				</SecurityType>

				<TransactionFXRate>
					<xsl:value-of select="'TransactionFXRate'"/>
				</TransactionFXRate>

				<FXBuyCCYISO>
					<xsl:value-of select="'FXBuyCCYISO'"/>
				</FXBuyCCYISO>

				<FXBuyCCYAmount>
					<xsl:value-of select="'FXBuyCCYAmount'"/>
				</FXBuyCCYAmount>

				<FXBuyCCYDeliveringBIC>
					<xsl:value-of select="'FXBuyCCYDeliveringBIC'"/>
				</FXBuyCCYDeliveringBIC>

				<FXSellCCYISO>
					<xsl:value-of select="'FXSellCCYISO'"/>
				</FXSellCCYISO>

				<FXSellCCYAmount>
					<xsl:value-of select="'FXSellCCYAmount'"/>
				</FXSellCCYAmount>

				<FXSellCCYAccountwithInstitutionBIC>
					<xsl:value-of select="'FXSellCCYAccountwithInstitutionBIC'"/>
				</FXSellCCYAccountwithInstitutionBIC>

				<FXSellCCYBeneficiaryAcct>
					<xsl:value-of select="'FXSellCCYBeneficiaryAcct'"/>
				</FXSellCCYBeneficiaryAcct>

				<FXSellCCYBeneficiaryBIC>
					<xsl:value-of select="'FXSellCCYBeneficiaryBIC'"/>
				</FXSellCCYBeneficiaryBIC>

				<FXSellCCYFFCInformation>
					<xsl:value-of select="'FXSellCCYFFCInformation'"/>
				</FXSellCCYFFCInformation>


				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>-->


			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test="AccountName='881'  or AccountName='885' or AccountName='886' or AccountName='888' or AccountName='889' or AccountName='661' or AccountName='665' or AccountName='666' or AccountName='668' or AccountName='669' or AccountName='771' or AccountName='775' or AccountName='776' or AccountName='778' or AccountName='779'">
					<ThirdPartyFlatFileDetail>

						<RowHeader>
							<xsl:value-of select ="'True'"/>
						</RowHeader>

						<TaxlotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxlotState>

						<xsl:variable name="PB_NAME" select="'US Bank'"/>

						<xsl:variable name = "PRANA_FUND_NAME">
							<xsl:value-of select="AccountName"/>
						</xsl:variable>

						<xsl:variable name ="THIRDPARTY_FUND_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
						</xsl:variable>



						<Account>
							<xsl:choose>
								<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
									<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'6746020804'"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</Account>

						<CUSIP>
							<xsl:value-of select="CUSIP"/>
						</CUSIP>

						<SEDOL>
							<xsl:value-of select="SEDOL"/>
						</SEDOL>

						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>

						<SettleDate>
							<xsl:value-of select="SettlementDate"/>
						</SettleDate>

						<QtyParValue>
							<xsl:choose>
								<xsl:when test="number(AllocatedQty)">
									<xsl:value-of select="AllocatedQty"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</QtyParValue>

						<OrigFaceAmt>
							<xsl:value-of select="''"/>
						</OrigFaceAmt>

						<ExecPrice>
							<xsl:choose>
								<xsl:when test="number(AveragePrice)">
									<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</ExecPrice>

						<xsl:variable name="CommissionCharged">
							<xsl:value-of select="format-number((SoftCommissionCharged + CommissionCharged),'0.####')"/>
						</xsl:variable>

						<CommissionAmt>
							<xsl:choose>
								<xsl:when test="number($CommissionCharged)">
									<xsl:value-of select="$CommissionCharged"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CommissionAmt>

						<xsl:variable name ="OtherFee" select="(ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
						<OtherFees>
							<xsl:choose>
								<xsl:when test="Country = 'United States'">
									<xsl:value-of select="format-number($OtherFee - SecFee,'0.##')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number($OtherFee)">
											<xsl:value-of select="format-number($OtherFee,'0.##')"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</OtherFees>

						<ExchangeFees>
							<xsl:choose>
								<xsl:when test="number(OtherBrokerFee)">
									<xsl:value-of select="format-number(OtherBrokerFee,'0.##')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExchangeFees>

						<SECFees>
							<xsl:choose>
								<xsl:when test="number(SecFee)">
									<xsl:value-of select="format-number(SecFee,'0.####')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SECFees>

						<xsl:variable name="varSideMul">
							<xsl:choose>
								<xsl:when test="SideTag = '5' or SideTag = 'C' or SideTag = '2' ">
									<xsl:value-of select="-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Principal" select="AllocatedQty * format-number(AveragePrice,'0.####') * AssetMultiplier"/>
						
						<xsl:variable name="varNetAmmount">
							<xsl:value-of select="$Principal + (($OtherFee + $CommissionCharged + StampDuty + OtherBrokerFee) * $varSideMul)"/>
						</xsl:variable>


						<NetSettlementAmount>
							<xsl:choose>
								<xsl:when test="number($varNetAmmount)">
									<xsl:value-of select="format-number($varNetAmmount,'0.##')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetSettlementAmount>
						
						
						
						<Principal>
							<xsl:choose>
								<xsl:when test="number($Principal)">
									<xsl:value-of select="format-number($Principal,'0.##')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Principal>

						<AccruedInterest>
							<xsl:choose>
								<xsl:when test="number(AccruedInterest)">
									<xsl:value-of select="format-number(AccruedInterest,'0.##')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</AccruedInterest>

						<TranCode>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="Side='Buy to Close'">
									<xsl:value-of select="'BC'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side='Sell to Open'">
									<xsl:value-of select="'SS'"/>
								</xsl:when>
								<xsl:when test="Side='Sell to Close' or Side='Sell'">
									<xsl:value-of select="'S'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>


						</TranCode>





						<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

						<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBrokerCode"/>
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

						<xsl:variable name = "PRANA_BIC_NAME">
							<xsl:value-of select="THIRDPARTY_BIC_CODE"/>
						</xsl:variable>

						<xsl:variable name="Country" select="Country"/>

						<xsl:variable name ="THIRDPARTY_BIC_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/BicMapping.xml')/BicMapping/PB[@Name=$PB_NAME]/BICData[@Country=$Country and @Broker=$Broker]/@BICCode"/>
						</xsl:variable>

						<xsl:variable name = "PRANA_COUNTRY_NAME">
							<xsl:value-of select="THIRDPARTY_COUNTRY_CODE"/>
						</xsl:variable>

						<xsl:variable name ="THIRDPARTY_COUNTRY_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/CountryCodeMapping.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PBCountryName=$Country]/@PranaCountryCode"/>
						</xsl:variable>

						<xsl:variable name ="THIRDPARTY_PLACE_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/PlaceOfSettlement.xml')/PlaceOfSettlement/PB[@Name=$PB_NAME]/CountryData[@PBCountryName=$Country]/@PranaCountryCode"/>
						</xsl:variable>
						<xsl:variable name ="THIRDPARTY_BBIC_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/BeneficiaryBicMapping.xml')/BicMapping/PB[@Name=$PB_NAME]/BICData[@Country=$Country and @Broker=$Broker]/@BICCode"/>
						</xsl:variable>

						<xsl:variable name="Country1" select="Country"/>

						<xsl:variable name="CounterParty" select="CounterParty"/>

						<xsl:variable name ="THIRDPARTY_BROKER">
							<xsl:value-of select ="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$CounterParty]/@ThirdPartyBrokerID"/>
						</xsl:variable>


						<ExecutingBroker>
							<xsl:choose>
								
										<xsl:when test="$THIRDPARTY_BROKER!=''">
											<xsl:value-of select="$THIRDPARTY_BROKER"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								
						</ExecutingBroker>

						

					


						<ExecutingBrokerName>
							<xsl:choose>
								<xsl:when test="$THIRDPARTY_BROKER!=''">
									<xsl:value-of select="$THIRDPARTY_BROKER"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</ExecutingBrokerName>

						<xsl:variable name ="THIRDPARTY_EBBIC_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/ExecutingBicMapping.xml')/BicMapping/PB[@Name=$PB_NAME]/BICData[@Country=$Country and @Broker=$Broker]/@BICCode"/>
						</xsl:variable>



						<ExecutingBrokerAC>
							<!--<xsl:choose>
								<xsl:when test="Country='United States'">
									<xsl:value-of select="'0443'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_EBBIC_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_EBBIC_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_BIC_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select="''"/>

						</ExecutingBrokerAC>

						<xsl:variable name ="THIRDPARTY_BROKER1">
							<xsl:value-of select ="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[ @PranaBroker=$CounterParty]/@ThirdPartyBrokerID"/>
						</xsl:variable>

						<ClearingBroker>
							<xsl:choose>

								<xsl:when test="$THIRDPARTY_BROKER1!=''">
									<xsl:value-of select="$THIRDPARTY_BROKER1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</ClearingBroker>

						<ClearingBrokerAC>
							<!--<xsl:choose>
								<xsl:when test="Country='United Kingdom'">
									<xsl:value-of select="'44771'"/>
								</xsl:when>
								<xsl:when test="Country='HongKong'">
									<xsl:value-of select="'C00010'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select="''"/>
						</ClearingBrokerAC>



						<PlaceOfSettlement>


							<xsl:choose>
								<xsl:when test="Country='United States'">
									<xsl:value-of select="'USA'"/>
								</xsl:when>

								<!--<xsl:when test="Country='China' and CurrencySymbol='USD' ">
					<xsl:value-of select="'USA'"/>
				</xsl:when>-->

								<xsl:when test="$THIRDPARTY_PLACE_CODE!=''">
									<xsl:value-of select="$THIRDPARTY_PLACE_CODE"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PRANA_COUNTRY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</PlaceOfSettlement>

						<xsl:variable name="FXRate">
							<xsl:choose>
								<xsl:when test="number(FXRate_Taxlot)">
									<xsl:value-of select="FXRate_Taxlot"/>
								</xsl:when>

								<xsl:when test="number(ForexRate)">
									<xsl:value-of select="ForexRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<ExchRateInd>

							<xsl:choose>
								<xsl:when test="Country='United States'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number($FXRate,'0.##')"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExchRateInd>



						<ExecuteFX>
							<xsl:choose>
								<xsl:when test="Country='United States'or Country='China'and CurrencySymbol='USD'">
									<xsl:value-of select="'N'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'Y'"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExecuteFX>



						<ForeignCCYISOOfTrade>
							<xsl:choose>
								<xsl:when test="Country='China' and CurrencySymbol='USD' ">
									<xsl:value-of select="'USA'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="Country='United States'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:when test="$THIRDPARTY_COUNTRY_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_COUNTRY_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTRY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>



						</ForeignCCYISOOfTrade>

						<SecurityDescription>
							<xsl:value-of select="FullSecurityName"/>
						</SecurityDescription>

						<Comments>

							<xsl:choose>
								<xsl:when test="Asset='Option' or Asset='Future'">
									<xsl:value-of select="FullSecurityName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</Comments>

						<SecurityType>

							<xsl:choose>
								<xsl:when test="Asset='Equity'">
									<xsl:value-of select="'E'"/>
								</xsl:when>
								<xsl:when test="Asset='EquityOption' or Asset='Future'">
									<xsl:value-of select="'O'"/>
								</xsl:when>
								<xsl:when test="Asset='Bond'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<!--<xsl:when test="Asset='MBS'">
                <xsl:value-of select="'M'"/>
              </xsl:when>-->
								<xsl:when test="Asset='FX'">
									<xsl:value-of select="'F'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>


						</SecurityType>

						<TransactionFXRate>
							<xsl:value-of select="''"/>
						</TransactionFXRate>

						<FXBuyCCYISO>
							<xsl:value-of select="''"/>
						</FXBuyCCYISO>

						<FXBuyCCYAmount>
							<xsl:value-of select="''"/>
						</FXBuyCCYAmount>

						<FXBuyCCYDeliveringBIC>
							<xsl:value-of select="''"/>
						</FXBuyCCYDeliveringBIC>

						<FXSellCCYISO>
							<xsl:value-of select="''"/>
						</FXSellCCYISO>

						<FXSellCCYAmount>
							<xsl:value-of select="''"/>
						</FXSellCCYAmount>

						<FXSellCCYAccountwithInstitutionBIC>
							<xsl:value-of select="''"/>
						</FXSellCCYAccountwithInstitutionBIC>

						<FXSellCCYBeneficiaryAcct>
							<xsl:value-of select="''"/>
						</FXSellCCYBeneficiaryAcct>

						<FXSellCCYBeneficiaryBIC>
							<xsl:value-of select="''"/>
						</FXSellCCYBeneficiaryBIC>

						<FXSellCCYFFCInformation>
							<xsl:value-of select="''"/>
						</FXSellCCYFFCInformation>


						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>
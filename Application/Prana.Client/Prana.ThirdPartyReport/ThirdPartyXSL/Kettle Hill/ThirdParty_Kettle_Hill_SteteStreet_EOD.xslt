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

					<xsl:variable name ="varAllocationState">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'NEWM'"/>

							</xsl:when>

							<xsl:when test="TaxLotState='Deleted' or TaxLotState='Amended' ">
								<xsl:value-of select="'CANC'"/>

							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<MessageFunction>
						<xsl:value-of select="$varAllocationState"/>
					</MessageFunction>

					<TransactionReference>
						<xsl:value-of select="EntityID"/>
					</TransactionReference>

					<RelatedReferenceNumber>
						<xsl:value-of select="''"/>
					</RelatedReferenceNumber>

					<xsl:variable name="PB_NAME" select="' '"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
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
						<xsl:value-of select="TradeDate"/>
					</TradeDate>


					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>
					</SettlementDate>

					<LateDeliveryDate>
						<xsl:value-of select="''"/>
					</LateDeliveryDate>

					<SecurityIDType>
						<xsl:choose>
							<xsl:when test="CUSIP!='*'">
								<xsl:value-of select="'US'"/>
							</xsl:when>
							<xsl:when test="SEDOL!='*'">
								<xsl:value-of select="'GB'"/>
							</xsl:when>
							<xsl:when test="ISIN!='*'">
								<xsl:value-of select="'ISIN'"/>
							</xsl:when>
							<xsl:when test="Symbol!='*' and (contains(Asset,'Future') or contains(Asset,'Option'))">
								<xsl:value-of select="'TS'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityIDType>

					<SecurityID>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:when test="CUSIP!='*'">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:when test="SEDOL!='*'">
								<xsl:value-of select="SEDOL"/>
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
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<SecurityType>
						<xsl:choose>
						
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="'OPT'"/>
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
						<xsl:choose>
							<xsl:when test="contains(Asset,'Future') or contains(Asset,'Option') ">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CurrencyOfDenomination>

					<OptionStyle>
						<xsl:value-of select="''"/>
					</OptionStyle>

					<OptionType>

						<xsl:value-of select="PutOrCall"/>

					</OptionType>

					<ContractSize>
						<xsl:value-of select="Multiplier"/>

					</ContractSize>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test="number(StrikePrice)">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

					<ExpirationDate>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExpirationDate>

					<UnderlyingSecurityIDType>
						<xsl:choose>	
							<xsl:when test="CUSIP!='*'">
								<xsl:value-of select="'US'"/>
							</xsl:when>
							<xsl:when test="SEDOL!='*'">
								<xsl:value-of select="'GB'"/>
							</xsl:when>
							<xsl:when test="ISIN!='*'">
								<xsl:value-of select="'ISIN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</UnderlyingSecurityIDType>

					<UnderlyingSecurityID>
						<xsl:value-of select="UnderlyingSymbol"/>
					</UnderlyingSecurityID>

					<UnderlyingSecurityDesc>
						<xsl:value-of select="FullSecurityName"/>
					</UnderlyingSecurityDesc>

					<MaturityDate>
						<xsl:choose>

							<xsl:when test="contains(Asset,'Future') or contains(Asset,'Option') ">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</MaturityDate>

					<IssueDate>
						<xsl:value-of select="TradeDate"/>
					</IssueDate>

					<InterestRate>
						<xsl:choose>
							<xsl:when test="number(Coupon)">
								<xsl:value-of select="format-number(Coupon,'#.######')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</InterestRate>

					<OriginalFace>
						<xsl:value-of select="''"/>
					</OriginalFace>


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

					<TradeCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</TradeCurrency>

					<DealPriceCode>
						<xsl:value-of select="'ACTU'"/>
					</DealPriceCode>

					<DealPrice>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</DealPrice>


					<xsl:variable name="Principal" select="AllocatedQty * AveragePrice"/>

					<PrincipalAmount>
						<xsl:choose>
							<xsl:when test="number($Principal)">
								<xsl:value-of select="format-number($Principal,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</PrincipalAmount>
					
					<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					
					
					<CommissionsAmount>
						<xsl:choose>
							<xsl:when test="number($varCommission)">
								<xsl:value-of select="format-number($varCommission,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CommissionsAmount>

					<xsl:variable name="OtherFees">
						<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + SoftCommission"/>
					</xsl:variable>

					<ChargesFeesAmount>
						<xsl:choose>
							<xsl:when test="number($OtherFees)">
								<xsl:value-of select="format-number($OtherFees,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</ChargesFeesAmount>

					<OtherAmount>
						<xsl:value-of select="''"/>
					</OtherAmount>

					<AccruedInterestAmount>
						<xsl:choose>
							<xsl:when test="number(AccruedInterest)">
								<xsl:value-of select="format-number(AccruedInterest,'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
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

					<xsl:variable name="SettleFX">
						<xsl:choose>
							<xsl:when test="number(SettlCurrFxRate)">
								<xsl:value-of select="SettlCurrFxRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varSideMul">
						<xsl:choose>
							<xsl:when test="SideID = '2'">
								<xsl:value-of select="-1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="1"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varNetAmmount">
						<xsl:value-of select="(AllocatedQty * AveragePrice) + ($varCommission)"/>
					</xsl:variable>

					<SettlementAmount>
						<xsl:choose>
							<xsl:when test="SettlCurrFxRateCalc='M'">
								<xsl:value-of select="format-number(($varNetAmmount * $SettleFX),'0.##')"/>
							</xsl:when>
							<xsl:when test="SettlCurrFxRateCalc='D'">
								<xsl:value-of select="format-number(($varNetAmmount div $SettleFX),'0.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varNetAmmount,'0.##')"/>
							</xsl:otherwise>
						</xsl:choose>
					</SettlementAmount>


					<TransactionSubType>
						<xsl:value-of select="'TRAD'"/>
					</TransactionSubType>

					<SettlementTransactionConditionIndicator>
						<xsl:value-of select="''"/>
					</SettlementTransactionConditionIndicator>


					<SettlementTransactionConditionIndicator2>
						<xsl:value-of select="''"/>
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
						<xsl:value-of select="''"/>
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
						<xsl:value-of select="''"/>
					</ExecutingBrokerIDType>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="Counterparty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
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
					<ExecutingBrokerID>
						<xsl:value-of select="$Broker"/>
					</ExecutingBrokerID>

					<ExecutingBrokerAcct>
						<xsl:value-of select="''"/>
					</ExecutingBrokerAcct>

					<ClearingBrokerAgentIDType>
						<xsl:value-of select="''"/>
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
						<xsl:value-of select="$BrokerName"/>
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
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

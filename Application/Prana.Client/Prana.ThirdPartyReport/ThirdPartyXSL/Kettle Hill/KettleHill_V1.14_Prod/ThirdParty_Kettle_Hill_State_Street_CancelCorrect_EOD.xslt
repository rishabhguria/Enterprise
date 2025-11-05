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
			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>


				<TransactionType>
					<xsl:value-of select ="'TransactionType'"/>
				</TransactionType>

				<FundID>
					<xsl:value-of select="'FundID'"/>
				</FundID>

				<TransactionReference>
					<xsl:value-of select ="'TransactionReference'"/>
				</TransactionReference>

				<RelatedReferenceNumber>
					<xsl:value-of select ="'RelatedReferenceNumber'"/>
				</RelatedReferenceNumber>

				<CashPurposeCode>
					<xsl:value-of select ="'CashPurposeCode'"/>
				</CashPurposeCode>

				<BankOperationCode>
					<xsl:value-of select ="'BankOperationCode'"/>
				</BankOperationCode>

				<InstructionCode>
					<xsl:value-of select ="'InstructionCode'"/>
				</InstructionCode>

				<ValueDate>
					<xsl:value-of select="'ValueDate'"/>
				</ValueDate>

				<Currency>
					<xsl:value-of select ="'Currency'"/>
				</Currency>
				
				<Amount>
					<xsl:value-of select ="'Amount'"/>
				</Amount>

				<RecCorrespondentBIC>
					<xsl:value-of select ="'RecCorrespondentBIC'"/>
				</RecCorrespondentBIC>

				<RecCorrespondentName>
					<xsl:value-of select ="'RecCorrespondentName'"/>
				</RecCorrespondentName>

				<RecCorrespondentAcct>
					<xsl:value-of select ="'RecCorrespondentAcct'"/>
				</RecCorrespondentAcct>

				<IntermediaryBIC>
					<xsl:value-of select ="'IntermediaryBIC'"/>
				</IntermediaryBIC>

				<IntermediaryName>
					<xsl:value-of select ="'IntermediaryName'"/>
				</IntermediaryName>

				<IntermediaryAcctType>
					<xsl:value-of select ="'IntermediaryAcctType'"/>
				</IntermediaryAcctType>


				<IntermediaryAcct>
					<xsl:value-of select ="'IntermediaryAcct'"/>
				</IntermediaryAcct>

				<AcctWithInstitutionBIC>
					<xsl:value-of select ="'AcctWithInstitutionBIC'"/>
				</AcctWithInstitutionBIC>

				<AcctWithInstitutionName>
					<xsl:value-of select ="'AcctWithInstitutionName'"/>
				</AcctWithInstitutionName>

				<AcctWithInstitutionAcctType>
					<xsl:value-of select ="'AcctWithInstitutionAcctType'"/>
				</AcctWithInstitutionAcctType>

				<AcctWithInstitutionAcct>
					<xsl:value-of select ="'AcctWithInstitutionAcct'"/>
				</AcctWithInstitutionAcct>

				<BeneficiaryBIC>
					<xsl:value-of select ="'BeneficiaryBIC'"/>
				</BeneficiaryBIC>

				<BeneficiaryName>
					<xsl:value-of select ="'BeneficiaryName'"/>
				</BeneficiaryName>

				<BeneficiaryAcctType>
					<xsl:value-of select ="'BeneficiaryAcctType'"/>
				</BeneficiaryAcctType>

				<BeneficiaryAcct>
					<xsl:value-of select ="'BeneficiaryAcct'"/>
				</BeneficiaryAcct>

				<OrderingInstBIC>
					<xsl:value-of select ="'OrderingInstBIC'"/>
				</OrderingInstBIC>

				<OrderingInstName>
					<xsl:value-of select ="'OrderingInstName'"/>
				</OrderingInstName>

				<OrderingInstAcctType>
					<xsl:value-of select ="'OrderingInstAcctType'"/>
				</OrderingInstAcctType>


				<OrderingInstAcct>
					<xsl:value-of select ="'OrderingInstAcct'"/>
				</OrderingInstAcct>

				<DetailCharges>
					<xsl:value-of select ="'DetailCharges'"/>
				</DetailCharges>

				<SendertoRecCodeword>
					<xsl:value-of select ="'SendertoRecCodeword'"/>
				</SendertoRecCodeword>

				<SendertoReceiverInfo>
					<xsl:value-of select ="'SendertoReceiverInfo'"/>
				</SendertoReceiverInfo>

				<OrgTransDate>
					<xsl:value-of select ="'OrgTransDate'"/>
				</OrgTransDate>

				<OrgTransType>
					<xsl:value-of select ="'OrgTransType'"/>
				</OrgTransType>

				<ReceiveGLAcct>
					<xsl:value-of select ="'ReceiveGLAcct'"/>
				</ReceiveGLAcct>

				<PayGLAccount>
					<xsl:value-of select ="'PayGLAccount'"/>
				</PayGLAccount>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[(AccountName='PACE - Longs - State Street' or AccountName='PACE - Shorts - Morgan Stanley') and (CurrencySymbol='JPY')]">

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
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<TransactionType>
								<xsl:value-of select ="202"/>
							</TransactionType>

							<FundID>
								<xsl:value-of select="'CFFW'"/>
							</FundID>

							<TransactionReference>
								<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
							</TransactionReference>

							<RelatedReferenceNumber>
								<xsl:value-of select ="''"/>
							</RelatedReferenceNumber>

							<CashPurposeCode>
								<xsl:value-of select ="'CASH'"/>
							</CashPurposeCode>

							<BankOperationCode>
								<xsl:value-of select ="''"/>
							</BankOperationCode>

							<InstructionCode>
								<xsl:value-of select ="''"/>
							</InstructionCode>
							
							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>							

							<ValueDate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</ValueDate>

							<Currency>
								<xsl:value-of select ="'USD'"/>
							</Currency>

							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="1"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<xsl:variable name = "NETAMNT">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='M'">
										<xsl:value-of select="$varNetamount * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='D'">
										<xsl:value-of select="$varNetamount div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<Amount>
								<xsl:value-of select ="format-number($NETAMNT,'0.')"/>
							</Amount>

							<RecCorrespondentBIC>
								<xsl:value-of select ="''"/>
							</RecCorrespondentBIC>

							<RecCorrespondentName>
								<xsl:value-of select ="''"/>
							</RecCorrespondentName>

							<RecCorrespondentAcct>
								<xsl:value-of select ="''"/>
							</RecCorrespondentAcct>

							<IntermediaryBIC>
								<xsl:value-of select ="''"/>
							</IntermediaryBIC>

							<IntermediaryName>
								<xsl:value-of select ="''"/>
							</IntermediaryName>

							<IntermediaryAcctType>
								<xsl:value-of select ="''"/>
							</IntermediaryAcctType>


							<IntermediaryAcct>
								<xsl:value-of select ="''"/>
							</IntermediaryAcct>

							<AcctWithInstitutionBIC>
								<xsl:value-of select ="''"/>
							</AcctWithInstitutionBIC>

							<AcctWithInstitutionName>
								<xsl:value-of select ="'CITIBANK'"/>
							</AcctWithInstitutionName>

							<AcctWithInstitutionAcctType>
								<xsl:value-of select ="'FW'"/>
							</AcctWithInstitutionAcctType>

							<AcctWithInstitutionAcct>
								<xsl:value-of select ="'021000089'"/>
							</AcctWithInstitutionAcct>

							<BeneficiaryBIC>
								<xsl:value-of select ="''"/>
							</BeneficiaryBIC>

							<BeneficiaryName>
								<xsl:value-of select ="'Morgan Stanley NY'"/>
							</BeneficiaryName>

							<BeneficiaryAcctType>
								<xsl:value-of select ="''"/>
							</BeneficiaryAcctType>

							<BeneficiaryAcct>
								<xsl:value-of select ="'38890774'"/>
							</BeneficiaryAcct>

							<OrderingInstBIC>
								<xsl:value-of select ="''"/>
							</OrderingInstBIC>

							<OrderingInstName>
								<xsl:value-of select ="''"/>
							</OrderingInstName>

							<OrderingInstAcctType>
								<xsl:value-of select ="''"/>
							</OrderingInstAcctType>


							<OrderingInstAcct>
								<xsl:value-of select ="''"/>
							</OrderingInstAcct>

							<DetailCharges>
								<xsl:value-of select ="''"/>
							</DetailCharges>

							<SendertoRecCodeword>
								<xsl:value-of select ="''"/>
							</SendertoRecCodeword>

							<SendertoReceiverInfo>
								<xsl:value-of select ="''"/>
							</SendertoReceiverInfo>

							<OrgTransDate>
								<xsl:value-of select ="''"/>
							</OrgTransDate>

							<OrgTransType>
								<xsl:value-of select ="''"/>
							</OrgTransType>

							<ReceiveGLAcct>
								<xsl:value-of select ="''"/>
							</ReceiveGLAcct>

							<PayGLAccount>
								<xsl:value-of select ="''"/>
							</PayGLAccount>

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

								<TransactionType>
									<xsl:value-of select ="292"/>
								</TransactionType>

								<FundID>
									<xsl:value-of select="'CFFW'"/>
								</FundID>

								<TransactionReference>
									<xsl:value-of select="concat(substring(EntityID,string-length(EntityID)-7,string-length(EntityID)),'E')"/>
								</TransactionReference>

								<RelatedReferenceNumber>
									<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
								</RelatedReferenceNumber>

								<CashPurposeCode>
									<xsl:value-of select ="'CASH'"/>
								</CashPurposeCode>

								<BankOperationCode>
									<xsl:value-of select ="''"/>
								</BankOperationCode>

								<InstructionCode>
									<xsl:value-of select ="''"/>
								</InstructionCode>



								<xsl:variable name="varOldTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>

								<ValueDate>
									<xsl:value-of select="concat(substring-before($varOldTradeDate,'/'),'/',substring-before(substring-after($varOldTradeDate,'/'),'/'),'/',substring-after(substring-after($varOldTradeDate,'/'),'/'))"/>
								</ValueDate>				



								<Currency>
									<xsl:value-of select ="'USD'"/>
								</Currency>


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
								<xsl:variable name="varOldFXRate">
									<!--only use for cancle/amend xslt for deleted trade-->
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != OldCurrencySymbol">
											<xsl:value-of select="OldFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="1"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<!--<xsl:variable name="varFXRate">
									<xsl:choose>
										<xsl:when test="SettlCurrency != CurrencySymbol">
											<xsl:choose>
												<xsl:when test="FXConversionMethodOperator_Trade != 'D'">
													<xsl:value-of select="FXRate_Taxlot"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="1 div FXRate_Taxlot"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="1"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>-->

								<xsl:variable name = "varoldNETAMNT">
									<xsl:choose>
										<xsl:when test="$varOldFXRate=0">
											<xsl:value-of select="$varOldNetAmount"/>
										</xsl:when>
										<xsl:when test="$varOldFXRate!=0 and OldFXConversionMethodOperator='M'">
											<xsl:value-of select="$varOldNetAmount * $varOldFXRate"/>
										</xsl:when>

										<xsl:when test="$varOldFXRate!=0 and OldFXConversionMethodOperator='D'">
											<xsl:value-of select="$varOldNetAmount div $varOldFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<Amount>
									<xsl:value-of select ="format-number($varoldNETAMNT,'0.')"/>
								</Amount>

								<RecCorrespondentBIC>
									<xsl:value-of select ="''"/>
								</RecCorrespondentBIC>

								<RecCorrespondentName>
									<xsl:value-of select ="''"/>
								</RecCorrespondentName>

								<RecCorrespondentAcct>
									<xsl:value-of select ="''"/>
								</RecCorrespondentAcct>

								<IntermediaryBIC>
									<xsl:value-of select ="''"/>
								</IntermediaryBIC>

								<IntermediaryName>
									<xsl:value-of select ="''"/>
								</IntermediaryName>

								<IntermediaryAcctType>
									<xsl:value-of select ="''"/>
								</IntermediaryAcctType>


								<IntermediaryAcct>
									<xsl:value-of select ="''"/>
								</IntermediaryAcct>

								<AcctWithInstitutionBIC>
									<xsl:value-of select ="''"/>
								</AcctWithInstitutionBIC>

								<AcctWithInstitutionName>
									<xsl:value-of select ="'CITIBANK'"/>
								</AcctWithInstitutionName>

								<AcctWithInstitutionAcctType>
									<xsl:value-of select ="'FW'"/>
								</AcctWithInstitutionAcctType>

								<AcctWithInstitutionAcct>
									<xsl:value-of select ="'021000089'"/>
								</AcctWithInstitutionAcct>

								<BeneficiaryBIC>
									<xsl:value-of select ="''"/>
								</BeneficiaryBIC>

								<BeneficiaryName>
									<xsl:value-of select ="'Morgan Stanley NY'"/>
								</BeneficiaryName>

								<BeneficiaryAcctType>
									<xsl:value-of select ="''"/>
								</BeneficiaryAcctType>

								<BeneficiaryAcct>
									<xsl:value-of select ="'38890774'"/>
								</BeneficiaryAcct>

								<OrderingInstBIC>
									<xsl:value-of select ="''"/>
								</OrderingInstBIC>

								<OrderingInstName>
									<xsl:value-of select ="''"/>
								</OrderingInstName>

								<OrderingInstAcctType>
									<xsl:value-of select ="''"/>
								</OrderingInstAcctType>


								<OrderingInstAcct>
									<xsl:value-of select ="''"/>
								</OrderingInstAcct>

								<DetailCharges>
									<xsl:value-of select ="''"/>
								</DetailCharges>

								<SendertoRecCodeword>
									<xsl:value-of select ="''"/>
								</SendertoRecCodeword>

								<SendertoReceiverInfo>
									<xsl:value-of select ="''"/>
								</SendertoReceiverInfo>

								<OrgTransDate>
									<xsl:value-of select="concat(substring-before($varOldTradeDate,'/'),'/',substring-before(substring-after($varOldTradeDate,'/'),'/'),'/',substring-after(substring-after($varOldTradeDate,'/'),'/'))"/>
								</OrgTransDate>

								<OrgTransType>
									<xsl:value-of select ="'202'"/>
								</OrgTransType>

								<ReceiveGLAcct>
									<xsl:value-of select ="''"/>
								</ReceiveGLAcct>

								<PayGLAccount>
									<xsl:value-of select ="''"/>
								</PayGLAccount>
								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>
						<ThirdPartyFlatFileDetail>
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>
							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>
							
							<TransactionType>
								<xsl:value-of select ="202"/>
							</TransactionType>

							<FundID>
								<xsl:value-of select="'CFFW'"/>
							</FundID>

							<TransactionReference>
								<!--<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>-->
								<xsl:value-of select="substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-7,string-length(AmendTaxLotId1))"/>
							</TransactionReference>

							<RelatedReferenceNumber>
								<xsl:value-of select ="''"/>
							</RelatedReferenceNumber>

							<CashPurposeCode>
								<xsl:value-of select ="'CASH'"/>
							</CashPurposeCode>

							<BankOperationCode>
								<xsl:value-of select ="''"/>
							</BankOperationCode>

							<InstructionCode>
								<xsl:value-of select ="''"/>
							</InstructionCode>

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<ValueDate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</ValueDate>


							<Currency>
								<xsl:value-of select ="'USD'"/>
							</Currency>


							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="1"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name = "NETAMNT">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='M'">
										<xsl:value-of select="$varNetamount * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='D'">
										<xsl:value-of select="$varNetamount div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<Amount>
								<xsl:value-of select="format-number($NETAMNT,'0.')"/>
							</Amount>

							<RecCorrespondentBIC>
								<xsl:value-of select ="''"/>
							</RecCorrespondentBIC>

							<RecCorrespondentName>
								<xsl:value-of select ="''"/>
							</RecCorrespondentName>

							<RecCorrespondentAcct>
								<xsl:value-of select ="''"/>
							</RecCorrespondentAcct>

							<IntermediaryBIC>
								<xsl:value-of select ="''"/>
							</IntermediaryBIC>

							<IntermediaryName>
								<xsl:value-of select ="''"/>
							</IntermediaryName>

							<IntermediaryAcctType>
								<xsl:value-of select ="''"/>
							</IntermediaryAcctType>


							<IntermediaryAcct>
								<xsl:value-of select ="''"/>
							</IntermediaryAcct>

							<AcctWithInstitutionBIC>
								<xsl:value-of select ="''"/>
							</AcctWithInstitutionBIC>

							<AcctWithInstitutionName>
								<xsl:value-of select ="'CITIBANK'"/>
							</AcctWithInstitutionName>

							<AcctWithInstitutionAcctType>
								<xsl:value-of select ="'FW'"/>
							</AcctWithInstitutionAcctType>

							<AcctWithInstitutionAcct>
								<xsl:value-of select ="'021000089'"/>
							</AcctWithInstitutionAcct>

							<BeneficiaryBIC>
								<xsl:value-of select ="''"/>
							</BeneficiaryBIC>

							<BeneficiaryName>
								<xsl:value-of select ="'Morgan Stanley NY'"/>
							</BeneficiaryName>

							<BeneficiaryAcctType>
								<xsl:value-of select ="''"/>
							</BeneficiaryAcctType>

							<BeneficiaryAcct>
								<xsl:value-of select ="'38890774'"/>
							</BeneficiaryAcct>

							<OrderingInstBIC>
								<xsl:value-of select ="''"/>
							</OrderingInstBIC>

							<OrderingInstName>
								<xsl:value-of select ="''"/>
							</OrderingInstName>

							<OrderingInstAcctType>
								<xsl:value-of select ="''"/>
							</OrderingInstAcctType>


							<OrderingInstAcct>
								<xsl:value-of select ="''"/>
							</OrderingInstAcct>

							<DetailCharges>
								<xsl:value-of select ="''"/>
							</DetailCharges>

							<SendertoRecCodeword>
								<xsl:value-of select ="''"/>
							</SendertoRecCodeword>

							<SendertoReceiverInfo>
								<xsl:value-of select ="''"/>
							</SendertoReceiverInfo>

							<OrgTransDate>
								<xsl:value-of select ="''"/>
							</OrgTransDate>

							<OrgTransType>
								<xsl:value-of select ="''"/>
							</OrgTransType>

							<ReceiveGLAcct>
								<xsl:value-of select ="''"/>
							</ReceiveGLAcct>

							<PayGLAccount>
								<xsl:value-of select ="''"/>
							</PayGLAccount>
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

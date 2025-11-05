<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[contains(AccountName,'PACE') and CurrencySymbol='JPY' and (Side='Buy to Close' or Side='Sell Short')]">
			
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					<TransactionType>
						<xsl:value-of select ="202"/>
					</TransactionType>
					
					<FundID>
						<xsl:value-of select="'CFFW'"/>
					</FundID>

					<TransactionReference>
						<xsl:value-of select ="'1202'"/>
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

					<ValueDate>
						<xsl:value-of select="concat(substring-before(TradeDate,'/'),'/',substring-before(substring-after(TradeDate,'/'),'/'),'/',substring-after(substring-after(TradeDate,'/'),'/'))"/>
					</ValueDate>



					<Currency>
						<xsl:value-of select ="'USD'"/>
					</Currency>


					<xsl:variable name="varFXRate">
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
					</xsl:variable>

					<xsl:variable name = "NETAMNT">
						<xsl:choose>
							<xsl:when test="$varFXRate=0">
								<xsl:value-of select="NetAmount"/>
							</xsl:when>
							<!--<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
								<xsl:value-of select="NetAmount * SettlCurrFxRate"/>
							</xsl:when>

							<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
								<xsl:value-of select="NetAmount div SettlCurrFxRate"/>
							</xsl:when>-->
							<xsl:otherwise>
								<xsl:value-of select="NetAmount * $varFXRate"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Amount>
						<xsl:value-of select ="format-number(NetAmount,'0.')"/>
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
						<xsl:value-of select ="''"/>
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
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

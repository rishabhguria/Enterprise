<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public int RoundOff(double Qty)
		{

		return (int)Math.Round(Qty,0);
		}
	</msxsl:script>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>


	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'False'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>



				<FunctionoftheMessage>
					<xsl:value-of select="'Function of the Message'"/>
				</FunctionoftheMessage>

				<TradeType>
					<xsl:value-of select="'Trade Type'"/>
				</TradeType>


				<AccountNo>
					<xsl:value-of select ="'Account No'"/>
				</AccountNo>



				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>



				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<BrokerID>
					<xsl:value-of select="'Broker ID'"/>
				</BrokerID>

				<ClearerID>
					<xsl:value-of select="'Clearer ID'"/>
				</ClearerID>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<AssetType>
					<xsl:value-of select="'Asset Type'"/>
				</AssetType>

				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>

				<LocalCost>
					<xsl:value-of select="'Local Cost'"/>
				</LocalCost>



				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<BrokerAccount>
					<xsl:value-of select="'Broker Account'"/>
				</BrokerAccount>

				<SECFee>
					<xsl:value-of select="'SEC Fee'"/>
				</SECFee>

				<OtherCharges>
					<xsl:value-of select="'Other Charges'"/>
				</OtherCharges>


				<Principal>
					<xsl:value-of select="'Principal'"/>
				</Principal>

				<Interest>
					<xsl:value-of select="'Interest'"/>
				</Interest>

				<FinalMoney>
					<xsl:value-of select="'Final Money'"/>
				</FinalMoney>

				<SecurityID>
					<xsl:value-of select="'Security ID'"/>
				</SecurityID>

				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>

				<MaturityDate>
					<xsl:value-of select="'Maturity Date'"/>
				</MaturityDate>

				<IssueDate>
					<xsl:value-of select="'Issue Date'"/>
				</IssueDate>

				<CurrentRate>
					<xsl:value-of select="'Current Rate'"/>
				</CurrentRate>

				<SafekeepingPlace>
					<xsl:value-of select="'Safekeeping Place'"/>
				</SafekeepingPlace>

				<SettlementPlace>
					<xsl:value-of select="'Settlement Place'"/>
				</SettlementPlace>

				<Reference>
					<xsl:value-of select="'Reference'"/>
				</Reference>

				<StampDuty>
					<xsl:value-of select="'Stamp Duty'"/>
				</StampDuty>

				<OriginalFace>
					<xsl:value-of select="'Original Face'"/>
				</OriginalFace>

				<FXExecute>
					<xsl:value-of select="'FX Execute'"/>
				</FXExecute>

				<BuySellCurrency>
					<xsl:value-of select="'Buy/Sell Currency'"/>
				</BuySellCurrency>

				<FXSpecial>
					<xsl:value-of select="'FX Special'"/>
				</FXSpecial>

				<Market>
					<xsl:value-of select="'Market'"/>
				</Market>

				<SpecialInstruction>
					<xsl:value-of select="'Special Instruction'"/>
				</SpecialInstruction>

				<BlockDetailCounter>
					<xsl:value-of select="'Block Detail Counter'"/>
				</BlockDetailCounter>

				<RelatedReference>
					<xsl:value-of select="'Related Reference'"/>
				</RelatedReference>

				<DeliverToAccount>
					<xsl:value-of select="'Deliver To Account'"/>
				</DeliverToAccount>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<PoolNumber>
					<xsl:value-of select="'Pool Number'"/>
				</PoolNumber>

				<Factor>
					<xsl:value-of select="'Factor'"/>
				</Factor>

				<ADELDate>
					<xsl:value-of select="'ADEL Date'"/>
				</ADELDate>

				<Taxes>
					<xsl:value-of select="'Taxes'"/>
				</Taxes>

				<SettlementMethod>
					<xsl:value-of select="'Settlement Method'"/>
				</SettlementMethod>

				<SecurityIDType>
					<xsl:value-of select="'Security ID Type'"/>
				</SecurityIDType>

				<Investor>
					<xsl:value-of select="'Investor'"/>
				</Investor>

				<BrokerIDType>
					<xsl:value-of select="'Broker ID Type'"/>
				</BrokerIDType>

				<BrokerDescription>
					<xsl:value-of select="'Broker Description'"/>
				</BrokerDescription>


				<ClearerIDType>
					<xsl:value-of select="'Clearer ID Type'"/>
				</ClearerIDType>

				<ClearerDescription>
					<xsl:value-of select="'Clearer Description'"/>
				</ClearerDescription>


				<ClearerAccount>
					<xsl:value-of select="'Clearer Account'"/>
				</ClearerAccount>

				<CustodianIDType>
					<xsl:value-of select="'Custodian ID Type'"/>
				</CustodianIDType>

				<CustodianID>
					<xsl:value-of select="'Custodian ID'"/>
				</CustodianID>

				<CustodianDescription>
					<xsl:value-of select="'Custodian Description'"/>
				</CustodianDescription>

				<CustodianAccount>
					<xsl:value-of select="'Custodian Account'"/>
				</CustodianAccount>

				<BaseCost>
					<xsl:value-of select="'Base Cost'"/>
				</BaseCost>

				<BaseCurrency>
					<xsl:value-of select="'Base Currency'"/>
				</BaseCurrency>

				<LocalCurrency>
					<xsl:value-of select="'Local Currency'"/>
				</LocalCurrency>


				<ChangeOwnerReg>
					<xsl:value-of select="'Change Owner/Reg'"/>
				</ChangeOwnerReg>

				<SettlementIndicator>
					<xsl:value-of select="'Settlement Indicator'"/>
				</SettlementIndicator>


				<ItalianTaxID>
					<xsl:value-of select="'Italian Tax ID'"/>
				</ItalianTaxID>

				<SettlementTransactionIndicator>
					<xsl:value-of select="'Settlement Transaction Indicator'"/>
				</SettlementTransactionIndicator>

				<Sub-CustodianSpecialInst>
					<xsl:value-of select="'Sub-Custodian Special Inst'"/>
				</Sub-CustodianSpecialInst>

				<InventoryBook>
					<xsl:value-of select="'Inventory Book'"/>
				</InventoryBook>



				<ManualBrokerDescriptionFlagBook>
					<xsl:value-of select="'Manual Broker Description Flag Book'"/>
				</ManualBrokerDescriptionFlagBook>

				<ManualBrokerDescription>
					<xsl:value-of select="'Manual Broker Description'"/>
				</ManualBrokerDescription>

				<IIJNumber>
					<xsl:value-of select="'IIJ Number'"/>
				</IIJNumber>

				<TrackingIndicator>
					<xsl:value-of select="'Tracking Indicator'"/>
				</TrackingIndicator>

				<TaxCode>
					<xsl:value-of select="'Tax Code'"/>
				</TaxCode>

				<TaxCodeDeliver>
					<xsl:value-of select="'Tax Code - Deliver'"/>
				</TaxCodeDeliver>

				<TaxCodeReceive>
					<xsl:value-of select="'Tax Code - Receive'"/>
				</TaxCodeReceive>

				<CurrentFaceQuantity>
					<xsl:value-of select="'Current Face Quantity'"/>
				</CurrentFaceQuantity>

				<AccountingDescriptionLine1>
					<xsl:value-of select="'Accounting Description Line 1'"/>
				</AccountingDescriptionLine1>

				<AccountingDescriptionLine2>
					<xsl:value-of select="'Accounting Description Line 2'"/>
				</AccountingDescriptionLine2>

				<AccountingDescriptionReceiveLine1>
					<xsl:value-of select="'Accounting Description Receive Line 1'"/>
				</AccountingDescriptionReceiveLine1>

				<AccountingDescriptionReceiveLine2>
					<xsl:value-of select="'Accounting Description Receive Line 2'"/>
				</AccountingDescriptionReceiveLine2>


				<TradeConditionIndicator>
					<xsl:value-of select="'Trade Condition Indicator'"/>
				</TradeConditionIndicator>

				<DealReference>
					<xsl:value-of select="'Deal Reference'"/>
				</DealReference>

				<CommonTradeReference>
					<xsl:value-of select="'Common TradeRe ference'"/>
				</CommonTradeReference>

				<PlaceofTrade>
					<xsl:value-of select="'Place of Trade'"/>
				</PlaceofTrade>

				<PlaceofTradeNarrative>
					<xsl:value-of select="'Place of Trade Narrative'"/>
				</PlaceofTradeNarrative>

				<ResearchFee>
					<xsl:value-of select="'Research Fee'"/>
				</ResearchFee>

				<TaxID>
					<xsl:value-of select="'Tax ID'"/>
				</TaxID>

				<RegistrationDetails>
					<xsl:value-of select="'Registration Details'"/>
				</RegistrationDetails>

				<EntityID>
					<xsl:value-of select="''"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>
			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<xsl:variable name="varNetamount">
					<xsl:choose>
						<xsl:when test="contains(Side,'Buy')">
							<xsl:value-of select="((OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Sell')">
							<xsl:value-of select="((OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee))"/>
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

							<xsl:variable name ="varAllocationState">
								<xsl:choose>
									<xsl:when test ="TaxLotState = 'Allocated'">
										<xsl:value-of  select="'NEW'"/>
									</xsl:when>

									<xsl:when test ="TaxLotState = 'Deleted'">
										<xsl:value-of  select="'CXL'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of  select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<FunctionoftheMessage>
								<xsl:value-of select="$varAllocationState"/>
							</FunctionoftheMessage>

							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<TradeType>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'Buy to Close'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</TradeType>


							<AccountNo>
								<xsl:value-of select="FundAccntNo"/>
							</AccountNo>



							<xsl:variable name="varTradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
							</TradeDate>

							<xsl:variable name="varSettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<SettlementDate>
								<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SettlementDate>

							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

							<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BNY']/BrokerData[@ThirdPartyBrokerID=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
							</xsl:variable>

							<xsl:variable name="DTC">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BNY']/BrokerData[@ThirdPartyBrokerID=$PRANA_COUNTERPARTY_NAME]/@BrokerBPS"/>
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

							<xsl:variable name="varCurr">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:variable>

							<xsl:variable name="BrokerCode">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@BrokerID"/>
							</xsl:variable>

							<xsl:variable name="ClearerCode">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@ClearerID"/>
							</xsl:variable>

							<BrokerID>
								<xsl:value-of select="$BrokerCode"/>
							</BrokerID>

							<ClearerID>
								<xsl:value-of select="$ClearerCode"/>
							</ClearerID>



							<xsl:variable name="varSettFxAmt">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:choose>
											<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
												<xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AvgPrice"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varPrice">
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varSettFxAmt"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<Price>
								<xsl:value-of select="format-number($varPrice,'0.####')"/>
							</Price>

							<AssetType>
								<xsl:value-of select="''"/>
							</AssetType>

							<Currency>
								<xsl:value-of select="SettlCurrency"/>
							</Currency>

							<LocalCost>
								<xsl:value-of select="''"/>
							</LocalCost>


							<xsl:variable name="varTCommission">
								<xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varTCommission"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varTCommission * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varTCommission div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<Commission>
								<xsl:choose>
									<xsl:when test="number($varCommission)">
										<xsl:value-of select="format-number($varCommission,'0.##')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Commission>

							<BrokerAccount>
								<xsl:value-of select="''"/>
							</BrokerAccount>

							<SECFee>
								<xsl:value-of select="format-number(SecFee,'0.##')"/>
							</SECFee>

							<OtherCharges>
								<xsl:value-of select="format-number (OtherBrokerFees + ClearingFee + OrfFee + TransactionLevy + TaxOnCommissions,'0.##')"/>
							</OtherCharges>

							<xsl:variable name="varPrincipal">
								<xsl:choose>
									<xsl:when test="FXConversionMethodOperator='M'">
										<xsl:value-of select="OrderQty * AvgPrice * FXRate"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="OrderQty * AvgPrice div FXRate"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<Principal>
								<xsl:value-of select="format-number($varPrincipal,'0.##')"/>
							</Principal>

							<Interest>
								<xsl:value-of select="''"/>
							</Interest>

							<xsl:variable name="varFXRateFORNEW">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varNetamountS">
								<xsl:choose>
									<xsl:when test="contains(Side,'Buy')">
										<xsl:value-of select="((OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Sell')">
										<xsl:value-of select="((OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee))"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varFinalMoney">
								<xsl:choose>
									<xsl:when test="$varFXRateFORNEW=0">
										<xsl:value-of select="$varNetamountS"/>
									</xsl:when>
									<xsl:when test="$varFXRateFORNEW!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varNetamountS * $varFXRateFORNEW"/>
									</xsl:when>

									<xsl:when test="$varFXRateFORNEW!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varNetamountS div $varFXRateFORNEW"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<FinalMoney>
								<xsl:choose>
									<xsl:when test="number($varFinalMoney)">
										<xsl:value-of select="format-number($varFinalMoney,'0.##')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</FinalMoney>

							<xsl:variable name="varSecurityIDBasedOnCurrency">
								<xsl:choose>
									<xsl:when test="CurrencySymbol ='USD'">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="SEDOL"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<SecurityID>
								<xsl:value-of select="$varSecurityIDBasedOnCurrency"/>
							</SecurityID>

							<SecurityDescription>
								<xsl:value-of select="CompanyName"/>
							</SecurityDescription>

							<MaturityDate>
								<xsl:value-of select="''"/>
							</MaturityDate>

							<IssueDate>
								<xsl:value-of select="''"/>
							</IssueDate>

							<CurrentRate>
								<xsl:value-of select="''"/>
							</CurrentRate>

							<SafekeepingPlace>
								<xsl:value-of select="''"/>
							</SafekeepingPlace>

							<xsl:variable name ="varAgentCountry">
								<xsl:value-of select ="CurrencySymbol"/>
							</xsl:variable>

							<xsl:variable name ="varPlaceOfSettlementMapping">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name= 'BNY']/InstructionData[@Currency=$varAgentCountry]/@SettlementPlace"/>
							</xsl:variable>
							<SettlementPlace>
								<xsl:choose>
									<xsl:when test="$varPlaceOfSettlementMapping!=''">
										<xsl:value-of select="$varPlaceOfSettlementMapping"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varAgentCountry"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlementPlace>

							<Reference>
								<xsl:value-of select="PBUniqueID"/>
							</Reference>

							<StampDuty>
								<xsl:value-of select="format-number(StampDuty,'0.##')"/>
							</StampDuty>

							<OriginalFace>
								<xsl:value-of select="''"/>
							</OriginalFace>

							<FXExecute>
								<xsl:value-of select="''"/>

							</FXExecute>

							<BuySellCurrency>
								<xsl:value-of select="''"/>
							</BuySellCurrency>

							<FXSpecial>
								<xsl:value-of select="''"/>
							</FXSpecial>

							<Market>
								<xsl:value-of select="'US'"/>
							</Market>

							<SpecialInstruction>
								<xsl:value-of select="''"/>
							</SpecialInstruction>

							<BlockDetailCounter>
								<xsl:value-of select="''"/>
							</BlockDetailCounter>

							<RelatedReference>
								<xsl:value-of select="''"/>
							</RelatedReference>

							<DeliverToAccount>
								<xsl:value-of select="''"/>
							</DeliverToAccount>

							<Quantity>
								<xsl:value-of select="OrderQty"/>
							</Quantity>

							<PoolNumber>
								<xsl:value-of select="''"/>
							</PoolNumber>

							<Factor>
								<xsl:value-of select="''"/>
							</Factor>

							<ADELDate>
								<xsl:value-of select="''"/>
							</ADELDate>

							<Taxes>
								<xsl:value-of select="''"/>
							</Taxes>

							<SettlementMethod>
								<xsl:value-of select="'NORMAL'"/>
							</SettlementMethod>

							<xsl:variable name="varSecurityIDTypeBasedOnCurrency">
								<xsl:choose>
									<xsl:when test="CurrencySymbol ='USD'">
										<xsl:value-of select="'CUSIP'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'SEDOL'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<SecurityIDType>
								<xsl:value-of select="$varSecurityIDTypeBasedOnCurrency"/>
							</SecurityIDType>


							<Investor>
								<xsl:value-of select="''"/>
							</Investor>

							<xsl:variable name="varBrokerIdType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@BrokerIDType"/>
							</xsl:variable>

							<BrokerIDType>
								<xsl:value-of select="$varBrokerIdType"/>
							</BrokerIDType>

							<BrokerDescription>
								<!--<xsl:value-of select="'Sanders Morris Harris LLC'"/>-->
								<xsl:choose>
									<xsl:when test="CounterParty='JONES'">
										<xsl:value-of select="'JONES'"/>
									</xsl:when>
									<xsl:when test="CounterParty='MACQ'">
										<xsl:value-of select="'Macquarie'"/>
									</xsl:when>
									<xsl:when test="CounterParty='JONE'">
										<xsl:value-of select="'JONE'"/>
									</xsl:when>
									<xsl:when test="CounterParty='OPCO'">
										<xsl:value-of select="'Oppenheimer'"/>
									</xsl:when>
									<xsl:when test="CounterParty='UBSS'">
										<xsl:value-of select="'UBS'"/>
									</xsl:when>
									<xsl:when test="CounterParty='ISIG'">
										<xsl:value-of select="'Evercore'"/>
									</xsl:when>
									<xsl:when test="CounterParty='SIDC'">
										<xsl:value-of select="'Sidoti'"/>
									</xsl:when>
									<xsl:when test="CounterParty='ROTH'">
										<xsl:value-of select="'Roth Capital'"/>
									</xsl:when>
									<xsl:when test="CounterParty='JPMS'">
										<xsl:value-of select="'JP Morgan'"/>
									</xsl:when>
									<xsl:when test="CounterParty='COLL'">
										<xsl:value-of select="'Colliers/Dougherty'"/>
									</xsl:when>
									<xsl:when test="CounterParty='PIPR'">
										<xsl:value-of select="'Piper Sandler'"/>
									</xsl:when>
									<xsl:when test="CounterParty='JEFF'">
										<xsl:value-of select="'Jefferies'"/>
									</xsl:when>
									<xsl:when test="CounterParty='WBLR'">
										<xsl:value-of select="'William Blair'"/>
									</xsl:when>
									<xsl:when test="CounterParty='COWN'">
										<xsl:value-of select="'Cowen'"/>
									</xsl:when>
									<xsl:when test="CounterParty='KING'">
										<xsl:value-of select="'CL King'"/>
									</xsl:when>
									<xsl:when test="CounterParty='CJSC'">
										<xsl:value-of select="'CJS Securties'"/>
									</xsl:when>
									<xsl:when test="CounterParty='CHLM'">
										<xsl:value-of select="'Craig Hallum'"/>
									</xsl:when>
									<xsl:when test="CounterParty='ADAM'">
										<xsl:value-of select="'Canaccord'"/>
									</xsl:when>
									<xsl:when test="CounterParty='DOTC'">
										<xsl:value-of select="'Dougherty Colliers'"/>
									</xsl:when>
								</xsl:choose>
							</BrokerDescription>


							<xsl:variable name="varClearerIdType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@ClearerIDType"/>
							</xsl:variable>

							<ClearerIDType>
								<xsl:value-of select="$varClearerIdType"/>
							</ClearerIDType>

							<ClearerDescription>
								<xsl:value-of select="''"/>
							</ClearerDescription>


							<ClearerAccount>
								<xsl:value-of select="''"/>
							</ClearerAccount>

							<CustodianIDType>
								<xsl:value-of select="''"/>
							</CustodianIDType>

							<CustodianID>
								<xsl:value-of select="''"/>
							</CustodianID>

							<CustodianDescription>
								<xsl:value-of select="''"/>
							</CustodianDescription>

							<CustodianAccount>
								<xsl:value-of select="''"/>
							</CustodianAccount>

							<BaseCost>
								<xsl:value-of select="''"/>
							</BaseCost>

							<BaseCurrency>
								<xsl:value-of select="''"/>
							</BaseCurrency>

							<LocalCurrency>
								<xsl:value-of select="''"/>
							</LocalCurrency>


							<ChangeOwnerReg>
								<xsl:value-of select="''"/>
							</ChangeOwnerReg>

							<SettlementIndicator>
								<xsl:value-of select="''"/>
							</SettlementIndicator>


							<ItalianTaxID>
								<xsl:value-of select="''"/>
							</ItalianTaxID>

							<SettlementTransactionIndicator>
								<xsl:value-of select="'TRAD'"/>
							</SettlementTransactionIndicator>

							<Sub-CustodianSpecialInst>
								<xsl:value-of select="''"/>
							</Sub-CustodianSpecialInst>

							<InventoryBook>
								<xsl:value-of select="''"/>
							</InventoryBook>



							<ManualBrokerDescriptionFlagBook>
								<xsl:value-of select="''"/>
							</ManualBrokerDescriptionFlagBook>

							<ManualBrokerDescription>
								<xsl:value-of select="''"/>
							</ManualBrokerDescription>

							<IIJNumber>
								<xsl:value-of select="''"/>
							</IIJNumber>

							<TrackingIndicator>
								<xsl:value-of select="''"/>
							</TrackingIndicator>

							<TaxCode>
								<xsl:value-of select="''"/>
							</TaxCode>

							<TaxCodeDeliver>
								<xsl:value-of select="''"/>
							</TaxCodeDeliver>

							<TaxCodeReceive>
								<xsl:value-of select="''"/>
							</TaxCodeReceive>

							<CurrentFaceQuantity>
								<xsl:value-of select="''"/>
							</CurrentFaceQuantity>

							<AccountingDescriptionLine1>
								<xsl:value-of select="''"/>
							</AccountingDescriptionLine1>

							<AccountingDescriptionLine2>
								<xsl:value-of select="''"/>
							</AccountingDescriptionLine2>

							<AccountingDescriptionReceiveLine1>
								<xsl:value-of select="''"/>
							</AccountingDescriptionReceiveLine1>

							<AccountingDescriptionReceiveLine2>
								<xsl:value-of select="''"/>
							</AccountingDescriptionReceiveLine2>


							<TradeConditionIndicator>
								<xsl:value-of select="''"/>
							</TradeConditionIndicator>

							<DealReference>
								<xsl:value-of select="''"/>
							</DealReference>

							<CommonTradeReference>
								<xsl:value-of select="''"/>
							</CommonTradeReference>

							<PlaceofTrade>
								<xsl:value-of select="''"/>
							</PlaceofTrade>

							<PlaceofTradeNarrative>
								<xsl:value-of select="''"/>
							</PlaceofTradeNarrative>

							<ResearchFee>
								<xsl:value-of select="''"/>
							</ResearchFee>

							<TaxID>
								<xsl:value-of select="''"/>
							</TaxID>

							<RegistrationDetails>
								<xsl:value-of select="''"/>
							</RegistrationDetails>




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

								<xsl:variable name ="varAllocationState">
									<xsl:value-of  select="'CXL'"/>
								</xsl:variable>

								<FunctionoftheMessage>
									<xsl:value-of select="$varAllocationState"/>
								</FunctionoftheMessage>

								<xsl:variable name="varFXRate">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != CurrencySymbol">
											<xsl:value-of select="OldFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<TradeType>
									<xsl:choose>
										<xsl:when test="OldSide='Buy'">
											<xsl:value-of select="'BUY'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell'">
											<xsl:value-of select="'SELL'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell short'">
											<xsl:value-of select="'Sell Short'"/>
										</xsl:when>
										<xsl:when test="OldSide='Buy to Close'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</TradeType>


								<AccountNo>
									<xsl:value-of select="FundAccntNo"/>
								</AccountNo>


								<xsl:variable name="varTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<TradeDate>
									<xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
								</TradeDate>

								<xsl:variable name="varSettlementDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<SettlementDate>
									<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
								</SettlementDate>

								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="OldCounterparty"/>

								<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BNY']/BrokerData[@ThirdPartyBrokerID=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
								</xsl:variable>

								<xsl:variable name="DTC">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BNY']/BrokerData[@ThirdPartyBrokerID=$PRANA_COUNTERPARTY_NAME]/@BrokerBPS"/>
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

								<xsl:variable name="varCurr">
									<xsl:value-of select="CurrencySymbol"/>
								</xsl:variable>

								<xsl:variable name="BrokerCode">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@BrokerID"/>
								</xsl:variable>

								<xsl:variable name="ClearerCode">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@ClearerID"/>
								</xsl:variable>

								<BrokerID>
									<xsl:value-of select="$BrokerCode"/>
								</BrokerID>

								<ClearerID>
									<xsl:value-of select="$ClearerCode"/>
								</ClearerID>

								<xsl:variable name="varSettFxAmt">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != CurrencySymbol">
											<xsl:choose>
												<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
													<xsl:value-of select="OldAvgPrice * FXRate_Taxlot"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="OldAvgPrice div FXRate_Taxlot"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="varPrice">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency = CurrencySymbol">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varSettFxAmt"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>


								<Price>
									<xsl:value-of select="format-number($varPrice,'0.####')"/>
								</Price>

								<AssetType>
									<xsl:value-of select="''"/>
								</AssetType>

								<Currency>
									<xsl:value-of select="SettlCurrency"/>
								</Currency>

								<LocalCost>
									<xsl:value-of select="''"/>
								</LocalCost>



								<xsl:variable name="varTCommission">
									<xsl:value-of select="OldSoftCommission + OldCommission"/>
								</xsl:variable>

								<xsl:variable name="varCommission">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="$varTCommission"/>
										</xsl:when>
										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
											<xsl:value-of select="$varTCommission * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
											<xsl:value-of select="$varTCommission div $varFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<Commission>
									<xsl:choose>
										<xsl:when test="number($varCommission)">
											<xsl:value-of select="format-number($varCommission,'0.##')"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</Commission>

								<BrokerAccount>
									<xsl:value-of select="''"/>
								</BrokerAccount>

								<SECFee>
									<xsl:value-of select="format-number(OldSecFee,'0.##')"/>
								</SECFee>

								<OtherCharges>
									<xsl:value-of select="format-number (OldOtherBrokerFees + OldClearingFee + OldOccFee + OldTransactionLevy + OldTaxOnCommissions,'0.##')"/>
								</OtherCharges>

								<xsl:variable name="varPrincipal">
									<xsl:value-of select="OldExecutedQuantity * OldAvgPrice"/>
								</xsl:variable>

								<xsl:variable name="varPrincipalAmount">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="$varPrincipal"/>
										</xsl:when>
										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
											<xsl:value-of select="$varPrincipal * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
											<xsl:value-of select="$varPrincipal div $varFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<Principal>
									<xsl:value-of select="format-number($varPrincipalAmount,'0.##')"/>
								</Principal>

								<Interest>
									<xsl:value-of select="''"/>
								</Interest>

								<xsl:variable name="varOldNetAmount">
									<xsl:choose>
										<xsl:when test="contains(OldSide,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee) "/>
										</xsl:when>
										<xsl:when test="contains(OldSide,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>






								<xsl:variable name="varFXRatecanNew">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != CurrencySymbol">
											<xsl:value-of select="OldFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="varOldFinalMoney">
									<xsl:choose>
										<xsl:when test="$varFXRatecanNew=0">
											<xsl:value-of select="$varOldNetAmount"/>
										</xsl:when>
										<xsl:when test="$varFXRatecanNew!=0 and FXConversionMethodOperator_Taxlot='M'">
											<xsl:value-of select="$varOldNetAmount * $varFXRatecanNew"/>
										</xsl:when>

										<xsl:when test="$varFXRatecanNew!=0 and FXConversionMethodOperator_Taxlot='D'">
											<xsl:value-of select="$varOldNetAmount div $varFXRatecanNew"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<FinalMoney>
									<xsl:choose>
										<xsl:when test="number($varOldFinalMoney)">
											<xsl:value-of select="format-number($varOldFinalMoney,'0.##')"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>

								</FinalMoney>

								<xsl:variable name="varSecurityIDBasedOnCurrency">
									<xsl:choose>
										<xsl:when test="CurrencySymbol ='USD'">
											<xsl:value-of select="CUSIP"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="SEDOL"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<SecurityID>
									<xsl:value-of select="$varSecurityIDBasedOnCurrency"/>
								</SecurityID>

								<SecurityDescription>
									<xsl:value-of select="CompanyName"/>
								</SecurityDescription>

								<MaturityDate>
									<xsl:value-of select="''"/>
								</MaturityDate>

								<IssueDate>
									<xsl:value-of select="''"/>
								</IssueDate>

								<CurrentRate>
									<xsl:value-of select="''"/>
								</CurrentRate>

								<SafekeepingPlace>
									<xsl:value-of select="''"/>
								</SafekeepingPlace>

								<xsl:variable name ="varAgentCountry">
									<xsl:value-of select ="CurrencySymbol"/>
								</xsl:variable>

								<xsl:variable name ="varPlaceOfSettlementMapping">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name= 'BNY']/InstructionData[@Currency=$varAgentCountry]/@SettlementPlace"/>
								</xsl:variable>
								<SettlementPlace>
									<xsl:choose>
										<xsl:when test="$varPlaceOfSettlementMapping!=''">
											<xsl:value-of select="$varPlaceOfSettlementMapping"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varAgentCountry"/>
										</xsl:otherwise>
									</xsl:choose>
								</SettlementPlace>

								<Reference>
									<xsl:value-of select="PBUniqueID"/>
								</Reference>

								<StampDuty>
									<xsl:value-of select="format-number(OldStampDuty,'0.##')"/>
								</StampDuty>

								<OriginalFace>
									<xsl:value-of select="''"/>
								</OriginalFace>

								<FXExecute>
									<xsl:value-of select="''"/>

								</FXExecute>

								<BuySellCurrency>
									<xsl:value-of select="''"/>
								</BuySellCurrency>

								<FXSpecial>
									<xsl:value-of select="''"/>
								</FXSpecial>

								<Market>
									<xsl:value-of select="'US'"/>
								</Market>

								<SpecialInstruction>
									<xsl:value-of select="''"/>
								</SpecialInstruction>

								<BlockDetailCounter>
									<xsl:value-of select="''"/>
								</BlockDetailCounter>

								<RelatedReference>
									<xsl:value-of select="''"/>
								</RelatedReference>

								<DeliverToAccount>
									<xsl:value-of select="''"/>
								</DeliverToAccount>

								<Quantity>
									<xsl:value-of select="OldExecutedQuantity"/>
								</Quantity>

								<PoolNumber>
									<xsl:value-of select="''"/>
								</PoolNumber>

								<Factor>
									<xsl:value-of select="''"/>
								</Factor>

								<ADELDate>
									<xsl:value-of select="''"/>
								</ADELDate>

								<Taxes>
									<xsl:value-of select="''"/>
								</Taxes>

								<SettlementMethod>
									<xsl:value-of select="'NORMAL'"/>
								</SettlementMethod>

								<xsl:variable name="varSecurityIDTypeBasedOnCurrency">
									<xsl:choose>
										<xsl:when test="CurrencySymbol ='USD'">
											<xsl:value-of select="'CUSIP'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'SEDOL'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<SecurityIDType>
									<xsl:value-of select="$varSecurityIDTypeBasedOnCurrency"/>
								</SecurityIDType>


								<Investor>
									<xsl:value-of select="''"/>
								</Investor>

								<xsl:variable name="varBrokerIdType">

									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@BrokerIDType"/>
								</xsl:variable>

								<BrokerIDType>
									<xsl:value-of select="$varBrokerIdType"/>
								</BrokerIDType>

								<BrokerDescription>
									<!--<xsl:value-of select="'Sanders Morris Harris LLC'"/>-->
									<xsl:choose>
										<xsl:when test="OldCounterparty='JONES'">
											<xsl:value-of select="'JONES'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='MACQ'">
											<xsl:value-of select="'Macquarie'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='JONE'">
											<xsl:value-of select="'JONE'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='OPCO'">
											<xsl:value-of select="'Oppenheimer'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='UBSS'">
											<xsl:value-of select="'UBS'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='ISIG'">
											<xsl:value-of select="'Evercore'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='SIDC'">
											<xsl:value-of select="'Sidoti'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='ROTH'">
											<xsl:value-of select="'Roth Capital'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='JPMS'">
											<xsl:value-of select="'JP Morgan'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='COLL'">
											<xsl:value-of select="'Colliers/Dougherty'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='PIPR'">
											<xsl:value-of select="'Piper Sandler'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='JEFF'">
											<xsl:value-of select="'Jefferies'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='WBLR'">
											<xsl:value-of select="'William Blair'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='COWN'">
											<xsl:value-of select="'Cowen'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='KING'">
											<xsl:value-of select="'CL King'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='CJSC'">
											<xsl:value-of select="'CJS Securties'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='CHLM'">
											<xsl:value-of select="'Craig Hallum'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='ADAM'">
											<xsl:value-of select="'Canaccord'"/>
										</xsl:when>
										<xsl:when test="OldCounterparty='DOTC'">
											<xsl:value-of select="'Dougherty Colliers'"/>
										</xsl:when>
									</xsl:choose>
								</BrokerDescription>


								<xsl:variable name="varClearerIdType">

									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@ClearerIDType"/>
								</xsl:variable>

								<ClearerIDType>
									<xsl:value-of select="$varClearerIdType"/>
								</ClearerIDType>

								<ClearerDescription>
									<xsl:value-of select="''"/>
								</ClearerDescription>


								<ClearerAccount>
									<xsl:value-of select="''"/>
								</ClearerAccount>

								<CustodianIDType>
									<xsl:value-of select="''"/>
								</CustodianIDType>

								<CustodianID>
									<xsl:value-of select="''"/>
								</CustodianID>

								<CustodianDescription>
									<xsl:value-of select="''"/>
								</CustodianDescription>

								<CustodianAccount>
									<xsl:value-of select="''"/>
								</CustodianAccount>

								<BaseCost>
									<xsl:value-of select="''"/>
								</BaseCost>

								<BaseCurrency>
									<xsl:value-of select="''"/>
								</BaseCurrency>

								<LocalCurrency>
									<xsl:value-of select="''"/>
								</LocalCurrency>


								<ChangeOwnerReg>
									<xsl:value-of select="''"/>
								</ChangeOwnerReg>

								<SettlementIndicator>
									<xsl:value-of select="''"/>
								</SettlementIndicator>


								<ItalianTaxID>
									<xsl:value-of select="''"/>
								</ItalianTaxID>

								<SettlementTransactionIndicator>
									<xsl:value-of select="'TRAD'"/>
								</SettlementTransactionIndicator>

								<Sub-CustodianSpecialInst>
									<xsl:value-of select="''"/>
								</Sub-CustodianSpecialInst>

								<InventoryBook>
									<xsl:value-of select="''"/>
								</InventoryBook>



								<ManualBrokerDescriptionFlagBook>
									<xsl:value-of select="''"/>
								</ManualBrokerDescriptionFlagBook>

								<ManualBrokerDescription>
									<xsl:value-of select="''"/>
								</ManualBrokerDescription>

								<IIJNumber>
									<xsl:value-of select="''"/>
								</IIJNumber>

								<TrackingIndicator>
									<xsl:value-of select="''"/>
								</TrackingIndicator>

								<TaxCode>
									<xsl:value-of select="''"/>
								</TaxCode>

								<TaxCodeDeliver>
									<xsl:value-of select="''"/>
								</TaxCodeDeliver>

								<TaxCodeReceive>
									<xsl:value-of select="''"/>
								</TaxCodeReceive>

								<CurrentFaceQuantity>
									<xsl:value-of select="''"/>
								</CurrentFaceQuantity>

								<AccountingDescriptionLine1>
									<xsl:value-of select="''"/>
								</AccountingDescriptionLine1>

								<AccountingDescriptionLine2>
									<xsl:value-of select="''"/>
								</AccountingDescriptionLine2>

								<AccountingDescriptionReceiveLine1>
									<xsl:value-of select="''"/>
								</AccountingDescriptionReceiveLine1>

								<AccountingDescriptionReceiveLine2>
									<xsl:value-of select="''"/>
								</AccountingDescriptionReceiveLine2>


								<TradeConditionIndicator>
									<xsl:value-of select="''"/>
								</TradeConditionIndicator>

								<DealReference>
									<xsl:value-of select="''"/>
								</DealReference>

								<CommonTradeReference>
									<xsl:value-of select="''"/>
								</CommonTradeReference>

								<PlaceofTrade>
									<xsl:value-of select="''"/>
								</PlaceofTrade>

								<PlaceofTradeNarrative>
									<xsl:value-of select="''"/>
								</PlaceofTradeNarrative>

								<ResearchFee>
									<xsl:value-of select="''"/>
								</ResearchFee>

								<TaxID>
									<xsl:value-of select="''"/>
								</TaxID>

								<RegistrationDetails>
									<xsl:value-of select="''"/>
								</RegistrationDetails>



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

							<xsl:variable name ="varAllocationState">
								<xsl:value-of  select="'NEW'"/>
							</xsl:variable>

							<FunctionoftheMessage>
								<xsl:value-of select="$varAllocationState"/>
							</FunctionoftheMessage>

							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<TradeType>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'BUY'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'SELL'"/>
									</xsl:when>
									<xsl:when test="Side='Sell short'">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>
									<xsl:when test="Side='Buy to Close'">
										<xsl:value-of select="'Buy to Close'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</TradeType>


							<AccountNo>
								<xsl:value-of select="FundAccntNo"/>
							</AccountNo>



							<xsl:variable name="varTradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
							</TradeDate>

							<xsl:variable name="varSettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<SettlementDate>
								<xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SettlementDate>

							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

							<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BNY']/BrokerData[@ThirdPartyBrokerID=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
							</xsl:variable>

							<xsl:variable name="DTC">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BNY']/BrokerData[@ThirdPartyBrokerID=$PRANA_COUNTERPARTY_NAME]/@BrokerBPS"/>
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

							<xsl:variable name="varCurr">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:variable>

							<xsl:variable name="BrokerCode">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@BrokerID"/>
							</xsl:variable>

							<xsl:variable name="ClearerCode">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@ClearerID"/>
							</xsl:variable>

							<BrokerID>
								<xsl:value-of select="$BrokerCode"/>
							</BrokerID>

							<ClearerID>
								<xsl:value-of select="$ClearerCode"/>
							</ClearerID>

							<xsl:variable name="varSettFxAmt">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:choose>
											<xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
												<xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="AvgPrice"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varPrice">
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varSettFxAmt"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<Price>
								<xsl:value-of select="format-number($varPrice,'0.####')"/>
							</Price>

							<AssetType>
								<xsl:value-of select="''"/>
							</AssetType>

							<Currency>
								<xsl:value-of select="SettlCurrency"/>
							</Currency>

							<LocalCost>
								<xsl:value-of select="''"/>
							</LocalCost>


							<xsl:variable name="varTCommission">
								<xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
							</xsl:variable>

							<xsl:variable name="varCommission">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varTCommission"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varTCommission * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varTCommission div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<Commission>
								<xsl:choose>
									<xsl:when test="number($varCommission)">
										<xsl:value-of select="format-number($varCommission,'0.##')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Commission>

							<BrokerAccount>
								<xsl:value-of select="''"/>
							</BrokerAccount>

							<SECFee>
								<xsl:value-of select="format-number(SecFee,'0.##')"/>
							</SECFee>

							<OtherCharges>
								<xsl:value-of select="format-number (OtherBrokerFees + ClearingFee + OrfFee + TransactionLevy + TaxOnCommissions,'0.##')"/>
							</OtherCharges>

							<xsl:variable name="varPrincipal">
								<xsl:value-of select="OrderQty * AvgPrice"/>
							</xsl:variable>

							<xsl:variable name="varPrincipalAmount">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varPrincipal"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varPrincipal * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varPrincipal div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<Principal>
								<xsl:value-of select="format-number($varPrincipalAmount,'0.##')"/>
							</Principal>

							<Interest>
								<xsl:value-of select="''"/>
							</Interest>


							<xsl:variable name="varFinalMoney">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
										<xsl:value-of select="$varNetamount * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
										<xsl:value-of select="$varNetamount div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<FinalMoney>
								<xsl:choose>
									<xsl:when test="number($varFinalMoney)">
										<xsl:value-of select="format-number($varFinalMoney,'0.##')"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</FinalMoney>

							<xsl:variable name="varSecurityIDBasedOnCurrency">
								<xsl:choose>
									<xsl:when test="CurrencySymbol ='USD'">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="SEDOL"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<SecurityID>
								<xsl:value-of select="$varSecurityIDBasedOnCurrency"/>
							</SecurityID>

							<SecurityDescription>
								<xsl:value-of select="CompanyName"/>
							</SecurityDescription>

							<MaturityDate>
								<xsl:value-of select="''"/>
							</MaturityDate>

							<IssueDate>
								<xsl:value-of select="''"/>
							</IssueDate>

							<CurrentRate>
								<xsl:value-of select="''"/>
							</CurrentRate>

							<SafekeepingPlace>
								<xsl:value-of select="''"/>
							</SafekeepingPlace>

							<xsl:variable name ="varAgentCountry">
								<xsl:value-of select ="CurrencySymbol"/>
							</xsl:variable>

							<xsl:variable name ="varPlaceOfSettlementMapping">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name= 'BNY']/InstructionData[@Currency=$varAgentCountry]/@SettlementPlace"/>
							</xsl:variable>
							<SettlementPlace>
								<xsl:choose>
									<xsl:when test="$varPlaceOfSettlementMapping!=''">
										<xsl:value-of select="$varPlaceOfSettlementMapping"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varAgentCountry"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlementPlace>

							<Reference>
								<xsl:value-of select="concat(PBUniqueID,position())"/>
							</Reference>

							<StampDuty>
								<xsl:value-of select="format-number(StampDuty,'0.##')"/>
							</StampDuty>

							<OriginalFace>
								<xsl:value-of select="''"/>
							</OriginalFace>

							<FXExecute>
								<xsl:value-of select="''"/>

							</FXExecute>

							<BuySellCurrency>
								<xsl:value-of select="''"/>
							</BuySellCurrency>

							<FXSpecial>
								<xsl:value-of select="''"/>
							</FXSpecial>

							<Market>
								<xsl:value-of select="'US'"/>
							</Market>

							<SpecialInstruction>
								<xsl:value-of select="''"/>
							</SpecialInstruction>

							<BlockDetailCounter>
								<xsl:value-of select="''"/>
							</BlockDetailCounter>

							<RelatedReference>
								<xsl:value-of select="''"/>
							</RelatedReference>

							<DeliverToAccount>
								<xsl:value-of select="''"/>
							</DeliverToAccount>

							<Quantity>
								<xsl:value-of select="OrderQty"/>
							</Quantity>

							<PoolNumber>
								<xsl:value-of select="''"/>
							</PoolNumber>

							<Factor>
								<xsl:value-of select="''"/>
							</Factor>

							<ADELDate>
								<xsl:value-of select="''"/>
							</ADELDate>

							<Taxes>
								<xsl:value-of select="''"/>
							</Taxes>

							<SettlementMethod>
								<xsl:value-of select="'NORMAL'"/>
							</SettlementMethod>

							<xsl:variable name="varSecurityIDTypeBasedOnCurrency">
								<xsl:choose>
									<xsl:when test="CurrencySymbol ='USD'">
										<xsl:value-of select="'CUSIP'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'SEDOL'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<SecurityIDType>
								<xsl:value-of select="$varSecurityIDTypeBasedOnCurrency"/>
							</SecurityIDType>


							<Investor>
								<xsl:value-of select="''"/>
							</Investor>

							<xsl:variable name="varBrokerIdType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@BrokerIDType"/>
							</xsl:variable>

							<BrokerIDType>
								<xsl:value-of select="$varBrokerIdType"/>
							</BrokerIDType>

							<BrokerDescription>
								<!--<xsl:value-of select="'Sanders Morris Harris LLC'"/>-->
								<xsl:choose>
									<xsl:when test="CounterParty='JONES'">
										<xsl:value-of select="'JONES'"/>
									</xsl:when>
									<xsl:when test="CounterParty='MACQ'">
										<xsl:value-of select="'Macquarie'"/>
									</xsl:when>
									<xsl:when test="CounterParty='JONE'">
										<xsl:value-of select="'JONE'"/>
									</xsl:when>
									<xsl:when test="CounterParty='OPCO'">
										<xsl:value-of select="'Oppenheimer'"/>
									</xsl:when>
									<xsl:when test="CounterParty='UBSS'">
										<xsl:value-of select="'UBS'"/>
									</xsl:when>
									<xsl:when test="CounterParty='ISIG'">
										<xsl:value-of select="'Evercore'"/>
									</xsl:when>
									<xsl:when test="CounterParty='SIDC'">
										<xsl:value-of select="'Sidoti'"/>
									</xsl:when>
									<xsl:when test="CounterParty='ROTH'">
										<xsl:value-of select="'Roth Capital'"/>
									</xsl:when>
									<xsl:when test="CounterParty='JPMS'">
										<xsl:value-of select="'JP Morgan'"/>
									</xsl:when>
									<xsl:when test="CounterParty='COLL'">
										<xsl:value-of select="'Colliers/Dougherty'"/>
									</xsl:when>
									<xsl:when test="CounterParty='PIPR'">
										<xsl:value-of select="'Piper Sandler'"/>
									</xsl:when>
									<xsl:when test="CounterParty='JEFF'">
										<xsl:value-of select="'Jefferies'"/>
									</xsl:when>
									<xsl:when test="CounterParty='WBLR'">
										<xsl:value-of select="'William Blair'"/>
									</xsl:when>
									<xsl:when test="CounterParty='COWN'">
										<xsl:value-of select="'Cowen'"/>
									</xsl:when>
									<xsl:when test="CounterParty='KING'">
										<xsl:value-of select="'CL King'"/>
									</xsl:when>
									<xsl:when test="CounterParty='CJSC'">
										<xsl:value-of select="'CJS Securties'"/>
									</xsl:when>
									<xsl:when test="CounterParty='CHLM'">
										<xsl:value-of select="'Craig Hallum'"/>
									</xsl:when>
									<xsl:when test="CounterParty='ADAM'">
										<xsl:value-of select="'Canaccord'"/>
									</xsl:when>
									<xsl:when test="CounterParty='DOTC'">
										<xsl:value-of select="'Dougherty Colliers'"/>
									</xsl:when>
								</xsl:choose>
							</BrokerDescription>


							<xsl:variable name="varClearerIdType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SettlementInstructionMapping.xml')/InstructionMapping/PB[@Name='BNY']/InstructionData[@Currency=$varCurr]/@ClearerIDType"/>
							</xsl:variable>

							<ClearerIDType>
								<xsl:value-of select="$varClearerIdType"/>
							</ClearerIDType>

							<ClearerDescription>
								<xsl:value-of select="''"/>
							</ClearerDescription>


							<ClearerAccount>
								<xsl:value-of select="''"/>
							</ClearerAccount>

							<CustodianIDType>
								<xsl:value-of select="''"/>
							</CustodianIDType>

							<CustodianID>
								<xsl:value-of select="''"/>
							</CustodianID>

							<CustodianDescription>
								<xsl:value-of select="''"/>
							</CustodianDescription>

							<CustodianAccount>
								<xsl:value-of select="''"/>
							</CustodianAccount>

							<BaseCost>
								<xsl:value-of select="''"/>
							</BaseCost>

							<BaseCurrency>
								<xsl:value-of select="''"/>
							</BaseCurrency>

							<LocalCurrency>
								<xsl:value-of select="''"/>
							</LocalCurrency>


							<ChangeOwnerReg>
								<xsl:value-of select="''"/>
							</ChangeOwnerReg>

							<SettlementIndicator>
								<xsl:value-of select="''"/>
							</SettlementIndicator>


							<ItalianTaxID>
								<xsl:value-of select="''"/>
							</ItalianTaxID>

							<SettlementTransactionIndicator>
								<xsl:value-of select="'TRAD'"/>
							</SettlementTransactionIndicator>

							<Sub-CustodianSpecialInst>
								<xsl:value-of select="''"/>
							</Sub-CustodianSpecialInst>

							<InventoryBook>
								<xsl:value-of select="''"/>
							</InventoryBook>



							<ManualBrokerDescriptionFlagBook>
								<xsl:value-of select="''"/>
							</ManualBrokerDescriptionFlagBook>

							<ManualBrokerDescription>
								<xsl:value-of select="''"/>
							</ManualBrokerDescription>

							<IIJNumber>
								<xsl:value-of select="''"/>
							</IIJNumber>

							<TrackingIndicator>
								<xsl:value-of select="''"/>
							</TrackingIndicator>

							<TaxCode>
								<xsl:value-of select="''"/>
							</TaxCode>

							<TaxCodeDeliver>
								<xsl:value-of select="''"/>
							</TaxCodeDeliver>

							<TaxCodeReceive>
								<xsl:value-of select="''"/>
							</TaxCodeReceive>

							<CurrentFaceQuantity>
								<xsl:value-of select="''"/>
							</CurrentFaceQuantity>

							<AccountingDescriptionLine1>
								<xsl:value-of select="''"/>
							</AccountingDescriptionLine1>

							<AccountingDescriptionLine2>
								<xsl:value-of select="''"/>
							</AccountingDescriptionLine2>

							<AccountingDescriptionReceiveLine1>
								<xsl:value-of select="''"/>
							</AccountingDescriptionReceiveLine1>

							<AccountingDescriptionReceiveLine2>
								<xsl:value-of select="''"/>
							</AccountingDescriptionReceiveLine2>


							<TradeConditionIndicator>
								<xsl:value-of select="''"/>
							</TradeConditionIndicator>

							<DealReference>
								<xsl:value-of select="''"/>
							</DealReference>

							<CommonTradeReference>
								<xsl:value-of select="''"/>
							</CommonTradeReference>

							<PlaceofTrade>
								<xsl:value-of select="''"/>
							</PlaceofTrade>

							<PlaceofTradeNarrative>
								<xsl:value-of select="''"/>
							</PlaceofTradeNarrative>

							<ResearchFee>
								<xsl:value-of select="''"/>
							</ResearchFee>

							<TaxID>
								<xsl:value-of select="''"/>
							</TaxID>

							<RegistrationDetails>
								<xsl:value-of select="''"/>
							</RegistrationDetails>


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

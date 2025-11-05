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

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
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


				<EntityID>
					<xsl:value-of select="''"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			
			<xsl:for-each select="ThirdPartyFlatFileDetail">
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
							<xsl:when test ="TaxLotState = 'Amended'">
								<xsl:value-of  select="'COR'"/>
							</xsl:when>
							<xsl:when test ="TaxLotState = 'Deleted'">
								<xsl:value-of  select="'CAN'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of  select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<FunctionoftheMessage>
						<xsl:value-of select="$varAllocationState"/>
					</FunctionoftheMessage>

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

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="PB_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
					</xsl:variable>

					<AccountNo>
						<xsl:choose>
							<xsl:when test ="$PB_FUND_CODE!=''">
								<xsl:value-of select ="$PB_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountNo>


					<xsl:variable name="varYear">
						<xsl:value-of select="substring-after(substring-after(TradeDate,'/'),'/')"/>
					</xsl:variable>

					<xsl:variable name="varMonth">
						<xsl:value-of select="substring-before(TradeDate,'/')"/>
					</xsl:variable>

					<xsl:variable name="varDay">
						<xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
					</xsl:variable>
					<TradeDate>
						<xsl:value-of select="concat($varYear,$varMonth,$varDay)"/>
					</TradeDate>
					
					<xsl:variable name="varSYear">
						<xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
					</xsl:variable>

					<xsl:variable name="varSMonth">
						<xsl:value-of select="substring-before(SettlementDate,'/')"/>
					</xsl:variable>

					<xsl:variable name="varSDay">
						<xsl:value-of select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
					</xsl:variable>

					<SettlementDate>
						<xsl:value-of select="concat($varSYear,$varSMonth,$varSDay)"/>
					</SettlementDate>

					<BrokerID>
						<!--<xsl:value-of select="'443'"/>-->
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'0352'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'0161'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'0443'"/>
							</xsl:when>
						</xsl:choose>
					</BrokerID>

					<ClearerID>
						<!--<xsl:value-of select="'443'"/>-->
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'0352'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'0161'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'0443'"/>
							</xsl:when>
						</xsl:choose>
					</ClearerID>

					<Price>
						<xsl:value-of select="format-number(AveragePrice,'0.####')"/>
					</Price>

					<AssetType>
						<xsl:value-of select="''"/>
					</AssetType>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<LocalCost>
						<xsl:value-of select="''"/>
					</LocalCost>


					<xsl:variable name="varCommission">
						<xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
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
						<!--<xsl:value-of select="format-number(SecFees,'0.##')"/>-->
						<xsl:value-of select="format-number(StampDuty,'0.##')"/>
					</SECFee>

					<OtherCharges>
						<xsl:value-of select="''"/>
					</OtherCharges>

					<xsl:variable name="varPrincipal">
						<xsl:value-of select="AllocatedQty * AveragePrice"/>
					</xsl:variable>
					<Principal>
						<xsl:value-of select="format-number($varPrincipal,'0.##')"/>
					</Principal>

					<Interest>
						<xsl:value-of select="''"/>
					</Interest>

					<FinalMoney>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="format-number(NetAmount,'0.##')"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>

					</FinalMoney>

					<SecurityID>
						<xsl:choose>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="concat(concat('=&quot;',CUSIP),'&quot;')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
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

					<SettlementPlace>
						<xsl:value-of select="'US-DTCYUS33'"/>
					</SettlementPlace>

					<Reference>
						<xsl:value-of select="PBUniqueID"/>
					</Reference>

					<StampDuty>
						<xsl:value-of select="''"/>						
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
						<xsl:value-of select="AllocatedQty"/>
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

					<SecurityIDType>
						<xsl:choose>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="'CUSIP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityIDType>

					<Investor>
						<xsl:value-of select="''"/>
					</Investor>

					<BrokerIDType>
						<xsl:value-of select="'DTC'"/>
					</BrokerIDType>

					<BrokerDescription>
						<!--<xsl:value-of select="'Sanders Morris Harris LLC'"/>-->
						<xsl:choose>
							<xsl:when test="CounterParty='JPM'">
								<xsl:value-of select="'JP Morgan'"/>
							</xsl:when>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'Merrill Lynch Broadcort'"/>
							</xsl:when>
							<xsl:when test="CounterParty='SMHI'">
								<xsl:value-of select="'Sanders Morris Harris LLC'"/>
							</xsl:when>
						</xsl:choose>
					</BrokerDescription>


					<ClearerIDType>
						<xsl:value-of select="'DTC'"/>
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


					<EntityID>
						<xsl:value-of select="''"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

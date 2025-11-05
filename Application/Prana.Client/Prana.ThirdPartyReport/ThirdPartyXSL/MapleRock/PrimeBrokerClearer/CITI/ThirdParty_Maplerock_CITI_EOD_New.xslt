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

				
					<RecordType>
						<xsl:choose>
							<xsl:when test="Asset ='Equity' and IsSwapped ='true'">
								<xsl:value-of select="'SWAP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="'Trade'"/>
							</xsl:otherwise>
						</xsl:choose>						
					</RecordType>



					<TransactionType>
						<xsl:choose>
							<xsl:when test="Asset ='Equity' and IsSwapped ='true'">
								<xsl:choose>
									<xsl:when test="Side ='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'CFP'"/>
									</xsl:when>

									<xsl:when test="Side ='Buy to Close'">
										<xsl:value-of select="'CFP'"/>
									</xsl:when>


									<xsl:when test="Side='Sell short'or Side='Sell to Open'">
										<xsl:value-of select="'CFS'"/>
									</xsl:when>

									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'CFS'"/>
									</xsl:when>

									<xsl:otherwise>										
											<xsl:value-of select="''"/>										
									</xsl:otherwise>

								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="Side ='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'BY'"/>
									</xsl:when>
									<xsl:when test="Side ='Buy to Close'">
										<xsl:value-of select="'BC'"/>
									</xsl:when>

									<xsl:when test="Side='Sell short'or Side='Sell to Open'">
										<xsl:value-of select="'SS'"/>
									</xsl:when>

									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'SL'"/>
									</xsl:when>
									<xsl:otherwise>
										
											<xsl:value-of select="''"/>
										
									</xsl:otherwise>
									
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>						
					</TransactionType>

					


					<ClientAccount>
						<xsl:value-of select ="AccountNo"/>
					</ClientAccount>

					

					<AccountType>

						<xsl:choose>
							<xsl:when test="Asset ='Equity' and IsSwapped ='true'">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="contains(Asset,'Option')">
										<xsl:value-of select="'Margin'"/>
									</xsl:when>
									<xsl:when test="(Asset ='Equity' or Asset ='FxForward' or  Asset ='FX' or Asset ='FixedIncome') and Side ='Buy to Close' or Side ='Sell short'">
										<xsl:value-of select="'Short'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'Margin'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					
					</AccountType>


					<ClientReference>						
						<xsl:value-of select="PBUniqueID"/>
					</ClientReference>


					<xsl:variable name="varInstruction">

						<xsl:choose>
							<xsl:when test="Asset ='Equity' and IsSwapped ='true'">
								<xsl:choose>
									<xsl:when test="TaxLotState = 'Allocated'">
										<xsl:value-of select="'NEW'"/>
									</xsl:when>
									<xsl:when test="TaxLotState = 'Amended'">
										<xsl:value-of select="'NEW'"/>
									</xsl:when>
									<xsl:when test="TaxLotState = 'Deleted'">
										<xsl:value-of select="'CXL'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="TaxLotState = 'Allocated'">
										<xsl:value-of select="'NEW'"/>
									</xsl:when>
									<xsl:when test="TaxLotState = 'Amended'">
										<xsl:value-of select="'MOD'"/>
									</xsl:when>
									<xsl:when test="TaxLotState = 'Deleted'">
										<xsl:value-of select="'CXL'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
						
					</xsl:variable>



					<xsl:variable name="varAmendNo">
						<xsl:choose>
							<xsl:when test="TaxLotState = 'Allocated'">
								<xsl:value-of select="'0'"/>
							</xsl:when>
							<xsl:when test="TaxLotState = 'Amended'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="TaxLotState = 'Deleted'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<AmendNo>
						<xsl:value-of select="'0'"/>
					</AmendNo>


					<Instruction>
						<xsl:value-of select ="$varInstruction"/>
					</Instruction>

					<xsl:variable name="PB_NAME" select="''"/>
					
					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
					<xsl:variable name="THIRDPARTY_BROKER">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
					</xsl:variable>

					<BrokerCode>
				     	<xsl:choose>
							<xsl:when test="$THIRDPARTY_BROKER!=''">
								<xsl:value-of select="$THIRDPARTY_BROKER"/>
							</xsl:when>
							<xsl:otherwise>
				                  <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
				
					</BrokerCode>


					<SecurityID>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select ="OSIOptionSymbol"/>
							</xsl:when>

							<!--<xsl:when test ="Asset='FixedIncome'">
								<xsl:value-of select ="OSIOptionSymbol"/>
							</xsl:when>-->
							
							<xsl:when test ="SEDOL!=''">
								<xsl:value-of select ="SEDOL"/>
							</xsl:when>
							<xsl:when test ="CUSIP!=''">
								<xsl:value-of select ="CUSIP"/>
							</xsl:when>
							<xsl:when test ="ISIN!=''">
								<xsl:value-of select ="ISIN"/>
							</xsl:when>
							<xsl:when test="Asset ='FX'">
								<xsl:value-of select ="concat(substring-before(Symbol,'-'),'*')"/>
							</xsl:when>
							<xsl:when test="Asset ='FXForward'">
								<xsl:value-of select ="concat(substring-before(Symbol,'-'),'*')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityID>

					<SecurityIdType>
						<xsl:choose>
							<xsl:when test ="SEDOL!=''">
								<xsl:value-of select ="'SEDL'"/>
							</xsl:when>
							<xsl:when test ="CUSIP!=''">
								<xsl:value-of select ="'CUSP'"/>
							</xsl:when>
							<xsl:when test ="ISIN!=''">
								<xsl:value-of select ="'ISIN'"/>
							</xsl:when>
							<xsl:when test="Asset ='FX'">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="'TCKR'"/>
							</xsl:otherwise>
						</xsl:choose>
					</SecurityIdType>

					<TradeDate>
						<xsl:value-of select ="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select ="SettlementDate"/>
					</SettlementDate>

					<Quantity>
						<xsl:value-of select ="AllocatedQty"/>
					</Quantity>

					<Price>
						<xsl:value-of select ="AveragePrice"/>
					</Price>

					<TradeCurrency>
						<xsl:value-of select ="CurrencySymbol"/>
					</TradeCurrency>

					<CommissionCode>
						<xsl:value-of select ="'G'"/>						
					</CommissionCode>

					<Commission>

						<xsl:choose>
							<xsl:when test="Asset ='Equity' and IsSwapped ='true'">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:when test="Asset ='FX'">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</Commission>


					<NetAmount>
						<xsl:value-of select ="NetAmount"/>
					</NetAmount>


					<MemoField1>
						<xsl:choose>
							<xsl:when test="Asset ='FX'">
								<xsl:value-of select ="'FXSpot'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>	
					</MemoField1>


					<MemoField2>
						<xsl:value-of select="''"/>
					</MemoField2>


					<RepoRate>
						<xsl:value-of select="''"/>	
					</RepoRate>


					<RepoTerminationDate>
						<xsl:value-of select="''"/>	
					</RepoTerminationDate>


					<RepoEndMoney>
						<xsl:value-of select="''"/>	
					</RepoEndMoney>

					<RepoAccruedInterest>
						<xsl:value-of select="''"/>	
					</RepoAccruedInterest>


					<RepoHaircut>
						<xsl:value-of select="''"/>	
					</RepoHaircut>


					<SettlementLocation>
						<xsl:value-of select="''"/>	
					</SettlementLocation>


					<AccountNo>
						<xsl:value-of select="''"/>	
					</AccountNo>


					<AgentBankName>
						<xsl:value-of select="''"/>	
					</AgentBankName>

					<AgentBankLocation>
						<xsl:value-of select="''"/>
					</AgentBankLocation>

					<AgentBankInstructions>
						<xsl:value-of select="''"/>	
					</AgentBankInstructions>


					<FeeType1>
						<xsl:choose>
							<xsl:when test="Asset ='Equity' and IsSwapped ='true'">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'SEC'"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</FeeType1>


					<FeeValue1>
						<xsl:choose>
							<xsl:when test="Asset ='Equity' and IsSwapped ='true'">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:when test="Asset ='FX'">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SecFees"/>
							</xsl:otherwise>
						</xsl:choose>
							
					</FeeValue1>


					<FeeType2>
						<xsl:choose>
							<xsl:when test="Asset ='FX'">
								<xsl:value-of select ="'MSCF'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</FeeType2>

					<FeeValue2>
						<xsl:value-of select="''"/>	
					</FeeValue2>


					<FeeType3>
						<xsl:value-of select="''"/>	
					</FeeType3>


					<FeeValue3>
						<xsl:value-of select="''"/>	
					</FeeValue3>


					<Strategy>
						<xsl:value-of select="''"/>	
					</Strategy>


					<TaxlotId>
						<xsl:value-of select="''"/>	
					</TaxlotId>


					<PreFiguredIndicator>
						<xsl:value-of select="''"/>	
					</PreFiguredIndicator>


					<BondInterest>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="AccruedInterest"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</BondInterest>


					<BondPrincipal>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="AllocatedQty * AveragePrice * 0.01"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>	
					</BondPrincipal>


					<ProcessingType>
						<xsl:value-of select="''"/>	
					</ProcessingType>


					<BlockId>
						<xsl:value-of select="''"/>	
					</BlockId>

					<ReservedField1>
						<xsl:value-of select="''"/>
					</ReservedField1>

					<ReservedField2>
						<xsl:value-of select="''"/>
					</ReservedField2>

					<ReservedField3>
						<xsl:value-of select="''"/>
					</ReservedField3>

					<BondFactor>
						<xsl:value-of select="''"/>
					</BondFactor>

					<Reserved>
						<xsl:value-of select="''"/>
					</Reserved>

					<ReservedField4>
						<xsl:value-of select="''"/>
					</ReservedField4>

					<ReservedField5>
						<xsl:value-of select="''"/>
					</ReservedField5>
				
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

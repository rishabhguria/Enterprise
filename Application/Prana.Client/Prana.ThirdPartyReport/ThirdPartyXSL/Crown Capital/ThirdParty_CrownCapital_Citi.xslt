<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<!--<ASSET>
						<xsl:value-of select ="Asset"/>
					</ASSET>-->

					<RecordType>
						<xsl:value-of select ="'Trade'"/>
					</RecordType>


					<xsl:choose>
						<xsl:when test="Side ='Buy' or Side='Buy to Open'">
							<TransactionType>
								<xsl:value-of select="'BY'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side ='Buy to Close'">
							<TransactionType>
								<xsl:value-of select="'BC'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side='Sell short'or Side='Sell to Open'">
							<TransactionType>
								<xsl:value-of select="'SS'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<TransactionType>
								<xsl:value-of select="'SL'"/>
							</TransactionType>
						</xsl:when>
						<xsl:otherwise>
							<TransactionType>
								<xsl:value-of select="''"/>
							</TransactionType>
						</xsl:otherwise>
					</xsl:choose>


					<ClientAccount>
						<xsl:value-of select ="FundAccountNo"/>
					</ClientAccount>

					<xsl:choose>
						<xsl:when test ="Asset='EquityOption'">
							<AccountType>
								<xsl:value-of select="'Margin'"/>
							</AccountType>
						</xsl:when>
						<xsl:when test ="(Side='Buy to Close' or Side='Sell short' or Side='Sell to Open') and Asset != 'EquityOption'">
							<AccountType>
								<xsl:value-of select="'Short'"/>
							</AccountType>
						</xsl:when>
						<xsl:when test ="Side='Buy' or Side='Sell' or Side='Buy to Open' and Asset != 'EquityOption'">
							<AccountType>
								<xsl:value-of select="'Margin'"/>
							</AccountType>
						</xsl:when>
						<xsl:otherwise>
							<AccountType>
								<xsl:value-of select="'Margin'"/>
							</AccountType>
						</xsl:otherwise>
					</xsl:choose>


					<ClientReference>
						<xsl:value-of select ="TradeRefID"/>
					</ClientReference>


					<xsl:choose>

						<xsl:when test ="TaxLotState='Allocated'">
							<AmendNo>
								<xsl:value-of select="'0'"/>
							</AmendNo>
							<Instruction>
								<xsl:value-of select ="'NEW'"/>
							</Instruction>
							</xsl:when>
						<xsl:when test ="TaxLotState='Amemded'">
							<AmendNo>
								<xsl:value-of select="'1'"/>
							</AmendNo>
							<Instruction>
								<xsl:value-of select ="'MOD'"/>
							</Instruction>
						</xsl:when>
						<xsl:when test ="TaxLotState='Deleted'">
							<AmendNo>
								<xsl:value-of select="'2'"/>
							</AmendNo>
							<Instruction>
								<xsl:value-of select ="'CXL'"/>
							</Instruction>
						</xsl:when>
						<xsl:otherwise>
							<AmendNo>
								<xsl:value-of select="'0'"/>
							</AmendNo>
							<Instruction>
								<xsl:value-of select ="'NEW'"/>
							</Instruction>
						</xsl:otherwise>
						</xsl:choose>

					<BrokerCode>
						<xsl:value-of select ="CounterParty"/>
					</BrokerCode>



					
							<!--<xsl:choose>
								<xsl:when test ="Asset='EquityOption'">
									<SecurityID>
										<xsl:value-of select="translate(OpraOptionSymbol,' ','+')"/>
									</SecurityID>
									<SecurityIdType>
										<xsl:value-of select="'TCKR'"/>
									</SecurityIdType>
									</xsl:when>
								<xsl:otherwise>
									<SecurityID>
										<xsl:value-of select="Symbol"/>
									</SecurityID>
									<SecurityIdType>
										<xsl:value-of select="'TCKR'"/>
									</SecurityIdType>
								</xsl:otherwise>
							</xsl:choose>-->

					<SecurityID>
						<xsl:value-of select="Symbol"/>
					</SecurityID>
					<SecurityIdType>
						<xsl:value-of select="'TCKR'"/>
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
						<xsl:value-of select ="CommissionCharged"/>
					</Commission>


					<NetAmount>
						<xsl:value-of select ="NetAmount"/>
					</NetAmount>

					<MemoField1>
						<xsl:value-of select ="''"/>
					</MemoField1>

					<MemoField2>
						<xsl:value-of select ="''"/>
					</MemoField2>

					<RepoRate>
						<xsl:value-of select ="''"/>
					</RepoRate>


					<TerminationDateofRepo>
						<xsl:value-of select ="''"/>
					</TerminationDateofRepo>

					<RepoEndMoney>
						<xsl:value-of select ="''"/>
					</RepoEndMoney>

					<RepoAccruedInterest>
						<xsl:value-of select ="''"/>
					</RepoAccruedInterest>

					<RepoHaircut>
						<xsl:value-of select ="''"/>
					</RepoHaircut>

					<SettlementLoc>
						<xsl:value-of select ="''"/>
					</SettlementLoc>

					<AccountNo>
						<xsl:value-of select ="''"/>
					</AccountNo>

					<AgentBankName>
						<xsl:value-of select ="''"/>
					</AgentBankName>

					<AgentBankLoc>
						<xsl:value-of select ="''"/>
					</AgentBankLoc>

					<InstructionsAtAgentBank>
						<xsl:value-of select ="''"/>
					</InstructionsAtAgentBank>

					<FeeType1>
						<xsl:value-of select ="'SEC'"/>
					</FeeType1>

					<FeeValue1>
						<xsl:value-of select ="MiscFees"/>
					</FeeValue1>

					<FeeType2>
						<xsl:value-of select ="''"/>
					</FeeType2>

					<FeeValue2>
						<xsl:value-of select ="''"/>
					</FeeValue2>

					<FeeType3>
						<xsl:value-of select ="''"/>
					</FeeType3>

					<FeeValue3>
						<xsl:value-of select ="''"/>
					</FeeValue3>

					<Strategy>
						<xsl:value-of select ="''"/>
					</Strategy>

					<TaxlotId>
						<xsl:value-of select ="''"/>
					</TaxlotId>

					<PreFiguredInd>
						<xsl:value-of select ="''"/>
					</PreFiguredInd>

					<BondInterest>
						<xsl:value-of select ="''"/>
					</BondInterest>

					<BondPrincipal>
						<xsl:value-of select ="''"/>
					</BondPrincipal>

					<ProcessingType>
						<xsl:value-of select ="''"/>
					</ProcessingType>

					<BlockId>
						<xsl:value-of select ="''"/>
					</BlockId>

					<ReservedFld1>
						<xsl:value-of select ="''"/>
					</ReservedFld1>

					<ReservedFld2>
						<xsl:value-of select ="''"/>
					</ReservedFld2>

					<ReservedFld3>
						<xsl:value-of select ="''"/>
					</ReservedFld3>

					<ReservedFld4>
						<xsl:value-of select ="''"/>
					</ReservedFld4>

		        	<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

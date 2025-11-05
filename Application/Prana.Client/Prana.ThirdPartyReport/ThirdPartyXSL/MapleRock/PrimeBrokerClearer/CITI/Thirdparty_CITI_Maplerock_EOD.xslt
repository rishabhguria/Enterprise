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
						<xsl:value-of select ="'SWAP'"/>
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
						<xsl:value-of select ="AccountNo"/>
					</ClientAccount>

					<!--<xsl:choose>
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
					</xsl:choose>-->

					<AccountType>
						<xsl:value-of select="''"/>
					</AccountType>


					<ClientReference>
						<!--<xsl:value-of select="PBUniqueID"/>-->
						<xsl:value-of select="PBUniqueID"/>
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
						<xsl:when test ="TaxLotState='Amended'">
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

					<!--<BrokerCode>

						<xsl:choose>
							<xsl:when test="contains(CounterParty,'GSEC')">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="contains(CounterParty,'WFB')">
								<xsl:value-of select="'0141'"/>
							</xsl:when>
							<xsl:when test="contains(CounterParty,'CUGX')">
								<xsl:value-of select="'824'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>

					</BrokerCode>-->
					
					<xsl:variable name="PB_NAME" select="''"/>
					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
					<xsl:variable name="THIRDPARTY_BROKER">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
					</xsl:variable>
					
					<BrokerCode>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_BROKER!=''">
								<xsl:value-of select="$THIRDPARTY_BROKER"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</BrokerCode>


					<SecurityID>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select ="OSIOptionSymbol"/>
							</xsl:when>
							<xsl:when test ="SEDOL!=''">
								<xsl:value-of select ="SEDOL"/>
							</xsl:when>
							<xsl:when test ="CUSIP!=''">
								<xsl:value-of select ="CUSIP"/>
							</xsl:when>
							<xsl:when test ="ISIN!=''">
								<xsl:value-of select ="ISIN"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
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
						<xsl:value-of select ="format-number(AveragePrice,'##.########')"/>
					</Price>

					<TradeCurrency>
						<xsl:value-of select ="CurrencySymbol"/>
					</TradeCurrency>

					<CommissionCode>
						<xsl:value-of select ="'G'"/>
					</CommissionCode>

					<Commission>
						<xsl:value-of select ="0"/>
					</Commission>


					<NetAmount>
						<xsl:value-of select ="NetAmount"/>
					</NetAmount>

					
					<AccruedInterest>
						<xsl:value-of select ="AccruedInterest"/>
					</AccruedInterest>

					<Strategy>
						<xsl:value-of select ="''"/>
					</Strategy>

					<ReservedField1>
						<xsl:value-of select ="''"/>
					</ReservedField1>

					<ReservedField2>
						<xsl:value-of select ="''"/>
					</ReservedField2>

					<ReservedField3>
						<xsl:value-of select ="''"/>
					</ReservedField3>

					<ReservedField4>
						<xsl:value-of select ="''"/>
					</ReservedField4>
					
					<ReservedField5>
						<xsl:value-of select ="''"/>
					</ReservedField5>
					
					<ReservedField6>
						<xsl:value-of select ="''"/>
					</ReservedField6>
					
					<ReservedField7>
						<xsl:value-of select ="''"/>
					</ReservedField7>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

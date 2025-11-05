<?xml version="1.0" encoding="UTF-8"?>

								<!--Description - GlobeOpEOD_Lyford file 
								 Date Created - 02-09-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<DealType>
					<xsl:value-of select="'DealType'"/>
				</DealType>

				<DealId>
					<xsl:value-of select="'DealId'"/>
				</DealId>

				<Action>
					<xsl:value-of select="'Action'"/>
				</Action>

				<Client>
					<xsl:value-of select="'Client'"/>
				</Client>

				<Reserved>
					<xsl:value-of select="'Reserved'"/>
				</Reserved>

				<Reserved1>
					<xsl:value-of select="'Reserved1'"/>
				</Reserved1>

				<Folder>
					<xsl:value-of select="'Folder'"/>
				</Folder>

				<Custodian>
					<xsl:value-of select="'Custodian'"/>
				</Custodian>

				<CashAccount>
					<xsl:value-of select="'CashAccount'"/>
				</CashAccount>

				<Counterparty>
					<xsl:value-of select="'Counterparty'"/>
				</Counterparty>

				<Comments>
					<xsl:value-of select="'Comments'"/>
				</Comments>

				<State>
					<xsl:value-of select="'State'"/>
				</State>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select="'SettlementDate'"/>
				</SettlementDate>

				<Reserved2>
					<xsl:value-of select="'Reserved2'"/>
				</Reserved2>

				<GlobeOpSecurityIdentifier>
					<xsl:value-of select="'GlobeOp Security Identifier'"/>
				</GlobeOpSecurityIdentifier>
				
				<Reserved3>
					<xsl:value-of select="'Reserved3'"/>
				</Reserved3>

				<Reserved4>
					<xsl:value-of select="'Reserved4'"/>
				</Reserved4>

				<Reserved5>
					<xsl:value-of select="'Reserved5'"/>
				</Reserved5>
				
				<BloombergTicker>
					<xsl:value-of select="'Bloomberg Ticker'"/>
				</BloombergTicker>

				<RIC>
					<xsl:value-of select="'RIC'"/>
				</RIC>

				<SecurityDescription>
					<xsl:value-of select="'SecurityDescription'"/>
				</SecurityDescription>

				<TransactionIndicator>
					<xsl:value-of select="'TransactionIndicator'"/>
				</TransactionIndicator>

				<Reserved6>
					<xsl:value-of select="'Reserved6'"/>
				</Reserved6>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>
				
				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>
				
				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<Tax>
					<xsl:value-of select="'Tax'"/>
				</Tax>

				<VAT>
					<xsl:value-of select="'VAT'"/>
				</VAT>

				<TradeCurrency>
					<xsl:value-of select="'TradeCurrency'"/>
				</TradeCurrency>

				<Reserved7>
					<xsl:value-of select="'Reserved7'"/>
				</Reserved7>

				<Reserved8>
					<xsl:value-of select="'Reserved8'"/>
				</Reserved8>

				<BrokerShortName>
					<xsl:value-of select="'BrokerShortName'"/>
				</BrokerShortName>

				<MaturityDate>
					<xsl:value-of select="'MaturityDate'"/>
				</MaturityDate>

				<Exchange>
					<xsl:value-of select="'Exchange'"/>
				</Exchange>

				<ClientReference>
					<xsl:value-of select="'ClientReference'"/>
				</ClientReference>

				<SwapType >
					<xsl:value-of select="'SwapType '"/>
				</SwapType>

				<InitialMargin>
					<xsl:value-of select="'Initial Margin'"/>
				</InitialMargin>

				<InitialMarginCurrency>
					<xsl:value-of select="'InitialMarginCurrency'"/>
				</InitialMarginCurrency>
								
				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>
					
					<!--for system use only-->					
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>
					
					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>
					
					<DealType>
						<xsl:value-of select="Asset"/>
					</DealType>

					<DealId>
						<xsl:value-of select="TradeRefID"/>
					</DealId>

					<xsl:variable name="varAction">
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'New'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select="'Amended'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="TaxLotState"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<Action>
						<xsl:value-of select="$varAction"/>
					</Action>

					<Client>
						<xsl:value-of select="'NEWFINANCE'"/>
					</Client>

					<Reserved>
						<xsl:value-of select="''"/>
					</Reserved>

					<Reserved1>
						<xsl:value-of select="''"/>
					</Reserved1>

					<Folder>
						<xsl:value-of select="'LYFORD'"/>
					</Folder>

					<Custodian>
						<xsl:value-of select="'FM'"/>
					</Custodian>

					<CashAccount>
						<xsl:value-of select="AccountNo"/>
					</CashAccount>

					<Counterparty>
						<xsl:value-of select="CounterParty"/>
					</Counterparty>

					<Comments>
						<xsl:value-of select="''"/>
					</Comments>
				
					<State>
						<xsl:value-of select="'Valid'"/>
					</State>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>
					</SettlementDate>

					<Reserved2>
						<xsl:value-of select="''"/>
					</Reserved2>

					<GlobeOpSecurityIdentifier>
						<xsl:value-of select="Symbol"/>
					</GlobeOpSecurityIdentifier>
					
					<Reserved3>
						<xsl:value-of select="''"/>
					</Reserved3>

					<Reserved4>
						<xsl:value-of select="''"/>
					</Reserved4>

					<Reserved5>
						<xsl:value-of select="''"/>
					</Reserved5>
					
					<BloombergTicker>
						<xsl:value-of select="BBCode"/>
					</BloombergTicker>

					<RIC>
						<xsl:value-of select="''"/>
					</RIC>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<TransactionIndicator>
						<xsl:value-of select="Side"/>
					</TransactionIndicator>

					<Reserved6>
						<xsl:value-of select="''"/>
					</Reserved6>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>
					
					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<xsl:variable name ="varUnderlying">
						<xsl:value-of select="substring-before(Symbol,' ')"/>
					</xsl:variable>

					<xsl:variable name ="varAssetCategory">
						<xsl:value-of select="Asset"/>
					</xsl:variable>

					<xsl:variable name ="varExchange">
						<xsl:value-of select="Exchange"/>
					</xsl:variable>

					<xsl:variable name="varcommissionRate">
						<xsl:value-of select="document('../ReconMappingXml/CommissionRate.xml')/CommissionRateMapping/PB[@Name='GlobeOp']/SymbolData [@Asset = $varAssetCategory and @Underlying = $varUnderlying and @Exchange = $varExchange]/@CommRate"/>
					</xsl:variable>

					<Commission>
						<xsl:choose>
							<xsl:when test ="$varcommissionRate != '' and number($varcommissionRate)">
								<xsl:value-of select="AllocatedQty * $varcommissionRate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CommissionCharged"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>


					<Tax>
						<xsl:value-of select="''"/>
					</Tax>

					<VAT>
						<xsl:value-of select="''"/>
					</VAT>

					<TradeCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</TradeCurrency>

					<Reserved7>
						<xsl:value-of select="''"/>
					</Reserved7>

					<Reserved8>
						<xsl:value-of select="''"/>
					</Reserved8>

					<BrokerShortName>
						<xsl:value-of select="CounterParty"/>
					</BrokerShortName>

					<MaturityDate>
						<xsl:value-of select="ExpirationDate"/>
					</MaturityDate>

					<Exchange>
						<xsl:value-of select="Exchange"/>
					</Exchange>

					<ClientReference>
						<xsl:value-of select="PutOrCall"/>
					</ClientReference>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>
										
					<xsl:variable name="varSwapType">
						<xsl:choose>
							<xsl:when test="StrikePrice =0">
								<xsl:value-of select="translate(Asset,$varLowerCase,$varUpperCase)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="StrikePrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					

					<SwapType >
						<xsl:value-of select="$varSwapType"/>
					</SwapType>

					<InitialMargin>
						<xsl:value-of select="0"/>
					</InitialMargin>

					<InitialMarginCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</InitialMarginCurrency>-->

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

					
				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
		
		
	</xsl:template>
</xsl:stylesheet>
